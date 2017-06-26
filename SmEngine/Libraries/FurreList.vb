Imports Furcadia.Net.Dream
Imports Monkeyspeak

Namespace Engine.Libraries

    Public Class MonkeySpeakFurreList
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
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

        ''' <summary>
        ''' (5:702) count the number of active furres in the drean and put
        ''' it in the variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreActiveListCount(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            Dim c As Double = 0
            For Each fur As FURRE In MyBase.FurcadiaSession.Dream.FurreList
                If fur.AFK = 0 Then
                    c += 1
                End If
            Next
            var.Value = c
            Return True

        End Function

        '(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.
        Function FurreAFKListCount(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            Dim c As Double = 0
            For Each fs As FURRE In FurcadiaSession.Dream.FurreList
                If fs.AFK > 0 Then
                    c += 1
                End If
            Next
            var.Value = c
            Return True

        End Function

        ''' <summary>
        ''' (5:701) save the dream list count to variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreListCount(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            var.Value = Convert.ToDouble(FurcadiaSession.Dream.FurreList.Count.ToString)
            Return True

        End Function

        ''' <summary>
        ''' (5:700) Copy the dreams's furre-list to array %Variable
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreListVar(reader As TriggerReader) As Boolean

            Dim var As Variable = reader.ReadVariable(True)
            Dim str As New ArrayList
            For Each fur As FURRE In FurcadiaSession.Dream.FurreList
                str.Add(fur.Name)
            Next
            var.Value = String.Join(", ", str.ToArray)
            Return True

        End Function

        ''' <summary>
        ''' (1:709) and the furre named {...} is active in the dream,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreNamedActive(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Target.AFK = 0

        End Function

        ''' <summary>
        ''' (1:708) and the furre named {...} is a.f.k.,
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreNamedAFK(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Target.AFK > 0

        End Function

        ''' <summary>
        ''' (1:706) and the furre named {...} is visible.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreNamedCanSe(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Target.Visible

        End Function

        ''' <summary>
        ''' (1:702) and the furre named {...} is in the dream.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreNamedInDream(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return InDream(Target.Name)

        End Function

        ''' <summary>
        ''' (1:707) and the furre named {...} is not visible
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreNamedNotCanSe(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Not Target.Visible

        End Function

        ''' <summary>
        ''' (1:703) and the furre named {...} is not in the dream
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function FurreNamedNotInDream(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Not InDream(Target.Name)

        End Function

        ''' <summary>
        ''' (1:704) and the triggering furre is visible.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function TriggeringCanSe(reader As TriggerReader) As Boolean

            Dim tPlayer As FURRE = FurcadiaSession.Player
            Return tPlayer.Visible

        End Function

        ''' <summary>
        ''' (1:700) and the triggering furre in the dream.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function TriggeringInDream(reader As TriggerReader) As Boolean

            Dim tPlayer As FURRE = FurcadiaSession.Player
            Return InDream(tPlayer.Name)

        End Function

        ''' <summary>
        ''' (1:705) and the triggering furre is not visible
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function TriggeringNotCanSe(reader As TriggerReader) As Boolean

            Dim tPlayer As FURRE = FurcadiaSession.Player
            Return Not tPlayer.Visible

        End Function

        ''' <summary>
        ''' (1:701) and the triggering furre is not in the dream.
        ''' </summary>
        ''' <param name="reader">
        ''' </param>
        ''' <returns>
        ''' </returns>
        Function TriggeringNotInDream(reader As TriggerReader) As Boolean

            Dim tPlayer As FURRE = FurcadiaSession.Player
            Return Not InDream(tPlayer.Name)

        End Function

#End Region

    End Class

End Namespace