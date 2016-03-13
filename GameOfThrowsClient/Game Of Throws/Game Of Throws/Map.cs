using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.IO;

namespace Game_Of_Throws
{
    /// <summary>
    /// Represents the Map
    /// </summary>
    public class Map
    {
        #region Data members

        // Const definition
        public const int WALL_SIZE = 1;
        public const int BLOCK_SIZE = 100;

        // Static members
        public static int MapWidth { get; set; }
        public static int MapHeight { get; set; }

        // Variable definition
        private Rectangle rectDraw;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the Block matrix
        /// </summary>
        public BlockID[,] BlockMatrix
        {
            get;
            private set;
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the Map class
        /// </summary>
        public Map()
        {
            // Read BlockID matrix from file
            this.BlockMatrix = this.GetMapTextureIndexes("map.dat");

            // Initialize Drawing Rectangle
            this.rectDraw = new Rectangle(0, 0, BLOCK_SIZE, BLOCK_SIZE);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the texture index map from a map file
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Integer Matrix with texture indexes</returns>
        public BlockID[,] GetMapTextureIndexes(string path)
        {
            // Initialize index matrix
            BlockID[,] arrbMap;

            using (FileStream fsFileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                using (BinaryReader brReader = new BinaryReader(fsFileStream))
                {
                    MapHeight = brReader.ReadInt32();
                    MapWidth = brReader.ReadInt32();

                    arrbMap = new BlockID[MapHeight, MapWidth];

                    for (int i = 0; i < MapHeight; i++)
                    {
                        for (int j = 0; j < MapWidth; j++)
                        {
                            arrbMap[i,j] = (BlockID)brReader.ReadByte();
                        }
                    }
                }
            }

            // Return map
            return (arrbMap);
        }

        public void Draw(SpriteBatch spriteBatch, Viewport vpViewport)
        {
            // Increase the DrawRectangle Y axis by the block size
            for (this.rectDraw.Y = 0;
                 this.rectDraw.Y < this.BlockMatrix.GetLength(0) * BLOCK_SIZE;
                 this.rectDraw.Y += BLOCK_SIZE)
            {
                // Increase the DrawRectangle X axis by the block  size
                for (this.rectDraw.X = 0;
                     this.rectDraw.X < this.BlockMatrix.GetLength(1) * BLOCK_SIZE;
                     this.rectDraw.X += BLOCK_SIZE)
                {
                    if (this.rectDraw.Intersects(new Rectangle((int)Camera.CameraVector.X,
                                                               (int)Camera.CameraVector.Y,
                                                               vpViewport.Width,
                                                               vpViewport.Height)))
                    {
                        spriteBatch.Draw(
                               Game1.tBlocks[(byte)(this.BlockMatrix[(this.rectDraw.Y / BLOCK_SIZE),
                                                                     (this.rectDraw.X / BLOCK_SIZE)])],
                               this.rectDraw,
                               Color.White);
                    }
                }
            }
        }

        #endregion
    }
}
