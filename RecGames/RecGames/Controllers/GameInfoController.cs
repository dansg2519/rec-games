using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using RecGames.Models;
using RecGames.Helpers;

namespace RecGames.Controllers
{
    public class GameInfoController : ApiController
    {

        private const string SteamKey = "3E2BA9478DC190757ABE4D1DABEA9802";
        HtmlDocument htmlDocument = new HtmlDocument();

        [HttpGet]
        // GET api/GameInfo
        public IHttpActionResult Details(int? id)
        {
            string gameInfoJson;
            Game game = new Game();
            using (WebClient client = new WebClient())
            {

                gameInfoJson = client.DownloadString(@"http://store.steampowered.com/api/appdetails/?appids="+ id.ToString());

                JObject jObject = JObject.Parse(gameInfoJson);
                JObject idObject = (JObject)jObject[id.ToString()];
                JObject dataObject = (JObject)idObject["data"];


                game.GameID = (int)dataObject["steam_appid"];
                game.Name = (string)dataObject["name"];
                game.ControllersSupported = (string)dataObject["controller_support"];
                game.Platforms = GameHelpers.SupportedPlatforms((JObject) dataObject["platforms"]);
                
            }

            return this.Ok(gameInfoJson);
        }


        [HttpGet]
        public IHttpActionResult Tags(int? id)
        {
            List<string> tags = new List<string>();
            string title;

            using (WebClient client = new WebClient())
            {
                string html = client.DownloadString("http://store.steampowered.com/app/" + id);
                htmlDocument.LoadHtml(html);

            }

            HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='game_highlights']/div[2]/div/div[5]/div[2]");
            
            try
            {
                string temp = htmlNode.InnerText;

                title = Regex.Replace(temp, "\t", "");
                title = Regex.Replace(title, @"( |\r?\n)\1+", "$1");
                tags = title.Split('\n').ToList();
                return this.Ok(tags);
            }
            catch (System.NullReferenceException)
            {
                return this.Ok("No Tags");
            }
        }
    }
}
