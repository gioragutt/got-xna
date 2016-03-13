using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Game_Of_Throws
{
    static class KeyboardHandler
    {
        #region Data Members

        // Const Members
        private const double SPEED_FACTOR = 25;

        // Data Members
        private static KeyboardState kbsPrev;
        
        #endregion

        #region Other Methods

        /// <summary>
        /// Handles keyboard input of a player.
        /// </summary>
        /// <param name="plrPlayer">Player to handle keyboard input</param>
        public static void Handle(LocalPlayer plrPlayer, out bool bIsScoresShown)
        {
            // Save keyboard state into kbsState
            KeyboardState kbsState = Keyboard.GetState();

            // Initialize output variables
            bIsScoresShown = false;
            plrPlayer.IsMoving = false;

            #region Handle Keys

            #region Movement Keys

            // Left movement
            if (kbsState.IsKeyDown(Keys.A))
            {
                plrPlayer.VelX -= (plrPlayer.MovementSpeed / SPEED_FACTOR);
                plrPlayer.Direction = Direction.Left;
                plrPlayer.IsMoving = true;
            }

            // Right movement
            if (kbsState.IsKeyDown(Keys.D))
            {
                plrPlayer.VelX += (plrPlayer.MovementSpeed / SPEED_FACTOR);
                plrPlayer.Direction = Direction.Right;
                plrPlayer.IsMoving = true;
            }

            // Up movement
            if (kbsState.IsKeyDown(Keys.W))
            {
                plrPlayer.VelY -= (plrPlayer.MovementSpeed / SPEED_FACTOR);
                plrPlayer.Direction = Direction.Up;
                plrPlayer.IsMoving = true;
            }

            // Down movement
            if (kbsState.IsKeyDown(Keys.S))
            {
                plrPlayer.VelY += (plrPlayer.MovementSpeed / SPEED_FACTOR);
                plrPlayer.Direction = Direction.Down;
                plrPlayer.IsMoving = true;
            }

            #endregion

            #region Skill Keys

            if (plrPlayer.IsAlive)
            {
                // If no item is selected
                if (plrPlayer.SelectedItem == -1)
                {
                    // Go over inventory
                    for (byte nCurrItem = 0; nCurrItem < Inventory.INVENTORY_SIZE; nCurrItem++)
                    {
                        // If item isn't null and cooldown is off and player pressed key and player has enough mana
                        if (kbsState.IsKeyDown(Keys.NumPad0 + nCurrItem) &&
                            kbsPrev.IsKeyUp(Keys.NumPad0 + nCurrItem) &&
                            (plrPlayer.Inventory[nCurrItem] != null) &&
                            (plrPlayer.Inventory[nCurrItem].RemainingCooldown == 0) &&
                            (plrPlayer.MP >= plrPlayer.Inventory[nCurrItem].ManaCost))
                        {
                            plrPlayer.SelectedItem = (sbyte)nCurrItem;
                            plrPlayer.IsWaitingDirection = plrPlayer.Inventory[nCurrItem].IsDirectionNeeded.Value;
                        }
                    }
                }
                else if (kbsState.IsKeyDown(Keys.NumPad0 + plrPlayer.SelectedItem) &&
                         kbsPrev.IsKeyUp(Keys.NumPad0 + plrPlayer.SelectedItem))
                {
                    // Deselect item
                    plrPlayer.SelectedItem = -1;
                    plrPlayer.IsWaitingDirection = false;
                }
            }

            #endregion

            #region Attack Keys

            if (!Game1.bIsShopOpen)
            {
                #region Character Attacking

                if (plrPlayer.IsAlive)
                {
                    if (kbsState.IsKeyDown(Keys.Left))
                    {
                        plrPlayer.Direction = Direction.Left;

                        if (plrPlayer.IsWaitingDirection)
                        {
                            plrPlayer.IsWaitingDirection = false;
                        }
                        else
                        {
                            plrPlayer.Attack();
                        }
                    }

                    if (kbsState.IsKeyDown(Keys.Right))
                    {
                        plrPlayer.Direction = Direction.Right;

                        if (plrPlayer.IsWaitingDirection)
                        {
                            plrPlayer.IsWaitingDirection = false;
                        }
                        else
                        {
                            plrPlayer.Attack();
                        }
                    }

                    if (kbsState.IsKeyDown(Keys.Up))
                    {
                        plrPlayer.Direction = Direction.Up;

                        if (plrPlayer.IsWaitingDirection)
                        {
                            plrPlayer.IsWaitingDirection = false;
                        }
                        else
                        {
                            plrPlayer.Attack();
                        }
                    }

                    if (kbsState.IsKeyDown(Keys.Down))
                    {
                        plrPlayer.Direction = Direction.Down;

                        if (plrPlayer.IsWaitingDirection)
                        {
                            plrPlayer.IsWaitingDirection = false;
                        }
                        else
                        {
                            plrPlayer.Attack();
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region Character InShop

                if ((kbsState.IsKeyDown(Keys.Down)) && (!kbsPrev.IsKeyDown(Keys.Down)))
                {
                    ItemShop.SelectedItem++;
                }
                else if ((kbsState.IsKeyDown(Keys.Up)) && (!kbsPrev.IsKeyDown(Keys.Up)))
                {
                    ItemShop.SelectedItem--;
                }

                // If player pressed enter
                if (kbsState.IsKeyDown(Keys.Enter) && kbsPrev.IsKeyUp(Keys.Enter))
                {
                    // Select item
                    plrPlayer.SelectedItemToBuy = (sbyte)ItemShop.SelectedItem;
                }

                #endregion
            }

            #endregion

            #region Toggle Keys

            // Shows ScoresTab
            if (kbsState.IsKeyDown(Keys.Tab))
            {
                bIsScoresShown = true;
            }

            // Toggle the shop menu
            if (kbsState.IsKeyDown(Keys.B) && !kbsPrev.IsKeyDown(Keys.B))
            {
                Game1.bIsShopOpen = !Game1.bIsShopOpen;
            }
            
            #endregion

            // Place kbsState in kbsPrev
            kbsPrev = kbsState;

            #endregion
        }
        
        #endregion
    }
}
