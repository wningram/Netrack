using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetrackServer.Controllers;
using NetrackServer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing;

namespace NetrackServerTests {
    [TestClass]
    public class APITests {
        [TestMethod]
        public void GetPlayerLocations_ReturnsAllPlayerLocations_WhenArgIsLessThanOne() {
            GameController gc = new GameController(true);

            JsonResult result = gc.PlayerLocations(playerId: -1) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(3, ((Dictionary<string, Point>)result.Value).Count);
        }

        [TestMethod]
        public void GetPlayerLocations_ReturnsOnePlayerLocation_WhenArgIsGreaterThanOne() {
            GameController gc = new GameController(true);

            JsonResult result = gc.PlayerLocations(1) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, ((Dictionary<string, Point>)result.Value).Count);
        }

        [TestMethod]
        public void PostPlayerLocations_UpdatesPayer() {
            GameController gc = new GameController(true);

            gc.PlayerLocations("100, 100", 1);

            Player firstPlayer = gc.Players.Find(p => p.Id == 1);
            Assert.AreEqual(100, firstPlayer.Location.X);
            Assert.AreEqual(100, firstPlayer.Location.Y);
        }

        [TestMethod]
        public void GetStatus_ReturnsPlayers() {
            GameController gc = new GameController(true);

            JsonResult result = gc.Status() as JsonResult;

            Dictionary<string, object> resultValue = result.Value as Dictionary<string, object>;
            Assert.AreEqual(3, ((int[])resultValue["players"]).Length);
        }
    }
}
