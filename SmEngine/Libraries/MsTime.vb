﻿Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Causes and effects to work with time and date material. Use this
    ''' Monkey Speak to launch a Phoenix Speak back up operation, to track
    ''' the date a furre was last seen in the dream,
    ''' <para>
    ''' Cause (0:299)
    ''' </para>
    ''' <para>
    ''' Effects: (5:30) - (5:35)
    ''' </para>
    ''' </summary>
    Public Class MsTime
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Dim lock As New Object

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)

            '(0:299) When the time is {...} hh:mm:ss am/pm FST,
            Add(TriggerCategory.Cause, 299,
                Function(reader As TriggerReader)
                    Dim Time As String = reader.ReadString
                    Dim str As String = ""
                    SyncLock lock
                        str = FurcTime.ToLongTimeString
                    End SyncLock
                    Return str.ToUpper = Time.ToUpper
                End Function,
            "(0:299) When the time is {...} hh:mm:ss am/pm FST,")

            '(5:30) set variable %Variable to the current local time.
            Add(TriggerCategory.Effect, 30,
                AddressOf CurrentTime, "(5:30) set variable %Variable to the current local time.")

            '(5:31) set variable %Variable to the current Furcadia Standard time
            Add(TriggerCategory.Effect, 31,
                AddressOf MsFurcTime, "(5:31) set variable %Variable to the current Furcadia Standard time.")
            '(5:32) set variable %Variable to current DateTime
            Add(TriggerCategory.Effect, 32,
                AddressOf LocalDateTimeVar, "(5:32) set variable %Variable to current local DateTime.")
            '(5:33) set variable %Variable to current Furcadia DateTime
            Add(TriggerCategory.Effect, 33,
                AddressOf FurcDateTimeVar, "(5:33) set variable %Variable to current Furcadia DateTime.")
            '(5:34) use variable %Variable as a DateTime string and subtract Date Time string {...} and put it into variable %Variable
            Add(TriggerCategory.Effect, 34,
                AddressOf SubsractDateTimeStr, "(5:34) use variable %Variable as a DateTime string and subtract Date Time string {...} and put it into variable %Variable.")
            '(5:35) use variable %Variable as a DateTime string and subtract Date Time variable %Variable and put it into variable %Variable
            Add(TriggerCategory.Effect, 35,
                AddressOf SubsractDateTimeVar, "(5:35) use variable %Variable as a DateTime string and subtract Date Time variable %Variable and put it into variable %Variable.")
            '(5:36) use variable %Variable as a DateTime string and add Date Time string {...} and put it into variable %Variable
            '        Add(TriggerCategory.Effect, 36,
            'AddressOf AddDateTimeStr, "(5:36) use variable %Variable as a DateTime string and add Date Time string {...} and put it into variable %Variable")
            '        '(5:37) use variable %Variable as a DateTime string and add Date Time variable %Variable and put it into variable %Variable
            '        Add(TriggerCategory.Effect, 37,
            'AddressOf AddDateTimeVar, "(5:37) use variable %Variable as a DateTime string and add Date Time variable %Variable and put it into variable %Variable")
            '
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:30) set variable %Variable to the current local time.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function CurrentTime(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Dim Str As String = DateTime.Now.ToLongTimeString.ToLower
            Var.Value = Str
            Return True

        End Function

        ''' <summary>
        ''' (5: ) set variable %Variable to current Furcadia
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function FurcDateTimeVar(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            SyncLock lock
                var.Value = FurcTime.ToString("yyyy-MM-dd HH:mm:ss tt")
            End SyncLock
            Return True

        End Function

        ''' <summary>
        ''' (5: ) set variable %Variable to current DateTime
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function LocalDateTimeVar(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            var.Value = Date.Now.ToString("yyyy-MM-dd HH:mm:ss tt")
            Return True

        End Function

        Function MsFurcTime(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Dim Str As String = ""
            SyncLock lock
                Str = FurcTime.ToLongTimeString.ToLower
            End SyncLock
            Var.Value = Str
            Return True

        End Function

        ''' <summary>
        ''' (5: ) use variable %Variable as a DateTime string and subtract
        ''' Date Time string {...} and put it into variable %Variable
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function SubsractDateTimeStr(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            Dim str As String = reader.ReadString
            Dim optVar As Variable = reader.ReadVariable(True)
            Dim time As DateTime
            Dim time2 As DateTime
            If DateTime.TryParse(var.Value.ToString, time) And DateTime.TryParse(str, time2) Then
                optVar.Value = time.Subtract(time2).ToString
                Return True
            End If
            Return False

        End Function

        ''' <summary>
        ''' (5: ) use variable %Variable as a DateTime string and subtract
        ''' Date Time variable %Variable and put it into variable %Variable
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function SubsractDateTimeVar(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            Dim str As Variable = reader.ReadVariable
            Dim optVar As Variable = reader.ReadVariable(True)
            Dim time As DateTime
            Dim time2 As DateTime
            If DateTime.TryParse(var.Value.ToString, time) And DateTime.TryParse(str.Value.ToString, time2) Then
                optVar.Value = time.Subtract(time2).ToString
            End If
            Return True

        End Function

#End Region

        '(5: ) use variable %Variable as a DateTime string and add Date Time string {...} and put it into variable %Variable

        '(5: ) use variable %Variable as a DateTime string and add Date Time variable %Variable and put it into variable %Variable
        'Public Function AddDateTimeVar(reader As TriggerReader) As Boolean
        '    Try
        '        Dim var As Variable = reader.ReadVariable(True)
        '        Dim str As Variable = reader.ReadVariable
        '        Dim optVar As Variable = reader.ReadVariable(True)
        '        Dim time As DateTime
        '        Dim time2 As DateTime
        '        If DateTime.TryParse(var.Value.ToString, time) And DateTime.TryParse(str.Value.ToString, time2) Then
        '            optVar.Value(time.Add(time2).ToString)
        '            Return True
        '        End If
        '        Return False
        '    Catch ex As Exception
        '       LogError(reader, ex)
        '       Return False
        '    End Try
        'End Function

    End Class

End Namespace