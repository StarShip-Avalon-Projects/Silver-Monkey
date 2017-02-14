Imports MonkeyCore

Public Class MonkeySpeakLibrary
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary

    Protected Shared writer As TextBoxWriter = Nothing

    ''' <summary>
    ''' Current Executing Furcadia Dream
    ''' </summary>
    Protected MyDream As Furcadia.Net.DREAM

    ''' <summary>
    ''' Current Executing Player
    ''' </summary>
    Protected MyPlayer As Furcadia.Net.FURRE

    Public MyMonkeySpeakEngine As MainMsEngine

    Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)
    End Sub

    Sub New(ByRef Dream As Furcadia.Net.DREAM, ByRef Player As Furcadia.Net.FURRE, ByRef MsEngine As MainMsEngine)
        writer = New TextBoxWriter(Variables.TextBox1)
        MyDream = Dream
        MyPlayer = Player
        MyMonkeySpeakEngine = MsEngine
    End Sub

    Public Overloads Sub ClientMessage(ByRef message As String)

    End Sub

    Public Overloads Sub SendClientMessage(ByRef System As String, ByRef message As String)
        SendClientMessage(String.Format("<b><i>[SM]{0}</i></b>:{1}", System, message))
    End Sub

    Public Overloads Sub SendClientMessage(ByRef message As String)

    End Sub

    Public Shared Sub sendServer(ByRef var As String)

    End Sub

End Class