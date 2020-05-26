using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetrackServer.Controllers;
using NetrackServer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing;
using NetrackServer.Data;

namespace NetrackServerTests {
    [TestClass]
    public class GeneralTests {
        public class TestSessionManager : ISessionManager {
            private List<Player> _players;

            public TestSessionManager() {
                _players = new List<Player>() {
                    new Player(1, new Point(500, 100)),
                    new Player(2, new Point(600, 0))
                };
            }

            Map ISessionManager.GetMap(int sessionId) {
                return new Map("", "TestMap");
            }

            List<Player> ISessionManager.GetPlayers(int sessionId) {
                return _players;
            }

            void ISessionManager.SetPlayer(Player player, int sessionId) {
                return;
            }
        }

        [TestMethod]
        public void GetPlayerLocations_ReturnsAllPlayerLocations_WhenArgIsLessThanOne() {
            GameController gc = new GameController(new TestSessionManager(), true);

            JsonResult result = gc.PlayerLocations(playerId: -1, sessionId: 1) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(2, ((Dictionary<string, Point>)result.Value).Count);
        }

        [TestMethod]
        public void GetPlayerLocations_ReturnsOnePlayerLocation_WhenArgIsGreaterThanOne() {
            GameController gc = new GameController(new TestSessionManager(), true);

            JsonResult result = gc.PlayerLocations(0, 1) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, ((Dictionary<string, Point>)result.Value).Count);
        }

        [TestMethod]
        public void PostPlayerLocations_UpdatesPayer() {
            ISessionManager testMgr = new TestSessionManager();
            GameController gc = new GameController(testMgr, true);
            GameController.PlayerLocation loc = new GameController.PlayerLocation() {
                Location = "100, 100",
                PlayerId = 1
            };

            gc.PlayerLocations(loc, 0);

            Player firstPlayer = testMgr.GetPlayers(0).Find(p => p.Id == 1);
            Assert.AreEqual(100, firstPlayer.Location.X);
            Assert.AreEqual(100, firstPlayer.Location.Y);
        }

        [TestMethod]
        public void GetStatus_ReturnsPlayers() {
            ISessionManager testMgr = new TestSessionManager();
            GameController gc = new GameController(testMgr, true);

            JsonResult result = gc.Status(0) as JsonResult;

            Dictionary<string, object> resultValue = result.Value as Dictionary<string, object>;
            Assert.AreEqual(2, ((int[])resultValue["players"]).Length);
        }
    }
}
