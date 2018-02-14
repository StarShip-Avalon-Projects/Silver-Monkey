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

        private string _BotController;

        private string _MonkeySpeakScriptFile;
        private bool _isEnabled;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EngineOptoons()
        {
            _BotController = null;
            CanOverrideTriggerHandlers = false;
            StringBeginSymbol = '{';

            StringEndSymbol = '}';

            VariableDeclarationSymbol = '%';

            LineCommentSymbol = '*';

            BlockCommentBeginSymbol = "/*";
            BlockCommentEndSymbol = "*/";
            TriggerLimit = 6000;
            VariableCountLimit = 1000;
            StringLengthLimit = Int32.MaxValue;
            _isEnabled = true;
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
            get => _BotController;
            set => _BotController = value;
        }

        /// <summary>
        /// Gets the short name of the bot controller.
        /// </summary>
        /// <value>
        /// The short name of the bot controller.
        /// </value>
        public string BotControllerShortName => _BotController.ToFurcadiaShortName();

        /// <summary>
        /// Gets or sets the monkey speak script file.
        /// </summary>
        /// <value>
        /// The monkey speak script file.
        /// </value>
        /// <exception cref="ArgumentException">Invalid File type, Not a ""*.ms"" file.</exception>
        public string MonkeySpeakScriptFile
        {
            get => _MonkeySpeakScriptFile;
            set
            {
                if (Path.GetExtension(value).ToLower() != ".ms")
                {
                    throw new ArgumentException("Invalid File type, Not a \"*.ms\" file.");
                }

                _MonkeySpeakScriptFile = value;
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
            get => _isEnabled;
            set => _isEnabled = value;
        }

        #endregion Public Properties
    }
}