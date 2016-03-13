using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Microsoft.Xna.Framework;

namespace Game_Of_Throws_Server.Player.Effects
{
    public class ActiveEffect_EmeraldStaffOfExiledMonks : IActiveEffect
    {
        public string Name { get; set; }

        public void Activate(HandledPlayer Player, ConnectionHandler Conn)
        {
            const short MANA_COST = 100;

            if (Player.MP >= MANA_COST)
            {
                Player.MP -= MANA_COST;
                const int DAMAGE = 175;
                const int AOE = 1000;

                #region Creating Rectangle

                // Creating rectangle (default is towards down)
                Rectangle Rect =
                    new Rectangle(Player.Rect.X, Player.Rect.Bottom, Player.Rect.Width, AOE);

                // Creates the attack rectangle according to the direction were looking at
                switch (Player.Direction)
                {
                    case (Direction.Up):
                        {
                            Rect =
                                new Rectangle(Player.Rect.X, Player.Rect.Y - AOE, Player.Rect.Width, AOE);

                            break;
                        }
                    case (Direction.Left):
                        {
                            Rect =
                                new Rectangle(Player.Rect.X - AOE, Player.Rect.Y, AOE, Player.Rect.Height);

                            break;
                        }
                    case (Direction.Right):
                        {
                            Rect =
                                new Rectangle(Player.Rect.Right, Player.Rect.Y, AOE, Player.Rect.Height);

                            break;
                        }
                }

                #endregion

                Conn.AffectArea(Rect, DAMAGE, Player); 
            }
        }
    }
}
