using GameOfThrowsServer.Player.Effects.EffectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Of_Throws_Server.Player.Effects.EffectTypes
{
    public interface IPassiveEffect : IEffect
    {
        #region Methods

        void ApplyOn();

        #endregion
    }
}
