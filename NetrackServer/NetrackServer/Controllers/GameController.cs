// TODO: Need actions for initiating/managing game session

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Drawing;

namespace NetrackServer.Controllers
{
    public class GameController : Controller {
        private List<Player> _players;
        private Map _gameMap;
        
        public GameController(bool testMode=true) {
            _players = new List<Player>();
            _gameMap = new Map(); // TODO: In teh future the parameter for this should be set using the app config file
            if (testMode)
                populateTestData();
        }

        public List<Player> Players {
            get => _players;
        }

        public Map GameMap { get => _gameMap; }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Status() {
            Dictionary<string, object> resp = new Dictionary<string, object>();
            resp["players"] = _players.Select(p => p.Id).ToArray();
            resp["status"] = "Active";
            return Json(resp);
        }

        [HttpGet]
        public IActionResult PlayerLocations([FromQuery] int playerId=-1) {
            Dictionary<string, Point> resp = new Dictionary<string, Point>();
            if (playerId > 0) {
                // Return the location of the player requested
                Player player = _players.Where(p => p.Id == playerId).FirstOrDefault();
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
                foreach (Player p in _players) {
                    resp[p.Id.ToString()] = p.Location;
                }
                return Json(resp);
            }
        }

        [HttpPost]
        public IActionResult PlayerLocations([FromBody] string loc, [FromBody] int playerId) {
            Player player = _players.Where(p => p.Id == playerId).First();
            // Parse location argument
            //Point location = parseLocation(loc);
            Point location = Common.DeserializePoint(loc);
            if (player != null) {
                player.Location = location;
                return Ok();
            } else {
                // REturn 404 error if player is not found
                return NotFound($"No player was found with Id: {playerId}");
            }
        }

        [HttpGet]
        public IActionResult MapDetails() {
            Dictionary<string, object> result;

            // Return an error if there's no map to return
            if (GameMap == null) {
                return NotFound("There is no map currently registered.");
            }

            result = new Dictionary<string, object>() {
                {"MapName", GameMap.MapName },
                {"MaxX", GameMap.MaxX },
                {"MaxY", GameMap.MaxY }
            };
            return Json(result);
        }

        /// <summary>
        /// Function is for testing only. Initializes server state with test data.
        /// </summary>
        private void populateTestData() {
            Player.PlayerHistoryCount = 0;
            _players.AddRange(new Player[] {
                new Player() {
                    Location=new Point(10, 20)
                },
                new Player() {
                    Location=new Point(10, 21)
                },
                new Player() {
                    Location=new Point(5, 20)
                },
            });

            // Populate map data
            this._gameMap = new Map();
            this._gameMap.MapName = "Untitled Map";
            this._gameMap.Tiles.Add(new Map.MapTile() {
                Location = new Point(35, 25),
                Type = Map.TileTypes.EMPTY
            });
            this._gameMap.Tiles.Add(new Map.MapTile() {
                Location = new Point(100, 15),
                Type = Map.TileTypes.ASPHALT
            });
            this._gameMap.CalculateMaxBounds();
        }
    }
}