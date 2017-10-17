Imports Furcadia.Net.Dream
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Conditions (1:700) - (1:709)
    ''' <para>
    ''' Effects (5:700) - (5:703)
    ''' </para>
    ''' <para>
    ''' Furres in dream
    ''' </para>
    ''' <para>
    ''' AFK vs active players
    ''' </para>
    ''' </summary>
    Public NotInheritable Class MsFurreList
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
            "(5:701) save the dream furre list count to variable %Variable.")

            '(5:702) count the number of active furres in the drean and put it in the variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 702), AddressOf FurreActiveListCount,
                 "(5:702) count the number of active furres in the dream and put it in the variable %Variable.")

            '(5:703) count the number of A.F.K furres in the drean and put it in the variable %Variable.
            Add(New Trigger(TriggerCategory.Effect, 703), AddressOf FurreAFKListCount,
             "(5:703) count the number of A.F.K furres in the dream and put it in the variable %Variable.")

        End Sub

#End Region

#Region "Helper Functions"

#Disable Warning BC40003 ' function 'InDream' shadows an overloadable member declared in the base class 'MonkeySpeakLibrary'.  If you want to overload the base method, this method must be declared 'Overloads'.

        ''' <summary>
        ''' Is the player named {...} in the dream?
        ''' </summary>
        ''' <param name="Name">
        ''' Furre Name
        ''' </param>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Private Function InDream(ByRef Name As String) As Boolean
#Enable Warning BC40003 ' function 'InDream' shadows an overloadable member declared in the base class 'MonkeySpeakLibrary'.  If you want to overload the base method, this method must be declared 'Overloads'.
            Dim found As Boolean = False
            For Each Fur In FurcadiaSession.Dream.FurreList
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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' the Number of Furre in dream as a <see cref="Monkeyspeak.Variable"/>
        ''' </returns>
        Public Function FurreActiveListCount(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable(True)
            Dim c As Double = 0
            For Each fur In FurcadiaSession.Dream.FurreList
                If fur.AfkTime = 0 Then
                    c += 1
                End If
            Next
            var.Value = c
            Return True

        End Function

        ''' <summary>
        ''' (5:703) count the number of A.F.K furres in the drean and put it
        ''' in the variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' the number of AFK furres in the Dream as a <see cref="Monkeyspeak.Variable"/>
        ''' <para>To be counted as AFK, Thier AFK time is greater than zero</para>
        ''' </returns>
        Public Function FurreAFKListCount(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable(True)
            Dim c As Double = 0
            For Each fs In FurcadiaSession.Dream.FurreList
                If fs.AfkTime > 0 Then
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
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' The total number of furres in a <see cref="Monkeyspeak.Variable"/>
        ''' </returns>
        Public Function FurreListCount(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable(True)
            var.Value = Convert.ToDouble(FurcadiaSession.Dream.FurreList.Count.ToString)
            Return True

        End Function

        ''' <summary>
        ''' (5:700) Copy the dreams's furre-list to array %Variable
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' a comoa seperated list of Furre Names
        ''' </returns>
        Public Function FurreListVar(reader As TriggerReader) As Boolean

            Dim var = reader.ReadVariable(True)
            Dim str As New List(Of String)
            For Each fur In FurcadiaSession.Dream.FurreList
                str.Add(fur.Name)
            Next
            var.Value = String.Join(", ", str.ToArray)
            Return True

        End Function

        ''' <summary>
        ''' (1:709) and the furre named {...} is active in the dream,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True is the furre specified has afk time of 0
        ''' </returns>
        Public Function FurreNamedActive(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Target.AfkTime = 0

        End Function

        ''' <summary>
        ''' (1:708) and the furre named {...} is a.f.k.,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' returns true if the Furre has a afk time greater than zero
        ''' </returns>
        Public Function FurreNamedAFK(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Target.AfkTime > 0

        End Function

        ''' <summary>
        ''' (1:706) and the furre named {...} is visible.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the specified furre is visible
        ''' </returns>
        Public Function FurreNamedCanSe(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Target.Visible

        End Function

        ''' <summary>
        ''' (1:702) and the furre named {...} is in the dream.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the furre specified is in the dream
        ''' </returns>
        Public Function FurreNamedInDream(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return InDream(Target.Name)

        End Function

        ''' <summary>
        ''' (1:707) and the furre named {...} is not visible
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' returns true if the spcified furre is not visible
        ''' </returns>
        Public Function FurreNamedNotCanSe(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Not Target.Visible

        End Function

        ''' <summary>
        ''' (1:703) and the furre named {...} is not in the dream
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the specified furre is not in the dream
        ''' </returns>
        Public Function FurreNamedNotInDream(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
            Return Not InDream(Target.Name)

        End Function

        ''' <summary>
        ''' (1:704) and the triggering furre is visible.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true is the triggering furre is visible
        ''' </returns>
        Public Function TriggeringCanSe(reader As TriggerReader) As Boolean

            Return Player.Visible

        End Function

        ''' <summary>
        ''' (1:700) and the triggering furre in the dream.
        ''' <para>Consider Whisperibg furres</para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the triggering furre is in the dream
        ''' </returns>
        Public Function TriggeringInDream(reader As TriggerReader) As Boolean

            Return InDream(Player.Name)

        End Function

        ''' <summary>
        ''' (1:705) and the triggering furre is not visible
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' returns true if the triggering furre is not visible
        ''' </returns>
        Public Function TriggeringNotCanSe(reader As TriggerReader) As Boolean

            Return Not Player.Visible

        End Function

        ''' <summary>
        ''' (1:701) and the triggering furre is not in the dream.
        ''' <para>considering a whispering furre</para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the triggering furre is not in the dream
        ''' </returns>
        Public Function TriggeringNotInDream(reader As TriggerReader) As Boolean

            Dim tPlayer = FurcadiaSession.Player
            Return Not InDream(tPlayer.Name)

        End Function



        Public Overrides Sub Unload(page As Page)

        End Sub


#End Region

    End Class

End Namespace