﻿Imports Furcadia.Net.Dream
Imports SilverMonkeyEngine.Engine

Namespace Interfaces

    Public Interface ImsHost

#Region "Public Properties"

        ReadOnly Property BotName As String
        Property Data As String
        Property Dream As DREAM
        WriteOnly Property MsPage As MonkeySpeakPage
        Property Player As FURRE

#End Region

#Region "Public Methods"

        Sub logError(ByRef Ex As Exception, ByRef ObjectThrowingError As Object)

        Sub SendClientMessage(msg As String, data As String)

        Sub sendServer(ByRef var As String)

        Sub start(ByRef page As MonkeySpeakPage)

#End Region

    End Interface

End Namespace