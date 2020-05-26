using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace NetrackServer {
    public static class Utilities {
        public static Point parseLocation(string loc) {
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
