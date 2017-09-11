Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports MonkeyCore
Imports SilverMonkeyEngine
Imports SilverMonkeyEngine.Engine

Public Class smHost
    Implements Interfaces.ImsHost

#Region "Private Fields"

    Dim serverData As String

#End Region

#Region "Public Properties"

    Public ReadOnly Property BotName As String Implements Interfaces.ImsHost.BotName
        Get
            Return ""
        End Get
    End Property

    Public Property data As String Implements Interfaces.ImsHost.Data
        Get
            Return serverData
        End Get
        Set(value As String)
            serverData = value
        End Set
    End Property

    Public Property Dream() As DREAM Implements Interfaces.ImsHost.Dream
        Get
            Return callbk.DREAM
        End Get
        Set(value As DREAM)
            callbk.DREAM = value
        End Set
    End Property

    Public WriteOnly Property MsPage() As MonkeySpeakPage Implements Interfaces.ImsHost.MsPage
        Set(value As MonkeySpeakPage)
            MsPage = value
        End Set
    End Property

    Public Property Player() As FURRE Implements Interfaces.ImsHost.Player
        Get
            Return callbk.Player
        End Get
        Set(value As FURRE)
            callbk.Player = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Sub logError(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object) Implements Interfaces.ImsHost.logError
        Dim Err As New ErrorLogging(Ex, ObjectThrowingError)
    End Sub

    Public Sub SendClientMessage(Tag As String, data As String) Implements Interfaces.ImsHost.SendClientMessage
        callbk.SendClientMessage(Tag, data)
    End Sub

    Sub sendServer(ByRef var As String) Implements Interfaces.ImsHost.sendServer
        callbk.TextToServer(var)
    End Sub

    Sub Start(ByRef page As MonkeySpeakPage) Implements Interfaces.ImsHost.start
        MsPage = page
    End Sub

#End Region

End Class