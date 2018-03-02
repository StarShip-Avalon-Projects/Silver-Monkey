// Furcadia Servver Parser
// Event/Delegates for server instructions
// call subsystem processor
// dream info
// Furre info
// Bot info
// Furre Update events?

using Furcadia.Net;
using Furcadia.Net.DreamInfo;
using Furcadia.Net.Proxy;
using Furcadia.Net.Utils.ServerParser;
using Libraries;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using Monkeyspeak.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Furcadia.Text.FurcadiaMarkup;
using static Libraries.MsLibHelper;

namespace Engine.BotSession
{
    ///  <summary>
    /// This Instance handles the current Furcadia Session.
    /// <para>
    /// Part1: Manage MonkeySpeak Engine Start,Stop,Restart. System Variables,
    /// MonkeySpeak Execution Triggers
    /// </para><para>
    /// Part2: Furcadia Proxy Controls, In/Out Ports, Host, Character Ini file.
    ///Connect, Disconnect, Reconnect
    ///</para><para>
    ///Part2a: Proxy Functions do link to Monkey Speak trigger execution
    ///</para><para>
    /// Part3: This Class Links loosley to the GUI
    /// </para>
    /// </summary>
    /// <seealso cref="Furcadia.Net.Proxy.ProxySession" />
    public class Bot : ProxySession
    {
        #region Public Fields

        /// <summary>
        /// Monkey Speak Page object
        /// </summary>
        public Page MSpage;

        #endregion Public Fields

        #region Private Fields

        private BotOptions _options;

        private Dream lastDream;

        /// <summary>
        /// Main MonkeySpeak Engine
        /// </summary>
        private MonkeyspeakEngine MsEngine;

        private EngineOptoons MsEngineOptions;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="BotSessionOptions">The bot session options.</param>
        public Bot(BotOptions BotSessionOptions) : base(BotSessionOptions)
        {
            SetOptions(BotSessionOptions);
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        public Bot()
        {
            SetOptions(new BotOptions());
            this.Initialize();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Name of the controller furre
        /// </summary>
        /// <value>
        /// The bot controller.
        /// </value>
        public string BotController => MsEngineOptions.BotController;

        /// <summary>
        /// Gets or sets a value indicating whether The Monkey
        /// Speak engine is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [engine enable]; otherwise, <c>false</c>.
        /// </value>
        public bool EngineEnable
        {
            get => MsEngineOptions.IsEnabled;
            set => MsEngineOptions.IsEnabled = value;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is bot controller.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is bot controller; otherwise, <c>false</c>.
        /// </value>
        public bool IsBotController => ConnectedFurre.ShortName == BotController.ToFurcadiaShortName();

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Connets to the game server asyncronously.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Can be thrown if there is no Monkey Speak File or Character Ini file supplied
        /// </exception>
        public async Task ConnetAsync()
        {
            if (EngineEnable)
            {
                StartEngine();
            }
            await Task.Run(() => Connect());
        }

        /// <summary>
        /// Gets the silver monkey settings.
        /// </summary>
        /// <returns></returns>
        public BotOptions GetOptions()
        {
            return _options;
        }

        /// <summary>
        /// Pages the set variable.
        /// </summary>
        /// <param name="VariableList">The variable list.</param>
        public void PageSetVariable(List<IVariable> VariableList)
        {
            foreach (var variable in VariableList)
            {
                MSpage.SetVariable(variable);
            }
        }

        /// <summary>
        /// Send a formatted string to the client and log window
        /// </summary>
        /// <param name="msg">
        /// Channel Subsystem?
        /// </param>
        /// <param name="data">
        /// %MESSAGE to send
        /// </param>
        public void SendToClientFormattedText(string msg, string data)
        {
            SendToClient($"(<b><i>[SM]</i> - {msg}:</b> \"{data}\"");
        }

        /// <summary>
        /// Sets the silver monkey settings.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetOptions(BotOptions value)
        {
            _options = value;
            base.Options = _options;
            MsEngineOptions = _options.MonkeySpeakEngineOptions;
        }

        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void StopEngine()
        {
            if (MSpage != null)
            {
                MSpage.Reset(true);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            lastDream = new Dream();
            MsEngineOptions = GetOptions().MonkeySpeakEngineOptions;

            ClientData2 += ClintData => SendToServer(ClintData);
            ServerData2 += ServerData => SendToClient(ServerData);
            ProcessServerChannelData += (s, o) => OnServerChannel(s, o);
            ProcessServerInstruction += (s, o) => OnParseSererInstructionAsync(s, o);
            ServerStatusChanged += (s, o) => OnServerStatusChanged(s, o);
            ClientStatusChanged += (s, o) => OnClientStatusChanged(s, o);

            TroatTiredEventHandler += e => MSpage.Execute(new int[] { 5, 6 }, e);

#if DEBUG
            if (!Debugger.IsAttached)
            {
                Logger.Disable<Bot>();
            }
#else
        Logger.Disable<Bot>();
#endif
        }

        /// <summary>
        /// Initializes the engine libraries.
        /// </summary>
        /// <returns></returns>
        private List<BaseLibrary> InitializeEngineLibraries()
        {
            var LibList = new List<BaseLibrary>()
            {
                new MsStartBot(),
                new Monkeyspeak.Libraries.IO(GetOptions().BotPath),
                new Monkeyspeak.Libraries.Math(),
                new StringOperations(),
                new Sys(),
                new Timers(100),
                new Loops(),
                new Tables(),
                new MsDreamInfo(),
                new MsBotInformation(),
                new MsSayLibrary(),
                new MsQuery(),
                new MsBanish(),
                new MsStringExtentions(),
                new MsDatabase(),
                new MsWebRequests(),
                new MsCookie(),
                new MsDice(),
                new MsFurres(),
                new MsMovement(),
                new MsBotMessage(),
                new MsMemberList(),
                new MsSound(),
                new MsPhoenixSpeak(),
                new MsPounce(),
                new MsPhoenixSpeakBackupAndRestore(),
            };
            return LibList;
        }

        private Page LoadLibrary(bool silent)
        {
            // Library Loaded?.. Get the Hell out of here

            // " When the Monkey Speak Engine starts,"
            var LibList = InitializeEngineLibraries();
            foreach (BaseLibrary Library in LibList)
            {
                try
                {
                    MSpage.LoadLibrary(Library, this);
                    if (!silent)
                    {
                        Logger.Info($"{Library.GetType().Name}");
                    }
                    Logger.Debug<Bot>($"{Library.GetType().Name}");
                }
                catch (Exception e)
                {
                    SendError(e, null);
                }
            }

            return MSpage;
        }

        private void OnClientStatusChanged(object Sender, NetClientEventArgs e)
        {
            if (MSpage == null || !EngineEnable) return;

            try
            {
                switch (e.ConnectPhase)
                {
                    case ConnectionPhase.Connected:
                        var b = (ConstantVariable)MSpage.GetVariable(BotNameVariable);
                        b.SetValue(ConnectedFurre.Name);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error<Bot>(ex);
            }
        }

        /// <summary>
        /// Pump MonkeySpeak Exceptions to the error handler
        /// </summary>
        /// <param name="page">The <see cref="Page"/>.</param>
        /// <param name="handler">The <see cref="TriggerHandler"/></param>
        /// <param name="trigger">The <see cref="Trigger"/></param>
        /// <param name="ex">The ex.</param>
        private void OnMonkeySpeakError(Page page, TriggerHandler handler, Trigger trigger, Exception ex)
        {
            if (ex.GetType() != typeof(MonkeyspeakException))
            {
                var PageError = new MonkeyspeakException($"Trigger Error: {trigger} {ex}");
                SendError(PageError, handler);
            }
            else
            {
                SendError(ex, handler);
            }
        }

        private async void OnParseSererInstructionAsync(object sender, ParseServerArgs e)
        {
            if (MSpage == null || !EngineEnable) return;

            CancellationToken cancel = new CancellationTokenSource(800).Token;

            try
            {
                Logger.Debug<Bot>((BaseServerInstruction)sender);
                switch (e.ServerInstruction)
                {
                    case ServerInstructionType.LoadDreamEvent:
                        // (0:36) When the bot leaves a Dream,
                        // (0:37) When the bot leaves the Dream named {..},

                        await MSpage.ExecuteAsync(new int[] { 36, 37 }, cancel, lastDream);
                        return;

                    case ServerInstructionType.BookmarkDream:
                        // (0:34) When the bot enters a Dream,
                        // (0:35) When the bot enters the Dream named {..},
                        await MSpage.ExecuteAsync(new int[] { 34, 35 }, cancel, Dream);
                        lastDream = Dream;
                        return;

                    case ServerInstructionType.MoveAvatar:
                    case ServerInstructionType.AnimatedMoveAvatar:
                        // (0:600) When anyone enters the bots view,
                        // (0:601) When a furre named {..} enters the bots view,
                        // (0:602) When anyone leaves the bots view,
                        // (0:603) When a furre named {..} leaves the bots view,
                        // (0:605) When a furre moves,
                        // (0:606) when a furre moves into(x, y),
                        await MSpage.ExecuteAsync(new int[] { 600, 601, 602, 603, 60, 605, 606 },
                            cancel, ((MoveFurre)sender).Player);
                        return;

                    case ServerInstructionType.RemoveAvatar:
                        // (0:32) When anyone leaves the Dream,
                        // (0:33) When a furre named {..} leaves the Dream,
                        await MSpage.ExecuteAsync(new int[] { 32, 33 }, cancel, ((RemoveAvatar)sender).Player);
                        return;

                    case ServerInstructionType.SpawnAvatar:
                        // (0:30) When anyone enters the Dream,
                        // (0:31) When the furre named {..} enters the Dream,
                        await MSpage.ExecuteAsync(new int[] { 30, 31 }, cancel, ((SpawnAvatar)sender).player);
                        return;

                    case ServerInstructionType.UpdateColorString:
                        var fur = ConnectedFurre;
                        return;

                    case ServerInstructionType.LookResponse:
                        // (0:600) When the bot reads a description.
                        //   Await MSpage.ExecuteAsync(600, DirectCast(sender, SpawnAvatar).player)
                        return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error<Bot>(ex);
            }
        }

        private async void OnServerChannel(object sender, ParseChannelArgs Args)
        {
            if (MSpage == null || !EngineEnable) return;

            CancellationToken cancel = new CancellationTokenSource(TimeSpan.FromSeconds(4)).Token;

            try
            {
                if (sender is ChannelObject ChanObject)
                {
                    string Text = ChanObject.ChannelText;
                    string data = ChanObject.RawInstruction;
                    Furre Furr = (Furre)ChanObject.Player;
                    switch (Args.Channel)
                    {
                        case "@roll":
                            if (IsConnectedCharacter(Furr))
                            {
                                // (0:130) When the bot rolls #d#,
                                // (0:131) When the bot rolls #d#+#,
                                // (0:132) When the bot rolls #d#-#,
                                // (0:136) When any one rolls anything,

                                await MSpage.ExecuteAsync(new int[] { 130, 131, 132, 136 },
                                    cancel, Furr);
                            }
                            else
                            {
                                // (0:133) When a furre rolls #d#,
                                // (0:134) When a furre rolls #d#+#,
                                // (0:135) When a furre rolls #d#-#,
                                // (0:136) When any one rolls anything,
                                await MSpage.ExecuteAsync(new int[] { 133, 134, 135, 136 },
                                    cancel, Furr);
                            }

                            return;

                        case "trade":

                            // await MSpage.ExecuteAsync(new int[] { 46, 46, 48 }, cancel, Furr);
                            return;

                        case "shout":
                            // (0:13) When anyone shouts something,
                            // (0:14) When anyone shouts {..},
                            // (0:15) When anyone shouts something with {..} in it,
                            await MSpage.ExecuteAsync(new int[] { 13, 14, 15 },
                                cancel, Furr);
                            return;

                        case "myspeech":
                            break;

                        case "whisper":
                            // (0:19) When anyone whispers something,
                            // (0:20) When anyone whispers {..},
                            // (0:21) When anyone whispers something with {..} in it,
                            await MSpage.ExecuteAsync(new int[] { 19, 20, 21 }, cancel, Furr);
                            return;

                        case "say":
                            // (0:10) When anyone says something,
                            // (0:11) When anyone says {..},
                            // (0:12) When anyone says something with {..} in it,

                            // (0:22) When anyone says or emotes something,
                            // (0:23) When anyone says or emotes {..},
                            // (0:24) When anyone says or emotes something with {..} in it,

                            await MSpage.ExecuteAsync(new int[] { 10, 11, 12, 22, 23, 24 }, cancel, Furr);
                            return;

                        case "emote":
                            if (IsConnectedCharacter(Furr))
                            {
                                return;
                            }

                            // (0:16) When anyone emotes something,
                            // (0:17) When anyone emotes {..},
                            // (0:18) When anyone emotes something with {..} in it,

                            // (0:22) When anyone says or emotes something,
                            // (0:23) When anyone says or emotes {..},
                            // (0:24) When anyone says or emotes something with {..} in it,

                            await MSpage.ExecuteAsync(new int[] { 16, 17, 18, 22, 23, 24 }, cancel, Furr);
                            return;

                        case "@emit":
                            // (0:25) When someone emits something,
                            // (0:26) When someone emits {..},
                            // (0:27) When someone emits something with {..} in it,
                            await MSpage.ExecuteAsync(new int[] { 24, 26, 27 }, cancel, Furr, Dream);
                            return;

                        case "query":
                            // (0:40) When anyone requests to summon the bot,
                            // (0:41) When a furre named {..} requests to summon the bot,
                            // (0:42) When anyone requests to join the bot,
                            // (0:43) When a furre named {..} requests to join the bot,
                            // (0:44) When anyone requests to follow the bot,
                            // (0:35) When a furre named {..} requests to follow the bot,
                            // (0:46) When anyone requests to lead the bot,
                            // (0:47) When a furre named {..} requests to lead the bot,
                            // (0:48) When anyone requests to cuddle with the bot.
                            // (0:49) When a furre named {..} requests to cuddle with the bot,
                            // (0:50) When the bot see a query (lead, follow summon, join, cuddle),
                            await MSpage.ExecuteAsync(new int[] { 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 },
                                cancel, ChanObject, ChanObject.Player);
                            return;

                        case "banish":

                            //TODO: Add these lines
                            // (0:60) When the bot fails to empty the banish list,

                            if (Text.Contains(" has been banished from your dreams."))
                            {
                                // banish <name> (online)
                                // Success: (.*?) has been banished from your dreams.

                                // (0:53) When the bot successfully banishes a furre,
                                // (0:54) When the bot successfully banishes the furre named {...},

                                await MSpage.ExecuteAsync(new int[] { 53, 54 },
                                    cancel, BanishName);
                            }
                            else if (Text == "You have canceled all banishments from your dreams.")
                            {
                                // banish-off-all (active list)
                                // Success: You have canceled all banishments from your dreams.
                                // (0:61) When the bot successfully clears the banish list,

                                await MSpage.ExecuteAsync(61,
                                    cancel, BanishList);
                            }
                            else if (Text.EndsWith(" has been temporarily banished from your dreams."))
                            {
                                // tempbanish <name> (online)
                                // Success: (.*?) has been temporarily banished from your dreams.
                                // (0:62) When the bot successfully temp banishes a furre,
                                // (0:63) When the bot successfully temp banishes the furre named {...},

                                await MSpage.ExecuteAsync(new int[] { 62, 63 },
                                    cancel, BanishName);
                            }
                            else if (Text.StartsWith("Players banished from your dreams: "))
                            {
                                // Banish-List
                                // [notify> Players banished from your dreams:
                                // (0:55) When the bot sees the banish list,
                                await MSpage.ExecuteAsync(55,
                                    cancel, BanishList);
                            }
                            else if (Text.StartsWith("The banishment of player "))
                            {
                                // banish-off <name> (on list)
                                // [notify> The banishment of player (.*?) has ended.
                                // (0:58) When the bot successfully removes a furre from the banish list,
                                // (0:59) When the bot successfully removes the furre named {...} from the banish list,

                                Regex t = new Regex("The banishment of player (.*?) has ended.", RegexOptions.Compiled);
                                string NameStr = t.Match(Text).Groups[1].Value;
                                await MSpage.ExecuteAsync(new int[] { 58, 59 },
                                    cancel, BanishName);
                            }
                            else if (Text.Contains("There are no furres around right now with a name starting with "))
                            {
                                // Banish <name> (Not online)
                                // Error:>>  There are no furres around right now with a name starting with (.*?) .
                                // (0:56) When the bot fails to remove a furre from the banish list,
                                //  (0:57) When the bot fails to remove the furre named {...} from the banish list,

                                Regex t = new Regex("There are no furres around right now with a name starting with (.*?) .", RegexOptions.Compiled);
                                string NameStr = t.Match(Text).Groups[1].Value;
                                await MSpage.ExecuteAsync(new int[] { 56, 57 },
                                    cancel, NameStr);
                            }
                            else if (Text == "Sorry, this player has not been banished from your dreams.")
                            {
                                // banish-off <name> (not on list)
                                // Error:>> Sorry, this player has not been banished from your dreams.
                                // (0:51) When the bot fails to banish a furre,
                                // (0:52) When the bot fails to banish the furre named {...},

                                await MSpage.ExecuteAsync(new int[] { 51, 52 },
                                    cancel, BanishName);
                            }
                            else if (Text == "You have not banished anyone.")
                            {
                                // banish-off-all (empty List)
                                // Error:>> You have not banished anyone.
                                // (0:55) When the bot sees the banish list,

                                await MSpage.ExecuteAsync(55,
                                    cancel, BanishList);
                            }
                            return;

                        case "@cookie":
                            var CookieToMe = new Regex(CookieToMeREGEX);
                            if (CookieToMe.Match(Text).Success)
                            {
                                //  await MSpage.ExecuteAsync(new int[] { 42, 43 }, cancel, Furr);
                            }

                            Regex CookieToAnyone = new Regex(string.Format("<name shortname=\'(.*?)\'>(.*?)</name> just gave <name shortname=\'(.*?)\'>(.*?)</name> a (.*?)", RegexOptions.Compiled));
                            if (CookieToAnyone.Match(Text).Success)
                            {
                                if (IsConnectedCharacter(Furr))
                                {
                                    //  await MSpage.ExecuteAsync(new int[] { 42, 43 }, cancel, Furr);
                                }
                                else
                                {
                                    //   await MSpage.ExecuteAsync(44, cancel, Furr);
                                }
                            }

                            var CookieFail = new Regex("You do not have any (.*?) left!", RegexOptions.Compiled);
                            if (CookieFail.Match(Text).Success)
                            {
                                // await MSpage.ExecuteAsync(45, cancel, Furr);
                            }

                            var EatCookie = new Regex((Regex.Escape("<img src=\'fsh://system.fsh:90\' alt=\'@cookie\'/><channel name=\'@cookie\'/> You eat a cookie.") + "(.*?)"), RegexOptions.Compiled);
                            if (EatCookie.Match(Text).Success)
                            {
                                //  await MSpage.ExecuteAsync(49, cancel, Furr);
                            }

                            // (0:96) When the Bot sees "Your cookies are ready."
                            Regex CookiesReady = new Regex($"<Your cookies are ready.http://furcadia.com/cookies/ for more info!", RegexOptions.Compiled);
                            if (CookiesReady.Match(Text).Success)
                            {
                                //  await MSpage.ExecuteAsync(96, cancel, Furr);
                            }

                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error<Bot>(ex);
            }
        }

        private async void OnServerStatusChanged(object Sender, NetServerEventArgs e)
        {
            if (MSpage == null || !EngineEnable) return;

            CancellationToken cancel = new CancellationTokenSource(800).Token;

            try
            {
                switch (e.ConnectPhase)
                {
                    case ConnectionPhase.Disconnected:
                        // (0:2) When the bot logs out of furcadia,

                        if (MSpage != null)
                        {
                            await MSpage.ExecuteAsync(2, cancel);
                            StopEngine();
                        }
                        return;

                    case ConnectionPhase.Connected:
                        // (0:1) When the bot logs into furcadia,

                        await MSpage.ExecuteAsync(1, cancel, this);
                        return;
                }
            }
            catch (Exception ex)
            { SendError(ex, this); }
        }

        /// <summary>
        /// Starts the engine.
        /// </summary>
        /// <returns></returns>
        private void StartEngine()
        {
            var TimeStart = DateTime.Now;
            CancellationToken cancel = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;

            MsEngine = new MonkeyspeakEngine(GetOptions().MonkeySpeakEngineOptions);
            string MonkeySpeakScript = MsEngineExtentionFunctions.LoadFromScriptFile(MsEngineOptions.MonkeySpeakScriptFile, MsEngine.Options.Version);
            MSpage = MsEngine.LoadFromString(MonkeySpeakScript);

            List<IVariable> VariableList = new List<IVariable>();
            MSpage = LoadLibrary(false);
            IFurre fur = new Furre();
            VariableList.Add(new ConstantVariable(DreamOwnerVariable, Dream.DreamOwner));
            VariableList.Add(new ConstantVariable(DreamNameVariable, Dream.Name));
            VariableList.Add(new ConstantVariable(BotNameVariable, ConnectedFurre.Name));
            VariableList.Add(new ConstantVariable(BotControllerVariable, GetOptions().MonkeySpeakEngineOptions.BotController));
            VariableList.Add(new ConstantVariable(TriggeringFurreNameVariable, fur.Name));
            VariableList.Add(new ConstantVariable(TriggeringFurreShortNameVariable, fur.ShortName));
            VariableList.Add(new ConstantVariable(MessageVariable, fur.Message));
            VariableList.Add(new ConstantVariable(BanishNameVariable, null));
            VariableList.Add(new ConstantVariable(BanishListVariable, null));
            PageSetVariable(VariableList);
            // (0:0) When the bot starts,
            MSpage.Execute(0, this);
            Logger.Info($"Done!!! Loaded {MSpage.Size} triggers in {DateTime.Now.Subtract(TimeStart).Milliseconds} miliseconds.");
        }

        #endregion Private Methods
    }
}