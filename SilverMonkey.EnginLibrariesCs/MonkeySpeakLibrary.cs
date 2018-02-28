using Furcadia.Net.DreamInfo;
using Furcadia.Net.Proxy;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Linq;
using MsLog = Monkeyspeak.Logging;
using static Libraries.MsLibHelper;
using MonkeyCore.Data;

namespace Libraries
{
    /// <summary>
    /// The base library in which all Silver Monkey's Monkey Speak Libraries
    /// are built on. This Library contains the commonly used functions for
    /// all the other libraries
    /// </summary>
    public class MonkeySpeakLibrary : AutoIncrementBaseLibrary
    {
        internal const string DateTimeFormat = "MM-dd-yyyy hh:mm:ss";
        internal const string DataBaseTimeZone = "Central Standard Time";

        /// <summary>
        /// Currenly used database filfe
        /// </summary>
        /// <returns>
        /// SQLite database file with Silver Monkey system tables and user data
        /// </returns>
        internal static string SQLitefile { get; set; }

        internal static SQLiteDatabase database = null;

        #region Public Fields

        /// <summary>
        /// The arguments
        /// </summary>
        private static object[] args = null;

        #endregion Public Fields

        #region Public Properties

        /// <summary>
        /// Connected Furre representing Silver Monkey
        /// </summary>
        /// <value>
        /// The connected furre.
        /// </value>
        public static Furre ConnectedFurre { get; set; }

        /// <summary>
        /// Reference to the Main Bot Session for the bot
        /// </summary>
        public static ProxySession ParentBotSession { get; set; }

        /// <summary>
        /// Current Triggering Furre
        /// <para/>
        /// Referenced as a Monkeyspeak <see cref="BaseLibrary.Initialize(params object[])"/> Argument.
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

        /// <summary>
        /// Gets or sets the current dream information for the dream Silver Monkey is located in.
        /// </summary>
        /// <value>The dream information.</value>
        public static Dream DreamInfo { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool FurreNamedIsBotController(TriggerReader reader)
        {
            if (ParentBotSession != null)
            {
                return Player.ShortName == reader.ReadString().ToFurcadiaShortName();
            }

            return false;
        }

        /// <summary>
        /// Gets the argumet.
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
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Unload(Page page)
        {
        }

        /// <summary>
        /// Gets the type of the argumets of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
        /// True if the furre is in the dream <see cref="FurreList"/>
        /// </returns>
        public bool InDream(Furre TargetFurre)
        {
            bool found = false;
            foreach (Furre Fur in DreamInfo.Furres)
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
        /// Parametized argument of vars to use to pass runtime vars to a library at initialization
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
        /// Seperate function for unit testing
        /// </summary>
        /// <param name="Furr"></param>
        /// <returns></returns>
        public bool IsConnectedCharacter(Furre Furr)
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
        /// any name is acepted and converted to Furcadia Machine name (ShortName version, lowercase
        /// with special characters stripped)
        /// </remarks>
        public bool NameIs(TriggerReader reader)
        {
            return reader.ReadString().ToFurcadiaShortName() == Player.ShortName;
        }

        /// <summary>
        /// Set <see cref="DreamInfo"/> from <see cref="TriggerReader.GetParameter"></see>
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">DreamInfo not set</exception>
        public static bool ReadDreamParams(TriggerReader reader)
        {
            Dream dreamInfo = reader.GetParametersOfType<Dream>().First();

            if (dreamInfo != DreamInfo)
            {
                DreamInfo = dreamInfo;
                UpdateCurrentDreamVariables(DreamInfo, reader.Page);
                return true;
            }

            return true;
        }

        /// <summary>
        /// Reads the triggering furre parameters.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static bool ReadTriggeringFurreParams(TriggerReader reader)
        {
            bool ParamSet = false;

            Furre ActiveFurre = reader.GetParametersOfType<Furre>().First();
            if (ActiveFurre != Player)
            {
                Player = ActiveFurre;
                if (ActiveFurre.FurreID != -1 && ActiveFurre.ShortName != "unknown")
                {
                    ParamSet = true;
                }

                UpdateTriggerigFurreVariables(Player, reader.Page);
            }

            return ParamSet;
        }

        /// <summary>
        /// Send a raw instruction to the client
        /// </summary>
        /// <param name="message">
        /// Message sring
        /// </param>
        public void SendClientMessage(string message)
        {
            ParentBotSession.SendToClient(message);
        }

        /// <summary>
        /// Send Formated Text to Server
        /// </summary>
        /// <param name="message">Client to server instruction</param>
        /// <returns>True is the Server is Connected</returns>
        public bool SendServer(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || ParentBotSession == null)
                return false;

            if (ParentBotSession.IsServerSocketConnected)
            {
                ParentBotSession.SendFormattedTextToServer(message);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Triggerings the furre is bot controller.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public bool TriggeringFurreIsBotController(TriggerReader reader)
        {
            var BotController = reader.Page.GetVariable(BotControllerVariable);
            if (string.IsNullOrWhiteSpace(BotController.Value.ToString()))
            {
                MsLog.Logger.Warn("BotController is not defined, Please specifiy a BotController in the Bot configuration settings,");
                return false;
            }

            return Player.ShortName == BotController.Value.ToString().ToFurcadiaShortName();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// <para>Comparisons are done with Fucadia Markup Stripped</para>
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>True if the %MESSAGE system variable contains the specified string</returns>
        protected bool MsgContains(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            ReadDreamParams(reader);
            var msMsg = reader.ReadString();
            var msg = Player.Message;
            if (msg is null || msMsg is null)
                return false;
            return msg.Contains(msMsg.ToStrippedFurcadiaMarkupString());
        }

        /// <summary>
        ///  <para>Comparisons are done with Fucadia Markup Stripped</para>
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>true if the System %MESSAGE varible ends with the specified string</returns>
        protected bool MsgEndsWith(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            ReadDreamParams(reader);
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();
            if (msg is null || msMsg is null)
                return false;
            return msg.EndsWith(msMsg) & !IsConnectedCharacter(Player);
        }

        /// <summary>
        /// the Main Message is Comparason function
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        protected bool MsgIs(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            ReadDreamParams(reader);
            string msg = Player.Message.ToStrippedFurcadiaMarkupString();
            string test = reader.ReadString().ToStrippedFurcadiaMarkupString();
            if (msg is null || test is null)
                return msg == test;
            return msg.ToLower() == test.ToLower();
        }

        /// <summary>
        /// (1:11) and triggering furre's message starts with {.},
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns></returns>
        protected bool MsgStartsWith(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            ReadDreamParams(reader);
            string msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
            string msg = Player.Message.ToStrippedFurcadiaMarkupString().ToLower();
            if (msg is null || msMsg is null)
                return false;
            return msg.ToLower().StartsWith(msMsg.ToLower()) & !IsConnectedCharacter(Player);
        }

        #endregion Protected Methods
    }
}