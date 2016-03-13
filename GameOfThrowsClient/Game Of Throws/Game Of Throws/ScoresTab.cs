using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Of_Throws
{
    static class ScoresTab
    {
        #region Data Members

        // Const Members
        private const int LOW_PING = 30;
        private const int MEDIUM_PING = 60;
        private const int UPDATE_RATE = 20;

        // Data Members
        private static int nTick = 0;
        public static int FRAME_WIDTH = 500;
        public static int FRAME_HEIGHT;

        public static long nPing;

        #endregion

        #region XNA Methods

        public static void Draw(SpriteBatch spriteBatch, Player[] nplPlayers, long nUpdatedPing, int nPosX, int nPosY)
        {
            #region Frame

            FRAME_HEIGHT = 100 + (nplPlayers.Length * 25);

            Rectangle rctFrame = new Rectangle(nPosX - 25, nPosY - 10, FRAME_WIDTH, FRAME_HEIGHT);
            spriteBatch.Draw(Game1.tScoreTab, rctFrame, Color.White);

            #endregion

            #region Ping

            // Draw own ping
            spriteBatch.DrawString(Game1.sfScoresTabFont,
                                   "Ping: ",
                                   new Vector2(nPosX, nPosY),
                                   Color.White);

            // If ping should be updated
            if (nTick == 0)
            {
                nPing = nUpdatedPing;
            }

            // Increase nTick by 1
            nTick = (nTick + 1) % UPDATE_RATE;

            Color clrPing;

            #region Choose Ping Color

            if (nPing <= LOW_PING)
            {
                clrPing = Color.LightGreen;
            }
            else if (nPing <= MEDIUM_PING)
            {
                clrPing = Color.Orange;
            }
            else
            {
                clrPing = Color.Red;
            }

            #endregion

            // Draw own ping
            spriteBatch.DrawString(Game1.sfScoresTabFont,
                                   nPing.ToString(),
                                   new Vector2(nPosX + (int)Game1.sfScoresTabFont.MeasureString("Ping: ").X,
                                               nPosY),
                                   clrPing);

            #endregion

            #region KD Table

            // Initialize scores tab text
            string strFinalScoresTab = "   Name      Kills     Deaths    Gold\n";


            spriteBatch.DrawString(Game1.sfScoresTabDetailsFont,
                                   strFinalScoresTab,
                                   new Vector2(nPosX, nPosY + 25),
                                   Color.Azure);

            // Add players to text
            for (int i = 0; i < nplPlayers.Length; i++)
            {
                Color cCurrentColor = (nplPlayers[i].IsAlive ? Color.Azure : Color.Gray);

                spriteBatch.DrawString(Game1.sfScoresTabDetailsFont,
                                       nplPlayers[i].Name,
                                       new Vector2(nPosX + 30, nPosY + 50 + (20 * (i))),
                                       cCurrentColor);

                spriteBatch.DrawString(Game1.sfScoresTabDetailsFont,
                                       nplPlayers[i].Kills.ToString(),
                                       new Vector2(nPosX + 137, nPosY + 50 + (20 * (i))),
                                       cCurrentColor);

                spriteBatch.DrawString(Game1.sfScoresTabDetailsFont,
                                       nplPlayers[i].Deaths.ToString(),
                                       new Vector2(nPosX + 223, nPosY + 50 + (20 * (i))),
                                       cCurrentColor);

                spriteBatch.DrawString(Game1.sfScoresTabDetailsFont,
                                       nplPlayers[i].Gold.ToString(),
                                       new Vector2(nPosX + 324, nPosY + 50 + (20 * (i))),
                                       cCurrentColor);
            }

            #endregion
        }

        #endregion
    }
}
