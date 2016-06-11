Imports Monkeyspeak
Imports System.Diagnostics
Imports System.Collections
Imports System.Collections.Generic
Imports SilverMonkey.ErrorLogging
Imports SilverMonkey.TextBoxWriter
Imports Furcadia.Net.Movement
Imports Furcadia.Net

Public Class FurreList
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private writer As TextBoxWriter = Nothing

    Dim FurreList As Dictionary(Of UInteger, FURRE) = DREAM.List

    Public Sub New()
        writer = New TextBoxWriter(Variables.TextBox1)

        '(1:700) and the triggering furre in the dream.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 700), AddressOf TriggeringInDream,
            "(1:700) and the triggering furre in the dream.")

        '(1:701) and the triggering furre is not in the dream.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 701), AddressOf TriggeringNotInDream,
            "(1:701) and the triggering furre is not in the dream.")

        '(1:702) and the furre named {...} is in the dream.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 702), AddressOf FurreNamedInDream,
            "(1:702) and the furre named {...} is in the dream.")

        '(1:703) and the furre named {...} is not in the dream
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 703), AddressOf FurreNamedNotInDream,
            "(1:703) and the furre named {...} is not in the dream")

        '(1:704) and the triggering furre is visible.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 704), AddressOf TriggeringCanSee,
            "(1:704) and the triggering furre is visible.")

        '(1:705) and the triggering furre is not visible
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 705), AddressOf TriggeringNotCanSee,
            "(1:705) and the triggering furre is not visible")

        '(1:706) and the furre named {...} is visible.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 706), AddressOf FurreNamedCanSee,
            "(1:706) and the furre named {...} is visible.")

        '(1:707) and the furre named {...} is not visible
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 707), AddressOf FurreNamedNotCanSee,
            "(1:707) and the furre named {...} is not visible")

        '(1:708) and the furre named {...} is a.f.k.,
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 708), AddressOf FurreNamedAFK,
            "(1:708) and the furre named {...} is a.f.k.,")

        '(1:709) and the furre named {...} is active in the dream,
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 709), AddressOf FurreNamedActive,
            "(1:709) and the furre named {...} is active in the dream,")

        '(5:700) Copy the dreams's furre-list to array %Variable
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 700), AddressOf FurreListVar,
            "(5:700) copy the dreams's furre-list to variable %Variable")

        '(5:701) save the dream list count to variable %Variable.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 701), AddressOf FurreListCount,
            "(5:701) save the dream list count to variable %Variable.")

        '(5:702) count the number of active furres in the drean and put it in the variable %Variable.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 702), AddressOf FurreActiveListCount,
    "(5:702) count the number of active furres in the drean and put it in the variable %Variable.")

        '(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.
        Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 703), AddressOf FurreAFKListCount,
            "(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.")


    End Sub

    '(1:700) and the triggering furre in the dream.
    Function TriggeringInDream(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim tPlayer As FURRE = callbk.Player
            Return InDream(tPlayer.Name)
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

    '(1:701) and the triggering furre is not in the dream.
    Function TriggeringNotInDream(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim tPlayer As FURRE = callbk.Player
            Return Not InDream(tPlayer.Name)
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
    '(1:702) and the furre named {...} is in the dream.
    Function FurreNamedInDream(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim name As String = reader.ReadString
            Dim Target As FURRE = callbk.NametoFurre(name, False)
            Return InDream(Target.Name)
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

    '(1:703) and the furre named {...} is not in the dream
    Function FurreNamedNotInDream(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim name As String = reader.ReadString
            Dim Target As FURRE = callbk.NametoFurre(name, False)
            Return Not InDream(Target.Name)
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

    '(1:704) and the triggering furre is visible.
    Function TriggeringCanSee(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim tPlayer As FURRE = callbk.Player
            Return tPlayer.Visible
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
    '(1:705) and the triggering furre is not visible
    Function TriggeringNotCanSee(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim tPlayer As FURRE = callbk.Player
            Return Not tPlayer.Visible
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
    '(1:706) and the furre named {...} is visible.
    Function FurreNamedCanSee(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim name As String = reader.ReadString
            Dim Target As FURRE = callbk.NametoFurre(name, False)
            Return Target.Visible
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
    '(1:707) and the furre named {...} is not visible
    Function FurreNamedNotCanSee(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim name As String = reader.ReadString
            Dim Target As FURRE = callbk.NametoFurre(name, False)
            Return Not Target.Visible
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

    '(1:708) and the furre named {...} is a.f.k.,
    Function FurreNamedAFK(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim name As String = reader.ReadString
            Dim Target As FURRE = callbk.NametoFurre(name, False)
            Return Target.AFK > 0
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

    '(1:709) and the furre named {...} is active in the dream,
    Function FurreNamedActive(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim name As String = reader.ReadString
            Dim Target As FURRE = callbk.NametoFurre(name, False)
            Return Target.AFK = 0
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

    '(5:700) Copy the dreams's furre-list to array %Variable
    Function FurreListVar(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim str As New ArrayList
            For Each kvp As KeyValuePair(Of UInteger, FURRE) In FurreList
                str.Add(kvp.Value.Name)
            Next
            var.Value = String.Join(", ", str.ToArray)
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

    '(5:701) save the dream list count to variable %Variable.
    Function FurreListCount(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim c As Double = 0
            Double.TryParse(FurreList.Count.ToString, c)
            var.Value = c
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

    '(5:702) count the number of active furres in the drean and put it in the variable %Variable.
    Function FurreActiveListCount(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim c As Double = 0
            For Each fs As KeyValuePair(Of UInteger, FURRE) In FurreList
                If fs.Value.AFK = 0 Then
                    c += 1
                End If
            Next
            var.Value = c
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
    '(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.
    Function FurreAFKListCount(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim var As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim c As Double = 0
            For Each fs As KeyValuePair(Of UInteger, FURRE) In FurreList
                If fs.Value.AFK > 0 Then
                    c += 1
                End If
            Next
            var.Value = c
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
#Region "Helper Functions"
    Private Function InDream(ByRef Name As String) As Boolean
        Dim found As Boolean = False
        For Each kvp As KeyValuePair(Of UInteger, FURRE) In FurreList
            If kvp.Value.Name.ToFurcShortName = Name.ToFurcShortName Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

End Class
