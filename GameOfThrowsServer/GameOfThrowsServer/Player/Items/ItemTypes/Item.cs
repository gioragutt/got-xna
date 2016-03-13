using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;

namespace Game_Of_Throws_Server.Player.Items.ItemTypes
{
    /// <summary>
    /// Basic abstract item
    /// </summary>
    public abstract class Item
    {
        #region Properties

        /// <summary>
        /// Gets and sets represents the attribute changes of the item
        /// </summary>
        public ItemAttributeChanges AttributeChanges { get; set; }

        /// <summary>
        /// Gets and sets the name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the active effect of the item.
        /// </summary>
        private IActiveEffect ActiveEffect { get; set; }

        /// <summary>
        /// Gets and sets the passive effect of the item.
        /// </summary>
        private IPassiveEffect PassiveEffect { get; set; }

        /// <summary>
        /// Gets and sets the UNQ ID of the item
        /// </summary>
        public sbyte ItemID { get; set; }

        /// <summary>
        /// Gets and sets the cost of the item
        /// </summary>
        public short Cost
        {
            get;
            set;
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor of abstract Item
        /// </summary>
        /// <param name="AttributeChanges">The Attribute Changes</param>
        /// <param name="Name">The name of the Item</param>
        /// <param name="ActiveEffect">The Active Effect of the Item</param>
        /// <param name="PassiveEffect">The Passive Effect of the Item</param>
        public Item(ItemAttributeChanges AttributeChanges,
                    string Name,
                    IActiveEffect ActiveEffect,
                    IPassiveEffect PassiveEffect,
                    sbyte bItemID,
                    short nCost)
        {
            this.ActiveEffect = ActiveEffect;
            this.PassiveEffect = PassiveEffect;
            this.Name = Name;
            this.AttributeChanges = AttributeChanges;
            this.ItemID = bItemID;
            this.Cost = nCost;
        }

        #endregion

        #region Methods

        // TODO - THINK ABOUT IT
        public void ActivateItem(HandledPlayer Player, ConnectionHandler Conn)
        {
            if (this.ActiveEffect != null)
            {
                this.ActiveEffect.Activate(Player, Conn);
            }
        }

        public void DeactivateItem()
        {

        }

        #endregion
    }
}
