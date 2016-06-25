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

#End Region

    Public Class cBot

        Public BotFile As String = ""
        Private _IniFile As String = ""

        Private _MS_Engine_Enable As Boolean = False
        Private _MsFileName As String = ""
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
            If Not String.IsNullOrEmpty(s) Then _GoMap = s.ToInteger

            s = BotIni.GetKeyValue("GoMap", "DreamURL")
            If Not String.IsNullOrEmpty(s) Then _DreamURL = s

        End Sub
        Public Sub SaveBotSettings()

            If File.Exists(CheckMyDocFile(BotFile)) Then BotIni.Load(CheckMyDocFile(BotFile))

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

            BotIni.Save(CheckMyDocFile(BotFile))
        End Sub
    End Class



End Class
