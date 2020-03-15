using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NetrackServer {
    public class Map {
        private List<MapTile> _tiles;

        public struct MapTile {
            public Point Location;
            public int Type;
        }
        
        public Map() {
            this._tiles = new List<MapTile>();
        }

        public Map(string mapFIle) : this() {
            LoadTileMap(mapFIle);
        }

        /// <summary>
        /// Gets the map tile at the specified location.
        /// </summary>
        /// <param name="loc">The location for the map tile to return.</param>
        /// <returns></returns>
        public MapTile GetTile(Point loc) {
            return _tiles.Where(t => t.Location == loc).First();
        }

        /// <summary>
        /// Loads map tiles from a file into the <see cref="_tiles"/> variable.
        /// </summary>
        /// <param name="filePath">The path of the file to read.</param>
        public void LoadTileMap(string filePath) {
            FileInfo file = new FileInfo(filePath);
            // Check that file exists
            if (file.Exists) {
                JObject jsonObj = JObject.Parse(File.ReadAllText(filePath));
                
            } else {
                throw new FileNotFoundException();
            }
        }
    }
}
