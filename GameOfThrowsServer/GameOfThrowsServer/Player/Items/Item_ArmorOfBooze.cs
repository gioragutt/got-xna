using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleServer.Player.Effects;

namespace ConsoleServer.Player.Items
{
    public class Item_ArmorOfBooze : Chest
    {
        #region Data members

        static readonly ItemAttributeChanges ATTRIBUTES = new ItemAttributeChanges(100, 0, 0, 0, 0, 0, 10, 0, -5);
        const string NAME = "Armor of Booze";
        static readonly IActiveEffect ACTIVE_EFFECT = new ActiveEffect_ArmorOfBooze();
        static readonly IPassiveEffect PASSIVE_EFFECT = null;
        const sbyte ITEM_ID = 3;
        const int COST = 100;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes all of the items abilities and attribute changes
        /// </summary>
        public Item_ArmorOfBooze()
            : base(ATTRIBUTES, NAME, ACTIVE_EFFECT, PASSIVE_EFFECT, ITEM_ID, COST)
        {
        }

        #endregion
    }
}
