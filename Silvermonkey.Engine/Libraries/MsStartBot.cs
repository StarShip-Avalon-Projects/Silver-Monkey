using Engine.BotSession;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Linq;

namespace Libraries
{
    /// <summary>
    /// Initial Class to Launch the Engie with
    /// </summary>
    /// <seealso cref="Libraries.MonkeySpeakLibrary" />
    public class MsStartBot : MonkeySpeakLibrary
    {
        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 0;

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">Parametized argument of objects to use to pass runtime objects to a library at initialization</param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            Add(TriggerCategory.Cause,
                 StartScript,
                "When the Monkey Speak Engine starts the script,");
        }

        [TriggerDescription("Triggers afte the Libraries are loaded and bot constant variables are set.")]
        private static bool StartScript(TriggerReader reader)
        {
            Bot bot = reader.GetParametersOfType<Bot>().FirstOrDefault();
            if (ParentBotSession != bot)
                ParentBotSession = bot;

            return true;
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }
    }
}