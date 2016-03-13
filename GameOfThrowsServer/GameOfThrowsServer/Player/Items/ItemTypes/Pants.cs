using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;

namespace Game_Of_Throws_Server.Player.Items.ItemTypes
{
    /// <summary>
    /// Chest item slot
    /// </summary>
    public class Pants : Item
    {
        public Pants(ItemAttributeChanges AttributeChanges, string Name,
                    IActiveEffect ActiveEffect, IPassiveEffect PassiveEffect, sbyte bItemID, short nCost)
            : base(AttributeChanges, Name, ActiveEffect, PassiveEffect, bItemID, nCost)
        {

        }
    }
}
