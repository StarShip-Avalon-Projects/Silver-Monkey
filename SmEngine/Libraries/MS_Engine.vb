Imports System.Text.RegularExpressions
Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Util
Imports Monkeyspeak
Imports SilverMonkeyEngine.SmConstants

'StringLibrary
'(1:108) - (1:111)
'(5:110) - (5:119)
'MathLibrary

'SayLibrary
Namespace Engine.Libraries
    ''' <summary>
    ''' Modules Based on SilverMonkey VB Example Modified by Gerolkae Script
    ''' Loader Moved to SilverMonkey.Main
    ''' </summary>
    Module MS_Engine

        Class MathLibrary
            Inherits MonkeySpeakLibrary

#Region "Public Constructors"

            Sub New(ByRef Session As BotSession)
                MyBase.New(Session)

            End Sub

#End Region

        End Class

        Class SayLibrary
            Inherits MonkeySpeakLibrary

#Region "Public Constructors"

            Public Sub New(ByRef Session As BotSession)
                MyBase.New(Session)
                Add(TriggerCategory.Cause, 1,
                Function()
                    Return True
                End Function, "(0:1) When the bot logs into furcadia,")
                Add(TriggerCategory.Cause, 2,
                Function()
                    Return True
                End Function, "(0:2) When the bot logs out of furcadia,")
                Add(TriggerCategory.Cause, 3,
                Function()
                    Return True
                End Function, "(0:3) When the Furcadia client disconnects or closes,")

                'says
                Add(TriggerCategory.Cause, 5,
                      Function()
                          ' Console.WriteLine("Cause (0:5):")
                          Return Not FurcadiaSession.IsConnectedCharacter
                      End Function,
                   "(0:5) When someone says something,")
                Add(TriggerCategory.Cause, 6,
                 AddressOf msgIs, "(0:6) When someone says {.},")

                '(0:7) When some one says something with {.} in it
                Add(TriggerCategory.Cause, 7,
                 AddressOf msgContains, "(0:7) When someone says something with {.} in it,")

                'Shouts
                Add(TriggerCategory.Cause, 8,
           Function()
               Return Not FurcadiaSession.IsConnectedCharacter
           End Function,
        "(0:8) When someone shouts something,")
                Add(TriggerCategory.Cause, 9,
                 AddressOf msgIs, "(0:9) When someone shouts {.},")

                '(0:10) When some one shouts something with {.} in it
                Add(TriggerCategory.Cause, 10,
                 AddressOf msgContains, "(0:10) When someone shouts something with {.} in it,")

                'emotes
                Add(TriggerCategory.Cause, 11,
           Function()
               Return Not FurcadiaSession.IsConnectedCharacter

           End Function,
        "(0:11) When someone emotes something,")
                Add(TriggerCategory.Cause, 12,
                 AddressOf msgIs, "(0:12) When someone emotes {.},")

                '(0:13) When some one emotes something with {.} in it
                Add(TriggerCategory.Cause, 13,
                 AddressOf msgContains, "(0:13) When someone emotes something with {.} in it,")

                'Whispers
                Add(TriggerCategory.Cause, 15,
           Function()
               Return Not FurcadiaSession.IsConnectedCharacter
           End Function,
        "(0:15) When someone whispers something,")
                Add(TriggerCategory.Cause, 16,
                 AddressOf msgIs, "(0:16) When someone whispers {.},")

                '(0:13) When some one emotes something with {.} in it
                Add(TriggerCategory.Cause, 17,
                 AddressOf msgContains, "(0:17) When someone whispers something with {.} in it,")

                'Says or Emotes
                Add(TriggerCategory.Cause, 18,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:18) When someone says or emotes something,")
                Add(TriggerCategory.Cause, 19,
                 AddressOf msgIs, "(0:19) When someone says or emotes {.},")

                '(0:13) When some one emotes something with {.} in it
                Add(TriggerCategory.Cause, 20,
                 AddressOf msgContains, "(0:20) When someone says or emotes something with {.} in it,")

                'Emits
                Add(TriggerCategory.Cause, 21,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:21) When someone emits something,")
                Add(TriggerCategory.Cause, 22,
                 AddressOf msgIs, "(0:22) When someone emits {.},")

                '(0:13) When some one emotes something with {.} in it
                Add(TriggerCategory.Cause, 23,
                 AddressOf msgContains, "(0:23) When someone emits something with {.} in it,")

                'Furre Enters
                '(0:4) When someone is added to the FurcadiaSession.Dream manifest,
                Add(TriggerCategory.Cause, 24,
                    Function()
                        Return True
                    End Function, "(0:24) When someone enters the FurcadiaSession.Dream,")
                '(0:25) When a furre Named {.} enters the FurcadiaSession.Dream,
                Add(TriggerCategory.Cause, 25,
                AddressOf NameIs, "(0:25) When a furre Named {.} enters the FurcadiaSession.Dream,")
                'Furre Leaves
                '(0:25) When someone leaves the FurcadiaSession.Dream,
                Add(TriggerCategory.Cause, 26,
                    Function()
                        Return True
                    End Function, "(0:26) When someone leaves the FurcadiaSession.Dream, ")
                '(0:27) When a furre named {.} leaves the FurcadiaSession.Dream,
                Add(TriggerCategory.Cause, 27,
                AddressOf NameIs, "(0:27) When a furre named {.} leaves the FurcadiaSession.Dream")

                'Furre In View
                '(0:28) When someone enters the bots view,
                Add(TriggerCategory.Cause, 28,
                AddressOf EnterView, "(0:28) When someone enters the bots view, ")
                '(0:28) When a furre named {.} enters the bots view
                Add(TriggerCategory.Cause, 29,
                AddressOf FurreNamedEnterView, "(0:29) When a furre named {.} enters the bots view")
                'Furre Leave View
                '(0:30) When someone leaves the bots view,
                Add(TriggerCategory.Cause, 30,
                AddressOf LeaveView, "(0:30) When someone leaves the bots view, ")
                '(0:31) When a furre named {.} leaves the bots view
                Add(TriggerCategory.Cause, 31,
                AddressOf FurreNamedLeaveView, "(0:31) When a furre named {.} leaves the bots view")

                'Summon
                '(0:32) When someone requests to summon the bot,
                Add(TriggerCategory.Cause, 32,
                  Function()
                      Return Not FurcadiaSession.IsConnectedCharacter
                  End Function, "(0:32) When someone requests to summon the bot,")

                '(0:33) When a furre named {.} requests to summon the bot,
                Add(TriggerCategory.Cause, 33,
                AddressOf NameIs, "(0:33) When a furre named {.} requests to summon the bot,")
                'Join
                '(0:34) When someone requests to join the bot,
                Add(TriggerCategory.Cause, 34,
                    Function()
                        Return Not FurcadiaSession.IsConnectedCharacter
                    End Function, "(0:34) When someone requests to join the bot,")
                '(0:35) When a furre named {.} requests to join the bot,
                Add(TriggerCategory.Cause, 35,
                AddressOf NameIs, "(0:35) When a furre named {.} requests to join the bot,")
                'Follow
                '(0:36) When someone requests to follow the bot,
                Add(TriggerCategory.Cause, 36,
                    Function()
                        Return Not FurcadiaSession.IsConnectedCharacter
                    End Function, "(0:36) When someone requests to follow the bot,")
                '(0:37) When a furre named {.} requests to follow the bot,
                Add(TriggerCategory.Cause, 37,
                AddressOf NameIs, "(0:37) When a furre named {.} requests to follow the bot,")
                'Lead
                '(0:38) When someone requests to lead the bot,
                Add(TriggerCategory.Cause, 38,
                    Function()
                        Return Not FurcadiaSession.IsConnectedCharacter
                    End Function, "(0:38) When someone requests to lead the bot,")
                '(0:39) When a furre named {.} requests to lead the bot,
                Add(TriggerCategory.Cause, 39,
                AddressOf NameIs, "(0:39) When a furre named {.} requests to lead the bot,")
                'Cuddle
                '(0:40) When someone requests to cuddle with the bot.
                Add(TriggerCategory.Cause, 40,
                    Function()
                        Return Not FurcadiaSession.IsConnectedCharacter
                    End Function, "(0:40) When someone requests to cuddle with the bot,")
                '(0:41) When a furre named {.} requests to cuddle with the bot,
                Add(TriggerCategory.Cause, 41,
                AddressOf NameIs, "(0:41) When a furre named {.} requests to cuddle with the bot,")

                'Trade rewuests

                '(0:46) When the bot sees a trade request,
                Add(TriggerCategory.Cause, 46,
                    Function()
                        Return Not FurcadiaSession.IsConnectedCharacter
                    End Function, "(0:46) When the bot sees a trade request,")
                '(0:47) When the bot sees the trade request {.},
                Add(TriggerCategory.Cause, 47,
                 AddressOf msgIs, "(0:47) When the bot sees the trade request {.}")

                '(0:48) When the bot sees a trade request with {.} in it,
                Add(TriggerCategory.Cause, 48,
                 AddressOf msgContains, "(0:48) When the bot sees a trade request with {.} in it,")

                'FurcadiaSession.Dream
                '(0:90) When the bot enters a FurcadiaSession.Dream,
                Add(TriggerCategory.Cause, 90,
                Function()
                    Return True
                End Function, "(0:90) When the bot enters a FurcadiaSession.Dream,")
                '(0:91) When the bot enters a FurcadiaSession.Dream named {.},
                Add(TriggerCategory.Cause, 91,
                AddressOf EnterMyDreamNamed, "(0:91) When the bot enters a FurcadiaSession.Dream named {.},")

                '(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
                Add(TriggerCategory.Cause, 92,
                Function()
                    Return True
                End Function, "(0:92) When the bot detects the ""Your throat is tired. Please wait a few seconds"" message,")

                '(0:93) When the bot resumes processing after seeing "Your throat is tired" message,
                Add(TriggerCategory.Cause, 93,
                Function()
                    Return True
                End Function, "(0:93) When the bot resumes processing after seeing ""Your throat is tired"" message,")

                ' (1:3) and the triggering furre's name is {.},
                Add(New Trigger(TriggerCategory.Condition, 5), AddressOf NameIs, "(1:5) and the triggering furre's name is {.},")
                ' (1:4) and the triggering furre's name is not {.},
                Add(New Trigger(TriggerCategory.Condition, 6), AddressOf NameIsNot, "(1:6) and the triggering furre's name is not {.},")

                ' (1:5) and the Triggering Furre's message is {.}, (say,
                ' emote, shot, whisper, or emit Channels)
                Add(New Trigger(TriggerCategory.Condition, 7), AddressOf msgIs, "(1:7) and the triggering furre's message is {.},")
                ' (1:8) and the triggering furre's message contains {.} in
                ' it, (say, emote, shot, whisper, or emit Channels)
                Add(New Trigger(TriggerCategory.Condition, 8), AddressOf msgContains, "(1:8) and the triggering furre's message contains {.} in it,")
                '(1:9) and the triggering furre's message does not contain {.} in it, (say, emote, shot, whisper, or emit Channels)
                Add(New Trigger(TriggerCategory.Condition, 9), AddressOf msgNotContain, "(1:9) and the triggering furre's message does not contain {.} in it,")
                '(1:10) and the triggering furre's message is not {.}, (say, emote, shot, whisper, or emit Channels)
                Add(New Trigger(TriggerCategory.Condition, 10), AddressOf msgIsNot, "(1:10) and the triggering furre's message is not {.},")

                '(1:11) and triggering furre's message starts with {.},
                Add(New Trigger(TriggerCategory.Condition, 11), AddressOf msgStartsWith, "(1:11) and triggering furre's message starts with {.},")
                '(1:12) and triggering furre's message doesn't start with {.},
                Add(New Trigger(TriggerCategory.Condition, 12), AddressOf msgNotStartsWith, "(1:12) and triggering furre's message doesn't start with {.},")
                '(1:13) and triggering furre's message  ends with {.},
                Add(New Trigger(TriggerCategory.Condition, 13), AddressOf msgEndsWith, "(1:13) and triggering furre's message  ends with {.},")
                '(1:14) and triggering furre's message doesn't end with {.},
                Add(New Trigger(TriggerCategory.Condition, 14), AddressOf msgNotEndsWith, "(1:14) and triggering furre's message doesn't end with {.},")

                '(1:904) and the triggering furre is the Bot Controller,
                Add(TriggerCategory.Condition, 15,
                Function()
                    Return FurcadiaSession.IsBotController
                End Function, "(1:15) and the triggering furre is the Bot Controller,")

                '(1:905) and the triggering furre is not the Bot Controller,
                Add(New Trigger(TriggerCategory.Condition, 16),
                Function()
                    Return Not FurcadiaSession.IsBotController
                End Function, "(1:16) and the triggering furre is not the Bot Controller,")

                '(1:17) and the triggering furre's name is {.}.
                Add(New Trigger(TriggerCategory.Condition, 17), AddressOf TrigFurreNameIs, "(1:17) and the triggering furre's name is {.},")

                '(1:18) and the triggering furre's name is not {.}.
                Add(New Trigger(TriggerCategory.Condition, 18), AddressOf TrigFurreNameIsNot, "(1:18) and the triggering furre's name is not {.},")

                '(1:19) and the bot is the FurcadiaSession.Dream owner,
                Add(New Trigger(TriggerCategory.Condition, 19), AddressOf BotIsDREAMOWNER, "(1:19) and the bot is the FurcadiaSession.Dream owner,")
                '(1:20) and the bot is not the FurcadiaSession.Dream owner,
                Add(New Trigger(TriggerCategory.Condition, 20), AddressOf BotIsNotDREAMOWNER, "(1:20) and the bot is not the FurcadiaSession.Dream owner,")
                '(1:21) and the furre named {.} is the FurcadiaSession.Dream owner,
                Add(New Trigger(TriggerCategory.Condition, 21), AddressOf FurreNamedIsDREAMOWNER, "(1:21) and the furre named {.} is the FurcadiaSession.Dream owner,")
                '(1:22) and the furre named {.} is not the FurcadiaSession.Dream owner,
                Add(New Trigger(TriggerCategory.Condition, 22), AddressOf FurreNamedIsNotDREAMOWNER, "(1:22) and the furre named {.} is not the FurcadiaSession.Dream owner,")
                '(1:23) and the FurcadiaSession.Dream Name is {.},
                Add(New Trigger(TriggerCategory.Condition, 23), AddressOf MyDreamNameIs, "(1:23) and the FurcadiaSession.Dream Name is {.},")
                '(1:24) and the FurcadiaSession.Dream Name is not {.},
                Add(New Trigger(TriggerCategory.Condition, 24), AddressOf MyDreamNameIsNot, "(1:24) and the FurcadiaSession.Dream Name is not {.},")
                '(1:25) and the triggering furre is the FurcadiaSession.Dream owner
                Add(New Trigger(TriggerCategory.Condition, 25), AddressOf TriggeringFurreIsDREAMOWNER, "(1:25) and the triggering furre is the FurcadiaSession.Dream owner,")
                '(1:26) and the triggering furre is not the FurcadiaSession.Dream owner
                Add(New Trigger(TriggerCategory.Condition, 26), AddressOf TriggeringFurreIsNotDREAMOWNER, "(1:26) and the triggering furre is not the FurcadiaSession.Dream owner,")

                '(1:27) and the bot has share control of the Dream or is the Dream owner,
                Add(TriggerCategory.Condition, 27,
                Function(reader As TriggerReader)
                    Dim tname As Variable = MsPage.GetVariable("DREAMOWNER")
                    If FurcadiaSession.HasShare Or (FurcadiaShortName(tname.Value.ToString()) = FurcadiaShortName(FurcadiaSession.ConnectedCharacterName)) Then
                        Return True
                    End If
                    Return False
                End Function, "(1:27) and the bot has share control of the Dream or is the Dream owner,")

                '(1:28) and the bot has share control of the FurcadiaSession.Dream,
                Add(New Trigger(TriggerCategory.Condition, 28),
                Function()
                    Return FurcadiaSession.HasShare
                End Function, "(1:28) and the bot has share control of the FurcadiaSession.Dream,")

                '(1:29) and the bot doesn't have share control in the FurcadiaSession.Dream,
                Add(New Trigger(TriggerCategory.Condition, 29),
                Function()
                    Return Not FurcadiaSession.HasShare
                End Function, "(1:29) and the bot doesn't have share control in the FurcadiaSession.Dream,")
                'Says
                ' (5:0) say {.}.
                Add(New Trigger(TriggerCategory.Effect, 0),
             Function(reader As TriggerReader)
                 Try
                     Dim msg As String = reader.ReadString(True)
                     sndSay(msg)
                     Return True
                 Catch ex As Exception
                     Dim tID As String = reader.TriggerId.ToString
                     Dim tCat As String = reader.TriggerCategory.ToString
                     Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                     Debug.Print(ErrorString)
                     Return False
                 End Try
             End Function,
             "(5:0) say {.}.")
                'emotes
                ' (5:1) emote {.}.
                Add(New Trigger(TriggerCategory.Effect, 1),
             Function(reader As TriggerReader)
                 Try
                     Dim msg As String = reader.ReadString
                     sndEmote(msg)
                     Return True
                 Catch ex As Exception
                     Dim tID As String = reader.TriggerId.ToString
                     Dim tCat As String = reader.TriggerCategory.ToString
                     Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                     Debug.Print(ErrorString)
                     Return False
                 End Try
             End Function,
             "(5:1) emote {.}.")

                'Shouts
                ' (5:2) shout {.}.
                Add(New Trigger(TriggerCategory.Effect, 2),
             Function(reader As TriggerReader)
                 Try
                     Dim msg As String = reader.ReadString
                     sndShout(msg)
                     Return True
                 Catch ex As Exception
                     Dim tID As String = reader.TriggerId.ToString
                     Dim tCat As String = reader.TriggerCategory.ToString
                     Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                     Debug.Print(ErrorString)
                     Return False
                 End Try
             End Function,
             "(5:2) shout {.}.")

                'Emits
                ' (5:3) emit {.}.
                Add(New Trigger(TriggerCategory.Effect, 3),
           Function(reader As TriggerReader)
               Try
                   Dim msg As String = reader.ReadString
                   sndEmit(msg)
                   Return True
               Catch ex As Exception
                   Dim tID As String = reader.TriggerId.ToString
                   Dim tCat As String = reader.TriggerCategory.ToString
                   Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                   Debug.Print(ErrorString)
                   Return False
               End Try
           End Function,
          "(5:3) Emit {.}.")
                ' (5:4) emitloud {.}.
                Add(New Trigger(TriggerCategory.Effect, 4),
           Function(reader As TriggerReader)
               Try
                   Dim msg As String = reader.ReadString
                   sndEmitLoud(msg)
                   Return True
               Catch ex As Exception
                   Dim tID As String = reader.TriggerId.ToString
                   Dim tCat As String = reader.TriggerCategory.ToString
                   Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                   Debug.Print(ErrorString)
                   Return False
               End Try
           End Function,
          "(5:4) Emitloud {.}.")

                'Whispers
                ' (5:5) whisper {.} to the triggering furre.
                Add(New Trigger(TriggerCategory.Effect, 5),
           Function(reader As TriggerReader)
               Try
                   Dim msg As String = reader.ReadString
                   Dim tname As Variable = MsPage.GetVariable(MS_Name)
                   sndWhisper(tname.Value.ToString, msg)
                   Return True
               Catch ex As Exception
                   Dim tID As String = reader.TriggerId.ToString
                   Dim tCat As String = reader.TriggerCategory.ToString
                   Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                   Debug.Print(ErrorString)
                   Return False
               End Try
           End Function,
         "(5:5) whisper {.} to the triggering furre.")

                ' (5:6) whisper {.} to {.}.
                Add(New Trigger(TriggerCategory.Effect, 6),
            Function(reader As TriggerReader)
                Try
                    Dim msg As String = reader.ReadString(True)
                    Dim tname As String = reader.ReadString
                    sndWhisper(tname, msg)
                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function,
         "(5:6) whisper {.} to {.}.")

                ' (5:7) whisper {.} to {.} even if they're offline.
                Add(New Trigger(TriggerCategory.Effect, 7),
          Function(reader As TriggerReader)
              Try
                  Dim msg As String = reader.ReadString
                  Dim tname As String = reader.ReadString

                  sndOffWhisper(tname, msg)
                  Return True
              Catch ex As Exception
                  Dim tID As String = reader.TriggerId.ToString
                  Dim tCat As String = reader.TriggerCategory.ToString
                  Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                  Debug.Print(ErrorString)
                  Return False
              End Try
          End Function,
            "(5:7) whisper {.} to {.} even if they're offline.")

                '(5:20) give share control to the triggering furre.
                Add(New Trigger(TriggerCategory.Effect, 20), AddressOf ShareTrigFurre, "(5:20) give share control to the triggering furre.")
                '(5:21) remove shae control from the triggering furre.
                Add(New Trigger(TriggerCategory.Effect, 21), AddressOf UnshareTrigFurre, "(5:21) remove share control from the triggering furre.")
                '(5:22) remove share from the furre named {.} if they're in the FurcadiaSession.Dream right now.
                Add(New Trigger(TriggerCategory.Effect, 22), AddressOf ShareFurreNamed, "(5:22) remove share from the furre named {.} if they're in the FurcadiaSession.Dream right now.")
                '(5:23) give share to the furre named {.} if they're in the FurcadiaSession.Dream right now.
                Add(New Trigger(TriggerCategory.Effect, 23), AddressOf UnshareFurreNamed, "(5:23) give share to the furre named {.} if they're in the FurcadiaSession.Dream right now.")

                '(5:40) Switch the bot to stand alone mode and close the Furcadia client.
                Add(New Trigger(TriggerCategory.Effect, 40), AddressOf StandAloneMode, "(5:40) Switch the bot to stand alone mode and close the Furcadia client.")

                '(5:41) Disconnect the bot from the Furcadia game server.
                Add(New Trigger(TriggerCategory.Effect, 41), AddressOf FurcadiaDisconnect, "(5:41) Disconnect the bot from the Furcadia game server.")

                '(5:42) start a new instance to Silver Monkey with botfile {.}.
                Add(New Trigger(TriggerCategory.Effect, 42), AddressOf StartNewBot,
                "(5:42) start a new instance to Silver Monkey with botfile {.}.")

            End Sub

#End Region

#Region "Public Methods"

            '(1:19) and the bot is the FurcadiaSession.Dream owner,
            Function BotIsDREAMOWNER(reader As TriggerReader) As Boolean
                Try
                    Dim tname As Variable = MsPage.GetVariable("DREAMOWNER")
                    'add Machine Name parser
                    If tname.Value Is Nothing Then
                        Return False
                    End If
                    Return FurcadiaShortName(tname.Value.ToString()) = FurcadiaSession.ConnectedFurre.ShortName
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    '
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    'Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function BotIsNotDREAMOWNER(reader As TriggerReader) As Boolean
                Return Not BotIsDREAMOWNER(reader)
            End Function

            '(0:91) When the bot enters a FurcadiaSession.Dream named {.},
            Public Function EnterMyDreamNamed(reader As TriggerReader) As Boolean
                Try
                    Dim msMsg As String = reader.ReadString()
                    Dim msg As Variable = MsPage.GetVariable("NAME")
                    Dim str As String = msMsg.ToLower.Replace("furc://", String.Empty).ToLower
                    If str.EndsWith("/") Then str = str.TrimEnd("/"c)
                    Dim str2 As String = Nothing
                    If Not String.IsNullOrEmpty(msg.Value.ToString) Then
                        str2 = msg.Value.ToString.ToLower.Replace("furc://", String.Empty)
                        If str2.EndsWith("/") Then str2 = str2.TrimEnd("/"c)
                    End If
                    Return str = str2
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Public Function EnterView(reader As TriggerReader) As Boolean
                Try
                    Dim tPlayer As FURRE = FurcadiaSession.Player
                    If tPlayer.Visible = tPlayer.WasVisible Then
                        Return False
                    End If
                    Return tPlayer.Visible = True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(5:41) Disconnect the bot from the Furcadia game server.
            Public Function FurcadiaDisconnect(reader As TriggerReader) As Boolean
                Try
                    FurcadiaSession.Disconnect()
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
                Return True
            End Function

            Public Function FurreNamedEnterView(reader As TriggerReader) As Boolean
                Try
                    Dim name As String = reader.ReadString
                    Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
                    If tPlayer.Visible = tPlayer.WasVisible Then
                        Return False
                    End If
                    Return tPlayer.Visible = True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(1:20) and the furre named {.} is the FurcadiaSession.Dream owner,
            Function FurreNamedIsDREAMOWNER(reader As TriggerReader) As Boolean
                Try
                    Dim tname As Variable = MsPage.GetVariable("DREAMOWNER")

                    Dim TrigFurreName As String = reader.ReadString
                    'add Machine Name parser
                    Return FurcadiaShortName(tname.Value.ToString()) = FurcadiaShortName(TrigFurreName)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    '
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    'Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function FurreNamedIsNotDREAMOWNER(reader As TriggerReader) As Boolean
                Return Not FurreNamedIsDREAMOWNER(reader)
            End Function

            Public Function FurreNamedLeaveView(reader As TriggerReader) As Boolean
                Try
                    Dim name As String = reader.ReadString
                    Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
                    If tPlayer.Visible = tPlayer.WasVisible Then
                        Return False
                    End If
                    Return tPlayer.Visible = False
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Public Function LeaveView(reader As TriggerReader) As Boolean
                Try
                    Dim tPlayer As FURRE = FurcadiaSession.Player
                    If tPlayer.Visible = tPlayer.WasVisible Then
                        Return False
                    End If
                    Return tPlayer.Visible = False
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(0:17) When someone whispers something with {.} in it,
            Function msgContains(reader As TriggerReader) As Boolean

                Try
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    Return test.Contains(msMsg)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(1:13) and triggering furre's message  ends with {.},
            Function msgEndsWith(reader As TriggerReader) As Boolean
                Try
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    'Debug.Print("Msg = " & msg)
                    Return test.EndsWith(msMsg)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Public Function msgIs(reader As TriggerReader) As Boolean
                Try
                    Dim safety As Boolean = Not FurcadiaSession.IsConnectedCharacter
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    Dim test2 As Boolean = msMsg.Equals(test) And safety
                    Return msMsg.Equals(test) And safety
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function msgIsNot(reader As TriggerReader) As Boolean
                Try
                    Dim safety As Boolean = Not FurcadiaSession.IsConnectedCharacter
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    Return Not msMsg.Equals(test) And safety
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function msgNotContain(reader As TriggerReader) As Boolean
                Try
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    Return test.Contains(msMsg)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(1:14) and triggering furre's message doesn't end with {.},
            Function msgNotEndsWith(reader As TriggerReader) As Boolean

                Try
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    'Debug.Print("Msg = " & msg)
                    Return Not test.EndsWith(msMsg)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(1:12) and triggering furre's message doesn't start with {.},
            Function msgNotStartsWith(reader As TriggerReader) As Boolean
                Try
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    Return Not test.StartsWith(msMsg)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(1:11) and triggering furre's message starts with {.},
            Function msgStartsWith(reader As TriggerReader) As Boolean
                Try
                    Dim msMsg As String = StripHTML(reader.ReadString())
                    Dim msg As Variable = MsPage.GetVariable("MESSAGE")

                    Dim test As String = StripHTML(msg.Value.ToString)
                    Return test.StartsWith(msMsg)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(1:21) and the FurcadiaSession.Dream Name is {.},
            Function MyDreamNameIs(reader As TriggerReader) As Boolean
                Try
                    Dim tname As Variable = MsPage.GetVariable("MyDreamNAME")
                    Dim TrigFurreName As String = reader.ReadString
                    TrigFurreName = TrigFurreName.ToLower.Replace("furc://", String.Empty)
                    'add Machine Name parser
                    Return FurcadiaShortName(tname.Value.ToString()) = FurcadiaShortName(TrigFurreName)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    '
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    'Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function MyDreamNameIsNot(reader As TriggerReader) As Boolean
                Return Not MyDreamNameIs(reader)
            End Function

            Function NameIs(reader As TriggerReader) As Boolean
                Try
                    Dim TmpName As String = reader.ReadString()
                    Dim tname As Variable = MsPage.GetVariable(MS_Name)
                    'add Machine Name parser
                    Return FurcadiaShortName(TmpName) = FurcadiaShortName(tname.Value.ToString)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    '
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    'Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function NameIsNot(reader As TriggerReader) As Boolean
                Try
                    Dim tname As String = MsPage.GetVariable(MS_Name).Value.ToString
                    Dim TmpName As String = reader.ReadString()
                    'add Machine Name parser
                    If FurcadiaShortName(TmpName) <> FurcadiaShortName(tname) Then Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
                Return False
            End Function

            '(5:22) remove share from the furre named {.} if they're in the FurcadiaSession.Dream right now.
            Public Function ShareFurreNamed(reader As TriggerReader) As Boolean
                Try
                    Dim furre As String = reader.ReadString
                    Dim Target As FURRE = Dream.FurreList.GerFurreByName(furre)
                    If InDream(Target.Name) Then sendServer("share " + FurcadiaShortName(furre))
                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(5:20) give share control to the triggering furre.
            Public Function ShareTrigFurre(reader As TriggerReader) As Boolean
                Dim furre As String = FurcadiaSession.Player.Name
                sendServer("share " + furre)
                Return True
            End Function

            Public Sub sndEmit(ByRef msg As String)
                If Not String.IsNullOrEmpty(msg) Then sendServer("emit " & msg)
            End Sub

            Public Sub sndEmitLoud(ByRef msg As String)
                If Not String.IsNullOrEmpty(msg) Then sendServer("emitloud " & msg)
            End Sub

            Public Sub sndEmote(ByRef msg As String)
                If Not String.IsNullOrEmpty(msg) Then sendServer(":" & msg)
            End Sub

            Public Sub sndOffWhisper(ByRef name As String, ByRef msg As String)
                If Not String.IsNullOrEmpty(msg) Then sendServer("/%%" & FurcadiaShortName(name) & " " & msg)
            End Sub

            Public Sub sndSay(ByRef msg As String)

                sendServer(msg)

            End Sub

            Public Sub sndShout(ByRef msg As String)
                If Not String.IsNullOrEmpty(msg) Then sendServer("-" & msg)
            End Sub

            Public Sub sndWhisper(ByRef name As String, ByRef msg As String)
                If Not String.IsNullOrEmpty(msg) Then sendServer("/%" & FurcadiaShortName(name) & " " & msg)
                ' Debug.Print("wh " & name & " " & msg)
            End Sub

            '(5:40) Switch the bot to stand alone mode and close the Furcadia client.
            Public Function StandAloneMode(reader As TriggerReader) As Boolean
                Try
                    FurcadiaSession.StandAlone = True
                    FurcadiaSession.CloseClient()
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
                Return True
            End Function

            '(5:42) start a new instance to Silver Monkey with botfile {.}.
            Public Function StartNewBot(reader As TriggerReader) As Boolean
                Try
                    'Dim ps As Process = New Process()
                    Dim File As String = reader.ReadString
                    Dim p As New ProcessStartInfo
                    p.Arguments = File
                    p.FileName = "SilverMonkey.exe"
                    Process.Start(p)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try

                Return True
            End Function

            Function TrigFurreNameIs(reader As TriggerReader) As Boolean
                Try
                    Dim TmpName As String = reader.ReadString()
                    Dim TrigFurreName As String = FurcadiaSession.Player.ShortName
                    'add Machine Name parser
                    Return Furcadia.Util.FurcadiaShortName(TmpName) = TrigFurreName
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    '
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    'Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function TrigFurreNameIsNot(reader As TriggerReader) As Boolean
                Return Not TrigFurreNameIs(reader)
            End Function

            '(1:22:) and the triggering furre is the FurcadiaSession.Dream owner
            Function TriggeringFurreIsDREAMOWNER(reader As TriggerReader) As Boolean
                Try
                    Dim tname As String = FurcadiaSession.Player.ShortName
                    Dim TrigFurreName As String = MsPage.GetVariable("DREAMOWNER").Value.ToString
                    'add Machine Name parser
                    Return tname = FurcadiaShortName(TrigFurreName)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    '
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    'Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Function TriggeringFurreIsNotDREAMOWNER(reader As TriggerReader) As Boolean
                Return Not TriggeringFurreIsDREAMOWNER(reader)
            End Function

            '(5:23) give share to the furre named {.} if they're in the FurcadiaSession.Dream right now.
            Public Function UnshareFurreNamed(reader As TriggerReader) As Boolean
                Try
                    Dim furre As String = reader.ReadString
                    Dim Target As FURRE = Dream.FurreList.GerFurreByName(furre)
                    If InDream(Target.Name) Then sendServer("unshare " + FurcadiaShortName(furre))
                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(5:21) remove shae control from the triggering furre.
            Public Function UnshareTrigFurre(reader As TriggerReader) As Boolean
                Dim furre As String = FurcadiaSession.Player.ShortName
                sendServer("unshare " + furre)
                Return True
            End Function

#End Region

#Region "Private Methods"

            Private Function InDream(ByRef Name As String) As Boolean
                Dim found As Boolean = False
                For Each kvp As KeyValuePair(Of Integer, FURRE) In FurcadiaSession.Dream.FurreList
                    If FurcadiaShortName(kvp.Value.Name) = FurcadiaShortName(Name) Then
                        Return True
                    End If
                Next
                Return False
            End Function

            ''' <summary>
            ''' </summary>
            ''' <param name="Text">
            ''' </param>
            ''' <returns>
            ''' </returns>
            Private Function StripHTML(ByVal Text As String) As String

                Dim r As New Regex("<(.*?)>")
                Text = r.Replace(Text, String.Empty)
                Return Text.Replace("|", " ").ToLower
            End Function

#End Region

        End Class

        Class StringLibrary
            Inherits MonkeySpeakLibrary

#Region "Public Constructors"

            Public Sub New(ByRef session As BotSession)
                MyBase.New(session)
                '(1:60) and variable %Variable matches wildcard expression {.} ( ""*"" or ""?""),
                Add(New Trigger(TriggerCategory.Condition, 60), AddressOf WildCard,
 "(1:60) and variable %Variable matches wildcard expression {.} ( ""*"" or ""?""),")
                Add(New Trigger(TriggerCategory.Condition, 61), AddressOf NotWildCard,
 "(1:61) and variable %Variable doesn't match wildcard expression {.} ( ""*"" or ""?""),")

                '(5:110) use variable % and take word # and put it into variable %
                Add(New Trigger(TriggerCategory.Effect, 120), AddressOf StringSplit,
             "(5:120) use variable %Variable and take word position # and put it into variable %Variable.")
                '(5:111) use variable % then remove character {.} and put it into variable %.
                Add(New Trigger(TriggerCategory.Effect, 121), AddressOf StripCharacters,
 "(5:121) use variable %Variable then remove all occurrences of character {.} and put it into variable %Variable.")
                '(5:122) chop off the beginning of variable %variable, removing the first # characters of it.
                Add(New Trigger(TriggerCategory.Effect, 122), AddressOf ChopStartString,
"(5:122) chop off the beginning of variable %variable, removing the first # characters of it.")
                '(5:123) chop off the end of variable %Variable, removing the last # characters of it.
                Add(New Trigger(TriggerCategory.Effect, 123), AddressOf ChopEndString,
           "(5:123) chop off the end of variable %Variable, removing the last # characters of it.")

                '(5:126) count the number of characters in string variable %variable and put them into variable %Variable .
                Add(New Trigger(TriggerCategory.Effect, 126), AddressOf CountChars,
  "(5:126) count the number of characters in string variable %variable and put them into variable %Variable.")

                '(5:127) take variable %Variable and Convert it to Furcadia short name. (with out special Characters or spaces)
                Add(New Trigger(TriggerCategory.Effect, 127), AddressOf ToShortName,
"(5:127) take variable %Variable and convert it to Furcadia short name. (without special characters or spaces or pipe ""|"").")

            End Sub

#End Region

#Region "Public Methods"

            '(5:123) chop off the end of variable %Variable, removing the last # characters of it.
            Public Function ChopEndString(reader As TriggerReader) As Boolean
                Dim Var As Variable
                Dim Count As Integer = 0
                Try
                    Var = reader.ReadVariable(True)
                    Dim test As Boolean = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
                    Dim str As String = Var.Value.ToString()

                    If str.Length < Count Then
                        Var.Value = str
                    Else
                        Var.Value = str.Substring(0, str.Length - Count)
                    End If

                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString
                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(5:122) chop off the beginning of variable %variable, removing the first # characters of it.
            Public Function ChopStartString(reader As TriggerReader) As Boolean
                Dim Var As Variable
                Dim Count As Integer = 0
                Try
                    Var = reader.ReadVariable(True)
                    Dim test As Boolean = Integer.TryParse(ReadVariableOrNumber(reader).ToString, Count)
                    Dim str As String = Var.Value.ToString()
                    If str.Length < Count Then
                        Var.Value = Nothing
                    Else
                        Var.Value = str.Substring(Count)
                    End If
                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try

            End Function

            '(5:126) count the number of characters in string variable %variable and put them into variable %Variable .
            Public Function CountChars(reader As TriggerReader) As Boolean

                Try

                    Dim var1 As Variable = reader.ReadVariable()
                    Dim var2 As Variable = reader.ReadVariable(True)
                    Dim Count As Double = Convert.ToDouble(var1.Value.ToString.Length)
                    var2.Value = Count
                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            Public Function MatchWildcardString(pattern As String, input As String) As [Boolean]
                If [String].Compare(pattern, input) = 0 Then
                    Return True
                ElseIf [String].IsNullOrEmpty(input) Then
                    If [String].IsNullOrEmpty(pattern.Trim(New [Char](0) {"*"c})) Then
                        Return True
                    Else
                        Return False
                    End If
                ElseIf pattern.Length = 0 Then
                    Return False
                ElseIf pattern(0) = "?"c Then
                    Return MatchWildcardString(pattern.Substring(1), input.Substring(1))
                ElseIf pattern(pattern.Length - 1) = "?"c Then
                    Return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1))
                ElseIf pattern(0) = "*"c Then
                    If MatchWildcardString(pattern.Substring(1), input) Then
                        Return True
                    Else
                        Return MatchWildcardString(pattern, input.Substring(1))
                    End If
                ElseIf pattern(pattern.Length - 1) = "*"c Then
                    If MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input) Then
                        Return True
                    Else
                        Return MatchWildcardString(pattern, input.Substring(0, input.Length - 1))
                    End If
                ElseIf pattern(0) = input(0) Then
                    Return MatchWildcardString(pattern.Substring(1), input.Substring(1))
                End If
                Return False
            End Function

            '(1:61) and variable %Variable doesn't match wildcard expression {.} ( ""*"" or ""?""),
            Function NotWildCard(reader As TriggerReader) As Boolean
                Try
                    Dim var As Variable = reader.ReadVariable
                    Dim Pattern As String = reader.ReadString
                    Return Not MatchWildcardString(Pattern, var.Value.ToString)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(5:120) use variable % and take word # and put it into variable %
            Function StringSplit(reader As TriggerReader) As Boolean
                Try
                    Dim Var As Variable = reader.ReadVariable()
                    Dim i As Double = ReadVariableOrNumber(reader)
                    Dim NewVar As Variable = reader.ReadVariable(True)
                    Dim fields() As String = Split(Var.Value.ToString, " ")
                    If i < fields.Length Then
                        NewVar.Value = fields(CInt(i))
                    End If
                    Return True
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

            '(5:121) use variable % then remove character {.} and put it into variable %.
            Public Function StripCharacters(reader As TriggerReader) As Boolean
                Dim ch As Char = Nothing
                Dim NewVar As Variable
                Dim Var As Variable
                Try
                    Var = reader.ReadVariable()
                    ch = CChar(reader.ReadString)
                    NewVar = reader.ReadVariable()
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
                Dim varStr As String = Var.Value.ToString
                Dim NewStr As String = varStr.Replace(ch, String.Empty)
                NewVar.Value = NewStr
                Return True
            End Function

            '(1:60) and variable %Variable matches wildcard expression {.} ( ""*"" or ""?""),
            Function WildCard(reader As TriggerReader) As Boolean
                Try
                    Dim var As Variable = reader.ReadVariable
                    Dim Pattern As String = reader.ReadString
                    Return MatchWildcardString(Pattern, var.Value.ToString)
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

#End Region

#Region "Private Methods"

            Private Function ToShortName(reader As TriggerReader) As Boolean
                Try
                    If reader.PeekVariable Then
                        Dim var As Variable = reader.ReadVariable
                        If String.IsNullOrEmpty(var.Value.ToString) Then
                            Return True
                        End If
                        var.Value = FurcadiaShortName(var.Value.ToString)
                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Dim tID As String = reader.TriggerId.ToString
                    Dim tCat As String = reader.TriggerCategory.ToString

                    Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message

                    Debug.Print(ErrorString)
                    Return False
                End Try
            End Function

#End Region

        End Class

    End Module
End Namespace