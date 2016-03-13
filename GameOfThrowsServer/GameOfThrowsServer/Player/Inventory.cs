using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using System.IO;
using ConsoleServer.Player.Items;

namespace Game_Of_Throws_Server.Player
{
    public class Inventory
    {
        #region Data Members

        // Const Members
        private const int ITEMS_AMOUNT = 5;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets inventory
        /// </summary>
        public Dictionary<Type, Item> ItemDic;

        /// <summary>
        /// Gets and sets the list of all items
        /// </summary>
        public static Type[] AllItems;

        /// <summary>
        /// Indexer operator for the inventory
        /// </summary>
        /// <param name="tItemType">The type of item i want to get or set</param>
        /// <returns>the item in sent item type</returns>
        public Item this[Type tItemType]
        {
            get
            {
                return (this.ItemDic[tItemType]);
            }
            private set
            {
                this.ItemDic[tItemType] = value;
            }
        }

        /// <summary>
        /// Indexer operator for the inventory
        /// </summary>
        /// <param name="tItemType">The type of item to get</param>
        /// <returns>the item in sent item type</returns>
        public Item this[sbyte bItemIndex]
        {
            get
            {
                return (this.ItemDic.Values.ElementAt(bItemIndex));
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the Invetory class
        /// </summary>
        public Inventory()
        {
            this.ItemDic = new Dictionary<Type, Item>();

            ItemDic.Add(typeof(Helmet), null);
            ItemDic.Add(typeof(Chest), null);
            ItemDic.Add(typeof(Weapon), null);
            ItemDic.Add(typeof(OffHand), null);
            ItemDic.Add(typeof(Pants), null);
            ItemDic.Add(typeof(Gloves), null);
            ItemDic.Add(typeof(Shoes), null);
        }

        static Inventory()
        {
            AllItems = new Type[ITEMS_AMOUNT];

            #region Insert Items Into Array

            AllItems[0] = typeof(Item_EmeraldStaffOfExiledMonks);
            AllItems[1] = typeof(Item_RingOfGodsFury);
            AllItems[2] = typeof(Item_Booties);
            AllItems[3] = typeof(Item_ArmorOfBooze);
            AllItems[4] = typeof(Item_BROwAndArrow);

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Equiping an item if its slot is available
        /// </summary>
        /// <param name="itmNewItem"></param>
        /// <returns></returns>
        public bool EquipItem(Item itmNewItem)
        {
            bool bIsEquipSuccessfull = false;

            // If this item slot is available
            if (this[itmNewItem.GetType().BaseType] == null)
            {
                // Placing the item in its slot
                this[itmNewItem.GetType().BaseType] = itmNewItem;

                bIsEquipSuccessfull = true;
            }

            return (bIsEquipSuccessfull);
        }

        /// <summary>
        /// Chaning the item slot to the sent item
        /// </summary>
        /// <param name="itmNewItem">The sent item</param>
        public void ReplaceItem(Item itmNewItem)
        {
            // Changing the item slot to this item
            this[itmNewItem.GetType().BaseType] = itmNewItem;
        }
        
        /// <summary>
        /// Empties the player's inventory
        /// </summary>
        public void EmptyInventory()
        {
            // Nullifies every item the player has
            foreach(Type itmCurr in this.ItemDic.Keys)
            {
                this.ItemDic[itmCurr] = null;
            }
        }

        /// <summary>
        /// Writes the inventory data to stream
        /// </summary>
        /// <param name="bwWriter">Binary writer</param>
        public void WriteToStream(BinaryWriter bwWriter)
        {
            // Go over items in inventory
            foreach (Item itmCurr in this.ItemDic.Values)
            {
                if (itmCurr != null)
                {
                    // Write current item id
                    bwWriter.Write(itmCurr.ItemID);
                }
                else
                {
                    // Write nothing representor
                    bwWriter.Write((sbyte)-1);
                }
            }
        }

        #endregion
    } 
}
