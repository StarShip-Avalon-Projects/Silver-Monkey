Imports Furcadia.Net
Imports Furcadia.Net.Dream
Imports Furcadia.Net.Utils.ServerParser
Imports Furcadia.Util
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Dream Information triggers
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates:
    ''' <para>
    ''' (0:90) When the bot enters a Dream,
    ''' </para>
    ''' (0:93) When the bot leaves the Dream named {..},
    ''' </remarks>
    Public NotInheritable Class MsDreamInfo
        Inherits MonkeySpeakLibrary

        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)

            '(0:90) When the bot enters a Dream,
            Add(TriggerCategory.Cause, 90,
                Function()
                    Return True
                End Function, " When the bot enters a Dream,")
            '(0:91) When the bot enters a Dream named {..},
            Add(TriggerCategory.Cause, 91,
                AddressOf DreamNameIs, " When the bot enters the Dream named {..},")
            '(0:92) When the bot leaves a Dream,
            Add(TriggerCategory.Cause, 97,
                Function()
                    Return True
                End Function, " When the bot leaves a Dream,")
            '(0:93) When the bot leaves the Dream named {..},
            Add(TriggerCategory.Cause, 98,
                AddressOf DreamNameIs, " When the bot leaves the Dream named {..},")

            '(1:19) and the bot is the Dream owner,
            Add(New Trigger(TriggerCategory.Condition, 19), AddressOf BotIsDreamOwner,
                " and the bot is the Dream owner,")

            '(1:20) and the bot is not the Dream-Owner,
            Add(New Trigger(TriggerCategory.Condition, 20), AddressOf BotIsNotDreamOwner,
                " and the bot is not the Dream-Owner,")

            '(1:21) and the furre named {..} is the Dream owner,
            Add(New Trigger(TriggerCategory.Condition, 21), AddressOf FurreNamedIsDREAMOWNER,
                " and the furre named {..} is the Dream owner,")

            '(1:22) and the furre named {..} is not the Dream owner,
            Add(New Trigger(TriggerCategory.Condition, 22), AddressOf FurreNamedIsNotDREAMOWNER,
                " and the furre named {..} is not the Dream owner,")
            '(1:23) and the Dream Name is {..},

            Add(New Trigger(TriggerCategory.Condition, 23), AddressOf DreamNameIs,
                " and the Dream Name is {..},")
            '(1:24) and the Dream Name is not {..},

            Add(New Trigger(TriggerCategory.Condition, 24), AddressOf DreamNameIsNot,
                " and the Dream Name is not {..},")

            '(1:25) and the triggering furre is the FurcadiaSession.Dream owner
            Add(New Trigger(TriggerCategory.Condition, 25), AddressOf TriggeringFurreIsDreamOwner,
                " and the triggering furre is the Dream owner,")

            '(1:26) and the triggering furre is not the FurcadiaSession.Dream owner
            Add(New Trigger(TriggerCategory.Condition, 26), AddressOf TriggeringFurreIsNotDreamOwner,
                " and the triggering furre is not the Dream owner,")

            '(1:27) and the bot has share control of the Dream or is the Dream owner,
            Add(TriggerCategory.Condition, 27,
                Function(reader As TriggerReader)
                    Dim tname = reader.Page.GetVariable("%DREAMOWNER")
                    If FurcadiaSession.HasShare OrElse FurcadiaSession.Dream.Owner = FurcadiaSession.ConnectedFurre.ShortName Then
                        Return True
                    End If
                    Return False
                End Function, " and the bot has share control of the Dream or is the Dream owner,")

            '(1:28) and the bot has share control of the Dream,
            Add(New Trigger(TriggerCategory.Condition, 28),
                 Function()
                     Return FurcadiaSession.HasShare
                 End Function, " and the bot has share control of the Dream,")

            '(1:29) and the bot doesn't have share control in the Dream,
            Add(New Trigger(TriggerCategory.Condition, 29),
                 Function()
                     Return Not FurcadiaSession.HasShare
                 End Function, " and the bot doesn't have share control in the Dream,")

            '(5:20) give share control to the triggering furre.
            Add(New Trigger(TriggerCategory.Effect, 20), AddressOf ShareTrigFurre,
                " give share control to the triggering furre.")
            '(5:21) remove share control from the triggering furre.
            Add(New Trigger(TriggerCategory.Effect, 21), AddressOf UnshareTrigFurre,
                " remove share control from the triggering furre.")
            '(5:22) remove share from the furre named {..} if they're in the Dream right now.
            Add(New Trigger(TriggerCategory.Effect, 22), AddressOf ShareFurreNamed,
                " remove share from the furre named {..} if they're in the Dream right now.")

            '(5:23) give share to the furre named {..} if they're in the Dream right now.
            Add(New Trigger(TriggerCategory.Effect, 23), AddressOf UnshareFurreNamed,
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
        ''' (1:20) and the bot is not the Dream-Owner,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function BotIsNotDreamOwner(reader As TriggerReader) As Boolean
            Return Not BotIsDreamOwner(reader)
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
            Dim DreamName As String = reader.ReadString
            DreamName = DreamName.ToLower.Replace("furc://", String.Empty)
            Dim DreamNameVariable = reader.Page.GetVariable("%DREAMNAME")
            'add Machine Name parser
            If DreamNameVariable.Value.ToString() <> Dream.Name Then
                Throw New MonkeyspeakException("%%DREAMNAME does not match Dream.Name")
            End If

            Return Dream.ShortName = FurcadiaShortName(DreamName)

        End Function

        ''' <summary>
        ''' (1:21) and the furre named {..} is the Dream owner,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedIsDREAMOWNER(reader As TriggerReader) As Boolean
            Dim Name = FurcadiaShortName(reader.ReadString)
            Return FurcadiaShortName(FurcadiaSession.Dream.Owner) = Name

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

            Dim Target = Dream.FurreList.GerFurreByName(reader.ReadString)
            If InDream(Target.Name) Then SendServer("share " + Target.ShortName)
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
        ''' FurcadiaSession.Dream right now.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function UnshareFurreNamed(reader As TriggerReader) As Boolean

            Dim Target = Dream.FurreList.GerFurreByName(reader.ReadString)
            If InDream(Target.Name) Then
                Return SendServer("unshare " + Target.ShortName)
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
            Dim furre = Player.ShortName
            SendServer("unshare " + furre)
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
            Dim furre = Player.ShortName
            Return SendServer("share " + furre)

        End Function

        ''' <summary>
        ''' (1:22) and the furre named {..} is not the Dream owner,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedIsNotDREAMOWNER(reader As TriggerReader) As Boolean
            Return Not FurreNamedIsDREAMOWNER(reader)
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

            Dim TrigFurreName = reader.Page.GetVariable("%DREAMOWNER").Value.ToString
            'add Machine Name parser
            Return Player.ShortName = FurcadiaShortName(TrigFurreName)

        End Function



        Public Overrides Sub Unload(page As Page)

        End Sub


    End Class

End Namespace