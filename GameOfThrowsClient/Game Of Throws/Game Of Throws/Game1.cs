using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using System.Windows.Forms;


namespace Game_Of_Throws
{
    #region Enums

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
    /// Enum for the different Block types
    /// </summary>
    public enum BlockID : byte
    {
        LogFront,  // 0
        LogBack,   // 1
        LogLeft,   // 2
        LogRight,  // 3
        LogCUL,    // 4
        LogCUR,    // 5
        LogCDL,    // 6
        LogCDR,    // 7
        Bricks,    // 8 - First Non Solid Block
        BloodyBricks1,
        BloodyBricks2,
        BloodyBricks3,
        MossyBricks1,
        MossyBricks2
    }

    public enum ItemType : byte
    {
        Helmet,
        Chest,
        Weapon,
        OffHand,
        Pants,
        Gloves,
        Shoes
    }
    
    #endregion

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public const bool DEBUG_MODE = false;

        #region Data Members

        #region Const Members

        private const int BLOCK_AMOUNT = 14;
        public const int MAP_WIDTH = 15;
        public const int MAP_HEIGHT = 15;
        public const byte NON_SOLID_INDEX = 8;
        public const int ITEM_AMOUNT = 5;

        #endregion

        #region Static Members

        public static Item[] arritmItem;
        public static Texture2D[] tItemTextures;
        public static Texture2D[] tItemIcons;
        public static Texture2D[,] tPlayerTexture;
        public static Texture2D[] tHpBarLayout;
        public static Texture2D[] tHpBar;
        public static Texture2D[] tBlocks;
        public static Texture2D[] tEmeraldStaffSkill;
        public static Texture2D tSwordTexture;
        public static Texture2D tMiniScoreTab;
        public static Texture2D tScoreTab;
        public static Texture2D tManaBarTexture;
        public static Texture2D tScoreBoardTexture;
        public static Texture2D tSelectedBorder;
        public static SpriteFont sfMiniScoresTabFont;
        public static SpriteFont sfPlayerNameFont;
        public static SpriteFont sfScoresTabFont;
        public static SpriteFont sfScoresTabDetailsFont;
        public static SpriteFont sfScoreBoardFont;
        public static SpriteFont sfShopFont;
        public static SpriteFont sfShopTooltipFont;
        public static Map gameMap;
        public static Connection cntConnection;
        public static Effect effPlayer;
        public static Effect effItemIcons;
        public static Effect effRingOfGodsFury;
        public static Effect effWholeScreenEffect;
        public static bool bIsShopOpen;
        public static ProjectileHandler projectileHandler;

        #endregion

        #region Data Members

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private LocalPlayer plrLocal;
        private string[] args;
        private bool bIsScoreTabShown;

        #endregion

        #endregion

        public Game1(string[] args)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.Window.Title = "Game Of Throws";

            // Game window resolution
            this.graphics.PreferredBackBufferHeight = 1024;
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.IsFullScreen = false;

            // Various settings
            if (DEBUG_MODE)
            {
                this.IsMouseVisible = true;
            }
            else
            {
                this.IsMouseVisible = false;
            }

            bIsShopOpen = false;

            // Place arguments in args
            this.args = args;

            projectileHandler = new ProjectileHandler(this.Content);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize game map
            gameMap = new Map();

            #region Textures Array Allocation
            
            // Allocate Textures
            tEmeraldStaffSkill = new Texture2D[2];
            tHpBarLayout = new Texture2D[2];
            tHpBar = new Texture2D[2];
            tBlocks = new Texture2D[BLOCK_AMOUNT];
            tPlayerTexture = new Texture2D[4, 4];
            arritmItem = new Item[ITEM_AMOUNT];
            tItemTextures = new Texture2D[ITEM_AMOUNT];
            tItemIcons = new Texture2D[ITEM_AMOUNT];

            #endregion

            #region Player Initialization
            
            // Initialize player
            if (DEBUG_MODE)
            {
                bool bIsGoodTeam = false;

                int nX, nY;
                nX = bIsGoodTeam ? 300 : 1150;
                nY = bIsGoodTeam ? 200 : 1200;

                plrLocal = new LocalPlayer(nX, nY, "EYAL", bIsGoodTeam);
            }
            else
            {
                bool bIsGoodTeam = args[2].Equals("0") ? true : false;

                int nX, nY;
                nX = bIsGoodTeam ? 300 : 1150;
                nY = bIsGoodTeam ? 200 : 1200;

                plrLocal = new LocalPlayer(nX, nY, args[0], bIsGoodTeam);
            }

            #endregion

            #region Connection Initialization

            // Intialiaze new connection
            cntConnection = new Connection(this.plrLocal);

            try
            {
                // Connect to server in the given ip
                if (DEBUG_MODE)
                {
                    //cntConnection.Connect("188.2.210.23");
                    cntConnection.Connect("89.138.81.78");
                }
                else
                {
                    cntConnection.Connect(args[1]);
                }
            }
            catch
            {
                MessageBox.Show("Possible reasons for error:\n" +
                                "1. Server is not up\n" +
                                "2. Invalid client version\n" +
                                "Version : Bow and Arrow",
                                "Unable to connect server",
                                MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button1);
                this.Exit();
            }

            #endregion

            #region Item Initialization

            arritmItem[0] = new Item(0,
                                     true,
                                     100,
                                     SkillsDraw.DrawEmeraldStaff,
                                     "Emerald Staff Of Exiled Monks",
                                     5000,
                                     300,
                                     "1 HP Reg\n" +
                                     "2 MP Reg\n" +
                                     "20 Physical Power\n" +
                                     "40 Magical Power\n" +
                                     "20 Increased Attack Range\n" +
                                     "Awesome Lazor Beam",
                                     60);

            arritmItem[1] = new Item(1,
                                     false,
                                     200,
                                     SkillsDraw.DrawRingOfGodsFury,
                                     "Ring Of Gods Fury",
                                     10000,
                                     1000,
                                     "50 MP\n" +
                                     "5 MP Reg\n" +
                                     "100 Magical Power\n" +
                                     "OP DAMAGE ALL PPL THING");

            arritmItem[2] = new Item(2,
                                     false,
                                     0,
                                     null,
                                     "Booties",
                                     0,
                                     100,
                                     "8 Movement speed\n" +
                                     "Very cool boots");

            arritmItem[3] = new Item(3,
                                     false,
                                     100,
                                     null,
                                     "Armor of booze",
                                     10000,
                                     100,
                                     "100 HP\n" +
                                     "10 Armor\n" +
                                     "Tiny bit of AS\n" +
                                     "Coolio Healing skill");

            arritmItem[4] = new Item(4,
                                     false,
                                     0,
                                     null,
                                     "BROw And Arrow",
                                     0,
                                     200,
                                     "", 80);

            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: REMOVE THIS
            tSwordTexture = new Texture2D(GraphicsDevice, 1, 1);
            tSwordTexture.SetData(new[] { Color.Black });

            #region Texture And Effects Loading

            #region Player Textures

            // Load player texture
            // TODO : Add other texures

            tPlayerTexture[(int)Direction.Up, 0] = this.Content.Load<Texture2D>("Player/BClayAvatarB");
            tPlayerTexture[(int)Direction.Up, 1] = this.Content.Load<Texture2D>("Player/BClayAvatarA");
            tPlayerTexture[(int)Direction.Up, 2] = tPlayerTexture[(int)Direction.Up, 0];
            tPlayerTexture[(int)Direction.Up, 3] = this.Content.Load<Texture2D>("Player/BClayAvatarC");

            tPlayerTexture[(int)Direction.Left, 0] = this.Content.Load<Texture2D>("Player/LClayAvatarB");
            tPlayerTexture[(int)Direction.Left, 1] = this.Content.Load<Texture2D>("Player/LClayAvatarA");
            tPlayerTexture[(int)Direction.Left, 2] = tPlayerTexture[(int)Direction.Left, 0];
            tPlayerTexture[(int)Direction.Left, 3] = this.Content.Load<Texture2D>("Player/LClayAvatarC");

            tPlayerTexture[(int)Direction.Down, 0] = this.Content.Load<Texture2D>("Player/FClayAvatarB");
            tPlayerTexture[(int)Direction.Down, 1] = this.Content.Load<Texture2D>("Player/FClayAvatarA");
            tPlayerTexture[(int)Direction.Down, 2] = tPlayerTexture[(int)Direction.Down, 0];
            tPlayerTexture[(int)Direction.Down, 3] = this.Content.Load<Texture2D>("Player/FClayAvatarC");

            tPlayerTexture[(int)Direction.Right, 0] = this.Content.Load<Texture2D>("Player/RClayAvatarB");
            tPlayerTexture[(int)Direction.Right, 1] = this.Content.Load<Texture2D>("Player/RClayAvatarA");
            tPlayerTexture[(int)Direction.Right, 2] = tPlayerTexture[(int)Direction.Right, 0];
            tPlayerTexture[(int)Direction.Right, 3] = this.Content.Load<Texture2D>("Player/RClayAvatarC");

            #endregion

            #region Health Bar Textures

            tHpBarLayout[0] = this.Content.Load<Texture2D>("HealthBars/RedHealthBarV2");
            tHpBarLayout[1] = this.Content.Load<Texture2D>("HealthBars/GreenHealthBarV2");

            #endregion

            #region Mana Bar Textures

            tManaBarTexture = new Texture2D(GraphicsDevice, 1, 1);
            tManaBarTexture.SetData(new[] { Color.DodgerBlue });

            #endregion

            #region Block Textures

            // Load blocks textures
            tBlocks[(int)BlockID.LogFront] = this.Content.Load<Texture2D>("Blocks/Wood/LogFront");
            tBlocks[(int)BlockID.LogBack] = this.Content.Load<Texture2D>("Blocks/Wood/LogBack");
            tBlocks[(int)BlockID.LogLeft] = this.Content.Load<Texture2D>("Blocks/Wood/LogLeft");
            tBlocks[(int)BlockID.LogRight] = this.Content.Load<Texture2D>("Blocks/Wood/LogRight");
            tBlocks[(int)BlockID.LogCUL] = this.Content.Load<Texture2D>("Blocks/Wood/LogCUL");
            tBlocks[(int)BlockID.LogCUR] = this.Content.Load<Texture2D>("Blocks/Wood/LogCUR");
            tBlocks[(int)BlockID.LogCDL] = this.Content.Load<Texture2D>("Blocks/Wood/LogCDL");
            tBlocks[(int)BlockID.LogCDR] = this.Content.Load<Texture2D>("Blocks/Wood/LogCDR");
            tBlocks[(int)BlockID.Bricks] = this.Content.Load<Texture2D>("Blocks/Brick/Bricks");
            tBlocks[(int)BlockID.BloodyBricks1] = this.Content.Load<Texture2D>("Blocks/Brick/BloodyBricks1");
            tBlocks[(int)BlockID.BloodyBricks2] = this.Content.Load<Texture2D>("Blocks/Brick/BloodyBricks2");
            tBlocks[(int)BlockID.BloodyBricks3] = this.Content.Load<Texture2D>("Blocks/Brick/BloodyBricks3");
            tBlocks[(int)BlockID.MossyBricks1] = this.Content.Load<Texture2D>("Blocks/Brick/MossyBricks1");
            tBlocks[(int)BlockID.MossyBricks2] = this.Content.Load<Texture2D>("Blocks/Brick/MossyBricks2");

            #endregion

            #region Health Bar Textures

            #region Red Health Bar

            // Health bar "progress bar"
            tHpBar[0] = new Texture2D(GraphicsDevice, 1, 1);
            tHpBar[0].SetData(new[] { Color.Red });

            #endregion

            #region Green Health Bar

            // Health bar "progress bar"
            tHpBar[1] = new Texture2D(GraphicsDevice, 1, 1);
            tHpBar[1].SetData(new[] { Color.LightGreen });

            #endregion

            #endregion

            #region Items Textures

            #region Item Icons
            
            tItemIcons[0] = this.Content.Load<Texture2D>("Items/Icons/EmeraldStaffOfExiledMonksIcon");
            tItemIcons[1] = this.Content.Load<Texture2D>("Items/Icons/RingOfGodsFuryIcon");
            tItemIcons[2] = this.Content.Load<Texture2D>("Items/Icons/BootiesIcon");
            tItemIcons[3] = this.Content.Load<Texture2D>("Items/Icons/ArmorOfBoozeIcon");
            tItemIcons[4] = this.Content.Load<Texture2D>("Items/Icons/BROwAndArrow");

            #endregion

            #region Emerald Staff Of Exiled Monks Textures

            tEmeraldStaffSkill[0] = this.Content.Load<Texture2D>("Skills/Emerald Staff/LazerHorizontal");
            tEmeraldStaffSkill[1] = this.Content.Load<Texture2D>("Skills/Emerald Staff/LazerVertical");

            #endregion

            tSelectedBorder = this.Content.Load<Texture2D>("Items/SelectedBorder");

            #endregion

            #region ScoreTab Textures

            // Load floating name font
            sfPlayerNameFont = this.Content.Load<SpriteFont>("Player/FloatingNameFont");

            // Load scores tab font
            sfScoresTabFont = this.Content.Load<SpriteFont>("ScoresTab/Ping");

            // Mini Score Tab texture
            tMiniScoreTab = this.Content.Load<Texture2D>("ScoresTab/MiniScoreTab");

            // Mini Scores Tab font
            sfMiniScoresTabFont = this.Content.Load<SpriteFont>("ScoresTab/MiniScoresTabFont");

            // Score tab texture
            tScoreTab = this.Content.Load<Texture2D>("ScoresTab/ScoresTab");

            // Scores tab details font
            sfScoresTabDetailsFont = this.Content.Load<SpriteFont>("ScoresTab/ScoreTabFont");

            // Score Board texture
            tScoreBoardTexture = this.Content.Load<Texture2D>("ScoresTab/ScoreBoard");

            // Score Board font
            sfScoreBoardFont = this.Content.Load<SpriteFont>("ScoresTab/ScoreBoardFont");

            #endregion

            #region Shop Textures

            sfShopFont = this.Content.Load<SpriteFont>("Shop/ShopFont");
            sfShopTooltipFont = this.Content.Load<SpriteFont>("Shop/ShopTooltipFont");

            #endregion

            #region Effects

            effPlayer = this.Content.Load<Effect>("Effects/PlayerEffects");
            effItemIcons = this.Content.Load<Effect>("Effects/ItemIconEffects");
            effRingOfGodsFury = this.Content.Load<Effect>("Effects/RingOfGodsFury");

            #endregion

            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F11))
            {
                this.graphics.ToggleFullScreen();
            }

            // Update network players
            cntConnection.Update();

            // Update camera for local player
            Camera.UpdateCamera(this.plrLocal, this.GraphicsDevice);

            // Handle keyboard input
            KeyboardHandler.Handle(plrLocal, out bIsScoreTabShown);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

            var rtbSaver = GraphicsDevice.GetRenderTargets();

            using (RenderTarget2D rtScreen = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height))
            {
                GraphicsDevice.SetRenderTarget(rtScreen);

                #region Game World Section

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);

                // Draw game map
                gameMap.Draw(spriteBatch, this.GraphicsDevice.Viewport);

                spriteBatch.End();

                // Draw connection players
                cntConnection.DrawPlayers(spriteBatch);
                
                try
                {
                    projectileHandler.Draw(spriteBatch);
                }
                catch
                {
                
                }

                #endregion

                #region Hud Section

                #region Inventory Draw

                int nCurrItem = 0;

                // Run over player's inventory dictionary
                for (byte bCurrIndex = 0; bCurrIndex < Inventory.INVENTORY_SIZE; bCurrIndex++)
                {
                    // If the player got the item
                    if (this.plrLocal.Inventory[bCurrIndex] != null)
                    {
                        // Set fill amount parameter
                        effItemIcons.Parameters["fFillAmount"].SetValue(this.plrLocal.Inventory[bCurrIndex].FillAmount);

                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);

                        // If player has an item selected and its the current item
                        if ((plrLocal.SelectedItem != -1) &&
                            (plrLocal.SelectedItem == bCurrIndex))
                        {
                            // Draw the icon
                            spriteBatch.Draw(tSelectedBorder,
                                             new Rectangle(8 + (50 * nCurrItem),
                                                           GraphicsDevice.Viewport.Height - 72,
                                                           52,
                                                           52),
                                             Color.White);
                        }
                        else
                        {
                            effItemIcons.CurrentTechnique.Passes["CoolDown"].Apply();
                        }

                        // If mana cost is bigger than current mana
                        if (this.plrLocal.Inventory[bCurrIndex].ManaCost > plrLocal.MP)
                        {
                            effItemIcons.CurrentTechnique.Passes["NotEnoughMana"].Apply();
                        }

                        // Draw the icon
                        spriteBatch.Draw(this.plrLocal.Inventory[bCurrIndex].Icon,
                                         new Rectangle(10 + (50 * nCurrItem),
                                                       GraphicsDevice.Viewport.Height - 70,
                                                       48,
                                                       48),
                                         Color.White);

                        spriteBatch.End();

                        spriteBatch.Begin();

                        // Write the key
                        spriteBatch.DrawString(sfPlayerNameFont,
                                               bCurrIndex.ToString(),
                                               new Vector2(25 + (50 * nCurrItem),
                                                           GraphicsDevice.Viewport.Height - 20),
                                               Color.White);

                        spriteBatch.End();

                        nCurrItem++;
                    }
                }

                #endregion

                spriteBatch.Begin();

                #region Team Score

                spriteBatch.Draw(tScoreBoardTexture,
                                 new Rectangle(this.GraphicsDevice.Viewport.Width / 2 - tScoreBoardTexture.Width / 2,
                                               5,
                                               tScoreBoardTexture.Width, tScoreBoardTexture.Height),
                                 Color.White);

                spriteBatch.DrawString(sfScoreBoardFont,
                                       cntConnection.GoatScore.ToString(),
                                       new Vector2(GraphicsDevice.Viewport.Width / 2 + 20, 30),
                                       Color.Azure);

                spriteBatch.DrawString(sfScoreBoardFont,
                                       cntConnection.HolyCowsScore.ToString(),
                                       new Vector2(GraphicsDevice.Viewport.Width / 2 - 40, 30),
                                       Color.Azure);
                #endregion

                #region Mini Score Tab

                #region Layout

                spriteBatch.Draw(tMiniScoreTab,
                                 new Rectangle(this.GraphicsDevice.Viewport.Width - tMiniScoreTab.Width,
                                               0, tMiniScoreTab.Width, tMiniScoreTab.Height),
                                 Color.White);

                #endregion

                #region Information

                spriteBatch.DrawString(sfMiniScoresTabFont,
                                       "Ping : " + cntConnection.Ping +
                                       "\nGold : " + this.plrLocal.Gold,
                                       new Vector2(GraphicsDevice.Viewport.Width - 350, 5),
                                       Color.Azure);

                spriteBatch.DrawString(sfMiniScoresTabFont,
                                       "Kill      : " + this.plrLocal.Kills +
                                       "\nDeath : " + this.plrLocal.Deaths,
                                       new Vector2(GraphicsDevice.Viewport.Width - 255, 5),
                                       Color.Azure);

                spriteBatch.DrawString(sfMiniScoresTabFont,
                                       "Health : " + this.plrLocal.HP +
                                       " / " + this.plrLocal.MaxHP + "\n" +
                                       "Mana   : " + this.plrLocal.MP +
                                       " / " + this.plrLocal.MaxMP,
                                       new Vector2(GraphicsDevice.Viewport.Width - 160, 5),
                                       Color.Azure);


                #endregion

                #endregion

                #region Scores Tab

                // If bIsScoreTab is true
                if (bIsScoreTabShown)
                {
                    // Draw the ping shiiiiiiit
                    ScoresTab.Draw(spriteBatch,
                                   cntConnection.Players,
                                   ScoresTab.nPing,
                                   GraphicsDevice.Viewport.Width - ScoresTab.FRAME_WIDTH + 30,
                                   65);

                    #region DEBUG

                    if (DEBUG_MODE)
                    {
                        // Draw the velocity shiiiiiiit
                        spriteBatch.DrawString(sfPlayerNameFont,
                                               "VelX : " + Math.Round(this.plrLocal.VelX, 3) +
                                               "\nVelY : " + Math.Round(this.plrLocal.VelY, 3),
                                               new Vector2(this.GraphicsDevice.Viewport.Width / 100, 20), Color.White);
                        // Draw the velocity shiiiiiiit
                        spriteBatch.DrawString(sfPlayerNameFont,
                                               "| PosX : " + this.plrLocal.PosX +
                                               "\n| PosY : " + this.plrLocal.PosY,
                                               new Vector2(this.GraphicsDevice.Viewport.Width / 100 + 120, 20), Color.White);

                        // Show the camera position
                        spriteBatch.DrawString(sfPlayerNameFont,
                                               "| ViewportX : " + Camera.CameraVector.X +
                                               "\n| ViewportY : " + Camera.CameraVector.Y,
                                               new Vector2(this.GraphicsDevice.Viewport.Width / 100 + 250, 20), Color.White);
                    }

                    #endregion
                }

                #endregion

                #region Shop

                if (bIsShopOpen)
                {
                    ItemShop.Draw(spriteBatch, this.GraphicsDevice.Viewport);
                }

                #endregion

                #region Messages

                // Go over messages
                for (int nCurrMessage = 0;
                     nCurrMessage < cntConnection.Messages.Length;
                     nCurrMessage++)
                {
                    if (cntConnection.Messages[nCurrMessage].MessageType != ActionType.Empty)
                    {
                        spriteBatch.DrawString(sfMiniScoresTabFont,
                                               cntConnection.Players[cntConnection.Messages[nCurrMessage].ActionPerformerID].Name +
                                               cntConnection.Messages[nCurrMessage].Mess +
                                               cntConnection.Players[cntConnection.Messages[nCurrMessage].ActionRecieverID].Name,
                                               new Vector2(5, 5 + (15 * nCurrMessage)), Color.White);
                    }
                }

                #endregion

                try
                {
                    spriteBatch.End();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }

                #endregion

                if (effWholeScreenEffect != null)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effRingOfGodsFury);
                }
                else
                {
                    spriteBatch.Begin();
                }

                GraphicsDevice.SetRenderTargets(rtbSaver);

                // Draw whole screen
                spriteBatch.Draw(rtScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}