﻿Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' General waring messages (Warning Channel?) Furcadia sends.
    ''' </summary>
    Public Class MsWarning
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Dim Lock As New Object()

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)

            '(0:800) When the bot sees error message {...},
            Add(TriggerCategory.Cause, 800,
                AddressOf ErrorIs, "(0:800) When the bot sees error message {...},")

            '(0:801) when the bot sees warning message{...},
            Add(TriggerCategory.Cause, 801,
              AddressOf ErrorIs, "(0:801) when the bot sees warning message{...},")

            '(1:800) and the last command sent returned an error or warning # (zero = none, one = warning, two = error)
            Add(New Trigger(TriggerCategory.Condition, 800), AddressOf CommandWariningOrError,
                "(1:800) and the last command sent returned an error or warning # (zero = none, one = warning, two = error)")

            '(1:801) and the last command sent didn't return an error or warning # (zero = none, one = warning, two = error)
            Add(New Trigger(TriggerCategory.Condition, 801), AddressOf CommandNotWariningOrError,
                "(1:801) and the last command sent didn't return an error or warning # (zero = none, one = warning, two = error)")

            '(5:800) set %Variable to the value of the message returned by the last command line. (zero = none, one = warning, two = error)
            Add(New Trigger(TriggerCategory.Effect, 800), AddressOf CommandNotWariningOrError,
                "(5:800) set %Variable to the value of the message returned by the last command line. (zero = none, one = warning, two = error)")

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (1:801) and the last command sent didn't return an error or
        ''' warning # (zero = none, one = warning, two = error)
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function CommandNotWariningOrError(reader As TriggerReader) As Boolean

            Dim err As Double = ReadVariableOrNumber(reader, False)
            Dim errNum As Short = 0
            SyncLock Lock
                errNum = FurcadiaSession.ErrorNum
            End SyncLock
            'add Machine Name parser
            Return Convert.ToDouble(errNum) <> err

        End Function

        ''' <summary>
        ''' (1:800) and the last command sent returned an error or warning #
        ''' (zero = none, one = warning, two = error)
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function CommandWariningOrError(reader As TriggerReader) As Boolean

            Dim err As Double = ReadVariableOrNumber(reader, False)
            Dim errNum As Short = 0
            SyncLock Lock
                errNum = FurcadiaSession.ErrorNum
            End SyncLock
            'add Machine Name parser
            Return Convert.ToDouble(errNum) = err

        End Function

        ''' <summary>
        ''' (0:800) When the bot sees error message {...},
        ''' <para>
        ''' 0:801) when the bot sees warning message{...},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function ErrorIs(reader As TriggerReader) As Boolean

            Dim TmpName As String = reader.ReadString()
            Dim errstr As String = ""
            SyncLock Lock
                errstr = FurcadiaSession.ErrorMsg
            End SyncLock
            'add Machine Name parser
            Return TmpName = errstr

        End Function

        ''' <summary>
        ''' (5:800) set %Variable to the value of the message returned by
        ''' the last command line. (zero = none, one = warning, two = error)
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function WarningVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Dim errNum As Short = 0
            SyncLock Lock
                errNum = FurcadiaSession.ErrorNum
            End SyncLock
            'add Machine Name parser
            Var.Value = Convert.ToDouble(errNum)
            Return True

        End Function

#End Region

    End Class

End Namespace