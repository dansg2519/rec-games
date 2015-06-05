using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RecGames.Models;

namespace RecGames.Controllers
{
    public class PlayerInfoController : ApiController
    {
        private const string SteamKey = "3E2BA9478DC190757ABE4D1DABEA9802";
        private const string SteamId = "76561197960435530";

        // GET api/PlayerInfo
        public IHttpActionResult Get()
        {
            string playerInfoJson;
            using (WebClient client = new WebClient())
            {
                //steamId = "76561197960435530";
                playerInfoJson = client.DownloadString(@"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + SteamKey + "&steamids=" + SteamId);
            }

            return this.Ok(playerInfoJson);
        }
    }
}
