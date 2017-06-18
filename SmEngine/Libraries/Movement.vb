﻿Imports System.Text.RegularExpressions
Imports Furcadia.Net.Dream
Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Furcadia Movement MonkeySpeak
    '''<para>
    ''' Processing of Furre Triggers around the map, Interact with avatar
    ''' settings such as thier map location, Type of Wings, Which avatar a
    ''' given furre has... The Avatar colors ect...
    ''' </para>
    ''' </summary>
    Public Class Movement
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Private Const RGEX_Mov_Steps As String = "(nw|ne|sw|se|1|3|7|9)"

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)

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
            '(0:601) When a furre moves,
            Add(TriggerCategory.Cause, 601,
            Function()
                Return True
            End Function, "(0:601) When a furre moves,")
            '(0:602) when a furre moves into (x,y),
            Add(TriggerCategory.Cause, 602, AddressOf MoveInto, "(0:602) when a furre moves into (x,y),")

            '(1:900) and the triggering furre moved into/is standing at (x,y),
            Add(New Trigger(TriggerCategory.Condition, 632), AddressOf MoveInto, "(1:632) and the triggering furre moved into/is standing at (x,y)")

            '(1:633) and the furre named {...} moved into/is standing at (x,y),
            Add(New Trigger(TriggerCategory.Condition, 633), AddressOf FurreNamedMoveInto, "(1:633) and the furre named {...} moved into/is standing at (x,y),")

            '(1:634) and the triggering furre moved from (x,y),
            Add(New Trigger(TriggerCategory.Condition, 634), AddressOf MoveFrom, "(1:634) and the triggering furre moved from (x,y),")

            '(1:635) and the furre named {...} moved from (x,y),
            Add(New Trigger(TriggerCategory.Condition, 635), AddressOf FurreNamedMoveFrom, "(1:635) and the furre named {...} moved from (x,y),")

            '(1:636) and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Condition, 636), AddressOf MoveIntoDirection, "(1:636) and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(1:637) and the furre named {...}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Condition, 637), AddressOf FurreNamedMoveIntoDirection, "(1:637) and the furre named {...}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(1:638) and the triggering furre tried to move but stood still.
            Add(New Trigger(TriggerCategory.Condition, 638), AddressOf StoodStill, "(1:638) and the triggering furre tried to move but stood still.")

            '(1:639) and the furre named {...} tried to move but stood still.
            Add(New Trigger(TriggerCategory.Condition, 639), AddressOf FurreNamedMoveIntoDirection, "(1:639) and the furre named {...} tried to move but stood still..")

            '(5:613) move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Effect, 613), AddressOf MoveBot, "(5:613) move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:614) turn the bot clock-wise one space.
            Add(New Trigger(TriggerCategory.Effect, 614), AddressOf TurnCW, "(5:614) turn the bot clock-wise one space.")

            '(5:615) turn the bot counter-clockwise one space.
            Add(New Trigger(TriggerCategory.Effect, 615), AddressOf TurnCCW, "(5:615) turn the bot counter-clockwise one space.")

            '(5:616) set variable %Variable to the X coordinate where the triggering furre moved into/is at.
            Add(New Trigger(TriggerCategory.Effect, 616), AddressOf SetCordX, "(5:616) set variable %Variable to the X coordinate where the triggering furre moved into/is at.")

            '(5:617) set variable %Variable to the Y coordinate where the triggering furre moved into/is at.
            Add(New Trigger(TriggerCategory.Effect, 617), AddressOf SetCordY, "(5:617) set variable %Variable to the Y coordinate where the triggering furre moved into/is at.")

            '(5:618) set variable %Variable to the X coordinate where the furre named {...} moved into/is at.
            Add(New Trigger(TriggerCategory.Effect, 618), AddressOf FurreNamedSetCordX, "(5:618) set variable %Variable to the X coordinate where the furre named {...} moved into/is at.")

            '(5:619) set variable %Variable to the Y coordinate where the furre named {...} moved into/is at.
            Add(New Trigger(TriggerCategory.Effect, 619), AddressOf FurreNamedSetCordY, "(5:619) set variable %Variable to the Y coordinate where the furre named {...} moved into/is at.")

            '(5:620) set %Variable to the direction number the triggering furre is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Effect, 620), AddressOf FaceDirectionNumber, "(5:620) set %Variable to the direction number the triggering furre is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:621) set %Variable to the direction number the furre names {...}, is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Effect, 621), AddressOf FurreNamedFaceDirectionNumber, "(5:621) set %Variable to the direction number the furre names {...}, is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:622) make the bot sit down.
            Add(New Trigger(TriggerCategory.Effect, 622), AddressOf BotSit, "(5:622) make the bot sit down.")

            '(5:623) make the bot lay down.
            Add(New Trigger(TriggerCategory.Effect, 623), AddressOf BotLie, "(5:623) make the bot lay down.")

            '(5:624) make the bot stand up.
            Add(New Trigger(TriggerCategory.Effect, 624), AddressOf BotStand, "(5:624) make the bot stand up.")

            '(5:625) Move the bot  in this sequence {...} (one, sw, three, se, seven, nw, nine, or ne)
            Add(New Trigger(TriggerCategory.Effect, 625), AddressOf BotMoveSequence, "(5:625) Move the bot  in this sequence {...} (one, sw, three, se, seven, nw, nine, or ne)")
        End Sub

#End Region

#Region "Public Methods"

        '(5:623) make the bot lay down
        Function BotLie(reader As TriggerReader) As Boolean
            Try
                sendServer("`lie")
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:625) Move the bot  in this sequence {...} (one, sw, three, se, seven, nw, nine, or ne)
        Function BotMoveSequence(reader As TriggerReader) As Boolean
            Try
                Dim directions As String = reader.ReadString
                Dim r As New Regex(RGEX_Mov_Steps, RegexOptions.IgnoreCase)
                Dim m As MatchCollection = r.Matches(directions)
                For Each n As Match In m
                    If n.Value.ToLower = "ne" Then
                        sendServer("`m9")
                    ElseIf n.Value.ToLower = "se" Then
                        sendServer("`m3")
                    ElseIf n.Value.ToLower = "nw" Then
                        sendServer("`m7")
                    ElseIf n.Value.ToLower = "sw" Then
                        sendServer("`m1")
                    Else
                        sendServer("`m" + n.Value)
                    End If
                Next

                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:622) make the bot sit down
        Function BotSit(reader As TriggerReader) As Boolean
            Try
                sendServer("`sit")
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:624) make the bot stand up
        Function BotStand(reader As TriggerReader) As Boolean
            Try
                sendServer("`stand")
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Pattern As String = reader.ReadString
                If Target.Desc = Nothing Then Throw New Exception("Description not found. Try looking at the furre first")
                Return Target.Desc.Contains(Pattern)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:620) set %Variable to the direction number the triggering furre is facing/ moved in.
        '(seven = North-West, nine = North-East, three = South-East, one = South=West)
        Function FaceDirectionNumber(reader As TriggerReader) As Boolean
            Try
                Dim Dir As Variable = reader.ReadVariable(True)
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim direction As Double = 0

                If tPlayer.SourceX <> tPlayer.X Or tPlayer.SourceY <> tPlayer.Y Then

                    If (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X))) Then
                        direction = 7
                    ElseIf (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX < tPlayer.X))) Then
                        direction = 9
                    ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX < tPlayer.X))) Then
                        direction = 3
                    ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X))) Then
                        direction = 1
                    End If

                ElseIf tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y Then

                    Select Case tPlayer.Direction

                    'SW
                        Case 0
                            direction = 1
                        'SE
                        Case 1
                            direction = 3
                        'NW
                        Case 3
                            direction = 7
                        'NE
                        Case 4
                            direction = 9
                        Case Else

                    End Select

                End If
                Dir.Value = direction
                Return True
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Var.Value = Target.Color.ToString
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                If Target.Desc = Nothing Then Throw New MonkeyspeakException("Description not found, Try looking at the furre first.")
                Var.Value = Target.Desc
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:621) set %Variable to the direction number the furre names {...}, is facing/ moved in.
        Function FurreNamedFaceDirectionNumber(reader As TriggerReader) As Boolean
            Try
                Dim Dir As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim direction As Double = 0

                If tPlayer.SourceX <> tPlayer.X Or tPlayer.SourceY <> tPlayer.Y Then

                    If (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X))) Then
                        direction = 7
                    ElseIf (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX < tPlayer.X))) Then
                        direction = 9
                    ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX < tPlayer.X))) Then
                        direction = 3
                    ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X))) Then
                        direction = 1
                    End If

                ElseIf tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y Then

                    Select Case tPlayer.Direction
                    'SW
                        Case 0
                            direction = 1
                        'SE
                        Case 1
                            direction = 3
                        'NW
                        Case 3
                            direction = 7
                        'NE
                        Case 4
                            direction = 9
                        Case Else

                    End Select

                End If
                Dir.Value = direction
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
                Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Direction
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Direction
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Direction
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Direction
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0 Or 1
                        Return Target.Color.Gender = 0

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0 Or 1
                        Var.Value = Target.Color.Gender

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Pose
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return Target.Color.Gender = 1
                    Case 1
                        If Target.Color.Gender = -1 Then
                            If Target.Color.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return Target.Color.Gender = 1
                        Else
                            Return Target.Color.Gender = 1
                        End If
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:635) and the furre named {...} moved from (x,y),
        Function FurreNamedMoveFrom(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim X As Double = ReadVariableOrNumber(reader, False)
                Dim Y As Double = ReadVariableOrNumber(reader, False)
                Return tPlayer.SourceX = Convert.ToUInt32(X) AndAlso tPlayer.SourceY = Convert.ToUInt32(Y)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:901) and the furre named {...} moved into/is standing at (x,y),
        Function FurreNamedMoveInto(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim X As Double = ReadVariableOrNumber(reader, False)
                Dim Y As Double = ReadVariableOrNumber(reader, False)
                Return tPlayer.X = Convert.ToUInt32(X) AndAlso tPlayer.Y = Convert.ToUInt32(Y)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:637) and the furre named {...}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
        Function FurreNamedMoveIntoDirection(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Dir As Double = ReadVariableOrNumber(reader, False)
                Dim Direction As Double = 0
                If tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y Then
                    Return False
                End If
                If (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X))) Then
                    Direction = 7
                ElseIf (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX < tPlayer.X))) Then
                    Direction = 9
                ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX < tPlayer.X))) Then
                    Direction = 3
                ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(tPlayer.SourceY) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(tPlayer.SourceY) And (tPlayer.SourceX = tPlayer.X))) Then
                    Direction = 1
                End If
                Return Direction = Dir
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:617) and the furre named {...}  doesn't wings of type #
        Function FurreNamedNoWings(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0 Or 1
                        Return Target.Color.Wings <> Spec

                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:618) set variable %Variable to the X coordinate where the furre named {...} moved into/is at.
        Function FurreNamedSetCordX(reader As TriggerReader) As Boolean
            Try
                Dim Cord As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Cord.Value = tPlayer.X
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:619) set variable %Variable to the Y coordinate where the furre named {...} moved into/is at.
        Function FurreNamedSetCordY(reader As TriggerReader) As Boolean
            Try
                Dim Cord As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Cord.Value = tPlayer.Y
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the furre named {...} is sitting.
        Function FurreNamedSitting(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Pose
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Species not found. Try looking at the furre first")
                    Case 0 Or 1
                        Return Target.Color.Species = Spec

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Species not found. Try looking at the furre first")
                    Case 0 Or 1
                        Var.Value = Target.Color.Species

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.Pose
                    Case 1 To 3
                        Return True
                End Select
                Return False
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:636) and the furre named {...} tried to move but stood still.
        Function FurreNamedStoodStill(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Return tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:610) and the furre Named {...} is unspecified,
        Function FurreNamedUnSpecified(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0 Or 1

                        Return Target.Color.Gender = 2

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Spec As Double = reader.ReadNumber()
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")

                    Case 1 Or 0

                        Return Target.Color.Wings = Spec

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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Select Case Target.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")

                    Case 1 Or 0

                        Var.Value = Target.Color.Wings

                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:613) move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South=West)
        Function MoveBot(reader As TriggerReader) As Boolean
            Try
                Dim Dir As Double = ReadVariableOrNumber(reader, False)
                Select Case Dir
                    Case 7 Or 9 Or 3 Or 1
                        sendServer("`m" + Dir.ToString)
                    Case Else
                        Throw New Exception("Directions must be in the form of  7, 9, 3, or 1")
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:634) and the triggering furre moved from (x,y),
        Function MoveFrom(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim X As Double = ReadVariableOrNumber(reader, False)
                Dim Y As Double = ReadVariableOrNumber(reader, False)
                Return tPlayer.SourceX = Convert.ToUInt32(X) AndAlso tPlayer.SourceY = Convert.ToUInt32(Y)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(0:901) when a furre moves into (x,y),
        '(1:900) and the triggering furre moved into/is standing at (x,y),
        Function MoveInto(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim X As Double = ReadVariableOrNumber(reader, False)
                Dim Y As Double = ReadVariableOrNumber(reader, False)
                Return tPlayer.X = Convert.ToUInt32(X) AndAlso tPlayer.Y = Convert.ToUInt32(Y)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:636) and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
        Function MoveIntoDirection(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Dim Dir As Double = ReadVariableOrNumber(reader, False)
                Dim Direction As Double = 0
                If tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y Then
                    Return False
                End If
                If (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX = tPlayer.X))) Then
                    Direction = 7
                ElseIf (tPlayer.SourceY > tPlayer.Y) And ((IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX < tPlayer.X))) Then
                    Direction = 9
                ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX = tPlayer.X)) Or (Not IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX < tPlayer.X))) Then
                    Direction = 3
                ElseIf (tPlayer.SourceY < tPlayer.Y) And ((IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX > tPlayer.X)) Or (Not IsOdd(CInt(tPlayer.SourceY)) And (tPlayer.SourceX = tPlayer.X))) Then
                    Direction = 1
                End If
                Return Direction = Dir
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
                Dim Target As FURRE = FurcadiaSession.Dream.FurreList.GerFurreByName(name)
                Dim Pattern As String = reader.ReadString
                If Target.Desc = Nothing Then Throw New Exception("Description not found. Try looking at the furre first")
                Return Not tPlayer.Desc.Contains(Pattern)
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:616) set variable %Variable to the X coordinate where the triggering furre moved into/is at.
        Function SetCordX(reader As TriggerReader) As Boolean
            Try
                Dim Cord As Variable = reader.ReadVariable(True)
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Cord.Value = tPlayer.X
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:617) set variable %Variable to the Y coordinate where the triggering furre moved into/is at.
        Function SetCordY(reader As TriggerReader) As Boolean
            Try
                Dim Cord As Variable = reader.ReadVariable(True)
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Cord.Value = tPlayer.Y
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:638) and the triggering furre tried to move but stood still.
        Function StoodStill(reader As TriggerReader) As Boolean
            Try
                Dim tPlayer As FURRE = FurcadiaSession.Player
                Return tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:604) set variable %Variable to the triggering furre's colors.
        Function TriggeringFurreColorsVar(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable(True)
                Var.Value = FurcadiaSession.Player.Color.ToString
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:600) set variable %Variable to the Triggering furre's description.
        Function TriggeringFurreDescVar(reader As TriggerReader) As Boolean
            Try
                Dim Var As Variable = reader.ReadVariable(True)
                If FurcadiaSession.Player.Desc = Nothing Then Throw New Exception("Description not found, Try looking at the furre first.")
                Var.Value = FurcadiaSession.Player.Desc
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:x) and the triggering furre is facing NE,
        Function TriggeringFurreFacingNE(reader As TriggerReader) As Boolean
            Try

                Select Case Player.Direction
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

                Select Case Player.Direction
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
                Select Case Player.Direction
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

                Select Case Player.Direction
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

                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return FurcadiaSession.Player.Color.Gender = 0
                    Case 1
                        If FurcadiaSession.Player.Color.Gender = -1 Then
                            If FurcadiaSession.Player.Color.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return FurcadiaSession.Player.Color.Gender = 0
                        Else
                            Return FurcadiaSession.Player.Color.Gender = 0
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

                Dim Var As Variable = reader.ReadVariable(True)
                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        If FurcadiaSession.Player.Color.Gender = -1 Then Throw New Exception("Gender not found. Try looking at the furre first")
                        Var.Value = FurcadiaSession.Player.Color.Gender
                    Case 1
                        If FurcadiaSession.Player.Color.Gender = -1 Then
                            If FurcadiaSession.Player.Color.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Var.Value = FurcadiaSession.Player.Color.Gender
                        Else
                            Var.Value = FurcadiaSession.Player.Color.Gender
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

                Select Case Player.Pose
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

                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return FurcadiaSession.Player.Color.Gender = 1
                    Case 1
                        If FurcadiaSession.Player.Color.Gender = -1 Then
                            If FurcadiaSession.Player.Color.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return FurcadiaSession.Player.Color.Gender = 1
                        Else
                            Return FurcadiaSession.Player.Color.Gender = 1
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

                Dim Spec As Double = reader.ReadNumber()
                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Return FurcadiaSession.Player.Color.Wings <> Spec
                    Case 1
                        If FurcadiaSession.Player.Color.Wings = -1 Then
                            If FurcadiaSession.Player.Color.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Return FurcadiaSession.Player.Color.Wings <> Spec
                        Else
                            Return FurcadiaSession.Player.Color.Wings <> Spec
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
                Select Case Player.Pose
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
                        Return FurcadiaSession.Player.Color.Species = Spec
                    Case 1

                        Return FurcadiaSession.Player.Color.Species = Spec

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
                Dim Var As Variable = reader.ReadVariable(True)
                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Species not found. Try looking at the furre first")
                    Case 0 Or 1
                        Var.Value = FurcadiaSession.Player.Color.Species

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
                Select Case Player.Pose
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

                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Gender not found. Try looking at the furre first")
                    Case 0
                        Return FurcadiaSession.Player.Color.Gender = 2
                    Case 1
                        If FurcadiaSession.Player.Color.Gender = -1 Then
                            If FurcadiaSession.Player.Color.Gender = -1 Then Throw New Exception("Gender not found, Try looking at the furre first")
                            Return FurcadiaSession.Player.Color.Gender = 2
                        Else
                            Return FurcadiaSession.Player.Color.Gender = 2
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

                Dim Spec As Double = reader.ReadNumber()
                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Return FurcadiaSession.Player.Color.Wings = Spec
                    Case 1
                        If FurcadiaSession.Player.Color.Wings = -1 Then
                            If FurcadiaSession.Player.Color.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Return FurcadiaSession.Player.Color.Wings = Spec
                        Else
                            Return FurcadiaSession.Player.Color.Wings = Spec
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

                Dim Var As Variable = reader.ReadVariable(True)
                Select Case FurcadiaSession.Player.LastStat
                    Case -1
                        Throw New Exception("Wings type not found. Try looking at the furre first")
                    Case 0
                        Var.Value = FurcadiaSession.Player.Color.Wings
                    Case 1
                        If FurcadiaSession.Player.Color.Wings = -1 Then
                            If FurcadiaSession.Player.Color.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                            Var.Value = FurcadiaSession.Player.Color.Wings
                        Else
                            Var.Value = FurcadiaSession.Player.Color.Wings
                        End If
                End Select
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:615) turn the bot counter-clockwise one space.
        Function TurnCCW(reader As TriggerReader) As Boolean
            Try
                sendServer("`<")
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(5:614) turn the bot clock-wise one space.
        Function TurnCW(reader As TriggerReader) As Boolean
            Try
                sendServer("`>")
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace