Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports Furcadia.IO
Imports Furcadia.IO.IniFile
Imports MonkeyCore2.IO

'Structure Data (Main Settings, Individual Program Groups)
'Read Master settings.ini
'read keys.ini/MS_keys.ini for MS Editor/Bot default settings ie Default MonkeySpeak file
' Write default Monkey Speak file for bot.
'Read Bot MonkeySpeak File

'Default Sqlite Database Tables SQL Syntax.
' Protect from Advanced User Modes?
'FURRE
' Dream/Bot Key/Value Pairs
' User Specified Key/Value pairs
' Phoenix Speak Backup Tables

<CLSCompliant(True)>
Public Class Settings

#Region "Private Fields"

    Private Const DragonSpeak As String = "DragonSpeak"
    Private Const FurcadiaPath As String = "Furcadia"
    Private Const Main_Host As String = "lightbringer.Furcadia.com"

    Private Const MainSection As String = "Main"
    Private Const MonkeySpeak As String = "MonkeySpeak"
    Private Const MSEditorSection As String = "Editor"
    Private Const SettingFile As String = "Settings.Ini"
    Private Shared _ini As New IniFile

    Private Shared _MS_KeysIni As New IniFile

    Private Shared _PluginList As New Dictionary(Of String, Boolean)
    Private Shared SettingsFile As String = Path.Combine(MonkeyCore2.IO.Paths.ApplicationSettingsPath, SettingFile)

#End Region

#Region "Public Properties"

    Public Shared Property ini As IniFile
        Get

            Return _ini
        End Get
        Set(ByVal value As IniFile)
            _ini = value
        End Set
    End Property

    ''' <summary>
    ''' Loads Key.ini file for Monkey Speak display
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Shared Property MS_KeysIni As IniFile
        Get
            Return _MS_KeysIni
        End Get
        Set(value As IniFile)
            _MS_KeysIni = value
        End Set
    End Property

#End Region

#Region "Public Classes"

    ''' <summary>
    ''' Main Configuration for every application in package
    ''' </summary>
    Public Class cMain

#Region "Private Fields"

        Private _Advertisment As Boolean = False
        Private _Announcement As Boolean = False
        Private _AppFont As New Font("Microsoft Sans Serif", 10, FontStyle.Regular)
        Private _AutoReconnect As Boolean = False
        Private _Broadcast As Boolean = False
        Private _CloseProc As Boolean = False
        Private _ConnectTimeOut As Integer = 45
        Private _debug As Boolean = False
        Private _defaultColor As Color = Color.Black
        Private _DisconnectPopupToggle As Boolean = True
        Private _emitColor As Color = Color.Blue
        Private _emoteColor As Color = Color.DarkCyan
        Private _errorColor As Color = Color.Red
        Private _FontFace As String = "Microsoft Sans Serif"

        'Display Settings
        Private _FontSize As Integer = 10

        Private _FurcPath As String = ""
        Private _Host As String = Main_Host
        Private _LoadLastBotFile As Boolean = False
        Private _ping As Integer = 300
        Private _PSShowClient As Boolean = True
        Private _PSShowMainWindow As Boolean = True
        Private _reconnectMax As Integer = 10
        Private _sayColor As Color = Color.DarkGoldenrod
        Private _shoutColor As Color = Color.DarkRed
        Private _sPort As Integer = 6500
        Private _TimeStamp As UShort = 0

        'systray Status
        Private _TrayIcon As CheckState = CheckState.Indeterminate

        'Throat Tired
        Private _TT_TimeOut As Integer = 90

        Private _whColor As Color = Color.Purple
        Private SettingsFile As String = Path.Combine(MonkeyCore2.IO.Paths.ApplicationSettingsPath, SettingFile)

#End Region

#Region "Public Constructors"

        Public Sub New()
            If File.Exists(SettingsFile) Then
                ini.Load(SettingsFile)
            End If

            Dim s As String = ""
            s = ini.GetKeyValue("Main", "Host")
            If Not String.IsNullOrEmpty(s) Then _Host = s.Trim

            s = ini.GetKeyValue("Main", "TT Interval")
            ' If Not String.IsNullOrEmpty(s) Then _TT_TimeInterval = s.ToInteger

            s = ini.GetKeyValue("Main", "TT TimeOut")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _TT_TimeOut)
            End If

            s = ini.GetKeyValue("Main", "SPort")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _sPort)
            End If

            s = ini.GetKeyValue("Main", "Time Out")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _reconnectMax)
            End If

            s = ini.GetKeyValue("Main", "Auto Reconnect")
            If Not String.IsNullOrEmpty(s) Then _AutoReconnect = Convert.ToBoolean(s)

            s = ini.GetKeyValue("Main", "AutoCloseProc")
            If Not String.IsNullOrEmpty(s) Then _CloseProc = Convert.ToBoolean(s)

            s = ini.GetKeyValue("Main", "Debug")
            If Not String.IsNullOrEmpty(s) Then _debug = Convert.ToBoolean(s)

            s = ini.GetKeyValue("Main", "TimeStamp")
            If Not String.IsNullOrEmpty(s) Then _TimeStamp = CUShort(Convert.ToInt16(s))

            s = ini.GetKeyValue("Main", "FontFace")
            If Not String.IsNullOrEmpty(s) Then _FontFace = s

            s = ini.GetKeyValue("Main", "FontSize")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _FontSize)
            End If

            _AppFont = New Font(_FontFace, _FontSize)

            s = ini.GetKeyValue("Main", "Ping")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _ping)
            End If

            s = ini.GetKeyValue("Main", "EmitColor")
            If Not String.IsNullOrEmpty(s) Then _emitColor = ColorTranslator.FromHtml(s)

            s = ini.GetKeyValue("Main", "SayColor")
            If Not String.IsNullOrEmpty(s) Then _sayColor = ColorTranslator.FromHtml(s)

            s = ini.GetKeyValue("Main", "ShoutColor")
            If Not String.IsNullOrEmpty(s) Then _shoutColor = ColorTranslator.FromHtml(s)

            s = ini.GetKeyValue("Main", "WhColor")
            If Not String.IsNullOrEmpty(s) Then _whColor = ColorTranslator.FromHtml(s)

            s = ini.GetKeyValue("Main", "DefaultColor")
            If Not String.IsNullOrEmpty(s) Then _defaultColor = ColorTranslator.FromHtml(s)

            s = ini.GetKeyValue("Main", "EmoteColor")
            If Not String.IsNullOrEmpty(s) Then _emoteColor = ColorTranslator.FromHtml(s)

            s = ini.GetKeyValue("Main", "FurcPath")
            If Not String.IsNullOrEmpty(s) Then _FurcPath = s

            s = ini.GetKeyValue("Main", "ConnectTimeOut")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _ConnectTimeOut)
            End If

            s = ini.GetKeyValue("Main", "SysTray")
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
            s = ini.GetKeyValue("Main", "Advertisment")
            If Not String.IsNullOrEmpty(s) Then _Advertisment = Convert.ToBoolean(s)
            s = ini.GetKeyValue("Main", "Broadcast")
            If Not String.IsNullOrEmpty(s) Then _Broadcast = Convert.ToBoolean(s)
            s = ini.GetKeyValue("Main", "Announcement")
            If Not String.IsNullOrEmpty(s) Then _Announcement = Convert.ToBoolean(s)
            s = ini.GetKeyValue("Main", "LoadLastBotFile")
            If Not String.IsNullOrEmpty(s) Then _LoadLastBotFile = Convert.ToBoolean(s)
            s = ini.GetKeyValue("Main", "DisconnectPopupToggle")
            If Not String.IsNullOrEmpty(s) Then _DisconnectPopupToggle = Convert.ToBoolean(s)
            s = ini.GetKeyValue("PhoenixSpeak", "ShowInClient")
            If Not String.IsNullOrEmpty(s) Then _PSShowClient = Convert.ToBoolean(s)

            s = ini.GetKeyValue("PhoenixSpeak", "ShowInMainWindow")
            If Not String.IsNullOrEmpty(s) Then _PSShowMainWindow = Convert.ToBoolean(s)

        End Sub

#End Region

#Region "Public Properties"

        Public Property Advertisment As Boolean
            Get
                Return _Advertisment
            End Get
            Set(value As Boolean)
                _Advertisment = value
            End Set
        End Property

        Public Property Announcement As Boolean
            Get
                Return _Announcement
            End Get
            Set(value As Boolean)
                _Announcement = value
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

        Public Property AutoReconnect As Boolean
            Get
                Return _AutoReconnect
            End Get
            Set(value As Boolean)
                _AutoReconnect = value
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

        Public Property CloseProc As Boolean
            Get
                Return _CloseProc
            End Get
            Set(value As Boolean)
                _CloseProc = value
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

        Public Property debug() As Boolean
            Get
                Return _debug
            End Get
            Set(ByVal value As Boolean)
                _debug = value
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

        Public Property DisconnectPopupToggle As Boolean
            Get
                Return _DisconnectPopupToggle
            End Get
            Set(value As Boolean)
                _DisconnectPopupToggle = value
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

        Public Property EmoteColor() As Color
            Get
                Return _emoteColor
            End Get
            Set(ByVal value As Color)
                _emoteColor = value
            End Set
        End Property

        Public Property ErrorColor() As Color
            Get
                Return _errorColor
            End Get
            Set(ByVal value As Color)
                _errorColor = value
            End Set
        End Property

        Public Property FurcPath As String
            Get
                If String.IsNullOrEmpty(_FurcPath) Then
                    Return MonkeyCore2.IO.Paths.FurcadiaProgramFolder
                End If
                Return _FurcPath
            End Get
            Set(value As String)
                _FurcPath = value
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

        Public Property LoadLastBotFile As Boolean
            Get
                Return _LoadLastBotFile
            End Get
            Set(value As Boolean)
                _LoadLastBotFile = value
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

        Public Property PSShowClient As Boolean
            Get
                Return _PSShowClient
            End Get
            Set(value As Boolean)
                _PSShowClient = value
            End Set
        End Property

        Public Property PSShowMainWindow As Boolean
            Get
                Return _PSShowMainWindow
            End Get
            Set(value As Boolean)
                _PSShowMainWindow = value
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

        Public Property sPort() As Integer
            Get
                Return _sPort
            End Get
            Set(ByVal value As Integer)
                _sPort = value
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

        'Time Stamp mode
        ' 0 = off
        ' 1 = time
        ' 2 = Date Time
#Disable Warning BC40027 ' Return type of function 'TimeStamp' is not CLS-compliant.

        Public Property TimeStamp() As UShort
#Enable Warning BC40027 ' Return type of function 'TimeStamp' is not CLS-compliant.
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As UShort)
                _TimeStamp = value
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

        Public Property WhColor() As Color
            Get
                Return _whColor
            End Get
            Set(ByVal value As Color)
                _whColor = value
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Save Application settings
        ''' </summary>
        Public Sub SaveMainSettings()
            ' Lets Read local appData Settings.ini for last used Settings as
            ' other programs use the file too
            If File.Exists(SettingsFile) Then
                ini.Load(SettingsFile, True)
            End If
            ini.SetKeyValue("Main", "FurcPath", _FurcPath)
            ini.SetKeyValue("Main", "Host", _Host)
            '_reconnectMax
            ini.SetKeyValue("Main", "Time Out", _reconnectMax.ToString)
            ini.SetKeyValue("Main", "SPort", _sPort.ToString)

            ini.SetKeyValue("Main", "AutoCloseProc", _CloseProc.ToString)
            ini.SetKeyValue("Main", "Auto Reconnect", _AutoReconnect.ToString)
            ini.SetKeyValue("Main", "Debug", _debug.ToString)

            ini.SetKeyValue("Main", "TimeStamp", _TimeStamp.ToString)
            ini.SetKeyValue("Main", "FontFace", ApFont.Name)
            ini.SetKeyValue("Main", "FontSize", ApFont.Size.ToString)
            ini.SetKeyValue("Main", "EmitColor", ColorTranslator.ToHtml(_emitColor).ToString)
            ini.SetKeyValue("Main", "SayColor", ColorTranslator.ToHtml(_sayColor).ToString)
            ini.SetKeyValue("Main", "ShoutColor", ColorTranslator.ToHtml(_shoutColor).ToString)
            ini.SetKeyValue("Main", "WhColor", ColorTranslator.ToHtml(_whColor).ToString)
            ini.SetKeyValue("Main", "DefaultColor", ColorTranslator.ToHtml(_defaultColor).ToString)
            ini.SetKeyValue("Main", "EmoteColor", ColorTranslator.ToHtml(_emoteColor).ToString)
            ini.SetKeyValue("Main", "SysTray", _TrayIcon.ToString)
            ini.SetKeyValue("Main", "TT TimeOut", _TT_TimeOut.ToString)
            ini.SetKeyValue("Main", "FurcPath", _FurcPath)
            ini.SetKeyValue("Main", "ConnectTimeOut", _ConnectTimeOut.ToString)
            ini.SetKeyValue("Main", "Ping", _ping.ToString)
            ini.SetKeyValue("Main", "Advertisment", _Advertisment.ToString)
            ini.SetKeyValue("Main", "Broadcast", _Broadcast.ToString)
            ini.SetKeyValue("Main", "Announcement", _Announcement.ToString)
            ini.SetKeyValue("Main", "LoadLastBotFile", _LoadLastBotFile.ToString)
            ini.SetKeyValue("Main", "DisconnectPopupToggle", _DisconnectPopupToggle.ToString)

            ini.SetKeyValue("PhoenixSpeak", "ShowInClient", _PSShowClient.ToString)
            ini.SetKeyValue("PhoenixSpeak", "ShowInMainWindow", _PSShowMainWindow.ToString)

            ini.Save(SettingsFile)
        End Sub

        Public Sub SetDefault()
            _debug = False
            _TimeStamp = 0
        End Sub

#End Region

    End Class

    Public Class EditSettings

#Region "Private Fields"

        Private MS_KeysIni As IniFile = Settings.MS_KeysIni

#End Region

    End Class

    <CLSCompliant(True)>
    Public Class MantisConnectSettings

#Region "Private Fields"

        Private _HttpPassword As String
        Private _HttpUserName As String
        Private _MantisPassword As String
        Private _MantisUserName As String
        Private _StoreSettings As Boolean

#End Region

#Region "Public Constructors"

        Public Sub New()
            If File.Exists(SettingsFile) Then
                ini.Load(SettingsFile, True)
            End If
            Dim MantisSection As IniSection = ini.GetSection("MantisConnect")

            Dim MantisKey As IniSection.IniKey = MantisSection.GetKey("HttpPassword")
            If Not String.IsNullOrEmpty(MantisKey.GetValue) Then
                _HttpPassword = MantisKey.GetValue
            End If
            MantisKey = MantisSection.GetKey("StoreSettings")
            If Not String.IsNullOrEmpty(MantisKey.GetValue) Then
                _StoreSettings = Boolean.Parse(MantisKey.GetValue)
            End If

            MantisKey = MantisSection.GetKey("HttpUserName")
            If Not String.IsNullOrEmpty(MantisKey.GetValue) Then
                _HttpUserName = MantisKey.GetValue
            End If
            MantisKey = MantisSection.GetKey("MantisUserName")
            If Not String.IsNullOrEmpty(MantisKey.GetValue) Then
                _MantisUserName = MantisKey.GetValue
            End If
            MantisKey = MantisSection.GetKey("MantisPassword")
            If Not String.IsNullOrEmpty(MantisKey.GetValue) Then
                _MantisPassword = MantisKey.GetValue
            End If

        End Sub

#End Region

#Region "Public Properties"

        Public Property HttpPassword As String
            Get
                Return _HttpPassword
            End Get
            Set(value As String)
                _HttpPassword = value
            End Set
        End Property

        Public Property HttpUserName As String
            Get
                Return _HttpUserName
            End Get
            Set(value As String)
                _HttpUserName = value
            End Set
        End Property

        Public Property MantisPassword As String
            Get
                Return _MantisPassword
            End Get
            Set(value As String)
                _MantisPassword = value
            End Set
        End Property

        Public Property MantisUserName As String
            Get
                Return _MantisUserName
            End Get
            Set(value As String)
                _MantisUserName = value
            End Set
        End Property

        Public Property StoreSettings As Boolean
            Get
                Return _StoreSettings
            End Get
            Set(value As Boolean)
                _StoreSettings = value
            End Set
        End Property

#End Region

#Region "Public Methods"

        Public Sub SaveSettings()
            ' Lets Read local appData Settings.ini for last used Settings as
            ' other programs use the file too
            If File.Exists(SettingsFile) Then
                ini.Load(SettingsFile, True)
            End If
            ini.SetKeyValue("Mantis", "StoreSettings", _StoreSettings.ToString)
            ini.SetKeyValue("Mantis", "HttpPassword", _HttpPassword)
            ini.SetKeyValue("Mantis", "HttpUserName", _HttpUserName)
            ini.SetKeyValue("Mantis", "MantisPassword", _MantisPassword)
            ini.SetKeyValue("Mantis", "MantisUserName", _MantisUserName)

            ini.Save(SettingsFile)
        End Sub

#End Region

    End Class

#End Region

End Class