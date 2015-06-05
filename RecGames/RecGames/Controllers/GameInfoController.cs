using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RecGames.Controllers
{
    public class GameInfoController : ApiController
    {

        private const string SteamKey = "3E2BA9478DC190757ABE4D1DABEA9802";


        // GET api/GameInfo
        public IHttpActionResult Get(int? id)
        {
            string gameInfoJson;
            using (WebClient client = new WebClient())
            {

                gameInfoJson = client.DownloadString(@"http://store.steampowered.com/api/appdetails/?appids="+ id.ToString());
            }

            return this.Ok(gameInfoJson);
        }
    }
}
