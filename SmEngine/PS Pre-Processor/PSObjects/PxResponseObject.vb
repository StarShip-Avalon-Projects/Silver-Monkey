Imports SilverMonkeyEngine.Engine.Libraries.PhoenixSpeak

Public Class PsResponseObject : Inherits Furcadia.Net.ParseChannelArgs

#Region "Private Fields"

    Private _PsObject As PhoenicSpeakDataObject

#End Region

#Region "Public Constructors"

    Sub New()
        Channel = "PhoenxSpeak"
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' PheonixSpeak object to send to <see cref="MsPhoenixSpeak"/>
    ''' </summary>
    Public Property PsObject() As PhoenicSpeakDataObject
        Get
            Return _PsObject
        End Get
        Set(ByVal value As PhoenicSpeakDataObject)
            _PsObject = value
        End Set
    End Property

#End Region

End Class