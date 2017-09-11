﻿Imports Furcadia.Util
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Cause:s (0:50) - (0:62
    ''' <para>
    ''' Conditions: (1:50) - (1:53)
    ''' </para>
    ''' Effects: (5:49) - (5:56)
    ''' <para>
    ''' Banish Monkey Speak
    ''' </para>
    ''' This system mirrors Furcadia's banish system by tracking the banish
    ''' commands sent to the game aerver and keep a list of banished furres
    ''' locally. To help keep the list accurate, It is reconmended to ask
    ''' the server for a banish list upon joining the dream. It maybe a good
    ''' idea to run a daily schedule to refresh the list for temp banishes
    ''' to drop off.
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates
    ''' <para>
    ''' (0:50) When the bot fails to banish a furre,
    ''' </para>
    ''' (0:54) When the bot sees the banish list,
    ''' <para>
    ''' (0:55) When the bot fails to remove a furre from the banish list,
    ''' </para>
    ''' <para>
    ''' (0:57) When the bot successfully removes a furre from the banish list,
    ''' </para>
    ''' (0:59) When the bot fails to empty the banish list,
    ''' <para>
    ''' (0:60) When the bot successfully clears the banish list,
    ''' </para>
    ''' (0:61) When the bot successfully temp banishes a furre,
    ''' </remarks>
    Public Class Banish
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
            '(0: ) When the bot fails to banish a furre,
            Add(TriggerCategory.Cause, 50,
            Function()
                Return True
            End Function, "(0:50) When the bot fails to banish a furre,")
            '(0: ) When the bot fails to banish the furre named {...},
            Add(TriggerCategory.Cause, 51, AddressOf AndBanishFurreNamed,
                "(0:51) When the bot fails to banish the furre named {...}")
            '(0: ) When the bot successfully banishes a furre,
            Add(TriggerCategory.Cause, 52,
            Function()
                Return True
            End Function, "(0:52) When the bot successfully banishes a furre,")
            '(0: ) When the bot successfully banishes the furre named {...},
            Add(TriggerCategory.Cause, 53, AddressOf AndBanishFurreNamed,
                "(0:53) When the bot successfully banishes the furre named {...},")

            '(0: ) When the bot sees the banish list,
            Add(TriggerCategory.Cause, 54,
            Function()
                Return True
            End Function, "(0:54) When the bot sees the banish list,")
            '(0: ) When the bot fails to remove a furre from the banish list,
            Add(TriggerCategory.Cause, 55,
            Function()
                Return True
            End Function, "(0:55) When the bot fails to remove a furre from the banish list,")
            '(0: ) When the bot fails to remove the furre named {...} from the banish list,
            Add(TriggerCategory.Cause, 56, AddressOf AndBanishFurreNamed,
                "(0:56) When the bot fails to remove the furre named {...} from the banish list,")

            '(0: ) When the bot successfully removes a furre from the banish list,
            Add(TriggerCategory.Cause, 57,
            Function()
                Return True
            End Function, "(0:57) When the bot successfully removes a furre from the banish list,")
            '(0: ) When the bot successfully removes the furre named {...} from the banish list,
            Add(TriggerCategory.Cause, 58, AddressOf AndBanishFurreNamed,
                "(0:58) When the bot successfully removes the furre named {...} from the banish list,")
            '(0: ) When the bot fails to empty the banish list,
            Add(TriggerCategory.Cause, 59,
            Function()
                Return True
            End Function, "(0:59) When the bot fails to empty the banish list,")

            '(0: ) When the bot successfully clears the banish list,
            Add(TriggerCategory.Cause, 60,
            Function()
                Return True
            End Function, "(0:60) When the bot successfully clears the banish list,")

            '(0: ) When the bot successfully temp banishes a furee,
            Add(TriggerCategory.Cause, 61,
            Function()
                Return True
            End Function, "(0:61) When the bot successfully temp banishes a furre,")

            '(0:62) When the bot successfully temp banishes the furre named {...},
            Add(TriggerCategory.Cause, 62, AddressOf AndBanishFurreNamed,
                "(0:62) When the bot successfully temp banishes the furre named {...},")

            '(1:50) and the triggering furre is not on the banish list,
            Add(New Trigger(TriggerCategory.Condition, 50), AddressOf TrigFurreIsNotBanished, "(1:50) and the triggering furre is not on the banish list,")

            '(1:51) and the triggering furre is on the banish list,
            Add(New Trigger(TriggerCategory.Condition, 51), AddressOf TrigFurreIsBanished, "(1:51) and the triggering furre is on the banish list,")
            '(1:52) and the furre named {...} is not on the banish list,
            Add(New Trigger(TriggerCategory.Condition, 52), AddressOf FurreNamedIsNotBanished, "(1:52) and the furre named {...} is not on the banish list,")
            '(1:53) and the furre named {...} is on the banish list,
            Add(New Trigger(TriggerCategory.Condition, 53), AddressOf FurreNamedIsBanished, "(1:53) and the furre named {...} is on the banish list,")

            ' (5: ) save the banish list to the variable % .
            Add(New Trigger(TriggerCategory.Effect, 49), AddressOf BanishSave,
                "(5:49) save the banish list to the variable % .")

            '(5:x) as the server for the banish-list.
            Add(New Trigger(TriggerCategory.Effect, 50), AddressOf BanishList,
            "(5:50) ask the server for the banish-list.")
            '(5:x) banish the triggering furre.
            Add(New Trigger(TriggerCategory.Effect, 51), AddressOf BanishTrigFurre,
            "(5:51) banish the triggering furre.")
            '(5:x) banish the furre named {...}.
            Add(New Trigger(TriggerCategory.Effect, 52), AddressOf BanishFurreNamed,
            "(5:52) banish the furre named {...}.")
            '(5:x) temporarily  banish the triggering furre for three days.
            Add(New Trigger(TriggerCategory.Effect, 53), AddressOf TempBanishTrigFurre,
                "(5:53) temporarily  banish the triggering furre for three days.")

            '(5:x) temporarily banish the furre named {...} for three days.
            Add(New Trigger(TriggerCategory.Effect, 54), AddressOf TempBanishFurreNamed,
                "(5:54) temporarily banish the furre named {...} for three days.")
            '(5:x) unbanish the triggering furre.
            Add(New Trigger(TriggerCategory.Effect, 55), AddressOf UnBanishTrigFurre,
                "(5:55) unbanish the triggering furre.")
            '(5:x) unbanish the furre named {...}.
            Add(New Trigger(TriggerCategory.Effect, 56), AddressOf UnBanishFurreNamed,
                "(5:56) unbanish the furre named {...}.")
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (0:51) When the bot fails to banish the furre named {...}
        ''' <para>
        ''' (0:53) When the bot successfully banishes the furre named {...},
        ''' </para>
        ''' <para>
        ''' (0:56) When the bot fails to remove the furre named {...} from
        ''' the banish list,
        ''' </para>
        ''' <para>
        ''' (0:58) When the bot successfully removes the furre named {...}
        ''' from the banish list,
        ''' </para>
        ''' <para>
        ''' (0:62) When the bot successfully temp banishes the furre named {...},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Name Match
        ''' </returns>
        Function AndBanishFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = reader.ReadString
            Return FurcadiaShortName(Furre) = FurcadiaShortName(FurcadiaSession.BanishName)

            Return True
        End Function

        ''' <summary>
        ''' (5:52) banish the furre named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on sending the banish command
        ''' </returns>
        Function BanishFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = reader.ReadString
            Return sendServer("banish " + Furre)

        End Function

        ''' <summary>
        ''' (5:50) ask the server for the banish-list.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on sending the Server Command
        ''' </returns>
        Function BanishList(reader As TriggerReader) As Boolean

            Return sendServer("banish-list")

        End Function

        ''' <summary>
        ''' (5:49) save the banish list to the variable % .
        ''' <para>
        ''' Reads the mirror banish list and stores is as a coma sepperated
        ''' list in a <see cref="MonkeySpeak.Variable"/>
        ''' </para>
        ''' <para>
        ''' It's a good idea to ask the server for the dreams banish list to
        ''' keep this list current
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Function BanishSave(reader As TriggerReader) As Boolean
            Dim NewVar As Variable

            NewVar = reader.ReadVariable(True)

            NewVar.Value = String.Join(" ", FurcadiaSession.BanishString.ToArray)
            Return True
        End Function

        ''' <summary>
        ''' (5:51) banish the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Function BanishTrigFurre(reader As TriggerReader) As Boolean
            Return sendServer("banish " + FurcadiaSession.Player.Name)

        End Function

        '
        ''' <summary>
        ''' (1:53) and the furre named {...} is on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the furre is on the mirror banish list
        ''' </returns>
        Function FurreNamedIsBanished(reader As TriggerReader) As Boolean
            Return Not FurreNamedIsNotBanished(reader)
        End Function

        ''' <summary>
        ''' (1:52) and the furre named {...} is not on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the furre is not on the mirror banish list
        ''' </returns>
        Function FurreNamedIsNotBanished(reader As TriggerReader) As Boolean
            Dim banishlist As List(Of String) = FurcadiaSession.BanishString

            Dim f As String = reader.ReadString
            For Each Furre As String In banishlist
                If FurcadiaShortName(Furre) = FurcadiaShortName(f) Then Return False
            Next
            Return True

        End Function

        ''' <summary>
        ''' (5:54) temporarily banish the furre named {...} for three days.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Function TempBanishFurreNamed(reader As TriggerReader) As Boolean

            Return sendServer("tempbanish " + FurcadiaSession.Player.Name)

        End Function

        ''' <summary>
        ''' (5:53) temporarily banish the triggering furre for three days.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Function TempBanishTrigFurre(reader As TriggerReader) As Boolean

            Dim Furre As String = reader.ReadString
            Return sendServer("tempbanish " + Furre)

        End Function

        ''' <summary>
        ''' (1:51) and the triggering furre is on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true is the triggering furre is on the mirror banish list
        ''' </returns>
        Function TrigFurreIsBanished(reader As TriggerReader) As Boolean
            Return Not TrigFurreIsNotBanished(reader)
        End Function

        ''' <summary>
        ''' (1:50) and the triggering furre is not on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the triggering furre is not on the mirror banish list
        ''' </returns>
        Function TrigFurreIsNotBanished(reader As TriggerReader) As Boolean
            Dim banishlist As List(Of String) = FurcadiaSession.BanishString

            For Each Furre As String In banishlist
                If FurcadiaShortName(Furre) = FurcadiaSession.Player.ShortName Then Return False
            Next

            Return True
        End Function

        ''' <summary>
        ''' (5:56) unbanish the furre named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on sending the server command
        ''' </returns>
        Function UnBanishFurreNamed(reader As TriggerReader) As Boolean

            Dim Furre As String = reader.ReadString
            Return sendServer("banish-off " + Furre)

        End Function

        ''' <summary>
        ''' (5:55) unbanish the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Function UnBanishTrigFurre(reader As TriggerReader) As Boolean

            Return sendServer("banish-off " + FurcadiaSession.Player.Name)

        End Function

#End Region

    End Class

End Namespace