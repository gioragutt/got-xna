using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using GameOfThrowsServer.Player.Effects.EffectTypes;

namespace Game_Of_Throws_Server.Player.Items.ItemTypes
{
    /// <summary>
    /// Item slot for both right hand and left hand item slots
    /// </summary>
    public class Weapon : Item
    {
        /// <summary>
        /// Gets and sets attack range of the weapon
        /// </summary>
        public int AttackRange { get; set; }

        /// <summary>
        /// Gets and sets whether the weapon is a ranged one or not
        /// </summary>
        public bool IsRanged
        {
            get;
            set;
        }

        public Weapon(ItemAttributeChanges AttributeChanges,
                      string Name,
                      IActiveEffect ActiveEffect,
                      IPassiveEffect PassiveEffect,
                      int nAttackRange,
                      bool bIsRanged,
                      sbyte bItemID,
                      short nCost)
            : base(AttributeChanges, Name, ActiveEffect, PassiveEffect, bItemID, nCost)
        {
            this.AttackRange = nAttackRange;
            this.IsRanged = bIsRanged;
        }
    }
}
