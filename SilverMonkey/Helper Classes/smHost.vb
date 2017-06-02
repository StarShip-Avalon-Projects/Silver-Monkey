Imports Furcadia.Net
Imports MonkeyCore
Imports SilverMonkeyEngine
Imports SilverMonkeyEngine.Engine

Public Class smHost
    Implements Interfaces.ImsHost

#Region "Private Fields"

    Private FurcadiaSession As BotSession

#End Region

#Region "Public Properties"

    Public ReadOnly Property BotName As String Implements Interfaces.ImsHost.BotName
        Get
            Return FurcadiaSession.ConnectedCharacterName
        End Get
    End Property

    Public Property data As String Implements Interfaces.ImsHost.Data
        Get
            Return Nothing
            'Return FurcSession.
        End Get
        Set(value As String)
            ' FurcSession.serverData = value
        End Set
    End Property

    Public Property Dream As DREAM Implements Interfaces.ImsHost.Dream
        Get
            Return FurcadiaSession.Dream
        End Get
        Set(value As DREAM)
            FurcadiaSession.Dream = value
        End Set

    End Property

    Public WriteOnly Property MsPage() As MonkeySpeakPage Implements Interfaces.ImsHost.MsPage
        Set(value As MonkeySpeakPage)
            FurcadiaSession.MSpage = value
        End Set
    End Property

    Public Property Player() As FURRE Implements Interfaces.ImsHost.Player
        Get
            Return FurcadiaSession.Player
        End Get
        Set(value As FURRE)
            FurcadiaSession.Player = value
        End Set

    End Property

#End Region

#Region "Public Methods"

    Public Sub New(Session As BotSession)
        FurcadiaSession = Session
    End Sub

    Sub logError(ByRef Ex As System.Exception, ByRef ObjectThrowingError As Object) Implements Interfaces.ImsHost.logError
        Dim Err As New ErrorLogging(Ex, ObjectThrowingError)
    End Sub

    Public Sub SendClientMessage(Tag As String, data As String) Implements Interfaces.ImsHost.SendClientMessage
        SendClientMessage(Tag, data)
    End Sub

    Sub sendServer(ByRef var As String) Implements Interfaces.ImsHost.sendServer

    End Sub

    Sub Start(ByRef page As MonkeySpeakPage) Implements Interfaces.ImsHost.start
        FurcadiaSession.MSpage = page
    End Sub

#End Region

End Class