﻿using System;
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
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerOwnedGames = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + Strings.SteamKey + "&steamid=" + SteamId + "&include_appinfo=1&include_played_free_games=1&format=json");
            }

            JObject playerOwnedGamesJson = JObject.Parse(playerOwnedGames);
            return Ok(playerOwnedGamesJson);
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
                    Debug.WriteLine(">>>>>>>>>>>>>>>>>> steam id = " + steamId);
                    if(isNumeric)
                    {
                        string html = client.DownloadString(@"http://steamcommunity.com/profiles/" + steamId);
                        validSteamId = !html.Contains(@"The specified profile could not be found.");
                        SteamId = steamId;
                        Debug.Write(">>>>>>>>>>>>>>>>>>>>>>>Profiles Id type");
                    }
                    else
                    {
                        Debug.Write(">>>>>>>>>>> "+steamId);
                        string html = client.DownloadString(@"http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key=" + Strings.SteamKey + "&vanityurl=" + steamId);
                        Debug.WriteLine(">>>>>>>>>>>>>>>>>>> parsing...");
                        JObject response = JObject.Parse(html);
                        if((int) response["success"] == 1)
                        {
                            SteamId = (string) response["steamid"];
                            validSteamId = true;
                        }
                        Debug.Write(">>>>>>>>>>>>>>>>>>>>>>>>Custom Id type");
                    }
                    
                }
                catch(System.Net.WebException)
                {

                }
                
            }
                
            return Ok(validSteamId);
        }

        [HttpPost]
        [ActionName("PlayerPortrait")]
        public IHttpActionResult PostPlayerPortrait(List<Game> ownedGames)
        {
            var playerTags = new List<string>();
            foreach (var game in ownedGames)
            {
                var tags = db.Games.Where(g => g.GameID == game.GameID).SelectMany(g => g.Tags).ToList();
                foreach (var tag in tags)
                {
                    playerTags.Add(tag.TagName);                    
                }
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