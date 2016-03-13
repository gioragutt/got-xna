using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Game_Of_Throws
{
    public class CollisionPoint
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public bool IsXCollision { get; set; }

        public void ApplyCollision(LocalPlayer plrPlayer)
        {
            plrPlayer.PosX = this.PosX - (plrPlayer.Rect.Width / 2);
            plrPlayer.PosY = this.PosY - (plrPlayer.Rect.Height / 2);

            if (IsXCollision)
            {
                plrPlayer.VelX = 0;
            }
            else
            {
                plrPlayer.VelY = 0;
            }
        }
    }

    /// <summary>
    /// Represents a local player.
    /// </summary>
    public class LocalPlayer : Player
    {        
        #region Data Members

        // Const Members
        private const double MAX_VEL = 50;

        // Data Members
        private double _dVelX;
        private double _dVelY;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the Velocity X of the player.
        /// </summary>
        public double VelX 
        { 
            get
            {
                return (this._dVelX);
            }
            set
            {
                if (Math.Abs(value) < MAX_VEL)
                {
                    this._dVelX = value;
                }
                else
                {
                    this._dVelX = Math.Sign(value) * MAX_VEL;
                }
            }
        }

        /// <summary>
        /// Gets and sets the Velocity Y of the player.
        /// </summary>
        public double VelY
        {
            get
            {
                return (this._dVelY);
            }
            set
            {
                if (Math.Abs(value) < MAX_VEL)
                {
                    this._dVelY = value;
                }
                else
                {
                    this._dVelY = Math.Sign(value) * MAX_VEL;
                }
            }
        }

        /// <summary>
        /// Gets and sets the is block to left indication.
        /// </summary>
        public bool BlockedLeft { get; private set; }

        /// <summary>
        /// Gets and sets the is block above indication.
        /// </summary>
        public bool BlockedAbove { get; private set; }

        /// <summary>
        /// Gets and sets the is block above indication.
        /// </summary>
        public bool BlockedBelow { get; private set; }

        /// <summary>
        /// Gets and sets the is block above indication.
        /// </summary>
        public bool BlockedRight { get; private set; }

        /// <summary>
        /// Gets and sets the attack timer of the player.
        /// </summary>
        public Stopwatch AttackTimer { get; set; }

        /// <summary>
        /// Gets and sets the ID of the item the player wants to buy
        /// </summary>
        public sbyte SelectedItemToBuy { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Intiailizes a new instance of LocalPlayer with the given parameters.
        /// </summary>
        /// <param name="nPosX">Position X of the player</param>
        /// <param name="nPosY">Position Y of the player</param>
        /// <param name="strName">Name of the player</param>
        public LocalPlayer(int nPosX, int nPosY, string strName, bool bIsGoodTeam) :
            base(nPosX, nPosY, strName)
        {
            // Initialize local player variables
            this.VelX = 0;
            this.VelY = 0;

            // Initialize team
            this.IsGoodTeam = bIsGoodTeam;

            // Initialize attack timer of the player
            this.AttackTimer = new Stopwatch();
            this.AttackTimer.Start();

            // Initialize selected item
            this.SelectedItemToBuy = -1;
        }

        #endregion

        #region Game Mechanics Methods

        /// <summary>
        /// Applies movement physics on player
        /// </summary>
        private void ApplyPhysics()
        {
            // Apply velocity
            this.PosY += (int)Math.Round(this.VelY);
            this.PosX += (int)Math.Round(this.VelX);

            // Apply decay
            this.VelX *= DECAY_AMOUNT;
            this.VelY *= DECAY_AMOUNT;

            #region Checking tiny velocity

            // Checking if velocity is so tiny it doesnt matter
            if (Math.Abs(this.VelX) < 0.1f)
            {
                this.VelX = 0;
            }
            if (Math.Abs(this.VelY) < 0.1f)
            {
                this.VelY = 0;
            }

            #endregion
        }

        /// <summary>
        /// Attacks infront of your hero
        /// </summary>
        public override void Attack()
        {
            // If player attack cooldown has passed
            if (this.AttackTimer.ElapsedMilliseconds > this.AttackDelay)
            {
                // Set is attacking indication to true
                this.IsAttacking = true;
                
                // Restart attack timer
                this.AttackTimer.Restart();
            }
        }

        /// <summary>
        /// Updates the character blocked indication
        /// </summary>
        protected void UpdateNearbyBlocksIndications()
        {
            // Check if next pixel to left is block
            if ((byte)Game1.gameMap.BlockMatrix[(this.PosY / Map.BLOCK_SIZE),
                                                ((this.PosX - 1) / Map.BLOCK_SIZE)] >= Game1.NON_SOLID_INDEX)
            {
                this.BlockedLeft = false;
            }
            else
            {
                this.BlockedLeft = true;
            }

            // Check if next pixel to right is block
            if ((byte)Game1.gameMap.BlockMatrix[(this.PosY / Map.BLOCK_SIZE),
                                                ((this.Rect.Right + 1) / Map.BLOCK_SIZE)] >= Game1.NON_SOLID_INDEX)
            {
                this.BlockedRight = false;
            }
            else
            {
                this.BlockedRight = true;
            }

            // Check if next pixel above is block
            if ((byte)Game1.gameMap.BlockMatrix[((this.PosY - 1) / Map.BLOCK_SIZE),
                                                (this.PosX / Map.BLOCK_SIZE)] >= Game1.NON_SOLID_INDEX)
            {
                this.BlockedAbove = false;
            }
            else
            {
                this.BlockedAbove = true;
            }

            // Check if next pixel below is block
            if ((byte)Game1.gameMap.BlockMatrix[((this.Rect.Bottom + 1) / Map.BLOCK_SIZE),
                                                (this.PosX / Map.BLOCK_SIZE)] >= Game1.NON_SOLID_INDEX)
            {
                this.BlockedBelow = false;
            }
            else
            {
                this.BlockedBelow = true;
            }
        }
        
        #endregion

        #region Server-Client Methods

        /// <summary>
        /// Reads local player data from server.
        /// </summary>
        /// <param name="brReader">Binary reader to read from</param>
        public override void Read(BinaryReader brReader)
        {
            if(this.IsAttacking)
            {
                Console.WriteLine();
            }

            // Read team
            this.IsGoodTeam = brReader.ReadBoolean();

            // If this player gives a fuck
            if (brReader.ReadBoolean())
            {
                this.PosX = brReader.ReadInt16();
                this.PosY = brReader.ReadInt16();
            }
            else
            {
                brReader.ReadBytes(4);
            }

            // If this player gives a fuck about velocity
            if (brReader.ReadBoolean())
            {
                this.VelX += brReader.ReadDouble();
                this.VelY += brReader.ReadDouble();
            }
            else
            {
                brReader.ReadBytes(16);
            }

            // Read player hp
            this.MaxHP = brReader.ReadInt16();
            this.PervHP = this.HP;
            this.HP = brReader.ReadInt16();

            // Read player mp
            this.MaxMP = brReader.ReadInt16();
            this.MP = brReader.ReadInt16();

            // Read direction indication
            brReader.ReadByte();

            // Read is moving indication
            brReader.ReadByte();

            // Read is attacking indication
            brReader.ReadByte();

            // Read name
            brReader.ReadBytes(Connection.NAME_BYTES);

            // Read kills
            this.Kills = brReader.ReadSByte();

            // Read deaths
            this.Deaths = brReader.ReadByte();

            // Read gold
            this.Gold = brReader.ReadInt16();

            // Read inventory
            this.Inventory.Read(brReader);

            // Read Selected Item
            brReader.ReadByte();

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

        #region Collision Methods

        /// <summary>
        /// Applies collision logics
        /// </summary>
        protected void ApplyCollisionLogic()
        {
            // Initialize list of collision
            List<CollisionPoint> lstcpCollisions = new List<CollisionPoint>();

            #region Vertical Axis Collision

            // Check collision UP
            if (this.VelY < 0)
            {
                CollisionPoint cpUpCollision = this.CheckCollisionUp();

                if (cpUpCollision != null)
                {
                    lstcpCollisions.Add(cpUpCollision);
                }
            }
            // Check collision DOWN
            else if (this.VelY > 0)
            {
                CollisionPoint cpDownCollision = this.CheckCollisionDown();

                if (cpDownCollision != null)
                {
                    lstcpCollisions.Add(cpDownCollision);
                }
            }
            
            #endregion

            #region Horizontal Axis Collision

            // Check collision LEFT
            if (this.VelX < 0)
            {
                CollisionPoint cpLeftCollision = this.CheckCollisionLeft();

                if (cpLeftCollision != null)
                {
                    lstcpCollisions.Add(cpLeftCollision);
                }
            }
            // Check collision RIGHT
            else if (this.VelX > 0)
            {
                CollisionPoint cpRightCollision = this.CheckCollisionRight();

                if (cpRightCollision != null)
                {
                    lstcpCollisions.Add(cpRightCollision);
                }
            }
            
            #endregion

            // If there are collision points
            if (lstcpCollisions.Count > 0)
            {
                // Get closest collision point
                lstcpCollisions.OrderBy(
                    cp => Math.Sqrt(Math.Pow(cp.PosX - this.Rect.Center.X, 2) +
                                    Math.Pow(cp.PosY - this.Rect.Center.Y, 2))).First().ApplyCollision(this);

                // If not all collision points were applied
                if (lstcpCollisions.Count > 1)
                {
                    // Apply collision again
                    this.ApplyCollisionLogic();
                }
            }
        }

        /// <summary>
        /// Checks collision above player
        /// </summary>
        /// <returns>CollisionPoint instance if collides, otherwise null</returns>
        protected CollisionPoint CheckCollisionUp()
        {
            // Variable definition
            int nBlocksAhead;
            int nBlockRowIndex;
            int nBlockColIndex;
            int nBlockRow;
            int nBlockCol;
            int nBlocksAbove = 1;
            CollisionPoint cpResult = null;

            // Code section

            // Set block ahead to check
            nBlocksAhead = (int)(this.VelY / Map.BLOCK_SIZE);

            // Check if character is below 2 blocks
            if ((this.PosX % Map.BLOCK_SIZE) > (Map.BLOCK_SIZE - this.Rect.Width))
            {
                // Increase block above by 1
                nBlocksAbove++;
            }

            // Check block ahead
            for (nBlockRowIndex = 0;
                 nBlockRowIndex <= nBlocksAhead && (cpResult == null);
                 nBlockRowIndex++)
            {
                // Set block row to check
                nBlockRow = (int)(this.PosY / Map.BLOCK_SIZE) - nBlockRowIndex;

                // Check blocks underneath
                for (nBlockColIndex = 0;
                    (nBlockColIndex < nBlocksAbove) && (cpResult == null);
                     nBlockColIndex++)
                {
                    // Set block col to check
                    nBlockCol = (int)(this.PosX / Map.BLOCK_SIZE) + nBlockColIndex;

                    // If block is solid 
                    if (((byte)Game1.gameMap.BlockMatrix[nBlockRow, nBlockCol]) < Game1.NON_SOLID_INDEX)
                    {
                        // Create and initialize collision point
                        cpResult = new CollisionPoint()
                        {
                            PosX = this.Rect.Center.X,
                            PosY = ((nBlockRow + 1) * Map.BLOCK_SIZE) +
                                   (this.Rect.Height / 2),
                            IsXCollision = false
                        };
                    }
                }
            }

            return (cpResult);
        }

        /// <summary>
        /// Checks collision below player
        /// </summary>
        /// <returns>CollisionPoint instance if collides, otherwise null</returns>
        protected CollisionPoint CheckCollisionDown()
        {
            // Variable definition
            int nBlocksAhead;
            int nBlockRowIndex;
            int nBlockColIndex;
            int nBlockRow;
            int nBlockCol;
            int nBlocksBelow = 1;
            CollisionPoint cpResult = null;

            // Code section

            // Set block ahead to check
            nBlocksAhead = (int)(this.VelY / Map.BLOCK_SIZE);

            // Check if character is below 2 blocks
            if ((this.PosX % Map.BLOCK_SIZE) > (Map.BLOCK_SIZE - this.Rect.Width))
            {
                // Increase block below by 1
                nBlocksBelow++;
            }

            // Check block ahead
            for (nBlockRowIndex = 0;
                 nBlockRowIndex <= nBlocksAhead && (cpResult == null);
                 nBlockRowIndex++)
            {
                // Set block row to check
                nBlockRow = (int)(this.Rect.Bottom / Map.BLOCK_SIZE) + nBlockRowIndex;

                // Check blocks underneath
                for (nBlockColIndex = 0;
                    (nBlockColIndex < nBlocksBelow) && (cpResult == null);
                     nBlockColIndex++)
                {
                    // Set block col to check
                    nBlockCol = (int)(this.PosX / Map.BLOCK_SIZE) + nBlockColIndex;

                    // If block is solid 
                    if (((byte)Game1.gameMap.BlockMatrix[nBlockRow, nBlockCol]) < Game1.NON_SOLID_INDEX)
                    {
                        // Create and initialize collision point
                        cpResult = new CollisionPoint()
                        {
                            PosX = this.Rect.Center.X,
                            PosY = (nBlockRow * Map.BLOCK_SIZE) -
                                   (this.Rect.Height / 2),
                            IsXCollision = false
                        };
                    }
                }
            }

            return (cpResult);
        }

        /// <summary>
        /// Checks collision left to the player
        /// </summary>
        /// <returns>CollisionPoint instance if collides, otherwise null</returns>
        protected CollisionPoint CheckCollisionLeft()
        {
            // Variable definition
            int nBlocksAhead;
            int nBlockRowIndex;
            int nBlockColIndex;
            int nBlockRow;
            int nBlockCol;
            int nBlocksToLeft = 1;
            CollisionPoint cpResult = null;

            // Code section

            // Set block ahead to check
            nBlocksAhead = (int)(this.VelX / Map.BLOCK_SIZE);

            // Check if character is below 2 blocks
            if ((this.PosY % Map.BLOCK_SIZE) > (Map.BLOCK_SIZE - this.Rect.Height))
            {
                // Increase block left by 1
                nBlocksToLeft++;
            }

            // Check block ahead
            for (nBlockColIndex = 0;
                 nBlockColIndex <= nBlocksAhead && (cpResult == null);
                 nBlockColIndex++)
            {
                // Set block col to check
                nBlockCol = (int)(this.PosX / Map.BLOCK_SIZE) - nBlockColIndex;

                // Check blocks to left
                for (nBlockRowIndex = 0;
                    (nBlockRowIndex < nBlocksToLeft) && (cpResult == null);
                     nBlockRowIndex++)
                {
                    // Set block col to check
                    nBlockRow = (int)(this.PosY / Map.BLOCK_SIZE) + nBlockRowIndex;

                    // If block is solid 
                    if (((byte)Game1.gameMap.BlockMatrix[nBlockRow, nBlockCol]) < Game1.NON_SOLID_INDEX)
                    {
                        // Create and initialize collision point
                        cpResult = new CollisionPoint()
                        {
                            PosX = ((nBlockCol + 1) * Map.BLOCK_SIZE) +
                                   (this.Rect.Width / 2),
                            PosY = this.Rect.Center.Y,
                            IsXCollision = true
                        };
                    }
                }
            }

            return (cpResult);
        }

        /// <summary>
        /// Checks collision right to the player
        /// </summary>
        /// <returns>CollisionPoint instance if collides, otherwise null</returns>
        protected CollisionPoint CheckCollisionRight()
        {
            // Variable definition
            int nBlocksAhead;
            int nBlockRowIndex;
            int nBlockColIndex;
            int nBlockRow;
            int nBlockCol;
            int nBlocksToRight = 1;
            CollisionPoint cpResult = null;

            // Code section

            // Set block ahead to check
            nBlocksAhead = (int)(this.VelX / Map.BLOCK_SIZE);

            // Check if character is below 2 blocks
            if ((this.PosY % Map.BLOCK_SIZE) > (Map.BLOCK_SIZE - this.Rect.Height))
            {
                // Increase block right by 1
                nBlocksToRight++;
            }

            // Check block ahead
            for (nBlockColIndex = 0;
                 nBlockColIndex <= nBlocksAhead && (cpResult == null);
                 nBlockColIndex++)
            {
                // Set block col to check
                nBlockCol = (int)(this.Rect.Right / Map.BLOCK_SIZE) + nBlockColIndex;

                // Check blocks to right
                for (nBlockRowIndex = 0;
                    (nBlockRowIndex < nBlocksToRight) && (cpResult == null);
                     nBlockRowIndex++)
                {
                    // Set block col to check
                    nBlockRow = (int)(this.PosY / Map.BLOCK_SIZE) + nBlockRowIndex;

                    // If block is solid 
                    if (((byte)Game1.gameMap.BlockMatrix[nBlockRow, nBlockCol]) < Game1.NON_SOLID_INDEX)
                    {
                        // Create and initialize collision point
                        cpResult = new CollisionPoint()
                        {
                            PosX = (nBlockCol * Map.BLOCK_SIZE) -
                                   (this.Rect.Width / 2),
                            PosY = this.Rect.Center.Y,
                            IsXCollision = true
                        };
                    }
                }
            }

            return (cpResult);
        }
        
        /// <summary>
        /// Applies basic collision
        /// </summary>
        protected void ApplyBasicCollision()
        {
            if ((this.VelX > 0 && this.BlockedRight) || (this.VelX < 0 && this.BlockedLeft))
            {
                this.VelX = 0;
            }

            if ((this.VelY > 0 && this.BlockedBelow) || (this.VelY < 0 && this.BlockedAbove))
            {
                this.VelY = 0;
            }
        }

        #endregion

        #region XNA Methods

        /// <summary>
        /// Updates player in the XNA Update method
        /// </summary>
        public override void Update(Connection cnhHandler)
        {
            if (this.IsAlive)
            {
                this.ApplyBasicCollision();
                this.UpdateNearbyBlocksIndications();
            }
            
            this.ApplyPhysics();

            if (this.IsAlive)
            {
                this.ApplyCollisionLogic();
            }
        }

        #endregion
    }
}
