Imports System.Runtime.InteropServices
Imports MonkeyCore.Utils
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Bot to Bot Messaging using Window Calls
    ''' </summary>
    Public Class WmCpyDta
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"
        ''' <summary>
        ''' Setup Windows messaging system
        ''' </summary>
        ''' <param name="session"></param>
        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
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

        ''' <summary>
        ''' (0:76) When the bot receives message {...} from another bot on
        ''' the same computer,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function ReceiveMessage(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim msg As String = ""

            'Debug.Print("msgContains Begin Execution")
            msMsg = reader.ReadString()
            'Debug.Print("msMsg = " & msMsg)
            msg = reader.Page.GetVariable("MESSAGE").Value.ToString
            'Debug.Print("Msg = " & msg)
            Return msg.Equals(msMsg)

        End Function

        ''' <summary>
        ''' (0:77) When the bot receives a message containing {...} from
        ''' another bot on the same computer,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function ReceiveMessageContaining(reader As TriggerReader) As Boolean
            Dim msMsg As String = ""
            Dim msg As String = ""

            'Debug.Print("msgContains Begin Execution")
            msMsg = reader.ReadString()
            'Debug.Print("msMsg = " & msMsg)
            msg = reader.Page.GetVariable("MESSAGE").Value.ToString
            'Debug.Print("Msg = " & msg)
            Return msg.Contains(msMsg)

        End Function

        ''' <summary>
        ''' (5:75) send message {...} to bot named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function SendMessage(reader As TriggerReader) As Boolean


            'Debug.Print("msgContains Begin Execution")
            Dim msMsg = reader.ReadString().Trim
            'Debug.Print("msMsg = " & msMsg)
            Dim Fur = reader.ReadString()
            'Step 1.
            'To send a message to another application the first thing we need is the
            'handle of the receiving application.
            'One way is to use the FindWindow API
            Dim cstrReceiverWindowName As String = "Silver Monkey: " + Fur
                Dim WindowHandleOfToProcess As Integer = FindWindow(Nothing, cstrReceiverWindowName)
                'find by window name
                Dim WindowHandle As New IntPtr(WindowHandleOfToProcess)
                Dim msg As MessageHelper = New MonkeyCore.Utils.MessageHelper
                'Step 2.
                'Create some information to send.
                Dim strTag As String = "MSG"

                Dim iResult As IntPtr = IntPtr.Zero
                If WindowHandle <> IntPtr.Zero Then
                iResult = msg.SendWindowsStringMessage(WindowHandle,
                           IntPtr.Zero,
                           FurcadiaSession.ConnectedCharacterName,
                           FurcadiaSession.ConnectedCharacterFurcadiaID, strTag, msMsg)
                SendClientMessage("SYSTEM Send Windows Message to " + Fur + ": " + msMsg)
            End If
                'Debug.Print("Msg = " & msg)
                Return True

                Return False
        End Function

        ''' <summary>
        ''' (5:76) set Variable %Variable to the Message the bot last received.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function SetVariable(reader As TriggerReader) As Boolean
            Dim Var As Variable

            Var = reader.ReadVariable(True)
            Var.Value = FurcadiaSession.Player.Message
            Return True

        End Function

#End Region

#Region "Private Methods"

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
        Private Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
        End Function

#End Region

    End Class

End Namespace