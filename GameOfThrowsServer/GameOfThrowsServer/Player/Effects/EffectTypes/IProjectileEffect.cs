using Game_Of_Throws_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfThrowsServer.Player.Effects.EffectTypes
{
    public interface IProjectileEffect : IEffect
    {
        void Impact(HandledPlayer hpSource, HandledPlayer hpHit, ConnectionHandler cnHandler, out bool bProjectileAlive);
    }
}
