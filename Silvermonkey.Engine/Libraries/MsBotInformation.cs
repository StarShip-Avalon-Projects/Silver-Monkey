using Engine.BotSession;
using Furcadia.Net.Proxy;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Libraries
{
    /// <summary>
    /// Bot Status and Furcadia Client Process control
    /// </summary>
    /// <seealso cref="Monkeyspeak.Libraries.BaseLibrary" />
    public class MsBotInformation : MonkeySpeakLibrary
    {
        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 0;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of vars to use to pass runtime vars to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            Add(TriggerCategory.Cause,
                 StartScript,
                "When the Monkey Speak Engine starts the script,");

            Add(TriggerCategory.Cause,
                 r =>
                 {
                     Bot bot = r.GetParametersOfType<Bot>().FirstOrDefault();
                     if (ParentBotSession != bot)
                         ParentBotSession = bot;
                     return true;
                 },
                 "When the bot logs into Furcadia,");

            Add(TriggerCategory.Cause,
                r =>
                {
                    Bot bot = r.GetParametersOfType<Bot>().FirstOrDefault();
                    if (ParentBotSession != bot)
                        ParentBotSession = bot;
                    return true;
                },
                "When the bot logs out of Furcadia,");

            Add(TriggerCategory.Cause,
                r =>
                {
                    {
                        Bot bot = r.GetParametersOfType<Bot>().FirstOrDefault();
                        if (ParentBotSession != bot)
                            ParentBotSession = bot;
                        return true;
                    }
                },
                "When the Furcadia client disconnects or closes,");

            // (0:92) When the bot detects the "Your throat is tired. Please wait a few seconds"message,
            Add(TriggerCategory.Cause,
                r =>
                {
                    var rtn = r.GetParametersOfType<bool?>().FirstOrDefault();
                    if (rtn != null)
                        return (bool)rtn;
                    return false;
                },
                "When the bot detects the \"Your throat is tired. Please wait a few seconds\"message,");

            // (0:93) When the bot resumes processing after seeing "Your throat is tired"message,
            Add(TriggerCategory.Cause,
                r =>
                {
                    var rtn = r.GetParametersOfType<bool?>().FirstOrDefault();
                    if (rtn != null)
                        return (bool)rtn;
                    return false;
                },
                "When the bot resumes processing after seeing \"Your throat is tired\"message,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsBotController(r),
                "and the triggering furre is the Bot Controller,");

            Add(TriggerCategory.Condition,
             r => !TriggeringFurreIsBotController(r),
                "and the triggering furre is not the Bot Controller,");

            Add(TriggerCategory.Effect,
                r => StandAloneMode(r),
                "switch the bot to stand alone mode and close the Furcadia client.");

            Add(TriggerCategory.Effect,
                r => FurcadiaDisconnect(r),
                "disconnect the bot from the Furcadia game server.");

            Add(TriggerCategory.Effect,
                r => StartNewBot(r),
                "start a new instance to Silver Monkey with bot-file {..}.");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        [TriggerDescription("starts a new Silver Monkey Instance")]
        private static bool StartNewBot(TriggerReader reader)
        {
            var File = reader.ReadString();
            ProcessStartInfo p = new ProcessStartInfo()
            {
                //TODO Change FileName to Reflection
                Arguments = File,
                FileName = "SilverMonkey.exe",
                WorkingDirectory = IO.Paths.ApplicationPath
            };
            Process.Start(p);
            return true;
        }

        [TriggerDescription("Triggers afte the Libraries are loaded and bot constant variables are set.")]
        private static bool StartScript(TriggerReader reader)
        {
            Bot bot = reader.GetParametersOfType<Bot>().FirstOrDefault();
            if (ParentBotSession != bot)
                ParentBotSession = bot;
            return true;
        }

        [TriggerDescription("Disconnects the bot from the Furcadia game server")]
        private bool FurcadiaDisconnect(TriggerReader reader)
        {
            Task.Run(() => ParentBotSession.DisconnectServerAndClientStreams()).Wait();
            return !ParentBotSession.IsServerSocketConnected;
        }

        [TriggerDescription("Changes the bot to Stand Alone mode and closes the Furcadia client")]
        private bool StandAloneMode(TriggerReader reader)
        {
            ParentBotSession.StandAlone = true;
            NetProxy.CloseFurcadiaClient();
            return true;
        }

        #endregion Private Methods
    }
}