'Silver Monkey standard Paths.
'This Class contains All the Shared paths for Furcadia Folders  and Silver Monkey Paths.
'Furcadia Programs (Furcadia.exe)
'Furcadia Documents Folder (DSC Templates/Scripts, Character Files)

'Furcadia Characters Folder (Furcadia uses My Documents/Furcadia/Furcadia Characters but we could use our own location Like if 
' our clients want to include a character file with a bot in a zip package

'Furcadia Settingd Folder  (Currently Unused)
'Furcadia Temporary Cache, Temp Dreams, Custom Portraits, Main Maps, Dynamic Avatars (Currently unused)
'Silver Monkey Application Path (default DStempates,DS Scripts, MS Templates, MS Scripts) Defaul Program Settings IE Keys.ini
'Silver Monkey Documents (BotFiles[bini,ms,db,txt(Memberlist,PounceList)],Scripts,Templates)
'Silver Monkey Settings Folder (User Application Data)

'Active Bot folder (default to Current  folder where .bini is, Preset to Documents\Silver Monkey)

'Search Paths, (File Browse Dialogs)
'Furcadia InStall path Program Files(x86)
'Monkey Speak Bots (Default Documents\Silver Monkey)

'TODO: Exceptions Handling
'Furcaida Paths need to be valid
' Let Furcadia.IO.Paths Populate our default settings we can Override the Settings for our use like if
' we have multiple Furcadia installations like DevClient and Live Client. Perhaps we want the Bot to 
' Use its own Furcadia Stash due to mangling the Furcadia Settings  with a localdir.ini installation

Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Win32

Public Class Paths

    'Settings folder in local AppData
    Private Const SettingsPath As String = "TSProjects/Silver Monkey"
    Private Const MyDocumentsPath As String = "Silver Monkey"

    Private Const MsTemplateDocsPath As String = "Templates-MS"
    Private Const MsScriptseDocsPath As String = "Scripts-MS"
    Private Const DsTemplateDocsPath As String = "Templates"
    Private Const DxScriptseDocsPath As String = "Scripts"

    Private Const PluginPath As String = "Plugins"

    Private Shared _SilverMonkeyDocumentsPath As String

    Private Const ErrorLogPath As String = "Error"

    'Bot folder
    'check current Folder Presumeably Current folder or settings last bot folder
    ' settings last bot folder Save path for all programs last save?
    'Check Documents folder
    'ask User what folder to use
    Private Shared _SilverMonkeyBotPath As String = Nothing
    Private Shared _ApplicationPath As String = Nothing
    Private Shared _ApplicationSettingsPath As String = Nothing
    Private Shared _ApplicationPluginPath As String = Nothing

    Private Shared _MonKeySpeakEditorTemplatesPath As String = Nothing
    Private Shared _MonKeySpeakEditorDocumentsTemplatesPath As String = Nothing

    Private Shared _MonKeySpeakEditorScriptsPath As String = Nothing
    Private Shared _MonKeySpeakEditorDocumentsScriptsPath As String = Nothing


    Private Shared _Paths As New Furcadia.IO.Paths()

    Private Shared _FurcadiaDocumentsFolder As String = Nothing
    Private Shared _FurcadiaProgramFolder As String = Nothing
    Private Shared _FurcadiaCharactersFolder As String = Nothing

    Private Shared _SilverMonkeyErrorLogPath As String = Nothing


    ''' <summary>
    ''' Gets or sets the furcadia documents folder.
    ''' </summary>
    ''' <value>
    ''' The furcadia documents folder.
    ''' </value>
    ''' <exception cref="System.IO.DirectoryNotFoundException">This needs to be a valid folder</exception>
    Public Shared Property FurcadiaDocumentsFolder() As String
        Get
            If Not String.IsNullOrEmpty(_FurcadiaDocumentsFolder) Then
                Return _FurcadiaDocumentsFolder
            End If
            If _Paths Is Nothing Then
                _Paths = New Furcadia.IO.Paths(_FurcadiaProgramFolder)
            End If
            _FurcadiaDocumentsFolder = _Paths.GetFurcadiaDocPath()
            Return _FurcadiaDocumentsFolder
        End Get
        Set(ByVal value As String)
            If Not Directory.Exists(value) Then
                Throw New DirectoryNotFoundException("This needs to be a valid folder")
            End If
            _FurcadiaDocumentsFolder = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the furcadia characters folder.
    ''' </summary>
    ''' <value>
    ''' The furcadia characters folder.
    ''' </value>
    ''' <exception cref="System.IO.DirectoryNotFoundException">This needs to be a valid folder</exception>
    Public Shared Property FurcadiaCharactersFolder() As String
        Get
            If String.IsNullOrEmpty(_FurcadiaCharactersFolder) Then
                If _Paths Is Nothing Then
                    _Paths = New Furcadia.IO.Paths(_FurcadiaProgramFolder)
                End If
                _FurcadiaCharactersFolder = _Paths.GetFurcadiaCharactersPath
            End If
            Return _FurcadiaCharactersFolder
        End Get
        Set(ByVal value As String)
            If Not Directory.Exists(value) Then
                Throw New DirectoryNotFoundException("This needs to be a valid folder")
            End If
            _FurcadiaDocumentsFolder = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the furcadia program folder.
    ''' </summary>
    ''' <value>
    ''' The furcadia program folder.
    ''' </value>
    ''' <exception cref="System.IO.FileNotFoundException">Furcadia.exe not found in path. Is this a Furcadia folder?</exception>
    Public Shared Property FurcadiaProgramFolder() As String
        Get
            If String.IsNullOrEmpty(_FurcadiaProgramFolder) Then
                Try
                    _FurcadiaProgramFolder = _Paths.GetInstallPath()
                Catch ex As Exception
                    Dim Broswe As New Windows.Forms.OpenFileDialog
                    Broswe.Title = "Locate Furcadia.exe"
                    Broswe.Filter = "Furcada.exe|Furcadia.exe"
                    Broswe.CheckPathExists = True
                    Broswe.CheckFileExists = True
                    'Check for Furcadia Install location.
                    If Environment.Is64BitOperatingSystem() Then
                        Broswe.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    Else
                        Broswe.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
                    End If

                    'Broswe.CheckFileExists = True
                    If Broswe.ShowDialog() = DialogResult.OK Then
                        Dim ThisPath As String = Path.GetDirectoryName(Broswe.FileName)
                        _Paths = New Furcadia.IO.Paths()
                        _FurcadiaProgramFolder = ThisPath
                    End If
                End Try

            End If
            Return _FurcadiaProgramFolder
        End Get
        Set(ByVal value As String)
            If Not File.Exists(Path.Combine(value, "Furcadia.exe")) Then
                'Throw New FileNotFoundException("Furcadia.exe not found in path. Is this a Furcadia folder?")
            End If
            _FurcadiaProgramFolder = value
        End Set

    End Property

    ''' <summary>
    ''' Silver Monkey Program Files  Folder
    ''' </summary>
    ''' <value>
    ''' The application path.
    ''' </value>
    Public Shared ReadOnly Property ApplicationPath() As String
        Get
            If String.IsNullOrEmpty(_ApplicationPath) Then
                _ApplicationPath = My.Application.Info.DirectoryPath
            End If
            Return _ApplicationPath
        End Get
    End Property

    ''' <summary>
    ''' Gets the application plugin path.
    ''' </summary>
    ''' <value>
    ''' The application plugin path.
    ''' </value>
    Public Shared ReadOnly Property ApplicationPluginPath() As String
        Get
            If String.IsNullOrEmpty(_ApplicationPluginPath) Then
                _ApplicationPluginPath = Path.Combine(ApplicationPath, PluginPath)
            End If
            Return _ApplicationPluginPath
        End Get
    End Property


    ''' <summary>
    ''' Silver Monkey Program Settings path (Local AppData)
    ''' </summary>
    ''' <value>
    ''' The application settings path.
    ''' </value>
    Public Shared ReadOnly Property ApplicationSettingsPath() As String
        Get
            If String.IsNullOrEmpty(_ApplicationSettingsPath) Then
                _ApplicationSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                          SettingsPath)
                If Not Directory.Exists(_ApplicationSettingsPath) Then
                    Directory.CreateDirectory(_ApplicationSettingsPath)
                End If
            End If
            Return _ApplicationSettingsPath
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the silver monkey documents path.
    ''' </summary>
    ''' <value>
    ''' The silver monkey documents path.
    ''' </value>
    Public Shared Property SilverMonkeyDocumentsPath() As String
        Get
            If String.IsNullOrEmpty(_SilverMonkeyDocumentsPath) Then
                _SilverMonkeyDocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath)
            End If
            If Not Directory.Exists(_SilverMonkeyDocumentsPath) Then
                Directory.CreateDirectory(_SilverMonkeyDocumentsPath)
            End If
            Return _SilverMonkeyDocumentsPath
        End Get
        Set(ByVal value As String)
            _SilverMonkeyDocumentsPath = value
        End Set

    End Property

    ''' <summary>
    ''' Gets or sets the silver monkey bot path.
    ''' </summary>
    ''' <value>
    ''' The silver monkey bot path.
    ''' </value>
    Public Shared Property SilverMonkeyBotPath() As String
        Get
            If String.IsNullOrEmpty(_SilverMonkeyBotPath) Then
                _SilverMonkeyBotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath)
            End If
            Return _SilverMonkeyBotPath
        End Get
        Set(ByVal value As String)
            _SilverMonkeyBotPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Monkey Speak templates path.
    ''' </summary>
    ''' <value>
    ''' The mon key speak editor templates path.
    ''' </value>
    Public Shared Property MonKeySpeakEditorTemplatesPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorTemplatesPath) Then
                _MonKeySpeakEditorTemplatesPath = Path.Combine(ApplicationPath, MsTemplateDocsPath)
            End If
            Return _MonKeySpeakEditorTemplatesPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorTemplatesPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the mon key Monkey Speak documents templates path.
    ''' </summary>
    ''' <value>
    ''' The mon key speak editor documents templates path.
    ''' </value>
    Public Shared Property MonKeySpeakEditorDocumentsTemplatesPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDocumentsTemplatesPath) Then
                _MonKeySpeakEditorDocumentsTemplatesPath = Path.Combine(SilverMonkeyDocumentsPath, MsTemplateDocsPath)
            End If
            Return _MonKeySpeakEditorDocumentsTemplatesPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorDocumentsTemplatesPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the mon key speak editor scripts path.
    ''' </summary>
    ''' <value>
    ''' The mon keyspeak editor scripts path.
    ''' </value>
    Public Shared Property MonKeySpeakEditorScriptsPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorScriptsPath) Then
                _MonKeySpeakEditorScriptsPath = Path.Combine(ApplicationPath, MsTemplateDocsPath)
                If Not Directory.Exists(_MonKeySpeakEditorScriptsPath) Then
                    Directory.CreateDirectory(_MonKeySpeakEditorScriptsPath)
                End If
            End If
            Return _MonKeySpeakEditorScriptsPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorScriptsPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the monkey speak editor documents scripts path.
    ''' </summary>
    ''' <value>
    ''' The mon key speak editor documents scripts path.
    ''' </value>
    Public Shared Property MonKeySpeakEditorDocumentsScriptsPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDocumentsScriptsPath) Then
                _MonKeySpeakEditorDocumentsScriptsPath = Path.Combine(SilverMonkeyDocumentsPath, MsTemplateDocsPath)
                If Not Directory.Exists(_MonKeySpeakEditorDocumentsScriptsPath) Then
                    Directory.CreateDirectory(_MonKeySpeakEditorDocumentsScriptsPath)
                End If
            End If
            Return _MonKeySpeakEditorDocumentsScriptsPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorDocumentsScriptsPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the silver monkey error log path.
    ''' </summary>
    ''' <value>
    ''' The silver monkey error log path.
    ''' </value>
    Public Shared ReadOnly Property SilverMonkeyErrorLogPath() As String
        Get
            If _SilverMonkeyErrorLogPath Is Nothing Then
                _SilverMonkeyDocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath)
                _SilverMonkeyErrorLogPath = Path.Combine(_SilverMonkeyDocumentsPath, ErrorLogPath)
                If Not Directory.Exists(_SilverMonkeyErrorLogPath) Then
                    Directory.CreateDirectory(_SilverMonkeyErrorLogPath)
                End If
            End If
            Return _SilverMonkeyErrorLogPath
        End Get
    End Property

    Public Shared Function CheckBotFolder(ByVal file As String) As String
        Dim Check As String = Path.GetDirectoryName(file)
        If String.IsNullOrEmpty(Check) Then
            Check = Path.Combine(Paths.SilverMonkeyBotPath, file)
            Return Check
        End If
        Return file
    End Function

End Class
