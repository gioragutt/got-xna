using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Game_Of_Throws
{
    public abstract class Player
    {
        #region Data Members

        // Const Members      
        public    const int   PLAYER_WIDTH                 = 40;
        public    const int   PLAYER_HEIGHT                = 92;
        private   const int   HP_BAR_WIDTH                 = 80;
        private   const int   HP_BAR_HEIGHT                = 10;
        private   const int   MP_BAR_WIDTH                 = HP_BAR_WIDTH;
        private   const int   MP_BAR_HEIGHT                = HP_BAR_HEIGHT;        
        private   const int   HP_BAR_LAYOUT_ADJUSTMENT     = 15;
        private   const int   NAME_HEIGHT_ABOVE_HEALTH_BAR = 15;
        private   const int   HP_BAR_HEIGHT_ABOVE_PLAYER   = 5;
        private   const int   NUM_OF_TEXTURES              = 4;
        protected const float DECAY_AMOUNT                 = (float)0.8;

        // Data Members
        private Rectangle _rectRectangle;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the Rectangle of the player.
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return (this._rectRectangle);
            }
            set
            {
                this._rectRectangle = value;
            }
        }

        public byte LowHPTick { get; set; }

        public byte HighHPTick { get; set; }

        /// <summary>
        /// Gets and sets the position X of the player.
        /// </summary>
        public int PosX
        {
            get
            {
                return (this._rectRectangle.X);
            }
            set
            {
                this._rectRectangle.X = value;
            }
        }

        /// <summary>
        /// Gets and sets the Position Y of the player.
        /// </summary>
        public int PosY
        {
            get
            {
                return (this._rectRectangle.Y);
            }
            set
            {
                this._rectRectangle.Y = value;
            }
        }

        /// <summary>
        /// Gets and sets the name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the HP of the player.
        /// </summary>
        public short HP { get; set; }

        /// <summary>
        /// Gets and sets the Max HP of the player.
        /// </summary>
        public short MaxHP { get; set; }

        /// <summary>
        /// Gets and sets the MP of the player.
        /// </summary>
        public short MP { get; set; }

        /// <summary>
        /// Gets and sets the Max MP of the player.
        /// </summary>
        public short MaxMP { get; set; }

        /// <summary>
        /// Gets and sets the Gold of the player.
        /// </summary>
        public short Gold { get; set; }

        /// <summary>
        /// Gets and sets the Armor of the player.
        /// </summary>
        public short Armor { get; set; }

        /// <summary>
        /// Gets and sets the Magical Power of the player.
        /// </summary>
        public short MagicalPower { get; set; }

        /// <summary>
        /// Gets and sets the Physical Power of the player.
        /// </summary>
        public short PhysicalPower { get; set; }

        /// <summary>
        /// Gets and sets the Movement Speed of the player.
        /// </summary>
        public short MovementSpeed { get; set; }

        /// <summary>
        /// Gets and sets the Attack Speed of the player.
        /// </summary>
        public short AttackDelay { get; set; }

        /// <summary>
        /// Gets and Sets the Team of the player
        /// </summary>
        /// Legend:
        ///     True  : Green Team
        ///     False : Red Team
        /// ------------------------------------
        public bool IsGoodTeam { get; set; }

        /// <summary>
        /// Gets and sets the direction the player is looking at
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Returns an idication to whether the player is moving or not
        /// </summary>
        public bool IsMoving { get; set; }

        /// <summary>
        /// Returns if it's the first time you enter
        /// </summary>
        public bool IsFirstTime { get; set; }

        /// <summary>
        /// Save the indication of the moving index
        /// </summary>
        private int MoveIndex { get; set; }

        /// <summary>
        /// Gets and sets the is attacking indication of the player.
        /// </summary>
        public bool IsAttacking { get; set; }

        /// <summary>
        /// Gets and sets the amount of players the player killed
        /// </summary>
        public sbyte Kills { get; set; }

        /// <summary>
        /// Gets and sets the amount of times the player died
        /// </summary>
        public byte Deaths { get; set; }

        /// <summary>
        /// Gets or sets if the player is alive or not
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return (this.HP != 0);
            }
        }

        /// <summary>
        /// The players HP from the last Tick
        /// </summary>
        public short PervHP { get; set; }

        /// <summary>
        /// Gets and sets the inventory of the player
        /// </summary>
        public Inventory Inventory { get; set; }

        public sbyte SelectedItem { get; set; }

        public sbyte ActivatedSkill { get; set; }

        public bool IsWaitingDirection { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of Player with default values.
        /// </summary>
        private Player()
        {
            this.Name = "Test";
            this.MovementSpeed = 30;
            this.HP = 500;
            this.MaxHP = 500;
            this.MP = 200;
            this.MaxMP = 200;
            this.Gold = 0;
            this.Armor = 0;
            this.PhysicalPower = 50;
            this.MagicalPower = 0;
            this.MovementSpeed = 30;
            this.AttackDelay = 300;
            this.Direction = Direction.Down;
            this.IsAttacking = false;
            this.Kills = 0;
            this.Deaths = 0;
            this.PervHP = this.HP;
            this.LowHPTick = 0;
            this.HighHPTick = 0;
            this.Inventory = new Inventory();
            this.IsWaitingDirection = false;
            this.SelectedItem = -1;
            this.IsFirstTime = true;
            this.ActivatedSkill = -1;
        }

        /// <summary>
        /// Intiailizes a new instance of Player with the given parameters. 
        /// </summary>
        /// <param name="nPosX">The position X of the player</param>
        /// <param name="nPosY">The position Y of the player</param>
        /// <param name="strName">The name of the player</param>
        protected Player(int nPosX,
                         int nPosY,
                         string strName)
            : this()
        {
            // Initialize player rectangle
            this.Rect = new Rectangle(nPosX, nPosY, PLAYER_WIDTH, PLAYER_HEIGHT);

            // Set player name
            this.Name = strName;
        }

        #endregion

        #region Other Methods

        abstract public void Read(BinaryReader brReader);

        public abstract void Attack();

        #endregion

        #region XNA Methods

        /// <summary>
        /// Draws the player.
        /// </summary>
        /// <param name="spriteBatch">Spirtebatch to use for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Get player team index
            int nTeamIndex = Convert.ToInt32(this.IsGoodTeam);
            int nAnim;

            #region Player Draw

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);

            // If the Hp is lower than last tick
            if (this.HP < this.PervHP)
            {
                this.LowHPTick = 3;
            }
            else if ((this.HP > this.PervHP) && (!this.IsFirstTime))
            {
                this.HighHPTick = 5;
            }

            if (!this.IsAlive)
            {
                Game1.effPlayer.CurrentTechnique.Passes["Ghost"].Apply();
            }
            else if (this.LowHPTick != 0)
            {
                this.LowHPTick--;
                Game1.effPlayer.CurrentTechnique.Passes["Hit"].Apply();
            }
            else if (this.HighHPTick != 0)
            {
                this.HighHPTick--;
                Game1.effPlayer.CurrentTechnique.Passes["Healed"].Apply();
            }

            // If the player isn't moving
            if (!this.IsMoving)
            {
                nAnim = 0;
            }
            else
            {
                this.MoveIndex = (this.MoveIndex + 1) % 60;
                nAnim = this.MoveIndex / 15;
            }

            spriteBatch.Draw(Game1.tPlayerTexture[(int)this.Direction, nAnim],
                             this._rectRectangle, Color.White);

            spriteBatch.End();

            #endregion

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);

            #region Inventory Draw

            #region Weapon Draw

            //// If player is alive
            //if (this.IsAlive)
            //{
            //    spriteBatch.Draw(this.Inventory[(byte)ItemType.Weapon].Texture, GetRect(this.Inventory[(byte)ItemType.Weapon].Texture), Color.Wheat);
            //}

            #endregion

            #endregion

            #region Health Bar Draw

            // Draw only if player is alive
            if (this.IsAlive)
            {
                #region Layout

                // Draw the health bar layout
                spriteBatch.Draw(Game1.tHpBarLayout[nTeamIndex],
                                 new Rectangle(/* Health Bar Layout X Pos */
                                               this.Rect.Center.X -
                                               Game1.tHpBarLayout[nTeamIndex].Width / 2,

                                               /* Health Bar Layout Y Pos */
                                               this.Rect.Y - Game1.tHpBarLayout[nTeamIndex].Height -
                                               HP_BAR_HEIGHT_ABOVE_PLAYER,

                                               /* Health Bar Layout Width and Height */
                                               Game1.tHpBarLayout[nTeamIndex].Width,
                                               Game1.tHpBarLayout[nTeamIndex].Height),
                                 Color.White);
                #endregion

                #region Health bar

                // Draw the health bar rectangle
                spriteBatch.Draw(Game1.tHpBar[nTeamIndex],
                                 new Rectangle(/* Health Bar X Pos */
                                               this.Rect.Center.X -
                                               (HP_BAR_WIDTH / 2),

                                               /* Health Bar Y Pos */
                                               this.Rect.Y -
                                               HP_BAR_LAYOUT_ADJUSTMENT + 2 -
                                               HP_BAR_HEIGHT_ABOVE_PLAYER - MP_BAR_HEIGHT,

                                               /* Health Bar Width and Height */
                                               (int)(HP_BAR_WIDTH * ((double)this.HP / this.MaxHP)),
                                               HP_BAR_HEIGHT),
                                 Color.White);

                spriteBatch.Draw(Game1.tManaBarTexture,
                                 new Rectangle(/* Mana Bar X Pos */
                                               this.Rect.Center.X -
                                               (MP_BAR_WIDTH / 2),

                                               /* Mana Bar Y Pos */
                                               this.Rect.Y -
                                               HP_BAR_LAYOUT_ADJUSTMENT + 2 -
                                               HP_BAR_HEIGHT_ABOVE_PLAYER,

                                               /* Mana Bar Width and Height */
                                               (int)(MP_BAR_WIDTH * ((double)this.MP / this.MaxMP)),
                                               MP_BAR_HEIGHT),
                                 Color.White);

                #endregion
            }

            #endregion

            #region Name Draw

            // Draw Name Above Player
            if (this.IsAlive)
            {
                spriteBatch.DrawString(Game1.sfPlayerNameFont,
                                           this.Name,
                                           new Vector2(this.Rect.Center.X -
                                                       (Game1.sfPlayerNameFont.
                                                        MeasureString(this.Name).X / 2),
                                                       this.Rect.Y -
                                                       Game1.tHpBarLayout[nTeamIndex].Height -
                                                       HP_BAR_HEIGHT_ABOVE_PLAYER -
                                                       NAME_HEIGHT_ABOVE_HEALTH_BAR),
                                           Color.Black);
            }
            else
            {
                spriteBatch.DrawString(Game1.sfPlayerNameFont,
                                           this.Name,
                                           new Vector2(this.Rect.Center.X -
                                                       (Game1.sfPlayerNameFont.
                                                        MeasureString(this.Name).X / 2),
                                                       this.Rect.Y -
                                                       (Game1.sfPlayerNameFont.
                                                        MeasureString(this.Name).Y - 5)),
                                           Color.Black);
            }


            #endregion

            #region Attack Draw

            if (this.IsAttacking)
            {
                spriteBatch.Draw(Game1.tSwordTexture, this.GetRect(), Color.Black * 0.5f);
            }

            #endregion

            #region Skills draw

            sbyte sbTempSelectedItem = this.SelectedItem;

            if (this.ActivatedSkill != -1 && Game1.arritmItem[(byte)this.ActivatedSkill].SkillDraw != null)
            {
                // Activate skill draw of the skill
                Game1.arritmItem[(byte)this.ActivatedSkill].SkillDraw(this, spriteBatch);

                // Set activated skill to -1
                this.ActivatedSkill = -1;
            }

            #endregion

            spriteBatch.End();

            this.IsFirstTime = false;
        }

        public abstract void Update(Connection cnhHandler);

        #endregion

        private Rectangle GetRect()
        {
            // Assign default attack range
            int AttackRange = 40;
            
            if(this.Inventory[(byte)ItemType.Weapon] != null)
            {
                AttackRange = this.Inventory[(byte)ItemType.Weapon].AttackRange;
            }

            Rectangle rctAttackBox =
                new Rectangle(this.Rect.X, this.Rect.Bottom, this.Rect.Width, AttackRange);

            // Creates the attack rectangle according to the direction were looking at
            switch (this.Direction)
            {
                case (Direction.Up):
                    {
                        rctAttackBox =
                            new Rectangle(this.Rect.X, this.Rect.Y - AttackRange, this.Rect.Width, AttackRange);

                        break;
                    }
                case (Direction.Left):
                    {
                        rctAttackBox =
                            new Rectangle(this.Rect.X - AttackRange, this.Rect.Y, AttackRange, this.Rect.Height);

                        break;
                    }
                case (Direction.Right):
                    {
                        rctAttackBox =
                            new Rectangle(this.Rect.Right, this.Rect.Y, AttackRange, this.Rect.Height);

                        break;
                    }
            }

            return (rctAttackBox);
        }
    }
}
