using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;
using Newtonsoft.Json;
using System.IO;

namespace NetrackServer {


    [Serializable]
    public class InvalidMapFileException : Exception {
        public InvalidMapFileException() { }
        public InvalidMapFileException(string message) : base(message) { }
        public InvalidMapFileException(string message, Exception inner) : base(message, inner) { }
        protected InvalidMapFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class Map {
        private List<MapTile> _tiles;

        public enum TileTypes {
            EMPTY = -1,
            ASPHALT = 0,
            BARRIER = 1,
            FINISH_LINE = 2
        }

        public struct MapTile {
            public Point Location;
            public TileTypes Type;
        }

        public Map() {
            this._tiles = new List<MapTile>();
        }

        public Map(string mapFIle) : this() {
            LoadTileMap(mapFIle);
        }

        public List<MapTile> Tiles { get=>this._tiles; }
        public string MapName { get; set; }
        public int MaxX { get; protected set; }
        public int MaxY { get; protected set; }

        /// <summary>
        /// Gets the map tile at the specified location.
        /// </summary>
        /// <param name="loc">The location for the map tile to return.</param>
        /// <returns></returns>
        public MapTile GetTile(Point loc) {
            return _tiles.Where(t => t.Location == loc).First();
        }

        public static void CreateTileMapFileTemplate(string filePath) {
            Console.WriteLine($"Creating template file at: {filePath}");
            using (JsonTextWriter jWriter = new JsonTextWriter(new StreamWriter(filePath))) {
                jWriter.WriteStartObject();
                jWriter.WritePropertyName("MapName");
                jWriter.WriteValue("UntitledMap");
                jWriter.WritePropertyName("MaxX");
                jWriter.WriteValue(0);
                jWriter.WritePropertyName("MaxY");
                jWriter.WriteValue(0);

                jWriter.WritePropertyName("TIles");
                jWriter.WriteStartArray();
                jWriter.WriteStartObject();
                jWriter.WritePropertyName("Location");
                jWriter.WriteValue("(12, 13)");
                jWriter.WritePropertyName("Type");
                jWriter.WriteValue("EMPTY");
                jWriter.WriteEndObject();
                jWriter.WriteEndArray();
                jWriter.WriteEndObject();
            }
            Console.WriteLine("Template file created.");
        }

        /// <summary>
        /// Loads map tiles from a file into the <see cref="_tiles"/> variable.
        /// </summary>
        /// <param name="filePath">The path of the file to read.</param>
        public void LoadTileMap(string filePath) {
            FileInfo file = new FileInfo(filePath);
            // Check that file exists
            if (file.Exists) {
                using (JsonTextReader jReader = new JsonTextReader(new StreamReader(filePath))) {
                    bool inMapTileObj = false;
                    string parentProperty = null;
                    MapTile nextTile = new MapTile();

                    while (jReader.Read()) {
                        switch (jReader.TokenType) {
                            case JsonToken.PropertyName:
                                // The parent property is equal to the last PropertyName that was read
                                parentProperty = jReader.Value.ToString();
                                break;
                            case JsonToken.StartArray:
                                // If property name for this array is Tiles, we are dealing with a MapTile object
                                inMapTileObj = (parentProperty == "Tiles");
                                break;
                            case JsonToken.EndArray:
                                inMapTileObj = false;
                                break;
                            case JsonToken.EndObject:
                                if (inMapTileObj)
                                    Tiles.Add(nextTile);
                                    nextTile = new MapTile();
                                break;
                            case JsonToken.String:
                            case JsonToken.Integer:
                                try {
                                    if (jReader.Value != null) {
                                        if (inMapTileObj) {
                                            switch (parentProperty) {
                                                case "Location":
                                                    // Convert serialized point to Point object
                                                    nextTile.Location = Common.DeserializePoint(jReader.Value.ToString());
                                                    break;
                                                case "Type":
                                                    // COnvert the string representation of the TileMap's type to a TileTypes value and set value on nextTile variable
                                                    nextTile.Type = Enum.Parse<TileTypes>(jReader.Value.ToString(), true);
                                                    break;
                                            }
                                        } else {
                                            // Variable to hold MaxX and MaxY conversion
                                            int convertedValue;
                                            switch (parentProperty) {
                                                case "MapName":
                                                    MapName = jReader.Value.ToString();
                                                    break;
                                                case "MaxX":
                                                    if (!int.TryParse(jReader.Value.ToString(), out convertedValue)) {
                                                        throw new InvalidMapFileException("MaxX value must be an integer type.");
                                                    } else {
                                                        MaxX = convertedValue;
                                                    }
                                                    break;
                                                case "MaxY":
                                                    if (!int.TryParse(jReader.Value.ToString(), out convertedValue)) {
                                                        throw new InvalidMapFileException("MaxY value must be an integer type.");
                                                    } else {
                                                        MaxY = convertedValue;
                                                    }
                                                    break;
                                            }
                                        } 
                                    }
                                } catch (ArgumentException ex) {
                                    throw new InvalidMapFileException($"The map file supplied is in an invalid format: '{filePath}'. Skipping current element.", ex);
                                }
                                break;
                        }
                    }
                }
            } else {
                throw new FileNotFoundException($"Could not load TileMap from: {filePath}");
            }

            //throw new NotImplementedException();
        }

        public void CalculateMaxBounds() {
            int xBound, yBound;

            // Get X Bound
            Tiles.Sort((a, b) => {
                int aX = a.Location.X,
                bX = b.Location.X;
                if (aX > bX) return 1;
                if (aX < bX) return -1;
                else return 0;
            });
            xBound = Tiles[Tiles.Count - 1].Location.X;

            // Get Y Bound
            Tiles.Sort((a, b) => {
                int aY = a.Location.Y,
                bY = b.Location.Y;
                if (aY > bY) return 1;
                if (aY < bY) return -1;
                else return 0;
            });
            yBound = Tiles[Tiles.Count - 1].Location.Y;

            MaxX = xBound;
            MaxY = yBound;
        }
    }
}
