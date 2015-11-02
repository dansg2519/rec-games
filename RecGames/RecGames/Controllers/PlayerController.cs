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

namespace RecGames.Controllers
{
    public class PlayerController : ApiController
    {
        private static string SteamId = "76561197960435530";
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
            // System.Net.WebException
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerOwnedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + Strings.SteamKey + "&steamid=" + SteamId + "&include_appinfo=1&include_played_free_games=1&format=json");
            }

            using (WebClient client = new WebClient())
            {
                recentlyPlayedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key=" + Strings.SteamKey + "&steamid=" + SteamId);
            }

            JObject playerOwnedGamesPack = new JObject();

            JObject recentlyPlayedGamesJson = JObject.Parse(recentlyPlayedGames);
            JObject playerOwnedGamesJson = JObject.Parse(playerOwnedGames);

            if (playerOwnedGamesJson["response"] == null)
            {
                return BadRequest();
            }

            playerOwnedGamesPack.Add("owned_games", playerOwnedGamesJson);
            playerOwnedGamesPack.Add("recently_played_games", recentlyPlayedGamesJson);
            return Ok(playerOwnedGamesPack);
        }

        [HttpGet]
        [ActionName("RecentlyPlayedGames")]
        public IHttpActionResult GetRecentlyPlayedGames()
        {
            string recentlyPlayedGames;
            using (WebClient client = new WebClient())
            {
                recentlyPlayedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key=" + Strings.SteamKey + "&steamid=" + SteamId);
            }

            JObject recentlyPlayedGamesJson = JObject.Parse(recentlyPlayedGames);
            return Ok(recentlyPlayedGamesJson);
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
                    Debug.WriteLine("Exception Thrown");
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
                var ownedGames = myGames["owned_games"]["response"]["games"].ToObject<List<Game>>();
                // System.DataException ou algo do tipo
                foreach (var game in ownedGames)
                {
                    var tags = db.Games.Where(g => g.GameID == game.GameID).SelectMany(g => g.Tags).ToList();
                    foreach (var tag in tags)
                    {
                        if (tag.TagName != "Singleplayer" && tag.TagName != "Multiplayer")
                        {
                            playerTags.Add(tag.TagName);
                        }
                    }
                }

            }
            catch (NullReferenceException e)
            {
                return BadRequest();
            }

            try {
                var recentlyPlayedGames = myGames["recently_played_games"]["response"]["games"].ToObject<List<Game>>();

                foreach (var game in recentlyPlayedGames)
                {
                    var playTimeTwoWeeks = (double)myGames["recently_played_games"]["response"]["games"][recentlyPlayedGames.FindIndex(g => g == game)]["playtime_2weeks"];
                    var tags = db.Games.Where(g => g.GameID == game.GameID).SelectMany(g => g.Tags).ToList();
                    foreach (var tag in tags)
                    {
                        if (tag.TagName != "Singleplayer" && tag.TagName != "Multiplayer")
                        {
                            playTimeTwoWeeks = 2 * Math.Sqrt(playTimeTwoWeeks / 60);
                            for (int i = 0; i < (50 + playTimeTwoWeeks); i++)
                            {
                                playerTags.Add(tag.TagName);

                            }
                        }
                    }
                }
            } catch (NullReferenceException e)
            {
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