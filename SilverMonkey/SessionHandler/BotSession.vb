Imports MonkeyCore
Imports Furcadia.Net
''' <summary>
''' Bot Specific Stuff. Monkey Speak Engine interface, GUI Interface
''' </summary>
Public Class BotSession : Inherits FurcSession

#Region "Public Methods"

    ''' <summary>
    ''' Connect to Furcadia and Launch our engine objects
    ''' <list type="bullet">
    ''' <listheader></listheader>
    ''' <item>Reconnection Manager</item>
    ''' <item>Launch MonkeySpeak Engine</item>
    ''' </list>
    ''' </summary>
    Public Overloads Sub Connect()

        'TODO: Reconnection.Manager.Start
        MainEngine.EngineStart(True)
        MyBase.Connect()
    End Sub

    ''' <summary>
    ''' Handle NetProxy Errors
    ''' </summary>
    ''' <param name="eX"></param>
    ''' <param name="o"></param>
    ''' <param name="n"></param>
    Public Sub onProxyError(eX As Exception, o As Object, n As String) Handles MyBase.Error

        'TODO: add custom event args

        'Dim args As New Furcadia.Net.Utils..ConnectionEventArgs
        'args.Status = ConnectionStats.error
        'RaiseEvent OnError(o, args)
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''
    ''' </summary>
    Private Sub OnServerDisconnect() Handles MyBase.ServerDisConnected

        ProcExit = False

        Disconnect()
    End Sub

#End Region

#Region "Public Constructors"

    Sub New()

    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="BotSettings"></param>
    ''' <param name="MainSrettions"></param>
    Sub New(BotSettings As Settings.cBot, MainSrettions As Settings.cMain)

    End Sub

#End Region

#Region "Public Delegates"

#End Region

#Region "Public Methods"

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="data">Raw Server Data</param>
    ''' <param name="e"> Event Arguments with Server Instruction</param>
    Public Sub onProcessChannelData(ByVal data As String, ByVal e As EventArgs) Handles MyBase.ProcessServerChannelData

    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="data">Raw Server Data</param>
    ''' <param name="e"> Event Arguments with client Instruction</param>
    Public Sub OnProcessClientData(ByVal data As String, ByVal e As EventArgs) Handles MyBase.ProcessClientData

    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="data">Raw Server Data</param>
    ''' <param name="e"> Event Arguments with Server Instruction</param>
    Public Sub onProcessServerData(ByVal data As String, ByVal e As EventArgs) Handles MyBase.ProcessServerData

    End Sub
#End Region

#Region "Public Delegates"

    ''' <summary>
    ''' Data Handler to display or log
    ''' </summary>
    ''' <param name="Message"> Text to Display</param>
    ''' <param name="e">Event Arguments</param>
    Public Delegate Sub DisplayText(ByVal Message As String, ByVal e As EventArgs)

    Public Event DisplayErrorText As DisplayText

    Public Event DisplayNormalText As DisplayText
#End Region
End Class