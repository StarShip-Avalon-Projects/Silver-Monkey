﻿Imports Furcadia.Net
Imports MonkeyCore

Public Class smHost
    Implements Interfaces.msHost
    Sub Start(ByRef page As Monkeyspeak.Page) Implements SilverMonkey.Interfaces.msHost.start
        FurcSession.MainEngine.MSpage = page
    End Sub

    Public WriteOnly Property Page() As Monkeyspeak.Page Implements SilverMonkey.Interfaces.msHost.Page
        Set(value As Monkeyspeak.Page)
            FurcSession.MainEngine.MSpage = value
        End Set
    End Property

    Public ReadOnly Property BotName As String Implements SilverMonkey.Interfaces.msHost.BotName
        Get
            Return FurcSession.BotName
        End Get
    End Property

    Public Property data As String Implements Interfaces.msHost.Data
        Get
            Return Nothing
            ' Return FurcSession.
        End Get
        Set(value As String)
            ' FurcSession.serverData = value
        End Set
    End Property

    Public Property Dream As DREAM Implements Interfaces.msHost.Dream
        Get
            Return FurcSession.Dream
        End Get
        Set(value As DREAM)
            FurcSession.Dream = value
        End Set
    End Property

    Public Property Player() As FURRE Implements SilverMonkey.Interfaces.msHost.Player
        Get
            Return FurcSession.Player
        End Get
        Set(value As FURRE)
            FurcSession.Player = value
        End Set
    End Property

    Public Sub SendClientMessage(Tag As String, data As String) Implements SilverMonkey.Interfaces.msHost.SendClientMessage
        SendClientMessage(Tag, data)
    End Sub

    Sub sendServer(ByRef var As String) Implements SilverMonkey.Interfaces.msHost.sendServer

    End Sub

    Sub logError(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object) Implements SilverMonkey.Interfaces.msHost.logError
        Dim Err As New ErrorLogging(Ex, ObjectThrowingError)
    End Sub

End Class