using System;
using RecGames.Helpers;
using RecGames.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace RecGames.Tests.Helpers
{
    [TestClass]
    public class GameHelpersTest
    {
        [TestMethod]
        public void TestSupportedPlatformsMustReturnWindowsIfWindowsIsSupported()
        {
            string jsonText = @"
            {
                windows: true,
                mac: false,
                linux: false
            }";
            var platforms = JObject.Parse(jsonText);

            Assert.AreEqual(GameHelpers.SupportedPlatforms(platforms), "Windows ");
            
        }

        [TestMethod]
        public void TestSupportedPlatformsMustReturnMacIfMacIsSupported()
        {
            string jsonText = @"
            {
                windows: false,
                mac: true,
                linux: false
            }";
            var platforms = JObject.Parse(jsonText);

            Assert.AreEqual(GameHelpers.SupportedPlatforms(platforms), "Mac ");
        }

        [TestMethod]
        public void TestSupportedPlatformsMustReturnLinuxIfLinuxIsSupported()
        {
            string jsonText = @"
            {
                windows: false,
                mac: false,
                linux: true
            }";
            var platforms = JObject.Parse(jsonText);

            Assert.AreEqual(GameHelpers.SupportedPlatforms(platforms), "Linux ");
        }

        [TestMethod]
        public void TestSupportedPlatformsMustReturnEmptyStringIfNonePlatformsAreSupported()
        {
            string jsonText = @"
            {
                windows: false,
                mac: false,
                linux: false
            }";
            var platforms = JObject.Parse(jsonText);

            Assert.AreEqual(GameHelpers.SupportedPlatforms(platforms), "");
        }

        [TestMethod]
        public void TestGameDevelopersMustReturnStringWithTheDevelopers()
        {
            string jsonText = "[\"Valve\", \"Source\"]";

            var developers = JArray.Parse(jsonText);

            Assert.AreEqual(GameHelpers.GameDevelopers(developers), "Valve,Source");
        }

        [TestMethod]
        public void TestGameDevelopersMustReturnEmptyStringInCaseOfEmptyDevelopersArray()
        {
            string jsonText = "[]";

            var developers = JArray.Parse(jsonText);

            Assert.AreEqual(GameHelpers.GameDevelopers(developers), "");
        }

        [TestMethod]
        public void TestGamePublishersMustReturnStringWithThePublishers()
        {
            string jsonText = "[\"Valve\", \"Source\"]";

            var publishers = JArray.Parse(jsonText);

            Assert.AreEqual(GameHelpers.GamePublishers(publishers), "Valve,Source");
        }

        [TestMethod]
        public void TestGamePublishersMustReturnEmptyStringInCaseOfEmptyPublishersArray()
        {
            string jsonText = "[]";

            var publishers = JArray.Parse(jsonText);

            Assert.AreEqual(GameHelpers.GamePublishers(publishers), "");
        }

        [TestMethod]
        public void TestCalculateRecommendationScoreMustReturnGameIdsListSortedByRecommendationScore()
        {
            Tag t1 = new Tag(1, "action");
            Tag t2 = new Tag(2, "fps");
            Tag t3 = new Tag(3, "war");
            Tag t4 = new Tag(4, "modern");
            Tag t5 = new Tag(5, "multiplayer");
            
            List<Tag> game1Tags = new List<Tag> { t1, t2, t3, t4, t5 };
            List<Tag> game2Tags = new List<Tag> { t1, t2, t3, t4 };
            List<Tag> game3Tags = new List<Tag> { t1, t2, t3 };
            List<Tag> game4Tags = new List<Tag> { t1, t2 };
            List<Tag> game5Tags = new List<Tag> { t1 };

            Game game1 = new Game(1, "Game1", 90, game1Tags, 700000);
            Game game2 = new Game(2, "Game2", 80, game2Tags, 600000);
            Game game3 = new Game(3, "Game3", 70, game3Tags, 500000);
            Game game4 = new Game(4, "Game4", 70, game4Tags, 400000);
            Game game5 = new Game(5, "Game5", 70, game5Tags, 300000);

            List<Game> notOwnedGames = new List<Game> { game1, game2, game3, game4, game5};

            List<string> playerPortrait = new List<string> { "action", "fps", "war", "modern", "multiplayer" };

            Assert.AreEqual(GameHelpers.CalculateRecommendationScore(playerPortrait, notOwnedGames)[0], 1);
            Assert.AreEqual(GameHelpers.CalculateRecommendationScore(playerPortrait, notOwnedGames)[1], 2);
            Assert.AreEqual(GameHelpers.CalculateRecommendationScore(playerPortrait, notOwnedGames)[2], 3);
            Assert.AreEqual(GameHelpers.CalculateRecommendationScore(playerPortrait, notOwnedGames)[3], 4);
            Assert.AreEqual(GameHelpers.CalculateRecommendationScore(playerPortrait, notOwnedGames)[4], 5);


        }

    }
}
