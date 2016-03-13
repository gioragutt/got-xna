using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;

namespace Game_Of_Throws
{
    /// <summary>
    /// Represents the camera in the game
    /// </summary>
    public static class Camera
    {
        #region Data Members

        public static Vector3 CameraVector = new Vector3(0, 0, 0);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the camera matrix of the camera.
        /// </summary>
        public static Matrix CameraMatrix
        {
            get
            {
                return (Matrix.CreateTranslation(-CameraVector));
            }
        }
        
        #endregion

        /// <summary>
        /// Updates the camera vector
        /// </summary>
        /// <param name="plrPlayer">Player to set camera on</param>
        /// <param name="grDevice">GraphicsDevice of game</param>
        public static void UpdateCamera(Player plrPlayer, GraphicsDevice grDevice)
        {
            // Get position for camera center
            CameraVector.X = plrPlayer.PosX - (grDevice.Viewport.Width / 2) + (plrPlayer.Rect.Width / 2);
            CameraVector.Y = plrPlayer.PosY - (grDevice.Viewport.Height / 2) + (plrPlayer.Rect.Height / 2);

            #region Ensuring camera does not move out of the map borders
            
            if (CameraVector.X <= 0)
            {
                CameraVector.X = 0;
            }

            if (CameraVector.X >= Map.BLOCK_SIZE * Map.MapWidth - grDevice.Viewport.Width)
            {
                CameraVector.X = Map.BLOCK_SIZE * Map.MapHeight - grDevice.Viewport.Width;
            }

            if (CameraVector.Y <= 0)
            {
                CameraVector.Y = 0;
            }

            if (CameraVector.Y >= Map.BLOCK_SIZE * Map.MapWidth - grDevice.Viewport.Height)
            {
                CameraVector.Y = Map.BLOCK_SIZE * Map.MapHeight - grDevice.Viewport.Height;
            }

            #endregion
        }
    }
}
