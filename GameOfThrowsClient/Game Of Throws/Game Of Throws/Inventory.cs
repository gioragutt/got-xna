using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Of_Throws
{
    public class Inventory
    {
        #region Data Members

        // Const Members
        public const int INVENTORY_SIZE = 7;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets inventory
        /// </summary>
        private Item[] arritmItems;

        /// <summary>
        /// Indexer operator for the inventory
        /// </summary>
        /// <param name="tItemType">The type of item i want to get or set</param>
        /// <returns>the item in sent item type</returns>
        public Item this[byte bItemType]
        {
            get
            {
                return (this.arritmItems[bItemType]);
            }
            private set
            {
                this.arritmItems[bItemType] = value;
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the Invetory class
        /// </summary>
        public Inventory()
        {
            this.arritmItems = new Item[INVENTORY_SIZE];
        }

        #endregion

        #region Other Methods

        public void Read(BinaryReader brReader)
        {
            for (byte i = 0; i < this.arritmItems.Length; i++)
            {
                sbyte sbItemID = brReader.ReadSByte();
                if (sbItemID != -1)
                {
                    this.arritmItems[i] = Game1.arritmItem.Single((itm) => itm.ItemID == sbItemID);
                }
                else
                {
                    this.arritmItems[i] = null;
                }
            }
        }

        #endregion
    } 
}
