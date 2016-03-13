using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using GameOfThrowsServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game_Of_Throws_Server
{
    public class CommandDictionary
    {
        #region Data Members

        private static Dictionary<string, Action<ConnectionHandler, string[]>> dicCommands;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the action of the command string.
        /// </summary>
        /// <param name="strCommand">Command string to check</param>
        /// <returns>Tuple containing action and description of the command or null if command wasn't found</returns>
        public Action<ConnectionHandler, string[]> this[string strCommand]
        {
            get
            {
                return (dicCommands[strCommand]);
            }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes command dictionary's dictionary.
        /// </summary>
        static CommandDictionary()
        {
            // Create new dictionary
            dicCommands = new Dictionary<string, Action<ConnectionHandler, string[]>>();

            #region Add Commands

            AddCommand("showplayers", ShowPlayers);

            AddCommand("sethp", SetHP);

            AddCommand("setmp", SetMP);

            AddCommand("setgold", SetGold);

            AddCommand("tp", Teleport);

            AddCommand("push", SetVelocity);

            AddCommand("balance", BalanceTeams);

            AddCommand("restart", Restart);

            AddCommand("restartgame", RestartGame);

            AddCommand("help", ShowAllCommands);

            AddCommand("exit", Exit);

            #endregion
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Adds a command to the command dictionary.
        /// </summary>
        /// <param name="strCommand">Command identifier name</param>
        /// <param name="actMethod">Method to execute</param>
        /// <returns>True if command is added, otherwise false</returns>
        private static bool AddCommand(string strCommand, Action<ConnectionHandler, string[]> actMethod)
        {
            bool bIsAdded = false;

            // If dictionary doesn't contain strCommand
            if (!dicCommands.ContainsKey(strCommand))
            {
                // Set bIsAdded to true
                bIsAdded = true;

                // Add command to command dictionary
                dicCommands.Add(strCommand, actMethod);
            }

            return (bIsAdded);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Sets players hp according to parameters.
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">Argument 0: Amount of hp to set
        ///                    Argument 1(Optional): Player to affect </param>
        private static void SetHP(ConnectionHandler cntConnection, string[] args)
        {
            short nHpArg;

            // Get hp amount as short
            if (args.Length > 0 && short.TryParse(args[0], out nHpArg))
            {
                if(args.Length > 2)
                {
                    throw new Exception("Invalid arguments");
                }

                if (args.Length == 2)
                {
                    HandledPlayer hnpPlayer = cntConnection.GetPlayer(args[1]);

                    // If player was found
                    if (hnpPlayer != null)
                    {
                        // Set player hp
                        hnpPlayer.HP = nHpArg;
                        Console.WriteLine("Set " + args[1] + " hp to " + args[0]);
                    }
                    else
                    {
                        throw new Exception("Player not found");
                    }
                }
                if (args.Length == 1)
                {
                    // Set players hp
                    cntConnection.Players.ForEach((plr) => plr.HP = nHpArg);

                    Console.WriteLine("Set all players hp to " + args[0]);
                }
            }
            else
            {
                throw new Exception("Invalid arguments");
            }
        }

        /// <summary>
        /// Sets players hp according to parameters.
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">Argument 0: Amount of hp to set
        ///                    Argument 1(Optional): Player to affect </param>
        private static void SetMP(ConnectionHandler cntConnection, string[] args)
        {
            short nMpArg;

            // Get hp amount as short
            if (args.Length > 0 && short.TryParse(args[0], out nMpArg))
            {
                if (args.Length > 2)
                {
                    throw new Exception("Invalid arguments");
                }

                if (args.Length == 2)
                {
                    HandledPlayer hnpPlayer = cntConnection.GetPlayer(args[1]);

                    // If player was found
                    if (hnpPlayer != null)
                    {
                        // Set player hp
                        hnpPlayer.MP = nMpArg;
                        Console.WriteLine("Set " + args[1] + " mp to " + args[0]);
                    }
                    else
                    {
                        throw new Exception("Player not found");
                    }
                }
                if (args.Length == 1)
                {
                    // Set players hp
                    cntConnection.Players.ForEach((plr) => plr.MP = nMpArg);

                    Console.WriteLine("Set all players mp to " + args[0]);
                }
            }
            else
            {
                throw new Exception("Invalid arguments");
            }
        }

        /// <summary>
        /// Sets players gold according to parameters.
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">Argument 0: Amount of hp to set
        ///                    Argument 1(Optional): Player to affect </param>
        private static void SetGold(ConnectionHandler cntConnection, string[] args)
        {
            short nGoldArg;

            // Get hp amount as short
            if (args.Length > 0 && short.TryParse(args[0], out nGoldArg))
            {
                if (args.Length > 2)
                {
                    throw new Exception("Invalid arguments");
                }

                if (args.Length == 2)
                {
                    HandledPlayer hnpPlayer = cntConnection.GetPlayer(args[1]);

                    // If player was found
                    if (hnpPlayer != null)
                    {
                        // Set player hp
                        hnpPlayer.Gold = nGoldArg;
                        Console.WriteLine("Set " + args[1] + " gold to " + args[0]);
                    }
                    else
                    {
                        throw new Exception("Player not found");
                    }
                }
                if (args.Length == 1)
                {
                    // Set players hp
                    cntConnection.Players.ForEach((plr) => plr.Gold = nGoldArg);

                    Console.WriteLine("Set all players gold to " + args[0]);
                }
            }
            else
            {
                throw new Exception("Invalid arguments");
            }
        }

        /// <summary>
        /// Restarts the game
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">No arguments needed</param>
        private static void Restart(ConnectionHandler cntConnection, string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    if (args.Length == 1)
                    {
                        int nCheckInt;

                        if (int.TryParse(args[0], out nCheckInt))
                        {
                            Console.WriteLine("Restart in " + args[0] + " milliseconds");
                        }

                        #region Wait

                        Stopwatch stp = new Stopwatch();
                        stp.Start();

                        while (stp.ElapsedMilliseconds < int.Parse(args[0]))
                        {
                            Thread.Sleep(5);
                        };

                        #endregion
                    }

                    Random rnd = new Random();

                    for (int i = 0; i < cntConnection.Players.Count; i++)
                    {
                        HandledPlayer hpPlayer = cntConnection.Players[i];
                        hpPlayer.HP = hpPlayer.FinalStats[Attributes.MaxHP];
                        hpPlayer.MP = hpPlayer.FinalStats[Attributes.MaxMP];
                        hpPlayer.SetPosition(hpPlayer.IsGoodTeam ? rnd.Next(250, 350) : rnd.Next(1100, 1200),
                                             hpPlayer.IsGoodTeam ? rnd.Next(200, 300) : rnd.Next(1175, 1225));
                    }

                    for (int i = 0;
                         i < cntConnection.ProjectilesThreadList.Keys.ToList<Projectile>().Count;
                         i++)
                    {
                        Projectile prjCurr =
                            cntConnection.ProjectilesThreadList.Keys.ToList<Projectile>()[i];
                        if (!prjCurr.IsAlive)
                        {
                            cntConnection.ProjectilesThreadList.Remove(prjCurr);
                        }
                    }

                    ConnectionHandler.IsRoundOver = false;

                    Console.WriteLine("Game restarted");
                }
                else
                {
                    throw new Exception("Invalid arguments");
                }
            }
            catch (Exception)
            {
                throw new Exception("Invalid arguments");
            }
        }

        /// <summary>
        /// Restarts the game and sets the game scores to 0
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">No arguments needed</param>
        private static void RestartGame(ConnectionHandler cntConnection, string[] args)
        {
            // Restart game
            Restart(cntConnection, args);

            // Initialize team scores
            cntConnection.GoatScore = 0;
            cntConnection.HolyCowScore = 0;

            // Initialize player scores
            for (int i = 0; i < cntConnection.Players.Count; i++)
            {
                cntConnection.Players[i].Kills = 0;
                cntConnection.Players[i].Deaths = 0;
                cntConnection.Players[i].Inventory.EmptyInventory();
                cntConnection.Players[i].Gold = 0;
            }
        }

        /// <summary>
        /// Shows the player state in the game
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">No arguments needed</param>
        private static void ShowPlayers(ConnectionHandler cntConnection, string[] args)
        {
            if (args.Length == 0)
            {
                // If no players are in the server
                if (cntConnection.Players.Count == 0)
                {
                    Console.WriteLine("No players in server");
                }
                else
                {
                    // Show how many players are currently connected to the server
                    Console.WriteLine("\n----" +
                                      "Players in server: " +
                                      cntConnection.Players.Count +
                                      " -----\n" +
                                      "HOLYCOW -> \t[ " +
                                      cntConnection.GoatScore +
                                      " : " +
                                      cntConnection.HolyCowScore +
                                      " ]\t<- GOAT\n");
                    Console.WriteLine("#\tName\t\tTeam\tKills\tDeaths\tGold\n");

                    // Go over each player in the server
                    for (int i = 0; i < cntConnection.Players.Count; i++)
                    {
                        Console.WriteLine("{0}\t{1}\t\t{2}\t{3}\t  {4}\t {5}",
                                          i + 1,
                                          cntConnection.Players[i].Name,
                                          cntConnection.Players[i].IsGoodTeam
                                          ? "HolyCow" : "Goat",
                                          cntConnection.Players[i].Kills,
                                          cntConnection.Players[i].Deaths,
                                          cntConnection.Players[i].Gold);
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                throw new Exception("Invalid arguments");
            }
        }

        /// <summary>
        /// Printing all commands to the user
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">No arguments needed</param>
        private static void ShowAllCommands(ConnectionHandler cntConnection, string[] args)
        {
            // Printing title
            Console.WriteLine("--- Commands: ---");

            // Going through all the commands in the dictionary and printing them
            foreach (string strCurrCommandName in dicCommands.Keys)
            {
                Console.WriteLine(" - " + strCurrCommandName);
            }

            // Extra row just to make it look good
            Console.WriteLine();
        }

        /// <summary>
        /// Exiting the server console
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">No arguments needed</param>
        private static void Exit(ConnectionHandler cntConnection, string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    if (args.Length == 1)
                    {
                        int nCheckInt;

                        if (int.TryParse(args[0], out nCheckInt))
                        {
                            Console.WriteLine("Server killed in " + args[0] + " milliseconds");
                        }

                        #region Wait

                        Stopwatch stp = new Stopwatch();
                        stp.Start();
                        while (stp.ElapsedMilliseconds < int.Parse(args[0]))
                            ;

                        #endregion
                    }

                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    throw new Exception("Invalid arguments");
                }
            }
            catch
            {
                throw new Exception("Invalid arguments");
            }

        }

        /// <summary>
        /// Teleports player to destination
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">Location to teleport to</param>
        private static void Teleport(ConnectionHandler cntConnection, string[] args)
        {
            int nPosX;
            int nPosY;
            HandledPlayer plrPlayer = cntConnection.GetPlayer(args[0]);

            // If player exists
            if (plrPlayer != null)
            {
                if (args.Length == 2)
                {
                    HandledPlayer plrOther = cntConnection.GetPlayer(args[1]);

                    if (plrOther != null)
                    {
                        plrPlayer.SetPosition(plrOther.Rect.X, plrOther.Rect.Y);

                        Console.WriteLine("Set player position to (" + plrOther.Rect.X + ", " + plrOther.Rect.Y + ')');
                    }
                    else
                    {
                        throw new Exception("Destination player doesn't exist");
                    }
                }
                else if (args.Length == 3 &&
                         int.TryParse(args[1], out nPosX) &&
                         int.TryParse(args[2], out nPosY))
                {
                    plrPlayer.SetPosition(nPosX, nPosY);

                    Console.WriteLine("Set player position to (" + nPosX + ", " + nPosY + ')');
                }
                else
                {
                    throw new Exception("Invalid arguments");
                }
            }
            else
            {
                throw new Exception("Player doesn't exist");
            }
        }

        /// <summary>
        /// Sets velocity
        /// </summary>
        /// <param name="cntConnection">Connection to execute command on</param>
        /// <param name="args">Velocity to set (X,Y)</param>
        private static void SetVelocity(ConnectionHandler cntConnection, string[] args)
        {
            HandledPlayer plrPlayer;

            if (args.Length > 0)
            {
                plrPlayer = cntConnection.GetPlayer(args[0]);
            }
            else
            {
                throw new Exception("Invalid arguments");
            }

            int nVelX;
            int nVelY;

            // If player found
            if (plrPlayer != null)
            {
                // If argument amount is valid
                if ((args.Length == 3) &&
                    int.TryParse(args[1], out nVelX) &&
                    int.TryParse(args[2], out nVelY))
                {
                    // Add velocity to player
                    plrPlayer.AddVelocity(nVelX, nVelY);

                    // Write sucess message
                    Console.WriteLine("Player pushed (" + nVelX + ", " + nVelY + ')');
                }
                else
                {
                    throw new Exception("Invalid arguments");
                }
            }
            else
            {
                throw new Exception("Player doesn't exist");
            }
        }

        /// <summary>
        /// Balances the team (KD & Amount of people taken in consideration)
        /// </summary>
        /// <param name="cntConnection">Connection</param>
        /// <param name="args">Arguments</param>
        private static void BalanceTeams(ConnectionHandler cntConnection, string[] args)
        {
            if (args.Length != 0)
            {
                throw new Exception("Invalid arguments");
            }
            else
            {
                // Get list of players
                List<HandledPlayer> lstPlayers = cntConnection.Players;

                // Draft players to each team
                for (int i = 0; i < lstPlayers.Count; i++)
                {
                    lstPlayers[i].IsGoodTeam = (i % 2 == 0);
                }

                Console.WriteLine("Teams balanced!");
            }
        }

        #endregion
    }
}
