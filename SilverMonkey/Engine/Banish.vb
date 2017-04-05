Imports System.Diagnostics
Imports System.Collections
Imports MonkeyCore
Imports System.Collections.Generic
Imports Furcadia.Util

Public Class Banish
    Inherits MonkeySpeakLibrary

#Region "Private Fields"

    Private writer As TextBoxWriter = Nothing

#End Region

#Region "Public Constructors"

    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)
        '(0: ) When the bot fails to banish a furre,
        Add(Monkeyspeak.TriggerCategory.Cause, 50,
            Function()
                Return True
            End Function, "(0:50) When the bot fails to banish a furre,")
        '(0: ) When the bot fails to banish the furre named {...},
        Add(Monkeyspeak.TriggerCategory.Cause, 51, AddressOf AndBanishFurreNamed, "(0:51) When the bot fails to banish the furre named {...}")
        '(0: ) When the bot successfully banishes a furre,
        Add(Monkeyspeak.TriggerCategory.Cause, 52,
            Function()
                Return True
            End Function, "(0:52) When the bot successfully banishes a furre,")
        '(0: ) When the bot successfully banishes the furre named {...},
        Add(Monkeyspeak.TriggerCategory.Cause, 53, AddressOf AndBanishFurreNamed, "(0:53) When the bot successfully banishes the furre named {...},")

        '(0: ) When the bot sees the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 54,
            Function()
                Return True
            End Function, "(0:54) When the bot sees the banish list,")
        '(0: ) When the bot fails to remove a furre from the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 55,
            Function()
                Return True
            End Function, "(0:55) When the bot fails to remove a furre from the banish list,")
        '(0: ) When the bot fails to remove the furre named {...} from the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 56, AddressOf AndBanishFurreNamed, "(0:56) When the bot fails to remove the furre named {...} from the banish list,")

        '(0: ) When the bot successfully removes a furre from the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 57,
            Function()
                Return True
            End Function, "(0:57) When the bot successfully removes a furre from the banish list,")
        '(0: ) When the bot successfully removes the furre named {...} from the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 58, AddressOf AndBanishFurreNamed, "(0:58) When the bot successfully removes the furre named {...} from the banish list,")
        '(0: ) When the bot fails to empty the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 59,
            Function()
                Return True
            End Function, "(0:59) When the bot fails to empty the banish list,")

        '(0: ) When the bot successfully clears the banish list,
        Add(Monkeyspeak.TriggerCategory.Cause, 60,
            Function()
                Return True
            End Function, "(0:60) When the bot successfully clears the banish list,")

        '(0: ) When the bot successfully temp banishes a furee,
        Add(Monkeyspeak.TriggerCategory.Cause, 61,
            Function()
                Return True
            End Function, "(0:61) When the bot successfully temp banishes a furre,")

        '(0:62) When the bot successfully temp banishes the furre named {...},
        Add(Monkeyspeak.TriggerCategory.Cause, 62, AddressOf AndBanishFurreNamed, "(0:62) When the bot successfully temp banishes the furre named {...},")

        '(1:50) and the triggering furre is not on the banish list,
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 50), AddressOf TrigFurreIsNotBanished, "(1:50) and the triggering furre is not on the banish list,")

        '(1:51) and the triggering furre is on the banish list,
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 51), AddressOf TrigFurreIsBanished, "(1:51) and the triggering furre is on the banish list,")
        '(1:52) and the furre named {...} is not on the banish list,
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 52), AddressOf FurreNamedIsNotBanished, "(1:52) and the furre named {...} is not on the banish list,")
        '(1:53) and the furre named {...} is on the banish list,
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 53), AddressOf FurreNamedIsBanished, "(1:53) and the furre named {...} is on the banish list,")

        ' (5: ) save the banish list to the variable % .
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 49), AddressOf BanishSave,
         "(5:49) save the banish list to the variable % .")

        '(5:x) as the server for the banish-list.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 50), AddressOf BanishList,
            "(5:50) ask the server for the banish-list.")
        '(5:x) banish the triggering furre.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 51), AddressOf BanishTrigFurre,
            "(5:51) banish the triggering furre.")
        '(5:x) banish the furre named {...}.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 52), AddressOf BanishFurreNamed,
            "(5:52) banish the furre named {...}.")
        '(5:x) temporarily  banish the triggering furre for three days.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 53), AddressOf TempBanishTrigFurre, "(5:53) temporarily  banish the triggering furre for three days.")

        '(5:x) temporarily banish the furre named {...} for three days.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 54), AddressOf TempBanishFurreNamed,
"(5:54) temporarily banish the furre named {...} for three days.")
        '(5:x) unbanish the triggering furre.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 55), AddressOf UnBanishTrigFurre,
"(5:55) unbanish the triggering furre.")
        '(5:x) unbanish the furre named {...}.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 56), AddressOf UnBanishFurreNamed,
"(5:56) unbanish the furre named {...}.")
    End Sub

#End Region

#Region "Public Methods"

    '(0:53) When the bot sucessfilly banishes the furre named {...},
    '(0: ) When the bot successfully temp banishes the furre named {...},
    Function AndBanishFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Furre As String = reader.ReadString
            Return FurcadiaShortName(Furre) = FurcadiaShortName(FurcSession.BanishName)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:x) banish the furre named {...}.
    Function BanishFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Furre As String = reader.ReadString
            sendServer("banish " + Furre)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:x) as the server for the banish-list.
    Function BanishList(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            sendServer("banish-list")
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    Function BanishSave(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim NewVar As Monkeyspeak.Variable

        Try

            NewVar = reader.ReadVariable(True)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try

        NewVar.Value = String.Join(" ", FurcSession.BanishString.ToArray)
        Return True
    End Function

    '(5:x) banish the triggering furre.
    Function BanishTrigFurre(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            sendServer("banish " + FurcSession.Player.Name)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(1:53) and the furre named {...} is on the banish list,
    Function FurreNamedIsBanished(reader As Monkeyspeak.TriggerReader) As Boolean
        Return Not FurreNamedIsNotBanished(reader)
    End Function

    '(1:52) and the furre named {...} is not on the banish list,
    Function FurreNamedIsNotBanished(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim banishlist As List(Of String) = FurcSession.BanishString
        Try
            Dim f As String = reader.ReadString
            For Each Furre As String In banishlist
                If FurcadiaShortName(Furre) = FurcadiaShortName(f) Then Return False
            Next
            Return True
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:x) temperary  banish the triggering furre for three days.
    Function TempBanishFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            sendServer("tempbanish " + FurcSession.Player.Name)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:x) temperoily banish the furre named {...} for three days.
    Function TempBanishTrigFurre(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Furre As String = reader.ReadString
            sendServer("tempbanish " + Furre)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(1:51) and the triggering furre is on the banish list,
    Function TrigFurreIsBanished(reader As Monkeyspeak.TriggerReader) As Boolean
        Return Not TrigFurreIsNotBanished(reader)
    End Function

    '(1:50) and the triggering furre is not on the banish list,
    Function TrigFurreIsNotBanished(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim banishlist As List(Of String) = FurcSession.BanishString
        Try
            For Each Furre As String In banishlist
                If FurcadiaShortName(Furre) = FurcSession.Player.ShortName Then Return False
            Next
            Return True
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function
    '(5:x) unbanish the furre named {...}.
    Function UnBanishFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Furre As String = reader.ReadString
            sendServer("banish-off " + Furre)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:x) unbanish the triggering furre.
    Function UnBanishTrigFurre(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            sendServer("banish-off " + FurcSession.Player.Name)
        Catch ex As Exception
            MainMsEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

#End Region

End Class