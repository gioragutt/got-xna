using Game_Of_Throws_Server;
using GameOfThrowsServer.Player.Effects.EffectTypes;
using Microsoft.Xna.Framework;
using System.Threading;

namespace GameOfThrowsServer
{
    /// <summary>
    /// Represents a base class for all projectile type objects,
    /// Can be used as is,
    /// Can be changed by inheriting and overloading update
    /// </summary>
    public class Projectile
    {
        #region Properties

        /// <summary>
        /// Gets and sets the projectile speed (in pixels/frame)
        /// </summary>
        public int Speed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the direction of the projectile
        /// </summary>
        public Direction Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the source of the projectile
        /// </summary>
        public HandledPlayer Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the projectile effect
        /// </summary>
        public IProjectileEffect Effect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the hitbox rectangle of the projectile
        /// </summary>
        public Rectangle Hitbox;

        /// <summary>
        /// Gets and sets the max range of the projectile
        /// </summary>
        public int MaxRange
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets whether the projectile is alive or not
        /// </summary>
        public bool IsAlive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the projectile width
        /// </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the projectile height
        /// </summary>
        public int Height
        {
            get;
            set;
        }

        #endregion

        #region Ctor

        public Projectile()
        {

        }

        /// <summary>
        /// Initializes a new instance of the Projectile class
        /// </summary>
        /// <param name="hpSource">Source of the projectile</param>
        /// <param name="nSpeed">Speed of the projectile</param>
        /// <param name="dDirection">Direction of the projectile</param>
        /// <param name="rectHitbox">Hitbox of the projectile</param>
        /// <param name="ipeEffect">Effect of the projectile</param>
        public Projectile(HandledPlayer hpSource,
                          int nSpeed,
                          int nMaxRange,
                          int nWidth,
                          int nHeight,
                          Direction dDirection,
                          IProjectileEffect ipeEffect)
        {
            this.Source = hpSource;
            this.Speed = nSpeed;
            this.MaxRange = nMaxRange;
            this.Direction = dDirection;
            this.Effect = ipeEffect;
            this.IsAlive = true;
            this.Width = nWidth;
            this.Height = nHeight;
            this.InitializeProjectileLocationAndHitbox();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the location and the hitbox of the projectile
        /// </summary>
        protected virtual void InitializeProjectileLocationAndHitbox()
        {
            // Initialize hitbox based on the direction of the projectile
            switch (this.Direction)
            {
                /* Projectile below character */
                case (Direction.Down):
                    {
                        this.Hitbox = new Rectangle(this.Source.Rect.Center.X - this.Width / 2,
                                                           this.Source.Rect.Bottom,
                                                           this.Width,
                                                           this.Height);

                        break;
                    }
                /* Projectile above character */
                case (Direction.Up):
                    {
                        this.Hitbox = new Rectangle(this.Source.Rect.Center.X - this.Width / 2,
                                                    this.Source.Rect.Top - this.Height,
                                                    this.Width,
                                                    this.Height);

                        break;
                    }
                /* Projectile to the right of the character */
                case (Direction.Right):
                    {
                        this.Hitbox = new Rectangle(this.Source.Rect.Right,
                                                    this.Source.Rect.Center.Y - this.Width / 2,
                                                    this.Height,
                                                    this.Width);

                        break;
                    }
                case (Direction.Left):
                    {
                        this.Hitbox = new Rectangle(this.Source.Rect.Left - this.Height,
                                                    this.Source.Rect.Center.Y - this.Width / 2,
                                                    this.Height, this.Width);

                        break;
                    }
            }
        }

        /// <summary>
        /// Responsible for updating the projectile, changing it's state,
        /// Position, and any other factor the projectile holds.
        /// Run in a loop as long as the projectile is alive
        /// </summary>
        /// <param name="cnConnection"></param>
        public virtual void Update(ConnectionHandler cnConnection)
        {
            /// -----------------------------------
            ///      ALGORITHEM EXPLANATION
            /// -----------------------------------
            /// 1. Zerofy a vector
            /// 2. Get amount of pixels the projectile will progress this update
            /// 3. Change vector to difference in positions
            /// 4. Move projectile according to the vector
            /// 5. Kill projectile if out of range; otherwise
            /// 6. Check if projectile intersected with a player
            /// 7. Handle impact if intersection is found and update projectile state

            #region Direction Switch

            Vector2 vcMovement = Vector2.Zero;

            switch (this.Direction)
            {
                case (Direction.Up):
                {
                    vcMovement.Y = -1;
                    break;
                }
                case (Direction.Down):
                {
                    vcMovement.Y = 1;

                    break;
                }
                case (Direction.Left):
                {
                    vcMovement.X = -1;

                    break;
                }
                case (Direction.Right):
                {
                    vcMovement.X = 1;

                    break;
                }
            }

            #endregion

            #region Movement

            // Pre movement
            int nMovementAmount = this.Speed > this.MaxRange ? this.MaxRange : this.Speed;

            vcMovement *= nMovementAmount;

            // Perform movement
            this.MoveHitBox((int)vcMovement.X, (int)vcMovement.Y);

            // Post Movement
            this.PostMovement();
            
            #endregion

            #region Handle Impact

            HandledPlayer hpHit = this.CheckIntersection(cnConnection);

            if (this.IsAlive && hpHit != null && hpHit.IsAlive)
            {
                bool bIsAlive;
                this.Effect.Impact(this.Source, hpHit, cnConnection, out bIsAlive);
                this.IsAlive = bIsAlive;
            }
            
            #endregion
        }

        /// <summary>
        /// Moves the hitbox of the rectangle
        /// </summary>
        /// <param name="diffX">Difference in X axis</param>
        /// <param name="diffY">Difference in Y axis</param>
        protected virtual void MoveHitBox(int diffX, int diffY)
        {
            this.Hitbox.X += diffX;
            this.Hitbox.Y += diffY;
        }

        /// <summary>
        /// Checks intersection with all players
        /// </summary>
        /// <param name="cnConnection">Connection with server</param>
        /// <returns>Hit player if intersection occurs; otherwise false</returns>
        protected virtual HandledPlayer CheckIntersection(ConnectionHandler cnConnection)
        {
            for (int i = 0; i < cnConnection.Players.Count; i++)
            {
                HandledPlayer hpCurrent = cnConnection.Players[i];

                if (hpCurrent.Rect.Intersects(this.Hitbox))
                {
                    return (hpCurrent);
                }
            }

            return (null);
        }

        /// <summary>
        /// Manages the state of the projectile after moving
        /// </summary>
        protected void PostMovement()
        {
            this.MaxRange -= this.Speed;

            if (this.MaxRange <= 0)
            {
                this.IsAlive = false;
            }
        }

        /// <summary>
        /// Method to the thread used to manage the projectile
        /// </summary>
        /// <param name="cnt">ConnectionHandler object</param>
        public void ThreadMethod(object cnt)
        {
            while (this.IsAlive)
            {
                this.Update((ConnectionHandler)cnt);
                Thread.Sleep(1);
            }
        }

        #endregion
    }
}
