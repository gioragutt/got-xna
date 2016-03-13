using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServer.Player.Items
{
    public class Item_Booties : Shoes
    {
        #region Data members

        static readonly ItemAttributeChanges ATTRIBUTES = new ItemAttributeChanges(0, 0, 0, 0, 0, 0, 0, 8, 0);
        const string NAME = "Booties";
        static readonly IActiveEffect ACTIVE_EFFECT = null;
        static readonly IPassiveEffect PASSIVE_EFFECT = null;
        const sbyte ITEM_ID = 2;
        const int COST = 100;
		 
	    #endregion

        #region Ctor

        /// <summary>
        /// Initializes all of the items abilities and attribute changes
        /// </summary>
        public Item_Booties()
            : base(ATTRIBUTES, NAME, ACTIVE_EFFECT, PASSIVE_EFFECT, ITEM_ID, COST)
        {
        }

        #endregion
    }
}
