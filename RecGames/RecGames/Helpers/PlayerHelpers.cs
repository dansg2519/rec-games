using Newtonsoft.Json.Linq;
using RecGames.DAL;
using RecGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecGames.Helpers
{
    public class PlayerHelpers
    {
        private const int RecentGamesBonus = 10;
        private const int WishlistGamesBonus = 5;
        private const int MinutesToHour = 60;

        public static void DefinePortraitByOwnedGames(List<string> playerTags, JObject myGames, RecGameContext db)
        {
            if ((int)myGames["owned_games"]["response"]["game_count"] > 0)
            {
                var ownedGames = myGames["owned_games"]["response"]["games"].ToObject<List<Game>>();

                foreach (var game in ownedGames)
                {
                    var playTimeForever = (double)myGames["owned_games"]["response"]["games"][ownedGames.FindIndex(g => g == game)]["playtime_forever"];

                    var tags = db.Games.Where(g => g.GameID == game.GameID).SelectMany(g => g.Tags).ToList();
                    foreach (var tag in tags)
                    {
                        if (tag.TagName != "Singleplayer" && tag.TagName != "Multiplayer")
                        {
                            playTimeForever = Math.Sqrt(playTimeForever / MinutesToHour)/4;
                            for (int i = 0; i < (1 + playTimeForever); i++)
                            {
                                playerTags.Add(tag.TagName);

                            }
                        }
                    }
                }
            }

        }

        public static void DefinePortraitByRecentlyPlayedGames(List<string> playerTags, JObject myGames, RecGameContext db)
        {
            if ((int)myGames["recently_played_games"]["response"]["total_count"] > 0)
            {
                var recentlyPlayedGames = myGames["recently_played_games"]["response"]["games"].ToObject<List<Game>>();

                foreach (var game in recentlyPlayedGames)
                {
                    var playTimeTwoWeeks = (double)myGames["recently_played_games"]["response"]["games"][recentlyPlayedGames.FindIndex(g => g == game)]["playtime_2weeks"];
                    var tags = db.Games.Where(g => g.GameID == game.GameID).SelectMany(g => g.Tags).ToList();
                    foreach (var tag in tags)
                    {
                        if (tag.TagName != "Singleplayer" && tag.TagName != "Multiplayer")
                        {
                            playTimeTwoWeeks = 2 * Math.Sqrt(playTimeTwoWeeks / MinutesToHour);
                            for (int i = 0; i < (RecentGamesBonus + playTimeTwoWeeks); i++)
                            {
                                playerTags.Add(tag.TagName);

                            }
                        }
                    }
                }
            }

        }

        public static void DefinePortraitByWishListGames(List<string> playerTags, JObject myGames, RecGameContext db)
        {
            var wishlistGames = myGames["wishlist_games"].ToList();
            if (wishlistGames != null)
            {
                foreach (var game in wishlistGames)
                {
                    var wishlistGame = game.ToString();
                    var tags = db.Games.Where(g => g.Name == wishlistGame).SelectMany(g => g.Tags).ToList();

                    foreach (var tag in tags)
                    {
                        if (tag.TagName != "Singleplayer" && tag.TagName != "Multiplayer")
                        {
                            for (int i = 0; i < WishlistGamesBonus; i++)
                            {
                                playerTags.Add(tag.TagName);
                            }
                        }
                    }
                }
            }
        }
    }
}
