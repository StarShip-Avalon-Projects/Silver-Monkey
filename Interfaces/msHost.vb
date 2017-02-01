Public Interface msHost

    WriteOnly Property Page As Monkeyspeak.Page
    Property Dream As Furcadia.Net.DREAM
    Property Player As Furcadia.Net.FURRE
    ReadOnly Property BotName As String
    Property Data As String
    Sub start(ByRef page As Monkeyspeak.Page)

    Sub sendServer(ByRef var As String)

    Sub logError(ByRef Ex As Exception, ByRef ObjectThrowingError As Object)

    Sub SendClientMessage(msg As String, data As String)
End Interface
