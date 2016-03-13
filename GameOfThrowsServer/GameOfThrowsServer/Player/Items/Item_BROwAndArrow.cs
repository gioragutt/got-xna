using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using GameOfThrowsServer.Player.Effects;
using GameOfThrowsServer.Player.Effects.EffectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Player.Items
{
    class Item_BROwAndArrow : Weapon
    {
        #region Data members

        static readonly ItemAttributeChanges ATTRIBUTES = new ItemAttributeChanges(0, 0, 0, 1, -20, 0, 0, 3, -10);
        const string NAME = "Emerald Staff Of Exiled Monks";
        static readonly IActiveEffect ACTIVE_EFFECT = null;
        static readonly IPassiveEffect PASSIVE_EFFECT = null;
        const int ATTACK_RANGE = 400;
        const sbyte ITEM_ID = 4;
        const int COST = 200;
        const bool IS_RANGED = true;
        public static int ARROW_WIDTH = 10;
        public static int ARROW_HEIGHT = 30;
        public static int ARROW_SPEED = 1;
		 
	    #endregion

        #region Ctor

        /// <summary>
        /// Initializes all of the items abilities and attribute changes
        /// </summary>
        public Item_BROwAndArrow()
            : base(ATTRIBUTES, NAME, ACTIVE_EFFECT, PASSIVE_EFFECT, ATTACK_RANGE, IS_RANGED, ITEM_ID, COST)
        {
        }

        #endregion
    }
}
