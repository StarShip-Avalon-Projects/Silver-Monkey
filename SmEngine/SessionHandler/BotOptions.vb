Imports System.IO
Imports Furcadia.Net
Imports MonkeyCore

<CLSCompliant(True)>
Public Class BotOptions : Inherits Options.ProxySessionOptions

#Region "Private Fields"

    Private _AutoConnect As Boolean
    Private _BiniFile As String
    Private _DreamURL As String
    Private _GoMap As Integer
    Private _MonkeySpeakEngineOption As Engine.EngineOptoons

    Private Property _BotController As String
    Private Property BotIni As IniFile

#Region "Log Options"

    Private _log As Boolean
    Private _logIdx As Integer
    Private _logNamebase As String
    Private _logOption As Short
    Private _logPath As String

#End Region

#End Region

#Region "Public Constructors"

    Sub New()
        _MonkeySpeakEngineOption = New Engine.EngineOptoons()
        _logPath = Paths.SilverMonkeyLogPath

    End Sub

    Public Sub New(ByRef BFile As String)
        _MonkeySpeakEngineOption = New Engine.EngineOptoons()
        If BotIni Is Nothing Then BotIni = New IniFile
        If File.Exists(Paths.CheckBotFolder(BFile)) Then
            Dim p As String = Path.GetDirectoryName(BFile)
            If Not String.IsNullOrEmpty(p) Then
                Paths.SilverMonkeyBotPath = p
            End If
            BotIni.Load(BFile)
        End If
        _BiniFile = BFile
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
        If Not String.IsNullOrEmpty(s) Then CharacterIniFile = s

        s = BotIni.GetKeyValue("Bot", "LPort")
        If Not String.IsNullOrEmpty(s) Then
            LocalhostPort = 0
            Integer.TryParse(s, LocalhostPort)
        End If

        s = BotIni.GetKeyValue("Bot", "MS_File")
        If Not String.IsNullOrEmpty(s) Then _MonkeySpeakEngineOption.MonkeySpeakScriptFile = s

        s = BotIni.GetKeyValue("Bot", "MSEngineEnable")
        If Not String.IsNullOrEmpty(s) Then _MonkeySpeakEngineOption.EngineEnable = Convert.ToBoolean(s)

        s = BotIni.GetKeyValue("Bot", "BotController")
        If Not String.IsNullOrEmpty(s) Then _MonkeySpeakEngineOption.BotController = s

        s = BotIni.GetKeyValue("Bot", "StandAlone")
        If Not String.IsNullOrEmpty(s) Then Standalone = Convert.ToBoolean(s)

        s = BotIni.GetKeyValue("Bot", "AutoConnect")
        If Not String.IsNullOrEmpty(s) Then _AutoConnect = Convert.ToBoolean(s)

        s = BotIni.GetKeyValue("Bot", "NoEndurance")

        ' _options()
        s = BotIni.GetKeyValue("GoMap", "IDX")
        If Not String.IsNullOrEmpty(s) Then
            _GoMap = 0
            Integer.TryParse(s, _GoMap)
        End If

        s = BotIni.GetKeyValue("GoMap", "DreamURL")
        If Not String.IsNullOrEmpty(s) Then _DreamURL = s

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Sets the bot to auto conect to the serer
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Property AutoConnect As Boolean
        Get
            Return _AutoConnect
        End Get
        Set(value As Boolean)
            _AutoConnect = value
        End Set
    End Property

    ''' <summary>
    ''' Furre controlling the Bot aka Bot Admin
    ''' </summary>
    ''' <returns>
    ''' Furcadia String
    ''' </returns>
    Public Property BotController() As String
        Get
            Return _MonkeySpeakEngineOption.BotController
        End Get
        Set(ByVal value As String)
            _MonkeySpeakEngineOption.BotController = value
        End Set
    End Property

    ''' <summary>
    ''' Dream Url to send the bot to when connected
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Property DreamURL As String
        Get
            Return _DreamURL
        End Get
        Set(value As String)
            _DreamURL = value
        End Set
    End Property

    Public Property GoMapIDX() As Integer
        Get
            Return _GoMap
        End Get
        Set(ByVal value As Integer)
            _GoMap = value
        End Set
    End Property

#Region "Log Options"

    Public ReadOnly Property BotPath As String
        Get
            Return Path.GetDirectoryName(_BiniFile)
        End Get
    End Property

    Public Property DestinationFile As String
        Get
            Return _BiniFile
        End Get
        Set(value As String)
            _BiniFile = value
        End Set
    End Property

    Public Property log As Boolean
        Get
            Return _log
        End Get
        Set(ByVal value As Boolean)
            _log = value
        End Set
    End Property

    Public Property LogIdx As Integer
        Get
            Return _logIdx
        End Get
        Set(value As Integer)
            _logIdx = value
        End Set
    End Property

    Public Property LogNameBase As String
        Get
            Return _logNamebase
        End Get
        Set(value As String)
            _logNamebase = value
        End Set
    End Property

    Public Property LogOption As Short
        Get
            Return _logOption
        End Get
        Set(value As Short)
            _logOption = value
        End Set
    End Property

    Public Property LogPath As String
        Get
            Return _logPath
        End Get
        Set(value As String)
            _logPath = value
        End Set
    End Property

    Public ReadOnly Property Name As String
        Get
            Return Path.GetFileName(_BiniFile)
        End Get
    End Property

#End Region

#End Region

#Region "Public Methods"

    Public Sub SaveBotSettings()
        BotIni = New IniFile()
        If File.Exists(Paths.CheckBotFolder(_BiniFile)) Then
            BotIni.Load(Paths.CheckBotFolder(_BiniFile))
        End If
        BotIni.SetKeyValue("Main", "Log", _log.ToString)
        BotIni.SetKeyValue("Main", "LogNameBase", _logNamebase)
        BotIni.SetKeyValue("Main", "LogOption", _logOption.ToString)
        BotIni.SetKeyValue("Main", "LogNamePath", _logPath)
        BotIni.SetKeyValue("Bot", "BotIni", CharacterIniFile)
        BotIni.SetKeyValue("Bot", "MS_File", _MonkeySpeakEngineOption.MonkeySpeakScriptFile)
        BotIni.SetKeyValue("Bot", "LPort", LocalhostPort.ToString)
        BotIni.SetKeyValue("Bot", "MSEngineEnable", _MonkeySpeakEngineOption.EngineEnable.ToString)
        BotIni.SetKeyValue("Bot", "BotController", _MonkeySpeakEngineOption.BotController)
        BotIni.SetKeyValue("Bot", "StandAlone", Standalone.ToString)
        BotIni.SetKeyValue("Bot", "AutoConnect", _AutoConnect.ToString)

        BotIni.SetKeyValue("GoMap", "IDX", _GoMap.ToString)
        BotIni.SetKeyValue("GoMap", "DreamURL", _DreamURL)

        BotIni.Save(Paths.CheckBotFolder(_BiniFile))
    End Sub

#End Region

#Region "Public Properties"

    Public Property MonkeySpeakEngineOptions As Engine.EngineOptoons
        Get
            Return _MonkeySpeakEngineOption
        End Get
        Set(ByVal value As Engine.EngineOptoons)
            _MonkeySpeakEngineOption = value
        End Set
    End Property

#End Region

End Class