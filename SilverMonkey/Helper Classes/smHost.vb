Imports Furcadia.Net
Imports MonkeyCore

Public Class smHost
    Implements SilverMonkey.Interfaces.msHost
    Sub Start(ByRef page As Monkeyspeak.Page) Implements SilverMonkey.Interfaces.msHost.start
        MainMSEngine.MSpage = page
    End Sub

    Public WriteOnly Property Page() As Monkeyspeak.Page Implements SilverMonkey.Interfaces.msHost.Page
        Set(value As Monkeyspeak.Page)
            MainMSEngine.MSpage = value
        End Set
    End Property

    Public ReadOnly Property BotName As String Implements SilverMonkey.Interfaces.msHost.BotName
        Get
            Return callbk.BotName
        End Get
    End Property

    Public Property data As String Implements SilverMonkey.Interfaces.msHost.Data
        Get
            Return callbk.serverData
        End Get
        Set(value As String)
            callbk.serverData = value
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
