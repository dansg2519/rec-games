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
using System.Web.Script.Serialization;
using Resources;
using HtmlAgilityPack;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using RecGames.Helpers;

namespace RecGames.Controllers
{
    public class PlayerController : ApiController
    {
        private const int TopTags = 5;
        private static string SteamId;
        private RecGameContext db = new RecGameContext();

        [HttpGet]
        [ActionName("Info")]
        public IHttpActionResult GetInfo()
        {
            string playerInfo;
            using (WebClient client = new WebClient())
            {
                playerInfo = client.DownloadString(@"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + Strings.SteamKey + "&steamids=" + SteamId);
            }

            JObject playerInfoJson = JObject.Parse(playerInfo);
            return Ok(playerInfoJson);
        }

        [HttpGet]
        [ActionName("OwnedGames")]
        public IHttpActionResult GetOwnedGames()
        {
            string playerOwnedGames;
            string recentlyPlayedGames;
            var wishlistGames = new List<string>();
            JObject playerOwnedGamesPack = new JObject();
            // System.Net.WebException
            try
            {
                using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    playerOwnedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + Strings.SteamKey + "&steamid=" + SteamId + "&include_appinfo=1&include_played_free_games=1&format=json");

                    


                    JObject playerOwnedGamesJson = JObject.Parse(playerOwnedGames);
                    if (playerOwnedGamesJson["response"].Count() == 0)
                    {
                        Debug.WriteLine("PRIVATE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        return Ok();
                    }

                    playerOwnedGamesPack.Add("owned_games", playerOwnedGamesJson);

                    recentlyPlayedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key=" + Strings.SteamKey + "&steamid=" + SteamId);

                    string html = client.DownloadString(@"http://steamcommunity.com/profiles/" + SteamId + @"/wishlist/");
                    if (html != null)
                    {
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(html);

                        wishlistGames = htmlDocument.DocumentNode.SelectNodes("//h4[@class='ellipsis']")
                                                    .Select(h => h.InnerText).ToList();
                    }

                }

                

                JObject recentlyPlayedGamesJson = JObject.Parse(recentlyPlayedGames);

                var wishlistGamesJson = JsonConvert.SerializeObject(wishlistGames);
                JArray wishlistGamesJArray = JArray.Parse(wishlistGamesJson);
            
                
                playerOwnedGamesPack.Add("recently_played_games", recentlyPlayedGamesJson);
                playerOwnedGamesPack.Add("wishlist_games", wishlistGamesJson);

                return Ok(playerOwnedGamesPack);
            }
            catch(System.Net.WebException e)
            {
                return BadRequest();
            }
        }
        
        [HttpPost]
        [ActionName("SteamId")]
        public IHttpActionResult PostSteamId([FromBody]string steamId)
        {
            bool validSteamId = false;
            using (WebClient client = new WebClient())
            {
                try
                {
                    long n;
                    bool isNumeric = Int64.TryParse(steamId, out n);
                    if(isNumeric)
                    {
                        string html = client.DownloadString(@"http://steamcommunity.com/profiles/" + steamId);
                        validSteamId = !html.Contains(@"The specified profile could not be found.");
                        SteamId = steamId;
                    }
                    else
                    {
                        string html = client.DownloadString(@"http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=" + Strings.SteamKey + "&vanityurl=" + steamId);
                        JObject resolve = JObject.Parse(html);
                        JObject response = (JObject) resolve["response"];
                        if((int) response["success"] == 1)
                        {
                            SteamId = (string) response["steamid"];
                            validSteamId = true;
                        }
                    }
                    
                }
                catch(System.Net.WebException)
                {
                    return BadRequest();
                }                
            }
                
            return Ok(validSteamId);
        }

        [HttpPost]
        [ActionName("PlayerPortrait")]
        public IHttpActionResult PostPlayerPortrait(JObject myGames)
        {
            var playerTags = new List<string>();
            try
            {
                PlayerHelpers.DefinePortraitByOwnedGames(playerTags, myGames, db);
                PlayerHelpers.DefinePortraitByRecentlyPlayedGames(playerTags, myGames, db);
                PlayerHelpers.DefinePortraitByWishListGames(playerTags, myGames, db);
            }
            catch (NullReferenceException e)
            {
                return Ok();
            }
            
            var topTags = playerTags.GroupBy(x => x)
                                    .ToDictionary(x => x.Key, x => x.Count())
                                    .OrderByDescending(x => x.Value)
                                    .Take(TopTags)
                                    .ToDictionary(x => x.Key, x => x.Value).Keys;

            var playerPortrait = new JavaScriptSerializer().Serialize(topTags);
            return Ok(playerPortrait);
        }
    }
}