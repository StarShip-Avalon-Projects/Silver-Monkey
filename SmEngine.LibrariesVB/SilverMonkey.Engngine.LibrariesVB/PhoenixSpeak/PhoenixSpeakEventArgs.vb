Namespace Engine.Libraries.PhoenixSpeak

#Region "Phoenix Speak Enums"

    <Flags>
    Public Enum PsBackupStage
        [error] = 0
        off = 1

        GetDream

        ''' <summary>
        ''' Read Multi Page responses for character list
        ''' </summary>
        GetList

        ''' <summary>
        ''' Read Character list one letter at a time
        ''' <para>
        ''' Picks up where Get List left Off
        ''' </para>
        ''' </summary>
        GetAlphaNumericList

        ''' <summary>
        ''' </summary>
        GetTargets

        ''' <summary>
        ''' </summary>
        GetSingle

        ''' <summary>
        ''' </summary>
        RestoreSibgleCharacterPs

        ''' <summary>
        ''' </summary>
        RestoreAllCharacterPS

        ''' <summary>
        ''' Pruning Database
        ''' </summary>
        PruneDatabase

    End Enum

    ''' <summary>
    ''' PS systems running
    ''' </summary>
    <CLSCompliant(False)>
    <Flags>
    Public Enum PsSystemRunning
        [Error] = 0
        PsNone
        PsBackup
        PsRestore
        PsPrune
    End Enum

#End Region

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