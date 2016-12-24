Imports Monkeyspeak
Imports MonkeyCore
Imports MonkeyCore.IO
Imports System.Collections.Generic

Public Class MS_MemberList
    Inherits Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing
    Private Shared MemberList As String
    Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)
        MemberList = "MemberList.txt"

        '(1:900) and the triggering furre is on my Dream Member List,
        Add(New Trigger(TriggerCategory.Condition, 900), AddressOf TrigFurreIsMember, "(1:900) and the triggering furre is on my dream Member List,")
        '(1:901) and the furre named {...} is on my Dream Member list.
        Add(New Trigger(TriggerCategory.Condition, 901), AddressOf FurreNamedIsMember, "(1:901) and the furre named {...} is on my Dream Member list,")

        '(1:902) and the triggering furre is not on my Dream Member list.
        Add(New Trigger(TriggerCategory.Condition, 902), AddressOf TrigFurreIsNotMember, "(1:902) and the triggering furre is not on my Dream Member list,")
        '(1:903) and the furre named {...} is not on my Dream Member list.
        Add(New Trigger(TriggerCategory.Condition, 903), AddressOf FurreNamedIsNotMember, "(1:903) and the furre named {...} is not on my Dream Member list,")


        '(1:900) add the triggering furre to my Dream Member list if they aren't already on it.
        Add(New Trigger(TriggerCategory.Effect, 900), AddressOf AddTrigFurre, "(5:900) add the triggering furre to my Dream Member list if they aren't already on it.")
        '(5:901) add the furre named {...} to my Dream Member list if they aren't already on it.
        Add(New Trigger(TriggerCategory.Effect, 901), AddressOf AddFurreNamed, "(5:901) add the furre named {...} to my Dream Member list if they aren't already on it.")

        '(5:902) remove the triggering furre to my Dream Member list if they are on it.
        Add(New Trigger(TriggerCategory.Effect, 902), AddressOf RemoveTrigFurre, "(5:902) remove the triggering furre to my Dream Member list if they are on it.")
        '(5:903) remove the furre named {...} from my Dream Member list if they are on it.
        Add(New Trigger(TriggerCategory.Effect, 903), AddressOf RemoveFurreNamed, "(5:903) remove the furre named {...} from my Dream Member list if they are on it.")

        '(5:904) Use file {...} as the dream member list. 
        Add(New Trigger(TriggerCategory.Effect, 904), AddressOf UseMemberFile, "(5:904) Use file {...} as the dream member list.")
        '(5:905) store member list to variable %Variable. 
        Add(New Trigger(TriggerCategory.Effect, 905), AddressOf ListToVariable, "(5:905) store member list to variable %Variable.")

    End Sub


    '(1:900) and the triggering furre is on my dream Member List,
    Private Function TrigFurreIsMember(reader As TriggerReader) As Boolean
        CheckMemberList()
        Dim Furre As String = Nothing
        Dim f As New List(Of String)
        Try
            Furre = MainMSEngine.MSpage.GetVariable(MS_Name).Value.ToString
            f.AddRange(File.ReadAllLines(MemberList))
            For Each l As String In f
                If MainMSEngine.ToFurcShortName(l) = MainMSEngine.ToFurcShortName(Furre) Then Return True
            Next
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return MainMSEngine.IsBotControler(Furre)
    End Function
    '(1:901) and the furre named {...} is on my Dream Member list.
    Private Function FurreNamedIsMember(reader As Monkeyspeak.TriggerReader) As Boolean
        CheckMemberList()
        Dim Furre As String = Nothing
        Dim f() As String
        Try
            Furre = reader.ReadString
            f = File.ReadAllLines(MemberList)
            For Each l As String In f
                If MainMSEngine.ToFurcShortName(l) = MainMSEngine.ToFurcShortName(Furre) Then
                    Return True
                End If
            Next

        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return MainMSEngine.IsBotControler(Furre)
    End Function
    '(1:902) and the triggering furre is not on my Dream Member list.
    Private Function TrigFurreIsNotMember(reader As Monkeyspeak.TriggerReader) As Boolean
        Return Not TrigFurreIsMember(reader)
    End Function
    '(1:903) and the furre named {...} is not on my Dream Member list.
    Private Function FurreNamedIsNotMember(reader As Monkeyspeak.TriggerReader) As Boolean
        Return Not FurreNamedIsMember(reader)
    End Function
    '(1:900) add the triggering furre to my Dream Member list if they aren't already on it.
    Private Function AddTrigFurre(reader As TriggerReader) As Boolean
        Dim Furre As String = Nothing

        Try
            Furre = MainMSEngine.MSpage.GetVariable(MS_Name).Value.ToString
            If TrigFurreIsMember(reader) = False And TrigFurreIsNotMember(reader) Then
                Dim sw As StreamWriter = New StreamWriter(MemberList, True)
                sw.WriteLine(Furre)
                sw.Close()
            End If
            Return True

        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try

    End Function


    '(5:901) add the furre named {...} to my Dream Member list if they aren't already on it.
    Private Function AddFurreNamed(reader As TriggerReader) As Boolean
        Dim Furre As String

        Try
            Furre = reader.ReadString
            If FurreNamedIsNotMember(reader) Then
                Using sw As StreamWriter = New StreamWriter(MemberList, True)
                    sw.WriteLine(Furre)
                End Using
            End If
            Return True

        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try

    End Function
    '(5:902) remove the triggering furre to my Dream Member list if they are on it.
    Private Function RemoveTrigFurre(reader As TriggerReader) As Boolean
        Dim Furre As String = Nothing
        CheckMemberList()
        Try
            Furre = MainMSEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Dim line As String
            Dim linesList As New List(Of String)(File.ReadAllLines(MemberList))
            Using SR As New StreamReader(MemberList)
                While SR.Peek() <> -1
                    line = SR.ReadLine()
                    For i As Integer = 0 To linesList.Count - 1
                        If MainMSEngine.ToFurcShortName(line) = MainMSEngine.ToFurcShortName(Furre) Then
                            linesList.RemoveAt(i)
                            File.WriteAllLines(MemberList, linesList.ToArray())
                            Exit For
                        End If
                    Next i
                End While
            End Using
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function
    '(5:903) remove the furre named {...} from my Dream Member list if they are on it.
    Private Function RemoveFurreNamed(reader As TriggerReader) As Boolean
        Dim Furre As String = Nothing
        CheckMemberList()
        Try
            Furre = reader.ReadString
            Dim line As String
            Dim linesList As New List(Of String)(File.ReadAllLines(MemberList))
            Using SR As New StreamReader(MemberList)
                While SR.Peek() <> -1
                    line = SR.ReadLine()
                    For i As Integer = 0 To linesList.Count - 1
                        If MainMSEngine.ToFurcShortName(line) = MainMSEngine.ToFurcShortName(Furre) Then
                            linesList.RemoveAt(i)
                            File.WriteAllLines(MemberList, linesList.ToArray())
                            Exit For
                        End If
                    Next i
                End While
            End Using
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    '(5:904) Use file {...} as the dream member list.  
    Private Function UseMemberFile(reader As TriggerReader) As Boolean

        Try
            MemberList = reader.ReadString
            CheckMemberList()
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function

    Private Function ListToVariable(reader As TriggerReader) As Boolean
        CheckMemberList()
        Dim Furre As Variable
        Dim f() As String
        Try
            Furre = reader.ReadVariable(True)
            f = File.ReadAllLines(MemberList)
            Furre.Value = String.Join(" ", f)
        Catch ex As Exception
            MainMSEngine.LogError(reader, ex)
            Return False
        End Try
        Return True
    End Function
    Private Sub CheckMemberList()
        MemberList = Paths.CheckBotFolder(MemberList)
        If Not File.Exists(MemberList) Then
            Using f As New StreamWriter(MemberList)
                f.WriteLine("")
            End Using
        End If
    End Sub
End Class
