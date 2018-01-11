using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IO
{
    /// <summary>
    /// Monkey System core File Paths
    /// Silver Monkey standard Paths.
    ///<para>
    ///This Class contains All the Shared paths For Furcadia Folders  And Silver Monkey Paths.
    ///</para>
    /// </summary>
    /// <remarks>
    ///<para>
    ///Furcadia Programs (Furcadia.exe)
    ///</para>
    ///<para>
    ///Furcadia Documents Folder (DSC Templates/Scripts, Character Files)
    ///</para>
    ///<para>
    ///Furcadia Characters Folder (Furcadia uses My Documents/Furcadia/Furcadia Characters but we could use our own location Like If
    ///our clients want To include a character file With a bot In a zip package
    ///</para>
    ///<para>
    ///Furcadia Setting Folder  (Currently Unused)</para>
    ///<para>
    ///Furcadia Temporary Cache, Temp Dreams, Custom Portraits, Main Maps, Dynamic Avatars (Currently unused)
    ///</para>
    ///<para>
    ///Silver Monkey Application Path (Default DS tempates,DS Scripts, MS Templates, MS Scripts) Defaul Program Settings IE Keys.ini MS_Keys.ini</para>
    ///<para> Silver Monkey Documents (BotFiles[bini,ms,db,txt(Memberlist,PounceList)],Scripts,Templates)</para>
    ///<para> Silver Monkey Settings Folder (User Application Data)</para>
    ///<para> Active Bot folder (Default To Current  folder where .bini Is, Preset To Documents\Silver Monkey)</para>
    ///<para> Search Paths, (File Browse Dialogs)</para>
    ///<para> Furcadia install path Program Files(x86)</para>
    ///<para> Monkey Speak Bots (Default Documents\Silver Monkey)</para>
    ///<para> TODO: Exceptions Handling</para>
    ///<para> Furcadia Paths need To be valid</para>
    ///<para> Let Furcadia.IO.Paths Populate our Default settings we can Override the Settings For our use Like If
    ///we have multiple Furcadia installations Like DevClient And Live Client. Perhaps we want the Bot To
    /// Use its own Furcadia Stash due To mangling the Furcadia Settings  With a localdir.ini installation</para>
    ///</remarks>
    public sealed class Paths
    {
        #region Private Fields

        private const string DsScriptsDocsPath = "Scripts";

        private const string DsTemplateDocsPath = "Templates";

        private const string ErrorLogPath = "Error";

        private const string LogPath = "Logs";

        private const string MsScriptseDocsPath = "Scripts-MS";

        private const string MsTemplateDocsPath = "Templates-MS";

        private const string MyDocumentsPath = "Silver Monkey";

        private const string PluginPath = "Plugins";

        private const string SettingsPath = "TSProjects/Silver Monkey";

        private static string _ApplicationPath = null;

        private static string _ApplicationPluginPath = null;

        private static string _ApplicationSettingsPath = null;

        private static string _FurcadiaCharactersFolder = null;

        private static string _FurcadiaDocumentsFolder = null;

        private static string _FurcadiaProgramFolder = null;

        private static string _FurcadiaSettingsFilder = null;

        private static string _MonKeySpeakEditorDocumentsDsScriptsPath;

        private static string _MonKeySpeakEditorDocumentsDsTemplatesPath;

        private static string _MonKeySpeakEditorDocumentsScriptsPath;

        private static string _MonKeySpeakEditorDocumentsTemplatesPath;

        private static string _MonKeySpeakEditorDsScriptsPath;

        private static string _MonKeySpeakEditorDsTemplatesPath;

        private static string _MonKeySpeakEditorScriptsPath;

        private static string _MonKeySpeakEditorTemplatesPath;

        private static Furcadia.IO.Paths _Paths = new Furcadia.IO.Paths();

        // Bot folder
        // check current Folder Presumeably Current folder or settings last bot folder
        //  settings last bot folder Save path for all programs last save?
        // Check Documents folder
        // ask User what folder to use
        private static string _SilverMonkeyBotPath = null;

        private static string _SilverMonkeyDocumentsPath;

        private static string _SilverMonkeyErrorLogPath = null;

        private static string _SilverMonkeyLogPath = null;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Silver Monkey Program Files Folder
        /// </summary>
        /// <value>
        /// The application path.
        /// </value>
        public static string ApplicationPath
        {
            get
            {
                if (string.IsNullOrEmpty(_ApplicationPath))
                {
                    _ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
                }

                return _ApplicationPath;
            }
        }

        /// <summary>
        /// Gets the application plugin path.
        /// </summary>
        /// <value>
        /// The application plugin path.
        /// </value>
        public static string ApplicationPluginPath
        {
            get
            {
                if (string.IsNullOrEmpty(_ApplicationPluginPath))
                {
                    _ApplicationPluginPath = Path.Combine(ApplicationPath, PluginPath);
                }

                return _ApplicationPluginPath;
            }
        }

        /// <summary>
        /// Gets the application settings path.
        /// </summary>
        /// <value>
        /// The application settings path.
        /// </value>
        public static string ApplicationSettingsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_ApplicationSettingsPath))
                {
                    _ApplicationSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SettingsPath);
                    if (!Directory.Exists(_ApplicationSettingsPath))
                    {
                        Directory.CreateDirectory(_ApplicationSettingsPath);
                    }
                }

                return _ApplicationSettingsPath;
            }
        }

        /// <summary>
        /// Gets or sets the furcadia characters folder.
        /// </summary>
        /// <value>
        /// The furcadia characters folder.
        /// </value>
        /// <exception cref="System.IO.DirectoryNotFoundException">This needs to be a valid folder</exception>
        public static string FurcadiaCharactersFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_FurcadiaCharactersFolder))
                {
                    if ((_Paths == null))
                    {
                        _Paths = new Furcadia.IO.Paths(_FurcadiaProgramFolder);
                    }

                    _FurcadiaCharactersFolder = _Paths.CharacterPath;
                }

                return _FurcadiaCharactersFolder;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException("This needs to be a valid folder");
                }

                _FurcadiaCharactersFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the furcadia documents folder.
        /// </summary>
        /// <value>
        /// The furcadia documents folder.
        /// </value>
        /// <exception cref="System.IO.DirectoryNotFoundException">This needs to be a valid folder</exception>
        public static string FurcadiaDocumentsFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_FurcadiaDocumentsFolder))
                {
                    return _FurcadiaDocumentsFolder;
                }

                if ((_Paths == null))
                {
                    _Paths = new Furcadia.IO.Paths(_FurcadiaProgramFolder);
                }

                _FurcadiaDocumentsFolder = _Paths.PersonalDataPath;
                return _FurcadiaDocumentsFolder;
            }
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException("This needs to be a valid folder");
                }

                _FurcadiaDocumentsFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the furcadia program folder.
        /// </summary>
        /// <value>
        /// The furcadia program folder.
        /// </value>
        public static string FurcadiaProgramFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_FurcadiaProgramFolder))
                {
                    _FurcadiaProgramFolder = _Paths.FurcadiaPath;
                }

                return _FurcadiaProgramFolder;
            }
            set
            {
                _FurcadiaProgramFolder = value;
            }
        }

        /// <summary>
        /// Gets the furcadia settings path.
        /// </summary>
        /// <value>
        /// The furcadia settings path.
        /// </value>
        public static string FurcadiaSettingsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_FurcadiaSettingsFilder))
                {
                    return _FurcadiaSettingsFilder;
                }

                if ((_Paths == null))
                {
                    _Paths = new Furcadia.IO.Paths(_FurcadiaSettingsFilder);
                }

                _FurcadiaSettingsFilder = _Paths.SettingsPath;
                return _FurcadiaSettingsFilder;
            }
        }

        /// <summary>
        /// Gets or sets the monkey speak editor documents ds scripts path.
        /// </summary>
        /// <value>
        /// The monkey speak editor documents ds scripts path.
        /// </value>
        public static string MonkeySpeakEditorDocumentsDsScriptsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorDocumentsScriptsPath))
                {
                    _MonKeySpeakEditorDocumentsDsScriptsPath = Path.Combine(SilverMonkeyDocumentsPath, DsScriptsDocsPath);
                }

                return _MonKeySpeakEditorDocumentsDsScriptsPath;
            }
            set
            {
                _MonKeySpeakEditorDocumentsDsScriptsPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the monkey speak editor documents ds templates path.
        /// </summary>
        /// <value>
        /// The monkey speak editor documents ds templates path.
        /// </value>
        public static string MonkeySpeakEditorDocumentsDsTemplatesPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorDocumentsDsTemplatesPath))
                {
                    _MonKeySpeakEditorDocumentsDsTemplatesPath = Path.Combine(SilverMonkeyDocumentsPath, DsTemplateDocsPath);
                }

                return _MonKeySpeakEditorDocumentsDsTemplatesPath;
            }
            set
            {
                _MonKeySpeakEditorDocumentsDsTemplatesPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the monkey speak editor documents scripts path.
        /// </summary>
        /// <value>
        /// The monkey speak editor documents scripts path.
        /// </value>
        public static string MonkeySpeakEditorDocumentsScriptsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorDocumentsScriptsPath))
                {
                    _MonKeySpeakEditorDocumentsScriptsPath = Path.Combine(SilverMonkeyDocumentsPath, MsScriptseDocsPath);
                    if (!Directory.Exists(_MonKeySpeakEditorDocumentsScriptsPath))
                    {
                        Directory.CreateDirectory(_MonKeySpeakEditorDocumentsScriptsPath);
                    }
                }

                return _MonKeySpeakEditorDocumentsScriptsPath;
            }
            set
            {
                _MonKeySpeakEditorDocumentsScriptsPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the monkey speak editor documents templates path.
        /// </summary>
        /// <value>
        /// The monkey speak editor documents templates path.
        /// </value>
        public static string MonkeySpeakEditorDocumentsTemplatesPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorDocumentsTemplatesPath))
                {
                    _MonKeySpeakEditorDocumentsTemplatesPath = Path.Combine(SilverMonkeyDocumentsPath, MsTemplateDocsPath);
                }

                return _MonKeySpeakEditorDocumentsTemplatesPath;
            }
            set
            {
                _MonKeySpeakEditorDocumentsTemplatesPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the monkey speak editor ds scripts path.
        /// </summary>
        /// <value>
        /// The monkey speak editor ds scripts path.
        /// </value>
        public static string MonkeySpeakEditorDsScriptsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorDsScriptsPath))
                {
                    _MonKeySpeakEditorDsScriptsPath = Path.Combine(ApplicationPath, DsScriptsDocsPath);
                }

                return _MonKeySpeakEditorDsScriptsPath;
            }
            set
            {
                _MonKeySpeakEditorDsScriptsPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the mon key speak editor ds templates path.
        /// </summary>
        /// <value>
        /// The mon key speak editor ds templates path.
        /// </value>
        public static string MonKeySpeakEditorDsTemplatesPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorDsTemplatesPath))
                {
                    _MonKeySpeakEditorDsTemplatesPath = Path.Combine(ApplicationPath, DsTemplateDocsPath);
                }

                return _MonKeySpeakEditorDsTemplatesPath;
            }
            set
            {
                _MonKeySpeakEditorDsTemplatesPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the mon key speak editor scripts path.
        /// </summary>
        /// <value>
        /// The mon key speak editor scripts path.
        /// </value>
        public static string MonKeySpeakEditorScriptsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorScriptsPath))
                {
                    _MonKeySpeakEditorScriptsPath = Path.Combine(ApplicationPath, MsScriptseDocsPath);
                    if (!Directory.Exists(_MonKeySpeakEditorScriptsPath))
                    {
                        Directory.CreateDirectory(_MonKeySpeakEditorScriptsPath);
                    }
                }

                return _MonKeySpeakEditorScriptsPath;
            }
            set
            {
                _MonKeySpeakEditorScriptsPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the mon key speak editor templates path.
        /// </summary>
        /// <value>
        /// The mon key speak editor templates path.
        /// </value>
        public static string MonKeySpeakEditorTemplatesPath
        {
            get
            {
                if (string.IsNullOrEmpty(_MonKeySpeakEditorTemplatesPath))
                {
                    _MonKeySpeakEditorTemplatesPath = Path.Combine(ApplicationPath, MsTemplateDocsPath);
                }

                return _MonKeySpeakEditorTemplatesPath;
            }
            set
            {
                _MonKeySpeakEditorTemplatesPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the silver monkey bot path.
        /// </summary>
        /// <value>
        /// The silver monkey bot path.
        /// </value>
        public static string SilverMonkeyBotPath
        {
            get
            {
                if (string.IsNullOrEmpty(_SilverMonkeyBotPath))
                {
                    _SilverMonkeyBotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath);
                }

                return _SilverMonkeyBotPath;
            }
            set
            {
                _SilverMonkeyBotPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the silver monkey documents path.
        /// </summary>
        /// <value>
        /// The silver monkey documents path.
        /// </value>
        public static string SilverMonkeyDocumentsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_SilverMonkeyDocumentsPath))
                {
                    _SilverMonkeyDocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath);
                }

                if (!Directory.Exists(_SilverMonkeyDocumentsPath))
                {
                    Directory.CreateDirectory(_SilverMonkeyDocumentsPath);
                }

                return _SilverMonkeyDocumentsPath;
            }
            set
            {
                _SilverMonkeyDocumentsPath = value;
            }
        }

        /// <summary>
        /// Gets the silver monkey error log path.
        /// </summary>
        /// <value>
        /// The silver monkey error log path.
        /// </value>
        public static string SilverMonkeyErrorLogPath
        {
            get
            {
                if ((_SilverMonkeyErrorLogPath == null))
                {
                    _SilverMonkeyDocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath);
                    _SilverMonkeyErrorLogPath = Path.Combine(_SilverMonkeyDocumentsPath, ErrorLogPath);
                    if (!Directory.Exists(_SilverMonkeyErrorLogPath))
                    {
                        Directory.CreateDirectory(_SilverMonkeyErrorLogPath);
                    }
                }

                return _SilverMonkeyErrorLogPath;
            }
        }

        /// <summary>
        /// Gets or sets the silver monkey log path.
        /// </summary>
        /// <value>
        /// The silver monkey log path.
        /// </value>
        public static string SilverMonkeyLogPath
        {
            get
            {
                if ((_SilverMonkeyLogPath == null))
                {
                    _SilverMonkeyDocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath);
                    _SilverMonkeyLogPath = Path.Combine(_SilverMonkeyDocumentsPath, LogPath);
                    if (!Directory.Exists(_SilverMonkeyLogPath))
                    {
                        Directory.CreateDirectory(_SilverMonkeyLogPath);
                    }
                }

                return _SilverMonkeyLogPath;
            }
            set
            {
                _SilverMonkeyLogPath = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Special Functoin for getting the default paths
        /// <para>
        /// Ideal logic is to check the location of the Bot.Bini file for the
        /// current path and then check the Silver Monkey folder in my documents
        /// </para>
        /// </summary>
        /// <param name="FileToCheck">The file to check.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public static string CheckBotFolder(ref string FileToCheck)
        {
            string FilePath = Path.GetDirectoryName(FileToCheck);
            if (string.IsNullOrEmpty(FilePath))
            {
                FileToCheck = Path.Combine(SilverMonkeyBotPath, FileToCheck);
            }

            return FileToCheck;
        }

        /// <summary>
        /// Check the Furcadia character folder for a character
        /// </summary>
        /// <param name="FileToCheck">The file to check.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public static string CheckCharacterFolder(ref string FileToCheck)
        {
            if (string.IsNullOrWhiteSpace(FileToCheck))
            {
                throw new FileNotFoundException(FileToCheck);
            }

            string FilePath = Path.GetDirectoryName(FileToCheck);
            if (string.IsNullOrEmpty(FilePath))
            {
                FileToCheck = Path.Combine(FurcadiaCharactersFolder, FileToCheck);
            }

            return FileToCheck;
        }

        #endregion Public Methods
    }
}