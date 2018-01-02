using static Engine.Libraries.MsLibHelper;
using Monkeyspeak.Libraries;
using Monkeyspeak;
using Furcadia.Net.DreamInfo;
using System.Threading;
using System;
using System.Linq;
using BotSession;
using System.Threading.Tasks;

namespace Engine.Libraries
{
    /// <summary>
    /// The base library in which all Silver Monkey's Monkey Speak Libraries
    /// are built on. This Library contains the commonly used functions for
    /// all the other libraries
    /// </summary>
    public class MonkeySpeakLibrary : BaseLibrary
    {
        #region Public Fields

        public object[] args;

        #endregion Public Fields

        #region Private Fields

        /// <summary>
        /// Current Dream the Bot is in
        /// <para/>
        /// Referenced as a Monkeyspeak Parameter.
        /// <para/>
        /// Updates when ever Monkey Speak needs it through <see cref="Monkeyspeak.Page.Execute(Integer(), Object())"/>
        /// </summary>
        private Dream _dream;

        private DateTime _FurcTime;
        private Timer FurcTimeTimer;

        #endregion Private Fields

        #region Public Properties

        public Furre ConnectedFurre { get; set; }

        public Dream DreamInfo
        {
            get { return _dream; }
            set { _dream = value; }
        }

        /// <summary>
        /// Current Furcadia Standard Time (fst)
        /// </summary>
        /// <returns>
        /// Furcadia Time Object in Furcadia Standard Time (fst)
        /// </returns>
        public DateTime FurcTime
        {
            get
            {
                return _FurcTime;
            }
        }

        /// <summary>
        /// Reference to the Main Bot Session for the bot
        /// </summary>
        public Bot ParentBotSession { get; internal set; }

        /// <summary>
        /// Current Triggering Furre
        /// <para/>
        /// Referenced as a Monkeyspeak Parameter.
        /// <para/>
        /// Updates when ever Monkey Speak needs it through <see cref="Monkeyspeak.Page.Execute(Integer(), Object())"/>
        /// </summary>
        public Furre Player { get; set; }

        #endregion Public Properties

        #region Public Methods

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
                if ((Fur == TargetFurre))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public override void Initialize(params object[] args)
        {
            this.args = args;
            FurcTimeTimer = new Timer(TimeUpdate, null, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
            if (args.Length == 1 && ParentBotSession == null)
            {
                ParentBotSession = GetArgumetsOfType<Bot>()[0];
                Add(TriggerCategory.Cause, 0, (t) => true, "When the Monkey Speak script starts,");
            }
        }

        /// <summary>
        /// Seperate function for unit testing
        /// </summary>
        /// <param name="Furr"></param>
        /// <returns></returns>
        public bool IsConnectedCharacter(Furre Furr)
        {
            if (ParentBotSession != null)
            {
                return ParentBotSession.IsConnectedCharacter(Furr);
            }

            return false;
        }

        /// <summary>
        /// Set <see cref="Player"/> and <see cref="DreamInfo"/> from
        /// GetParametersOfType<T>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>true if any parameter was set; false otherwise</returns>
        public bool ReadDreamParams(TriggerReader reader)
        {
            bool ParamSet = false;
            var dreamInfo = reader.GetParametersOfType<Dream>();
            if (dreamInfo != null && (dreamInfo.Length > 0))
            {
                if (string.IsNullOrWhiteSpace(dreamInfo[0].Name))
                {
                    throw new ArgumentException("DreamInfo not set");
                }

                DreamInfo = dreamInfo[0];
                ParamSet = true;
                UpdateCurrentDreamVariables(DreamInfo, reader.Page);
            }

            return ParamSet;
        }

        /// <summary>
        /// Set <see cref="Player"/> and <see cref="DreamInfo"/> from
        /// GetParametersOfType<T>
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>true if any parameter was set; false otherwise</returns>
        public bool ReadTriggeringFurreParams(TriggerReader reader)
        {
            var ParamSet = false;
            var ActiveFurre = reader.GetParameter<Furre>();
            if (ActiveFurre != null)
            {
                Player = ActiveFurre;
                if ((ActiveFurre.FurreID != -1))
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
        public void SendClientMessage(ref string message)
        {
            ParentBotSession.SendToClient(message);
        }

        /// <summary>
        /// Send Formated Text to Server
        /// </summary>
        /// <param name="message">
        /// Client to server instruction
        /// </param>
        /// <returns>
        /// True is the Server is Connected
        /// </returns>
        public bool SendServer(string message)
        {
            if (ParentBotSession.IsServerSocketConnected)
            {
                ParentBotSession.SendFormattedTextToServer(message);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task SendServerAsync(string message)
        {
            await Task.Run(() => this.SendServer(message));
        }

        public override void Unload(Page page)
        {
            FurcTimeTimer.Dispose();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// <para>
        /// Comparisons are done with Fucadia Markup Stripped
        /// </para>
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// True if the %MESSAGE system variable contains the specified string
        /// </returns>
        protected virtual bool MsgContains(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
            var msg = Player.Message;
            return msg.Contains(msMsg.ToStrippedFurcadiaMarkupString().ToLower());
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true if the System %MESSAGE varible ends with the specified string
        /// </returns>
        protected virtual bool MsgEndsWith(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();
            // Debug.Print("Msg = " & msg)
            return (msg.ToLower().EndsWith(msMsg.ToLower())
                        & !IsConnectedCharacter(Player));
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
        protected virtual bool MsgIs(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var msg = Player.Message.ToStrippedFurcadiaMarkupString();
            var test = reader.ReadString().ToStrippedFurcadiaMarkupString();
            return msg.ToLower() == test.ToLower();
        }

        /// <summary>
        /// (1:14) and triggering furre's message doesn't end with {.},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        protected bool MsgNotEndsWith(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString().ToLower();
            return (!msg.ToLower().EndsWith(msMsg.ToLower())
                        & !IsConnectedCharacter(Player));
        }

        /// <summary>
        /// (1:11) and triggering furre's message starts with {.},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        protected bool MsgStartsWith(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var msMsg = reader.ReadString().ToStrippedFurcadiaMarkupString().ToLower();
            var msg = Player.Message.ToStrippedFurcadiaMarkupString().ToLower();
            return (msg.ToLower().StartsWith(msMsg.ToLower())
                        & !IsConnectedCharacter(Player));
        }

        /// <summary>
        /// Generic base Furre named {...} is Triggering Furre
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// True on Name match
        /// </returns>
        /// <remarks>
        /// any name is acepted and converted to Furcadia Machine name
        /// (ShortName version, lowercase with special characters stripped)
        /// </remarks>
        protected virtual bool NameIs(TriggerReader reader)
        {
            return reader.ReadString().ToFurcadiaShortName() == Player.ShortName;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Furcadia Clock updater
        /// </summary>
        /// <param name="obj">
        /// Nothing
        /// </param>
        private void TimeUpdate(object obj)
        {
            _FurcTime = DateTime.Now;
        }

        #endregion Private Methods
    }
}