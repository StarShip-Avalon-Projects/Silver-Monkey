Imports Furcadia.Net
Imports Furcadia.Text.Base220
Imports MonkeyCore

Imports Monkeyspeak

Namespace Engine.Libraries
    ''' <summary>
    ''' Work with Furre Descriptions in Monkey Speak
    ''' </summary>
    Public Class Description
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Sub New()
            MyBase.New

            '(0:600) When the bot reads a description.
            Add(TriggerCategory.Cause, 600,
        Function()
            Return True
        End Function, "(0:600) When the bot reads a description.")

            '(1:600) and triggering furre's description contains {...}
            Add(New Trigger(TriggerCategory.Condition, 600), AddressOf DescContains,
"(1:600) and triggering furre's description contains {...}")

            '(1:601) and triggering furre's description does not contain {...}
            Add(New Trigger(TriggerCategory.Condition, 601), AddressOf NotDescContains,
"(1:601) and triggering furre's description does not contain {...}")

            '(1:602) and the furre named {...} description contains {...}
            Add(New Trigger(TriggerCategory.Condition, 602), AddressOf DescContainsFurreNamed,
"(1:602) and the furre named {...} description contains {...}if they are in the dream,")

            '(1:603) and the furre named {...} description does not contain {...}
            Add(New Trigger(TriggerCategory.Condition, 603), AddressOf NotDescContainsFurreNamed,
"(1:603) and the furre named {...} description does not contain {...} if they are in the dream")

            '(1:604) and the triggering furre is male,
            Add(New Trigger(TriggerCategory.Condition, 604), AddressOf TriggeringFurreMale,
"(1:604) and the triggering furre is male,")

            '(1:605) and the triggering furre is female,
            Add(New Trigger(TriggerCategory.Condition, 605), AddressOf TriggeringFurreFemale,
"(1:605) and the triggering furre is female,")

            '(1:606) and the triggering furre is unspecified,
            Add(New Trigger(TriggerCategory.Condition, 606), AddressOf TriggeringFurreUnspecified,
"(1:606) and the triggering furre is unspecified,")

            '(1:608) and the furre named {...}'s is male,
            Add(New Trigger(TriggerCategory.Condition, 608), AddressOf FurreNamedMale,
"(1:608) and the furre named {...}'s is male if they are in the dream,")
            '(1:609) and the furre named {...}'s is female,
            Add(New Trigger(TriggerCategory.Condition, 609), AddressOf FurreNamedFemale,
"(1:609) and the furre named {...}'s is female if they are in the dream,")
            '(1:609) and the furre named {...}'s is female,
            Add(New Trigger(TriggerCategory.Condition, 610), AddressOf FurreNamedUnSpecified,
"(1:610) and the furre Named {...}'s is unspecified if they are in the dream,")

            '(1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(New Trigger(TriggerCategory.Condition, 612), AddressOf TriggeringFurreSpecies,
"(1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:613) and the furre named {...} is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(New Trigger(TriggerCategory.Condition, 613), AddressOf FurreNamedSpecies,
"(1:613) and the furre named {...} is Species # if they are in the dream (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:614) and the triggering furre has wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 614), AddressOf TriggeringFurreWings,
"(1:614) and the triggering furre has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:615) and the triggering furre doesn't wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 615), AddressOf TriggeringFurreNoWings,
"(1:615) and the triggering furre doesn't wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:616) and the furre named {...} has wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 616), AddressOf FurreNamedWings,
"(1:616) and the furre named {...} has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:617) and the furre named {...}  doesn't wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 617), AddressOf FurreNamedNoWings,
"(1:617) and the furre named {...}  doesn't wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:618) and the triggering furre is standing.
            Add(New Trigger(TriggerCategory.Condition, 618), AddressOf TriggeringFurreStanding,
"(1:618) and the triggering furre is standing.")

            '(1:619) and the triggering furre is sitting.
            Add(New Trigger(TriggerCategory.Condition, 619), AddressOf TriggeringFurreSitting,
"(1:619) and the triggering furre is sitting.")

            '(1:620) and the triggering furre is laying.
            Add(New Trigger(TriggerCategory.Condition, 620), AddressOf TriggeringFurreLaying,
"(1:620) and the triggering furre is laying.")

            '(1:621) and the triggering furre is facing NE,
            Add(New Trigger(TriggerCategory.Condition, 621), AddressOf TriggeringFurreFacingNE,
"(1:621) and the triggering furre is facing NE,")

            '(1:622) and the triggering furre is facing NW,
            Add(New Trigger(TriggerCategory.Condition, 622), AddressOf TriggeringFurreFacingNW,
"(1:622) and the triggering furre is facing NW,")

            '(1:623) and the triggering furre is facing SE,
            Add(New Trigger(TriggerCategory.Condition, 623), AddressOf TriggeringFurreFacingSE,
"(1:623) and the triggering furre is facing SE,")

            '(1:624) and the triggering furre is facing SW,
            Add(New Trigger(TriggerCategory.Condition, 624), AddressOf TriggeringFurreFacingSW,
"(1:624) and the triggering furre is facing SW,")

            '(1:625) and the furre named {...} is standing.
            Add(New Trigger(TriggerCategory.Condition, 625), AddressOf FurreNamedStanding,
"(1:625) and the furre named {...} is standing.")

            '(1:626) and the furre named {...} is sitting.
            Add(New Trigger(TriggerCategory.Condition, 626), AddressOf FurreNamedSitting,
"(1:626) and the furre named {...} is sitting.")

            '(1:627) and the furre named {...} is laying.
            Add(New Trigger(TriggerCategory.Condition, 627), AddressOf FurreNamedLaying,
"(1:627) and the furre named {...} is laying.")

            '(1:628) and the furre named {...} is facing NE,
            Add(New Trigger(TriggerCategory.Condition, 628), AddressOf FurreNamedFacingNE,
"(1:628) and the furre named {...} is facing NE,")

            '(1:629) and the furre named {...} is facing NW,
            Add(New Trigger(TriggerCategory.Condition, 629), AddressOf FurreNamedFacingNW,
"(1:629) and the furre named {...} is facing NW,")

            '(1:630) and the furre named {...} is facing SE,
            Add(New Trigger(TriggerCategory.Condition, 630), AddressOf FurreNamedFacingSE,
"(1:630) and the furre named {...} is facing SE,")

            '(1:631) and the furre named {...} is facing SW,
            Add(New Trigger(TriggerCategory.Condition, 631), AddressOf FurreNamedFacingSW,
"(1:631) and the furre named {...} is facing SW,")

            '(5:600) set variable %Variable to the Triggering furre's description.
            Add(New Trigger(TriggerCategory.Effect, 600), AddressOf TriggeringFurreDescVar,
"(5:600) set variable %Variable to the Triggering furre's description.")

            '(5:601) set variable %Variable to the triggering furre's gender.
            Add(New Trigger(TriggerCategory.Effect, 601), AddressOf TriggeringFurreGenderVar,
"(5:601) set variable %Variable to the triggering furre's gender.")

            '(5:602) set variable %Variable to the triggering furre's species.
            Add(New Trigger(TriggerCategory.Effect, 602), AddressOf TriggeringFurreSpeciesVar,
"(5:602) set variable %Variable to the triggering furre's species.")

            '(5:604) set variable %Variable to the triggering furre's colors.
            Add(New Trigger(TriggerCategory.Effect, 604), AddressOf TriggeringFurreColorsVar,
"(5:604) set variable %Variable to the triggering furre's colors.")

            '(5:605) set variable %Variable to the furre named {...}'s gender if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 605), AddressOf FurreNamedGenderVar,
"(5:605) set variable %Variable to the furre named {...}'s gender if they are in the dream.")

            '(5:606) set variable %Variable to the furre named {...}'s species, if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 606), AddressOf FurreNamedSpeciesVar,
"(5:606) set variable %Variable to the furre named {...}'s species, if they are in the dream.")

            '(5:607) set variable %Variable to the furred named {...}'s description, if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 607), AddressOf FurreNamedDescVar,
"(5:607) set variable %Variable to the furred named {...}'s description, if they are in the dream.")

            '(5:608) set variable %Variable to the furre named {...}'s colors, if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 608), AddressOf FurreNamedColorsVar,
"(5:608) set variable %Variable to the furre named {...}'s colors, if they are in the dream.")

            '(5:609) set %Variable to the wings type the triggering furre is wearing.
            Add(New Trigger(TriggerCategory.Effect, 609), AddressOf TriggeringFurreWingsVar,
"(5:609) set %Variable to the wings type the triggering furre is wearing.")

            '(5:610) set %Variable to the wings type the furre named {...} is wearing.
            Add(New Trigger(TriggerCategory.Effect, 610), AddressOf FurreNamedWingsVar,
"(5:610) set %Variable to the wings type the furre named {...} is wearing.")

        End Sub

#End Region

#Region "Public Methods"

        '(1:600) and triggering furre's description contains {...}
        Function DescContains(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Pattern As String = reader.ReadString
                If tPlayer.Desc = Nothing Then Throw New Exception("Description not found. Try looking at the furre first")
                Return tPlayer.Desc.Contains(Pattern)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:602) and the furre named {...} description contains {...}
        Function DescContainsFurreNamed(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString()
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Pattern As String = reader.ReadString
                If Target.Desc = Nothing Then Throw New Exception("Description not found. Try looking at the furre first")
                Return Target.Desc.Contains(Pattern)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:608) set variable %Variable to the furre named {...} colors, if they are in the dream.
        Function FurreNamedColorsVar(reader As TriggerReader) As Boolean
            Try

                Dim Var As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim ColorString As String = Target.Color
                If Target.Color.Length < 10 Then
                    Throw New Exception("Color string not found. Try looking at the furre first.")
                ElseIf Target.Color.Length = 10 Then
                    ColorString = Target.Color & ConvertToBase220(Target.Gender) & ConvertToBase220(Target.Species) & ConvertToBase220(Target.Special)
                Else
                    ColorString = Target.Color
                End If
                Var.Value = ColorString
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:607) set variable %Variable to the furred named {...} description, if they are in the dream.
        Function FurreNamedDescVar(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                If Target.Desc = Nothing Then Throw New Exception("Description not found, Try looking at the furre first.")
                Var.Value = Target.Desc
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is facing NE,
        Function FurreNamedFacingNE(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.dir
                    Case 3
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is facing NW,
        Function FurreNamedFacingNW(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.dir
                    Case 2
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is facing SE,
        Function FurreNamedFacingSE(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.dir
                    Case 1
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is facing SW,
        Function FurreNamedFacingSW(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.dir
                    Case 0
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:609) and the furre named {...} is female,
        Function FurreNamedFemale(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return Target.Gender = 0
                    Case 1
                        If Target.FrameInfo.gender = -1 Then
                            If Target.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return Target.Gender = 0
                        Else
                            Return Target.FrameInfo.gender = 0
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:605) set variable %Variable to the furre named {...} gender if they are in the dream.
        Function FurreNamedGenderVar(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Var.Value = Target.Gender
                    Case 1
                        If Target.FrameInfo.gender = -1 Then
                            If Target.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Var.Value = Convert.ToDouble(Target.Gender)
                        Else
                            Var.Value = Convert.ToDouble(Target.FrameInfo.gender)
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is laying.
        Function FurreNamedLaying(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.pose
                    Case 4
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:608) and the furre named {...} is male,
        Function FurreNamedMale(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return Target.Gender = 1
                    Case 1
                        If Target.FrameInfo.gender = -1 Then
                            If Target.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return Target.Gender = 1
                        Else
                            Return Target.FrameInfo.gender = 1
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:617) and the furre named {...}  doesn't wings of type #
        Function FurreNamedNoWings(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Return Target.Wings <> Spec
                    Case 1
                        If Target.FrameInfo.Wings = -1 Then
                            If Target.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Return Target.Wings <> Spec
                        Else
                            Return Target.FrameInfo.Wings <> Spec
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is sitting.
        Function FurreNamedSitting(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.pose
                    Case 0
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:613) and the furre named {...} is Species # (please see http://www.furcadia.com/dsparams/ for info)
        Function FurreNamedSpecies(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Species not found. Try looking at the furre first")
                    Case 0
                        Return Target.DSSpecies = Spec
                    Case 1
                        If Target.FrameInfo.DS_Number = -1 Then
                            If Target.DSSpecies = -1 Then Throw New Exception("Species not found, Try looking at the furre first")
                            Return Target.DSSpecies = Spec
                        Else
                            Return Target.FrameInfo.DS_Number = Spec
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:606) set variable %Variable to the furre named {...} species, if they are in the dream.
        Function FurreNamedSpeciesVar(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Species not found. Try looking at the furre first")
                    Case 0
                        Var.Value = Convert.ToDouble(Target.DSSpecies)
                    Case 1
                        If Target.FrameInfo.DS_Number = -1 Then
                            If Target.DSSpecies = -1 Then Throw New Exception("Species not found, Try looking at the furre first")
                            Var.Value = Convert.ToDouble(Target.DSSpecies)
                        Else
                            Var.Value = Convert.ToDouble(Target.FrameInfo.DS_Number)
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is standing.
        Function FurreNamedStanding(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.FrameInfo.pose
                    Case 1 To 3
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:610) and the furre Named {...} is unspecified,
        Function FurreNamedUnSpecified(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return Target.Gender = 2
                    Case 1
                        If Target.FrameInfo.gender = -1 Then
                            If Target.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return Target.Gender = 2
                        Else
                            Return Target.FrameInfo.gender = 2
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
                Return False
            End Try
        End Function

        '(1:616) and the furre named {...} has wings of type #
        Function FurreNamedWings(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Return Target.Wings = Spec
                    Case 1
                        If Target.FrameInfo.Wings = -1 Then
                            If Target.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Return Target.Wings = Spec
                        Else
                            Return Target.FrameInfo.Wings = Spec
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:610) set %Variable to the wings type the furre named {...} is wearing.
        Function FurreNamedWingsVar(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Var.Value = Convert.ToDouble(Target.Wings)
                    Case 1
                        If Target.FrameInfo.Wings = -1 Then
                            If Target.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Var.Value = Convert.ToDouble(Target.Wings)
                        Else
                            Var.Value = Convert.ToDouble(Target.FrameInfo.Wings)
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:601) and triggering furre's description does not contain {...}
        Function NotDescContains(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Pattern As String = reader.ReadString
                If tPlayer.Desc = Nothing Then Throw New Exception("Description not found. Try looking at the furre first")
                Return Not tPlayer.Desc.Contains(Pattern)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(1:603) and the furre named {...} description does not contain {...}
        Function NotDescContainsFurreNamed(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim name As String = reader.ReadString()
                Dim Target As FURRE = FurcadiaSession.NameToFurre(name, False)
                Dim Pattern As String = reader.ReadString
                If Target.Desc = Nothing Then Throw New Exception("Description not found. Try looking at the furre first")
                Return Not tPlayer.Desc.Contains(Pattern)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(5:604) set variable %Variable to the triggering furre's colors.
        Function TriggeringFurreColorsVar(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Var As Variable = reader.ReadVariable(True)
                Dim ColorString As String = tPlayer.Color
                If tPlayer.Color.Length < 10 Then
                    Throw New Exception("Color string not found. Try looking at the furre first.")
                ElseIf tPlayer.Color.Length = 10 Then
                    ColorString = tPlayer.Color & ConvertToBase220(tPlayer.Gender) & ConvertToBase220(tPlayer.Species) & ConvertToBase220(tPlayer.Special)
                Else
                    ColorString = tPlayer.Color
                End If

                Var.Value = ColorString
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:600) set variable %Variable to the Triggering furre's description.
        Function TriggeringFurreDescVar(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Var As Variable = reader.ReadVariable(True)
                If tPlayer.Desc = Nothing Then Throw New Exception("Description not found, Try looking at the furre first.")
                Var.Value = tPlayer.Desc
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is facing NE,
        Function TriggeringFurreFacingNE(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.dir
                    Case 3
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is facing NW,
        Function TriggeringFurreFacingNW(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.dir
                    Case 2
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is facing SE,
        Function TriggeringFurreFacingSE(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.dir
                    Case 1
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is facing SW,
        Function TriggeringFurreFacingSW(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.dir
                    Case 0
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:605) and the triggering furre is female,
        Function TriggeringFurreFemale(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return tPlayer.Gender = 0
                    Case 1
                        If tPlayer.FrameInfo.gender = -1 Then
                            If tPlayer.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return tPlayer.Gender = 0
                        Else
                            Return tPlayer.FrameInfo.gender = 0
                        End If
                End Select

            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function

        '(5:601 set variable %Variable to the triggering furre's gender.
        Function TriggeringFurreGenderVar(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Var As Variable = reader.ReadVariable(True)
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        If tPlayer.Gender = -1 Then Throw New Exception("Gender not found. Try looking at the furre first")
                        Var.Value = Convert.ToDouble(tPlayer.Gender)
                    Case 1
                        If tPlayer.FrameInfo.gender = -1 Then
                            If tPlayer.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Var.Value = Convert.ToDouble(tPlayer.Gender)
                        Else
                            Var.Value = Convert.ToDouble(tPlayer.FrameInfo.gender)
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is laying.
        Function TriggeringFurreLaying(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.pose
                    Case 4
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:604) and the triggering furre is male,
        Function TriggeringFurreMale(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return tPlayer.Gender = 1
                    Case 1
                        If tPlayer.FrameInfo.gender = -1 Then
                            If tPlayer.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return tPlayer.Gender = 1
                        Else
                            Return tPlayer.FrameInfo.gender = 1
                        End If
                End Select

            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
            Return False
        End Function
        '(1:615) and the triggering furre doesn't wings of type #
        Function TriggeringFurreNoWings(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Spec As Double = reader.ReadNumber()
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Return tPlayer.Wings <> Spec
                    Case 1
                        If tPlayer.FrameInfo.Wings = -1 Then
                            If tPlayer.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Return tPlayer.Wings <> Spec
                        Else
                            Return tPlayer.FrameInfo.Wings <> Spec
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is sitting.
        Function TriggeringFurreSitting(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.pose
                    Case 0
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)
        Function TriggeringFurreSpecies(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Spec As Double = reader.ReadNumber()
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Species type not found. Try looking at the furre first")
                    Case 0
                        Return tPlayer.DSSpecies = Spec
                    Case 1
                        If tPlayer.FrameInfo.DS_Number = -1 Then
                            If tPlayer.DSSpecies = -1 Then Throw New Exception("Species type not found, Try looking at the furre first")
                            Return tPlayer.DSSpecies = Spec
                        Else
                            Return tPlayer.FrameInfo.DS_Number = Spec
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:602) set variable %Variable to the triggering furre's species.
        Function TriggeringFurreSpeciesVar(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Var As Variable = reader.ReadVariable(True)
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Species not found. Try looking at the furre first")
                    Case 0
                        Var.Value = Convert.ToDouble(tPlayer.DSSpecies)
                    Case 1
                        If tPlayer.FrameInfo.DS_Number = -1 Then
                            If tPlayer.DSSpecies = -1 Then Throw New Exception("Species not found, Try looking at the furre first")
                            Var.Value = Convert.ToDouble(tPlayer.DSSpecies)
                        Else
                            Var.Value = Convert.ToDouble(tPlayer.FrameInfo.DS_Number)
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is standing.
        Function TriggeringFurreStanding(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.FrameInfo.pose
                    Case 1 To 3
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:606) and the triggering furre is unspecified,
        Function TriggeringFurreUnspecified(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return tPlayer.Gender = 2
                    Case 1
                        If tPlayer.FrameInfo.gender = -1 Then
                            If tPlayer.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return tPlayer.Gender = 2
                        Else
                            Return tPlayer.FrameInfo.gender = 2
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(1:614) and the triggering furre has wings of type #
        Function TriggeringFurreWings(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Spec As Double = reader.ReadNumber()
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Return tPlayer.Wings = Spec
                    Case 1
                        If tPlayer.FrameInfo.Wings = -1 Then
                            If tPlayer.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Return tPlayer.Wings = Spec
                        Else
                            Return tPlayer.FrameInfo.Wings = Spec
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function
        '(5:609) set %Variable to the wings type the triggering furre is wearing.
        Function TriggeringFurreWingsVar(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Var As Variable = reader.ReadVariable(True)
                Select Case tPlayer.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Var.Value = Convert.ToDouble(tPlayer.Wings)
                    Case 1
                        If tPlayer.FrameInfo.Wings = -1 Then
                            If tPlayer.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Var.Value = Convert.ToDouble(tPlayer.Wings)
                        Else
                            Var.Value = Convert.ToDouble(tPlayer.FrameInfo.Wings)
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

#End Region

    End Class
End Namespace