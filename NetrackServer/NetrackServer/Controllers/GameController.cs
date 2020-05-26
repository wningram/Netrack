using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Drawing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetrackServer.Data;
using Microsoft.AspNetCore.Http;

namespace NetrackServer.Controllers
{
    public class GameController : Controller {
        private Map _gameMap;
        private ISessionManager _sessionManager;
        
        public GameController(ISessionManager session, bool testMode=true) {
            _gameMap = new Map(); // TODO: In teh future the parameter for this should be set using the app config file
            _sessionManager = session;
            if (testMode)
                populateTestData();
        }

        public class PlayerLocation {
            public string Location;
            public int PlayerId;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Status([FromQuery] int sessionId) {
            Dictionary<string, object> resp = new Dictionary<string, object>();
            resp["players"] = _sessionManager.GetPlayers(sessionId).Select(p => p.Id).ToArray();
            resp["status"] = "Active";
            return Json(resp);
        }

        [HttpGet]
        public IActionResult Test([FromQuery] int sessionId) {
            List<Player> players = _sessionManager.GetPlayers(sessionId);
            return Ok("Test method has been run.");
        }

        [HttpGet]
        public IActionResult PlayerLocations([FromQuery] int sessionId, [FromQuery] int playerId=-1) {
            Dictionary<string, Point> resp = new Dictionary<string, Point>();
            if (playerId > 0) {
                // Return the location of the player requested
                Player player = _sessionManager.GetPlayers(sessionId).Where(p => p.Id == playerId).FirstOrDefault();
                if (player != null) {
                    // If player with playerId is foudn, return its location as JSON data
                    resp[player.Id.ToString()] = player.Location;
                    return Json(resp);
                } else {
                    // If no player with playerId is found, return 404 status.
                    return NotFound($"Could not find player with Id: {playerId}");
                }
            } else {
                // Return all players
                foreach (Player p in _sessionManager.GetPlayers(sessionId)) {
                    resp[p.Id.ToString()] = p.Location;
                }
                return Json(resp);
            }
        }

        [HttpPost]
        public IActionResult PlayerLocations([FromBody] PlayerLocation playerLocation, [FromQuery] int sessionId) {
            Player player = _sessionManager.GetPlayers(sessionId).Where(p => p.Id == playerLocation.PlayerId).First();
            // Parse location argument
            Point location =Utilities. parseLocation(playerLocation.Location);
            if (player != null) {
                player.Location = location;
                _sessionManager.SetPlayer(player, sessionId);
                return Ok();
            } else {
                // Return 404 error if player is not found
                return NotFound($"No player was found with Id: {playerLocation.PlayerId}");
            }
        }

        private void populateTestData() {
            Player.PlayerHistoryCount = 0;
            //_sessionManager.GetPlayers(sessionId).AddRange(new Player[] {
            //    new Player() {
            //        Location=new Point(10, 20)
            //    },
            //    new Player() {
            //        Location=new Point(10, 21)
            //    },
            //    new Player() {
            //        Location=new Point(5, 20)
            //    },
            //});
        }

//        private Point parseLocation(string loc) {
//            int x, y;
//            string[] data;
//            // Remove leading/trailing spaces
//            loc.Trim();
//            // Get values on either side of the comma
//            data = loc.Split(",");
//            x = int.Parse(data[0]);
//            y = int.Parse(data[1]);
//            return new Point(x, y);
//        }
    }
}