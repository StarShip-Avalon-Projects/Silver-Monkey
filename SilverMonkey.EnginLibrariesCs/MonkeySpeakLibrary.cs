using Furcadia.Net.DreamInfo;
using Furcadia.Net.Proxy;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Linq;
using MonkeyCore.Logging;
using static Libraries.MsLibHelper;
using MonkeyCore.Data;
using Furcadia.Net.Utils.ServerParser;
using System.Threading;
using System;

namespace Libraries
{
    /// <summary>
    /// The base library in which all Silver Monkey's Monkey Speak Libraries
    /// are built on. This Library contains the commonly used functions for
    /// all the other libraries
    /// </summary>
    public class MonkeySpeakLibrary : AutoIncrementBaseLibrary
    {
        #region Internal Fields

        internal const string DataBaseTimeZone = "Central Standard Time";
        internal const string DateTimeFormat = "MM-dd-yyyy hh:mm:ss";
        internal static SQLiteDatabase database = null;

        #endregion Internal Fields

        #region Private Fields

        private static Dream _dreamInfo;
        private static object[] args = null;
        private static Furre player;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the current dream information for the dream Silver Monkey is located in.
        /// </summary>
        /// <value>The dream information.</value>
        public static Dream DreamInfo
        {
            get
            {
                if (_dreamInfo is null)
                    _dreamInfo = ParentBotSession.Dream;
                return _dreamInfo;
            }
            set => _dreamInfo = value;
        }

        public static FurreList Furres { get => ParentBotSession.Furres; }

        /// <summary>
        /// Reference to the Main Bot Session for the bot
        /// </summary>
        public static ProxySession ParentBotSession { get; set; }

        /// <summary>
        /// Current Triggering Furre
        /// <para/>
        /// Referenced as a Monkey-Speak <see cref="BaseLibrary.Initialize(params object[])"/> Argument.
        /// <para/>
        /// Updates when ever Monkey Speak needs it through <see cref="Page.Execute(int[],
        /// object[])"/> or <see cref="Page.ExecuteAsync(int[], object[])"/>
        /// </summary>
        static public Furre Player
        {
            get
            {
                if (player == null)
                    player = ParentBotSession.Player;
                return player;
            }
            set => player = value;
        }

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 0;

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// Currently used database file.
        /// </summary>
        /// <returns>
        /// SQLite database file with Silver Monkey system tables and user data
        /// </returns>
        internal static string SQLitefile { get; set; } = null;

        #endregion Internal Properties

        #region Public Methods

        /// <summary>
        /// Set <see cref="DreamInfo"/> from <see cref="TriggerReader.GetParameter"></see>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">DreamInfo not set</exception>
        [TriggerDescription("Gets the information for the current dream and set the dream Monkeyspeak variables.")]
        public static bool ReadDreamParams(TriggerReader reader)
        {
            Dream dreamInfo = reader.GetParametersOfType<Dream>().FirstOrDefault();

            if (dreamInfo != DreamInfo)
            {
                DreamInfo = dreamInfo;
                UpdateCurrentDreamVariables(DreamInfo, reader.Page);
            }

            return dreamInfo == DreamInfo;
        }

        /// <summary>
        /// Reads the triggering Furre parameters.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        [TriggerDescription("gets the triggering furre information and set the Triggering furre Monkeyspeak Variables.")]
        public static bool ReadTriggeringFurreParams(TriggerReader reader)
        {
            bool ParamSet = false;

            Furre ActiveFurre = reader.GetParametersOfType<Furre>().FirstOrDefault();
            if (ActiveFurre != Player)
            {
                Player = ActiveFurre;
                if (ActiveFurre.FurreID != -1 && ActiveFurre.ShortName != "unknown")
                {
                    ParamSet = true;
                }

                UpdateTriggerigFurreVariables(Player, reader.Page);
                return ParamSet;
            }

            return Player == ActiveFurre;
        }

        /// <summary>
        /// Check to see if the specified Furre is the bot controller.
        /// <para/>
        /// used for bot control Monkey-Speak
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Check the specified furre to see if they are the bot controller")]
        public bool FurreNamedIsBotController(TriggerReader reader)
        {
            var BotController = reader.Page.GetVariable(BotControllerVariable);

            if (string.IsNullOrWhiteSpace(BotController.Value.ToString()))
            {
                Logger.Warn("BotController is not defined, Please specify a BotController in the Bot configuration settings,");
                return false;
            }
            return BotController.Value.ToString().ToFurcadiaShortName() == reader.ReadString().ToFurcadiaShortName();
        }

        /// <summary>
        /// Gets the argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T GetArgumet<T>(int index = 0)
        {
            if (args != null && args.Length > index)
                return (T)args[index];
            return default(T);
        }

        /// <summary>
        /// Gets the arguments of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>array of type T or null</returns>
        public T[] GetArgumetsOfType<T>()
        {
            if (args != null && args.Length > 0)
                return args.OfType<T>().ToArray();
            return new T[0];
        }

        /// <summary>
        /// checks the <see cref="FurreList"/>
        /// in the <see cref="DreamInfo">Dream Parameter</see>
        /// for the Target Furre.
        /// </summary>
        /// <param name="TargetFurre">
        /// Target Furre
        /// </param>
        /// <returns>
        /// True if the Furre is in the dream <see cref="FurreList"/>
        /// </returns>
        public bool InDream(IFurre TargetFurre)
        {
            bool found = false;
            foreach (IFurre Fur in Furres)
            {
                if (Fur == TargetFurre)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">
        /// Parametrized argument of vars to use to pass runtime vars to a library at initialization
        /// </param>
        public override void Initialize(params object[] args)
        {
            MonkeySpeakLibrary.args = args;

            var bot = GetArgumetsOfType<ProxySession>().FirstOrDefault();
            if (bot != ParentBotSession)
            {
                ParentBotSession = bot;
            }
        }

        /// <summary>
        /// Separate function for unit testing
        /// </summary>
        /// <param name="Furr"></param>
        /// <returns></returns>
        public bool IsConnectedCharacter(IFurre Furr)
        {
            if (Furr == null || ParentBotSession == null)
                return false;
            return ParentBotSession.IsConnectedCharacter(Furr);
        }

        /// <summary>
        /// Generic base Furre named {...} is Triggering Furre
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>True on Name match</returns>
        /// <remarks>
        /// any name is accepted and converted to Furcadia Machine name (ShortName version, lowercase
        /// with special characters stripped)
        /// </remarks>
        [TriggerDescription("Checks to see if the current triggering furre is the specified furre")]
        [TriggerStringParameter]
        public bool NameIs(TriggerReader reader)
        {
            return reader.ReadString().ToFurcadiaShortName() == Player.ShortName;
        }

        /// <summary>
        /// Send a raw instruction to the client
        /// </summary>
        /// <param name="message">
        /// Message string.
        /// </param>
        public void SendClientMessage(string message)
        {
            var SendTextToServerThread = new Thread(() =>
            {
                ParentBotSession.SendToClient(message);
            });
            SendTextToServerThread.Start();
        }

        /// <summary>
        /// Send Formated Text to Server
        /// </summary>
        /// <param name="message">Client to server instruction</param>
        /// <returns>True is the Server is Connected</returns>
        public virtual bool SendServer(string message)
        {
            if (ParentBotSession == null)
                return false;
            if (ParentBotSession.IsServerSocketConnected)
            {
                var SendTextToServerThread = new Thread(() =>
                {
                    ParentBotSession.SendFormattedTextToServer(message);
                });
                SendTextToServerThread.Start();
            }
            return true;
        }

        /// <summary>
        /// Triggering Furre is bot controller.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        [TriggerDescription("Check to see if the current triggering furre is the bot controller")]
        public bool TriggeringFurreIsBotController(TriggerReader reader)
        {
            var BotController = reader.Page.GetVariable(BotControllerVariable);

            if (string.IsNullOrWhiteSpace(BotController.Value.ToString()))
            {
                Logger.Warn("BotController is not defined, Please specify a BotController in the Bot configuration settings,");
                return false;
            }

            return Player.ShortName == BotController.Value.ToString().ToFurcadiaShortName();
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Unload(Page page)
        {
            args = null;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Check <see cref="Player.Message"/> against the specified string it should contain.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageContains(TriggerReader reader)
        {
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();

            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().Contains(msMsg.ToLower());
        }

        /// <summary>
        /// Check <see cref="Player.Message"/> against the specified string it should contain.
        /// <para/>
        /// This also set the Triggering Furre and Dream Variable sets with <see cref="Page.Execute(int, object[])"/> Parameters
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>True if the %MESSAGE system variable contains the specified string</returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageContainsAndSetVariables(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            return MessageContains(reader);
        }

        /// <summary>
        /// Check <see cref="Player.Message"/> against the specified string it should end with.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageEndsWith(TriggerReader reader)
        {
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();

            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().EndsWith(msMsg.ToLower()) & !IsConnectedCharacter(Player);
        }

        /// <summary>
        /// Check <see cref="Player.Message"/> against the specified string it should end with.
        /// <para/>
        /// This also set the Triggering Furre and Dream Variable sets with <see cref="Page.Execute(int, object[])"/> Parameters
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>true if the System %MESSAGE variable ends with the specified string</returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageEndsWithAndSetVariabled(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            return MessageEndsWith(reader);
        }

        /// <summary>
        /// Check <see cref="Player.Message"/> against the specified Message
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageIs(TriggerReader reader)
        {
            string msg = Player.Message.ToStrippedFurcadiaMarkupString();
            string test = reader.ReadString().ToStrippedFurcadiaMarkupString();

            if (msg is null)
                return test is null;
            return msg.ToLower() == test.ToLower();
        }

        /// <summary>
        /// Check <see cref="Player.Message"/> against the specified Message
        /// <para/>
        /// This also set the Triggering Furre and Dream Variable sets with <see cref="Page.Execute(int, object[])"/> Parameters
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageIsAndSetVariables(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            return MessageIs(reader);
        }

        /// <summary>
        /// Check <see cref="Player.Message"/>  against the specified string the message should contain
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageStartsWith(TriggerReader reader)
        {
            string msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            string msg = Player.Message.ToStrippedFurcadiaMarkupString();

            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().StartsWith(msMsg.ToLower()) & !IsConnectedCharacter(Player);
        }

        /// <summary>
        /// Check <see cref="Player.Message"/>  against the specified string the message should contain
        /// <para/>
        /// This also set the Triggering Furre and Dream Variable sets with <see cref="Page.Execute(int, object[])"/> Parameters
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [TriggerDescription("Check the specified string against the Triggering Furre's message. The message is compared with Fucadia Markup stripped with non case sensitivity")]
        [TriggerStringParameter]
        protected bool MessageStartsWithAndSetVariables(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            return MessageStartsWith(reader);
        }

        #endregion Protected Methods
    }
}