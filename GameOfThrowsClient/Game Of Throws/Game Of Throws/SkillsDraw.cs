using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Threading;

namespace Game_Of_Throws
{
    public static class SkillsDraw
    {
        public static void DrawEmeraldStaff(Player plrPlayer, SpriteBatch spriteBatch)
        {
            int TEMP_WEAPON_RANGE = 1000;
            bool bIsHorizontal = false;

            Rectangle rctAttackBox =
                new Rectangle(plrPlayer.Rect.X, plrPlayer.Rect.Bottom, plrPlayer.Rect.Width, TEMP_WEAPON_RANGE);

            // Creates the attack rectangle according to the direction were looking at
            switch (plrPlayer.Direction)
            {
                case (Direction.Up):
                    {
                        rctAttackBox =
                            new Rectangle(plrPlayer.Rect.X, plrPlayer.Rect.Y - TEMP_WEAPON_RANGE, plrPlayer.Rect.Width, TEMP_WEAPON_RANGE);

                        break;
                    }
                case (Direction.Left):
                    {
                        rctAttackBox =
                            new Rectangle(plrPlayer.Rect.X - TEMP_WEAPON_RANGE, plrPlayer.Rect.Y, TEMP_WEAPON_RANGE, plrPlayer.Rect.Height);
                        bIsHorizontal = true;

                        break;
                    }
                case (Direction.Right):
                    {
                        rctAttackBox =
                            new Rectangle(plrPlayer.Rect.Right, plrPlayer.Rect.Y, TEMP_WEAPON_RANGE, plrPlayer.Rect.Height);
                        bIsHorizontal = true;

                        break;
                    }
            }

            if (bIsHorizontal)
            {
                spriteBatch.Draw(Game1.tEmeraldStaffSkill[0], rctAttackBox, Color.Green);
            }
            else
            {
                spriteBatch.Draw(Game1.tEmeraldStaffSkill[1], rctAttackBox, Color.Green);
            }
        }

        public static void DrawRingOfGodsFury(Player plrPlayer, SpriteBatch spriteBatch)
        {
            Thread thrdLightining = new Thread(NegativeFlash);
            thrdLightining.IsBackground = true;

            thrdLightining.Start();
        }

        private static void NegativeFlash()
        {
            Stopwatch stpTimer = new Stopwatch();
            stpTimer.Start();

            while (stpTimer.ElapsedMilliseconds < 500)
            {
                if (Game1.effWholeScreenEffect == null)
                {
                    Game1.effWholeScreenEffect = Game1.effRingOfGodsFury;
                }
                else
                {
                    Game1.effWholeScreenEffect = null;
                }

                Thread.Sleep(30);
            }

            Game1.effWholeScreenEffect = null;
        }
    }
}
