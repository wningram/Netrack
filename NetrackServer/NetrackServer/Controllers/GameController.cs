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
        
        public GameController() {
            _players = new List<Player>();
            _gameMap = new Map(); // TODO: In teh future the parameter for this should be set using the app config file
            // XXX: For testing purposed
            populateTestData();
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult PlayerLocations([FromQuery] int playerId=-1) {
            if (playerId < 1) {
                // Return the location of the player requested
                Player player = _players.Where(p => p.Id == playerId).First();
                return Json($"{player.Id}: {player.Location.ToString()}"); // TODO: Test to see what result looks like
            } else {
                throw new NotImplementedException();
            }
        }

        [HttpPost]
        public IActionResult PlayerLocations([FromBody] string loc, [FromBody] int playerId) {
            Player player = _players.Where(p => p.Id == playerId).First();
            // Parse location argument
            Point location = parseLocation(loc);
            player.Location = location;
        }

        private void populateTestData() {
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