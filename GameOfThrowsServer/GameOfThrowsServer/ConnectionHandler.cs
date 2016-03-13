using Game_Of_Throws_Server.Player.Effects.EffectTypes;
using GameOfThrowsServer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace Game_Of_Throws_Server
{
    /// <summary>
    /// Enum that describes possible outcomes of a player dying
    /// </summary>
    public enum ActionType : byte
    {
        Empty,
        Kill,
        Suicide
    }

    /// <summary>
    /// Represents a message to send to the user
    /// </summary>
    public class Message
    {
        public byte       MessagePattern;
        public byte       ActionPerformerID;
        public ActionType MessageType;
        public sbyte      ActionRecieverID;
        public DateTime   TimePerformed;
    }

    /// <summary>
    /// Class reponsible for handling connections from players
    /// And their actions
    /// </summary>
    public class ConnectionHandler
    {
        #region Data Members
        
        // Const Members
        public const bool DEBUG_MODE = false;
        public const int SLEEP_AMOUNT = 10;
        public const int TIMEOUT_TICKS = 500;
        public const int GOLD_GAIN_RATE = 100;
        public const int REGEN_RATE = 50;
        public const int MESSAGES_COUNT = 5;
        public const int MESSAGES_DURATION = 5;
        public const int MESSAGE_PATTERNS = 9;
        public const int MAX_TIME_PER_ROUND = 150000;

        // Static Members
        public static bool IsRoundOver { get; set; }
        public static Random rnd = new Random();
        private static object LockObject = new object();

        // Data Members
        private bool _bIsBackground;
        public byte GoatScore;
        public byte HolyCowScore;
        public Dictionary<Projectile, Thread> ProjectilesThreadList = new Dictionary<Projectile, Thread>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the timer
        /// </summary>
        public Stopwatch Timer { get; set; }

        /// <summary>
        /// Gets and sets the player list.
        /// </summary>
        public List<HandledPlayer> Players { get; set; }

        /// <summary>
        /// Gets and sets the listening thread.
        /// </summary>
        private Thread WaitConnectionThread { get; set; }

        /// <summary>
        /// Gets and sets the disconnection thread.
        /// </summary>
        private Thread DisconnectionThread { get; set; }

        /// <summary>
        /// Gets and sets the command thread.
        /// </summary>
        private Thread CommandThread { get; set; }

        /// <summary>
        /// Gets and sets the Listener of the server.
        /// </summary>
        private TcpListener Listener { get; set; }

        /// <summary>
        /// Indicates whether the server is waiting for new players or not.
        /// </summary>
        public bool Listening { get; set; }

        /// <summary>
        /// Gets or sets whether the server is background or not.
        /// </summary>
        public bool IsBackground
        {
            get
            {
                return (this._bIsBackground);
            }
            set
            {
                this._bIsBackground = value;

                // If wait connection thread isn't null
                if (this.WaitConnectionThread != null)
                {
                    // Set isbackground to value
                    this.WaitConnectionThread.IsBackground = value;
                }

                // Set players thread to value
                for (int nCurrPlayer = 0; nCurrPlayer < this.Players.Count; nCurrPlayer++)
                {
                    this.Players[nCurrPlayer].ClientThread.IsBackground = value;
                }
            }
        }

        public CommandDictionary CommandRunner { get; set; }

        /// <summary>
        /// List of messages to send to all users
        /// </summary>
        public List<Message> ActionMessages { get; set; }

        /// <summary>
        /// Gets and sets the server tick of the server.
        /// </summary>
        public int ServerTick { get; set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Start a new connection handler.
        /// </summary>
        public ConnectionHandler()
        {
            // Initialize player list
            this.Players = new List<HandledPlayer>();

            // Initialize command dictionary
            CommandRunner = new CommandDictionary();

            // Messages to send
            ActionMessages = new List<Message>(MESSAGES_COUNT);

            // Set IsBackground to false
            this.IsBackground = false;

            // Intialize bIsWaiting status
            this.Listening = false;

            // Initialize scores
            this.GoatScore = 0;
            this.HolyCowScore = 0;

            // Initialize disconnection thread
            this.DisconnectionThread = new Thread(DisconnectPlayers);

            // Set is background to connection is background
            this.DisconnectionThread.IsBackground = this.IsBackground;

            // Start disconnection thread
            this.DisconnectionThread.Start();

            // Initialize command thread
            this.CommandThread = new Thread(ReadCommands);

            // Start command thread
            this.CommandThread.Start();

            // Reset timer
            this.Timer = new Stopwatch();
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Allows users to connect to the server.
        /// </summary>
        public void Listen()
        {
            // If server isn't waiting for connection
            if (!this.Listening)
            {
                // Intialize listener
                this.Listener = new TcpListener(IPAddress.Any, 27015);

                // Place socket in listening state
                this.Listener.Start();

                // Start waiting connection function on the thread
                this.WaitConnectionThread = new Thread(this.WaitConnection);

                // Set thread as background
                this.WaitConnectionThread.IsBackground = this.IsBackground;

                // Set listening to true
                this.Listening = true;

                // Start the thread
                this.WaitConnectionThread.Start();
            }
            else
            {
                // Throw thread already alive exception
                Exception ex =
                    new Exception("ERROR: Tried to listen for new connections" +
                                  "when already listening");

                throw ex;
            }
        }

        /// <summary>
        /// Stops anymore users from connecting the server.
        /// </summary>
        public void StopListening()
        {
            // If server is listening for connections
            if (this.Listening)
            {
                // Set listening to false
                this.Listening = false;

                // Stop the thread
                this.WaitConnectionThread.Abort();

                // Close the listener
                this.Listener.Stop();
            }
            else
            {
                // Throw thread already dead exception
                throw new Exception("ERROR: Tried to stop listening " +
                                    "when server wasn't listening");
            }
        }

        /// <summary>
        /// Write server data snapshot to player.
        /// </summary>
        /// <param name="plrPlayer"></param>
        public void WriteStream(HandledPlayer plrPlayer)
        {
            MemoryStream msCurrStream = new MemoryStream();
            BinaryWriter brWriter = new BinaryWriter(msCurrStream);
            try
            {
                // Write players id
                brWriter.Write((byte)this.Players.FindIndex(e => e.GetHashCode() ==
                                                                 plrPlayer.GetHashCode()));

                // Write players amount
                brWriter.Write((byte)this.Players.Count);

                // Write GOAT team score
                brWriter.Write(GoatScore);

                // Write HOLYCOW team score
                brWriter.Write(HolyCowScore);

                // Write messsages
                for (int nCurrIndex = 0; nCurrIndex < MESSAGES_COUNT; nCurrIndex++)
                {
                    if (nCurrIndex >= this.ActionMessages.Count)
                    {
                        brWriter.Write((byte)0);
                        brWriter.Write((byte)0);
                        brWriter.Write((sbyte)0);
                        brWriter.Write((byte)0);
                    }
                    else
                    {
                        brWriter.Write(this.ActionMessages[nCurrIndex].MessagePattern);
                        brWriter.Write(this.ActionMessages[nCurrIndex].ActionPerformerID);
                        brWriter.Write(this.ActionMessages[nCurrIndex].ActionRecieverID);
                        brWriter.Write((byte)this.ActionMessages[nCurrIndex].MessageType);
                    }
                }

                // Write amount of projectiles
                brWriter.Write(this.ProjectilesThreadList.Count);

                for (int i = 0; i < this.ProjectilesThreadList.Keys.Count; i++)
                {
                    Projectile prjCurr = this.ProjectilesThreadList.Keys.ElementAt<Projectile>(i);
                    brWriter.Write(prjCurr.Hitbox.Width);
                    brWriter.Write(prjCurr.Hitbox.Height);
                    brWriter.Write(prjCurr.Hitbox.X);
                    brWriter.Write(prjCurr.Hitbox.Y);
                    brWriter.Write((byte)prjCurr.Direction);
                }

                // Write data to player stream
                for (int nCurrPlayer = 0; nCurrPlayer < this.Players.Count; nCurrPlayer++)
                {
                    this.Players[nCurrPlayer].WriteDataToStream(brWriter);

                    // If current player is the player we write to
                    if (this.Players[nCurrPlayer] == plrPlayer)
                    {
                        plrPlayer.IsPositionCare = false;
                        plrPlayer.IsVelocityCare = false;
                    }
                }

                // Write data to player packet sender
                plrPlayer.PacketSender.Write(msCurrStream.ToArray());
            }
            catch (Exception ex)
            {
                if (DEBUG_MODE)
                {
                    throw ex;
                }
                else
                {
                    Console.WriteLine("Connection terminated with player: " + plrPlayer.Name);
                }

                // Set self as disconnected
                plrPlayer.Connected = false;
            }
        }

        /// <summary>
        /// Gets whether all players has names or not.
        /// לכל איש יש שם.
        /// </summary>
        public bool AllPlayersHasName()
        {
            bool bAllPlayersHasName = true;
            
            // Go over players in server
            for (int nCurrPlayer = 0; (nCurrPlayer < this.Players.Count) && (bAllPlayersHasName); nCurrPlayer++)
            {
                // If current player doens't have a name
                if (this.Players[nCurrPlayer].Name == null)
                {
                    bAllPlayersHasName = false;
                }
            }

            return (bAllPlayersHasName);
        }

        /// <summary>
        /// Attack a single player by a single player
        /// </summary>
        /// <param name="plrAttacker"> The Hunter </param>
        /// <param name="plrReciever"> The Hunted </param>
        /// <param name="nHp"> The hp to affec players with </param>
        public void SingleTarget(HandledPlayer plrAttacker, HandledPlayer plrReciever, short nHp)
        {
            plrReciever.AddVelocity((int)((plrReciever.Rect.Center.X -
                                           plrAttacker.Rect.Center.X) * (0.01) * (nHp / 10)),
                                    (int)((plrReciever.Rect.Center.Y -
                                           plrAttacker.Rect.Center.Y) * (0.01) * (nHp / 10)));

            // Hit player
            plrReciever.HP -= nHp;

            // Check for kill
            if (plrReciever.HP == 0)
            {
                // Check if not team kill
                if (plrReciever.IsGoodTeam != plrAttacker.IsGoodTeam)
                {
                    // Make sure that it's not self kill
                    if (plrAttacker != plrReciever)
                    {
                        // Increment executer's kill counter
                        plrAttacker.Kills++;

                        // Add gold to killer and remove gold from killed
                        plrAttacker.Gold += HandledPlayer.KILL_GOLD;
                    }

                    plrReciever.Gold -= (short)(HandledPlayer.KILL_GOLD / 2);
                }
                // Team kill
                else
                {
                    // Increment executer's kill counter
                    plrAttacker.Kills--;
                    plrAttacker.Gold -= HandledPlayer.KILL_GOLD;
                }

                // Increment executed player's death counter 
                plrReciever.Deaths++;

                #region Creating Message

                // Creating the new message
                Message msgKill = new Message()
                {
                    MessagePattern = (byte)rnd.Next(MESSAGE_PATTERNS),
                    ActionPerformerID = (byte)Players.IndexOf(plrAttacker),
                    ActionRecieverID = (sbyte)Players.IndexOf(plrReciever),
                    MessageType = ActionType.Kill,
                    TimePerformed = DateTime.Now
                };

                // Checking if we passed the max message number
                if (this.ActionMessages.Count == MESSAGES_COUNT)
                {
                    // Removing the first one
                    this.ActionMessages.RemoveAt(0);
                }

                // Adding the message
                this.ActionMessages.Add(msgKill); 

                #endregion
            }
        }

        /// <summary>
        /// Affects players in the whole map
        /// </summary>
        /// <param name="nHp">Hp to affect players with</param>
        /// <param name="hpAttacker">The person that executed the attack</param>
        public void AffectArea(short nHp, HandledPlayer hpAttacker)
        {
            // Go over players
            for (int nCurrPlayer = 0; nCurrPlayer < this.Players.Count; nCurrPlayer++)
            {
                if (this.Players[nCurrPlayer].IsAlive)
                {
                    SingleTarget(hpAttacker, this.Players[nCurrPlayer], nHp);
                }
            }

            // Handle post-damage status
            this.PostDamage();
        }

        /// <summary>
        /// Affects players in the hitzone.
        /// </summary>
        /// <param name="rctHitZone">Rectangle represents the hit zone</param>
        /// <param name="nHp">Hp to affect players with</param>
        /// <param name="hpAttacker">The person that executed the attack</param>
        public void AffectArea(Rectangle rctHitZone, short nHp, HandledPlayer hpAttacker)
        {
            // Go over players
            for (int nCurrPlayer = 0; nCurrPlayer < this.Players.Count; nCurrPlayer++)
            {
                // If current player is in the hit zone
                if (this.Players[nCurrPlayer].Rect.Intersects(rctHitZone))
                {
                    if (this.Players[nCurrPlayer].IsAlive)
                    {
                        SingleTarget(hpAttacker, this.Players[nCurrPlayer], nHp);
                    }
                }
            }

            this.PostDamage();
        }

        /// <summary>
        /// The stuff you do after attacking
        /// </summary>
        public void PostDamage()
        {
            // Get Goat Team
            List<HandledPlayer> GoatTeam =
                this.Players.Where((plr) => plr.IsGoodTeam).ToList<HandledPlayer>();

            // Get HolyCow Team
            List<HandledPlayer> HolyCowTeam =
                this.Players.Where((plr) => !plr.IsGoodTeam).ToList<HandledPlayer>();

            // Check that there are players in each team
            if ((!IsRoundOver) && (GoatTeam.Count > 0) && (HolyCowTeam.Count > 0))
            {
                // Check if goat team is dead
                if (GoatTeam.Count((plr) => !plr.IsAlive) == GoatTeam.Count)
                {
                    this.HolyCowScore++;
                    IsRoundOver = true;
                }
                // Check if holy cow team is dead
                else if (HolyCowTeam.Count((plr) => !plr.IsAlive) == HolyCowTeam.Count)
                {
                    this.GoatScore++;
                    IsRoundOver = true;
                }
            }
        }

        /// <summary>
        /// Gets player by name.
        /// </summary>
        /// <param name="strName">Player name</param>
        /// <returns>HandledPlayer instance of found, otherwise null</returns>
        public HandledPlayer GetPlayer(string strName)
        {
            return (this.Players.FirstOrDefault((plr) => plr.Name == strName));
        }

        #endregion

        #region Thread Methods

        #region Wait Connection - Thread

        /// <summary>
        /// Waits for players to connect the server.
        /// </summary>
        private void WaitConnection()
        {
            // Infnite loop
            while (this.Listening)
            {
                // Accept and add new handled player
                HandledPlayer hpNewIncomingPlayer = new HandledPlayer(this.Listener.AcceptTcpClient(), this);

                this.Players.Add(hpNewIncomingPlayer);
                
                lock (LockObject)
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    // Write new connection to console
                    Console.WriteLine("New player connected.");

                    Console.ResetColor();
                }
            }
        }

        #endregion

        #region Disconnection - Thread

        /// <summary>
        /// Scans players for disconnected players and disconnects them.
        /// </summary>
        private void DisconnectPlayers()
        {
            // Infinite loop
            while (true)
            {
                #region Collect Garbage People
                
                // Scan for disconnected players
                for (int i = 0; i < this.Players.Count; i++)
                {
                    // If player isn't connected
                    if (!this.Players[i].Connected)
                    {
                        // Write player disconnection message
                        Console.WriteLine("Disconnected " + this.Players[i].Name);

                        // Remove player from list
                        this.Players[i].KickPlayer();
                        this.Players.RemoveAt(i);

                        // Restart scores when server is empty
                        if (this.Players.Count == 0)
                        {
                            this.GoatScore = 0;
                            this.HolyCowScore = 0;
                        }
                    }
                }

                #endregion
                
                #region Collect Dead Projectiles

                for (int i = 0; i < this.ProjectilesThreadList.Keys.ToList<Projectile>().Count; i++ )
                {
                    Projectile prjCurr = this.ProjectilesThreadList.Keys.ToList<Projectile>()[i];
                    if(!prjCurr.IsAlive)
                    {
                        this.ProjectilesThreadList.Remove(prjCurr);
                    }
                }

                #endregion

                #region Round Restarter

                    // Restart round
                    if (IsRoundOver)
                    {
                        this.CommandRunner["restart"](this, new string[] { "3000" });
                        this.Timer.Reset();
                    }

                #endregion

                #region Regeneration Section
                
                // Increment server ticks
                this.ServerTick = (this.ServerTick + 1) % 1000;

                // Go over all players
                for (int nCount = 0; nCount < this.Players.Count; nCount++)
                {
                    // Get current player
                    HandledPlayer hpCurrent = this.Players[nCount];

                    if (hpCurrent.Connected)
                    {
                        if (this.ServerTick % GOLD_GAIN_RATE == 0)
                        {
                            hpCurrent.Gold++;
                        }

                        if ((hpCurrent.IsAlive) && (this.ServerTick % REGEN_RATE == 0))
                        {
                            hpCurrent.MP += hpCurrent.FinalStats[Attributes.MPRegen];
                            hpCurrent.HP += hpCurrent.FinalStats[Attributes.HPRegen];
                        }
                    }
                }

                #endregion

                #region Collect Garbage Message

                // Going through all messages checking if they are too old
                for (int nIndex = 0; nIndex < this.ActionMessages.Count; nIndex++)
                {
                    // Checking if MESSAGES_DURATION seconds passed since it was created
                    if (DateTime.Now.Subtract(ActionMessages[nIndex].TimePerformed).Seconds >= MESSAGES_DURATION)
                    {
                        this.ActionMessages.Remove(ActionMessages[nIndex]);
                    }
                }

                #endregion

                // Sleep for sleep amount
                Thread.Sleep(SLEEP_AMOUNT);
            }
        }
        
        #endregion

        #region Command - Thread

        private void ReadCommands()
        {
            while (true)
            {
                // Split command input
                string[] arrstrCommand = Regex.Split(Console.ReadLine(), " ");

                try
                {
                    // Execute command
                    this.CommandRunner[arrstrCommand[0]](this, arrstrCommand.Skip(1).ToArray());
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("ERROR: Command doesn't exist");
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                }
            }
        }

        #endregion

        #endregion
    }
}
