Imports System.Media
Imports MonkeyCore
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Effects: (5:2010) - (5:2012)
    ''' <para>
    ''' Simple way to play wave files
    ''' </para>
    ''' <para>
    ''' Default wave folder: <see cref="Paths.SilverMonkeyBotPath"/>
    ''' </para>
    ''' </summary>
    ''' <remarks>
    ''' This lib contains the following unnamed delegates
    ''' <para>
    ''' (5:2010) play the wave file {...}.
    ''' </para>
    ''' <para>
    ''' (5:2011) play the wave file {...} in a loop. if theres not one playing
    ''' </para>
    ''' </remarks>
    Public Class MsSound
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Private simpleSound As SoundPlayer

        Public Sub New(Session As BotSession)
            MyBase.New(Session)
            Add(New Trigger(TriggerCategory.Effect, 2010),
                Function(reader As TriggerReader) As Boolean
                    Dim SoundFile As String = Paths.CheckBotFolder(reader.ReadString(True))
                    Using simpleSound = New SoundPlayer(SoundFile)
                        simpleSound.Play()

                    End Using
                    Return True
                End Function, "(5:2010) play the wave file {...}.")

            Add(New Trigger(TriggerCategory.Effect, 2011),
                Function(reader As TriggerReader) As Boolean
                    If Not simpleSound Is Nothing Then
                        Dim SoundFile As String = Paths.CheckBotFolder(reader.ReadString(True))
                        simpleSound = New SoundPlayer(SoundFile)
                        simpleSound.PlayLooping()
                    End If
                    Return simpleSound Is Nothing

                End Function, "(5:2011) play the wave file {...} in a loop. if theres not one playing")
            Add(New Trigger(TriggerCategory.Effect, 2012),
                 AddressOf StopSound, "(5:2012) stop playing the sound file.")
        End Sub

        ''' <summary>
        ''' (5:2012) stop playing the sound file.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Function StopSound(reader As TriggerReader) As Boolean
            If Not simpleSound Is Nothing Then
                Dim SoundFile As String = Paths.CheckBotFolder(reader.ReadString(True))
                simpleSound = New SoundPlayer(SoundFile)
                simpleSound.[Stop]()
                simpleSound.Dispose()
            End If
            Return True

        End Function

#End Region

    End Class

End Namespace