using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using NetrackServer;

namespace NetrackServerTests {
    [TestClass]
    public class MapTests {
        [TestMethod]
        public void Test_LoadTileMap() {
            string testFilePath = "loadtilemap-test.map";
            try {
                // Setup
                // Create Test File
                string testFileData = @"{
                    'MapName': 'MyMap',
                    'MaxX': 20,
                    'MaxY': 20,
                    'Tiles': [
                        {
                            'Location': '(12, 13)', // Not sure how points will be represented
                            'Type': 'EMPTY'
                        },
                        {
                            'Location': '(23, 34)',
                            'Type': 'EMPTY'
                        }
                    ]
                }";
                File.WriteAllText(testFilePath, testFileData);

                // Test
                Map map = new Map(testFilePath);

                // Assertions
                Assert.AreEqual("MyMap", map.MapName);
                Assert.AreEqual(20, map.MaxX);
                Assert.AreEqual(20, map.MaxY);

                Assert.AreEqual(2, map.Tiles.Count);
                Map.MapTile loadedTile = map.Tiles[0];
                // TODO: Add assertion for "Location" when implementation is compeleted.
                Assert.AreEqual(new System.Drawing.Point(23, 34), map.Tiles[1].Location);
                Assert.AreEqual(Map.TileTypes.EMPTY, loadedTile.Type);
            } finally {
                // Cleanup test file if it exists
                if (File.Exists(testFilePath))
                    File.Delete(testFilePath);
            }
        }

        [TestMethod]
        public void Test_CalculateMaxBounds() {
            // Setup
            Map map = new Map();
            map.MapName = "Test Map";
            map.Tiles.AddRange(
                new Map.MapTile[] {
                    new Map.MapTile() {
                        Location = new System.Drawing.Point(35, 25),
                        Type = Map.TileTypes.EMPTY
                    },
                    new Map.MapTile() {
                        Location = new System.Drawing.Point(100, 15),
                        Type = Map.TileTypes.ASPHALT
                    }
                });

            // Call function
            map.CalculateMaxBounds();

            // Assertions
            Assert.AreEqual(100, map.MaxX);
            Assert.AreEqual(25, map.MaxY);
        }
    }
}
