using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecGames.Helpers
{
    public class GameHelpers
    {
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