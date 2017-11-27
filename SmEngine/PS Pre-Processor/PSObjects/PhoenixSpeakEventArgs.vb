Namespace Engine.Libraries.PhoenixSpeak

    ''' <summary>
    ''' Phoenix mSpeak Even Arguments
    ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/psalpha">Phoenix Speak</see>
    ''' </summary>
    <CLSCompliant(True)>
    Public Class PhoenixSpeakEventArgs
        Inherits Furcadia.Net.NetChannelEventArgs

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
            Channel = "PhoenixSpeak"
        End Sub

#End Region

#Region "Public Fields"

        ''' <summary>
        ''' Do we have too much Phoienix-Speak Data then the Server can send
        ''' to us?
        ''' </summary>
        Public PageOverFlow As Boolean

#End Region

        ''' <summary>
        ''' PhoenixSpeak id for cerver/client instructions
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property id As Short

    End Class

End Namespace