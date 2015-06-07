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
        
    }
    
}