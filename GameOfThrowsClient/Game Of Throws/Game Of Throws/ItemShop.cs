using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Of_Throws
{
    /// <summary>
    /// Represents the game item shop
    /// </summary>
    public static class ItemShop
    {
        #region Data Members

        private static byte _nSelectedItem;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the texture of the background of the shop
        /// </summary>
        private static Texture2D BGTexture { get; set; }

        /// <summary>
        /// Gets and sets the texture of the selected item indicator
        /// </summary>
        private static Texture2D SelectedItemTexture { get; set; }

        /// <summary>
        /// Gets and sets the font of the shop
        /// </summary>
        private static SpriteFont ShopFont { get; set; }

        /// <summary>
        /// Gets and sets the shop tooltip font
        /// </summary>
        private static SpriteFont TooltipFont { get; set; }

        /// <summary>
        /// Gets and sets the item array of the game
        /// </summary>
        private static Item[] Items { get; set; }

        /// <summary>
        /// Gets and sets the selected item in the store
        /// </summary>
        public static byte SelectedItem
        {
            get
            {
                return (_nSelectedItem);
            }
            set
            {
                if (value < 0)
                {
                    _nSelectedItem = 0;
                }
                else if (value >= Items.Length)
                {
                    _nSelectedItem = (byte)(Items.Length - 1);
                }
                else
                {
                    _nSelectedItem = value;
                }
            }
        }

        #endregion

        #region Ctor

        static ItemShop()
        {
            BGTexture = Game1.tScoreTab;
            Items = Game1.arritmItem;
            ShopFont = Game1.sfShopFont;
            TooltipFont = Game1.sfShopTooltipFont;
            SelectedItemTexture = Game1.tMiniScoreTab;
            SelectedItem = 0;
        }

        #endregion

        #region XNA Methods

        /// <summary>
        /// Draws the item shop
        /// </summary>
        /// <param name="nXpos">X position of the shop</param>
        /// <param name="nYpos">Y position of the shop</param>
        public static void Draw(SpriteBatch spriteBatch, Viewport vpViewport)
        {
            const int HEIGHT_DIFF = 60;
            int WIDTH = 550;
            int HEIGHT = 60 + HEIGHT_DIFF * Items.Length;

            int nXpos, nYpos;
            nXpos = vpViewport.Width / 2 - WIDTH / 2;
            nYpos = vpViewport.Height / 2 - HEIGHT / 2;

            #region Titles

            spriteBatch.DrawString(ShopFont,
                                   "Item name",
                                   new Vector2((float)(nXpos + 95),
                                               (float)(nYpos + 10)),
                                   Color.Ivory);

            spriteBatch.DrawString(ShopFont,
                                   "Cost",
                                   new Vector2((float)(nXpos + 465),
                                               (float)(nYpos + 10)),
                                   Color.Ivory);

            #endregion

            #region Background

            // Get background rectangle
            Rectangle rctBackground = new Rectangle(nXpos, nYpos, WIDTH, HEIGHT);

            // Draw background
            spriteBatch.Draw(BGTexture, rctBackground, Color.White);

            #endregion

            #region Item Draw

            // Go over each item
            for (int nCount = 0; nCount < Items.Length; nCount++)
            {
                // Draw the icon
                spriteBatch.Draw(Items[nCount].Icon,
                                 new Rectangle(nXpos + 25,
                                               50 + nYpos + HEIGHT_DIFF * nCount,
                                               Items[nCount].Icon.Width,
                                               Items[nCount].Icon.Height),
                                 Color.White);

                // Draw the name
                spriteBatch.DrawString(ShopFont,
                                       Items[nCount].Name,
                                       new Vector2((float)(nXpos + 95),
                                                   (float)(60 + nYpos + HEIGHT_DIFF * nCount)),
                                       Color.White);

                // Draw the cost
                spriteBatch.DrawString(ShopFont,
                                       Items[nCount].Cost.ToString(),
                                       new Vector2((float)(nXpos + 465),
                                                   (float)(60 + nYpos + HEIGHT_DIFF * nCount)),
                                       Color.White);
            }

            #endregion

            #region Tooltip

            Rectangle rctTooltip = new Rectangle(5, vpViewport.Height / 2 - 100, nXpos - 5, 300);

            spriteBatch.Draw(BGTexture, rctTooltip, Color.White);

            spriteBatch.DrawString(ShopFont,
                                   Items[SelectedItem].Name,
                                   new Vector2(rctTooltip.Center.X -
                                               (ShopFont.MeasureString(Items[SelectedItem].Name).X / 2),
                                               rctTooltip.Y + 5),
                                   Color.White);

            spriteBatch.DrawString(TooltipFont,
                                   Items[SelectedItem].Description,
                                   new Vector2(rctTooltip.X + 15,
                                               rctTooltip.Y + 30),
                                   Color.White);

            #endregion

            #region Selection Rectangle

            Rectangle rctSelection = new Rectangle(nXpos + 5,
                                                   nYpos + 45 + HEIGHT_DIFF * SelectedItem,
                                                   WIDTH - 10,
                                                   HEIGHT_DIFF);

            spriteBatch.Draw(SelectedItemTexture, rctSelection, Color.White * 0.3f);

            #endregion
        }
        
        #endregion
    }
}
