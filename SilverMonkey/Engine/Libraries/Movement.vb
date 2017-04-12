Imports System.Text.RegularExpressions
Imports Furcadia.Net

Imports MonkeyCore
Imports Monkeyspeak

Namespace Engine.Libraries
    Public Class Movement
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        Private Const RGEX_Mov_Steps As String = "(nw|ne|sw|se|1|3|7|9)"
#End Region

#Region "Public Constructors"

        Public Sub New()
            MyBase.New()
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

                    Select Case tPlayer.FrameInfo.dir
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

        '(5:621) set %Variable to the direction number the furre names {...}, is facing/ moved in.
        Function FurreNamedFaceDirectionNumber(reader As TriggerReader) As Boolean
            Try
                Dim Dir As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
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

                    Select Case tPlayer.FrameInfo.dir
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

        '(1:635) and the furre named {...} moved from (x,y),
        Function FurreNamedMoveFrom(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
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
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
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
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
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

        '(5:618) set variable %Variable to the X coordinate where the furre named {...} moved into/is at.
        Function FurreNamedSetCordX(reader As TriggerReader) As Boolean
            Try
                Dim Cord As Variable = reader.ReadVariable(True)
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
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
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
                Cord.Value = tPlayer.Y
                Return True
            Catch ex As Exception
                LogError(reader, ex)
                Return False
            End Try
        End Function

        '(1:636) and the furre named {...} tried to move but stood still.
        Function FurreNamedStoodStill(reader As TriggerReader) As Boolean
            Try
                Dim name As String = reader.ReadString
                Dim tPlayer As FURRE = FurcadiaSession.NameToFurre(name, False)
                Return tPlayer.SourceX = tPlayer.X AndAlso tPlayer.SourceY = tPlayer.Y
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

#Region "Helper Functions"

#End Region

    End Class
End Namespace