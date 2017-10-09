Imports SilverMonkeyEngine.Engine.Libraries.PhoenixSpeak.SubSystem

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
        ''' Phoenix Speak Flag
        ''' </summary>
        '  Public Flag As PsFlag

        ''' <summary>
        ''' Do we have too much Phoienix-Speak Data then the Server can send
        ''' to us?
        ''' </summary>
        Public PageOverFlow As Boolean

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' PhoenixSpeak id for cerver/client instructions
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Property id As Short

        ''' <summary>
        ''' </summary>
        ''' <returns>
        ''' </returns>
        ' Public Property PsType As PsFlag

#End Region

    End Class

End Namespace