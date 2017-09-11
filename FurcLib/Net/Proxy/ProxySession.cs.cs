﻿using Furcadia.Drawing;
using Furcadia.Movement;
using Furcadia.Net.Dream;
using Furcadia.Net.Utils.ServerParser;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//Furcadia Servver Parser
//Event/Delegates for server instructions
//call subsystem processor

//dream info
//Furre info
//Bot info

//Furre Update events?

using static Furcadia.Drawing.VisibleArea;
using static Furcadia.Movement.CharacterFlags;
using static Furcadia.Text.Base220;
using static Furcadia.Text.FurcadiaMarkup;
using static Furcadia.Util;

namespace Furcadia.Net.Proxy
{
    /// <summary>
    /// This Instance handles the current Furcadia Session.
    /// <para>
    /// Part1: Manage MonkeySpeak Engine Start,Stop,Restart. System
    ///        Variables, MonkeySpeak Execution Triggers
    /// </para>
    /// <para>
    /// Part2: Furcadia Proxy Controls, In/Out Ports, Host, Character Ini
    ///        file. Connect, Disconnect, Reconnect
    /// </para>
    /// <para>
    /// Part2a: Proxy Functions do link to Monkey Speak trigger execution
    /// </para>
    /// <para>
    /// Part3: This Class Links loosley to the GUI
    /// </para>
    /// </summary>
    public class ProxySession : NetProxy, IDisposable
    {
        #region "Constructors"

        /// <summary>
        /// </summary>
        public ProxySession() : base()
        {
            options = new Options.ProxySessionOptions();
            Initilize();
        }

        /// <summary>
        /// </summary>
        /// <param name="Options">
        /// ProxySession Options
        /// </param>
        public ProxySession(ref Options.ProxySessionOptions Options) : base( ref Options)
        {
            options = Options;
            Initilize();
        }

        /// <summary>
        /// Disconnect from Furcadia and notify delegates.
        /// </summary>
        public override void Disconnect()
        {
            base.Disconnect();

            if (serverconnectphase == ConnectionPhase.Disconnected)
            {
                serverconnectphase = ConnectionPhase.Init;
                ServerStatusChanged?.Invoke(this, new NetServerEventArgs(serverconnectphase, ServerInstructionType.None));
            }
            if (clientconnectionphase == ConnectionPhase.Disconnected)
            {
                clientconnectionphase = ConnectionPhase.Init;
                ClientStatusChanged?.Invoke(this, new NetClientEventArgs(clientconnectionphase));
            }
        }

        private void Initilize()
        {
            serverconnectphase = ConnectionPhase.Init;
            clientconnectionphase = ConnectionPhase.Init;

            ServerBalancer = new Utils.ServerQue();
            ServerBalancer.OnServerSendMessage += onServerQueSent;

            base.ServerData2 += onServerDataReceived;
            base.Connected += onServerConnected;
            base.ServerDisConnected += onServerDisconnected;
            // if (!options.Standalone) {
            base.ClientData2 += onClientDataReceived;
            base.ClientDisConnected += onClientDisconnected;
            base.Connected += onClientConnected;
            base.ClientExited += onClientExited;
            // }
            ReconnectionManager = new Furcadia.Net.Utils.ProxyReconnect(options.ReconnectOptions);
            dream = new DREAM();
            BadgeTag = new Queue<string>(50);
            LookQue = new Queue<string>(50);
            SpeciesTag = new Queue<string>(50);
            BanishString = new List<string>(50);
        }

        private void onClientExited()
        {
        }

        #endregion "Constructors"

        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        public override void Connect()
        {
            if (serverconnectphase == ConnectionPhase.Init && clientconnectionphase == ConnectionPhase.Init)
            {
                serverconnectphase = ConnectionPhase.Connecting;
                clientconnectionphase = ConnectionPhase.Connecting;
                base.Connect();
            }
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Allows the Furcadia Client to Disconnect from the session,
        /// allowing the session to remain connected to the game server
        /// </summary>
        public bool StandAlone
        {
            get
            {
                return options.Standalone;
            }
            set
            {
                options.Standalone = value;
            }
        }

        #endregion Public Properties

        #region Connected Furre

        private FURRE connectedFurre;

        /// <summary>
        /// Connected Characters Furcadia ID
        /// </summary>
        public int ConnectedCharacterFurcadiaID
        {
            get
            {
                if (connectedFurre == null)
                    return -1;
                return connectedFurre.ID;
            }
            set
            {
                if (connectedFurre == null)
                    connectedFurre = new FURRE(value);
                connectedFurre.ID = value;
            }
        }

        /// <summary>
        /// Our Connected Character name
        /// </summary>
        public string ConnectedCharacterName
        {
            get
            {
                if (connectedFurre == null)
                    return null;
                return connectedFurre.Name;
            }
            set
            {
                if (connectedFurre == null)
                    connectedFurre = new FURRE(value);
                connectedFurre.Name = value;
            }
        }

        /// <summary>
        /// Connected Furre (Who we are)
        /// </summary>
        public FURRE ConnectedFurre
        {
            get
            {
                return connectedFurre;
            }
        }

        /// <summary>
        /// Are we the current executing character?
        /// </summary>
        public bool IsConnectedCharacter
        {
            get
            {
                //if (player == null | connectedFurre == null)
                //    return false;
                return player == connectedFurre;
            }
        }

        #endregion Connected Furre

        #region Private Fields

        private string banishName = "";
        private List<string> banishString = new List<string>();

        /// <summary>
        /// Furre Name we connected with
        /// </summary>
        private string botName;

        private int botUID;
        private string channel;
        private object ChannelLock = new object();
        private bool clientClose = false;
        private ConnectionPhase clientconnectionphase;
        private object clientlock = new object();
        private object DataReceived = new object();
        private bool disposed = false;
        private DREAM dream;
        private string errorMsg = "";
        private short errorNum = 0;
        private bool hasShare;
        private bool inDream = false;
        private bool Look;
        private Queue<string> LookQue;
        private Options.ProxySessionOptions options;
        private FURRE player;

        /// <summary>
        /// Manage out Auto reconnects
        /// </summary>
        private Utils.ProxyReconnect ReconnectionManager;

        /// <summary>
        /// Balance thhe out going load to server
        /// <para>
        /// Throat Tired Syndrome and No Endurance Control
        /// </para>
        /// </summary>
        private Utils.ServerQue ServerBalancer;

        private Queue<string> SpeciesTag;

        #endregion Private Fields

        #region "Public Events"

        /// <summary>
        /// </summary>
        /// <param name="Sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public delegate void ClientStatusChangedEventHandler(object Sender, NetClientEventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public delegate void OnErrorEventHandler(object sender, EventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="Sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public delegate void ServerStatusChangedEventHandler(object Sender, NetServerEventArgs e);

        /// <summary>
        /// This is triggered when the Client sends data to the server.
        /// Expects a return value.
        /// </summary>
        public override event DataEventHandler2 ClientData2;

        /// <summary>
        /// Track the Furcadia Client status
        /// </summary>
        public event ClientStatusChangedEventHandler ClientStatusChanged;

        /// <summary>
        /// This is triggered when the Server sends data to the client.
        /// Doesn't expect a return value.
        /// </summary>
        public override event DataEventHandler2 ServerData2;

        /// <summary>
        /// Track the Server Status
        /// </summary>
        public event ServerStatusChangedEventHandler ServerStatusChanged;

        #region "Client/Server Fata Handling"

        /// <summary>
        /// Send Data to Furcadia Client or Game Server
        /// </summary>
        /// <param name="Message">
        /// Raw instruction to send
        /// </param>
        /// <param name="e">
        /// Cliemt or Server Event Argumentss with Instruction type
        /// </param>
        public delegate void DataHandler(string Message, EventArgs e);

        /// <summary>
        /// </summary>
        /// <param name="InstructionObject">
        /// </param>
        /// <param name="Args">
        /// </param>
        public delegate void ProcessChannel(ChannelObject InstructionObject, ParseServerArgs Args);

        /// <summary>
        /// Send Server to Client Instruction object to Subclassed for handlings
        /// </summary>
        /// <param name="InstructionObject">
        /// Server Instruction Object
        /// </param>
        /// <param name="Args">
        /// </param>
        public delegate void ProcessInstruction(BaseServerInstruction InstructionObject, ParseServerArgs Args);

        /// <summary>
        /// Process Display Text and Channels
        /// </summary>
        public event ProcessChannel ProcessServerChannelData;

        /// <summary>
        /// </summary>
        public event ProcessInstruction ProcessServerInstruction;

        #endregion "Client/Server Fata Handling"

        #endregion "Public Events"

        #region Public Properties

        #region Server Queue Manager

        /// <summary>
        /// ServerQueue Throat Tired Mode
        /// </summary>
        /// <returns>
        /// State <see cref="Furcadia.Net.Utils.ServerQue.ThroatTired"/>
        /// </returns>
        public bool ThroatTired
        {
            get { return ServerBalancer.ThroatTired; }
            set { ServerBalancer.ThroatTired = value; }
        }

        /// <summary>
        /// Server Queue NoEndurance mode
        /// </summary>
        internal bool NoEndurance
        {
            get { return ServerBalancer.NoEndurance; }
            set { ServerBalancer.NoEndurance = value; }
        }

        #endregion Server Queue Manager

        private ConnectionPhase serverconnectphase;

        /// <summary>
        /// Beekin Badge
        /// </summary>
        public Queue<string> BadgeTag { get; set; }

        /// <summary>
        /// Current Name for Banish Operations
        /// <para>
        /// We mirror Furcadia's Banish system for efficiency
        /// </para>
        /// </summary>
        public string BanishName { get { return banishName; } set { banishName = value; } }

        /// <summary>
        /// </summary>
        public List<string> BanishString { get { return banishString; } set { banishString = value; } }

        /// <summary>
        /// </summary>
        public string Channel
        {
            get { return channel; }
        }

        /// <summary>
        /// Current Connection Phase
        /// </summary>
        public ConnectionPhase ClientConnectPhase
        {
            get { return clientconnectionphase; }
        }

        /// <summary>
        /// Client Connection status
        /// </summary>
        /// <returns>
        /// Status tog the Furcadia Client
        /// </returns>
        public ConnectionPhase ClientStatus
        {
            get { return clientconnectionphase; }
        }

        /// <summary>
        /// Current Dream Information with Furre List
        /// </summary>
        public DREAM Dream
        {
            get { return dream; }
            set
            {
                dream = value;
            }
        }

        /// <summary>
        /// </summary>
        public string ErrorMsg { get { return errorMsg; } }

        /// <summary>
        /// </summary>
        public short ErrorNum { get { return errorNum; } }

        /// <summary>
        /// We have Dream Share or We are Dream owner
        /// </summary>
        public bool HasShare { get { return hasShare; } }

        /// <summary>
        /// </summary>
        public bool InDream { get { return inDream; } }

        /// <summary>
        /// Current Triggering player
        /// </summary>
        public FURRE Player
        {
            get { return player; }
            // set { player = value; }
        }

        /// <summary>
        /// Curent server connection phase
        /// </summary>
        public ConnectionPhase ServerConnectPhase
        {
            get { return serverconnectphase; }
        }

        /// <summary>
        /// Server Connection status
        /// </summary>
        /// <returns>
        /// Status of the Furcadia Game server
        /// </returns>
        public ConnectionPhase ServerStatus
        {
            get { return serverconnectphase; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// implementation of Dispose pattern callable by consumers.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="enumVal">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetEnumInt<T>(T enumVal)
        {
            return Convert.ToInt32(enumVal);
        }

        #endregion Public Methods

        /// <summary>
        /// Parse Channel Data
        /// </summary>
        /// <param name="data">
        /// Raw Game Server to Client instruction
        /// </param>
        /// <param name="Handled">
        /// Is this data already handled?
        /// </param>
        /// <remarks>
        /// This is derived content from the Furcadia Dev Docs and Furcadia
        /// Technical Resources
        /// <para>
        /// Update 23 Avatar Movement http://dev.furcadia.com/docs/023_new_movement.pdf
        /// </para>
        /// <para>
        /// Update 27 Movement http://dev.furcadia.com/docs/027_movement.html
        /// </para>
        /// <para>
        /// FTR http://ftr.icerealm.org/ref-instructions/
        /// </para>
        /// </remarks>
        public virtual void ParseServerChannel(string data, bool Handled)
        {
            //TODO: needs to move and re factored to ServerParser Class

            data = data.Remove(0, 1);
            Regex NameRegex = new Regex(NameFilter, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            try
            {
                string Color = Regex.Match(data, EntryFilter).Groups[1].Value;
                string User = null;
                if (NameRegex.Match(data).Success)
                {
                    User = NameRegex.Match(data).Groups[1].Value;
                    player = dream.FurreList.GerFurreByName(User);
                }

                string Text = NameRegex.Replace(data, "$2");
                Regex ChannelRegEx = new Regex(ChannelNameFilter);
                if (ChannelRegEx.Match(data).Success)
                {
                    channel = ChannelRegEx.Match(data).Groups[1].Value;
                }
                if (!Handled)
                {
                    Regex DescRegex = new Regex(DescFilter);
                    if (DescRegex.Match(data).Success)
                    {
                        player = dream.FurreList.GerFurreByName(DescRegex.Match(data).Groups[1].Value);
                        player.Desc = DescRegex.Match(data).Groups[2].Value;
                        // player = NameToFurre(DescName);
                        if (LookQue.Count > 0)
                        {
                            player.Color = new ColorString(LookQue.Dequeue());
                        }
                        if (BadgeTag.Count > 0)
                        {
                            player.Badge = BadgeTag.Dequeue(); ;
                        }
                        else if (!string.IsNullOrEmpty(player.Badge))
                        {
                            player.Badge = "";
                        }
                        Look = false;
                    }

                    Text = ChannelRegEx.Replace(data, "[$1]");
                    bool HasFurcIcon = SystemFshIcon(ref Text, string.Empty);
                    player.Message = Text;
                }

                errorMsg = "";
                errorNum = 0;

                if (Color == "success")
                {
                    if (Text.Contains(" has been banished from your dreams."))
                    {
                        //banish <name> (online)
                        //Success: (.*?) has been banished from your dreams.

                        Regex t = new Regex("(.*?) has been banished from your dreams.");
                        BanishName = t.Match(Text).Groups[1].Value;

                        BanishString.Add(BanishName);
                    }
                    else if (Text == "You have canceled all banishments from your dreams.")
                    {
                        //banish-off-all (active list)
                        //Success: You have canceled all banishments from your dreams.
                        BanishString.Clear();
                    }
                    else if (Text.EndsWith(" has been temporarily banished from your dreams."))
                    {
                        //tempbanish <name> (online)
                        //Success: (.*?) has been temporarily banished from your dreams.

                        Regex t = new Regex("(.*?) has been temporarily banished from your dreams.");
                        BanishName = t.Match(Text).Groups[1].Value;

                        // MainMSEngine.PageExecute(61)
                        BanishString.Add(BanishName);
                    }
                    else if (Text == "Control of this dream is now being shared with you.")
                    {
                        hasShare = true;
                    }
                    else if (Text.EndsWith("is now sharing control of this dream with you."))
                    {
                        hasShare = true;
                    }
                    else if (Text.EndsWith("has stopped sharing control of this dream with you."))
                    {
                        hasShare = false;
                    }
                    else if (Text.StartsWith("The endurance limits of player "))
                    {
                        Regex t = new Regex("The endurance limits of player (.*?) are now toggled off.");
                        string m = t.Match(Text).Groups[1].Value;
                        if (FurcadiaShortName(m) == FurcadiaShortName(botName))
                        {
                            NoEndurance = true;
                        }
                    }
                }
                else if (Color == "bcast")
                {
                    string AdRegEx = "<channel name='(.*)' />";

                    string chan = Regex.Match(data, AdRegEx).Groups[1].Value;
                    string u;
                    switch (chan)
                    {
                        case "@advertisements":

                            AdRegEx = "\\[(.*?)\\] (.*?)</font>";
                            string adMessage = Regex.Match(data, AdRegEx).Groups[2].Value;

                            break;

                        case "@bcast":

                            u = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups[2].Value;

                            break;

                        case "@announcements":

                            u = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups[2].Value;

                            break;

                        default:
#if DEBUG
                            Console.WriteLine("Unknown ");
                            Console.WriteLine("BCAST:" + data);
#endif
                            break;
                    }
                }
                else if (Color == "myspeech")
                {
                    Regex t = new Regex(YouSayFilter);
                    string u = t.Match(data).Groups[1].Value;
                    Text = t.Match(data).Groups[2].Value;

                    player.Message = Text;
                }
                else if (!string.IsNullOrEmpty(User)
                    & string.IsNullOrEmpty(Channel) &
                    string.IsNullOrEmpty(Color) &
                    Regex.Match(data, NameFilter).Groups[2].Value != "forced")
                {
                    Match tt = Regex.Match(data, "\\(you see(.*?)\\)", RegexOptions.IgnoreCase);
                    Regex t = new Regex(NameFilter);

                    if (!tt.Success)
                    {
                        Text = t.Replace(data, "");
                        Text = Text.Remove(0, 2);

                        if (SpeciesTag.Count > 0)
                        {
                            //player.Color = SpeciesTag.Dequeue();
                            if (Dream.FurreList.Contains(player))
                                Dream.FurreList[player] = player;
                        }
                        channel = "say";
                    }
                    else
                    {
                        //sndDisplay("You See '" & User & "'")
                        Look = true;
                    }
                }
                else if (Color == "query")
                {
                    channel = "query";
                }
                else if (data.StartsWith("PS"))
                {
                    channel = "PhoenixSpeak";
                }
                else if (Color == "shout")
                {
                    channel = "shout";
                }
                else if (Color == "whisper")
                {
                    channel = "whisper";
                    Regex WhisperIncoming = new Regex("^\\<font color=('whisper'|\"whisper\")\\>\\[ \\<name shortname=('[a-z0-9]{2,64}'|\"[a-z0-9]{2,64}\") src=('whisper-from'|\"whisper-from\")\\>(?<name>.{2,64})\\</name\\> whispers, \"(?<msg>.+)\" to you\\. \\]\\</font\\>$", RegexOptions.IgnoreCase);
                    Regex WhisperOutgoing = new Regex("^\\<font color=('whisper'|\"whisper\")\\>\\[ You whisper \"(?<msg>.+)\" to \\<name shortname=('[a-z0-9]{2,64}'|\"[a-z0-9]{2,64}\") forced(=('forced'|\"forced\"))? src=('whisper-to'|\"whisper-to\")\\>(?<name>.{2,64})\\</name\\>\\. \\]\\</font\\>$", RegexOptions.IgnoreCase);
                    //'WHISPER
                    // string WhisperFrom = WhisperIncoming.Match(data).Captures[1].Value;
                    if (WhisperIncoming.Match(data).Success)
                    {
                        player = dream.FurreList.GerFurreByName(WhisperIncoming.Match(data).Groups[4].Value);
                        player.Message = WhisperIncoming.Match(data).Groups[5].Value;
                        if (BadgeTag.Count > 0)
                        {
                            player.Badge = BadgeTag.Dequeue();
                        }
                        else
                        {
                            player.Badge = "";
                        }
                    }
                    else if (WhisperOutgoing.Match(data).Success)
                    {
                        player = dream.FurreList.GerFurreByName(WhisperOutgoing.Match(data).Groups[4].Value);
                        player.Message = WhisperOutgoing.Match(data).Groups[5].Value;
                    }
                }
                else if (Color == "warning")
                {
                    errorMsg = Text;
                    errorNum = 1;
                }
                else if (Color == "trade")
                {
                    channel = "trade";
                    string TextStr = Regex.Match(data, "\\s<name (.*?)</name>").Groups[0].Value;
                    Text = Text.Substring(6);
                    if (!string.IsNullOrEmpty(User))
                        Text = " " + User + Text.Replace(TextStr, "");
                    player.Message = Text;
                }
                else if (Color == "emote")
                {
                    channel = "emote";
                    // ''EMOTE
                    if (SpeciesTag.Count > 0)
                    {
                        player.Color = new ColorString(SpeciesTag.Dequeue());
                    }
                    Regex usr = new Regex(NameFilter);
                    System.Text.RegularExpressions.Match n = usr.Match(Text);
                    Text = usr.Replace(Text, string.Empty);
                }
                else if (Color == "notify")
                {
                    string NameStr = "";
                    if (Text.StartsWith("players banished from your dreams: "))
                    {
                        //Banish-List
                        //[notify> players banished from your dreams:
                        //`(0:54) When the bot sees the banish list
                        BanishString.Clear();
                        string[] tmp = Text.Substring(35).Split(',');
                        foreach (string t in tmp)
                        {
                            BanishString.Add(t);
                        }
                    }
                    else if (Text.StartsWith("The banishment of player "))
                    {
                        //banish-off <name> (on list)
                        //[notify> The banishment of player (.*?) has ended.

                        Regex t = new Regex("The banishment of player (.*?) has ended.");
                        NameStr = t.Match(data).Groups[1].Value;
                        player = new FURRE(NameStr);
                        bool found = false;
                        int I;
                        for (I = 0; I <= BanishString.Count - 1; I++)
                        {
                            if (FurcadiaShortName(BanishString[I]) == FurcadiaShortName(NameStr))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                            BanishString.RemoveAt(I);
                    }
                }
                else if (Color == "error")
                {
                    channel = "error";
                    errorMsg = Text;
                    errorNum = 2;

                    string NameStr = "";
                    if (Text.Contains("There are no furres around right now with a name starting with "))
                    {
                        //Banish <name> (Not online)
                        //Error:>>  There are no furres around right now with a name starting with (.*?) .

                        Regex t = new Regex("There are no furres around right now with a name starting with (.*?) .");
                        NameStr = t.Match(data).Groups[1].Value;
                    }
                    else if (Text == "Sorry, this player has not been banished from your dreams.")
                    {
                        //banish-off <name> (not on list)
                        //Error:>> Sorry, this player has not been banished from your dreams.

                        NameStr = BanishName;
                    }
                    else if (Text == "You have not banished anyone.")
                    {
                        //banish-off-all (empty List)
                        //Error:>> You have not banished anyone.

                        BanishString.Clear();
                    }
                }
                else if (data.StartsWith("Communication"))
                {
                    Disconnect();
                }
                else if (channel == "@cookie")
                {
                    // <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
                    // <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
                    // <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
                    //<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

                    Regex EatCookie = new Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)");
                    if (EatCookie.Match(data).Success)
                    {
                        player.Message = "You eat a cookie." + EatCookie.Replace(data, "");
                    }
                }
            }
            catch (Exception ex)
            {
                SendError(ex, this, "");
            }

            if (Dream.FurreList.Contains(player))
                Dream.FurreList[player] = player;

            ChannelObject chanObject = new ChannelObject(data);
            chanObject.Player = player;

            chanObject.Channel = channel;
            chanObject.InstructionType = ServerInstructionType.DisplayText;

            ParseChannelArgs args = new ParseChannelArgs(ServerInstructionType.DisplayText, serverconnectphase);

            ProcessServerChannelData?.Invoke(chanObject, args);
        }

        /// <summary>
        /// Parse Server Data
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <param name="Handled">
        /// </param>
        /// ///
        /// <remarks>
        /// This is derived content from the Furcadia Dev Docs and Furcadia
        /// Technical Resources
        /// <para>
        /// Update 23 Avatar Moement http://dev.furcadia.com/docs/023_new_movement.pdf
        /// </para>
        /// <para>
        /// Update 27 Movement http://dev.furcadia.com/docs/027_movement.html
        /// </para>
        /// <para>
        /// FTR http://ftr.icerealm.org/ref-instructions/
        /// </para>
        /// </remarks>
        public virtual void ParseServerData(string data, bool Handled)
        {
            switch (serverconnectphase)
            {
                case ConnectionPhase.MOTD:
                    // SSL/TLS `starttls
                    if (data.StartsWith("]s"))
                    {
                        Regex t = new Regex("\\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.Match m = t.Match(data);
                    }
                    if (data == "Dragonroar")
                    {
                        // Login Sucessful
                        serverconnectphase = ConnectionPhase.Auth;
                        ServerStatusChanged?.Invoke(data, new NetServerEventArgs(serverconnectphase, ServerInstructionType.Unknown));
                        // Standalone / account Send(String.Format("connect
                        // {0} {1}\n", sUsername, sPassword));

                        if (IsClientConnected)
                        {
                            clientconnectionphase = ConnectionPhase.Auth;
                            ClientStatusChanged?.Invoke(data, new NetClientEventArgs(clientconnectionphase));
                        }
                    }
                    break;

                case ConnectionPhase.Auth:
                    if (data.StartsWith("]#"))
                    {
                        //char[] sep = { ' ' };
                        //string[] tokens = data.Split(sep, 3);
                        //sErrorMessage = tokens[2];
                    }
                    else if (data.StartsWith("]]"))
                    {
                        Disconnect();
                        //bIsRunning = false;
                    }
                    else if (data == "&&&&&&&&&&&&&")
                    {
                        //We've connected to Furcadia
                        //Stop the reconnection manager

                        serverconnectphase = ConnectionPhase.Connected;
                        //if (IsClientConnected)
                        //{
                        //    clientconnectionphase = ConnectionPhase.Connected;
                        //    ClientStatusChanged?.Invoke(null, new NetClientEventArgs(clientconnectionphase));

                        //}

                        //ProcessServerInstruction?.Invoke(FurreSpawn,
                        //        new ParseServerArgs(ServerInstructionType.SpawnAvatar, serverconnectphase));

                        ServerStatusChanged?.Invoke(data, new NetServerEventArgs(serverconnectphase, ServerInstructionType.Unknown));
                    }
                    break;

                case ConnectionPhase.Connected:

                    if (data.StartsWith("]f") & InDream == true)
                    {
                        short length = 14;
                        if (Look)
                        {
                            LookQue.Enqueue(data.Substring(2));
                        }
                        else
                        {
                            length = ColorString.ColorStringSize;
                            // player = NameToFurre(data.Remove(0, length +
                            // 2)); If player.ID = 0 Then Exit Sub
                            player.Color = new ColorString(data.Substring(2, length));
                            if (IsConnectedCharacter)
                                Look = false;
                            if (Dream.FurreList.Contains(player))
                                Dream.FurreList[Dream.FurreList.IndexOf(player)] = player;
                        }
                    }
                    //Spawn Avatar
                    else if (data.StartsWith("<"))
                    {
                        var FurreSpawn = new SpawnAvatar(data);
                        player = FurreSpawn.player;
                        Dream.FurreList.Add(player);
                        if (IsConnectedCharacter && !dream.FurreList.Contains(player))
                        {
                            connectedFurre = player;
                        }
                        if (!FurreSpawn.PlayerFlags.HasFlag(CHAR_FLAG_NEW_AVATAR))
                        {
                            player = Dream.FurreList.GetFurreByID(Player.ID);
                        }

                        if (FurreSpawn.PlayerFlags.HasFlag(CHAR_FLAG_SET_VISIBLE))
                        {
                            // FURRE Bot = NameToFurre(botName);
                            //ViewArea VisableRectangle = getTargetRectFromCenterCoord(Bot.Position.x, Bot.Position.y);
                            //if (VisableRectangle.X <= player.Position.x & VisableRectangle.Y <=
                            //    player.Position.y & VisableRectangle.height >=
                            //    player.Position.y & VisableRectangle.length >= player.Position.x)
                            //{
                            //    player.Visible = true;
                            //}
                            //else
                            //{
                            //    player.Visible = false;
                            //}
                        }
                        //if (FurreSpawn.PlayerFlags.HasFlag(CHAR_FLAG_HAS_PROFILE))
                        //{
                        //}

                        if (InDream)
                        {
                            if (ProcessServerInstruction != null)
                            {
                                ProcessServerInstruction.Invoke(FurreSpawn,
                                new ParseServerArgs(ServerInstructionType.SpawnAvatar, serverconnectphase));
                            }
                        }
                    }
                    //Remove Furre
                    else if (data.StartsWith(")") || data.StartsWith(" "))
                    {
                        RemoveAvatar RemoveFurre = new RemoveAvatar(data);
                        Dream.FurreList.Remove(RemoveFurre.AvatarID);

                        if (InDream)
                        {
                            if (ProcessServerInstruction != null)
                            {
                                Handled = true;
                                ProcessServerInstruction.Invoke(RemoveFurre,
                                    new ParseServerArgs(ServerInstructionType.RemoveAvatar, serverconnectphase));
                            }
                        }
                    }
                    //Animated Move
                    else if (data.StartsWith("/"))
                    {
                        player = Dream.FurreList.GetFurreByID(data.Substring(1, 4));
                        player.Position.x = ConvertFromBase220(data.Substring(5, 2)) * 2;
                        player.Position.y = ConvertFromBase220(data.Substring(7, 2));
                        // player.Shape =
                        // ConvertFromBase220(data.Substring(9, 2));
                        connectedFurre = Dream.FurreList[connectedFurre];
                        ViewArea VisableRectangle = getTargetRectFromCenterCoord(connectedFurre.Position.x, connectedFurre.Position.y);
                        if (VisableRectangle.X <= player.Position.x & VisableRectangle.Y <= player.Position.y &
                            VisableRectangle.height >= player.Position.y & VisableRectangle.length >=
                            player.Position.x)
                        {
                            player.Visible = true;
                        }
                        else
                        {
                            player.Visible = false;
                        }
                        if (Dream.FurreList.Contains(player))
                            Dream.FurreList[player] = player;
                    }
                    // Move Avatar
                    else if (data.StartsWith("A"))
                    {
                        int id = ConvertFromBase220(data.Substring(1, 4));
                        player = Dream.FurreList.GetFurreByID(data.Substring(1, 4));
                        player.Position.x = ConvertFromBase220(data.Substring(5, 2)) * 2;
                        player.Position.y = ConvertFromBase220(data.Substring(7, 2));
                        // player.Shape =
                        // ConvertFromBase220(data.Substring(9, 2));

                        connectedFurre = Dream.FurreList[connectedFurre];
                        ViewArea VisableRectangle = getTargetRectFromCenterCoord(connectedFurre.Position.x, connectedFurre.Position.y);

                        if (VisableRectangle.X <= player.Position.x & VisableRectangle.Y <=
                            player.Position.y & VisableRectangle.height >= player.Position.y &
                            VisableRectangle.length >= player.Position.x)
                        {
                            player.Visible = true;
                        }
                        else
                        {
                            player.Visible = false;
                        }
                    }
                    //Update ColorString
                    else if (data.StartsWith("B"))
                    {
                        //fuid 4 b220 bytes
                        player = Dream.FurreList.GetFurreByID(data.Substring(1, 4));
                        UpdateColorString ColorStringUpdate = new UpdateColorString(ref player, data);

                        if (InDream)
                        {
                            if (ProcessServerInstruction != null)
                            {
                                ProcessServerInstruction.Invoke(ColorStringUpdate,
                                new ParseServerArgs(ServerInstructionType.UpdateColorString, serverconnectphase));
                            }
                        }
                    }
                    //Hide Avatar
                    else if (data.StartsWith("C") != false)
                    {
                        player = Dream.FurreList.GetFurreByID(data.Substring(1, 4));
                        player.Position.x = ConvertFromBase220(data.Substring(5, 2)) * 2;
                        player.Position.y = ConvertFromBase220(data.Substring(7, 2));
                        player.Visible = false;
                        if (Dream.FurreList.Contains(player))
                        {
                            Dream.FurreList[player] = player;
                        }
                        //Display Disconnection Dialog
                    }
                    // Species Tags
                    else if (data.StartsWith("]-"))
                    {
                        if (data.StartsWith("]-#A"))
                        {
                            SpeciesTag.Enqueue(data.Substring(4));
                        }
                        else if (data.StartsWith("]-#B"))
                        {
                            BadgeTag.Enqueue(data.Substring(2));
                        }

                        //DS Variables
                    }

                    //Popup Dialogs!
                    else if (data.StartsWith("]#"))
                    {
                        //]#<idstring> <style 0-17> <message that might have spaces in>
                        Regex repqq = new Regex("^\\]#(.*?) (\\d+) (.*?)$");
                        Match m = repqq.Match(data);
                        Rep r = default(Rep);
                        r.ID = m.Groups[1].Value;
                        int num = 0;
                        int.TryParse(m.Groups[2].Value, out num);
                        r.Type = num;
                        Repq.Enqueue(r);
                        player.Message = m.Groups[3].Value;

                        //]s(.+)1 (.*?) (.*?) 0
                    }

                    //Disconnection Error
                    else if (data.StartsWith("["))
                    {
#if DEBUG
                        Console.WriteLine("Disconnection Dialog:" + data);
#endif
                        inDream = false;
                        dream.FurreList.Clear();
                        dream.FurreList.Add(connectedFurre);
                        // RaiseEvent UpDateDreamList("")

                        //;{mapfile}	Load a local map (one in the furcadia folder)
                        //]q {name} {id}	Request to download a specific patch
                    }
                    //else if (data.StartsWith(";") || data.StartsWith("]r"))
                    //{
                    //}

                    //Dream Load ]q
                    //vasecodegamma
                    //
                    else if (data.StartsWith("]q"))
                    {
#if DEBUG

                        Console.WriteLine("Entering new Dream" + data);
#endif
                        //Set defaults (should move to some where else?
                        hasShare = false;
                        NoEndurance = false;
                        Dream.FurreList.Clear();
                        dream.FurreList.Add(connectedFurre);
                        inDream = false;

                        LoadDream loadDream = new LoadDream(data);
                        if (ProcessServerInstruction != null)
                        {
                            ProcessServerInstruction.Invoke(loadDream,
                            new ParseServerArgs(ServerInstructionType.LoadDreamEvent, serverconnectphase));
                        }

                        // Set Proxy to Stand-Alone Operation
                        if (!IsClientConnected && StandAlone)
                        {
                            SendToServer("vasecodegamma");
                            inDream = true;
                        }
                    }
                    else if (data.StartsWith("]z"))
                    {
                        int ID = int.Parse(data.Remove(0, 2));
                        if (ConnectedCharacterFurcadiaID == 0)
                            ConnectedCharacterFurcadiaID = ID;
                        //Snag out UID
                    }
                    else if (data.StartsWith("]B"))
                    {
                        int ID = int.Parse(data.Substring(2, data.Length - connectedFurre.Name.Length - 3));
                        if (ConnectedCharacterFurcadiaID == 0)
                            ConnectedCharacterFurcadiaID = ID;
                    }
                    else if (data.StartsWith("]c"))
                    {
#if DEBUG
                        Console.WriteLine(data);
#endif
                    }
                    // Dream Bookmark
                    //]CBookmark Type[1]Dream URL[*]
                    // Type 0 = temporary
                    // Type 1 = Regular (per user requests)
                    // DreamUrl = "furc://uploadername:dreamname/entrycode "
                    // Credits FTR
                    else if (data.StartsWith("]C"))
                    {
                        if (data.StartsWith("]C0") || data.StartsWith("]C1"))
                        {
                            inDream = true;
                            string dname = data.Substring(10);
                            Dream.Name = dname;
                            Dream.URL = data.Substring(3);
                            if (dname.Contains(":"))
                            {
                                string NameStr = dname.Substring(0, dname.IndexOf(":"));
                                Dream.Owner = NameStr;
                                if (FurcadiaShortName(NameStr) == connectedFurre.ShortName)
                                {
                                    hasShare = true;
                                }
                            }
                            else if (dname.EndsWith("/") && !dname.Contains(":"))
                            {
                                string NameStr = dname.Substring(0, dname.IndexOf("/"));
                                Dream.Owner = NameStr;
                                if (FurcadiaShortName(NameStr) == connectedFurre.ShortName)
                                {
                                    hasShare = true;
                                }
                            }
                        }
#if DEBUG
                        Console.WriteLine(data);
#endif
                    }

                    //Process Channels Seperatly
                    else if (data.StartsWith("("))
                    {
                        channel = "";
                        if (ThroatTired == false & data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>"))
                        {
                            //Using Furclib ServQue
                            ThroatTired = true;

                            //(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                        }
                        try
                        {
                            ParseServerChannel(data, Handled);
                        }
                        catch (Exception ex)
                        {
                            SendError(ex, this, "");
                        }

                        return;
                    }

                    break;

                case ConnectionPhase.Disconnected:
                    // Do nothing - we're disconnected...
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// Format basic furcadia commands and send to server
        /// <para>
        /// We also mirror the client to server banish system.
        /// </para>
        /// <para>
        /// This maybe a good place to place Proxy/Bot commands for controls
        /// </para>
        /// <para>
        /// default to say or "normal spoken command"
        /// </para>
        /// </summary>
        /// <param name="data">
        /// Raw Client to Server instruction
        /// </param>
        public virtual void SendFormattedTextToServer(ref string data)
        {
            if (data.StartsWith("banish "))
            {
                BanishName = data.Substring(7);
            }
            else if (data.StartsWith("banish-off ") | data.StartsWith("tempbanish "))
            {
                BanishName = data.Substring(11);
            }
            else if (data == "banish-list")
            {
                BanishName = "";
            }

            TextToServer(ref data);
        }

        /// <summary>
        /// Send a raw instruction to the client
        /// </summary>
        /// <param name="data">
        /// </param>
        public override void SendToClient(string data)
        {
            if (IsClientConnected && clientconnectionphase != ConnectionPhase.Disconnected)
                base.SendToClient(data);
        }

        /// <summary>
        /// Send a raw instruction to Server through the Load Balancer
        /// </summary>
        /// <param name="message">
        /// Client to server Instruction
        /// </param>
        public override void SendToServer(string message)
        {
            if (serverconnectphase != ConnectionPhase.Disconnected)
                ServerBalancer.SendToServer(ref message);
        }

        /// <summary>
        /// Text Channel Prefixes (shout,whisper emote, Raw Server command)
        /// <para>
        /// default to say or "normal spoken command"
        /// </para>
        /// </summary>
        /// <param name="arg">
        /// </param>
        public void TextToServer(ref string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
                return;
            //Clean Text input to match Client
            string result = "";
            switch (arg.Substring(0, 1))
            {
                case "`":
                    result = arg.Remove(0, 1);
                    break;

                case "/":
                    result = "wh " + arg.Substring(1);
                    break;

                case ":":
                    result = arg;

                    break;

                case "-":
                    result = arg;

                    break;

                default:
                    result = "\"" + arg;
                    break;
            }
            SendToServer(result);
        }

        private void onClientConnected()
        {
            clientconnectionphase = ConnectionPhase.MOTD;
            ClientStatusChanged?.Invoke(this, new NetClientEventArgs(clientconnectionphase));
        }

        /// <summary>
        /// Client sent us some data, Let's deal with it
        /// </summary>
        /// <param name="data">
        /// </param>
        private void onClientDataReceived(string data)
        {
            //TODO Raise Client Data Received event

            //lock (clientlock)
            //{
            switch (clientconnectionphase)
            {
                case ConnectionPhase.Auth:

                    if (data.StartsWith("connect"))
                    {
                        string test = data.Replace("connect ", "").TrimStart(' ');
                        string botName = test.Substring(0, test.IndexOf(" "));
                        if (connectedFurre == null)
                            connectedFurre = new FURRE(botName);
                        else
                            connectedFurre.Name = botName;
                    }
                    else if (data.ToLower().StartsWith("account"))
                    {
                        string[] words = data.Split(' ');
                        if (connectedFurre == null)
                            connectedFurre = new FURRE(words[2]);
                        else
                            connectedFurre.Name = words[2];
                    }
                    if (data == "vascodagama")
                    {
                        clientconnectionphase = ConnectionPhase.Connected;
                        ClientStatusChanged?.Invoke(this, new NetClientEventArgs(clientconnectionphase));
                        if (options.Standalone)
                        {
                            CloseClient();
                        }
                    }
                    ClientData2?.Invoke(data);
                    break;

                // Disconnected? Do Nohing
                case ConnectionPhase.Disconnected:
                    break;

                case ConnectionPhase.Connected:
                    // Snag the quit command from the client so we can
                    // handle the connection properly
                    if (data == "quit")
                    {
                        clientconnectionphase = ConnectionPhase.Disconnected;
                        ClientStatusChanged?.Invoke(this, new NetClientEventArgs(clientconnectionphase));
                        if (options.Standalone)
                        {
                            ClientDisconnect();
                        }
                        else
                            Disconnect();
                    }
                    if (IsClientConnected)
                        ClientData2?.Invoke(data);
                    break;

                case ConnectionPhase.Connecting:
                default:
                    ClientData2?.Invoke(data);
                    break;
            }
        }

        private void onClientDisconnected()
        {
            clientconnectionphase = ConnectionPhase.Disconnected;
            ClientStatusChanged?.Invoke(this, new NetClientEventArgs(clientconnectionphase));
        }

        private void onServerConnected()
        {
            serverconnectphase = ConnectionPhase.MOTD;
            ServerStatusChanged?.Invoke(this, new NetServerEventArgs(serverconnectphase, ServerInstructionType.Unknown));
        }

        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        private void onServerDataReceived(string data)
        {
            player = new FURRE();
            bool handled = false;
            ParseServerData(data, handled);

            if (!handled)
                ServerData2?.Invoke(data);
        }

        #region "Dice Rolls"

        private string diceCompnentMatch;

        private double diceCount = 0.0;

        private double diceModifyer = 0.0;
        private double diceResult = 0.0;
        private double diceSides = 0.0;
        //TODO Check MS Engine Dice lines

        #endregion "Dice Rolls"

        #region "Popup Dialogs"

        /// <summary>
        /// </summary>
        public Queue<Rep> Repq = new Queue<Rep>();

        //TODO Check Furcadoia Popup Windows
        /// <summary>
        /// </summary>
        public struct Rep
        {
            #region "Public Fields"

            private string iD;

            private int type;

            /// <summary>
            /// </summary>
            public string ID { get { return iD; } set { iD = value; } }

            /// <summary>
            /// </summary>
            public int Type { get { return type; } set { type = value; } }

            #endregion "Public Fields"
        }

        #endregion "Popup Dialogs"

        private void onServerDisconnected()
        {
            serverconnectphase = ConnectionPhase.Disconnected;
            ServerStatusChanged?.Invoke(null, new NetServerEventArgs(serverconnectphase, ServerInstructionType.Unknown));
        }

        private void onServerQueSent(object o, EventArgs e)
        {
            base.SendToServer(o.ToString());
        }

        #region "Dispose"

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                base.Dispose();
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        #endregion "Dispose"
    }
}