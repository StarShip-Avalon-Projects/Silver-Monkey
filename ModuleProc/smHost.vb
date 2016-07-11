Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Monkeyspeak
Imports Furcadia.Net

Public Class smHost
    Implements SilverMonkey.Interfaces.msHost
    Sub Start(ByRef page As Monkeyspeak.Page) Implements SilverMonkey.Interfaces.msHost.start
        MainEngine.MSpage = page
    End Sub

    Public ReadOnly Property BotName As String Implements SilverMonkey.Interfaces.msHost.BotName
        Get
            Return ""
        End Get
    End Property

    Dim serverData As String
    Public Property data As String Implements SilverMonkey.Interfaces.msHost.Data
        Get
            Return serverData
        End Get
        Set(value As String)
            serverData = value
        End Set
    End Property

    Public WriteOnly Property Page() As Monkeyspeak.Page Implements SilverMonkey.Interfaces.msHost.Page
        Set(value As Monkeyspeak.Page)
            MainEngine.MSpage = value
        End Set
    End Property

    Public Property Dream() As DREAM Implements SilverMonkey.Interfaces.msHost.Dream
        Get
            Return callbk.DREAM
        End Get
        Set(value As DREAM)
            callbk.DREAM = value
        End Set
    End Property

    Public Property Player() As FURRE Implements SilverMonkey.Interfaces.msHost.Player
        Get
            Return callbk.Player
        End Get
        Set(value As FURRE)
            callbk.Player = value
        End Set
    End Property


    Public Sub SendClientMessage(Tag As String, data As String) Implements SilverMonkey.Interfaces.msHost.SendClientMessage
        callbk.SendClientMessage(Tag, data)
    End Sub

    Sub sendServer(ByRef var As String) Implements SilverMonkey.Interfaces.msHost.sendServer
        callbk.TextToServer(var)
    End Sub

    Sub logError(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object) Implements SilverMonkey.Interfaces.msHost.logError
        Dim Err As New ErrorLogging(Ex, ObjectThrowingError)
    End Sub


End Class
