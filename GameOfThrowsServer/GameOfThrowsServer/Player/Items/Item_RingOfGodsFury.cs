using ConsoleServer.Player.Effects;
using ConsoleServer.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Player.Items
{
    class Item_RingOfGodsFury : OffHand
    {
        #region Data Members

        static readonly ItemAttributeChanges ATTRIBUTES = new ItemAttributeChanges(0, 50, 0, 5, 0, 100, 0, 0, 0);
        const string NAME = "Ring Of Gods Fury";
        static readonly IActiveEffect ACTIVE_EFFECT = new ActiveEffect_RingOfGodsFury();
        static readonly IPassiveEffect PASSIVE_EFFECT = new PassiveEffect_RingOfGodsFury();
        const int ATTACK_RANGE = 10000;
        const sbyte ITEM_ID = 1;
        const short COST = 1000;

        #endregion

        #region Ctor

        public Item_RingOfGodsFury()
            : base(ATTRIBUTES, NAME, ACTIVE_EFFECT, PASSIVE_EFFECT, ITEM_ID, COST)
        {
        }

        #endregion
    }
}
