Imports System.IO
Imports Furcadia.IO
Imports Furcadia.Net
Imports MonkeyCore
Imports MonkeyCore.Utils.Logging

<CLSCompliant(True)>
Public Class BotOptions : Inherits Options.ProxySessionOptions

#Region "Private Fields"

    Private _AutoConnect As Boolean
    Private _BiniFile As String
    Private _DreamURL As String
    Private _GoMap As Integer
    Private _MonkeySpeakEngineOption As Engine.EngineOptoons

    Private Property BotIni As IniFile

    ''' <summary>
    ''' Silver Monkey Logging options
    ''' </summary>
    ''' <returns></returns>
    Public Property LogOptions As LogSteamOptions

#End Region

#Region "Public Constructors"

    Sub New()
        _MonkeySpeakEngineOption = New Engine.EngineOptoons()
        LogOptions = New LogSteamOptions() With {
        .LogPath = MonkeyCore.Paths.SilverMonkeyLogPath
        }
        BotIni = New IniFile()
    End Sub

    Public Sub New(ByRef BFile As String)
        Me.New()

        Dim dir As String = Path.GetDirectoryName(BFile)
        If String.IsNullOrEmpty(dir) Then
            BFile = Path.Combine(MonkeyCore.Paths.SilverMonkeyBotPath, BFile)
        End If
        If BotIni Is Nothing Then BotIni = New IniFile
        If File.Exists(MonkeyCore.Paths.CheckBotFolder(BFile)) Then
            Dim p As String = Path.GetDirectoryName(BFile)
            If Not String.IsNullOrEmpty(p) Then
                MonkeyCore.Paths.SilverMonkeyBotPath = p
            End If
            BotIni.Load(BFile)
        End If
        _BiniFile = BFile
        Dim s As String = ""
        LogOptions = New LogSteamOptions() With {
                 .LogNameBase = BotIni.GetKeyValue("Main", "LogNameBase"),
                 .LogPath = BotIni.GetKeyValue("Main", "LogNamePath")
        }
        LogOptions.LogOption = Integer.Parse(BotIni.GetKeyValue("Main", "LogOption"))
        Boolean.TryParse(BotIni.GetKeyValue("Main", "Log"), LogOptions.log)
        s = BotIni.GetKeyValue("Bot", "BotIni")
        If Not String.IsNullOrEmpty(s) Then CharacterIniFile = s

        s = BotIni.GetKeyValue("Bot", "LPort")
        If Not String.IsNullOrEmpty(s) Then
            LocalhostPort = 6700
            Integer.TryParse(s, LocalhostPort)
        End If

        s = BotIni.GetKeyValue("Bot", "MS_File")
        If Not String.IsNullOrEmpty(s) Then _MonkeySpeakEngineOption.MonkeySpeakScriptFile = s

        s = BotIni.GetKeyValue("Bot", "MSEngineEnable")
        If Not String.IsNullOrEmpty(s) Then _MonkeySpeakEngineOption.MS_Engine_Enable = Convert.ToBoolean(s)

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
    ''' Sets the bot to auto connect to the serer
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
    ''' Dream Url to send the bot to when connected
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Property DreamLink As String
        Get
            Return _DreamURL
        End Get
        Set(value As String)
            _DreamURL = value
        End Set
    End Property

    ''' <summary>
    ''' Which map are we going to?
    ''' </summary>
    ''' <returns></returns>
    Public Property GoMapIDX() As Integer
        Get
            Return _GoMap
        End Get
        Set(ByVal value As Integer)
            _GoMap = value
        End Set
    End Property

#Region "Log Options"

    ''' <summary>
    ''' Bot File (*.bini) path
    ''' </summary>
    ''' <returns></returns>
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

    Public ReadOnly Property Name As String
        Get
            Return Path.GetFileName(_BiniFile)
        End Get
    End Property

#End Region

#End Region

#Region "Public Methods"

    Public Sub SaveBotSettings()
        If BotIni Is Nothing Then BotIni = New IniFile()

        'If File.Exists(Paths.CheckBotFolder(_BiniFile)) Then
        '    BotIni.Load(Paths.CheckBotFolder(_BiniFile))
        'End If
        BotIni.SetKeyValue("Main", "Log", LogOptions.log.ToString)
        BotIni.SetKeyValue("Main", "LogNameBase", LogOptions.LogNameBase)
        BotIni.SetKeyValue("Main", "LogOption", LogOptions.LogOption.ToString)
        BotIni.SetKeyValue("Main", "LogNamePath", LogOptions.LogPath)
        BotIni.SetKeyValue("Bot", "BotIni", CharacterIniFile)
        BotIni.SetKeyValue("Bot", "MS_File", _MonkeySpeakEngineOption.MonkeySpeakScriptFile)
        BotIni.SetKeyValue("Bot", "LPort", LocalhostPort.ToString)
        BotIni.SetKeyValue("Bot", "MSEngineEnable", _MonkeySpeakEngineOption.MS_Engine_Enable.ToString)
        BotIni.SetKeyValue("Bot", "BotController", _MonkeySpeakEngineOption.BotController)
        BotIni.SetKeyValue("Bot", "StandAlone", Standalone.ToString)
        BotIni.SetKeyValue("Bot", "AutoConnect", _AutoConnect.ToString)

        BotIni.SetKeyValue("GoMap", "IDX", _GoMap.ToString)
        BotIni.SetKeyValue("GoMap", "DreamURL", _DreamURL.ToString())

        BotIni.Save(MonkeyCore.Paths.CheckBotFolder(_BiniFile))
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