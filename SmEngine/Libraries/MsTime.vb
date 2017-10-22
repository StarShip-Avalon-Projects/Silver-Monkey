Imports Monkeyspeak

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
    ''' <remarks>
    ''' This Library includes the following MonkeySpeak unnamed delegates:
    ''' <para>
    ''' (0:299) When the time is {...} hh:mm:ss am/pm FST,
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class MsTime
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Dim lock As New Object

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
        End Sub

        Public Overrides Sub Initialize()
            '(0:299) When the time is {...} hh:mm:ss am/pm FST,
            Add(TriggerCategory.Cause, 299,
                Function(reader As TriggerReader)
                    Dim Time = reader.ReadString
                    Dim str As String = ""
                    SyncLock lock
                        str = FurcTime.ToLongTimeString
                    End SyncLock
                    Return str.ToUpper = Time.ToUpper
                End Function,
            " When the time is {...} hh:mm:ss am/pm FST,")

            '(5:30) set variable %Variable to the current local time.
            Add(TriggerCategory.Effect, 30,
                AddressOf CurrentTime, " set variable %Variable to the current local time.")

            '(5:31) set variable %Variable to the current Furcadia Standard time
            Add(TriggerCategory.Effect, 31,
                AddressOf MsFurcTime, " set variable %Variable to the current Furcadia Standard time.")
            '(5:32) set variable %Variable to current DateTime
            Add(TriggerCategory.Effect, 32,
                AddressOf LocalDateTimeVar, " set variable %Variable to current local DateTime.")
            '(5:33) set variable %Variable to current Furcadia DateTime
            Add(TriggerCategory.Effect, 33,
                AddressOf FurcDateTimeVar, " set variable %Variable to current Furcadia DateTime.")
            '(5:34) use variable %Variable as a DateTime string and subtract Date Time string {...} and put it into variable %Variable
            Add(TriggerCategory.Effect, 34,
                AddressOf SubsractDateTimeStr, " use variable %Variable as a DateTime string and subtract Date Time string {...} and put it into variable %Variable.")
            '(5:35) use variable %Variable as a DateTime string and subtract Date Time variable %Variable and put it into variable %Variable
            Add(TriggerCategory.Effect, 35,
                AddressOf SubsractDateTimeVar, " use variable %Variable as a DateTime string and subtract Date Time variable %Variable and put it into variable %Variable.")
            '(5:36) use variable %Variable as a DateTime string and add Date Time string {...} and put it into variable %Variable
            '        Add(TriggerCategory.Effect, 36,
            'AddressOf AddDateTimeStr, " use variable %Variable as a DateTime string and add Date Time string {...} and put it into variable %Variable")
            '        '(5:37) use variable %Variable as a DateTime string and add Date Time variable %Variable and put it into variable %Variable
            '        Add(TriggerCategory.Effect, 37,
            'AddressOf AddDateTimeVar, " use variable %Variable as a DateTime string and add Date Time variable %Variable and put it into variable %Variable")
            '
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:30) set variable %Variable to the current local time.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Shared Function CurrentTime(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)
            Dim Str = DateTime.Now.ToLongTimeString.ToLower
            Var.Value = Str
            Return True

        End Function

        ''' <summary>
        ''' (5:33) set variable %Variable to current Furcadia DateTime.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Function FurcDateTimeVar(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable(True)
            SyncLock lock
                var.Value = FurcTime.ToString("yyyy-MM-dd HH:mm:ss tt")
            End SyncLock
            Return True

        End Function

        ''' <summary>
        ''' (5:32) set variable %Variable to current local DateTime.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Shared Function LocalDateTimeVar(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable(True)
            var.Value = Date.Now.ToString("yyyy-MM-dd HH:mm:ss tt")
            Return True

        End Function

        ''' <summary>
        ''' (5:31) set variable %Variable to the current Furcadia Standard time.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Function MsFurcTime(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)
            Dim Str As String = ""
            SyncLock lock
                Str = FurcTime.ToLongTimeString.ToLower
            End SyncLock
            Var.Value = Str
            Return True

        End Function

        ''' <summary>
        ''' (5:34) use variable %Variable as a DateTime string and subtract
        ''' Date Time string {...} and put it into variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when DateTimeVariable and DateTimeString are not in the
        ''' correct format
        ''' </exception>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Shared Function SubsractDateTimeStr(reader As TriggerReader) As Boolean

            Dim DateTimeVariable = reader.ReadVariable(True)
            Dim DateTimeString = reader.ReadString
            Dim ResultVariable = reader.ReadVariable(True)
            Dim time As DateTime
            Dim time2 As DateTime
            If DateTime.TryParse(DateTimeVariable.Value.ToString, time) And DateTime.TryParse(DateTimeString, time2) Then
                ResultVariable.Value = time.Subtract(time2).ToString
                Return True
            End If
            Throw New MonkeyspeakException("unable to parse DateTime variable and/or DateTimeString")

        End Function

        ''' <summary>
        ''' (5:35) use variable %Variable as a DateTime string and subtract
        ''' Date Time variable %Variable and put it into variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <exception cref="MonkeySpeakException">Thrown when
        ''' DateTimeVariable and DateTimeString are not in the correct format</exception><see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Shared Function SubsractDateTimeVar(reader As TriggerReader) As Boolean

            Dim DateTimeVariable = reader.ReadVariable(True)
            Dim DateTimeString = reader.ReadVariable
            Dim ResultVariable = reader.ReadVariable(True)
            Dim time As DateTime
            Dim time2 As DateTime
            If DateTime.TryParse(DateTimeVariable.Value.ToString, time) And DateTime.TryParse(DateTimeString.Value.ToString, time2) Then
                ResultVariable.Value = time.Subtract(time2).ToString
            End If
            Throw New MonkeyspeakException("unable to parse DateTime variable and/or DateTimeString")

        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

        '(5: ) use variable %Variable as a DateTime string and add Date Time string {...} and put it into variable %Variable

        '(5: ) use variable %Variable as a DateTime string and add Date Time variable %Variable and put it into variable %Variable
        'Public Function AddDateTimeVar(reader As TriggerReader) As Boolean
        '    Try
        '        Dim var  = reader.ReadVariable(True)
        '        Dim str  = reader.ReadVariable
        '        Dim optVar  = reader.ReadVariable(True)
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