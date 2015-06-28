using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RecGames.Models;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using RecGames.DAL;

namespace RecGames.Controllers
{
    public class PlayerController : ApiController
    {
        private const string SteamKey = "3E2BA9478DC190757ABE4D1DABEA9802";
        private const string SteamId = "76561197960435530";
        private const int TopTags = 5;
        private RecGameContext db = new RecGameContext();

        [HttpGet]
        [ActionName("Info")]
        public IHttpActionResult GetInfo()
        {
            string playerInfo;
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerInfo = client.DownloadString(@"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + SteamKey + "&steamids=" + SteamId);
            }

            JObject playerInfoJson = JObject.Parse(playerInfo);
            return this.Ok(playerInfoJson);
        }

        [HttpGet]
        [ActionName("OwnedGames")]
        public IHttpActionResult GetOwnedGames()
        {
            string playerOwnedGames;
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerOwnedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + SteamKey + "&steamid=" + SteamId + "&include_appinfo=1&include_played_free_games=1&format=json");
            }

            JObject playerOwnedGamesJson = JObject.Parse(playerOwnedGames);
            return this.Ok(playerOwnedGamesJson);
        }

        [HttpGet]
        [ActionName("RecentlyPlayedGames")]
        public IHttpActionResult GetRecentlyPlayedGames()
        {
            string recentlyPlayedGames;
            using (WebClient client = new WebClient())
            {
                recentlyPlayedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key=" + SteamKey + "&steamid=" + SteamId);
            }


            JObject recentlyPlayedGamesJson = JObject.Parse(recentlyPlayedGames);
            return this.Ok(recentlyPlayedGamesJson);
        }

        [HttpPost]
        [ActionName("PlayerPortrait")]
        public IHttpActionResult PostPlayerPortrait(List<Game> gamesOwned)
        {
            var playerTags = new List<string>();
            foreach (var game in gamesOwned)
            {
                var groupTags = db.Games.Where(g => g.GameID == game.GameID).Select(g => g.Tags).ToList();
                foreach (var tags in groupTags)
                {
                    foreach (var tag in tags)
                    {
                        playerTags.Add(tag.TagName);
                    }                    
                }
            }

            var topTags = playerTags.GroupBy(x => x)
                                    .ToDictionary(x => x.Key, x => x.Count())
                                    .OrderByDescending(x => x.Value)
                                    .Take(TopTags)
                                    .ToDictionary(x => x.Key, x => x.Value).Keys;
            return this.Ok();
        }
    }
}