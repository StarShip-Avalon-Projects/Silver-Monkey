Imports Monkeyspeak

Imports System.Diagnostics
Imports Furcadia.Net

Namespace Engine.Libraries
    Public Class WmCpyDta
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
            '(0:75) When the bot receives a message from another bot on the same computer,
            Add(TriggerCategory.Cause, 75,
Function()
    Return True
End Function, "(0:75) When the bot receives a message from another bot on the same computer,")
            '(0:76) When the bot receives message {...} from another bot on the same computer,
            Add(TriggerCategory.Cause, 76,
AddressOf ReceiveMessage, "(0:76) When the bot receives message {...} from another bot on the same computer,")
            '(0:77) When the bot receives a message containing {...} from another bot on the same computer,
            Add(TriggerCategory.Cause, 77,
AddressOf ReceiveMessageContaining, "(0:77) When the bot receives a message containing {...} from another bot on the same computer,")

            '(5:75) send message {...} to bot named {...}.
            Add(TriggerCategory.Effect, 75,
AddressOf SendMessage, "(5:75) send message {...} to bot named {...}.")

            '(5:76) set Variable %Variable to the Message the bot last received.
            Add(TriggerCategory.Effect, 76,
AddressOf SetVariable, "(5:76) set Variable %Variable to the Message the bot last received.")
        End Sub

#End Region

#Region "Public Methods"

        '(0:76) When the bot receives message {...} from another bot on the same computer,
        Function ReceiveMessage(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim msg As String = ""
            Try
                'Debug.Print("msgContains Begin Execution")
                msMsg = reader.ReadString()
                'Debug.Print("msMsg = " & msMsg)
                msg = MsPage.GetVariable("MESSAGE").Value.ToString
                'Debug.Print("Msg = " & msg)
                Return msg.Equals(msMsg)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(0:77) When the bot receives a message containing {...} from another bot on the same computer,
        Function ReceiveMessageContaining(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim msg As String = ""
            Try
                'Debug.Print("msgContains Begin Execution")
                msMsg = reader.ReadString()
                'Debug.Print("msMsg = " & msMsg)
                msg = MsPage.GetVariable("MESSAGE").Value.ToString
                'Debug.Print("Msg = " & msg)
                Return msg.Contains(msMsg)
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(5:75) send message {...} to bot named {...}.
        Function SendMessage(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim Fur As String = ""
            Try
                'Debug.Print("msgContains Begin Execution")
                msMsg = reader.ReadString().Trim
                'Debug.Print("msMsg = " & msMsg)
                Fur = reader.ReadString()
                'Step 1.
                'To send a message to another application the first thing we need is the
                'handle of the receiving application.
                'One way is to use the FindWindow API
                Dim cstrReceiverWindowName As String = "Silver Monkey: " + Fur
                Dim WindowHandleOfToProcess As Integer = FindWindow(Nothing, cstrReceiverWindowName)
                'find by window name
                Dim WindowHandle As New IntPtr(WindowHandleOfToProcess)
                Dim msg As MessageHelper = New MessageHelper
                'Step 2.
                'Create some information to send.
                Dim strTag As String = "MSG"

                Dim iResult As IntPtr = IntPtr.Zero
                If WindowHandle <> IntPtr.Zero Then
                    iResult = msg.sendWindowsStringMessage(WindowHandle, IntPtr.Zero, FurcadiaSession.BotName, FurcadiaSession.BotUID, strTag, msMsg)
                    'SendClientMessage("SYSTEM Send Windows Message to " + Fur + ": ", msMsg)
                End If
                'Debug.Print("Msg = " & msg)
                Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

        '(5:76) set Variable %Variable to the Message the bot last received.
        Function SetVariable(reader As TriggerReader) As Boolean
            Dim Var As Variable
            Try
                Var = reader.ReadVariable(True)
                Var.Value = FurcadiaSession.Player.Message
                Return True
            Catch ex As Exception
                Dim tID As String = reader.TriggerId.ToString
                Dim tCat As String = reader.TriggerCategory.ToString
                Console.WriteLine(MS_ErrWarning)
                Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                writer.WriteLine(ErrorString)
                Debug.Print(ErrorString)
                Return False
            End Try
        End Function

#End Region

#Region "Private Methods"

        Private Shared Function FindWindow(_ClassName As String, _WindowName As String) As Integer
        End Function

#End Region

    End Class
End Namespace