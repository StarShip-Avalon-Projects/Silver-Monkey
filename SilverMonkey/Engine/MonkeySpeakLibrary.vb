Imports MonkeyCore

Public Class MonkeySpeakLibrary
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

#Region "Public Fields"

    Public MyMonkeySpeakEngine As MainMsEngine

#End Region

#Region "Public Delegates"

    Public Delegate Sub TextSend(ByRef message As String)

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

    ''' <summary>
    ''' Current Executing Furcadia Dream
    ''' </summary>
    Protected MyDream As Furcadia.Net.DREAM

    ''' <summary>
    ''' Current Executing Player
    ''' </summary>
    Protected MyPlayer As Furcadia.Net.FURRE

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

    Sub New(ByRef Dream As Furcadia.Net.DREAM, ByRef Player As Furcadia.Net.FURRE, ByRef MsEngine As MainMsEngine)
        Me.New()
        MyDream = Dream
        MyPlayer = Player
        MyMonkeySpeakEngine = MsEngine
    End Sub

#End Region

End Class