Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports Furcadia.IO
Imports Furcadia.IO.IniFile

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

Public Class Settings

#Region "Private Fields"

    Private Const Main_Host As String = "lightbringer.Furcadia.com"

    Private Const MainSection As String = "Main"
    Private Const SettingFile As String = "Settings.Ini"
    Private Shared _ini As New IniFile

    Private Shared _MS_KeysIni As New IniFile

    Private Shared SettingsFile As String = Path.Combine(IO.Paths.ApplicationSettingsPath, SettingFile)

#End Region

#Region "Public Properties"

    Public Shared Property Ini As IniFile
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
    Public Class CMain

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
        Private SettingsFile As String = Path.Combine(IO.Paths.ApplicationSettingsPath, SettingFile)

#End Region

#Region "Public Constructors"

        Public Sub New()
            If File.Exists(SettingsFile) Then
                Ini.Load(SettingsFile)
            End If

            Dim s As String = ""
            s = Ini.GetKeyValue("Main", "Host")
            If Not String.IsNullOrEmpty(s) Then _Host = s.Trim

            s = Ini.GetKeyValue("Main", "TT Interval")
            ' If Not String.IsNullOrEmpty(s) Then _TT_TimeInterval = s.ToInteger

            s = Ini.GetKeyValue("Main", "TT TimeOut")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _TT_TimeOut)
            End If

            s = Ini.GetKeyValue("Main", "SPort")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _sPort)
            End If

            s = Ini.GetKeyValue("Main", "Time Out")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _reconnectMax)
            End If

            s = Ini.GetKeyValue("Main", "Auto Reconnect")
            If Not String.IsNullOrEmpty(s) Then _AutoReconnect = Convert.ToBoolean(s)

            s = Ini.GetKeyValue("Main", "AutoCloseProc")
            If Not String.IsNullOrEmpty(s) Then _CloseProc = Convert.ToBoolean(s)

            s = Ini.GetKeyValue("Main", "Debug")
            If Not String.IsNullOrEmpty(s) Then _debug = Convert.ToBoolean(s)

            s = Ini.GetKeyValue("Main", "TimeStamp")
            If Not String.IsNullOrEmpty(s) Then _TimeStamp = CUShort(Convert.ToInt16(s))

            s = Ini.GetKeyValue("Main", "FontFace")
            If Not String.IsNullOrEmpty(s) Then _FontFace = s

            s = Ini.GetKeyValue("Main", "FontSize")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _FontSize)
            End If

            _AppFont = New Font(_FontFace, _FontSize)

            s = Ini.GetKeyValue("Main", "Ping")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _ping)
            End If

            s = Ini.GetKeyValue("Main", "EmitColor")
            If Not String.IsNullOrEmpty(s) Then _emitColor = ColorTranslator.FromHtml(s)

            s = Ini.GetKeyValue("Main", "SayColor")
            If Not String.IsNullOrEmpty(s) Then _sayColor = ColorTranslator.FromHtml(s)

            s = Ini.GetKeyValue("Main", "ShoutColor")
            If Not String.IsNullOrEmpty(s) Then _shoutColor = ColorTranslator.FromHtml(s)

            s = Ini.GetKeyValue("Main", "WhColor")
            If Not String.IsNullOrEmpty(s) Then _whColor = ColorTranslator.FromHtml(s)

            s = Ini.GetKeyValue("Main", "DefaultColor")
            If Not String.IsNullOrEmpty(s) Then _defaultColor = ColorTranslator.FromHtml(s)

            s = Ini.GetKeyValue("Main", "EmoteColor")
            If Not String.IsNullOrEmpty(s) Then _emoteColor = ColorTranslator.FromHtml(s)

            s = Ini.GetKeyValue("Main", "FurcPath")
            If Not String.IsNullOrEmpty(s) Then _FurcPath = s

            s = Ini.GetKeyValue("Main", "ConnectTimeOut")
            If Not String.IsNullOrEmpty(s) Then
                Integer.TryParse(s, _ConnectTimeOut)
            End If

            s = Ini.GetKeyValue("Main", "SysTray")
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
            s = Ini.GetKeyValue("Main", "Advertisment")
            If Not String.IsNullOrEmpty(s) Then _Advertisment = Convert.ToBoolean(s)
            s = Ini.GetKeyValue("Main", "Broadcast")
            If Not String.IsNullOrEmpty(s) Then _Broadcast = Convert.ToBoolean(s)
            s = Ini.GetKeyValue("Main", "Announcement")
            If Not String.IsNullOrEmpty(s) Then _Announcement = Convert.ToBoolean(s)
            s = Ini.GetKeyValue("Main", "LoadLastBotFile")
            If Not String.IsNullOrEmpty(s) Then _LoadLastBotFile = Convert.ToBoolean(s)
            s = Ini.GetKeyValue("Main", "DisconnectPopupToggle")
            If Not String.IsNullOrEmpty(s) Then _DisconnectPopupToggle = Convert.ToBoolean(s)
            s = Ini.GetKeyValue("PhoenixSpeak", "ShowInClient")
            If Not String.IsNullOrEmpty(s) Then _PSShowClient = Convert.ToBoolean(s)

            s = Ini.GetKeyValue("PhoenixSpeak", "ShowInMainWindow")
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

        Public Property Debug() As Boolean
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
                    Return IO.Paths.FurcadiaProgramFolder
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

        ''' <summary>
        ''' Gets or sets the system tray.
        ''' </summary>
        ''' <value>
        ''' The system tray.
        ''' </value>
        Public Property SysTray As CheckState
            Get
                Return _TrayIcon
            End Get
            Set(value As CheckState)
                _TrayIcon = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the time stamp.
        ''' <para/>
        ''' 0 = off
        ''' <para/>
        ''' 1 = Time
        ''' <para/>
        ''' 2 = Date Time
        ''' </summary>
        ''' <value>
        ''' The time stamp.
        ''' </value>
        Public Property TimeStamp() As UShort
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
                Ini.Load(SettingsFile, True)
            End If
            Ini.SetKeyValue("Main", "FurcPath", _FurcPath)
            Ini.SetKeyValue("Main", "Host", _Host)
            '_reconnectMax
            Ini.SetKeyValue("Main", "Time Out", _reconnectMax.ToString)
            Ini.SetKeyValue("Main", "SPort", _sPort.ToString)

            Ini.SetKeyValue("Main", "AutoCloseProc", _CloseProc.ToString)
            Ini.SetKeyValue("Main", "Auto Reconnect", _AutoReconnect.ToString)
            Ini.SetKeyValue("Main", "Debug", _debug.ToString)

            Ini.SetKeyValue("Main", "TimeStamp", _TimeStamp.ToString)
            Ini.SetKeyValue("Main", "FontFace", ApFont.Name)
            Ini.SetKeyValue("Main", "FontSize", ApFont.Size.ToString)
            Ini.SetKeyValue("Main", "EmitColor", ColorTranslator.ToHtml(_emitColor).ToString)
            Ini.SetKeyValue("Main", "SayColor", ColorTranslator.ToHtml(_sayColor).ToString)
            Ini.SetKeyValue("Main", "ShoutColor", ColorTranslator.ToHtml(_shoutColor).ToString)
            Ini.SetKeyValue("Main", "WhColor", ColorTranslator.ToHtml(_whColor).ToString)
            Ini.SetKeyValue("Main", "DefaultColor", ColorTranslator.ToHtml(_defaultColor).ToString)
            Ini.SetKeyValue("Main", "EmoteColor", ColorTranslator.ToHtml(_emoteColor).ToString)
            Ini.SetKeyValue("Main", "SysTray", _TrayIcon.ToString)
            Ini.SetKeyValue("Main", "TT TimeOut", _TT_TimeOut.ToString)
            Ini.SetKeyValue("Main", "FurcPath", _FurcPath)
            Ini.SetKeyValue("Main", "ConnectTimeOut", _ConnectTimeOut.ToString)
            Ini.SetKeyValue("Main", "Ping", _ping.ToString)
            Ini.SetKeyValue("Main", "Advertisment", _Advertisment.ToString)
            Ini.SetKeyValue("Main", "Broadcast", _Broadcast.ToString)
            Ini.SetKeyValue("Main", "Announcement", _Announcement.ToString)
            Ini.SetKeyValue("Main", "LoadLastBotFile", _LoadLastBotFile.ToString)
            Ini.SetKeyValue("Main", "DisconnectPopupToggle", _DisconnectPopupToggle.ToString)

            Ini.SetKeyValue("PhoenixSpeak", "ShowInClient", _PSShowClient.ToString)
            Ini.SetKeyValue("PhoenixSpeak", "ShowInMainWindow", _PSShowMainWindow.ToString)

            Ini.Save(SettingsFile)
        End Sub

        Public Sub SetDefault()
            _debug = False
            _TimeStamp = 0
        End Sub

#End Region

    End Class

#End Region

End Class