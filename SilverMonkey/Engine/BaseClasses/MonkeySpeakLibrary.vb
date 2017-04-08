Imports MonkeyCore

Public Class MonkeySpeakLibrary
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

#Region "Public Fields"

    Public MyMonkeySpeakEngine As MainMsEngine

#End Region

#Region "Public Delegates"

    Public Shared FurcadiaSession As FurcSession = Main.FurcadiaSession

    Public Delegate Sub MessageDelegate(ByRef message As String)

    Public Sub LogError(reader As Monkeyspeak.TriggerReader, ex As Exception)
        Main.FurcadiaSession.MainEngine.LogError(reader, ex)
    End Sub

    ''' <summary>
    '''Sends Client Message to out Furcadia Session
    ''' </summary>
    ''' <param name="message">Message sring</param>
    Public Sub SendClientMessage(ByRef message As String)
        Main.FurcadiaSession.SendClient(message)
    End Sub
    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="message">Message String</param>
    Public Sub sendServer(ByRef message As String)

        Main.FurcadiaSession.SendServer(message)
    End Sub

#End Region

#Region "Protected Fields"

    Protected Shared writer As TextBoxWriter = Nothing

#End Region

#Region "Public Constructors"

    ''' <summary>
    ''' Default Constructor
    ''' <para> Loads default Text Writer</para>
    ''' </summary>
    Sub New()
        MyBase.New()
        writer = New TextBoxWriter(Variables.TextBox1)
    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="TriggerIDs">MonkeySpeak Triggers</param>
    Protected Sub PageExecute(ParamArray TriggerIDs() As Integer)

        FurcadiaSession.MainEngine.PageExecute(TriggerIDs)
    End Sub

    ''' <summary>
    '''
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="ServerData"></param>
    Protected Sub PageSetVariable(Message As String, ServerData As String)
        FurcadiaSession.MainEngine.PageSetVariable(Message, ServerData)
    End Sub

#End Region

End Class