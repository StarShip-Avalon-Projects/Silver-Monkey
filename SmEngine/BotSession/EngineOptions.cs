using System;
using System.IO;

namespace BotSession
{
    // '' <summary>
    // '' MonkeySpeak Engine settings
    // '' </summary>
    public class EngineOptoons : Monkeyspeak.Options
    {
        #region Private Fields

        private string _BotController;

        private string _MonkeySpeakScriptFile;
        private bool _MS_Engine_Enable;

        #endregion Private Fields

        #region Public Constructors

        // '' <summary>
        // '' Default Constructor
        // '' </summary>
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
            _MS_Engine_Enable = true;
        }

        #endregion Public Constructors

        #region Public Properties

        public string BotController
        {
            get
            {
                return _BotController;
            }
            set
            {
                _BotController = value;
            }
        }

        public string BotControllerShortName
        {
            get
            {
                return _BotController.ToFurcadiaShortName();
            }
        }

        // '' <summary>
        // '' Gets or sets the monkey speak script file.
        // '' </summary>
        // '' <value>
        // '' The monkey speak script file.
        // '' </value>
        // '' <exception cref="ArgumentException">Invalid File type, Not a ""*.ms"" file.</exception>
        public string MonkeySpeakScriptFile
        {
            get
            {
                return _MonkeySpeakScriptFile;
            }
            set
            {
                if (!(Path.GetExtension(value).ToLower() == ".ms"))
                {
                    throw new ArgumentException("Invalid File type, Not a \"*.ms\" file.");
                }

                _MonkeySpeakScriptFile = value;
            }
        }

        public bool MS_Engine_Enable
        {
            get
            {
                return _MS_Engine_Enable;
            }
            set
            {
                _MS_Engine_Enable = value;
            }
        }

        #endregion Public Properties
    }
}