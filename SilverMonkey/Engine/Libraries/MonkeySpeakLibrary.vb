Imports MonkeyCore
Imports Furcadia.Util
Imports Furcadia.Net
Imports Monkeyspeak

Namespace Engine.Libraries
    Public Class MonkeySpeakLibrary
        Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

#Region "Private Fields"

        Private FuscariaSession As BotSession

#End Region

#Region "Public Fields"

        ''' <summary>
        ''' Current Dream the Bot is in
        ''' </summary>
        Public Dream As DREAM

        Public MsEngine As MonkeyspeakEngine

        Public MsPage As Page

        ''' <summary>
        ''' Current Triggering Furre
        ''' </summary>
        Public Player As FURRE
#End Region

#Region "Public Delegates"

        Public Shared FurcadiaSession As BotSession

        Public Delegate Sub MessageDelegate(ByRef message As String)

        Public Sub LogError(reader As TriggerReader, ex As Exception)
            '  Main.FurcadiaSession..LogError(reader, ex)
        End Sub

        ''' <summary>
        '''Sends Client Message to out Furcadia Session
        ''' </summary>
        ''' <param name="message">Message sring</param>
        Public Sub SendClientMessage(ByRef message As String)
            Main.FurcadiaSession.SendToClient(message)
        End Sub
        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="message">Message String</param>
        Public Sub sendServer(ByRef message As String)

            Main.FurcadiaSession.SendToServer(message)
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
        Sub New(ByRef Session As BotSession)
            MyBase.New()
            FurcadiaSession = Session
            writer = New TextBoxWriter(Variables.TextBox1)
        End Sub
        Sub New()
            MyBase.New()
            FurcadiaSession = Main.FurcadiaSession
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

#Region "Common Library Methods"

        ''' <summary>
        ''' Reads a Double or a MonkeySpeak Variable
        ''' </summary>
        ''' <param name="reader"><see cref="TriggerReader"/></param>
        ''' <param name="addIfNotExist">Add Variable to Variable Scope is it Does not exist</param>
        ''' <returns><see cref="Double"/></returns>
        Public Function ReadVariableOrNumber(ByVal reader As TriggerReader, Optional addIfNotExist As Boolean = False) As Double
            Dim result As Double = 0
            If reader.PeekVariable Then
                Dim value As String = reader.ReadVariable(addIfNotExist).Value.ToString
                Double.TryParse(value, result)
            ElseIf reader.PeekNumber Then
                result = reader.ReadNumber
            End If
            Return result
        End Function
#End Region

    End Class
End Namespace