using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;

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

        public Map(string mapFIle, string mapName) : this(mapFIle) {
            MapName = mapName;
        }

        public string MapName { get; protected set; }

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
            // TODO: Implement ability to load tilemaps from file
            throw new NotImplementedException();
        }
    }
}
