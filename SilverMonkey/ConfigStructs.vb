Imports System
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports SilverMonkey.ConfigStructs
Imports Furcadia.IO
Imports SilverMonkey.IniFile
Imports Microsoft.VisualBasic

Public Class ConfigStructs
#Region "Properties"


#End Region
#Region "Const"
    Private Const Main_Host As String = "lightbringer.Furcadia.com"
#End Region
    Public Shared Function pPath() As String
        Return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                      "Furcadia Framework/SilverMonkey")
    End Function
    Public Shared SetFile As String = pPath() & "/Settings.Ini"
    Public Shared Function mPath() As String
        mPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + My_Docs

        If Not Directory.Exists(mPath) Then
            Directory.CreateDirectory(mPath)
        End If
        Return mPath
    End Function

    Private Shared _BotName As String = ""


    Public Class cMain

        Private _sPort As Integer = 6500
        Private _FurcPath As String = ""
        Private _Host As String = Main_Host

        Private _CloseProc As Boolean = False
        Private _reconnectMax As Integer = 10


        Private _debug As Boolean = False
        Private _TimeStamp As UShort = 0
        Private _AutoReconnect As Boolean = False
        Private _ConnectTimeOut As Integer = 45
        'Display Settings
        Private _FontSize As Integer = 10
        Private _FontFace As String = "Microsoft Sans Serif"
        Private _AppFont As New Font(_FontFace, _FontSize, FontStyle.Regular)

        Private _emitColor As Color = Color.Blue
        Private _sayColor As Color = Color.DarkGoldenrod
        Private _shoutColor As Color = Color.DarkRed
        Private _whColor As Color = Color.Purple
        Private _defaultColor As Color = Color.Black
        Private _emoteColor As Color = Color.DarkCyan

        'Throat Tired
        Private _TT_TimeOut As Integer = 90

        Private _ping As Integer = 300
        'systray Status
        Private _TrayIcon As CheckState = CheckState.Indeterminate

        Private _Broadcast As Boolean = False
        Private _Advertisment As Boolean = False
        Private _Announcement As Boolean = False

        Public Property AutoReconnect As Boolean
            Get
                Return _AutoReconnect
            End Get
            Set(value As Boolean)
                _AutoReconnect = value
            End Set
        End Property


        Public Property CloseProc As Boolean
            Get
                Return _CloseProc
            End Get
            Set(value As Boolean)
                _CloseProc = value
            End Set
        End Property
        Public Property ApFont As Font
            Get
                Return _AppFont
            End Get
            Set(value As Font)
                _AppFont = value
            End Set
        End Property


        Public Property EmitColor() As Color
            Get
                Return _emitColor
            End Get
            Set(ByVal value As Color)
                _emitColor = value
            End Set
        End Property
        Public Property SayColor() As Color
            Get
                Return _sayColor
            End Get
            Set(ByVal value As Color)
                _sayColor = value
            End Set
        End Property
        Public Property ShoutColor() As Color
            Get
                Return _shoutColor
            End Get
            Set(ByVal value As Color)
                _shoutColor = value
            End Set
        End Property
        Public Property WhColor() As Color
            Get
                Return _whColor
            End Get
            Set(ByVal value As Color)
                _whColor = value
            End Set
        End Property
        Public Property DefaultColor() As Color
            Get
                Return _defaultColor
            End Get
            Set(ByVal value As Color)
                _defaultColor = value
            End Set
        End Property
        Public Property EmoteColor() As Color
            Get
                Return _emoteColor
            End Get
            Set(ByVal value As Color)
                _emoteColor = value
            End Set
        End Property


        Public Property ReconnectMax() As Integer
            Get
                Return _reconnectMax
            End Get
            Set(ByVal value As Integer)
                _reconnectMax = value
            End Set
        End Property

        Public Property sPort() As Integer
            Get
                Return _sPort
            End Get
            Set(ByVal value As Integer)
                _sPort = value
            End Set
        End Property


        Public Property Host() As String
            Get
                Return _Host
            End Get
            Set(ByVal value As String)
                _Host = value
            End Set
        End Property

        Public Property Ping As Integer
            Get
                Return _ping
            End Get
            Set(value As Integer)
                _ping = value
            End Set
        End Property

        'Throat Tired
        Public Property TT_TimeOut As Integer
            Get
                Return _TT_TimeOut
            End Get
            Set(value As Integer)
                _TT_TimeOut = value
            End Set
        End Property


        Public Property debug() As Boolean
            Get
                Return _debug
            End Get
            Set(ByVal value As Boolean)
                _debug = value
            End Set
        End Property

        'Time Stamp mode
        ' 0 = off
        ' 1 = time
        ' 2 = Date Time
        Public Property TimeStamp() As UShort
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As UShort)
                _TimeStamp = value
            End Set
        End Property

        'systray Status
        Public Property SysTray As CheckState
            Get
                Return _TrayIcon
            End Get
            Set(value As CheckState)
                _TrayIcon = value
            End Set
        End Property

        Public Property FurcPath As String
            Get
                Return _FurcPath
            End Get
            Set(value As String)
                _FurcPath = value
            End Set
        End Property

        Public Property ConnectTimeOut As Integer
            Get
                Return _ConnectTimeOut
            End Get
            Set(value As Integer)
                _ConnectTimeOut = value
            End Set
        End Property

        Public Sub SetDefault()
            _debug = False
            _TimeStamp = 0
        End Sub

        Public Property Announcement As Boolean
            Get
                Return _Announcement
            End Get
            Set(value As Boolean)
                _Announcement = value
            End Set
        End Property
        Public Property Broadcast As Boolean
            Get
                Return _Broadcast
            End Get
            Set(value As Boolean)
                _Broadcast = value
            End Set
        End Property
        Public Property Advertisment As Boolean
            Get
                Return _Advertisment
            End Get
            Set(value As Boolean)
                _Advertisment = value
            End Set
        End Property
        Private _LoadLastBotFile As Boolean = False
        Public Property LoadLastBotFile As Boolean
            Get
                Return _LoadLastBotFile
            End Get
            Set(value As Boolean)
                _LoadLastBotFile = value
            End Set
        End Property

        Private _DisconnectPopupToggle As Boolean = True
        Public Property DisconnectPopupToggle As Boolean
            Get
                Return _DisconnectPopupToggle
            End Get
            Set(value As Boolean)
                _DisconnectPopupToggle = value
            End Set



        End Property
        Private _PSShowMainWindow As Boolean = True
        Public Property PSShowMainWindow As Boolean
            Get
                Return _PSShowMainWindow
            End Get
            Set(value As Boolean)
                _PSShowMainWindow = value
            End Set
        End Property
        Private _PSShowClient As Boolean = True
        Public Property PSShowClient As Boolean
            Get
                Return _PSShowClient
            End Get
            Set(value As Boolean)
                _PSShowClient = value
            End Set
        End Property
        Dim _PluginList As New Dictionary(Of String, Boolean)
        Public Property PluginList As Dictionary(Of String, Boolean)
            Get
                Return _PluginList
            End Get
            Set(value As Dictionary(Of String, Boolean))
                _PluginList = value
            End Set
        End Property
        Public Sub New()
            System.IO.Directory.CreateDirectory(pPath())
            If File.Exists(SetFile) Then SettingsIni.Load(SetFile)

            Dim s As String = ""
            s = SettingsIni.GetKeyValue("Main", "Host")
            If Not String.IsNullOrEmpty(s) Then _Host = s.Trim

            s = SettingsIni.GetKeyValue("Main", "TT Interval")
           ' If Not String.IsNullOrEmpty(s) Then _TT_TimeInterval = s.ToInteger

            s = SettingsIni.GetKeyValue("Main", "TT TimeOut")
            If Not String.IsNullOrEmpty(s) Then _TT_TimeOut = s.ToInteger

            s = SettingsIni.GetKeyValue("Main", "SPort")
            If Not String.IsNullOrEmpty(s) Then _sPort = s.ToInteger

            s = SettingsIni.GetKeyValue("Main", "Time Out")
            If Not String.IsNullOrEmpty(s) Then _reconnectMax = s.ToInteger

            s = SettingsIni.GetKeyValue("Main", "Auto Reconnect")
            If Not String.IsNullOrEmpty(s) Then _AutoReconnect = Convert.ToBoolean(s)

            s = SettingsIni.GetKeyValue("Main", "AutoCloseProc")
            If Not String.IsNullOrEmpty(s) Then _CloseProc = Convert.ToBoolean(s)



            s = SettingsIni.GetKeyValue("Main", "Debug")
            If Not String.IsNullOrEmpty(s) Then _debug = Convert.ToBoolean(s)

            s = SettingsIni.GetKeyValue("Main", "TimeStamp")
            If Not String.IsNullOrEmpty(s) Then _TimeStamp = CUShort(Convert.ToInt16(s))

            s = SettingsIni.GetKeyValue("Main", "FontFace")
            If Not String.IsNullOrEmpty(s) Then _FontFace = s
            s = SettingsIni.GetKeyValue("Main", "FontSize")
            If Not String.IsNullOrEmpty(s) Then _FontSize = s.ToInteger
            _AppFont = New Font(_FontFace, _FontSize)

            s = SettingsIni.GetKeyValue("Main", "Ping")
            If Not String.IsNullOrEmpty(s) Then _ping = s.ToInteger

            s = SettingsIni.GetKeyValue("Main", "EmitColor")
            If Not String.IsNullOrEmpty(s) Then _emitColor = ColorTranslator.FromHtml(s)

            s = SettingsIni.GetKeyValue("Main", "SayColor")
            If Not String.IsNullOrEmpty(s) Then _sayColor = ColorTranslator.FromHtml(s)

            s = SettingsIni.GetKeyValue("Main", "ShoutColor")
            If Not String.IsNullOrEmpty(s) Then _shoutColor = ColorTranslator.FromHtml(s)

            s = SettingsIni.GetKeyValue("Main", "WhColor")
            If Not String.IsNullOrEmpty(s) Then _whColor = ColorTranslator.FromHtml(s)

            s = SettingsIni.GetKeyValue("Main", "DefaultColor")
            If Not String.IsNullOrEmpty(s) Then _defaultColor = ColorTranslator.FromHtml(s)

            s = SettingsIni.GetKeyValue("Main", "EmoteColor")
            If Not String.IsNullOrEmpty(s) Then _emoteColor = ColorTranslator.FromHtml(s)

            s = SettingsIni.GetKeyValue("Main", "FurcPath")
            If Not String.IsNullOrEmpty(s) Then _FurcPath = s

            s = SettingsIni.GetKeyValue("Main", "ConnectTimeOut")
            If Not String.IsNullOrEmpty(s) Then _ConnectTimeOut = s.ToInteger

            s = SettingsIni.GetKeyValue("Main", "SysTray")
            If Not String.IsNullOrEmpty(s) Then
                Select Case s.ToLower
                    Case "checked"
                        _TrayIcon = CheckState.Checked
                    Case "unchecked"
                        _TrayIcon = CheckState.Unchecked
                    Case Else
                        _TrayIcon = CheckState.Indeterminate
                End Select
            End If
            s = SettingsIni.GetKeyValue("Main", "Advertisment")
            If Not String.IsNullOrEmpty(s) Then _Advertisment = Convert.ToBoolean(s)
            s = SettingsIni.GetKeyValue("Main", "Broadcast")
            If Not String.IsNullOrEmpty(s) Then _Broadcast = Convert.ToBoolean(s)
            s = SettingsIni.GetKeyValue("Main", "Announcement")
            If Not String.IsNullOrEmpty(s) Then _Announcement = Convert.ToBoolean(s)
            s = SettingsIni.GetKeyValue("Main", "LoadLastBotFile")
            If Not String.IsNullOrEmpty(s) Then _LoadLastBotFile = Convert.ToBoolean(s)
            s = SettingsIni.GetKeyValue("Main", "DisconnectPopupToggle")
            If Not String.IsNullOrEmpty(s) Then _DisconnectPopupToggle = Convert.ToBoolean(s)
            s = SettingsIni.GetKeyValue("PhoenixSpeak", "ShowInClient")
            If Not String.IsNullOrEmpty(s) Then _PSShowClient = Convert.ToBoolean(s)

            s = SettingsIni.GetKeyValue("PhoenixSpeak", "ShowInMainWindow")
            If Not String.IsNullOrEmpty(s) Then _PSShowMainWindow = Convert.ToBoolean(s)

            Dim DSSection As IniSection = SettingsIni.GetSection("Plugins")
            If Not IsNothing(DSSection) Then
                PluginList.Clear()
                For Each K As IniSection.IniKey In DSSection.Keys
                    s = K.Value
                    If Not String.IsNullOrEmpty(s) Then
                        PluginList.Add(K.Name, Convert.ToBoolean(K.Value))
                    Else
                        PluginList.Add(K.Name, True)
                    End If
                Next
            End If
        End Sub
        Public Sub SaveMainSettings()
            SettingsIni.SetKeyValue("Main", "Host", _Host)
            '_reconnectMax
            SettingsIni.SetKeyValue("Main", "Time Out", _reconnectMax.ToString)
            SettingsIni.SetKeyValue("Main", "SPort", _sPort.ToString)

            SettingsIni.SetKeyValue("Main", "AutoCloseProc", _CloseProc.ToString)
            SettingsIni.SetKeyValue("Main", "Auto Reconnect", _AutoReconnect.ToString)
            SettingsIni.SetKeyValue("Main", "Debug", _debug.ToString)

            SettingsIni.SetKeyValue("Main", "TimeStamp", _TimeStamp.ToString)
            SettingsIni.SetKeyValue("Main", "FontFace", ApFont.Name)
            SettingsIni.SetKeyValue("Main", "FontSize", ApFont.Size.ToString)
            SettingsIni.SetKeyValue("Main", "EmitColor", ColorTranslator.ToHtml(_emitColor).ToString)
            SettingsIni.SetKeyValue("Main", "SayColor", ColorTranslator.ToHtml(_sayColor).ToString)
            SettingsIni.SetKeyValue("Main", "ShoutColor", ColorTranslator.ToHtml(_shoutColor).ToString)
            SettingsIni.SetKeyValue("Main", "WhColor", ColorTranslator.ToHtml(_whColor).ToString)
            SettingsIni.SetKeyValue("Main", "DefaultColor", ColorTranslator.ToHtml(_defaultColor).ToString)
            SettingsIni.SetKeyValue("Main", "EmoteColor", ColorTranslator.ToHtml(_emoteColor).ToString)
            SettingsIni.SetKeyValue("Main", "SysTray", _TrayIcon.ToString)
            SettingsIni.SetKeyValue("Main", "TT TimeOut", _TT_TimeOut.ToString)
            SettingsIni.SetKeyValue("Main", "FurcPath", _FurcPath)
            SettingsIni.SetKeyValue("Main", "ConnectTimeOut", _ConnectTimeOut.ToString)
            SettingsIni.SetKeyValue("Main", "Ping", _ping.ToString)
            SettingsIni.SetKeyValue("Main", "Advertisment", _Advertisment.ToString)
            SettingsIni.SetKeyValue("Main", "Broadcast", _Broadcast.ToString)
            SettingsIni.SetKeyValue("Main", "Announcement", _Announcement.ToString)
            SettingsIni.SetKeyValue("Main", "LoadLastBotFile", _LoadLastBotFile.ToString)
            SettingsIni.SetKeyValue("Main", "DisconnectPopupToggle", _DisconnectPopupToggle.ToString)

            SettingsIni.SetKeyValue("PhoenixSpeak", "ShowInClient", _PSShowClient.ToString)
            SettingsIni.SetKeyValue("PhoenixSpeak", "ShowInMainWindow", _PSShowMainWindow.ToString)
            SettingsIni.RemoveSection("Plugins")
            'SettingsIni.AddSection("Plugins")
            For Each kv As KeyValuePair(Of String, Boolean) In PluginList
                SettingsIni.SetKeyValue("Plugins", kv.Key, kv.Value.ToString)
            Next

            SettingsIni.Save(SetFile)
        End Sub

    End Class

    Public Class cBot

        Public BotFile As String = ""
        Private _IniFile As String = ""

        Private _MS_Engine_Enable As Boolean = False
        Private _MS_File As String = ""
        Private _MS_Script As String = ""
        Private _BotController As String = ""

        Private _FormattedOptions As String = ""
        Private _StandAlone As Boolean = False
        Private _AutoConnect As Boolean = False

        Private _log As Boolean
        Public Property log As Boolean
            Get
                Return _log
            End Get
            Set(ByVal value As Boolean)
                _log = value
            End Set
        End Property

        Private _logNamebase As String = "Default"
        Public Property LogNameBase As String
            Get
                Return _logNamebase
            End Get
            Set(value As String)
                _logNamebase = value
            End Set
        End Property

        Private _logPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\Silver Monkey\Logs"
        Public Property LogPath As String
            Get
                Return _logPath
            End Get
            Set(value As String)
                _logPath = value
            End Set
        End Property

        Private _logOption As Short
        Public Property LogOption As Short
            Get
                Return _logOption
            End Get
            Set(value As Short)
                _logOption = value
            End Set
        End Property

        Private _logIdx As Integer
        Public Property LogIdx As Integer
            Get
                Return _logIdx
            End Get
            Set(value As Integer)
                _logIdx = value
            End Set
        End Property



        Public Property AutoConnect As Boolean
            Get
                Return _AutoConnect
            End Get
            Set(value As Boolean)
                _AutoConnect = value
            End Set
        End Property

        Public Property StandAlone() As Boolean
            Get
                Return _StandAlone
            End Get
            Set(ByVal value As Boolean)
                _StandAlone = value
            End Set
        End Property

        Private _lPort As Integer = 6700
        Public Property lPort() As Integer
            Get
                Return _lPort
            End Get
            Set(ByVal value As Integer)
                _lPort = value
            End Set
        End Property

        Public Property IniFile() As String
            Get
                Return _IniFile
            End Get
            Set(ByVal value As String)
                _IniFile = value
            End Set
        End Property

        Public Property MS_Engine_Enable As Boolean
            Get
                Return _MS_Engine_Enable
            End Get
            Set(ByVal value As Boolean)
                _MS_Engine_Enable = value
            End Set
        End Property

        Public Property MS_File As String
            Get
                Return _MS_File
            End Get
            Set(value As String)
                _MS_File = value
            End Set
        End Property

        Public Property MS_Script As String
            Get
                Return _MS_Script
            End Get
            Set(value As String)
                _MS_Script = value
            End Set
        End Property

        Public Property BotController As String
            Get
                Return _BotController
            End Get
            Set(value As String)
                _BotController = value
            End Set
        End Property

        Private _GoMap As Integer = 3
        Public Property GoMapIDX() As Integer
            Get
                Return _GoMap
            End Get
            Set(ByVal value As Integer)
                _GoMap = value
            End Set
        End Property
        Private _DreamURL As String = "furc://"
        Public Property DreamURL As String
            Get
                Return _DreamURL
            End Get
            Set(value As String)
                _DreamURL = value
            End Set
        End Property
       

        Public Sub New()

        End Sub

        Public Sub New(ByRef BFile As String)
            If File.Exists(CheckMyDocFile(BFile)) Then BotIni.Load(CheckMyDocFile(BFile))
            BotFile = BFile
            Dim s As String = ""

            s = BotIni.GetKeyValue("Main", "Log")
            If Not String.IsNullOrEmpty(s) Then _log = Convert.ToBoolean(s)

            s = BotIni.GetKeyValue("Main", "LogOption")
            If Not String.IsNullOrEmpty(s) Then _logOption = Convert.ToInt16(s)

            s = BotIni.GetKeyValue("Main", "LogNameBase")
            If Not String.IsNullOrEmpty(s) Then _logNamebase = s

            s = BotIni.GetKeyValue("Main", "LogNamePath")
            If Not String.IsNullOrEmpty(s) Then _logPath = s


            s = BotIni.GetKeyValue("Bot", "BotIni")
            If Not String.IsNullOrEmpty(s) Then _IniFile = s

            s = BotIni.GetKeyValue("Bot", "LPort")
            If Not String.IsNullOrEmpty(s) Then _lPort = s.ToInteger

            s = BotIni.GetKeyValue("Bot", "MS_File")
            If Not String.IsNullOrEmpty(s) Then _MS_File = s

            s = BotIni.GetKeyValue("Bot", "MSEngineEnable")
            If Not String.IsNullOrEmpty(s) Then _MS_Engine_Enable = Convert.ToBoolean(s)

            s = BotIni.GetKeyValue("Bot", "BotController")
            If Not String.IsNullOrEmpty(s) Then _BotController = s

            s = BotIni.GetKeyValue("Bot", "StandAlone")
            If Not String.IsNullOrEmpty(s) Then _StandAlone = Convert.ToBoolean(s)

            s = BotIni.GetKeyValue("Bot", "AutoConnect")
            If Not String.IsNullOrEmpty(s) Then _AutoConnect = Convert.ToBoolean(s)

            s = BotIni.GetKeyValue("Bot", "NoEndurance")

            '  _options()
            s = BotIni.GetKeyValue("GoMap", "IDX")
            If Not String.IsNullOrEmpty(s) Then _GoMap = s.ToInteger

            s = BotIni.GetKeyValue("GoMap", "DreamURL")
            If Not String.IsNullOrEmpty(s) Then _DreamURL = s

        End Sub
        Public Sub SaveBotSettings()
            BotIni.SetKeyValue("Main", "Log", _log.ToString)
            BotIni.SetKeyValue("Main", "LogNameBase", _logNamebase)
            BotIni.SetKeyValue("Main", "LogOption", _logOption.ToString)
            BotIni.SetKeyValue("Main", "LogNamePath", _logPath)
            BotIni.SetKeyValue("Bot", "BotIni", _IniFile)
            BotIni.SetKeyValue("Bot", "MS_File", _MS_File)
            BotIni.SetKeyValue("Bot", "LPort", _lPort.ToString)
            BotIni.SetKeyValue("Bot", "MSEngineEnable", _MS_Engine_Enable.ToString)
            BotIni.SetKeyValue("Bot", "BotController", _BotController)
            BotIni.SetKeyValue("Bot", "StandAlone", _StandAlone.ToString)
            BotIni.SetKeyValue("Bot", "AutoConnect", _AutoConnect.ToString)

            BotIni.SetKeyValue("GoMap", "IDX", _GoMap.ToString)
            BotIni.SetKeyValue("GoMap", "DreamURL", _DreamURL)

            BotIni.Save(CheckMyDocFile(BotFile))
        End Sub
    End Class



End Class
