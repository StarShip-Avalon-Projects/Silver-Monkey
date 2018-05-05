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

        private static object[] args = null;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Connected Furre representing Silver Monkey
        /// </summary>
        /// <value>
        /// The connected Furre.
        /// </value>
        public static Furre ConnectedFurre { get; set; }

        /// <summary>
        /// Gets or sets the current dream information for the dream Silver Monkey is located in.
        /// </summary>
        /// <value>The dream information.</value>
        public static Dream DreamInfo { get; set; }

        public static FurreList Furres { get; set; }

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
        static public Furre Player { get; set; }

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
        public bool FurreNamedIsBotController(TriggerReader reader)
        {
            return Player.ShortName == reader.ReadString().ToFurcadiaShortName();
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
                DreamInfo = ParentBotSession.Dream;
                Player = ParentBotSession.Player;
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
        [TriggerDescription("Triggers when the specified furre is the triggering furre")]
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
        /// Comparisons are done with Fucadia Markup Stripped
        /// <para/>
        /// comparisons done Markup Stripped and lower-case
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>True if the %MESSAGE system variable contains the specified string</returns>
        protected bool MsgContains(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();

            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().Contains(msMsg.ToLower());
        }

        /// <summary>
        /// <para/>
        /// comparisons done Markup Stripped and lower-case
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>true if the System %MESSAGE variable ends with the specified string</returns>
        protected bool MsgEndsWith(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();

            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().EndsWith(msMsg.ToLower()) & !IsConnectedCharacter(Player);
        }

        /// <summary>
        /// the Main Message is comparison function
        /// <para/>
        /// comparisons done Markup Stripped and lower-case
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        protected bool MsgIs(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            string msg = Player.Message.ToStrippedFurcadiaMarkupString();
            string test = reader.ReadString().ToStrippedFurcadiaMarkupString();

            if (msg is null || test is null)
                return msg == test;
            return msg.ToLower() == test.ToLower();
        }

        /// <summary>
        /// message starts with ...
        /// <para/>
        /// comparisons done Markup Stripped and lower-case
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns></returns>
        [TriggerDescription("Continues processing if the triggering furre's text contains the specified text")]
        [TriggerStringParameter]
        protected bool MsgStartsWith(TriggerReader reader)
        {
            if (!ReadTriggeringFurreParams(reader))
                throw new MonkeyspeakException("Failed to set Triggering Furre Variables");
            if (!ReadDreamParams(reader))
                throw new MonkeyspeakException("Failed to set Dream Variables");

            string msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            string msg = Player.Message.ToStrippedFurcadiaMarkupString();

            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().StartsWith(msMsg.ToLower()) & !IsConnectedCharacter(Player);
        }

        #endregion Protected Methods
    }
}