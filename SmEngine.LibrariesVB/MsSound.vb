﻿Imports System.Media
Imports MonkeyCore2.IO
Imports MonkeyCore
Imports Monkeyspeak
Imports Engine.Libraries
Imports Libraries

Namespace Libraries

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
    Public NotInheritable Class MsSound
        Inherits MonkeySpeakLibrary
        Implements IDisposable

#Region "Public Constructors"

        Public Overrides ReadOnly Property BaseId As Integer
            Get
                Return 2010
            End Get
        End Property

        Private simpleSound As SoundPlayer

        Public Overrides Sub Initialize(ParamArray args() As Object)
            MyBase.Initialize(args)
            Add(TriggerCategory.Effect,
            Function(reader As TriggerReader) As Boolean
                Dim SoundFile = Paths.CheckBotFolder(reader.ReadString(True))
                Using PlaySound = New SoundPlayer(SoundFile)
                    PlaySound.Play()

                End Using
                Return True
            End Function, "play the wave file {...}.")

            Add(TriggerCategory.Effect,
            Function(reader As TriggerReader) As Boolean
                If Not simpleSound Is Nothing Then
                    Dim SoundFile = Paths.CheckBotFolder(reader.ReadString(True))
                    simpleSound = New SoundPlayer(SoundFile)
                    simpleSound.PlayLooping()
                End If
                Return simpleSound Is Nothing
            End Function, "play the wave file {...} in a loop. if theres not one playing")

            Add(TriggerCategory.Effect,
             AddressOf StopSound, "stop playing the sound file.")
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
        Public Function StopSound(reader As TriggerReader) As Boolean
            If Not simpleSound Is Nothing Then
                Dim SoundFile = MonkeyCore2.IO.Paths.CheckBotFolder(reader.ReadString(True))
                ' simpleSound = New SoundPlayer(SoundFile)
                simpleSound.[Stop]()
                simpleSound.Dispose()
            End If
            Return True

        End Function

#Region "IDisposable Support"

        Private disposedValue As Boolean ' To detect redundant calls

        ''' <summary>
        ''' This code added by Visual Basic to correctly implement the disposable pattern.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
        End Sub

        ''' <summary>
        ''' Override Dispose method
        ''' </summary>
        ''' <param name="page"></param>
        Public Overrides Sub Unload(page As Page)
            Dispose(True)
        End Sub

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then

                    If simpleSound IsNot Nothing Then
                        simpleSound.Dispose()
                        simpleSound = Nothing
                    End If

                End If
            End If
            disposedValue = True
        End Sub

#End Region

#End Region

    End Class

End Namespace