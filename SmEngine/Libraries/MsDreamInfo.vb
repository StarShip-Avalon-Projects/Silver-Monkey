Imports Monkeyspeak
Imports Monkeyspeak.Logging

Namespace Engine.Libraries

    ''' <summary>
    ''' Dream Information triggers
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates:
    ''' <para/>
    ''' (0:90) When the bot enters a Dream,
    ''' <para/>
    ''' (0:93) When the bot leaves the Dream named {..},
    ''' <para/>
    ''' (0:22) When someone emits {..},
    ''' </remarks>
    Public NotInheritable Class MsDreamInfo
        Inherits MonkeySpeakLibrary

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
        End Sub

        Public Overrides Sub Initialize(ParamArray args() As Object)
            'Emits
            Add(TriggerCategory.Cause, 21,
                 Function(reader)
                     ReadParams(reader)
                     Return Not FurcadiaSession.IsConnectedCharacter(Player)
                 End Function, " When someone emits something,")

            Add(TriggerCategory.Cause, 22,
                     AddressOf MsgIs, " When someone emits {..},")

            '(0:13) When some one emotes something with {..} in it
            Add(TriggerCategory.Cause, 23,
                 AddressOf MsgContains, " When someone emits something with {..} in it,")

            '(0:90) When the bot enters a Dream,
            Add(TriggerCategory.Cause, 90, AddressOf ReadParams, " When the bot enters a Dream,")
            '(0:91) When the bot enters a Dream named {..},
            Add(TriggerCategory.Cause, 91,
                AddressOf DreamNameIs, " When the bot enters the Dream named {..},")
            '(0:92) When the bot leaves a Dream,
            Add(TriggerCategory.Cause, 97, AddressOf ReadParams, " When the bot leaves a Dream,")
            '(0:93) When the bot leaves the Dream named {..},
            Add(TriggerCategory.Cause, 98,
                AddressOf DreamNameIs, " When the bot leaves the Dream named {..},")

            '(1:19) and the bot is the Dream owner,
            Add(TriggerCategory.Condition, 19, AddressOf BotIsDreamOwner,
                " and the bot is the Dream owner,")

            '(1:20) and the bot is not the Dream-Owner,
            Add(TriggerCategory.Condition, 20,
                Function(reader) Not BotIsDreamOwner(reader),
                " and the bot is not the Dream-Owner,")

            '(1:21) and the furre named {..} is the Dream owner,
            Add(TriggerCategory.Condition, 21,
                Function(reader) Dream.OwnerShortName =
                reader.ReadString.ToFurcadiaShortName(),
                " and the furre named {..} is the Dream owner,")

            '(1:22) and the furre named {..} is not the Dream owner,
            Add(TriggerCategory.Condition, 22,
               Function(reader) Dream.OwnerShortName <>
              reader.ReadString.ToFurcadiaShortName(),
                " and the furre named {..} is not the Dream owner,")
            '(1:23) and the Dream Name is {..},

            Add(TriggerCategory.Condition, 23, AddressOf DreamNameIs,
                " and the Dream Name is {..},")
            '(1:24) and the Dream Name is not {..},

            Add(TriggerCategory.Condition, 24, AddressOf DreamNameIsNot,
                " and the Dream Name is not {..},")

            '(1:25) and the triggering furre is the Dream owner
            Add(TriggerCategory.Condition, 25, AddressOf TriggeringFurreIsDreamOwner,
                " and the triggering furre is the Dream owner,")

            '(1:26) and the triggering furre is not the Dream owner
            Add(TriggerCategory.Condition, 26, AddressOf TriggeringFurreIsNotDreamOwner,
                " and the triggering furre is not the Dream owner,")

            '(1:27) and the bot has share control of the Dream or is the Dream owner,
            Add(TriggerCategory.Condition, 27,
                Function(reader)
                    If FurcadiaSession.HasShare OrElse Dream.OwnerShortName = FurcadiaSession.ConnectedFurre.ShortName Then
                        Return True
                    End If
                    Return False
                End Function, " and the bot has share control of the Dream or is the Dream owner,")

            '(1:28) and the bot has share control of the Dream,
            Add(TriggerCategory.Condition, 28,
                 Function()
                     Return FurcadiaSession.HasShare
                 End Function, " and the bot has share control of the Dream,")

            '(1:29) and the bot doesn't have share control in the Dream,
            Add(TriggerCategory.Condition, 29,
                 Function()
                     Return Not FurcadiaSession.HasShare
                 End Function, " and the bot doesn't have share control in the Dream,")

            '(5:20) give share control to the triggering furre.
            Add(TriggerCategory.Effect, 20, AddressOf ShareTrigFurre,
                " give share control to the triggering furre.")
            '(5:21) remove share control from the triggering furre.
            Add(TriggerCategory.Effect, 21, AddressOf UnshareTrigFurre,
                " remove share control from the triggering furre.")
            '(5:22) remove share from the furre named {..} if they're in the Dream right now.
            Add(TriggerCategory.Effect, 22, AddressOf UnshareFurreNamed,
                " remove share from the furre named {..} if they're in the Dream right now.")

            '(5:23) give share to the furre named {..} if they're in the Dream right now.
            Add(TriggerCategory.Effect, 23, AddressOf ShareFurreNamed,
                " give share to the furre named {..} if they're in the Dream right now.")

        End Sub

        ''' <summary>
        ''' (1:19) and the bot is the Dream owner,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function BotIsDreamOwner(reader As TriggerReader) As Boolean
            Return Dream.ShortName = FurcadiaSession.ConnectedFurre.ShortName
        End Function

        ''' <summary>
        ''' (1:23) and the Dream Name is {..},
        ''' <para>
        ''' (0:91) When the bot enters a Dream named {..},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function DreamNameIs(reader As TriggerReader) As Boolean
            ReadParams(reader)
            Return Dream.ShortName = reader.ReadString.ToFurcadiaShortName()

        End Function

        ''' <summary>
        ''' (5:22) remove share from the furre named {..} if they're in the
        ''' Dream right now.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function ShareFurreNamed(reader As TriggerReader) As Boolean

            Dim Target = Dream.Furres.GerFurreByName(reader.ReadString())
            If InDream(Target) Then
                If InDream(Target) Then SendServer("share " + Target.ShortName)
            Else
                Logger.Info(Of MsDreamInfo)($"{Target.Name} Is Not in the dream")

            End If
            Return True

        End Function

        ''' <summary>
        ''' (1:26) and the triggering furre is not the Dream owner,"
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreIsNotDreamOwner(reader As TriggerReader) As Boolean
            Return Not TriggeringFurreIsDreamOwner(reader)
        End Function

        ''' <summary>
        ''' (5:23) give share to the furre named {..} if they're in the
        ''' Dream right now.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function UnshareFurreNamed(reader As TriggerReader) As Boolean

            Dim Target = Dream.Furres.GerFurreByName(reader.ReadString())
            If InDream(Target) Then
                Return SendServer("unshare " + Target.ShortName)
            Else
                Logger.Info(Of MsDreamInfo)($"{Target.Name} Is Not in the dream")
            End If
            Return False

        End Function

        ''' <summary>
        ''' (0:25) When a furre Named {.} enters the Dream,
        ''' <para>
        ''' (0:27) When a furre named {.} leaves the Dream
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
        ''' (5:21) remove share control from the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function UnshareTrigFurre(reader As TriggerReader) As Boolean
            SendServer("unshare " + Player.ShortName)
            Return True
        End Function

        ''' <summary>
        ''' (5:20) give share control to the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function ShareTrigFurre(reader As TriggerReader) As Boolean

            Return SendServer("share " + Player.ShortName)

        End Function

        ''' <summary>
        ''' (1:24) and the Dream Name is not {..},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function DreamNameIsNot(reader As TriggerReader) As Boolean
            Return Not DreamNameIs(reader)
        End Function

        ''' <summary>
        ''' (1:22:) and the triggering furre is the Dream owner
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreIsDreamOwner(reader As TriggerReader) As Boolean

            Return Player.ShortName = Dream.OwnerShortName

        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

    End Class

End Namespace