using Newtonsoft.Json.Linq;
using RecGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            catch (System.NullReferenceException)
            {
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
            catch (System.NullReferenceException)
            {
                return String.Empty;
            }
        }
    }
    
}