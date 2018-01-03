using Monkeyspeak;
using Monkeyspeak.Libraries;

namespace Libraries
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Monkeyspeak.Libraries.BaseLibrary" />
    public class MsStartup : BaseLibrary
    {
        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void Initialize(params object[] args)
        {
            Add(TriggerCategory.Cause, 0, (t) => true, "When the Monkey Speak Engine starts the script,");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods
    }
}