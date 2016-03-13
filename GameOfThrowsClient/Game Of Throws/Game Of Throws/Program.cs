using System;
using System.Windows.Forms;

namespace Game_Of_Throws
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (!Game1.DEBUG_MODE && args.Length != 3)
            {
                MessageBox.Show("You must open the game from GameOfForms!", "Error starting the game");
            }
            else
            {
                using (Game1 game = new Game1(args))
                {
                    game.Run();
                }
            }
        }
    }
#endif
}

