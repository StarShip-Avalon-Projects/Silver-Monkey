Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Monkey Speak for Furcadia's in-game trade system
    ''' <para>
    ''' DEP recommends this system for trading digos and things
    ''' </para>
    ''' </summary>
    Public NotInheritable Class MsTrades
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
            End Function, " When the bot sees a trade request,")
            '(0:47) When the bot sees the trade request {..},
            Add(TriggerCategory.Cause, 47,
            AddressOf MsgIs, " When the bot sees the trade request {..},")

            '(0:48) When the bot sees a trade request with {..} in it,
            Add(TriggerCategory.Cause, 48,
            AddressOf MsgContains, " When the bot sees a trade request with {..} in it,")
        End Sub

        Public Overrides Sub Unload(page As Page)

        End Sub

        ''' <summary>
        ''' (0:48) When the bot sees a trade request with {..} in it,
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Protected Overrides Function MsgContains(reader As TriggerReader) As Boolean
            Return MyBase.MsgContains(reader)
        End Function

        ''' <summary>
        ''' (0:47) When the bot sees the trade request {..},
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Protected Overrides Function MsgIs(reader As TriggerReader) As Boolean
            Return MyBase.MsgIs(reader)
        End Function



    End Class

End Namespace