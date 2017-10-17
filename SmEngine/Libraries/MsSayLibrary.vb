Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Util
Imports Monkeyspeak
Imports SilverMonkeyEngine.SmConstants

Namespace Engine.Libraries

    ''' <summary>
    ''' Legacy Furcadia channel processing
    ''' <para>
    ''' This lib handles the basic channels, Emote, Say (Speech and Spoken
    ''' furcadia commands), Whispers
    ''' </para>
    ''' <pra>Bot Testers: Be aware this class needs to be tested any way possible!</pra>
    ''' <para>
    ''' TODO: Upgrade to AngelCat style Channels and Reintegrate into the
    '''       engine. These channels may still work with the existing
    '''       system.
    '''       <see href="http://bugtraq.tsprojects.org/view.php?id=107">[BUG: 107]</see>
    ''' </para>
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates:
    ''' <para>
    ''' (0:1) When the bot logs into furcadia,
    ''' </para>
    ''' <para>
    ''' (0:2) When the bot logs out of furcadia,
    ''' </para>
    ''' <para>
    ''' (0:3) When the Furcadia client disconnects or closes,"
    ''' </para>
    ''' <para>
    ''' (0:5) When someone says something,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:8) When someone shouts something,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:11) When someone emotes something,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:15) When someone whispers something,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:18) When someone says or emotes something,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:21) When someone emits something,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:24) When someone enters the Dream,
    ''' </para>
    ''' <para>
    ''' (0:26) When someone leaves the Dream,
    ''' </para>
    ''' <para>
    ''' (0:32) When someone requests to summon the bot,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:34) When someone requests to join the bot,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:36) When someone requests to follow the bot,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:38) When someone requests to lead the bot,
    ''' <para>Ignores Bots speech</para>
    ''' </para>
    ''' <para>
    ''' (0:40) When someone requests to cuddle with the bot,
    ''' <para>Ignores Bots speech</para>
    ''' (0:92) When the bot detects the "Your throat is tired. Please wait a
    ''' few seconds" message,
    ''' </para>
    ''' <para>
    ''' (0:93) When the bot resumes processing after seeing ""Your throat is
    ''' tired"" message,
    ''' </para>
    ''' <para>
    ''' (5:0) say {..}. (Normal Furcadia Text commands)
    ''' </para>
    ''' <para>
    ''' (5:1) emote {..}.
    ''' </para>
    ''' <para>
    ''' (5:2) shout {..}.
    ''' </para>
    ''' <para>
    ''' (5:3) Emit {..}.
    ''' </para>
    ''' <para>
    ''' (5:4) Emitloud {..}.
    ''' </para>
    ''' <para>
    ''' (5:5) whisper {..} to the triggering furre.
    ''' </para>
    ''' <para>
    ''' (5:6) whisper {..} to furre named {..}.
    ''' </para>
    ''' <para>
    ''' (5:7) whisper {..} to furre named {..} even if they're off-line.
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class MsSayLibrary
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
                 AddressOf MsgIs, "(0:6) When someone says {..},")

            '(0:7) When some one says something with {..} in it
            Add(TriggerCategory.Cause, 7,
                AddressOf MsgContains, "(0:7) When someone says something with {..} in it,")

            'Shouts
            Add(TriggerCategory.Cause, 8,
                 Function()
                     Return Not FurcadiaSession.IsConnectedCharacter
                 End Function,
             "(0:8) When someone shouts something,")

            Add(TriggerCategory.Cause, 9,
             AddressOf MsgIs, "(0:9) When someone shouts {..},")

            '(0:10) When some one shouts something with {..} in it
            Add(TriggerCategory.Cause, 10,
             AddressOf MsgContains, "(0:10) When someone shouts something with {..} in it,")

            'emotes
            Add(TriggerCategory.Cause, 11,
                 Function()
                     Return Not FurcadiaSession.IsConnectedCharacter

                 End Function,
                  "(0:11) When someone emotes something,")
            Add(TriggerCategory.Cause, 12,
                 AddressOf MsgIs, "(0:12) When someone emotes {..},")

            '(0:13) When some one emotes something with {..} in it
            Add(TriggerCategory.Cause, 13,
                AddressOf MsgContains, "(0:13) When someone emotes something with {..} in it,")

            'Whispers
            Add(TriggerCategory.Cause, 15,
                 Function()
                     Return Not FurcadiaSession.IsConnectedCharacter
                 End Function,
                  "(0:15) When someone whispers something,")

            Add(TriggerCategory.Cause, 16,
                AddressOf MsgIs, "(0:16) When someone whispers {..},")

            '(0:13) When some one emotes something with {..} in it
            Add(TriggerCategory.Cause, 17,
                 AddressOf MsgContains, "(0:17) When someone whispers something with {..} in it,")

            'Says or Emotes
            Add(TriggerCategory.Cause, 18,
                 Function()
                     Return Not FurcadiaSession.IsConnectedCharacter
                 End Function, "(0:18) When someone says or emotes something,")

            Add(TriggerCategory.Cause, 19,
                AddressOf MsgIs, "(0:19) When someone says or emotes {..},")

            '(0:13) When some one emotes something with {..} in it
            Add(TriggerCategory.Cause, 20,
                AddressOf MsgContains, "(0:20) When someone says or emotes something with {..} in it,")

            'Emits
            Add(TriggerCategory.Cause, 21,
                 Function()
                     Return Not FurcadiaSession.IsConnectedCharacter
                 End Function, "(0:21) When someone emits something,")

            Add(TriggerCategory.Cause, 22,
                     AddressOf MsgIs, "(0:22) When someone emits {..},")

            '(0:13) When some one emotes something with {..} in it
            Add(TriggerCategory.Cause, 23,
                 AddressOf MsgContains, "(0:23) When someone emits something with {..} in it,")

            'Furre Enters
            '(0:4) When someone is added to the Dream manifest,
            Add(TriggerCategory.Cause, 24,
                Function()
                    Return True
                End Function, "(0:24) When someone enters the Dream,")

            '(0:25) When a furre Named {..} enters the Dream,
            Add(TriggerCategory.Cause, 25,
                AddressOf NameIs, "(0:25) When a furre Named {..} enters the Dream,")

            'Furre Leaves
            '(0:25) When someone leaves the FurcadiaSession.Dream,
            Add(TriggerCategory.Cause, 26,
                Function()
                    Return True
                End Function, "(0:26) When someone leaves the Dream,")

            '(0:27) When a furre named {..} leaves the FurcadiaSession.Dream,
            Add(TriggerCategory.Cause, 27,
                AddressOf NameIs, "(0:27) When a furre named {..} leaves the Dream,")

            'Furre In View
            'TODO: Move to Movement?
            '(0:28) When someone enters the bots view,
            Add(TriggerCategory.Cause, 28,
                AddressOf EnterView, "(0:28) When someone enters the bots view, ")

            '(0:28) When a furre named {..} enters the bots view
            Add(TriggerCategory.Cause, 29,
             AddressOf FurreNamedEnterView, "(0:29) When a furre named {..} enters the bots view,")

            'Furre Leave View
            '(0:30) When someone leaves the bots view,
            Add(TriggerCategory.Cause, 30,
                 AddressOf LeaveView, "(0:30) When someone leaves the bots view, ")

            '(0:31) When a furre named {..} leaves the bots view
            Add(TriggerCategory.Cause, 31,
            AddressOf FurreNamedLeaveView, "(0:31) When a furre named {..} leaves the bots view,")

            'Summon
            '(0:32) When someone requests to summon the bot,
            Add(TriggerCategory.Cause, 32,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:32) When someone requests to summon the bot,")

            '(0:33) When a furre named {..} requests to summon the bot,
            Add(TriggerCategory.Cause, 33,
                AddressOf NameIs, "(0:33) When a furre named {..} requests to summon the bot,")

            'Join
            '(0:34) When someone requests to join the bot,
            Add(TriggerCategory.Cause, 34,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:34) When someone requests to join the bot,")

            '(0:35) When a furre named {..} requests to join the bot,
            Add(TriggerCategory.Cause, 35,
                 AddressOf NameIs, "(0:35) When a furre named {..} requests to join the bot,")

            'Follow
            '(0:36) When someone requests to follow the bot,
            Add(TriggerCategory.Cause, 36,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:36) When someone requests to follow the bot,")

            '(0:37) When a furre named {..} requests to follow the bot,
            Add(TriggerCategory.Cause, 37,
            AddressOf NameIs, "(0:37) When a furre named {..} requests to follow the bot,")

            'Lead
            '(0:38) When someone requests to lead the bot,
            Add(TriggerCategory.Cause, 38,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:38) When someone requests to lead the bot,")

            '(0:39) When a furre named {..} requests to lead the bot,
            Add(TriggerCategory.Cause, 39,
            AddressOf NameIs, "(0:39) When a furre named {..} requests to lead the bot,")

            'Cuddle
            '(0:40) When someone requests to cuddle with the bot.
            Add(TriggerCategory.Cause, 40,
                Function()
                    Return Not FurcadiaSession.IsConnectedCharacter
                End Function, "(0:40) When someone requests to cuddle with the bot,")

            '(0:41) When a furre named {..} requests to cuddle with the bot,
            Add(TriggerCategory.Cause, 41,
            AddressOf NameIs, "(0:41) When a furre named {..} requests to cuddle with the bot,")

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

            ' (1:3) and the triggering furre's name is {..},
            Add(New Trigger(TriggerCategory.Condition, 5), AddressOf NameIs,
                "(1:5) and the triggering furre's name is {..},")

            ' (1:4) and the triggering furre's name is not {..},
            Add(New Trigger(TriggerCategory.Condition, 6), AddressOf NameIsNot,
                "(1:6) and the triggering furre's name is not {..},")

            ' (1:5) and the Triggering Furre's message is {..}, (say, emote,
            ' shot, whisper, or emit Channels)
            Add(New Trigger(TriggerCategory.Condition, 7), AddressOf MsgIs,
                "(1:7) and the triggering furre's message is {..},")

            ' (1:8) and the triggering furre's message contains {..} in it,
            ' (say, emote, shot, whisper, or emit Channels)
            Add(New Trigger(TriggerCategory.Condition, 8), AddressOf MsgContains,
                "(1:8) and the triggering furre's message contains {..} in it,")

            '(1:9) and the triggering furre's message does not contain {..} in it,
            '(say, emote, shot, whisper, or emit Channels)
            Add(New Trigger(TriggerCategory.Condition, 9), AddressOf MsgNotContain,
                "(1:9) and the triggering furre's message does not contain {..} in it,")

            '(1:10) and the triggering furre's message is not {..},
            '(say, emote, shot, whisper, or emit Channels)
            Add(New Trigger(TriggerCategory.Condition, 10), AddressOf MsgIsNot,
                "(1:10) and the triggering furre's message is not {..},")

            '(1:11) and triggering furre's message starts with {..},
            Add(New Trigger(TriggerCategory.Condition, 11), AddressOf MsgStartsWith,
                "(1:11) and triggering furre's message starts with {..},")

            '(1:12) and triggering furre's message doesn't start with {..},
            Add(New Trigger(TriggerCategory.Condition, 12), AddressOf MsgNotStartsWith,
                "(1:12) and triggering furre's message doesn't start with {..},")

            '(1:13) and triggering furre's message  ends with {..},
            Add(New Trigger(TriggerCategory.Condition, 13), AddressOf MsgEndsWith,
                "(1:13) and triggering furre's message  ends with {..},")

            '(1:14) and triggering furre's message doesn't end with {..},
            Add(New Trigger(TriggerCategory.Condition, 14), AddressOf MsgNotEndsWith,
                "(1:14) and triggering furre's message doesn't end with {..},")

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

            'Says
            ' (5:0) say {..}.
            Add(New Trigger(TriggerCategory.Effect, 0),
                Function(reader As TriggerReader)

                    Dim msg As String = reader.ReadString(True)
                    Return SendSay(msg)

                End Function,
         "(5:0) say {..}.")
            'emotes
            ' (5:1) emote {..}.
            Add(New Trigger(TriggerCategory.Effect, 1),
                Function(reader As TriggerReader)

                    Dim msg As String = reader.ReadString
                    SendEmote(msg)
                    Return True

                End Function,
                 "(5:1) emote {..}.")

            'Shouts
            ' (5:2) shout {..}.
            Add(New Trigger(TriggerCategory.Effect, 2),
                 Function(reader As TriggerReader)

                     Dim msg = reader.ReadString
                     SndShout(msg)
                     Return True

                 End Function,
                "(5:2) shout {..}.")

            'Emits
            ' (5:3) emit {..}.
            Add(New Trigger(TriggerCategory.Effect, 3),
                Function(reader As TriggerReader)

                    Dim msg = reader.ReadString
                    SendEmit(msg)
                    Return True

                End Function,
                 "(5:3) Emit {..}.")
            ' (5:4) emitloud {..}.
            Add(New Trigger(TriggerCategory.Effect, 4),
                 Function(reader As TriggerReader)

                     Dim msg = reader.ReadString
                     SendEmitLoud(msg)
                     Return True

                 End Function,
                  "(5:4) Emitloud {..}.")

            'Whispers
            ' (5:5) whisper {..} to the triggering furre.
            Add(New Trigger(TriggerCategory.Effect, 5),
                   Function(reader As TriggerReader)

                       Dim msg = reader.ReadString
                       Dim tname = reader.Page.GetVariable(MS_Name)
                       SndWhisper(tname.Value.ToString, msg)
                       Return True

                   End Function,
                  "(5:5) whisper {..} to the triggering furre.")

            ' (5:6) whisper {..} to {..}.
            Add(New Trigger(TriggerCategory.Effect, 6),
                 Function(reader As TriggerReader)

                     Dim msg = reader.ReadString(True)
                     Dim tname = reader.ReadString
                     SndWhisper(tname, msg)
                     Return True

                 End Function,
                         "(5:6) whisper {..} to furre named {..}.")

            ' (5:7) whisper {..} to {..} even if they're off-line.
            Add(New Trigger(TriggerCategory.Effect, 7),
                  Function(reader As TriggerReader)

                      Dim msg = reader.ReadString
                      Dim tname = reader.ReadString

                      SendOffLineWhisper(tname, msg)
                      Return True

                  End Function,
                      "(5:7) whisper {..} to furre named {..} even if they're off-line.")

            '(5:40) Switch the bot to stand alone mode and close the Furcadia client.
            Add(New Trigger(TriggerCategory.Effect, 40), AddressOf StandAloneMode,
                "(5:40) Switch the bot to stand alone mode and close the Furcadia client.")

            '(5:41) Disconnect the bot from the Furcadia game server.
            Add(New Trigger(TriggerCategory.Effect, 41), AddressOf FurcadiaDisconnect,
                "(5:41) Disconnect the bot from the Furcadia game server.")

            '(5:42) start a new instance to Silver Monkey with botfile {..}.
            Add(New Trigger(TriggerCategory.Effect, 42), AddressOf StartNewBot,
            "(5:42) start a new instance to Silver Monkey with bot-file {..}.")

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (0:28) When someone enters the bots view,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function EnterView(reader As TriggerReader) As Boolean

            Dim tPlayer = Player
            If tPlayer.Visible = tPlayer.WasVisible Then
                Return False
            End If
            Return tPlayer.Visible = True

        End Function

        ''' <summary>
        ''' (5:41) Disconnect the bot from the Furcadia game server.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurcadiaDisconnect(reader As TriggerReader) As Boolean

            FurcadiaSession.Disconnect()

            Return Not FurcadiaSession.IsServerConnected
        End Function

        ''' <summary>
        ''' (0:29) When a furre named {..} enters the bots view,"
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedEnterView(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim tPlayer = Dream.FurreList.GerFurreByName(name)
            If tPlayer.Visible = tPlayer.WasVisible Then
                Return False
            End If
            Return tPlayer.Visible = True

        End Function

        ''' <summary>
        ''' (0:31) When a furre named {..} leaves the bots view,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedLeaveView(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim tPlayer = Dream.FurreList.GerFurreByName(name)
            If tPlayer.Visible = tPlayer.WasVisible Then
                Return False
            End If
            Return tPlayer.Visible = False

        End Function

        ''' <summary>
        ''' (0:30) When someone leaves the bots view,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function LeaveView(reader As TriggerReader) As Boolean

            Dim tPlayer = Player
            If tPlayer.Visible = tPlayer.WasVisible Then
                Return False
            End If
            Return tPlayer.Visible = False
        End Function

        ''' <summary>
        ''' send a local emit to the server queue
        ''' </summary>
        ''' <param name="msg">
        ''' message to send
        ''' </param>
        Public Sub SendEmit(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then SendServer("emit " & msg)
        End Sub

        ''' <summary>
        ''' send an emitloud command to the server queue
        ''' </summary>
        ''' <param name="msg">
        ''' message to send
        ''' </param>
        Public Sub SendEmitLoud(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then SendServer("emitloud " & msg)
        End Sub

        ''' <summary>
        ''' Send an emote to the server queue
        ''' </summary>
        ''' <param name="msg">
        ''' message to send
        ''' </param>
        Public Sub SendEmote(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then SendServer(":" & msg)
        End Sub

        ''' <summary>
        ''' Send an off line whisper to the server queue
        ''' </summary>
        ''' <param name="name">
        ''' recipients name
        ''' </param>
        ''' <param name="msg">
        ''' Message to send
        ''' </param>
        Public Sub SendOffLineWhisper(ByRef name As String, ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then SendServer("/%%" & FurcadiaShortName(name) & " " & msg)
        End Sub

        ''' <summary>
        ''' send a speech command to the server queue
        ''' </summary>
        ''' <param name="msg">
        ''' message to send
        ''' </param>
        Public Function SendSay(ByRef msg As String) As Boolean

            Return SendServer(msg)

        End Function

        ''' <summary>
        ''' Send a shout to the server queue
        ''' </summary>
        ''' <param name="msg">
        ''' Message to send
        ''' </param>
        Public Sub SndShout(ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then SendServer("-" & msg)
        End Sub

        ''' <summary>
        ''' Send a whisper to the server queue
        ''' </summary>
        ''' <param name="name">
        ''' recipients name
        ''' </param>
        ''' <param name="msg">
        ''' Message to send
        ''' </param>
        Public Sub SndWhisper(ByRef name As String, ByRef msg As String)
            If Not String.IsNullOrEmpty(msg) Then SendServer("/%" & FurcadiaShortName(name) & " " & msg)
        End Sub

        ''' <summary>
        ''' (5:40) Switch the bot to stand alone mode and close the Furcadia client.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function StandAloneMode(reader As TriggerReader) As Boolean

            FurcadiaSession.StandAlone = True
            FurcadiaSession.CloseClient()

            Return True
        End Function

        ''' <summary>
        ''' (5:42) start a new instance to Silver Monkey with bot-file {..}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function StartNewBot(reader As TriggerReader) As Boolean

            'Dim ps As Process = New Process()
            Dim File = reader.ReadString
            Dim p As New ProcessStartInfo With {
                .Arguments = File,
                .FileName = "SilverMonkey.exe",
                .WorkingDirectory = MonkeyCore.Paths.ApplicationPath
            }
            Process.Start(p)
            Return True
        End Function

        ''' <summary>
        ''' (0:17) When someone whispers something with {..} in it,
        ''' <para>(0:10) When someone shouts something with {..} in it,</para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function MsgContains(reader As TriggerReader) As Boolean
            Return MyBase.MsgContains(reader)
        End Function

        ''' <summary>
        ''' (1:13) and triggering furre's message ends with {..},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function MsgEndsWith(reader As TriggerReader) As Boolean
            Return MyBase.MsgEndsWith(reader)
        End Function

        ''' <summary>
        ''' 0:6) When someone says {..},
        ''' <para>
        ''' (0:9) When someone shouts {..},
        ''' </para>
        ''' <para>
        ''' (0:12) When someone emotes {..},
        ''' </para>
        ''' <para>
        ''' (0:16) When someone whispers {..},"
        ''' </para>
        ''' <para>
        ''' (0:19) When someone says or emotes {..},
        ''' </para>
        ''' <para>
        ''' (0:22) When someone emits {..},
        ''' </para>
        ''' <para>
        ''' (1:7) and the triggering furre's message is {..},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function MsgIs(reader As TriggerReader) As Boolean
            Return MyBase.MsgIs(reader)
        End Function

        ''' <summary>
        ''' (1:10) and the triggering furre's message is not {..},"
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function MsgIsNot(reader As TriggerReader) As Boolean
            Return MyBase.MsgIsNot(reader)
        End Function

        ''' <summary>
        ''' (1:9) and the triggering furre's message does not contain {..}
        ''' in it,"
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function MsgNotContain(reader As TriggerReader) As Boolean
            Return MyBase.MsgNotContain(reader)
        End Function

        ''' <summary>
        ''' (0:33) When a furre named {.} requests to summon the bot,"
        ''' <para>
        ''' (0:35) When a furre named {.} requests to join the bot,
        ''' </para>
        ''' <para>
        ''' (0:37) When a furre named {.} requests to follow the bot,
        ''' </para>
        ''' <para>
        ''' (0:39) When a furre named {.} requests to lead the bot,
        ''' </para>
        ''' <para>
        ''' (0:41) When a furre named {.} requests to cuddle with the bot,
        ''' </para>
        ''' <para>
        ''' (1:5) and the triggering furre's name is {.},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function NameIs(reader As TriggerReader) As Boolean
            Return MyBase.NameIs(reader)
        End Function

        ''' <summary>
        ''' (1:6) and the triggering furre's name is not {..},"
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Protected Overrides Function NameIsNot(reader As TriggerReader) As Boolean
            Return MyBase.NameIsNot(reader)
        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

    End Class

End Namespace