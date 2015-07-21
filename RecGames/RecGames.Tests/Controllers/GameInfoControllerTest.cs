using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecGames.Controllers;
using System.Web.Http;
using System.Collections.Generic;

namespace RecGames.Tests.Controllers
{
    [TestClass]
    public class GameInfoControllerTest
    {
        [TestMethod]
        public void TestDetailsMustAssingCorrectValuesForGameAttributes()
        {
            GameInfoController controller = new GameInfoController();

            IHttpActionResult result = controller.Details(500) as IHttpActionResult;

            Assert.AreEqual(controller.game.GameID, 500);
            Assert.AreEqual(controller.game.Name, "Left 4 Dead");
            Assert.AreEqual(controller.game.ControllersSupported, "full");
            Assert.AreEqual(controller.game.Platforms, "Windows Mac ");
            Assert.AreEqual(controller.game.Developers, "Valve");
            Assert.AreEqual(controller.game.Publishers, "Valve");
            Assert.AreEqual(controller.game.MetacriticScore, 89);
            Assert.AreEqual(controller.game.TotalAchievements, 73);
            Assert.AreEqual(controller.game.LaunchDate, "17 Nov, 2008");

        }

        //[TestMethod]
        //public void TestTagsMustFillTheTagsListCorrectly()
        //{
        //    GameInfoController controller = new GameInfoController();

        //    IHttpActionResult result = controller.Tags(500) as IHttpActionResult;

        //    List<string> tags = new List<string>();

        //    CollectionAssert.AreEqual(controller.tags, tags);
        //}
    }
}
