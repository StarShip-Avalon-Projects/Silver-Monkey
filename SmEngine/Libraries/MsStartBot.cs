using Engine.BotSession;
using Libraries;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Linq;

namespace SilverMonkey.Engine.Libraries
{
    /// <summary>
    /// Initial Class to Launch the Engie with
    /// </summary>
    /// <seealso cref="Libraries.MonkeySpeakLibrary" />
    public class MsStartBot : MonkeySpeakLibrary
    {
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
                 r => StartScript(r),
                "When the Monkey Speak Engine starts the script,");
        }

        [TriggerDescription("Triggers afte the Libraries are loaded and bot constant variables are set.")]
        private bool StartScript(TriggerReader reader)
        {
            Bot ThisBot = reader.GetParametersOfType<Bot>().FirstOrDefault();
            if (ParentBotSession != ThisBot)
                ParentBotSession = ThisBot;

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