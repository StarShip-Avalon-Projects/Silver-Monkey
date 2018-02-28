using System;
using System.IO;

namespace Engine.BotSession
{
    /// <summary>
    /// MonkeySpeak Engine settings
    /// </summary>
    public class EngineOptoons : Monkeyspeak.Options
    {
        #region Private Fields

        private string botController;

        private string monkeySpeakScriptFile;
        private bool isEnabled;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EngineOptoons() : base()
        {
            // Bot Defaults
            botController = null;
            isEnabled = true;

            // Monkeyspeak overrides
            TriggerLimit = 512 * 1024;
            StringLengthLimit = 2 * 1024;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the furre in charge of the bot.
        /// </summary>
        /// <value>
        /// The bot controller.
        /// </value>
        public string BotController
        {
            get => botController;
            set => botController = value;
        }

        /// <summary>
        /// Gets the short name of the bot controller.
        /// </summary>
        /// <value>
        /// The short name of the bot controller.
        /// </value>
        public string BotControllerShortName => botController.ToFurcadiaShortName();

        /// <summary>
        /// Gets or sets the monkey speak script file.
        /// </summary>
        /// <value>
        /// The monkey speak script file.
        /// </value>
        /// <exception cref="ArgumentException">Invalid File type, Not a ""*.ms"" file.</exception>
        public string MonkeySpeakScriptFile
        {
            get => monkeySpeakScriptFile;
            set
            {
                if (Path.GetExtension(value).ToLower() != ".ms")
                {
                    throw new ArgumentException("Invalid File type, Not a \"*.ms\" file.");
                }

                monkeySpeakScriptFile = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the monkeyspeak engine is enabled for this instance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ms engine enable]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        #endregion Public Properties
    }
}