Imports MonkeyCore.IniFile
Imports MonkeyCore.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO


'Structure Data (Main Settings, Individual Program Groups)
'Read Master settings.ini
'read keys.ini/MS_keys.ini for MS Editor/Bot default settings ie Default MonkeySpeak file
' Write default monkey Speak file for bot.
'Read Bot MonkeySpeak File

'Default Sqlite Database Tables SQL Syntax.
' Protect from Advanced User Modes?
'FURRE
' Dream/Bot Key/Value Pairs
' User Specified Key/Value pairs
' Phoenix Speak Backup Tables

Public Class Settings

    Private Const FurcadiaPath As String = "Furcadia"
    Private Const Main_Host As String = "lightbringer.Furcadia.com"



    Private Const SettingFile As String = "Settings.Ini"
    Private Const MainSection As String = "Main"
    Private Const MSEditorSection As String = "Editor"
    Private Const MonkeySpeak As String = "MonkeySpeak"
    Private Const DragonSpeak As String = "DragonSpeak"

    Private Shared _ini As New IniFile
    Private Shared _KeysIni As New IniFile
    Private Shared _MS_KeysIni As New IniFile

    Public Shared Property ini As IniFile
        Get

            Return _ini
        End Get
        Set(ByVal value As IniFile)
            _ini = value
        End Set
    End Property

    Public Shared Property KeysIni As IniFile
        Get

            Return _KeysIni
        End Get
        Set(value As IniFile)
            _KeysIni = value
        End Set
    End Property

    Public Shared Property MS_KeysIni As IniFile
        Get
            Return _MS_KeysIni
        End Get
        Set(value As IniFile)
            _MS_KeysIni = value
        End Set
    End Property

    Public Class EditSettings
        Private SettingsFile As String = Path.Combine(Paths.ApplicationSettingsPath, SettingFile)
        Private _FurcPath As String
        'Dragon Speak
        Private _IDcolor As Color
        Private _CommentColor As Color
        Private _StringColor As Color
        Private _NumberColor As Color
        Private _VariableColor As Color
        Private _StringVariableColor As Color

        'Monkey Speak
        Private _MS_IDcolor As Color
        Private _MS_CommentColor As Color
        Private _MS_StringColor As Color
        Private _MS_NumberColor As Color
        Private _MS_VariableColor As Color

        Private _AutoCompleteEnable As Boolean

        Private KeysIni As IniFile = Settings.KeysIni
        Private MS_KeysIni As IniFile = Settings.MS_KeysIni
        Public Property FurcPath As String
            Get
                Return Paths.FurcadiaProgramFolder
            End Get
            Set(value As String)
                Paths.FurcadiaProgramFolder = value
            End Set
        End Property

        Public Property AutoCompleteEnable As Boolean
            Get
                Return _AutoCompleteEnable
            End Get
            Set(value As Boolean)
                _AutoCompleteEnable = value
            End Set
        End Property

        Public Property IDColor() As Color
            Get
                Return _IDcolor
            End Get
            Set(ByVal value As Color)
                _IDcolor = value
            End Set
        End Property
        Public Property StringColor() As Color
            Get
                Return _StringColor
            End Get
            Set(ByVal value As Color)
                _StringColor = value
            End Set
        End Property
        Public Property VariableColor() As Color
            Get
                Return _VariableColor
            End Get
            Set(ByVal value As Color)
                _VariableColor = value
            End Set
        End Property
        Public Property StringVariableColor() As Color
            Get
                Return _StringVariableColor
            End Get
            Set(ByVal value As Color)
                _StringVariableColor = value
            End Set
        End Property
        Public Property NumberColor() As Color
            Get
                Return _NumberColor
            End Get
            Set(ByVal value As Color)
                _NumberColor = value
            End Set
        End Property
        Public Property CommentColor() As Color
            Get
                Return _CommentColor
            End Get
            Set(ByVal value As Color)
                _CommentColor = value
            End Set
        End Property

        'MonkeySpeak
        Public Property MS_IDColor() As Color
            Get
                Return _MS_IDcolor
            End Get
            Set(ByVal value As Color)
                _MS_IDcolor = value
            End Set
        End Property
        Public Property MS_StringColor() As Color
            Get
                Return _MS_StringColor
            End Get
            Set(ByVal value As Color)
                _MS_StringColor = value
            End Set
        End Property
        Public Property MS_VariableColor() As Color
            Get
                Return _MS_VariableColor
            End Get
            Set(ByVal value As Color)
                _MS_VariableColor = value
            End Set
        End Property

        Public Property MS_NumberColor() As Color
            Get
                Return _MS_NumberColor
            End Get
            Set(ByVal value As Color)
                _MS_NumberColor = value
            End Set
        End Property
        Public Property MS_CommentColor() As Color
            Get
                Return _MS_CommentColor
            End Get
            Set(ByVal value As Color)
                _MS_CommentColor = value
            End Set
        End Property

        Public Property PluginList As Dictionary(Of String, Boolean)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="EditSettings"/> class.
        ''' </summary>
        Public Sub New()
            ' Load defaults
            '
            Dim s As String = ""
            ini.AddSection(MSEditorSection).AddKey("IDColor").Value = Color.Blue.ToArgb.ToString
            ini.AddSection(MSEditorSection).AddKey("CommentColor").Value = Color.Green.ToArgb.ToString
            ini.AddSection(MSEditorSection).AddKey("StringColor").Value = Color.Red.ToArgb.ToString
            ini.AddSection(MSEditorSection).AddKey("VariableColor").Value = Color.DarkGray.ToArgb.ToString
            ini.AddSection(MSEditorSection).AddKey("StringVariableColor").Value = Color.Blue.ToArgb.ToString
            ini.AddSection(MSEditorSection).AddKey("NumberColor").Value = Color.Brown.ToArgb.ToString

            'MonkeySpeak
            ini.AddSection(MonkeySpeak).AddKey("IDColor").Value = Color.Blue.ToArgb.ToString
            ini.AddSection(MonkeySpeak).AddKey("CommentColor").Value = Color.Green.ToArgb.ToString
            ini.AddSection(MonkeySpeak).AddKey("StringColor").Value = Color.Red.ToArgb.ToString
            ini.AddSection(MonkeySpeak).AddKey("VariableColor").Value = Color.DarkGray.ToArgb.ToString
            ini.AddSection(MonkeySpeak).AddKey("NumberColor").Value = Color.Brown.ToArgb.ToString.ToString

            ini.AddSection(MSEditorSection).AddKey("AutoComplete").Value = True.ToString

            'Load DS  settings from Keys.ini in the application folder
            Dim Count As Integer = 0
            Integer.TryParse(KeysIni.GetKeyValue("Init-Types", "Count"), Count)
            ini.AddSection("Init-Types").AddKey("Count").Value = Count.ToString

            For i As Integer = 1 To Count
                Dim key As String = KeysIni.GetKeyValue("Init-Types", i.ToString)
                Dim val As String = KeysIni.GetKeyValue("Indent-Lookup", key)
                ini.SetKeyValue("Ident-LookUp", key, i.ToString)
                ini.SetKeyValue("Init-Types", i.ToString, key)

                Dim dvalue As Integer = 0
                Integer.TryParse(KeysIni.GetKeyValue("C-Indents", key), dvalue)

                ini.AddSection("C-Indents")
                ini.SetKeyValue("C-Indents", key, dvalue.ToString)

            Next

            'Load Monkey Speak defaults from MS_Keys.ini in the application folder
            Count = 0
            Integer.TryParse(MS_KeysIni.GetKeyValue("Init-Types", "Count"), Count)
            ini.AddSection("MS-Init-Types").AddKey("Count").Value = Count.ToString
            For i As Integer = 1 To Count
                Dim key As String = MS_KeysIni.GetKeyValue("Init-Types", i.ToString)
                Dim val As String = MS_KeysIni.GetKeyValue("Indent-Lookup", key)
                ini.AddSection("MS-Init-Types").AddKey(i.ToString).Value = key
                ini.AddSection("MS-Indent-Lookup").AddKey(key).Value = val

                Dim dvalue As Integer = 0
                Integer.TryParse(MS_KeysIni.GetKeyValue("C-Indents", key), dvalue)
                ini.AddSection("MS-C-Indents").AddKey(key).Value = dvalue.ToString

            Next

            ' Defaults are now loaded
            ' Lets Read local appData Settings.ini for  last used Settings and Override existing settings
            If System.IO.File.Exists(SettingsFile) Then
                ini.Load(SettingsFile, True)
            End If



            'Dragon Speak
            _IDcolor = ColorTranslator.FromHtml(ini.GetKeyValue(MSEditorSection, "IDColor"))
            _CommentColor = ColorTranslator.FromHtml(ini.GetKeyValue(MSEditorSection, "CommentColor"))
            _StringColor = ColorTranslator.FromHtml(ini.GetKeyValue(MSEditorSection, "StringColor"))
            _VariableColor = ColorTranslator.FromHtml(ini.GetKeyValue(MSEditorSection, "VariableColor"))
            _StringVariableColor = ColorTranslator.FromHtml(ini.GetKeyValue(MSEditorSection, "StringVariableColor"))
            _NumberColor = ColorTranslator.FromHtml(ini.GetKeyValue(MSEditorSection, "NumberColor"))

            'Monkey Speak
            _MS_IDcolor = ColorTranslator.FromHtml(ini.GetKeyValue(MonkeySpeak, "IDColor"))
            _MS_CommentColor = ColorTranslator.FromHtml(ini.GetKeyValue(MonkeySpeak, "CommentColor"))
            _MS_StringColor = ColorTranslator.FromHtml(ini.GetKeyValue(MonkeySpeak, "StringColor"))
            _MS_VariableColor = ColorTranslator.FromHtml(ini.GetKeyValue(MonkeySpeak, "VariableColor"))
            _MS_NumberColor = ColorTranslator.FromHtml(ini.GetKeyValue(MonkeySpeak, "NumberColor"))

            _AutoCompleteEnable = Convert.ToBoolean(ini.GetKeyValue(MSEditorSection, "AutoComplete"))

            Dim DSSection As IniSection = ini.GetSection("plugins")
            If Not IsNothing(DSSection) Then
                PluginList = New Dictionary(Of String, Boolean)
                For Each K As IniSection.IniKey In DSSection.Keys
                    s = K.Value
                    If Not String.IsNullOrEmpty(s) Then
                        PluginList.Add(K.Name, Convert.ToBoolean(K.Value))
                    Else
                        PluginList.Add(K.Name, True)
                    End If
                Next
            End If

            s = ini.GetKeyValue(MainSection, "FurcPath")
            If Not String.IsNullOrEmpty(s) Then _FurcPath = s
        End Sub

        Public Sub SaveEditorSettings()

            ' Lets Read local appData Settings.ini for  last used Settings as other programs use the file too
            If System.IO.File.Exists(SettingsFile) Then
                ini.Load(SettingsFile, True)
            End If
            'Main Settings
            ini.SetKeyValue(MainSection, "FurcPath", _FurcPath)

            ini.SetKeyValue(MSEditorSection, "IDColor", ColorTranslator.ToHtml(_IDcolor).ToString)
            ini.SetKeyValue(MSEditorSection, "NumberColor", ColorTranslator.ToHtml(_NumberColor).ToString)
            ini.SetKeyValue(MSEditorSection, "StringColor", ColorTranslator.ToHtml(_StringColor).ToString)
            ini.SetKeyValue(MSEditorSection, "VariableColor", ColorTranslator.ToHtml(_VariableColor).ToString)
            ini.SetKeyValue(MSEditorSection, "StringVariableColor", ColorTranslator.ToHtml(_StringVariableColor).ToString)
            ini.SetKeyValue(MSEditorSection, "CommentColor", ColorTranslator.ToHtml(_CommentColor).ToString)

            ini.SetKeyValue(MonkeySpeak, "IDColor", ColorTranslator.ToHtml(_MS_IDcolor).ToString)
            ini.SetKeyValue(MonkeySpeak, "NumberColor", ColorTranslator.ToHtml(_MS_NumberColor).ToString)
            ini.SetKeyValue(MonkeySpeak, "StringColor", ColorTranslator.ToHtml(_MS_StringColor).ToString)
            ini.SetKeyValue(MonkeySpeak, "VariableColor", ColorTranslator.ToHtml(_MS_VariableColor).ToString)
            ini.SetKeyValue(MonkeySpeak, "CommentColor", ColorTranslator.ToHtml(_MS_CommentColor).ToString)

            ini.SetKeyValue(MSEditorSection, "AutoComplete", _AutoCompleteEnable.ToString)

            ini.RemoveSection("Plugins")
            For Each kv As KeyValuePair(Of String, Boolean) In PluginList
                ini.SetKeyValue("Plugins", kv.Key, kv.Value.ToString)
            Next

            ini.Save(SettingsFile)
        End Sub

    End Class

    Public Class cMain
        Private SettingsFile As String = Path.Combine(Paths.ApplicationSettingsPath, SettingFile)
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
                If String.IsNullOrEmpty(_FurcPath) Then
                    _FurcPath = Paths.FurcadiaProgramFolder
                End If
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
            If System.IO.File.Exists(SettingsFile) Then ini.Load(SettingsFile)

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
                Integer.TryParse(_sPort, s)
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

            Dim DSSection As IniSection = ini.GetSection("Plugins")
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
            ' Lets Read local appData Settings.ini for  last used Settings as other programs use the file too
            If System.IO.File.Exists(SettingsFile) Then
                ini.Load(SettingsFile, True)
            End If
            ini.SetKeyValue("Main", "Host", _FurcPath)
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
            ini.RemoveSection("Plugins")
            'ini.AddSection("Plugins")
            For Each kv As KeyValuePair(Of String, Boolean) In PluginList
                ini.SetKeyValue("Plugins", kv.Key, kv.Value.ToString)
            Next

            ini.Save(SettingsFile)
        End Sub

    End Class

    Public Class cBot

        Private _IniFile As String = ""

        Private _MS_Engine_Enable As Boolean = False
        Private _MsFileName As String = ""
        Private _MS_Script As String = ""
        Private _BotController As String = ""

        Private _FormattedOptions As String = ""
        Private _StandAlone As Boolean = False
        Private _AutoConnect As Boolean = False

        Private Property BotIni As IniFile

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


        ''' <summary>
        ''' Gets or sets the m s_ file.
        ''' </summary>
        ''' <value>
        ''' The ms file name.
        ''' </value>
        Public Property MS_File As String
            Get
                Return _MsFileName
            End Get
            Set(value As String)
                If Not value.ToLower().EndsWith(".ms") Then
                    Throw New ArgumentException("Invalid File type, Not a ""*.ms"" file.")
                End If
                _MsFileName = value
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
            If BotIni Is Nothing Then BotIni = New IniFile
            If System.IO.File.Exists(Path.Combine(Paths.SilverMonkeyBotPath, BFile)) Then
                Paths.SilverMonkeyBotPath = Path.GetDirectoryName(BFile)
                BotIni.Load(BFile)
            End If
            IniFile = BFile
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
            If Not String.IsNullOrEmpty(s) Then
                _lPort = 0
                Integer.TryParse(s, _lPort)
            End If

            s = BotIni.GetKeyValue("Bot", "MS_File")
            If Not String.IsNullOrEmpty(s) Then _MsFileName = s

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
            If Not String.IsNullOrEmpty(s) Then
                _GoMap = 0
                Integer.TryParse(s, _GoMap)
            End If

            s = BotIni.GetKeyValue("GoMap", "DreamURL")
            If Not String.IsNullOrEmpty(s) Then _DreamURL = s

        End Sub
        Public Sub SaveBotSettings()
            If System.IO.File.Exists(Path.Combine(Paths.SilverMonkeyBotPath, IniFile)) Then
                BotIni.Load(Path.Combine(Paths.SilverMonkeyBotPath, IniFile))
            End If

            BotIni.SetKeyValue("Main", "Log", _log.ToString)
            BotIni.SetKeyValue("Main", "LogNameBase", _logNamebase)
            BotIni.SetKeyValue("Main", "LogOption", _logOption.ToString)
            BotIni.SetKeyValue("Main", "LogNamePath", _logPath)
            BotIni.SetKeyValue("Bot", "BotIni", _IniFile)
            BotIni.SetKeyValue("Bot", "MS_File", _MsFileName)
            BotIni.SetKeyValue("Bot", "LPort", _lPort.ToString)
            BotIni.SetKeyValue("Bot", "MSEngineEnable", _MS_Engine_Enable.ToString)
            BotIni.SetKeyValue("Bot", "BotController", _BotController)
            BotIni.SetKeyValue("Bot", "StandAlone", _StandAlone.ToString)
            BotIni.SetKeyValue("Bot", "AutoConnect", _AutoConnect.ToString)

            BotIni.SetKeyValue("GoMap", "IDX", _GoMap.ToString)
            BotIni.SetKeyValue("GoMap", "DreamURL", _DreamURL)

            BotIni.Save(Path.Combine(Paths.SilverMonkeyBotPath, IniFile))
        End Sub
    End Class

End Class
