Imports Furcadia.Net
Imports Furcadia.Net.Utils.ServerParser
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Monkey Speak for Furcadia's in-game trade system
    ''' <para>
    ''' DEP recommends this system for trading digos and things
    ''' </para>
    ''' </summary>
    Public Class MsTrades
        Inherits MonkeySpeakLibrary

        ''' <summary>
        ''' default constructor to link with server events
        ''' </summary>
        ''' <param name="session"></param>
        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)

            '(0:46) When the bot sees a trade request,
            Add(TriggerCategory.Cause, 46,
            Function()
                Return Not FurcadiaSession.IsConnectedCharacter
            End Function, "(0:46) When the bot sees a trade request,")
            '(0:47) When the bot sees the trade request {..},
            Add(TriggerCategory.Cause, 47,
            AddressOf msgIs, "(0:47) When the bot sees the trade request {..},")

            '(0:48) When the bot sees a trade request with {..} in it,
            Add(TriggerCategory.Cause, 48,
            AddressOf msgContains, "(0:48) When the bot sees a trade request with {..} in it,")
        End Sub

        ''' <summary>
        ''' (0:48) When the bot sees a trade request with {..} in it,
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Protected Overrides Function msgContains(reader As TriggerReader) As Boolean
            Return MyBase.msgContains(reader)
        End Function

        ''' <summary>
        ''' (0:47) When the bot sees the trade request {..},
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Protected Overrides Function msgIs(reader As TriggerReader) As Boolean
            Return MyBase.msgIs(reader)
        End Function

        ''' <summary>
        ''' Trade trigger handler
        ''' </summary>
        ''' <param name="InstructionObject">Server Instruction</param>
        ''' <param name="Args">Server Event Arguments</param>
        Private Sub OnServerChannel(InstructionObject As ChannelObject, Args As ParseServerArgs) Handles FurcadiaSession.ProcessServerInstruction
            Player = InstructionObject.Player
            Dim Text = InstructionObject.ChannelText

            Select Case Text
                Case "trade"
                    MsPage.Execute(46, 47, 48)
            End Select
        End Sub

    End Class

End Namespace