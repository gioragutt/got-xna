using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using Game_Of_Throws_Server.Player.Items.ItemTypes;
using Game_Of_Throws_Server.Player;
using GameOfThrowsServer;
using ConsoleServer.Player.Items;
using GameOfThrowsServer.Player.Effects;

namespace Game_Of_Throws_Server
{
    /// <summary>
    /// The direction a player is looking towards
    /// </summary>
    public enum Direction : byte
    {
        Up,
        Left,
        Down,
        Right
    }

    /// <summary>
    /// Represents a player handled by a connection handler.
    /// </summary>
    public class HandledPlayer
    {
        #region Data Members

        // Const members
        private const int NAME_BYTES = 10;
        private const int PLAYER_WIDTH = 40;
        private const int PLAYER_HEIGHT = 92;

        // Static members
        public readonly static short KILL_GOLD = 100;


        // Data Members
        private short _nMaxHP;
        private short _nHP;
        private short _nMaxMP;
        private short _nMP;
        private short _nGold;
        public Rectangle Rect;
        private ItemAttributeChanges _iacFinalStats;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the Client of the player.
        /// </summary>
        private TcpClient Client { get; set; }

        /// <summary>
        /// Gets and sets the is care about velocity indication
        /// </summary>
        public bool IsVelocityCare { get; set; }

        /// <summary>
        /// Gets and sets the Velocity X of the player.
        /// </summary>
        private double VelX { get; set; }

        /// <summary>
        /// Gets and sets the Velocity Y of the player.
        /// </summary>
        private double VelY { get; set; }
        
        /// <summary>
        /// Gets and sets the max hp of the player.
        /// </summary>
        public short MaxHP
        {
            get
            {
                return (this._nMaxHP);
            }
            set
            {
                // If new max hp is smaller than 0
                if (value < 0)
                {
                    this._nMaxHP = 0;
                }
                else
                {
                    // Set max hp to value
                    this._nMaxHP = value;
                }

                // If new max hp is smaller than hp
                if (this._nMaxHP < this.HP)
                {
                    this.HP = this._nMaxHP;
                }
            }
        }

        /// <summary>
        /// Gets and sets the gold a player has
        /// </summary>
        public short Gold
        {
            get
            {
                return (this._nGold);
            }
            set
            {
                if (value < 0)
                {
                    this._nGold = 0;
                }
                else
                {
                    this._nGold = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the hp of the player.
        /// </summary>
        public short HP
        {
            get
            {
                return (this._nHP);
            }
            set
            {
                // If value is bigger than 0
                if (value >= 0)
                {
                    // Check value is between 0 and MaxHP
                    if (value <= this.FinalStats[Attributes.MaxHP])
                    {
                        this._nHP = value;
                    }
                    else
                    {
                        this._nHP = this.FinalStats[Attributes.MaxHP];
                    }
                }

                // If value is less than 0
                else
                {
                    this.HP = 0;
                }
            }
        }

        /// <summary>
        /// Gets and sets the max mana points of the player
        /// </summary>
        public short MaxMP
        {
            get
            {
                return (this._nMaxMP);
            }
            set
            {
                // If new max mp is smaller than 0
                if (value < 0)
                {
                    this._nMaxMP = 0;
                }
                else
                {
                    // Set max mp to value
                    this._nMaxMP = value;
                }

                // If new max mp is smaller than mp
                if (this._nMaxMP < this.MP)
                {
                    this.MP = this._nMaxMP;
                }
            }
        }

        /// <summary>
        /// Gets and sets the current mana points of the player
        /// </summary>
        public short MP
        {
            get
            {
                return (this._nMP);
            }
            set
            {
                // If value is bigger than 0
                if (value >= 0)
                {
                    // Check value is between 0 and MaxHP
                    if (value <= this.FinalStats[Attributes.MaxMP])
                    {
                        this._nMP = value;
                    }
                    else
                    {
                        this._nMP = this.FinalStats[Attributes.MaxMP];
                    }
                }
                // If value is less than 0
                else
                {
                    this.MP = 0;
                }
            }
        }

        /// <summary>
        /// Gets and sets the player's mana regeneration
        /// </summary>
        public short MPRegen { get; set; }

        /// <summary>
        /// Gets and sets the player's health regeneration
        /// </summary>
        public short HPRegen { get; set; }

        /// <summary>
        /// Gets and sets the physical power of the player.
        /// </summary>
        public short PhysicalPower { get; set; }

        /// <summary>
        /// Gets and sets the is attacking indication.
        /// </summary>
        public bool IsAttacking { get; set; }

        /// <summary>
        /// Gets and sets the direction of the player.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets and sets the inventory of the player.
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// Gets whether the player is connected or not.
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// Gets and sets is updated indication.
        /// </summary>
        public bool IsUpdated { get; set; }

        /// <summary>
        /// Gets and sets the amount of not responding ticks.
        /// </summary>
        private int NotRespondingTicks { get; set; }

        /// <summary>
        /// Gets and sets the binary reader of the client.
        /// </summary>
        public BinaryReader PacketReader { get; set; }

        /// <summary>
        /// Gets and sets the binary writer of the client.
        /// </summary>
        public BinaryWriter PacketSender { get; set; }

        /// <summary>
        /// Gets and sets the thread of the player.
        /// </summary>
        public Thread ClientThread { get; set; }

        /// <summary>
        /// Gets and sets the connection handler of the client.
        /// </summary>
        private ConnectionHandler CurrentHandler { get; set; }

        /// <summary>
        /// Gets and sets the is moving indication of the player.
        /// </summary>
        public bool IsMoving { get; set; }

        /// <summary>
        /// Gets and sets the is good team indication.
        /// </summary>
        public bool IsGoodTeam { get; set; }

        /// <summary>
        /// Gets and sets the amount of players the player killed
        /// </summary>
        public sbyte Kills { get; set; }

        /// <summary>
        /// Gets and sets the amount of times the players died
        /// </summary>
        public byte Deaths { get; set; }

        /// <summary>
        /// Gets and sets if the player is alive or not
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return (this.HP != 0);
            }
        }

        /// <summary>
        /// Gets or sets the is team received indication
        /// </summary>
        private bool IsTeamReceived { get; set; }

        /// <summary>
        /// Gets or sets the is care about position indication
        /// </summary>
        public bool IsPositionCare { get; set; }

        /// <summary>
        /// Gets and sets the destination x pos
        /// </summary>
        public int DestPosX { get; set; }

        /// <summary>
        /// Gets and sets the destination y pos
        /// </summary>
        public int DestPosY { get; set; }

        /// <summary>
        /// Gets and sets the player's selected item
        /// </summary>
        public sbyte SelectedItem { get; set; }

        /// <summary>
        /// Gets and sets the player's magical power
        /// </summary>
        public short MagicalPower { get; set; }

        /// <summary>
        /// Gets and sets the player's armor
        /// </summary>
        public short Armor { get; set; }  

        /// <summary>
        /// Gets and sets the index of the selected item to buy from the client
        /// </summary>
        public sbyte SelectedItemToBuy { get; set; }

        /// <summary>
        /// Gets and sets the movement speed of the player.
        /// </summary>
        public byte MovementSpeed { get; set; }

        /// <summary>
        /// Gets and sets the attack delay of the player.
        /// </summary>
        public short AttackDelay { get; set; }

        /// <summary>
        /// Gets and sets the final stats of the player.
        /// </summary>
        public ItemAttributeChanges FinalStats
        {
            get
            {
                return (this._iacFinalStats);
            }
            private set
            {
                #region MaxHP Intergrity

                // If new max hp is smaller than 0
                if (value[Attributes.MaxHP] < 0)
                {
                    value[Attributes.MaxHP] = 0;
                }

                // If new max hp is smaller than hp
                if (value[Attributes.MaxHP] < this.HP)
                {
                    this.HP = value[Attributes.MaxHP];
                }
                
                #endregion

                #region MaxMP Intergrity

                // If new max mp is smaller than 0
                if (value[Attributes.MaxMP] < 0)
                {
                    value[Attributes.MaxMP] = 0;
                }

                // If new max mp is smaller than mp
                if (value[Attributes.MaxMP] < this.MP)
                {
                    this.MP = value[Attributes.MaxMP];
                }

                #endregion

                // Set final stats to value
                this._iacFinalStats = value;
            }
        }

        /// <summary>
        /// Gets and sets the attack timer of the player.
        /// </summary>
        private Stopwatch AttackTimer { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Create a new instance of handled player with a client.
        /// </summary>
        /// <param name="tccClient">Client of the player</param>
        /// <param name="cnhHandler">Client's connection handler</param>
        public HandledPlayer(TcpClient tccClient, ConnectionHandler cnhHandler)
        {
            // Initialize attack timer
            this.AttackTimer = new Stopwatch();

            // Set current handler as cnhHandler
            this.CurrentHandler = cnhHandler;

            // Set handled player client as given client
            this.Client = tccClient;

            // Set connected to true
            this.Connected = true;

            // Set is updated to false
            this.IsUpdated = false;

            // Set is team received to false
            this.IsTeamReceived = false;

            // Set is position care to false
            this.IsPositionCare = false;

            // Set is velocity care to false
            this.IsVelocityCare = false;

            // Create packet reader and sender of the client
            this.PacketReader = new BinaryReader(this.Client.GetStream());
            this.PacketSender = new BinaryWriter(this.Client.GetStream());

            #region Get Team Data

            int nHolyCowTeamCount = cnhHandler.Players.Count(e => e.IsGoodTeam);
            int nGoatTeamCount = cnhHandler.Players.Count(e => !e.IsGoodTeam);

            #endregion

            // Initialize final stats
            this.FinalStats = new ItemAttributeChanges(1000, 200, 0, 0, 0, 0, 0, 0, 0);

            #region Initialize player attributes

            // TODO: DO THIS PROPERLY
            this.Rect = new Rectangle(0, 0, PLAYER_WIDTH, PLAYER_HEIGHT);
            this.IsAttacking = false;
            this.IsMoving = false;
            this.Inventory = new Inventory();
            this.Name = "Unamed";
            this.MaxHP = 1000;
            this.HP = ((nHolyCowTeamCount > 0) && (nGoatTeamCount > 0)) ? (short)0 : this.MaxHP;
            this.MaxMP = 200;
            this.MP = this.MaxMP;
            this.HPRegen = 0;
            this.MPRegen = 1;
            this.PhysicalPower = 50;
            this.MagicalPower = 0;
            this.Armor = 0;
            this.MovementSpeed = 30;
            this.AttackDelay = 500;

            #endregion

            // Create a new thread to update current player
            this.ClientThread = new Thread(new ThreadStart(Update));

            // Set is background as is background in currenthandler
            this.ClientThread.IsBackground = this.CurrentHandler.IsBackground;

            // Start attack timer
            this.AttackTimer.Start();

            // Start client thread
            this.ClientThread.Start();
        }

        #endregion

        #region Other Methods

        #region Timeout

        public void KickPlayer()
        {
            this.Client.Close();
        }

        /// <summary>
        /// Sets the client as didn't respond in this tick.
        /// </summary>
        public void DidntRespond()
        {
            // If client didn't respond for timeout ticks
            if (this.NotRespondingTicks == ConnectionHandler.TIMEOUT_TICKS)
            {
                this.Connected = false;
                Console.WriteLine("Client timed out: " + this.Name);
            }
            else
            {
                this.NotRespondingTicks++;
            }
        }

        /// <summary>
        /// Sets the client as responded in this tick.
        /// </summary>
        public void ClientResponded()
        {
            this.NotRespondingTicks = 0;
        }

        #endregion
        
        #region Data Writing

        /// <summary>
        /// Write data to packet sender.
        /// </summary>
        /// <param name="bwWriter">Binary writer to write to</param>
        /// <param name="bwWriter">Boolean indicating wheter to write name or not</param>
        public void WriteDataToStream(BinaryWriter bwWriter)
        {
            // Write team
            bwWriter.Write(this.IsGoodTeam);

            // Write is position care
            bwWriter.Write(this.IsPositionCare);

            // If player gives a fuck about position
            if (this.IsPositionCare)
            {
                // Write destination position
                bwWriter.Write((short)this.DestPosX);
                bwWriter.Write((short)this.DestPosY); 

                // Set position to destination position
                this.Rect.X = this.DestPosX;
                this.Rect.Y = this.DestPosY;
            }
            else
            {
                // Write position
                bwWriter.Write((short)this.Rect.X);
                bwWriter.Write((short)this.Rect.Y); 
            }

            // Write is velocity care
            bwWriter.Write(this.IsVelocityCare);

            // Write velocity
            bwWriter.Write(this.VelX);
            bwWriter.Write(this.VelY);

            // Write health status
            bwWriter.Write(this.FinalStats[Attributes.MaxHP]);
            bwWriter.Write(this.HP);

            // Write mana status
            bwWriter.Write(this.FinalStats[Attributes.MaxMP]);
            bwWriter.Write(this.MP);

            // Write direction
            bwWriter.Write((byte)this.Direction);

            // Write is moving indication of the players
            bwWriter.Write(this.IsMoving);

            // Write is attacking indication
            bwWriter.Write(this.IsAttacking);

            // Send name to server
            bwWriter.Write(ASCIIEncoding.ASCII.
                                            GetBytes(this.Name.PadRight(NAME_BYTES)));

            // Send Kills to server
            bwWriter.Write(this.Kills);

            // Send Deaths
            bwWriter.Write(this.Deaths);

            // Send player gold
            bwWriter.Write(this.Gold);

            // Write inventory
            this.Inventory.WriteToStream(bwWriter);

            // Write selected item
            bwWriter.Write(this.SelectedItem);

            // Write physical power
            bwWriter.Write(this.FinalStats[Attributes.PhysicalPower]);

            // Writes player armor
            bwWriter.Write(this.FinalStats[Attributes.Armor]);

            // Write magical power
            bwWriter.Write(this.FinalStats[Attributes.MagicalPower]);

            if (this.AttackTimer.ElapsedMilliseconds < this.AttackDelay)
            {
                // Write movement speed with slow
                bwWriter.Write((short)(this.FinalStats[Attributes.MovementSpeed] * 0.78));
            }
            else
            {
                // Write movement speed
                bwWriter.Write(this.FinalStats[Attributes.MovementSpeed]);
            }
        }

        #endregion

        #region Data Reading

        private void ReadData()
        {
            try
            {
                // If is good team is null
                if (!this.IsTeamReceived)
                {
                    // Set is team received to trues
                    this.IsTeamReceived = true;

                    // Read good team indication
                    this.IsGoodTeam = this.PacketReader.ReadBoolean();
                }

                // If player doesnt give a fuck
                if (!this.IsPositionCare)
                {
                    // Read position
                    this.Rect.X = (int)this.PacketReader.ReadInt16();
                    this.Rect.Y = (int)this.PacketReader.ReadInt16();
                }
                else
                {
                    this.PacketReader.ReadBytes(4);
                }

                // Read direction of the player
                this.Direction = (Direction)this.PacketReader.ReadByte();

                // Read is moving indication
                this.IsMoving = this.PacketReader.ReadBoolean();

                // Read is attacking indication
                this.IsAttacking = this.PacketReader.ReadBoolean();

                // If player was attacking
                if (this.IsAttacking)
                {
                    // Activate attack
                    this.Attack();
                }

                // Read name
                this.Name = ASCIIEncoding.ASCII.GetString(this.PacketReader.ReadBytes(NAME_BYTES)).Trim();

                // Read selected item
                this.SelectedItem = this.PacketReader.ReadSByte();

                // Check if there's an item to be activated
                if ((this.SelectedItem != -1) &&
                    (this.Inventory[this.SelectedItem] != null))
                {
                    this.Inventory[this.SelectedItem].ActivateItem(this, this.CurrentHandler);
                }

                // Read item to buy
                this.SelectedItemToBuy = this.PacketReader.ReadSByte();

                // Check if there's an buy request
                if (this.SelectedItemToBuy != -1)
                {
                    // Get item-to-buy reference
                    Item itmToBuy = (Item)Inventory.AllItems[this.SelectedItemToBuy].GetConstructor(Type.EmptyTypes).Invoke(null);

                    // Check if you have enought gold to buy the item
                    if (this.Gold >= itmToBuy.Cost)
                    {
                        // Reduce gold
                        this.Gold -= itmToBuy.Cost;

                        // Equip item
                        this.Inventory.EquipItem(itmToBuy);
                    }
                }

                // Update final stats
                this.FinalStats = this.GetInventoryAdditions();
            }
            catch (Exception ex)
            {
                if (ConnectionHandler.DEBUG_MODE)
                {
                    throw ex;
                }
                else
                {
                    Console.WriteLine("Connection terminated with player: " + this.Name);
                }

                // Set self as disconnected
                this.Connected = false;
            }
        }

        #endregion

        /// <summary>
        /// Sets player position.
        /// </summary>
        /// <param name="nPosX">X position</param>
        /// <param name="nPosY">Y position</param>
        public void SetPosition(int nPosX, int nPosY)
        {
            // Set player position
            this.DestPosX = nPosX;
            this.DestPosY = nPosY;

            // Set is position care to true
            this.IsPositionCare = true;
        }

        /// <summary>
        /// Adds velocity to the player.
        /// </summary>
        /// <param name="nVelX">X velocity</param>
        /// <param name="nVelY">Y velocity</param>
        public void AddVelocity(int nVelX, int nVelY)
        {
            // Set player velocity
            this.VelX = nVelX;
            this.VelY = nVelY;

            // Set is velocity care to true
            this.IsVelocityCare = true;
        }

        private void Attack()
        {
            Weapon currentWeapon = this.Inventory[typeof(Weapon)] as Weapon;

            #region Melee

            if (currentWeapon == null || !currentWeapon.IsRanged)
            {
                // Fists range
                int nCurrWeaponRange = 40;

                // Checking if we have a weapon
                if (currentWeapon != null)
                {
                    // If so updating the range
                    nCurrWeaponRange = currentWeapon.AttackRange;
                }

                #region Creating Rectangle

                Rectangle rctAttackBox =
                    new Rectangle(this.Rect.X, this.Rect.Bottom, this.Rect.Width, nCurrWeaponRange);

                // Creates the attack rectangle according to the direction were looking at
                switch (this.Direction)
                {
                    case (Direction.Up):
                        {
                            rctAttackBox =
                                new Rectangle(this.Rect.X, this.Rect.Y - nCurrWeaponRange, this.Rect.Width, nCurrWeaponRange);

                            break;
                        }
                    case (Direction.Left):
                        {
                            rctAttackBox =
                                new Rectangle(this.Rect.X - nCurrWeaponRange, this.Rect.Y, nCurrWeaponRange, this.Rect.Height);

                            break;
                        }
                    case (Direction.Right):
                        {
                            rctAttackBox =
                                new Rectangle(this.Rect.Right, this.Rect.Y, nCurrWeaponRange, this.Rect.Height);

                            break;
                        }
                }

                #endregion

                // Doing physical damage in the calculated aoe
                this.CurrentHandler.AffectArea(rctAttackBox, this.FinalStats[Attributes.PhysicalPower], this);
            }
            
            #endregion

            #region Ranged

            else
            {
                Projectile prjAttack = new Projectile(this, Item_BROwAndArrow.ARROW_SPEED, currentWeapon.AttackRange, Item_BROwAndArrow.ARROW_WIDTH, Item_BROwAndArrow.ARROW_HEIGHT, this.Direction, new ProjectileEffect_BrowAndArrow());
                ParameterizedThreadStart ptsAttack = new ParameterizedThreadStart(prjAttack.ThreadMethod);
                Thread trdOfAttack = new Thread(ptsAttack);
                this.CurrentHandler.ProjectilesThreadList.Add(prjAttack, trdOfAttack);
                trdOfAttack.Start(this.CurrentHandler);
            }

            #endregion

            // Restart attack timer
            this.AttackTimer.Restart();
        }

        #region GetInventoryAdditions

        /// <summary>
        /// Gets the added amount for a player's attributes
        /// </summary>
        /// <returns>The total added attributes of the player</returns>
        public ItemAttributeChanges GetInventoryAdditions()
        {
            ItemAttributeChanges iacResult = new ItemAttributeChanges();

            #region Get Basic Stats

            iacResult[Attributes.MaxHP] = this.MaxHP;
            iacResult[Attributes.MaxMP] = this.MaxMP;
            iacResult[Attributes.HPRegen] = this.HPRegen;
            iacResult[Attributes.MPRegen] = this.MPRegen;
            iacResult[Attributes.PhysicalPower] = this.PhysicalPower;
            iacResult[Attributes.MagicalPower] = this.MagicalPower;
            iacResult[Attributes.Armor] = this.Armor;
            iacResult[Attributes.MovementSpeed] = this.MovementSpeed;
            iacResult[Attributes.AttackDelay] = this.AttackDelay;

            #endregion

            // Going through all items and checking if they have dmg to add
            foreach (Item itmCurrItem in this.Inventory.ItemDic.Values)
            {
                if (itmCurrItem != null)
                {
                    // Go over item attributes
                    for (int nCurrAttribute = (int)Attributes.MaxHP;
                         nCurrAttribute < Enum.GetNames(typeof(Attributes)).Count();
                         nCurrAttribute++)
                    {
                        // Add current attribute
                        iacResult[(Attributes)nCurrAttribute] += itmCurrItem.AttributeChanges[(Attributes)nCurrAttribute];
                    }
                }
            }

            return (iacResult);
        }

        #endregion

        #endregion

        #region Client Thread Methods

        /// <summary>
        /// Updates the player from packet reader.
        /// </summary>
        public void Update()
        {
            // Infinite loop
            while (this.Connected)
            {
                // If stream is complete
                if (this.Client.Available > 0)
                {
                    // Set is updated to true
                    this.IsUpdated = true;

                    // Mark client as responded
                    this.ClientResponded();

                    // Read player data
                    this.ReadData();

                    // Write data to player
                    this.CurrentHandler.WriteStream(this);
                }
                else
                {
                    // Mark client as didn't respond
                    this.DidntRespond();
                }

                // Sleep for sleep amount
                Thread.Sleep(ConnectionHandler.SLEEP_AMOUNT);
            }
        }

        #endregion
    }
}
