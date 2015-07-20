using System;
using RecGames.Helpers;
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

    }
}
