using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Of_Throws_Server.Player.Effects;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using GameOfThrowsServer.Player.Effects.EffectTypes;

namespace ConsoleServer.Player.Items
{
    public class Item_EmeraldStaffOfExiledMonks : Weapon
    {
        #region Data members

        static readonly ItemAttributeChanges ATTRIBUTES = new ItemAttributeChanges(0, 0, 1, 2, 20, 40, 0, 0, 0);
        const string NAME = "Emerald Staff Of Exiled Monks";
        static readonly IActiveEffect ACTIVE_EFFECT = new ActiveEffect_EmeraldStaffOfExiledMonks();
        static readonly IPassiveEffect PASSIVE_EFFECT = new PassiveEffect_EmeraldStaffPfExiledMonks();
        const int ATTACK_RANGE = 60;
        const sbyte ITEM_ID = 0;
        const int COST = 150;
        const bool IS_RANGED = false;

	    #endregion

        #region Ctor

        /// <summary>
        /// Initializes all of the items abilities and attribute changes
        /// </summary>
        public Item_EmeraldStaffOfExiledMonks()
            : base(ATTRIBUTES, NAME, ACTIVE_EFFECT, PASSIVE_EFFECT, ATTACK_RANGE, IS_RANGED, ITEM_ID, COST)
        {
        }

        #endregion
    }
}
