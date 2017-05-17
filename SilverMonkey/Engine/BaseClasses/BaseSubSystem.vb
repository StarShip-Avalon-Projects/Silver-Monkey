Imports Furcadia.Net

Public Class BaseSubSystem
    Inherits Furcadia.IO.ParseServer

    Protected MyDream As Furcadia.Net.DREAM

    Protected MyEngine As MainMsEngine

    Public Sub New(ByRef Dream As DREAM, ByRef Player As FURRE, MsEngine As MainMsEngine)
        MyBase.New(Dream, Player)
        MyEngine = MsEngine

    End Sub

    Public Sub New(ByRef Dream As DREAM, ByRef Player As FURRE)
        MyBase.New(Dream, Player)
        MyEngine = New MainMsEngine()
    End Sub

    Public Overrides Sub ParseServerChannel(ByVal data As String, ByRef handled As Boolean)
        MyBase.ParseServerChannel(data, handled)
    End Sub

    Public Overrides Sub ParseServerData(data As String, ByRef handled As Boolean)
        MyBase.ParseServerData(data, handled)
    End Sub

End Class