﻿using System;
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
            Point location = parseLocation(loc);
            if (player != null) {
                player.Location = location;
                return Ok();
            } else {
                // REturn 404 error if player is not found
                return NotFound($"No player was found with Id: {playerId}");
            }
        }

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
        }

        private Point parseLocation(string loc) {
            int x, y;
            string[] data;
            // Remove leading/trailing spaces
            loc.Trim();
            // Get values on either side of the comma
            data = loc.Split(",");
            x = int.Parse(data[0]);
            y = int.Parse(data[1]);
            return new Point(x, y);
        }
    }
}