using GameOfThrowsServer.Player.Effects.EffectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Of_Throws_Server.Player.Effects.EffectTypes
{
    public interface IActiveEffect : IEffect
    {
        #region Methods

        void Activate(HandledPlayer Player, ConnectionHandler Conn);

        #endregion
    }
}
