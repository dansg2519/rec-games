using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using RecGames.DAL;
using RecGames.Models;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using RecGames.Helpers;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace RecGames.Controllers
{
    public class GameController : ApiController
    {
        private RecGameContext db = new RecGameContext();

        [HttpPost]
        [ActionName("RecommendedGames")]
        public IHttpActionResult PostRecommendedGames(JObject playerData)
        {
            var ownedGamesIds = playerData.GetValue("owned_games").Select(o => o.SelectToken("appid").ToObject<int>()).ToList();
            var playerPortrait = playerData.GetValue("player_portrait").Select(p => p.ToString()).ToList();
            var wishlistGames = playerData.GetValue("wishlist_games").Select(p => p.ToString()).ToList();

            //ideia:pegar todos os jogos que o jogador não tem; pegar todos os jogos que não tem nenhuma tag que descrevem o jogador;
            //remover de todos os jogos que não tem, os que não tem nenhuma tag que descrevem o jogador, para assim obter 
            //jogos que o jogador não tem mas tem alguma das tags que o descrevem
            var playerNotOwnedGamesIds = db.Games.Where(g => !ownedGamesIds.Contains(g.GameID)).Select(g => g.GameID).ToList();
            var gamesIdsNotMatchingPortrait = db.Games.Where(g => g.Tags.All(t => !playerPortrait.Contains(t.TagName))).Select(g => g.GameID).ToList();

            //linha abaixo usada pra avaliar comportamento correto da query
            //var tags = db.Games.Where(g => gamesIdsNotMatchingPortrait.Contains(g.GameID)).SelectMany(g => g.Tags.Select(t => t.TagName)).ToList();

            playerNotOwnedGamesIds.RemoveAll(g => gamesIdsNotMatchingPortrait.Contains(g));

            var playerNotOwnedGames = db.Games.Where(g => playerNotOwnedGamesIds.Contains(g.GameID)).ToList();

            //remove jogos que não são free mas estão com preço 0
            var tagFreeToPlay = db.Tags.Single(t => t.TagName.Equals("Free to Play"));
            playerNotOwnedGames.RemoveAll(g => (g.PriceValue == 0) && !(g.Tags.Contains(tagFreeToPlay)));

            //remove jogos que estão na wishlist
            foreach (var game in wishlistGames)
            {
                playerNotOwnedGames.RemoveAll(g => g.Name == game);
            }

            var gamesIdsToRecommend = GameHelpers.CalculateRecommendationScore(playerPortrait, playerNotOwnedGames);
            var gamesToRecommend = db.Games.Where(g => gamesIdsToRecommend.Contains(g.GameID)).ToList();
            var gamesToRecommendJson = GameHelpers.SetUpGamesToRecommendJson(gamesToRecommend, playerPortrait);

            var gamesToRecommendPack = new JObject();
            gamesToRecommendPack.Add("recommendation", gamesToRecommendJson);

            return Ok(gamesToRecommendPack);
        }
    }
}