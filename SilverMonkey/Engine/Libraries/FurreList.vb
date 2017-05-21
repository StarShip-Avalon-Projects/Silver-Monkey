Imports System.Collections
Imports System.Collections.Generic
Imports Furcadia.Net
Imports Monkeyspeak

Namespace Engine.Libraries
    Public Class MonkeySpeakFurreList
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

#End Region

#Region "Public Constructors"

        Public Sub New()
            MyBase.New
            '(1:700) and the triggering furre in the dream.
            Add(New Trigger(TriggerCategory.Condition, 700), AddressOf TriggeringInDream,
            "(1:700) and the triggering furre in the dream.")

            '(1:701) and the triggering furre is not in the dream.
            Add(New Trigger(TriggerCategory.Condition, 701), AddressOf TriggeringNotInDream,
            "(1:701) and the triggering furre is not in the dream.")

            '(1:702) and the furre named {...} is in the dream.
            Add(New Trigger(TriggerCategory.Condition, 702), AddressOf FurreNamedInDream,
            "(1:702) and the furre named {...} is in the dream.")

            '(1:703) and the furre named {...} is not in the dream
            Add(New Trigger(TriggerCategory.Condition, 703), AddressOf FurreNamedNotInDream,
            "(1:703) and the furre named {...} is not in the dream")

            '(1:704) and the triggering furre is visible.
            Add(New Trigger(TriggerCategory.Condition, 704), AddressOf TriggeringCanSe,
            "(1:704) and the triggering furre is visible.")

            '(1:705) and the triggering furre is not visible
            Add(New Trigger(TriggerCategory.Condition, 705), AddressOf TriggeringNotCanSe,
            "(1:705) and the triggering furre is not visible")

            '(1:706) and the furre named {...} is visible.
            Add(New Trigger(TriggerCategory.Condition, 706), AddressOf FurreNamedCanSe,
            "(1:706) and the furre named {...} is visible.")

            '(1:707) and the furre named {...} is not visible
            Add(New Trigger(TriggerCategory.Condition, 707), AddressOf FurreNamedNotCanSe,
            "(1:707) and the furre named {...} is not visible")

            '(1:708) and the furre named {...} is a.f.k.,
            Add(New Trigger(TriggerCategory.Condition, 708), AddressOf FurreNamedAFK,
            "(1:708) and the furre named {...} is a.f.k.,")

            '(1:709) and the furre named {...} is active in the dream,
            Add(New Trigger(TriggerCategory.Condition, 709), AddressOf FurreNamedActive,
            "(1:709) and the furre named {...} is active in the dream,")

            '(5:700) Copy the dreams's furre-list to array %Variable
            Add(New Trigger(TriggerCategory.Effect, 700), AddressOf FurreListVar,
            "(5:700) copy the dreams's furre-list to variable %Variable")

            '(5:701) save the dream list count to variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 701), AddressOf FurreListCount,
            "(5:701) save the dream list count to variable %Variable.")

            '(5:702) count the number of active furres in the drean and put it in the variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 702), AddressOf FurreActiveListCount,
    "(5:702) count the number of active furres in the drean and put it in the variable %Variable.")

            '(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 703), AddressOf FurreAFKListCount,
            "(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.")

        End Sub

#End Region

#Region "Helper Functions"
        Private Function InDream(ByRef Name As String) As Boolean
            Dim found As Boolean = False
            For Each Fur As FURRE In FurcadiaSession.Dream.FurreList
                If Fur.ShortName = Furcadia.Util.FurcadiaShortName(Name) Then
                    Return True
                End If
            Next
            Return False
        End Function

#End Region

#Region "Public Methods"

        '(5:702) count the number of active furres in the drean and put it in the variable %Variable.
        Function FurreActiveListCount(reader As TriggerReader) As Boolean
            Try
                Dim var As Variable = reader.ReadVariable(True)
                Dim c As Double = 0
                For Each fur As FURRE In MyBase.FurcadiaSession.Dream.FurreList
                    If fur.AFK = 0 Then
                        c += 1
                    End If
                Next
                var.Value = c
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.
        Function FurreAFKListCount(reader As TriggerReader) As Boolean
            Try
                Dim var As Variable = reader.ReadVariable(True)
                Dim c As Double = 0
                For Each fs As FURRE In FurcadiaSession.Dream.FurreList
                    If fs.AFK > 0 Then
                        c += 1
                    End If
                Next
                var.Value = c
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:701) save the dream list count to variable %Variable.
        Function FurreListCount(reader As TriggerReader) As Boolean
            Try
                Dim var As Variable = reader.ReadVariable(True)
                var.Value = Convert.ToDouble(FurcadiaSession.Dream.FurreList.Count.ToString)
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:700) Copy the dreams's furre-list to array %Variable
        Function FurreListVar(reader As TriggerReader) As Boolean
            Try
                Dim var As Variable = reader.ReadVariable(True)
                Dim str As New ArrayList
                For Each fur As FURRE In FurcadiaSession.Dream.FurreList
                    str.Add(fur.Name)
                Next
                var.Value = String.Join(", ", str.ToArray)
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:709) and the furre named {...} is active in the dream,
        Function FurreNamedActive(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return Target.AFK = 0
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:708) and the furre named {...} is a.f.k.,
        Function FurreNamedAFK(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return Target.AFK > 0
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(1:706) and the furre named {...} is visible.
        Function FurreNamedCanSe(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return Target.Visible
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(1:702) and the furre named {...} is in the dream.
        Function FurreNamedInDream(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return InDream(Target.Name)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(1:707) and the furre named {...} is not visible
        Function FurreNamedNotCanSe(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return Not Target.Visible
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:703) and the furre named {...} is not in the dream
        Function FurreNamedNotInDream(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return Not InDream(Target.Name)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:704) and the triggering furre is visible.
        Function TriggeringCanSe(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Return tPlayer.Visible
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:700) and the triggering furre in the dream.
        Function TriggeringInDream(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Return InDream(tPlayer.Name)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(1:705) and the triggering furre is not visible
        Function TriggeringNotCanSe(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Return Not tPlayer.Visible
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:701) and the triggering furre is not in the dream.
        Function TriggeringNotInDream(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Return Not InDream(tPlayer.Name)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

#End Region

    End Class
End Namespace