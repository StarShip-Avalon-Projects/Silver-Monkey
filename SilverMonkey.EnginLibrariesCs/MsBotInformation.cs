using Engine.Libraries;
using Furcadia.Net;
using Monkeyspeak;
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
        public override int BaseId => 1;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void Initialize(params object[] args)
        {
            Add(TriggerCategory.Cause,
                 r => true,
                 "When the bot logs into Furcadia,");

            Add(TriggerCategory.Cause,
                r => true,
                "When the bot logs out of Furcadia,");

            Add(TriggerCategory.Cause,
                r => true,
                "When the Furcadia client disconnects or closes,");

            // (0:92) When the bot detects the "Your throat is tired. Please wait a few seconds"message,
            Add(TriggerCategory.Cause,
                r =>
                {
                    var rtn = r.GetParametersOfType<bool?>().First();
                    if (rtn != null)
                        return (bool)rtn;
                    return false;
                },
                "When the bot detects the \"Your throat is tired. Please wait a few seconds\"message,");

            // (0:93) When the bot resumes processing after seeing "Your throat is tired"message,
            Add(TriggerCategory.Cause,
                r =>
                {
                    var rtn = r.GetParametersOfType<bool?>().First();
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

        private bool FurcadiaDisconnect(TriggerReader reader)
        {
            Task.Run(() => ParentBotSession.DisconnectServerAndClientStreams()).Wait();
            return !ParentBotSession.IsServerSocketConnected;
        }

        private bool StandAloneMode(TriggerReader reader)
        {
            ParentBotSession.StandAlone = true;
            NetProxy.CloseFurcadiaClient();
            return true;
        }

        #endregion Private Methods
    }
}