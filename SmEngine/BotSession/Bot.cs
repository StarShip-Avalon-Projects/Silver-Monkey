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

using Monkeyspeak;
using Monkeyspeak.Libraries;
using Monkeyspeak.Logging;
using SilverMonkey.Engine.Libraries;
using SilverMonkey.Engine.Libraries.Engine.Libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Furcadia.Text.FurcadiaMarkup;
using static MsLibHelper;

namespace BotSession
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
        public Bot(BotOptions BotSessionOptions) :
                base(BotSessionOptions)
        {
#if DEBUG
            // TODO: #If Then ... Warning!!! not translated
            if (!Debugger.IsAttached)
            {
                Logger.Disable<Bot>();
            }
#else
        // TODO: # ... Warning!!! not translated
        Logger.Disable<Bot>();
#endif
            SetOptions(BotSessionOptions);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        public Bot()
        {
#if DEBUG
            // TODO: #If Then ... Warning!!! not translated
            if (!Debugger.IsAttached)
            {
                Logger.Disable<Bot>();
            }
#else
        // TODO: # ... Warning!!! not translated
        Logger.Disable<Bot>();
#endif
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
        public string BotController
        {
            get
            {
                return MsEngineOptions.BotController;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is bot controller.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is bot controller; otherwise, <c>false</c>.
        /// </value>
        public bool IsBotController
        {
            get
            {
                return ConnectedFurre.ShortName == BotController.ToFurcadiaShortName();
            }
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
        /// Sets the silver monkey settings.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetOptions(BotOptions value)
        {
            _options = value;
            base.Options = _options;
            MsEngineOptions = _options.MonkeySpeakEngineOptions;
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Bots the session.
        /// </summary>
        public void BotSession()
        {
            _options = new BotOptions();
            MsEngineOptions = _options.MonkeySpeakEngineOptions;
            Initialize();
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
        /// Stops the engine.
        /// </summary>
        public void StopEngine()
        {
            //   RemoveHandler ProcessServerChannelData, Me
            if (MSpage != null)
            {
                MSpage.Reset(true);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void OnClientStatusChanged(object Sender, NetClientEventArgs e)
        {
            if ((MSpage == null))
            {
                return;
            }

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
        /// Connets to the game server asyncronously.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Can be thrown if there is no Monkey Speak File or Character Ini file supplied
        /// </exception>
        public async Task ConnetAsync()
        {
            if (MsEngineOptions.IsEnabled)
            {
                await StartEngine();
            }

            await Task.Run(() => Connect());
        }

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
        }

        /// <summary>
        /// Initializes the engine libraries.
        /// </summary>
        /// <returns></returns>
        private List<BaseLibrary> InitializeEngineLibraries()
        {
            var LibList = new List<BaseLibrary>()
            {
                new IO(GetOptions().BotPath),
                new Monkeyspeak.Libraries.Math(),
                new StringOperations(),
                new Sys(),
                new Timers(100),
                new Loops(),
                new Tables(),
                new MsDreamInfo(),
                new MsBotInformation(),
                new MsSayLibrary(),
                new MsBanish(),
                new MsDatabase(),
                new MsWebRequests(),
                new MsCookie(),
                new MsDice(),
                new MsFurres(),
                new MsMovement(),
                new WmCpyDta(),
                new MsMemberList(),
                new MsSound(),
                new MsTrades(),
                //new MsPhoenixSpeak(),
                //  new MsPounce(),
                // new StringLibrary(),
                //  New MsPhoenixSpeakBackupAndRestore(Me),
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
                        Furcadia.Logging.Logger.Info($"{Library.GetType().Name}");
                    }
                }
                catch (Exception ex)
                {
                    throw new MonkeyspeakException($"{Library.GetType().Name} {ex}", ex);
                }
            }

            return MSpage;
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
            CancellationTokenSource cancel = new CancellationTokenSource(500);
            if ((MSpage == null))
            {
                return;
            }

            try
            {
                Logger.Debug<Bot>((BaseServerInstruction)sender);
                switch (e.ServerInstruction)
                {
                    case ServerInstructionType.LoadDreamEvent:
                        // (0:97) When the bot leaves a Dream,
                        // (0:98) When the bot leaves the Dream named {..},
                        await MSpage.ExecuteAsync(new int[] { 97, 98 }, cancel.Token, lastDream);
                        break;

                    case ServerInstructionType.BookmarkDream:
                        // (0:90) When the bot enters a Dream,
                        // (0:91) When the bot enters a Dream named {..},
                        await MSpage.ExecuteAsync(new int[] { 90, 91 }, cancel.Token, Dream);
                        lastDream = Dream;
                        break;

                    case ServerInstructionType.MoveAvatar:
                    case ServerInstructionType.AnimatedMoveAvatar:
                        // (0:28) When someone enters the bots view,
                        // (0:30) When someone leaves the bots view,
                        // (0:31) When a furre named {..} leaves the bots view,
                        // (0:29) When a furre named {..} enters the bots view,

                        await MSpage.ExecuteAsync(new int[] { 28, 30, 31, 29 }, cancel.Token, ((MoveFurre)sender).Player);
                        break;

                    case ServerInstructionType.RemoveAvatar:
                        // (0:27) When a furre named {..} leaves the Dream,
                        // (0:26) When someone leaves the Dream,
                        await MSpage.ExecuteAsync(new int[] { 27, 26 }, cancel.Token, ((RemoveAvatar)sender).Player);
                        break;

                    case ServerInstructionType.SpawnAvatar:
                        // (0:24) When someone enters the Dream,
                        // (0:25) When a furre Named {..} enters the Dream,
                        await MSpage.ExecuteAsync(new int[] { 24, 25 }, cancel.Token, ((SpawnAvatar)sender).player);
                        break;

                    case ServerInstructionType.UpdateColorString:
                        var fur = ConnectedFurre;
                        break;

                    case ServerInstructionType.LookResponse:
                        // (0:600) When the bot reads a description.
                        //   Await MSpage.ExecuteAsync(600, DirectCast(sender, SpawnAvatar).player)
                        break;

                    case ServerInstructionType.Unknown:
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error<Bot>(ex);
            }
        }

        private async void OnServerChannel(object sender, ParseChannelArgs Args)
        {
            CancellationTokenSource cancel = new CancellationTokenSource(500);
            if ((MSpage == null))
            {
                return;
            }

            if (!MsEngineOptions.IsEnabled)
            {
                return;
            }

            ChannelObject InstructionObject = (ChannelObject)sender;
            Furre Furr = InstructionObject.Player;
            string Text = InstructionObject.ChannelText;
            try
            {
                switch (Args.Channel)
                {
                    case "@roll":
                        if (IsConnectedCharacter(Furr))
                        {
                            // (0:130) When the bot rolls #d#,
                            // (0:132) When the bot rolls #d#+#,
                            // (0:134) When the bot rolls #d#-#,
                            // (0:136) When any one rolls anything,

                            await MSpage.ExecuteAsync(new int[] { 130, 131, 132, 136 }, cancel.Token, Furr);
                        }
                        else
                        {
                            // (0:136) When a furre rolls #d#,
                            // (0:138) When a fuure rolls #d#+#,
                            // (0:140) When a furre rolls #d#-#,
                            // (0:136) When any one rolls anything,
                            await MSpage.ExecuteAsync(new int[] { 133, 134, 135, 136 }, cancel.Token, Furr);
                        }

                        return;

                    case "trade":

                        await MSpage.ExecuteAsync(new int[] { 46, 46, 48 }, cancel.Token, Furr);
                        return;

                    case "shout":

                        await MSpage.ExecuteAsync(new int[] { 8, 9, 10 }, cancel.Token, Furr);
                        return;

                    case "myspeech":
                        break;

                    case "whisper":
                        await MSpage.ExecuteAsync(new int[] { 15, 16, 17 }, cancel.Token, Furr);
                        return;

                    case "say":

                        await MSpage.ExecuteAsync(new int[] { 5, 6, 7, 18, 19, 20 }, cancel.Token, Furr);
                        return;

                    case "emote":
                        if (IsConnectedCharacter(Furr))
                        {
                            return;
                        }

                        //  (0:12) When someone emotes {...} Execute
                        //  (0:13) When someone emotes something with {...} in it
                        //  (0:18) When someone says or emotes something
                        //  (0:19) When someone says or emotes {...}
                        //  (0:20) When someone says or emotes something
                        //  with {...} in it
                        await MSpage.ExecuteAsync(new int[] { 11, 12, 13, 18, 19, 20 }, cancel.Token, Furr);
                        return;

                    case "@emit":

                        await MSpage.ExecuteAsync(new int[] { 21, 22, 23 }, cancel.Token, Furr, Dream);
                        return;

                    case "query":

                        Regex QueryCommand = new Regex("<a.*?href=\'command://(.*?)\'>", RegexOptions.Compiled);
                        await MSpage.ExecuteAsync(new int[] { 34, 35, 32, 33, 40, 41, 38, 39, 36, 37 },
                             cancel.Token, Furr, QueryCommand.Match(Text).Groups[1].Value);
                        return;

                    case "banish":
                        string NameStr;

                        ((ConstantVariable)MSpage.GetVariable(BanishListVariable)).SetValue(string.Join(" ", BanishList.ToArray()));

                        if (Text.Contains(" has been banished from your dreams."))
                        {
                            // banish <name> (online)
                            // Success: (.*?) has been banished from your dreams.
                            // Success: You have canceled all banishments from your dreams.

                            // (0:52) When the bot sucessfilly banishes a furre,
                            // (0:53) When the bot sucessfilly banishes the furre named {...},

                            ((ConstantVariable)MSpage.GetVariable(BanishNameVariable)).SetValue(BanishName);
                            await MSpage.ExecuteAsync(new int[] { 52, 53 }, cancel.Token);
                        }
                        else if ((Text == "You have canceled all banishments from your dreams."))
                        {
                            // banish-off-all (active list)
                            // Success: You have canceled all banishments from your dreams.

                            ((ConstantVariable)MSpage.GetVariable(BanishListVariable)).SetValue(null);
                            ((ConstantVariable)MSpage.GetVariable(BanishNameVariable)).SetValue(null);
                            await MSpage.ExecuteAsync(60, cancel.Token);
                        }
                        else if (Text.EndsWith(" has been temporarily banished from your dreams."))
                        {
                            // tempbanish <name> (online)
                            // Success: (.*?) has been temporarily banished from your dreams.
                            // (0:61) When the bot sucessfully temp banishes a Furre
                            // (0:62) When the bot sucessfully temp banishes the furre named {...}

                            ((ConstantVariable)MSpage.GetVariable(BanishNameVariable)).SetValue(BanishName);
                            await MSpage.ExecuteAsync(new int[] { 61, 62 }, cancel.Token);
                        }
                        else if (Text.StartsWith("Players banished from your dreams: "))
                        {
                            // Banish-List
                            // [notify> Players banished from your dreams:
                            // `(0:54) When the bot sees the banish list
                            await MSpage.ExecuteAsync(54, cancel.Token);
                        }
                        else if (Text.StartsWith("The banishment of player "))
                        {
                            // banish-off <name> (on list)
                            // [notify> The banishment of player (.*?) has ended.
                            // (0:56) When the bot successfully removes a furre from the banish list,
                            // (0:58) When the bot successfully removes the furre named {...} from the banish list,

                            Regex t = new Regex("The banishment of player (.*?) has ended.", RegexOptions.Compiled);
                            NameStr = t.Match(Text).Groups[1].Value;
                            ((ConstantVariable)MSpage.GetVariable(BanishNameVariable)).SetValue(BanishName);

                            await MSpage.ExecuteAsync(new int[] { 56, 58 }, cancel.Token);
                        }
                        else if (Text.Contains("There are no furres around right now with a name starting with "))
                        {
                            // Banish <name> (Not online)
                            // Error:>>  There are no furres around right now with a name starting with (.*?) .
                            // (0:50) When the Bot fails to banish a furre,
                            // (0:51) When the bot fails to banish the furre named {...},

                            Regex t = new Regex("There are no furres around right now with a name starting with (.*?) .", RegexOptions.Compiled);
                            NameStr = t.Match(Text).Groups[1].Value;
                            ((ConstantVariable)MSpage.GetVariable(BanishNameVariable)).SetValue(NameStr);

                            await MSpage.ExecuteAsync(new int[] { 50, 51 }, cancel.Token);
                        }
                        else if ((Text == "Sorry, this player has not been banished from your dreams."))
                        {
                            // banish-off <name> (not on list)
                            // Error:>> Sorry, this player has not been banished from your dreams.
                            // (0:55) When the Bot fails to remove a furre from the banish list,
                            // (0:56) When the bot fails to remove the furre named {...} from the banish list,

                            ((ConstantVariable)MSpage.GetVariable(BanishNameVariable)).SetValue(BanishName);
                            await MSpage.ExecuteAsync(new int[] { 50, 51 }, cancel.Token);
                        }
                        else if ((Text == "You have not banished anyone."))
                        {
                            // banish-off-all (empty List)
                            // Error:>> You have not banished anyone.
                            // (0:59) When the bot fails to see the banish list,

                            ((ConstantVariable)MSpage.GetVariable(BanishListVariable)).SetValue(null);
                            await MSpage.ExecuteAsync(59, cancel.Token);
                        }
                        else if ((Text == "You do not have any cookies to give away right now!"))
                        {
                            await MSpage.ExecuteAsync(95, cancel.Token);
                        }

                        return;

                    case "@cookie":
                        var CookieToMe = new Regex(CookieToMeREGEX);
                        if (CookieToMe.Match(Text).Success)
                        {
                            await MSpage.ExecuteAsync(new int[] { 42, 43 }, cancel.Token, Furr);
                        }

                        Regex CookieToAnyone = new Regex(string.Format("<name shortname=\'(.*?)\'>(.*?)</name> just gave <name shortname=\'(.*?)\'>(.*?)</name> a (.*?)", RegexOptions.Compiled));
                        if (CookieToAnyone.Match(Text).Success)
                        {
                            if (IsConnectedCharacter(Furr))
                            {
                                await MSpage.ExecuteAsync(new int[] { 42, 43 }, cancel.Token, Furr);
                            }
                            else
                            {
                                await MSpage.ExecuteAsync(44, cancel.Token, Furr);
                            }
                        }

                        var CookieFail = new Regex("You do not have any (.*?) left!", RegexOptions.Compiled);
                        if (CookieFail.Match(Text).Success)
                        {
                            await MSpage.ExecuteAsync(45, cancel.Token, Furr);
                        }

                        var EatCookie = new Regex((Regex.Escape("<img src=\'fsh://system.fsh:90\' alt=\'@cookie\'/><channel name=\'@cookie\'/> You eat a cookie.") + "(.*?)"), RegexOptions.Compiled);
                        if (EatCookie.Match(Text).Success)
                        {
                            await MSpage.ExecuteAsync(49, cancel.Token, Furr);
                        }

                        // (0:96) When the Bot sees "Your cookies are ready."
                        Regex CookiesReady = new Regex($"<, aGreaterYour cookies are ready.http://furcadia.com/cookies/ for more info!", RegexOptions.Compiled);
                        if (CookiesReady.Match(Text).Success)
                        {
                            await MSpage.ExecuteAsync(96, cancel.Token, Furr);
                        }

                        return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error<Bot>(ex);
            }
        }

        private async void OnServerStatusChanged(object Sender, NetServerEventArgs e)
        {
            var cancel = new CancellationTokenSource(500);
            if (!MsEngineOptions.IsEnabled)
            {
                return;
            }

            try
            {
                switch (e.ConnectPhase)
                {
                    case ConnectionPhase.Disconnected:
                        // (0:2) When the bot logs out of furcadia,

                        await MSpage.ExecuteAsync(2, cancel.Token);

                        MSpage.Dispose();
                        return;

                    case ConnectionPhase.Connected:
                        // (0:1) When the bot logs into furcadia,

                        await MSpage.ExecuteAsync(1, cancel.Token);
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
        private async Task StartEngine()
        {
            var cancel = new CancellationTokenSource(500);
            MsEngine = new MonkeyspeakEngine(MsEngineOptions);
            string MonkeySpeakScript = Engine.MsEngineExtentionFunctions.LoadFromScriptFile(MsEngineOptions.MonkeySpeakScriptFile);
            MSpage = await MsEngine.LoadFromStringAsync(MonkeySpeakScript);
            var TimeStart = DateTime.Now;
            List<IVariable> VariableList = new List<IVariable>();
            MSpage = LoadLibrary(false);
            IFurre fur = new Furre();
            VariableList.Add(new ConstantVariable(DreamOwnerVariable, Dream.DreamOwner));
            VariableList.Add(new ConstantVariable(DreamNameVariable, Dream.Name));
            VariableList.Add(new ConstantVariable(BotNameVariable, ConnectedFurre.Name));
            VariableList.Add(new ConstantVariable(BotControllerVariable, MsEngineOptions.BotController));
            VariableList.Add(new ConstantVariable(TriggeringFurreNameVariable, fur.Name));
            VariableList.Add(new ConstantVariable(TriggeringFurreShortNameVariable, fur.ShortName));
            VariableList.Add(new ConstantVariable(MessageVariable, fur.Message));
            VariableList.Add(new ConstantVariable(BanishNameVariable, null));
            VariableList.Add(new ConstantVariable(BanishListVariable, null));
            PageSetVariable(VariableList);
            // (0:0) When the bot starts,
            await MSpage.ExecuteAsync(0, cancel.Token);
            Logger.Info($"Done!!! Executed {MSpage.Size} triggers in {DateTime.Now.Subtract(TimeStart).Seconds} seconds.");
        }

        #endregion Private Methods
    }
}