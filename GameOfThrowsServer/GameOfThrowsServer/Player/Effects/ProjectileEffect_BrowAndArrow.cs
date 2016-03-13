using Game_Of_Throws_Server;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using GameOfThrowsServer.Player.Effects.EffectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfThrowsServer.Player.Effects
{
    public class ProjectileEffect_BrowAndArrow : IProjectileEffect
    {
        public void Impact(HandledPlayer hpSource, HandledPlayer hpHit, ConnectionHandler cnHandler, out bool bProjectileAlive)
        {
            cnHandler.SingleTarget(hpSource, hpHit, hpSource.FinalStats[Attributes.PhysicalPower]);
            bProjectileAlive = false;
            cnHandler.PostDamage();
        }

        public string Name
        {
            get
            {
                return ("BROw and Arrow projectile");
            }
        }
    }
}
