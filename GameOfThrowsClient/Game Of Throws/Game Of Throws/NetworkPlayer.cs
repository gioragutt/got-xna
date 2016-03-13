using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Game_Of_Throws
{
    /// <summary>
    /// Represents a player in the network.
    /// </summary>
    class NetworkPlayer : Player
    {
        #region Properties

        /// <summary>
        /// Gets and sets the previous X position of the network player.
        /// </summary>
        private short PreviousPosX { get; set; }

        /// <summary>
        /// Gets and sets the previous Y position of the network player.
        /// </summary>
        private short PreviousPosY { get; set; }

        /// <summary>
        /// Gets and sets the latest X position of the network player.
        /// </summary>
        private short LatestPosX { get; set; }

        /// <summary>
        /// Gets and sets the latest Y position of the network player.
        /// </summary>
        private short LatestPosY { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Intiailizes a new instance of NetworkPlayer with the given parameters.
        /// </summary>
        /// <param name="nPosX">Position X of the player</param>
        /// <param name="nPosY">Position Y of the player</param>
        /// <param name="strName">Name of the player</param>
        /// <param name="tTexture">Texture of the player</param>
        /// <param name="bIsGoodTeam">Team of the player</param>
        public NetworkPlayer(int nPosX, int nPosY, string strName) :
            base(nPosX, nPosY, strName)
        {
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Activates attack animation.
        /// </summary>
        public override void Attack()
        {
        }

        /// <summary>
        /// Updates the player from binary reader.
        /// </summary>
        /// <param name="brReader">Binary reader to update from</param>
        public override void Read(BinaryReader brReader)
        {
            // Read team
            this.IsGoodTeam = brReader.ReadBoolean();

            // Read giving a fuck indication
            brReader.ReadByte();

            // Set current position and previous position as latest position
            this.PosX = this.PreviousPosX = this.LatestPosX;
            this.PosY = this.PreviousPosY = this.LatestPosY;
            
            // Read position to latest position
            this.LatestPosX = brReader.ReadInt16();
            this.LatestPosY = brReader.ReadInt16();

            // Read giving a fuck indication and velocity
            brReader.ReadBytes(17);

            // Read hp status
            this.MaxHP = brReader.ReadInt16();
            this.PervHP = this.HP;
            this.HP = brReader.ReadInt16();

            this.MaxMP = brReader.ReadInt16();
            this.MP = brReader.ReadInt16();

            // Read direction of player
            this.Direction = (Direction)brReader.ReadByte();

            // Read is moving indication of the player
            this.IsMoving = brReader.ReadBoolean();

            // Read is attacking indication of the player
            this.IsAttacking = brReader.ReadBoolean();

            // Set name as name received from server
            this.Name = ASCIIEncoding.ASCII.GetString(brReader.ReadBytes(Connection.NAME_BYTES)).Trim();

            // Read kills
            this.Kills = brReader.ReadSByte();

            // Read deaths
            this.Deaths = brReader.ReadByte();

            // Read gold
            this.Gold = brReader.ReadInt16();

            // Read inventory
            this.Inventory.Read(brReader);

            // Read selected item
            this.SelectedItem = brReader.ReadSByte();

            // If selected item isn't -1
            if (this.SelectedItem != -1 &&
                this.Inventory[(byte)this.SelectedItem] != null)
            {
                this.ActivatedSkill = this.Inventory[(byte)this.SelectedItem].ItemID;
            }

            // Read physical power
            this.PhysicalPower = brReader.ReadInt16();

            // Read armor
            this.Armor = brReader.ReadInt16();

            // Read magical power
            this.MagicalPower = brReader.ReadInt16();

            // Read movement speed
            this.MovementSpeed = brReader.ReadInt16();
        }

        #endregion

        #region XNA Methods

        /// <summary>
        /// Predicts the network player activity using interpolation.
        /// </summary>
        /// <param name="InterpolationFactor">The factor to interpolate by</param>
        public override void Update(Connection cnhHandler)
        {
        }

        #endregion
    }
}
