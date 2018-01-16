Imports Monkeyspeak

''' <summary>
''' Monkey Speak for Furcadia's in-game trade system
''' <para>
''' DEP recommends this system for trading digos and things
''' </para>
''' </summary>
Public NotInheritable Class MsTrades
    Inherits MonkeySpeakLibrary

#Region "Public Methods"

    Public Overrides ReadOnly Property BaseId As Integer
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides Sub Initialize(ParamArray args() As Object)
        MyBase.Initialize(args)

        '    ''(0:46) When the bot sees a trade request,
        '    'Add(TriggerCategory.Cause, 46,
        '    'Function(reader)
        '    '    ReadTriggeringFurreParams(reader)
        '    '    Return Not FurcadiaSession.IsConnectedCharacter
        '    'End Function, "When the bot sees a trade request,")
        '    '(0:47) When the bot sees the trade request {..},
        '    Add(TriggerCategory.Cause, 47,
        '        AddressOf MsgIs, "When the bot sees the trade request {..},")

        '    '(0:48) When the bot sees a trade request with {..} in it,
        '    Add(TriggerCategory.Cause, 48,
        '    AddressOf MsgContains, "When the bot sees a trade request with {..} in it,")
    End Sub

    Public Overrides Sub Unload(page As Page)

    End Sub

#End Region

End Class