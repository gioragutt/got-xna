using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;

namespace Game_Of_Throws
{
    /// <summary>
    /// Enum that describes action types
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
        public byte ActionPerformerID;
        public ActionType MessageType;
        public sbyte ActionRecieverID;
        public string Mess;

        public static string[] arrMessages = new string[] {" has killed ", 
                                                           " has slain ", 
                                                           " has murdered ",
                                                           " has pooped on ",
                                                           " has slashed ",
                                                           " has dimolished ",
                                                           " has banana-ed ",
                                                           " REKT ",
                                                           " wipped "};
    }

    /// <summary>
    /// Represents a connection to the server.
    public class Connection
    {
        #region Data Members

        // Const Members
        private const int TIMEOUT_TICKS = 250;
        private const int SLEEP_AMOUNT = 20;
        private const int MESSAGE_AMOUNT = 5;
        public const int NAME_BYTES = 10;

        // Data Members
        public byte GoatScore;
        public byte HolyCowsScore;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the tcp client of the connection.
        /// </summary>
        private TcpClient Client { get; set; }
        
        /// <summary>
        /// Gets and sets the list of the players in the network.
        /// </summary>
        public Player[] Players { get; private set; }

        /// <summary>
        /// Gets and sets the local player.
        /// </summary>
        private LocalPlayer LocalPlayer { get; set; }

        /// <summary>
        /// Gets the connection status of the connection.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Gets and sets the is team sent indication.
        /// </summary>
        private bool TeamSent { get; set; }

        /// <summary>
        /// Gets and sets the update thread.
        /// </summary>
        private Thread UpdateThread { get; set; }

        /// <summary>
        /// Gets and sets the connection packet reader.
        /// </summary>
        private BinaryReader PacketReader { get; set; }

        /// <summary>
        /// Gets and sets the connection packet sender.
        /// </summary>
        private BinaryWriter PacketSender { get; set; }

        /// <summary>
        /// Gets and sets the ping of the network player.
        /// </summary>
        public long Ping { get; private set; }

        /// <summary>
        /// Gets and sets the id of the local player.
        /// </summary>
        private byte LocalPlayerID { get; set; }

        /// <summary>
        /// Gets and sets amount of not responding ticks.
        /// </summary>
        private int NotRespondingTicks { get; set; }

        /// <summary>
        /// Gets and sets the stop watch of the connection.
        /// </summary>
        private Stopwatch stpPingWatch { get; set; }

        /// <summary>
        /// Gets and sets the messages of the connection
        /// </summary>
        public Message[] Messages { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the Connection class for a local player
        /// </summary>
        /// <param name="plrPlayer">Local player to set connection for</param>
        public Connection(LocalPlayer plrPlayer)
        {
            // Initialize scores
            this.GoatScore = 0;
            this.HolyCowsScore = 0;

            // Initialize message array
            this.Messages = new Message[MESSAGE_AMOUNT];

            // Initialize with empty messages
            for (int nCurrMessage = 0; nCurrMessage < this.Messages.Length; nCurrMessage++)
            {
                this.Messages[nCurrMessage] = new Message()
                                              {
                                                  ActionPerformerID = 0,
                                                  ActionRecieverID = 0,
                                                  MessageType = ActionType.Empty
                                              };
            }

            // Create new tcp client
            this.Client = new TcpClient();

            // Set connection local player as plrPlayer
            this.LocalPlayer = plrPlayer;

            // Initialize player array
            this.Players = new Player[0];

            // Set team sent to false
            this.TeamSent = false;

            // Initialize and start the stopwatch
            this.stpPingWatch = new Stopwatch();
            this.stpPingWatch.Start();
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Sets the server as didn't respond in this tick.
        /// </summary>
        public void DidntRespond()
        {
            // If client didn't respond for timeout ticks
            if (this.NotRespondingTicks == TIMEOUT_TICKS)
            {
                // Set connected as false
                this.Connected = false;
            }
            else
            {
                this.NotRespondingTicks++;
            }
        }

        /// <summary>
        /// Sets the server as responded in this tick.
        /// </summary>
        public void ServerResponded()
        {
            this.NotRespondingTicks = 0;
        }

        /// <summary>
        /// Tries to open a connection to the given ip.
        /// </summary>
        /// <param name="strIp">The address of the server</param>
        public void Connect(string strIp)
        {
            // If connection isn't open
            if (!this.Connected)
            {
                // Connect to strIp in port 27015
                this.Client.Connect(strIp, 27015);

                // Set connected to true
                this.Connected = true;
                
                // Create packet sender and reader
                this.PacketSender = new BinaryWriter(this.Client.GetStream());
                this.PacketReader = new BinaryReader(this.Client.GetStream());

                // Send current player data
                this.WriteData();

                this.UpdateThread = new Thread(this.UpdateFromConnection);

                this.UpdateThread.IsBackground = true;

                this.UpdateThread.Start();
            }
            else
            {
                throw new Exception("Connection attempt when already connected");
            }
        }

        /// <summary>
        /// Disconnect from server.
        /// </summary>
        public void Disconnect()
        {
            // If connection is open
            if (this.Connected)
            {
                // Set connected to false
                this.Connected = false;

                MessageBox.Show("Disconnected from server");
            }
            else
            {
                throw new Exception("Disconnect attempt when connection wasn't open");
            }
        }

        /// <summary>
        /// Sets players slots in the network.
        /// </summary>
        /// <param name="nPlayersInNetwork">Amount of players in the network</param>
        private void SetPlayerAmount(int nPlayersInNetwork)
        {
            // If player amount has changed
            if (this.Players.Length != nPlayersInNetwork)
            {
                // Create a new array
                Player[] plrTemp = new Player[nPlayersInNetwork];

                // Go over slots in player array
                for (int nCurrPlayer = 0; nCurrPlayer < plrTemp.Length; nCurrPlayer++)
                {
                    if (nCurrPlayer != this.LocalPlayerID)
                    {
                        // Add new network player slot.
                        plrTemp[nCurrPlayer] = new NetworkPlayer(0, 0, "Un-named");
                    }
                    else
                    {
                        // Add connection's local player to player slot
                        plrTemp[nCurrPlayer] = this.LocalPlayer;
                    }
                }

                // Set players as plrTemp
                this.Players = plrTemp;
            }
        }

        /// <summary>
        /// Write local player data to server.
        /// </summary>
        private void WriteData()
        {
            MemoryStream msCurrentStream = new MemoryStream();
            BinaryWriter brWriter = new BinaryWriter(msCurrentStream);

            // If team wasn't sent yet
            if (!this.TeamSent)
            {
                // Set team sent indication to false
                this.TeamSent = true;

                // Send team
                brWriter.Write(this.LocalPlayer.IsGoodTeam);
            }

            // Write position to stream
            brWriter.Write((short)this.LocalPlayer.PosX);
            brWriter.Write((short)this.LocalPlayer.PosY);

            // Write direction
            brWriter.Write((byte)this.LocalPlayer.Direction);

            // Write is moving indication
            brWriter.Write(this.LocalPlayer.IsMoving);

            // Write is attacking indication of the player
            brWriter.Write(this.LocalPlayer.IsAttacking);

            // Send name to server
            brWriter.Write(ASCIIEncoding.ASCII.
                                            GetBytes(this.LocalPlayer.
                                                            Name.PadRight(NAME_BYTES)));

            // Write item activation
            if ((this.LocalPlayer.SelectedItem != -1) &&
               (!this.LocalPlayer.IsWaitingDirection))
            {
                brWriter.Write(this.LocalPlayer.SelectedItem);
                this.LocalPlayer.Inventory[(byte)this.LocalPlayer.SelectedItem].SetCooldown();
                this.LocalPlayer.ActivatedSkill = this.LocalPlayer.Inventory[(byte)this.LocalPlayer.SelectedItem].ItemID;
                this.LocalPlayer.SelectedItem = -1;
            }
            else
            {
                brWriter.Write((sbyte)-1);
            }
            
            // Write item buy
            brWriter.Write(this.LocalPlayer.SelectedItemToBuy);

            // Reset item to buy to default
            this.LocalPlayer.SelectedItemToBuy = -1;

            // Write data to server
            this.PacketSender.Write(msCurrentStream.ToArray());

            // Reset is attacking indication
            this.LocalPlayer.IsAttacking = false;
        }

        /// <summary>
        /// Updates network players in local game.
        /// </summary>
        public void UpdateFromConnection()
        {
            while (this.Connected)
            {
                // If data available read player amount bytes
                if (this.Client.Available >= 2)
                {
                    // Set server as responded in this tick
                    this.ServerResponded();

                    // Place stopwatch's elapsed milliseconds in ping
                    this.Ping = stpPingWatch.ElapsedMilliseconds;

                    // Read local player id
                    this.LocalPlayerID = this.PacketReader.ReadByte();

                    // Set players amount
                    this.SetPlayerAmount(this.PacketReader.ReadByte());

                    // Read team scores
                    this.GoatScore = this.PacketReader.ReadByte();
                    this.HolyCowsScore = this.PacketReader.ReadByte();

                    // Read messages
                    for (int nCurrMessage = 0; nCurrMessage < this.Messages.Length; nCurrMessage++)
                    {
                        this.Messages[nCurrMessage] = new Message()
                                                      {
                                                        Mess = Message.arrMessages[this.PacketReader.ReadByte()],
                                                        ActionPerformerID = this.PacketReader.ReadByte(),
                                                        ActionRecieverID = this.PacketReader.ReadSByte(),
                                                        MessageType = (ActionType)this.PacketReader.ReadByte()
                                                      };
                    }

                    Game1.projectileHandler.Update(this.PacketReader);

                    // Update players in the network
                    for (int nCurrPlayer = 0;
                         (nCurrPlayer < this.Players.Length);
                         nCurrPlayer++)
                    {
                        this.Players[nCurrPlayer].Read(this.PacketReader);
                    }

                    // Send current player data
                    this.WriteData();

                    // Restart stwPingCount
                    stpPingWatch.Restart();
                }
                else
                {
                    this.DidntRespond();
                }

                // Sleep for sleep amount
                Thread.Sleep(SLEEP_AMOUNT);
            }
        }

        #endregion

        #region XNA Methods

        /// <summary>
        /// Predicts the network players activity using interpolation.
        /// </summary>
        public void Update()
        {
            // Go over players in connection
            for (int nCurrPlayer = 0; nCurrPlayer < this.Players.Length; nCurrPlayer++)
            {
                // Predict current netowrk player next tick
                this.Players[nCurrPlayer].Update(this);
            }
        }
        
        /// <summary>
        /// Draws the players in the connection.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch to draw in</param>
        public void DrawPlayers(SpriteBatch spriteBatch)
        {
            // Go over players in the connection
            foreach (Player plrCurr in this.Players)
            {
                // Draw current player
                plrCurr.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
