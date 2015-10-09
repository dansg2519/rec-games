using Newtonsoft.Json.Linq;
using RecGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Resources;

namespace RecGames.Helpers
{
    public class GameHelpers
    {
        private const int TopGamesToRecommend = 5;
        public static List<int> CalculateRecommendationScore(List<string> playerPortrait, List<Game> playerNotOwnedGames)
        {
            var gamesRecommendationScores = new Dictionary<int, float>();
            foreach (var game in playerNotOwnedGames)
            {
                float recommendationScore = 0.0f;
                var tagsMatch = game.Tags.Count(t => playerPortrait.Contains(t.TagName));

                recommendationScore += tagsMatch * 10.0f;
                recommendationScore += game.MetacriticScore * 0.25f;
                recommendationScore += game.Recommendations * 0.000025f;

                gamesRecommendationScores.Add(game.GameID, recommendationScore);
            }
            
            var gamesIdsToRecommend = gamesRecommendationScores.OrderByDescending(p => p.Value).Take(TopGamesToRecommend).Select(p => p.Key).ToList();
            return gamesIdsToRecommend;
        }

        public static List<int> CalculateRecommendationScore2(List<string> playerPortrait, List<Game> playerNotOwnedGames)
        {
            var gamesRecommendationScores = new Dictionary<int, float>();
            foreach (var game in playerNotOwnedGames)
            {
                float recommendationScore = 0.0f;
                var tagsMatch = game.Tags.Count(t => playerPortrait.Contains(t.TagName));

                recommendationScore += tagsMatch * 3.2f;
                recommendationScore += game.Recommendations * 0.000015f;
                recommendationScore += game.MetacriticScore * 0.22f;
                recommendationScore += (game.PriceValue == 0) ? (float)(300) : (float)(300 / (game.PriceValue / 100));

                gamesRecommendationScores.Add(game.GameID, recommendationScore);
            }

            var gamesIdsToRecommend = gamesRecommendationScores.OrderByDescending(p => p.Value).Take(TopGamesToRecommend).Select(p => p.Key).ToList();
            return gamesIdsToRecommend;
        }

        public static JArray SetUpGamesToRecommendJson(List<Game> gamesToRecommend, List<string> playerPortrait)
        {
            JArray gamesToRecommendJson = new JArray(gamesToRecommend.Select(g => (
                new JObject(
                    new JProperty("game_name", g.Name),
                    new JProperty("developers", g.Developers),
                    new JProperty("genre", g.Genre),
                    new JProperty("publishers", g.Publishers),
                    new JProperty("launch_date", g.LaunchDate),
                    new JProperty("metacritic_score", g.MetacriticScore),
                    new JProperty("recommendations", g.Recommendations),
                    new JProperty("total_achievements", g.TotalAchievements),
                    new JProperty("header_image", g.HeaderImage),
                    new JProperty("price_value", (g.PriceValue/100)),
                    new JProperty("price_currency", g.PriceCurrency),
                    new JProperty("platforms", g.Platforms),
                    new JProperty("game_steam_url", @"http://store.steampowered.com/app/" + g.GameID.ToString()),
                    new JProperty("justification", string.Format(Strings.Justification, g.Name, string.Join(",", g.Tags.Select(t => t.TagName).ToArray()), g.Recommendations, g.MetacriticScore)),
                    new JProperty("tags", g.Tags.Select(t => t.TagName)),
                    new JProperty("common_tags", string.Join(",", g.Tags.Select(t => t.TagName).ToArray().Intersect(playerPortrait))),
                    new JProperty("tags_string", string.Join(",", g.Tags.Select(t => t.TagName).ToArray())),
                    new JProperty("uncommon_tags", string.Join(",", g.Tags.Select(t => t.TagName).ToArray().Except(playerPortrait)))))));

            return gamesToRecommendJson;
        }

        public static string SupportedPlatforms(JObject platformObject)
        {
            string platforms;
            bool windows = (bool)platformObject["windows"];
            bool linux = (bool)platformObject["linux"];
            bool mac = (bool)platformObject["mac"];

            platforms = String.Empty;

            if (windows)
            {
                platforms += "Windows ";
            }
            if (mac)
            {
                platforms += "Mac ";
            }
            if (linux)
            {
                platforms += "Linux ";
            }

            return platforms;
        }
        
        public static string GameDevelopers(JArray developersArray)
        {
            string developers = String.Empty;

            List<string> developersList = developersArray.ToObject<List<string>>();

            try
            {
                developers = developersList[0];
                if (developersList.Count > 1)
                {
                    for (int index = 1; index < developersList.Count; index++)
                    {
                        developers += "," + developersList[index];
                    }
                }
                return developers;
            }
            catch (Exception) {
                return String.Empty;
            }
        }

        public static string GamePublishers(JArray publishersArray)
        {
            List<string> publishersList = publishersArray.ToObject<List<string>>();

            string publishers;

            try
            {
                publishers = publishersList[0];
                if (publishersList.Count > 1)
                {
                    for (int index = 1; index < publishersList.Count; index++)
                    {
                        publishers += "," + publishersList[index];
                    }
                }

                return publishers;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
    
}