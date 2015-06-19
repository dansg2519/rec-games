namespace RecGames.Migrations
{
    using Newtonsoft.Json.Linq;
    using RecGames.Helpers;
    using RecGames.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Net;

    internal sealed class Configuration : DbMigrationsConfiguration<RecGames.DAL.RecGameContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
        }

        protected override void Seed(RecGames.DAL.RecGameContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            using (WebClient client = new WebClient())
            {
                string appsJson = client.DownloadString(@"http://api.steampowered.com/ISteamApps/GetAppList/v0001/");
                JObject jObjectApps = JObject.Parse(appsJson);
                JArray jArrayApps = (JArray)jObjectApps["applist"]["apps"]["app"];
                List<string> fullList = new List<string>();
                foreach (var item in jArrayApps)
                {
                    fullList.Add((string)item["appid"]);
                }
                var appsId = fullList.Distinct().ToList();

                using (var writer = new StreamWriter("appsId.txt"))
                {
                    // Loop through ten numbers.
                    for (int i = 0; i < appsId.Count; i++)
                    {
                        // Write format string to file.
                        writer.Write("{0\n} ", appsId[i]);
                    }
                }

                //var games = new List<Game>();
                //for (int i = 0; i < appsId.Count; i++)
                //{
                //    var games = new List<Game>();
                //    getGames(client, games, appsId[i]);
                //    games.ForEach(g => context.Games.AddOrUpdate(d => d.GameID, g));
                //    context.SaveChanges();
                //}                

                //games.Add(
                //    new Game
                //    {
                //        GameID = 202930,
                //        Name = "Witcher3"
                //    }
                //);
                //games.ForEach(g => context.Games.AddOrUpdate(i => i.GameID, g));
                //context.SaveChanges();
            }
        }

        private static void getGames(WebClient client, List<Game> games, string appsId)
        {
            try
            {
                string appDetailsJson = client.DownloadString(@"http://store.steampowered.com/api/appdetails/?appids=" + appsId);
                JObject jObjectAppDetails = JObject.Parse(appDetailsJson);
                JObject jObjectApp = (JObject)jObjectAppDetails[appsId];
                JObject jObjectAppData = (JObject)jObjectApp["data"];
                bool success = (bool)jObjectApp["success"];
                if (success)
                {
                    string appType = (string)jObjectAppData["type"];
                    if (appType == "game")
                    {
                        var game = new Game();
                        game.GameID = (int)jObjectAppData["steam_appid"];
                        System.Console.WriteLine(game.GameID);
                        game.Name = (string)jObjectAppData["name"];
                        //Tem jsons sem controller_support
                        game.ControllersSupported = (string)jObjectAppData["controller_support"];
                        game.Platforms = GameHelpers.SupportedPlatforms((JObject)jObjectAppData["platforms"]);
                        //game.Developers = GameHelpers.GameDevelopers((JArray)jObjectAppData["developers"]);
                        game.Publishers = GameHelpers.GamePublishers((JArray)jObjectAppData["publishers"]);
                        //game.MetacriticScore = (int)jObjectAppData["metacritic"]["score"];
                        //game.Recommendations = (int)jObjectAppData["recommendations"]["total"];
                        //game.TotalAchievements = (int)jObjectAppData["achievements"]["total"];
                        game.LaunchDate = (string)jObjectAppData["release_date"]["date"];

                        //Não sei como price vai funcionar
                        //var price = new Price();
                        //price.Currency = (string)jObjectAppData["price_overview"]["currency"];
                        //price.Value = (int)jObjectAppData["price_overview"]["initial"];

                        games.Add(game);
                    }
                }
            }
            catch (WebException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
