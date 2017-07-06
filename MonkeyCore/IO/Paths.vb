Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Monkey System core File Paths
''' Silver Monkey standard Paths.
'''<para>
'''This Class contains All the Shared paths For Furcadia Folders  And Silver Monkey Paths.
'''</para>
'''<para>
'''Furcadia Programs (Furcadia.exe)
'''</para>
'''<para>
'''Furcadia Documents Folder (DSC Templates/Scripts, Character Files)
'''</para>
'''<para>
'''Furcadia Characters Folder (Furcadia uses My Documents/Furcadia/Furcadia Characters but we could use our own location Like If
'''our clients want To include a character file With a bot In a zip package
'''</para>
'''<para>
'''Furcadia Setting Folder  (Currently Unused)</para>
'''<para>
'''Furcadia Temporary Cache, Temp Dreams, Custom Portraits, Main Maps, Dynamic Avatars (Currently unused)
'''</para>
'''<para>
'''Silver Monkey Application Path (Default DS tempates,DS Scripts, MS Templates, MS Scripts) Defaul Program Settings IE Keys.ini MS_Keys.ini</para>
'''<para> Silver Monkey Documents (BotFiles[bini,ms,db,txt(Memberlist,PounceList)],Scripts,Templates)</para>
'''<para> Silver Monkey Settings Folder (User Application Data)</para>
'''<para> Active Bot folder (Default To Current  folder where .bini Is, Preset To Documents\Silver Monkey)</para>
'''<para> Search Paths, (File Browse Dialogs)</para>
'''<para> Furcadia install path Program Files(x86)</para>
'''<para> Monkey Speak Bots (Default Documents\Silver Monkey)</para>
'''<para> TODO: Exceptions Handling</para>
'''<para> Furcadia Paths need To be valid</para>
'''<para> Let Furcadia.IO.Paths Populate our Default settings we can Override the Settings For our use Like If
'''we have multiple Furcadia installations Like DevClient And Live Client. Perhaps we want the Bot To
''' Use its own Furcadia Stash due To mangling the Furcadia Settings  With a localdir.ini installation</para>
''' </summary>
Public Class Paths

#Region "Private Fields"

    Private Const DsScriptsDocsPath As String = "Scripts"
    Private Const DsTemplateDocsPath As String = "Templates"
    Private Const ErrorLogPath As String = "Error"

    Private Const LogPath As String = "Logs"

    Private Const MsScriptseDocsPath As String = "Scripts-MS"

    Private Const MsTemplateDocsPath As String = "Templates-MS"

    Private Const MyDocumentsPath As String = "Silver Monkey"

    Private Const PluginPath As String = "Plugins"

    'Settings folder in local AppData
    Private Const SettingsPath As String = "TSProjects/Silver Monkey"

    Private Shared _ApplicationPath As String = Nothing
    Private Shared _ApplicationPluginPath As String = Nothing
    Private Shared _ApplicationSettingsPath As String = Nothing
    Private Shared _FurcadiaCharactersFolder As String = Nothing
    Private Shared _FurcadiaDocumentsFolder As String = Nothing
    Private Shared _FurcadiaProgramFolder As String = Nothing

    Private Shared _MonKeySpeakEditorDocumentsDsScriptsPath As String
    Private Shared _MonKeySpeakEditorDocumentsDsTemplatesPath As String
    Private Shared _MonKeySpeakEditorDocumentsScriptsPath As String
    Private Shared _MonKeySpeakEditorDocumentsTemplatesPath As String
    Private Shared _MonKeySpeakEditorDsScriptsPath As String
    Private Shared _MonKeySpeakEditorDsTemplatesPath As String
    Private Shared _MonKeySpeakEditorScriptsPath As String
    Private Shared _MonKeySpeakEditorTemplatesPath As String
    Private Shared _Paths As New Furcadia.IO.Paths()

    'Bot folder
    'check current Folder Presumeably Current folder or settings last bot folder
    ' settings last bot folder Save path for all programs last save?
    'Check Documents folder
    'ask User what folder to use
    Private Shared _SilverMonkeyBotPath As String = Nothing

    Private Shared _SilverMonkeyDocumentsPath As String
    Private Shared _SilverMonkeyErrorLogPath As String = Nothing
    Private Shared _SilverMonkeyLogPath As String = Nothing

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Silver Monkey Program Files Folder
    ''' </summary>
    ''' <value>
    ''' The application path.
    ''' </value>
    Public Shared ReadOnly Property ApplicationPath() As String
        Get
            If String.IsNullOrEmpty(_ApplicationPath) Then
                _ApplicationPath = Application.StartupPath
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
    ''' Gets or sets the Furcadia characters folder.
    ''' </summary>
    ''' <value>
    ''' The Furcadia characters folder.
    ''' </value>
    ''' <exception cref="System.IO.DirectoryNotFoundException">
    ''' This needs to be a valid folder
    ''' </exception>
    Public Shared Property FurcadiaCharactersFolder() As String
        Get
            If String.IsNullOrEmpty(_FurcadiaCharactersFolder) Then
                If _Paths Is Nothing Then
                    _Paths = New Furcadia.IO.Paths(_FurcadiaProgramFolder)
                End If
                _FurcadiaCharactersFolder = _Paths.CharacterPath
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
    ''' Gets or sets the Furcadia documents folder.
    ''' </summary>
    ''' <value>
    ''' The Furcadia documents folder.
    ''' </value>
    ''' <exception cref="System.IO.DirectoryNotFoundException">
    ''' This needs to be a valid folder
    ''' </exception>
    Public Shared Property FurcadiaDocumentsFolder() As String
        Get
            If Not String.IsNullOrEmpty(_FurcadiaDocumentsFolder) Then
                Return _FurcadiaDocumentsFolder
            End If
            If _Paths Is Nothing Then
                _Paths = New Furcadia.IO.Paths(_FurcadiaProgramFolder)
            End If
            _FurcadiaDocumentsFolder = _Paths.PersonalDataPath
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
    ''' Gets or sets the Furcadia program folder.
    ''' </summary>
    ''' <value>
    ''' The Furcadia program folder.
    ''' </value>
    ''' <exception cref="System.IO.FileNotFoundException">
    ''' Furcadia.exe not found in path. Is this a Furcadia folder?
    ''' </exception>
    Public Shared Property FurcadiaProgramFolder() As String
        Get
            If String.IsNullOrEmpty(_FurcadiaProgramFolder) Then
                Try
                    _FurcadiaProgramFolder = _Paths.FurcadiaPath
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
    ''' Gets or sets the Monkey Speak Editor documents MonkeySpeak scripts path.
    ''' <para>
    ''' Default: Documents\Silver Monkey\Scripts
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor documents scripts path.
    ''' </value>
    Public Shared Property MonkeySpeakEditorDocumentsDsScriptsPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDocumentsScriptsPath) Then
                _MonKeySpeakEditorDocumentsDsScriptsPath = Path.Combine(SilverMonkeyDocumentsPath, DsScriptsDocsPath)
            End If
            Return _MonKeySpeakEditorDocumentsDsScriptsPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorDocumentsDsScriptsPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Monkey Speak Editor Dragon Speak documents
    ''' templates path.
    ''' <para>
    ''' Default: Documents\Silver Monkey\Templates
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor documents templates path.
    ''' </value>
    Public Shared Property MonkeySpeakEditorDocumentsDsTemplatesPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDocumentsDsTemplatesPath) Then
                _MonKeySpeakEditorDocumentsDsTemplatesPath = Path.Combine(SilverMonkeyDocumentsPath, DsTemplateDocsPath)
            End If
            Return _MonKeySpeakEditorDocumentsDsTemplatesPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorDocumentsDsTemplatesPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Monkey Speak Editor documents MonkeySpeak scripts path.
    ''' <para>
    ''' Default: Documents\Silver Monkey\Scripts-MS
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor documents scripts path.
    ''' </value>
    Public Shared Property MonkeySpeakEditorDocumentsScriptsPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDocumentsScriptsPath) Then
                _MonKeySpeakEditorDocumentsScriptsPath = Path.Combine(SilverMonkeyDocumentsPath, MsScriptseDocsPath)
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
    ''' Gets or sets the Monkey Speak documents templates path.
    ''' <para>
    ''' Default: Documents\Silver Monkey\Templates-MS
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor documents templates path.
    ''' </value>
    Public Shared Property MonkeySpeakEditorDocumentsTemplatesPath() As String
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
    ''' Gets or sets the Monkey Speak Editor Dragon Speak scripts path.
    ''' <para>
    ''' Default: Program Files(x86)\Silver Monkey\Scripts
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor scripts path.
    ''' </value>
    Public Shared Property MonkeySpeakEditorDsScriptsPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDsScriptsPath) Then
                _MonKeySpeakEditorDsScriptsPath = Path.Combine(ApplicationPath, DsScriptsDocsPath)
            End If
            Return _MonKeySpeakEditorDsScriptsPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorDsScriptsPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Monkey Speak templates path.
    ''' <para>
    ''' Default: Program Files(x86)\Silver Monkey\Templates
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor templates path.
    ''' </value>
    Public Shared Property MonKeySpeakEditorDsTemplatesPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorDsTemplatesPath) Then
                _MonKeySpeakEditorDsTemplatesPath = Path.Combine(ApplicationPath, DsTemplateDocsPath)
            End If
            Return _MonKeySpeakEditorDsTemplatesPath
        End Get
        Set(ByVal value As String)
            _MonKeySpeakEditorDsTemplatesPath = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Monkey Speak Editor MonkeySpeak scripts path.
    ''' <para>
    ''' Default: Program Files(x86)\Silver Monkey\Scripts-MS
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor scripts path.
    ''' </value>
    Public Shared Property MonKeySpeakEditorScriptsPath() As String
        Get
            If String.IsNullOrEmpty(_MonKeySpeakEditorScriptsPath) Then
                _MonKeySpeakEditorScriptsPath = Path.Combine(ApplicationPath, MsScriptseDocsPath)
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
    ''' Gets or sets the Monkey Speak templates path.
    ''' <para>
    ''' Default: Program Files(x86)\Silver Monkey\Templates-MS
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Monkey Speak Editor templates path.
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
    ''' Gets or sets the Silver Monkey bot path.
    ''' <para>
    ''' Set this path at Engine Start. Use path of Bot.bini if outside
    ''' "Documents\Silver monkey"
    ''' </para>
    ''' <para>
    ''' Default Path: "Documents\Silver Monkey"
    ''' </para>
    ''' </summary>
    ''' <value>
    ''' The Silver Monkey bot path.
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
    ''' Gets or sets the Silver Monkey documents path.
    ''' </summary>
    ''' <value>
    ''' The Silver Monkey documents path.
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
    ''' Gets or sets the Silver Monkey error log path.
    ''' </summary>
    ''' <value>
    ''' The Silver Monkey error log path.
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

    ''' <summary>
    ''' Default path for bot logs
    ''' </summary>
    ''' <returns>
    ''' </returns>
    Public Shared Property SilverMonkeyLogPath() As String
        Get
            If _SilverMonkeyLogPath Is Nothing Then
                _SilverMonkeyDocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), MyDocumentsPath)
                _SilverMonkeyLogPath = Path.Combine(_SilverMonkeyDocumentsPath, LogPath)
                If Not Directory.Exists(_SilverMonkeyLogPath) Then
                    Directory.CreateDirectory(_SilverMonkeyLogPath)
                End If
            End If
            Return _SilverMonkeyLogPath
        End Get
        Set(value As String)
            _SilverMonkeyLogPath = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Special Functoin for getting the default paths
    ''' <para>
    ''' Ideal logic is to check the location of the Bot.Bini file for the
    ''' current path and then check the Silver Monkey folder in my documents
    ''' </para>
    ''' </summary>
    ''' <param name="FileToCheck">
    ''' file path to check
    ''' </param>
    ''' <returns>
    ''' the correct file path
    ''' </returns>

    Public Shared Function CheckBotFolder(ByVal FileToCheck As String) As String
        Dim FilePath As String = Path.GetDirectoryName(FileToCheck)
        If String.IsNullOrEmpty(FilePath) Then
            If String.IsNullOrEmpty(FileToCheck) Then
                Return SilverMonkeyBotPath
            End If
            Return Path.Combine(SilverMonkeyBotPath, FileToCheck)
        End If
        If File.Exists(FileToCheck) Then
            Return FileToCheck
        End If
        Return SilverMonkeyBotPath
    End Function

#End Region

End Class