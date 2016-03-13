using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Of_Throws_Server.Player.Effects.EffectTypes
{
    public enum Attributes : byte
    {
        MaxHP = 0,
        MaxMP,
        HPRegen,
        MPRegen,
        PhysicalPower,
        MagicalPower,
        Armor,
        MovementSpeed,
        AttackDelay
    }

    public class ItemAttributeChanges
    {
        #region Properties

        /// <summary>
        /// Represents the attribute changes that apply on the 
        /// player who has an item with an attribute change
        /// </summary>
        private Dictionary<Attributes, short> AttributeChanges { get; set; }

        public short this[Attributes attIndexer]
        {
            get
            {
                return (this.AttributeChanges[attIndexer]);
            }
            set
            {
                this.AttributeChanges[attIndexer] = value;
            }
        }

        #endregion

        #region Ctors

        /// <summary>
        /// Default ctor - sets attribute changes size to number
        /// of attributes a player has and initializes every attribute change to 0
        /// </summary>
        public ItemAttributeChanges()
        {
            this.AttributeChanges = new Dictionary<Attributes,short>();

            this.AttributeChanges.Add(Attributes.MaxHP, 0);
            this.AttributeChanges.Add(Attributes.MaxMP, 0);
            this.AttributeChanges.Add(Attributes.HPRegen, 0);
            this.AttributeChanges.Add(Attributes.MPRegen, 0);
            this.AttributeChanges.Add(Attributes.PhysicalPower, 0);
            this.AttributeChanges.Add(Attributes.MagicalPower, 0);
            this.AttributeChanges.Add(Attributes.Armor, 0);
            this.AttributeChanges.Add(Attributes.MovementSpeed, 0);
            this.AttributeChanges.Add(Attributes.AttackDelay, 0);
        }

        /// <summary>
        /// Creates the attribute of the items by what you send
        /// ---------------
        /// HP
        /// MP
        /// HP Regen
        /// MP Regen
        /// Physical Power
        /// Magical Power
        /// Armor
        /// Magical Defence
        /// Movement Speed
        /// Attack Speed
        /// ---------------
        /// </summary>
        /// <param name="HP">HP</param>
        /// <param name="MP">MP</param>
        /// <param name="HPRegen">HP Regen</param>
        /// <param name="MPRegen">MP Regen</param>
        /// <param name="PhysicalPower">Physical Power</param>
        /// <param name="MagicalPower">Magical Power</param>
        /// <param name="Armor">Armor</param>
        /// <param name="MagicalDef">Magical defence</param>
        /// <param name="MovementSpeed">Movement speed</param>
        /// <param name="AttackSpeed">Attack speed</param>
        public ItemAttributeChanges(short MaxHP,
                                    short MaxMP,
                                    short HPRegen,
                                    short MPRegen,
                                    short PhysicalPower,
                                    short MagicalPower,
                                    short Armor,
                                    short MovementSpeed,
                                    short AttackDelay) 
            : this()
        {
            this.AttributeChanges[Attributes.MaxHP] = MaxHP;
            this.AttributeChanges[Attributes.MaxMP] = MaxMP;
            this.AttributeChanges[Attributes.HPRegen] = HPRegen;
            this.AttributeChanges[Attributes.MPRegen] = MPRegen;
            this.AttributeChanges[Attributes.PhysicalPower] = PhysicalPower;
            this.AttributeChanges[Attributes.MagicalPower] = MagicalPower;
            this.AttributeChanges[Attributes.Armor] = Armor;
            this.AttributeChanges[Attributes.MovementSpeed] = MovementSpeed;
            this.AttributeChanges[Attributes.AttackDelay] = AttackDelay;
        }

        #endregion
    }
}
