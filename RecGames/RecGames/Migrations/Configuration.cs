namespace RecGames.Migrations
{
    using HtmlAgilityPack;
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
    using System.Text.RegularExpressions;

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
                List<string> appsId = new List<string>();
                //saveAppsIds(client, appsId);

                appsId = getAppsId();

                for (int i = 0; i < appsId.Count; i++)
                {
                    var games = new List<Game>();
                    var tags = new List<Tag>();
                    getGames(context, client, games, tags, appsId[i]);
                    games.ForEach(g => context.Games.AddOrUpdate(d => d.GameID, g));
                    context.SaveChanges();
                    tags.ForEach(t => context.Tags.AddOrUpdate(a => a.TagID, t));
                    context.SaveChanges();
                }
                //linha 500 A 1407 nao adicionou nada, muitos arquivos tipo movie 
            }
        }

        private static List<string> getAppsId()
        {
            List<string> appsId;
            using (StreamReader reader = new StreamReader(@"C:\Users\Daniel\Coder\C#\rec-games\RecGames\ImportantFiles\gamesId.txt"))
            {
                string[] fileAppsId = reader.ReadToEnd().Split('\n');
                appsId = fileAppsId.ToList();
            }

            return appsId;
        }

        private static void saveAppsIds(WebClient client, List<string> appsId)
        {
            string appsJson = client.DownloadString(@"http://api.steampowered.com/ISteamApps/GetAppList/v0001/");
            JObject jObjectApps = JObject.Parse(appsJson);
            JArray jArrayApps = (JArray)jObjectApps["applist"]["apps"]["app"];
            List<string> fullList = new List<string>();
            foreach (var item in jArrayApps)
            {
                fullList.Add((string)item["appid"]);
            }
            appsId = fullList.Distinct().ToList();

            using (var writer = new StreamWriter(@"C:\Users\Daniel\Coder\C#\rec-games\RecGames\ImportantFiles\appsId.txt"))
            {
                for (int i = 0; i < appsId.Count; i++)
                {
                    writer.Write("{0}\n", appsId[i]);
                }
            }
        }

        private static void getGames(RecGames.DAL.RecGameContext context, WebClient client, List<Game> games, List<Tag> tags, string appsId)
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
                        game.Name = (string)jObjectAppData["name"];
                        //Tem jsons sem controller_support
                        game.ControllersSupported = (string)jObjectAppData["controller_support"];
                        game.Platforms = GameHelpers.SupportedPlatforms((JObject)jObjectAppData["platforms"]);
                        game.Publishers = GameHelpers.GamePublishers((JArray)jObjectAppData["publishers"]);
                        game.LaunchDate = (string)jObjectAppData["release_date"]["date"];

                        try
                        {
                            game.Developers = GameHelpers.GameDevelopers((JArray)jObjectAppData["developers"]);
                        }
                        catch (NullReferenceException)
                        {
                            game.Developers = string.Empty;
                        }

                        try
                        {
                            game.MetacriticScore = (int)jObjectAppData["metacritic"]["score"];
                        }
                        catch (NullReferenceException)
                        {
                            game.MetacriticScore = default(int);
                        }

                        try
                        {
                            game.Recommendations = (int)jObjectAppData["recommendations"]["total"];
                        }
                        catch (NullReferenceException)
                        {
                            game.Recommendations = default(int);
                        }

                        try
                        {
                            game.TotalAchievements = (int)jObjectAppData["achievements"]["total"];
                        }
                        catch (NullReferenceException)
                        {
                            game.TotalAchievements = default(int);
                        }

                        //game.PriceCurrency = (string)jObjectAppData["price_overview"]["currency"];
                        try
                        {
                            game.PriceValue = (int)jObjectAppData["price_overview"]["initial"];
                        }
                        catch (NullReferenceException)
                        {
                            game.PriceValue = default(int);
                        }

                        game.Tags = new List<Tag>();
                        getTags(context, client, tags, appsId, game);
                        games.Add(game);
                    }
                }
            }
            catch (WebException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        private static void getTags(RecGames.DAL.RecGameContext context, WebClient client, List<Tag> tags, string appsId, Game game)
        {
            string html = client.DownloadString(@"http://store.steampowered.com/app/" + appsId);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='game_highlights']/div[2]/div/div[5]/div[2]");

            List<string> tagsFromHtml = new List<string>();
            try
            {
                string tagsText = Regex.Replace(htmlNode.InnerText, "\t", "");
                tagsText = Regex.Replace(tagsText, @"^\r\n\r\n", "");
                tagsText = Regex.Replace(tagsText, @"\r\n?", "\n");
                tagsText = Regex.Replace(tagsText, @"\+\n", "");
                tagsFromHtml = tagsText.Split('\n').ToList();
            }
            catch (System.NullReferenceException)
            {
                //Console.WriteLine("Não possui tags");
            }

            foreach (var tagsItem in tagsFromHtml)
            {
                var tag = new Tag();
                int appsIdNumber = int.Parse(appsId);

                if (context.Tags.Any(t => t.TagName == tagsItem))
                {
                    tag = context.Tags.Single(t => t.TagName == tagsItem);
                    tag.Games.Add(context.Games.FirstOrDefault(g => g.GameID == appsIdNumber));
                } else {
                    tag.TagName = tagsItem;

                    tag.Games = new List<Game>();
                    tag.Games.Add(context.Games.FirstOrDefault(g => g.GameID == appsIdNumber));
                }

                game.Tags.Add(tag);
                tags.Add(tag);
            }
        }
    }
}
