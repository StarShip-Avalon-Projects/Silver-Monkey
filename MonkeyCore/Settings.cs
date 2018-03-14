using Furcadia.IO;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

// Structure Data (Main Settings, Individual Program Groups)
// Read Master settings.ini
// read keys.ini/MS_keys.ini for MS Editor/Bot default settings ie Default MonkeySpeak file
//  Write default Monkey Speak file for bot.
// Read Bot MonkeySpeak File
// Default Sqlite Database Tables SQL Syntax.
//  Protect from Advanced User Modes?
// FURRE
//  Dream/Bot Key/Value Pairs
//  User Specified Key/Value pairs
//  Phoenix Speak Backup Tables
public class Settings
{
    #region Private Fields

    private const string Main_Host = "lightbringer.Furcadia.com";

    private const string MainSection = "Main";

    private const string SettingFile = "Settings.Ini";
    private static string SettingsFile = Path.Combine(IO.Paths.ApplicationSettingsPath, SettingFile);

    #endregion Private Fields

    #region Public Properties

    public static IniFile Ini { get; set; } = new IniFile();
    public static IniFile MS_KeysIni { get; set; } = new IniFile();

    #endregion Public Properties

    #region Public Classes

    /// <summary>
    /// Main Configuration for every application in package
    /// </summary>
    public class CMain
    {
        #region Private Fields

        private bool _Advertisment = false;

        private bool _Announcement = false;

        private Font _AppFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);

        private bool _AutoReconnect = false;

        private bool _Broadcast = false;

        private bool _CloseProc = false;

        private int _ConnectTimeOut = 45;

        private bool _debug = false;

        private Color _defaultColor = Color.Black;

        private bool _DisconnectPopupToggle = true;

        private Color _emitColor = Color.Blue;

        private Color _emoteColor = Color.DarkCyan;

        private Color _errorColor = Color.Red;

        private string _FontFace = "Microsoft Sans Serif";

        private int _FontSize = 10;

        private string _FurcPath = "";
        private bool _LoadLastBotFile = false;

        private int _ping = 300;

        private bool _PSShowClient = true;

        private bool _PSShowMainWindow = true;

        private int _reconnectMax = 10;

        private Color _sayColor = Color.DarkGoldenrod;

        private Color _shoutColor = Color.DarkRed;

        private int _sPort = 6500;

        private ushort _TimeStamp = 0;

        // systray Status
        private CheckState _TrayIcon = CheckState.Indeterminate;

        // Throat Tired
        private int _TT_TimeOut = 90;

        private Color _whColor = Color.Purple;

        private string SettingsFile = Path.Combine(IO.Paths.ApplicationSettingsPath, SettingFile);

        #endregion Private Fields

        #region Public Constructors

        public CMain()
        {
            if (File.Exists(SettingsFile))
            {
                Ini.Load(SettingsFile);
            }

            string s = "";
            s = Ini.GetKeyValue("Main", "Host");
            if (!string.IsNullOrEmpty(s))
            {
                Host = s.Trim();
            }

            s = Ini.GetKeyValue("Main", "TT Interval");
            //  If Not String.IsNullOrEmpty(s) Then _TT_TimeInterval = s.ToInteger
            s = Ini.GetKeyValue("Main", "TT TimeOut");
            if (!string.IsNullOrEmpty(s))
            {
                int.TryParse(s, out _TT_TimeOut);
            }

            s = Ini.GetKeyValue("Main", "SPort");
            if (!string.IsNullOrEmpty(s))
            {
                int.TryParse(s, out _sPort);
            }

            s = Ini.GetKeyValue("Main", "Time Out");
            if (!string.IsNullOrEmpty(s))
            {
                int.TryParse(s, out _reconnectMax);
            }

            s = Ini.GetKeyValue("Main", "Auto Reconnect");
            if (!string.IsNullOrEmpty(s))
            {
                _AutoReconnect = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "AutoCloseProc");
            if (!string.IsNullOrEmpty(s))
            {
                _CloseProc = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "Debug");
            if (!string.IsNullOrEmpty(s))
            {
                _debug = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "TimeStamp");
            if (!string.IsNullOrEmpty(s))
            {
                _TimeStamp = ushort.Parse(s);
            }

            s = Ini.GetKeyValue("Main", "FontFace");
            if (!string.IsNullOrEmpty(s))
            {
                _FontFace = s;
            }

            s = Ini.GetKeyValue("Main", "FontSize");
            if (!string.IsNullOrEmpty(s))
            {
                int.TryParse(s, out _FontSize);
            }

            _AppFont = new Font(_FontFace, _FontSize);
            s = Ini.GetKeyValue("Main", "Ping");
            if (!string.IsNullOrEmpty(s))
            {
                int.TryParse(s, out _ping);
            }

            s = Ini.GetKeyValue("Main", "EmitColor");
            if (!string.IsNullOrEmpty(s))
            {
                _emitColor = ColorTranslator.FromHtml(s);
            }

            s = Ini.GetKeyValue("Main", "SayColor");
            if (!string.IsNullOrEmpty(s))
            {
                _sayColor = ColorTranslator.FromHtml(s);
            }

            s = Ini.GetKeyValue("Main", "ShoutColor");
            if (!string.IsNullOrEmpty(s))
            {
                _shoutColor = ColorTranslator.FromHtml(s);
            }

            s = Ini.GetKeyValue("Main", "WhColor");
            if (!string.IsNullOrEmpty(s))
            {
                _whColor = ColorTranslator.FromHtml(s);
            }

            s = Ini.GetKeyValue("Main", "DefaultColor");
            if (!string.IsNullOrEmpty(s))
            {
                _defaultColor = ColorTranslator.FromHtml(s);
            }

            s = Ini.GetKeyValue("Main", "EmoteColor");
            if (!string.IsNullOrEmpty(s))
            {
                _emoteColor = ColorTranslator.FromHtml(s);
            }

            s = Ini.GetKeyValue("Main", "FurcPath");
            if (!string.IsNullOrEmpty(s))
            {
                _FurcPath = s;
            }

            s = Ini.GetKeyValue("Main", "ConnectTimeOut");
            if (!string.IsNullOrEmpty(s))
            {
                int.TryParse(s, out _ConnectTimeOut);
            }

            s = Ini.GetKeyValue("Main", "SysTray");
            if (!string.IsNullOrEmpty(s))
            {
                switch (s.ToLower())
                {
                    case "checked":
                        _TrayIcon = CheckState.Checked;
                        break;

                    case "unchecked":
                        _TrayIcon = CheckState.Unchecked;
                        break;

                    default:
                        _TrayIcon = CheckState.Indeterminate;
                        break;
                }
            }

            s = Ini.GetKeyValue("Main", "Advertisment");
            if (!string.IsNullOrEmpty(s))
            {
                _Advertisment = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "Broadcast");
            if (!string.IsNullOrEmpty(s))
            {
                _Broadcast = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "Announcement");
            if (!string.IsNullOrEmpty(s))
            {
                _Announcement = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "LoadLastBotFile");
            if (!string.IsNullOrEmpty(s))
            {
                _LoadLastBotFile = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("Main", "DisconnectPopupToggle");
            if (!string.IsNullOrEmpty(s))
            {
                _DisconnectPopupToggle = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("PhoenixSpeak", "ShowInClient");
            if (!string.IsNullOrEmpty(s))
            {
                _PSShowClient = Convert.ToBoolean(s);
            }

            s = Ini.GetKeyValue("PhoenixSpeak", "ShowInMainWindow");
            if (!string.IsNullOrEmpty(s))
            {
                _PSShowMainWindow = Convert.ToBoolean(s);
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets Announcment Channel Enable
        /// </summary>
        /// <value>
        ///   <c>true</c> if announcement; otherwise, <c>false</c>.
        /// </value>
        public bool Announcement
        {
            get => _Announcement;
            set => _Announcement = value;
        }

        public Font ApFont
        {
            get => _AppFont;
            set => _AppFont = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic reconnect].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic reconnect]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoReconnect
        {
            get => _AutoReconnect;
            set => _AutoReconnect = value;
        }

        /// <summary>
        /// Ignore Broadcast channel?
        /// </summary>
        /// <value>
        ///   <c>true</c> if broadcast; otherwise, <c>false</c>.
        /// </value>
        public bool Broadcast
        {
            get => _Broadcast;
            set => _Broadcast = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [close proc].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [close proc]; otherwise, <c>false</c>.
        /// </value>
        public bool CloseProc
        {
            get => _CloseProc;
            set => _CloseProc = value;
        }

        /// <summary>
        /// Gets or sets the connect time out.
        /// </summary>
        /// <value>
        /// The connect time out.
        /// </value>
        public int ConnectTimeOut
        {
            get => _ConnectTimeOut;
            set => _ConnectTimeOut = value;
        }

        public bool Debug
        {
            get => _debug;
            set => _debug = value;
        }

        public Color DefaultColor
        {
            get => _defaultColor;
            set => _defaultColor = value;
        }

        public bool DisconnectPopupToggle
        {
            get => _DisconnectPopupToggle;
            set => _DisconnectPopupToggle = value;
        }

        public Color EmitColor
        {
            get => _emitColor;
            set => _emitColor = value;
        }

        public Color EmoteColor
        {
            get => _emoteColor;
            set => _emoteColor = value;
        }

        public Color ErrorColor
        {
            get => _errorColor;
            set => _errorColor = value;
        }

        public string FurcPath
        {
            set => _FurcPath = value;
            get
            {
                if (string.IsNullOrEmpty(_FurcPath))
                {
                    return IO.Paths.FurcadiaProgramFolder;
                }
                return _FurcPath;
            }
        }

        public string Host { get; set; } = Main_Host;

        public bool LoadLastBotFile
        {
            get => _LoadLastBotFile;
            set => _LoadLastBotFile = value;
        }

        public int Ping
        {
            get => _ping;
            set => _ping = value;
        }

        public bool PSShowClient
        {
            get => _PSShowClient;
            set => _PSShowClient = value;
        }

        public bool PSShowMainWindow
        {
            get => _PSShowMainWindow;
            set => _PSShowMainWindow = value;
        }

        public int ReconnectMax
        {
            get => _reconnectMax;
            set => _reconnectMax = value;
        }

        public Color SayColor
        {
            get => _sayColor;
            set => _sayColor = value;
        }

        public Color ShoutColor
        {
            get => _shoutColor;
            set => _shoutColor = value;
        }

        public int SPort
        {
            get => _sPort;
            set => _sPort = value;
        }

        public CheckState SysTray
        {
            get => _TrayIcon;
            set => _TrayIcon = value;
        }

        public ushort TimeStamp
        {
            get => _TimeStamp;
            set => _TimeStamp = value;
        }

        public int TT_TimeOut
        {
            get => _TT_TimeOut;
            set => _TT_TimeOut = value;
        }

        public Color WhColor
        {
            get => _whColor;
            set => _whColor = value;
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Save Application settings
        /// </summary>
        public void SaveMainSettings()
        {
            //  Lets Read local appData Settings.ini for last used Settings as
            //  other programs use the file too
            if (File.Exists(SettingsFile))
            {
                Ini.Load(SettingsFile, true);
            }

            Ini.SetKeyValue("Main", "FurcPath", _FurcPath);
            Ini.SetKeyValue("Main", "Host", Host);
            // _reconnectMax
            Ini.SetKeyValue("Main", "Time Out", _reconnectMax.ToString());
            Ini.SetKeyValue("Main", "SPort", _sPort.ToString());
            Ini.SetKeyValue("Main", "AutoCloseProc", _CloseProc.ToString());
            Ini.SetKeyValue("Main", "Auto Reconnect", _AutoReconnect.ToString());
            Ini.SetKeyValue("Main", "Debug", _debug.ToString());
            Ini.SetKeyValue("Main", "TimeStamp", _TimeStamp.ToString());
            Ini.SetKeyValue("Main", "FontFace", ApFont.Name);
            Ini.SetKeyValue("Main", "FontSize", ApFont.Size.ToString());
            Ini.SetKeyValue("Main", "EmitColor", ColorTranslator.ToHtml(_emitColor).ToString());
            Ini.SetKeyValue("Main", "SayColor", ColorTranslator.ToHtml(_sayColor).ToString());
            Ini.SetKeyValue("Main", "ShoutColor", ColorTranslator.ToHtml(_shoutColor).ToString());
            Ini.SetKeyValue("Main", "WhColor", ColorTranslator.ToHtml(_whColor).ToString());
            Ini.SetKeyValue("Main", "DefaultColor", ColorTranslator.ToHtml(_defaultColor).ToString());
            Ini.SetKeyValue("Main", "EmoteColor", ColorTranslator.ToHtml(_emoteColor).ToString());
            Ini.SetKeyValue("Main", "SysTray", _TrayIcon.ToString());
            Ini.SetKeyValue("Main", "TT TimeOut", _TT_TimeOut.ToString());
            Ini.SetKeyValue("Main", "FurcPath", _FurcPath);
            Ini.SetKeyValue("Main", "ConnectTimeOut", _ConnectTimeOut.ToString());
            Ini.SetKeyValue("Main", "Ping", _ping.ToString());
            Ini.SetKeyValue("Main", "Advertisment", _Advertisment.ToString());
            Ini.SetKeyValue("Main", "Broadcast", _Broadcast.ToString());
            Ini.SetKeyValue("Main", "Announcement", _Announcement.ToString());
            Ini.SetKeyValue("Main", "LoadLastBotFile", _LoadLastBotFile.ToString());
            Ini.SetKeyValue("Main", "DisconnectPopupToggle", _DisconnectPopupToggle.ToString());
            Ini.SetKeyValue("PhoenixSpeak", "ShowInClient", _PSShowClient.ToString());
            Ini.SetKeyValue("PhoenixSpeak", "ShowInMainWindow", _PSShowMainWindow.ToString());
            Ini.Save(SettingsFile);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}