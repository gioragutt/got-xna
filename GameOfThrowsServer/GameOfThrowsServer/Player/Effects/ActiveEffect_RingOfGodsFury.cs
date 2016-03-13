using Game_Of_Throws_Server;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Player.Effects.EffectTypes
{
    public class ActiveEffect_RingOfGodsFury : IActiveEffect 
    {
        public string Name { get; set; }

        public void Activate(HandledPlayer Player, ConnectionHandler Conn)
        {
            const short MANA_COST = 200;

            // If the player has enough Mana
            if (Player.MP >= MANA_COST)
            {
                Player.MP -= MANA_COST;
                const int DAMAGE = 500;

                Conn.AffectArea(DAMAGE, Player);
            }
        }
    }
}
