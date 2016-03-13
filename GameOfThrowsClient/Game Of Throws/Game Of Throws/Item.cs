 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Game_Of_Throws
{
    public class Item
    {
        #region Data Members

        /// <summary>
        /// Gets the default attack range of an item
        /// </summary>
        private static int DefaultAttackRange
        {
            get
            {
                return (40);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the ID of the item
        /// </summary>
        public sbyte ItemID { get; set; }

        /// <summary>
        /// Gets and sets the Texture of the item
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return (Game1.tItemTextures[this.ItemID]);
            }
        }

        /// <summary>
        /// Gets and sets the Icon (Texture2D) of the item
        /// </summary>
        public Texture2D Icon
        {
            get
            {
                return (Game1.tItemIcons[this.ItemID]);
            }
        }

        /// <summary>
        /// Gets and sets whethers the item skill needs a direciton or not
        /// </summary>
        public bool? IsDirectionNeeded { get; set; }

        /// <summary>
        /// Gets and sets the mana cost of the item skill
        /// </summary>
        public int ManaCost { get; set; }

        /// <summary>
        /// Gets and sets the skill draw action of the item skill
        /// </summary>
        public Action<Player, SpriteBatch> SkillDraw { get; set; }

        /// <summary>
        /// Gets and sets the cooldown stopwatch
        /// </summary>
        private Stopwatch SkillCooldownWatch { get; set; }

        /// <summary>
        /// Gets and sets the cooldown time of the item skill
        /// </summary>
        public int CooldownTime { get; set; }

        /// <summary>
        /// Gets and sets the cost of the item
        /// </summary>
        public short Cost { get; set; }

        /// <summary>
        /// Gets and sets the name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets remaining cooldown
        /// </summary>
        public double RemainingCooldown
        {
            get
            {
                double dResult = (this.CooldownTime - SkillCooldownWatch.ElapsedMilliseconds) / 1000d;

                // If result is smaller than 0
                if(dResult < 0)
                {
                    dResult = 0;
                }

                return (Math.Round(dResult, 1));
            }
        }

        /// <summary>
        /// Gets and sets the attack range of the item [ Only relevant for weapons ]
        /// </summary>
        public int AttackRange
        {
            get;
            set;
        }

        /// <summary>
        /// Gets fill amount
        /// </summary>
        public float FillAmount
        {
            get
            {
                float fResult = 0;

                if (this.SkillCooldownWatch.ElapsedMilliseconds != 0)
                {
                    if (this.CooldownTime < this.SkillCooldownWatch.ElapsedMilliseconds)
                    {
                        fResult = 1;
                    }
                    else
                    {
                        fResult = this.SkillCooldownWatch.ElapsedMilliseconds / (float)this.CooldownTime;
                    }
                }

                return (fResult);
            }
        }

        /// <summary>
        /// Gets and sets the item description
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the Item class
        /// </summary>
        /// <param name="bItemID">Item ID</param>
        /// <param name="tTexture">Item Texture</param>
        /// <param name="tIcon">Item Icon Texture</param>
        /// <param name="bIsDirectionNeeded">Indication to whether the 
        /// item skill needs a direction or not</param>
        /// <param name="nManaCost">Mana Cost of the skill</param>
        /// <param name="nAction">SkillDraw</param>
        /// <param name="nCooldown">Cooldown time in milliseconds</param>
        public Item(sbyte bItemID,
                    bool? bIsDirectionNeeded,
                    int nManaCost,
                    Action<Player, SpriteBatch> nAction,
                    string strName,
                    int nCooldown,
                    short nCost,
                    string strDescription,
                    [Optional] int nAttackRange)
        {
            this.ItemID = bItemID;
            this.IsDirectionNeeded = bIsDirectionNeeded;
            this.ManaCost = nManaCost;
            this.SkillDraw = nAction;
            this.CooldownTime = nCooldown;
            this.Cost = nCost;
            this.Name = strName;
            this.Description = strDescription;
            this.SkillCooldownWatch = new Stopwatch();
            this.SkillCooldownWatch.Start();

            if ((object)nAttackRange == Type.Missing)
            {
                this.AttackRange = Item.DefaultAttackRange;
            }
            else
            {
                this.AttackRange = nAttackRange;
            }
        }

        /// <summary>
        /// Sets the cooldown of the item
        /// </summary>
        public void SetCooldown()
        {
            this.SkillCooldownWatch.Restart();
        }

        #endregion
    }
}
