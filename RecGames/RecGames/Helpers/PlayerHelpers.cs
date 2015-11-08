﻿using Newtonsoft.Json.Linq;
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
        public static void DefinePortraitByOwnedGames(List<string> playerTags, JObject myGames, RecGameContext db)
        {
            if ((int)myGames["owned_games"]["response"]["game_count"] > 0)
            {
                var ownedGames = myGames["owned_games"]["response"]["games"].ToObject<List<Game>>();

                foreach (var game in ownedGames)
                {
                    var tags = db.Games.Where(g => g.GameID == game.GameID).SelectMany(g => g.Tags).ToList();
                    foreach (var tag in tags)
                    {
                        if (tag.TagName != "Singleplayer" && tag.TagName != "Multiplayer")
                        {
                            playerTags.Add(tag.TagName);
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
                            playTimeTwoWeeks = 2 * Math.Sqrt(playTimeTwoWeeks / 60);
                            for (int i = 0; i < (50 + playTimeTwoWeeks); i++)
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
                            playerTags.Add(tag.TagName);
                        }
                    }
                }
            }
        }
    }
}
