Imports MonkeyCore

Public Class MonkeySpeakLibrary
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

#Region "Public Fields"

    Public MyMonkeySpeakEngine As MainMsEngine

#End Region

#Region "Protected Fields"

    Protected Shared writer As TextBoxWriter = Nothing

    ''' <summary>
    ''' Current Executing Furcadia Dream
    ''' </summary>
    Protected MyDream As Furcadia.Net.DREAM

    ''' <summary>
    ''' Current Executing Player
    ''' </summary>
    Protected MyPlayer As Furcadia.Net.FURRE

#End Region

#Region "Public Constructors"

    Sub New()
        MyBase.New()
        writer = New TextBoxWriter(Variables.TextBox1)
    End Sub

    Sub New(ByRef Dream As Furcadia.Net.DREAM, ByRef Player As Furcadia.Net.FURRE, ByRef MsEngine As MainMsEngine)
        Me.New()
        MyDream = Dream
        MyPlayer = Player
        MyMonkeySpeakEngine = MsEngine
    End Sub

#End Region

#Region "Public Methods"

    Public Shared Sub sendServer(ByRef var As String)

    End Sub

    Public Overloads Sub ClientMessage(ByRef message As String)

    End Sub

    Public Overloads Sub SendClientMessage(ByRef System As String, ByRef message As String)
        SendClientMessage(String.Format("<b><i>[SM]{0}</i></b>:{1}", System, message))
    End Sub

    Public Overloads Sub SendClientMessage(ByRef message As String)

    End Sub

#End Region

End Class