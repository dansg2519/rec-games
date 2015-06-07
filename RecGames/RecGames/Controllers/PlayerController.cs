using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RecGames.Models;

namespace RecGames.Controllers
{
    public class PlayerController : ApiController
    {
        private const string SteamKey = "3E2BA9478DC190757ABE4D1DABEA9802";
        private const string SteamId = "76561197960435530";

        [HttpGet]
        [ActionName("Info")]
        public IHttpActionResult GetInfo()
        {
            string playerInfoJson;
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerInfoJson = client.DownloadString(@"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + SteamKey + "&steamids=" + SteamId);
            }

            return this.Ok(playerInfoJson);
        }

        [HttpGet]
        [ActionName("OwnedGames")]
        public IHttpActionResult GetOwnedGames()
        {
            string playerOwnedGamesJson;
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerOwnedGamesJson = client.DownloadString(@"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + SteamKey + "&steamid=" + SteamId + "&include_appinfo=1&include_played_free_games=1&format=json");
            }

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

            return this.Ok(recentlyPlayedGames);
        }
    }
}
