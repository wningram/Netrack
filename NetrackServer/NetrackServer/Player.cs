using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;

namespace NetrackServer {
    public class Player {
        public static int PlayerHistoryCount = 0;

        public Player() {
            // Increment and set player ID
            Player.PlayerHistoryCount++;
            this.Id = Player.PlayerHistoryCount;
        }

        public int Id { get; protected set; }
        public Point Location { get; set; }
    }
}
