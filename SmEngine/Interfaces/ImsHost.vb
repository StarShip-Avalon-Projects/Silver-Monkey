Imports Furcadia.Net.Dream

Namespace Interfaces

    Public Interface ImsHost

#Region "Public Properties"

        ReadOnly Property BotName As String
        Property Data As String
        Property Dream As DREAM
        WriteOnly Property MsPage As Monkeyspeak.Page
        ReadOnly Property Player As Furre

#End Region

#Region "Public Methods"

        Sub logError(ByRef Ex As Exception, ByRef ObjectThrowingError As Object)

        Sub SendClientMessage(msg As String, data As String)

        Sub sendServer(ByRef var As String)

        Sub start(ByRef page As Monkeyspeak.Page)

#End Region

    End Interface

End Namespace