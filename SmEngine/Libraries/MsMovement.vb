Imports System.Text.RegularExpressions
Imports Furcadia.Net.Dream
Imports Monkeyspeak
Imports SilverMonkeyEngine.MsLibHelper

Namespace Engine.Libraries

    ''' <summary>
    ''' <para>
    ''' Causes: (0:600) -
    ''' </para>
    ''' <para>
    ''' Conditions: (1:600) - (1:631)
    ''' </para>
    ''' <para>
    ''' Effects: (5:600) - (5:625)
    ''' </para>
    ''' Furcadia Movement MonkeySpeak
    '''<para>
    ''' Processing of Furre Triggers around the map, Interact with avatar
    ''' settings such as thier map location, Type of Wings, Which avatar a
    ''' given furre has.. The Avatar colors ect..
    ''' </para>
    ''' <para>
    ''' Note: A Furre object only contains a description after the bot sends a look command to the server.
    ''' </para>
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates
    ''' <para>
    ''' (0:600) When the bot sees a furre description,
    ''' </para>
    ''' <para>
    ''' (0:601) When a furre moves,
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class MsMovement
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        ''' <summary>
        ''' Regex syntax for Fircadia isometric directions
        ''' </summary>
        Public Const RGEX_Mov_Steps As String = "(nw|ne|sw|se|1|3|7|9)"

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)
        End Sub

        Public Overrides Sub Initialize(ParamArray args() As Object)
            '(0:600) When the bot reads a description.
            Add(TriggerCategory.Cause, 600, Function() True, " When the bot sees a furre description,")

            '(1:600) and triggering furre's description contains {..}
            Add(TriggerCategory.Condition, 600, AddressOf DescContains,
                " and triggering furre's description contains {..}")

            '(1:601) and triggering furre's description does not contain {..}
            Add(TriggerCategory.Condition, 601, AddressOf NotDescContains,
                " and triggering furre's description does not contain {..}")

            '(1:602) and the furre named {..} description contains {..}
            Add(TriggerCategory.Condition, 602, AddressOf FurreNamedDescContains,
                " and the furre named {..} description contains {..}if they are in the dream,")

            '(1:603) and the furre named {..} description does not contain {..}
            Add(TriggerCategory.Condition, 603, AddressOf NotDescContainsFurreNamed,
             " and the furre named {..} description does not contain {..} if they are in the dream")

            '(1:604) and the triggering furre is male,
            Add(TriggerCategory.Condition, 604, AddressOf TriggeringFurreMale,
                " and the triggering furre is male,")

            '(1:605) and the triggering furre is female,
            Add(TriggerCategory.Condition, 605, AddressOf TriggeringFurreFemale,
                " and the triggering furre is female,")

            '(1:606) and the triggering furre is unspecified,
            Add(TriggerCategory.Condition, 606, AddressOf TriggeringFurreUnspecified,
                " and the triggering furre is unspecified,")

            '(1:608) and the furre named {..}'s is male,
            Add(TriggerCategory.Condition, 608, AddressOf FurreNamedMale,
                " and the furre named {..}'s is male if they are in the dream,")

            '(1:609) and the furre named {..}'s is female,
            Add(TriggerCategory.Condition, 609, AddressOf FurreNamedFemale,
                " and the furre named {..}'s is female if they are in the dream,")

            '(1:609) and the furre named {..}'s is female,
            Add(TriggerCategory.Condition, 610, AddressOf FurreNamedUnSpecified,
                 " and the furre Named {..}'s is unspecified if they are in the dream,")

            'TODO: as a URL replace tag for descriptions with urls in an ini file

            '(1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(TriggerCategory.Condition, 612, AddressOf TriggeringFurreSpecies,
                " and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:613) and the furre named {..} is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(TriggerCategory.Condition, 613, AddressOf FurreNamedSpecies,
                " and the furre named {..} is Species # if they are in the dream (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:614) and the triggering furre has wings of type #,
            Add(TriggerCategory.Condition, 614, AddressOf TriggeringFurreWings,
                " and the triggering furre has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:615) and the triggering furre doesn't wings of type #,
            Add(TriggerCategory.Condition, 615, AddressOf TriggeringFurreNoWings,
                " and the triggering furre doesn't wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:616) and the furre named {..} has wings of type #,
            Add(TriggerCategory.Condition, 616, AddressOf FurreNamedWings,
                " and the furre named {..} has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:617) and the furre named {..}  doesn't wings of type #,
            Add(TriggerCategory.Condition, 617, AddressOf FurreNamedNoWings,
                " and the furre named {..}  doesn't wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:618) and the triggering furre is standing.
            Add(TriggerCategory.Condition, 618, AddressOf TriggeringFurreStanding,
                " and the triggering furre is standing.")

            '(1:619) and the triggering furre is sitting.
            Add(TriggerCategory.Condition, 619, AddressOf TriggeringFurreSitting,
                " and the triggering furre is sitting.")

            '(1:620) and the triggering furre is laying.
            Add(TriggerCategory.Condition, 620, AddressOf TriggeringFurreLaying,
                " and the triggering furre is laying.")

            '(1:621) and the triggering furre is facing NE,
            Add(TriggerCategory.Condition, 621, AddressOf TriggeringFurreFacingNE,
                " and the triggering furre is facing NE,")

            '(1:622) and the triggering furre is facing NW,
            Add(TriggerCategory.Condition, 622, AddressOf TriggeringFurreFacingNW,
                " and the triggering furre is facing NW,")

            '(1:623) and the triggering furre is facing SE,
            Add(TriggerCategory.Condition, 623, AddressOf TriggeringFurreFacingSE,
                " and the triggering furre is facing SE,")

            '(1:624) and the triggering furre is facing SW,
            Add(TriggerCategory.Condition, 624, AddressOf TriggeringFurreFacingSW,
                " and the triggering furre is facing SW,")

            '(1:625) and the furre named {..} is standing.
            Add(TriggerCategory.Condition, 625, AddressOf FurreNamedStanding,
                " and the furre named {..} is standing.")

            '(1:626) and the furre named {..} is sitting.
            Add(TriggerCategory.Condition, 626, AddressOf FurreNamedSitting,
                " and the furre named {..} is sitting.")

            '(1:627) and the furre named {..} is laying.
            Add(TriggerCategory.Condition, 627, AddressOf FurreNamedLaying,
                " and the furre named {..} is laying.")

            '(1:628) and the furre named {..} is facing NE,
            Add(TriggerCategory.Condition, 628, AddressOf FurreNamedFacingNE,
                " and the furre named {..} is facing NE,")

            '(1:629) and the furre named {..} is facing NW,
            Add(TriggerCategory.Condition, 629, AddressOf FurreNamedFacingNW,
                " and the furre named {..} is facing NW,")

            '(1:630) and the furre named {..} is facing SE,
            Add(TriggerCategory.Condition, 630, AddressOf FurreNamedFacingSE,
                " and the furre named {..} is facing SE,")

            '(1:631) and the furre named {..} is facing SW,
            Add(TriggerCategory.Condition, 631, AddressOf FurreNamedFacingSW,
                " and the furre named {..} is facing SW,")

            '(5:600) set variable %Variable to the Triggering furre's description.
            Add(TriggerCategory.Effect, 600, AddressOf TriggeringFurreDescVar,
                " set variable %Variable to the Triggering furre's description.")

            '(5:601) set variable %Variable to the triggering furre's gender.
            Add(TriggerCategory.Effect, 601, AddressOf TriggeringFurreGenderVar,
                " set variable %Variable to the triggering furre's gender.")

            '(5:602) set variable %Variable to the triggering furre's species.
            Add(TriggerCategory.Effect, 602, AddressOf TriggeringFurreSpeciesVar,
                " set variable %Variable to the triggering furre's species.")

            '(5:604) set variable %Variable to the triggering furre's colors.
            Add(TriggerCategory.Effect, 604, AddressOf TriggeringFurreColorsVar,
                " set variable %Variable to the triggering furre's colors.")

            '(5:605) set variable %Variable to the furre named {..}'s gender if they are in the dream.
            Add(TriggerCategory.Effect, 605, AddressOf FurreNamedGenderVar,
                " set variable %Variable to the furre named {..}'s gender if they are in the dream.")

            '(5:606) set variable %Variable to the furre named {..}'s species, if they are in the dream.
            Add(TriggerCategory.Effect, 606, AddressOf FurreNamedSpeciesVar,
                " set variable %Variable to the furre named {..}'s species, if they are in the dream.")

            '(5:607) set variable %Variable to the furred named {..}'s description, if they are in the dream.
            Add(TriggerCategory.Effect, 607, AddressOf FurreNamedDescVar,
                    " set variable %Variable to the furred named {..}'s description, if they are in the dream.")

            '(5:608) set variable %Variable to the furre named {..}'s colors, if they are in the dream.
            Add(TriggerCategory.Effect, 608, AddressOf FurreNamedColorsVar,
                " set variable %Variable to the furre named {..}'s colors, if they are in the dream.")

            '(5:609) set %Variable to the wings type the triggering furre is wearing.
            Add(TriggerCategory.Effect, 609, AddressOf TriggeringFurreWingsVar,
                " set %Variable to the wings type the triggering furre is wearing.")

            '(5:610) set %Variable to the wings type the furre named {..} is wearing.
            Add(TriggerCategory.Effect, 610, AddressOf FurreNamedWingsVar,
                " set %Variable to the wings type the furre named {..} is wearing.")

            '(0:601) When a furre moves,
            Add(TriggerCategory.Cause, 601,
          Function() True, " When a furre moves,")
            '(0:602) when a furre moves into (x,y),
            Add(TriggerCategory.Cause, 602, AddressOf MoveInto, " when a furre moves into (x,y),")

            '(1:900) and the triggering furre moved into/is standing at (x,y),
            Add(TriggerCategory.Condition, 632, AddressOf MoveInto, " and the triggering furre moved into/is standing at (x,y)")

            '(1:633) and the furre named {..} moved into/is standing at (x,y),
            Add(TriggerCategory.Condition, 633, AddressOf FurreNamedMoveInto, " and the furre named {..} moved into/is standing at (x,y),")

            '(1:634) and the triggering furre moved from (x,y),
            Add(TriggerCategory.Condition, 634, AddressOf MoveFrom, " and the triggering furre moved from (x,y),")

            '(1:635) and the furre named {..} moved from (x,y),
            Add(TriggerCategory.Condition, 635, AddressOf FurreNamedMoveFrom, " and the furre named {..} moved from (x,y),")

            '(1:636) and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(TriggerCategory.Condition, 636, AddressOf MoveIntoDirection, " and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(1:637) and the furre named {..}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(TriggerCategory.Condition, 637, AddressOf FurreNamedMoveIntoDirection, " and the furre named {..}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(1:638) and the triggering furre tried to move but stood still.
            Add(TriggerCategory.Condition, 638, AddressOf StoodStill, " and the triggering furre tried to move but stood still.")

            '(1:639) and the furre named {..} tried to move but stood still.
            Add(TriggerCategory.Condition, 639, AddressOf FurreNamedMoveIntoDirection, " and the furre named {..} tried to move but stood still.")

            '(5:613) move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(TriggerCategory.Effect, 613, AddressOf MoveBot, " move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:614) turn the bot clock-wise one space.
            Add(TriggerCategory.Effect, 614, AddressOf TurnCW, " turn the bot clock-wise one space.")

            '(5:615) turn the bot counter-clockwise one space.
            Add(TriggerCategory.Effect, 615, AddressOf TurnCCW, " turn the bot counter-clockwise one space.")

            '(5:616) set variable %Variable to the X coordinate where the triggering furre moved into/is at.
            Add(TriggerCategory.Effect, 616, AddressOf SetCordX, " set variable %Variable to the X coordinate where the triggering furre moved into/is at.")

            '(5:617) set variable %Variable to the Y coordinate where the triggering furre moved into/is at.
            Add(TriggerCategory.Effect, 617, AddressOf SetCordY, " set variable %Variable to the Y coordinate where the triggering furre moved into/is at.")

            '(5:618) set variable %Variable to the X coordinate where the furre named {..} moved into/is at.
            Add(TriggerCategory.Effect, 618, AddressOf FurreNamedSetCordX, " set variable %Variable to the X coordinate where the furre named {..} moved into/is at.")

            '(5:619) set variable %Variable to the Y coordinate where the furre named {..} moved into/is at.
            Add(TriggerCategory.Effect, 619, AddressOf FurreNamedSetCordY, " set variable %Variable to the Y coordinate where the furre named {..} moved into/is at.")

            '(5:620) set %Variable to the direction number the triggering furre is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(TriggerCategory.Effect, 620, AddressOf FaceDirectionNumber, " set %Variable to the direction number the triggering furre is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:621) set %Variable to the direction number the furre names {..}, is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(TriggerCategory.Effect, 621, AddressOf FurreNamedFaceDirectionNumber, " set %Variable to the direction number the furre names {..}, is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:622) make the bot sit down.
            Add(TriggerCategory.Effect, 622, AddressOf BotSit, " make the bot sit down.")

            '(5:623) make the bot lay down.
            Add(TriggerCategory.Effect, 623, AddressOf BotLie, " make the bot lay down.")

            '(5:624) make the bot stand up.
            Add(TriggerCategory.Effect, 624, AddressOf BotStand, " make the bot stand up.")

            '(5:625) Move the bot  in this sequence {..} (one, sw, three, se, seven, nw, nine, or ne)
            Add(TriggerCategory.Effect, 625, AddressOf BotMoveSequence, " Move the bot  in this sequence {..} (one, sw, three, se, seven, nw, nine, or ne)")
        End Sub

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (5:623) make the bot lay down
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sucessess sending the command to the game server
        ''' </returns>
        Public Function BotLie(reader As TriggerReader) As Boolean

            Return SendServer("`lie")

        End Function

        ''' <summary>
        ''' (5:625) Move the bot in this sequence {..} (one, sw, three, se,
        ''' seven, nw, nine, or ne)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' is thrown when the arguments supplied are invalid
        ''' </exception>
        ''' <returns>
        ''' true on sending the command to the server queue
        ''' </returns>
        Public Function BotMoveSequence(reader As TriggerReader) As Boolean
            'TODO: http://bugtraq.tsprojects.org/view.php?id=55
            'Queue System?

            Dim directions = reader.ReadString
            Dim r = New Regex(RGEX_Mov_Steps, RegexOptions.Compiled)
            Dim m = r.Matches(directions)
            Dim ServerSend As Boolean
            For Each n In m
                If n.Value.ToLower = "ne" Then
                    ServerSend = SendServer("`m9")
                ElseIf n.Value.ToLower = "se" Then
                    ServerSend = SendServer("`m3")
                ElseIf n.Value.ToLower = "nw" Then
                    ServerSend = SendServer("`m7")
                ElseIf n.Value.ToLower = "sw" Then
                    ServerSend = SendServer("`m1")
                ElseIf n.Value = (1 Or 7 Or 3 Or 9) Then
                    ServerSend = SendServer("`m" + n.Value)
                Else
                    Throw New ArgumentOutOfRangeException(m.ToString)
                End If

                If Not ServerSend Then Exit For
            Next

            Return ServerSend

        End Function

        ''' <summary>
        ''' (5:622) make the bot sit down
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the commands to the game server
        ''' </returns>
        Public Function BotSit(reader As TriggerReader) As Boolean
            Return SendServer("`sit")
        End Function

        ''' <summary>
        ''' (5:624) make the bot stand up
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the commands to the game server
        ''' </returns>
        Public Function BotStand(reader As TriggerReader) As Boolean
            Return SendServer("`stand")
        End Function

        ''' <summary>
        ''' (1:600) and triggering furre's description contains {..}
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <remarks>
        ''' </remarks>
        Public Function DescContains(reader As TriggerReader) As Boolean
            If String.IsNullOrEmpty(Player.FurreDescription) Then
                Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            End If
            Return Player.FurreDescription.Contains(reader.ReadString)

        End Function

        ''' <summary>
        ''' (5:620) set <see cref="Monkeyspeak.Variable">%Variable</see> to
        ''' the direction number the triggering furre is facing/ moved in.
        ''' (seven = North-West, nine = North-East, three = South-East, one
        ''' = South=West)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Public Function FaceDirectionNumber(reader As TriggerReader) As Boolean
            Dim Dir = reader.ReadVariable(True)
            Dim tPlayer = Player
            Dim direction As Double = 0

            If tPlayer.LastPosition.X <> tPlayer.Position.X Or tPlayer.LastPosition.Y <> tPlayer.Position.Y Then

                If (tPlayer.LastPosition.Y > tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X > tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X))) Then
                    direction = 7
                ElseIf (tPlayer.LastPosition.Y > tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X < tPlayer.Position.X))) Then
                    direction = 9
                ElseIf (tPlayer.LastPosition.Y < tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X < tPlayer.Position.X))) Then
                    direction = 3
                ElseIf (tPlayer.LastPosition.Y < tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X > tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X))) Then
                    direction = 1
                End If

            ElseIf tPlayer.LastPosition.X = tPlayer.Position.X AndAlso tPlayer.LastPosition.Y = tPlayer.Position.Y Then

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

        End Function

        ''' <summary>
        ''' (5:608) set variable
        ''' <see cref="Monkeyspeak.Variable">%Variable</see> to the furre
        ''' named {..} colors, if they are in the dream.
        ''' <para>
        ''' For this line to work, the bot must look at the specified furre
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the specified furre is in the dream
        ''' </returns>
        Public Function FurreNamedColorsVar(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)
            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Var.Value = Target.FurreColors.ToString
            Return Target.FurreID > 0

        End Function

        ''' <summary>
        ''' (1:602) and the furre named {..} description contains {..}
        ''' description, if they are in the dream.
        ''' <para>
        ''' For this line to work, the bot must look at the specified furre
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the specified-furre first
        ''' </exception>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Public Function FurreNamedDescContains(reader As TriggerReader) As Boolean
            Dim name = reader.ReadString()
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Dim Pattern = reader.ReadString
            If String.IsNullOrEmpty(Target.FurreDescription) Then Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            Return Target.FurreDescription.Contains(Pattern)

        End Function

        ''' <summary>
        ''' (5:607) set variable
        ''' <see cref="Monkeyspeak.Variable">%Variable</see> to the furred
        ''' named {..} description, if they are in the dream.
        ''' <para>
        ''' For this line to work, the bot must look at the specified furre
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' True if the specified furre is in the dream
        ''' </returns>
        Public Function FurreNamedDescVar(reader As TriggerReader) As Boolean
            Dim Var = reader.ReadVariable(True)
            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            If String.IsNullOrEmpty(Target.FurreDescription) Then Throw New MonkeyspeakException("Description not found, Try looking at the furre first.")
            Var.Value = Target.FurreDescription
            Return True

        End Function

        ''' <summary>
        ''' (5:621) set <see cref="Monkeyspeak.Variable">%Variable</see> to
        ''' the direction number the furre names {..}, is facing/ moved in.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedFaceDirectionNumber(reader As TriggerReader) As Boolean
            Dim Dir = reader.ReadVariable(True)
            Dim name = reader.ReadString
            Dim tPlayer As Furre = Dream.Furres.GerFurreByName(name)
            Dim direction As Double = 0

            If tPlayer.LastPosition.X <> tPlayer.Position.X Or tPlayer.LastPosition.Y <> tPlayer.Position.Y Then

                If (tPlayer.LastPosition.Y > tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X > tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X))) Then
                    direction = 7
                ElseIf (tPlayer.LastPosition.Y > tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X < tPlayer.Position.X))) Then
                    direction = 9
                ElseIf (tPlayer.LastPosition.Y < tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X < tPlayer.Position.X))) Then
                    direction = 3
                ElseIf (tPlayer.LastPosition.Y < tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X > tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X))) Then
                    direction = 1
                End If

            ElseIf tPlayer.LastPosition.X = tPlayer.Position.X AndAlso tPlayer.LastPosition.Y = tPlayer.Position.Y Then

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

        End Function

        ''' <summary>
        ''' (1:628) and the furre named {..} is facing NE,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedFacingNE(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Dim Spec = reader.ReadNumber()
            Select Case Target.Direction
                Case 3
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:629) and the furre named {..} is facing NW,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedFacingNW(reader As TriggerReader) As Boolean
            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Dim Spec = reader.ReadNumber()
            Select Case Target.Direction
                Case 2
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:630) and the furre named {..} is facing SE,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on success
        ''' </returns>
        Public Function FurreNamedFacingSE(reader As TriggerReader) As Boolean
            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Return Target.Direction = 1
        End Function

        ''' <summary>
        ''' (1:631) and the furre named {..} is facing SW,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedFacingSW(reader As TriggerReader) As Boolean
            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Return Target.Direction = 0
        End Function

        ''' <summary>
        ''' (1:609) and the furre named {..} is female,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedFemale(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0 Or 1
                    Return Target.FurreColors.Gender = 0

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:605) set variable %Variable to the furre named {..} gender if
        ''' they are in the dream.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedGenderVar(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)
            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Select Case Target.FurreColors.Gender
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0 Or 1
                    Var.Value = Target.FurreColors.Gender

            End Select
            Return True

        End Function

        ''' <summary>
        ''' (1:627) and the furre named {..} is laying.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedLaying(reader As TriggerReader) As Boolean
            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Return Target.Direction = 4

        End Function

        ''' <summary>
        ''' (1:608) and the furre named {..} is male,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedMale(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim Target As Furre = Dream.Furres.GerFurreByName(name)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Target.FurreColors.Gender = 1
                Case 1
                    If Target.FurreColors.Gender = -1 Then
                        If Target.FurreColors.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Target.FurreColors.Gender = 1
                    Else
                        Return Target.FurreColors.Gender = 1
                    End If
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:635) and the furre named {..} moved from (x,y),
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedMoveFrom(reader As TriggerReader) As Boolean

            Dim tPlayer = Dream.Furres.GerFurreByName(reader.ReadString)
            Dim X = ReadVariableOrNumber(reader)
            Dim Y = ReadVariableOrNumber(reader)
            Return DirectCast(tPlayer, Furre).LastPosition.X = X AndAlso DirectCast(tPlayer, Furre).LastPosition.Y = Y

        End Function

        ''' <summary>
        ''' (1:901) and the furre named {..} moved into/is standing at (x,y),
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedMoveInto(reader As TriggerReader) As Boolean

            Dim tPlayer As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Dim X = ReadVariableOrNumber(reader)
            Dim Y = ReadVariableOrNumber(reader)
            Return tPlayer.Position.X = X AndAlso tPlayer.Position.Y = Y

        End Function

        ''' <summary>
        ''' (1:637) and the furre named {..}, successfully moved in
        ''' direction # (seven = North-West, nine = North-East, three =
        ''' South-East, one = South=West)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedMoveIntoDirection(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim tPlayer As Furre = Dream.Furres.GerFurreByName(name)
            Dim Dir = ReadVariableOrNumber(reader, False)
            Dim Direction As Double = 0
            If tPlayer.LastPosition.X = tPlayer.Position.X AndAlso tPlayer.LastPosition.Y = tPlayer.Position.Y Then
                Return False
            End If
            If (tPlayer.LastPosition.Y > tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X > tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X))) Then
                Direction = 7
            ElseIf (tPlayer.LastPosition.Y > tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X < tPlayer.Position.X))) Then
                Direction = 9
            ElseIf (tPlayer.LastPosition.Y < tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X < tPlayer.Position.X))) Then
                Direction = 3
            ElseIf (tPlayer.LastPosition.Y < tPlayer.Position.Y) And ((IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X > tPlayer.Position.X)) Or (Not IsOdd(tPlayer.LastPosition.Y) And (tPlayer.LastPosition.X = tPlayer.Position.X))) Then
                Direction = 1
            End If
            Return Direction = Dir

        End Function

        ''' <summary>
        ''' (1:617) and the furre named {..} doesn't wings of type #
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedNoWings(reader As TriggerReader) As Boolean
            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0 Or 1
                    Return Target.FurreColors.Wings <> reader.ReadNumber()
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:618) set variable %Variable to the X coordinate where the
        ''' furre named {..} moved into/is at.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedSetCordX(reader As TriggerReader) As Boolean

            Dim Cord = reader.ReadVariable(True)
            Dim tPlayer As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Cord.Value = tPlayer.Position.X
            Return True

        End Function

        ''' <summary>
        ''' (5:619) set variable %Variable to the Y coordinate where the
        ''' furre named {..} moved into/is at.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedSetCordY(reader As TriggerReader) As Boolean

            Dim Cord = reader.ReadVariable(True)
            Dim tPlayer As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Cord.Value = tPlayer.Position.Y
            Return True

        End Function

        ''' <summary>
        ''' (1:626) and the furre named {..} is sitting.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedSitting(reader As TriggerReader) As Boolean
            Return 0 = Dream.Furres.GerFurreByName(reader.ReadString).Pose
        End Function

        ''' <summary>
        ''' (1:613) and the furre named {..} is Species # (please
        ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/dsparams"/>
        ''' for info)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedSpecies(reader As TriggerReader) As Boolean

            Dim name = reader.ReadString
            Dim TargetFurre As Furre = Dream.Furres.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case TargetFurre.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species not found. Try looking at the furre first")
                Case 0 Or 1
                    Return TargetFurre.FurreColors.Species = Spec

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:606) set variable %Variable to the furre named {..} species,
        ''' if they are in the dream.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedSpeciesVar(reader As TriggerReader) As Boolean
            Dim Var = reader.ReadVariable(True)
            Dim name = reader.ReadString
            Dim TargetFurre As Furre = Dream.Furres.GerFurreByName(name)
            Select Case TargetFurre.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species not found. Try looking at the furre first")
                Case 0 Or 1
                    Var.Value = TargetFurre.FurreColors.Species

            End Select
            Return True

        End Function

        ''' <summary>
        ''' (1:625) and the furre named {..} is standing.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedStanding(reader As TriggerReader) As Boolean

            Dim TargetFurre As Furre = Dream.Furres.GerFurreByName(reader.ReadString)

            Select Case TargetFurre.Pose
                Case 1 To 3
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:636) and the furre named {..} tried to move but stood still.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the Furres location doesn't change
        ''' </returns>
        Public Function FurreNamedStoodStill(reader As TriggerReader) As Boolean

            Dim tPlayer As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Return tPlayer.LastPosition.X = tPlayer.Position.X AndAlso tPlayer.LastPosition.Y = tPlayer.Position.Y

        End Function

        ''' <summary>
        ''' (1:610) and the furre Named {..} is unspecified,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedUnSpecified(reader As TriggerReader) As Boolean

            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Select Case Target.FurreColors.Gender
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0 To 2

                    Return Target.FurreColors.Gender = 2

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:616) and the furre named {..} has wings of type #
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedWings(reader As TriggerReader) As Boolean

            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 1 Or 0
                    Return Target.FurreColors.Wings = reader.ReadNumber()

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:610) set %Variable to the wings type the furre named {..} is wearing.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedWingsVar(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)

            Dim TargetFurre As Furre = Dream.Furres.GerFurreByName(reader.ReadString)
            Select Case TargetFurre.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")

                Case 1 Or 0

                    Var.Value = TargetFurre.FurreColors.Wings

            End Select
            Return True

        End Function

        ''' <summary>
        ''' (5:613) move the bot in direction # one space. (seven =
        ''' North-West, nine = North-East, three = South-East, one = South=West)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when the directions don't match the correct format
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function MoveBot(reader As TriggerReader) As Boolean

            Dim Direction = ReadVariableOrNumber(reader)
            Select Case Direction
                Case 7 Or 9 Or 3 Or 1
                    Return SendServer("`m" + Direction.ToString)
                Case Else
                    Throw New MonkeyspeakException("Directions must be in the form of  7, 9, 3, or 1")
            End Select
            Return True

        End Function

        ''' <summary>
        ''' (1:634) and the triggering furre moved from (x,y),
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function MoveFrom(reader As TriggerReader) As Boolean

            Dim X = ReadVariableOrNumber(reader)
            Dim Y = ReadVariableOrNumber(reader,)
            Return Player.LastPosition.X = X AndAlso Player.LastPosition.Y = Y

        End Function

        ''' <summary>
        ''' 0:901) when a furre moves into (x,y),
        ''' <para>
        ''' (1:900) and the triggering furre moved into/is standing at (x,y),
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function MoveInto(reader As TriggerReader) As Boolean

            Dim X = ReadVariableOrNumber(reader)
            Dim Y = ReadVariableOrNumber(reader)
            Return Player.Position.X = X AndAlso Player.Position.Y = Y

        End Function

        ''' <summary>
        ''' (1:636) and the triggering furre successfully moved in direction
        ''' # (seven = North-West, nine = North-East, three = South-East,
        ''' one = South=West)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function MoveIntoDirection(reader As TriggerReader) As Boolean
            Dim Dir = ReadVariableOrNumber(reader)
            Dim Direction As Double = 0
            If Player.LastPosition.X = Player.Position.X AndAlso Player.LastPosition.Y = Player.Position.Y Then
                Return False
            End If
            If (Player.LastPosition.Y > Player.Position.Y) And ((IsOdd(Player.LastPosition.Y) And (Player.LastPosition.X > Player.Position.X)) Or (Not IsOdd((Player.LastPosition.Y) And (Player.LastPosition.X = Player.Position.X)))) Then
                Direction = 7
            ElseIf (Player.LastPosition.Y > Player.Position.Y) And ((IsOdd(Player.LastPosition.Y) And (Player.LastPosition.X = Player.Position.X)) Or (Not IsOdd(Player.LastPosition.Y) And (Player.LastPosition.X < Player.Position.X))) Then
                Direction = 9
            ElseIf (Player.LastPosition.Y < Player.Position.Y) And ((IsOdd(Player.LastPosition.Y) And (Player.LastPosition.X = Player.Position.X)) Or (Not IsOdd(Player.LastPosition.Y)) And (Player.LastPosition.X < Player.Position.X)) Then
                Direction = 3
            ElseIf (Player.LastPosition.Y < Player.Position.Y) And (IsOdd((Player.LastPosition.Y) And (Player.LastPosition.X > Player.Position.X)) Or (Not IsOdd(Player.LastPosition.Y) And (Player.LastPosition.X = Player.Position.X))) Then
                Direction = 1
            End If
            Return Direction = Dir

        End Function

        ''' <summary>
        ''' (1:601) and triggering furre's description does not contain {..},
        ''' <para>
        ''' NOTE: This line only works after the bot has looked at the
        '''       specified furre
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function NotDescContains(reader As TriggerReader) As Boolean
            If String.IsNullOrEmpty(Player.FurreDescription) Then Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            Return Not Player.FurreDescription.Contains(reader.ReadString)

        End Function

        ''' <summary>
        ''' (1:603) and the furre named {..} description does not contain {..}
        ''' <para>
        ''' NOTE: This line only works after the bot has looked at the
        '''       specified furre
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function NotDescContainsFurreNamed(reader As TriggerReader) As Boolean

            Dim Target As Furre = Dream.Furres.GerFurreByName(reader.ReadString())
            If String.IsNullOrEmpty(Target.FurreDescription) Then Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            Return Not Target.FurreDescription.Contains(reader.ReadString)

        End Function

        ''' <summary>
        ''' (5:616) set variable %Variable to the X coordinate where the
        ''' triggering furre moved into/is at.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SetCordX(reader As TriggerReader) As Boolean
            reader.ReadVariable(True).Value = Player.Position.X
            Return True
        End Function

        ''' <summary>
        ''' (5:617) set variable %Variable to the Y coordinate where the
        ''' triggering furre moved into/is at.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SetCordY(reader As TriggerReader) As Boolean
            reader.ReadVariable(True).Value = Player.Position.Y
            Return True
        End Function

        ''' <summary>
        ''' (1:638) and the triggering furre tried to move but stood still.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function StoodStill(reader As TriggerReader) As Boolean

            Return Player.LastPosition.X = Player.Position.X AndAlso Player.LastPosition.Y = Player.Position.Y

        End Function

        ''' <summary>
        ''' (5:604) set variable %Variable to the triggering furre's colors.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreColorsVar(reader As TriggerReader) As Boolean
            reader.ReadVariable(True).Value = Player.FurreColors.ToString
            Return True
        End Function

        ''' <summary>
        ''' (5:600) set variable %Variable to the Triggering furre's description.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreDescVar(reader As TriggerReader) As Boolean

            If Player.FurreDescription = Nothing Then Throw New MonkeyspeakException("Description not found, Try looking at the furre first.")
            reader.ReadVariable(True).Value = Player.FurreDescription
            Return True
        End Function

        ''' <summary>
        ''' (1:621) and the triggering furre is facing NE,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreFacingNE(reader As TriggerReader) As Boolean
            Return Player.Direction = 3
        End Function

        ''' <summary>
        ''' (1:622) and the triggering furre is facing NW,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreFacingNW(reader As TriggerReader) As Boolean
            Return Player.Direction = 2

        End Function

        ''' <summary>
        ''' (1:623) and the triggering furre is facing SE,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreFacingSE(reader As TriggerReader) As Boolean
            Return Player.Direction = 1

        End Function

        ''' <summary>
        ''' (1:624) and the triggering furre is facing SW,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreFacingSW(reader As TriggerReader) As Boolean
            Return Player.Direction = 0

        End Function

        ''' <summary>
        ''' (1:605) and the triggering furre is female,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreFemale(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Player.FurreColors.Gender = 0
                Case 1
                    If Player.FurreColors.Gender = -1 Then
                        If Player.FurreColors.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Player.FurreColors.Gender = 0
                    Else
                        Return Player.FurreColors.Gender = 0
                    End If
            End Select

            Return False
        End Function

        ''' <summary>
        ''' (5:601 set variable %Variable to the triggering furre's gender.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreGenderVar(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    If Player.FurreColors.Gender = -1 Then Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                    Var.Value = Player.FurreColors.Gender
                Case 1
                    If Player.FurreColors.Gender = -1 Then
                        If Player.FurreColors.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Var.Value = Player.FurreColors.Gender
                    Else
                        Var.Value = Player.FurreColors.Gender
                    End If
            End Select
            Return True

        End Function

        ''' <summary>
        ''' (1:x) and the triggering furre is laying.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreLaying(reader As TriggerReader) As Boolean
            Return Player.Pose = 4
        End Function

        ''' <summary>
        ''' (1:604) and the triggering furre is male,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreMale(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Player.FurreColors.Gender = 1
                Case 1
                    If Player.FurreColors.Gender = -1 Then
                        If Player.FurreColors.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Player.FurreColors.Gender = 1
                    Else
                        Return Player.FurreColors.Gender = 1
                    End If
            End Select
            Return False
        End Function

        ''' <summary>
        ''' (1:615) and the triggering furre doesn't wings of type #
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreNoWings(reader As TriggerReader) As Boolean

            Dim Spec = reader.ReadNumber()
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0
                    Return Player.FurreColors.Wings <> Spec
                Case 1
                    If Player.FurreColors.Wings = -1 Then
                        If Player.FurreColors.Wings = -1 Then Throw New MonkeyspeakException("Wings type not found, Try looking at the furre first")
                        Return Player.FurreColors.Wings <> Spec
                    Else
                        Return Player.FurreColors.Wings <> Spec
                    End If
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:x) and the triggering furre is sitting.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreSitting(reader As TriggerReader) As Boolean
            Return Player.Pose = 0
        End Function

        ''' <summary>
        ''' (1:612) and the trigger furre is Species # (please
        ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/dsparams"/>
        ''' for info)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreSpecies(reader As TriggerReader) As Boolean

            Dim Spec = reader.ReadNumber()
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species type not found. Try looking at the furre first")
                Case 0
                    Return Player.FurreColors.Species = Spec
                Case 1
                    Return Player.FurreColors.Species = Spec
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:602) set variable %Variable to the triggering furre's species.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreSpeciesVar(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species not found. Try looking at the furre first")
                Case 0 Or 1
                    reader.ReadVariable(True).Value = Player.FurreColors.Species

            End Select
            Return True

        End Function

        ''' <summary>
        ''' (1:x) and the triggering furre is standing.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function TriggeringFurreStanding(reader As TriggerReader) As Boolean

            Select Case Player.Pose
                Case 1 To 3
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:606) and the triggering furre is unspecified,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreUnspecified(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Player.FurreColors.Gender = 2
                Case 1
                    If Player.FurreColors.Gender = -1 Then
                        If Player.FurreColors.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Player.FurreColors.Gender = 2
                    Else
                        Return Player.FurreColors.Gender = 2
                    End If
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:614) and the triggering furre has wings of type #
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreWings(reader As TriggerReader) As Boolean

            Dim Spec As Double = reader.ReadNumber()
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0
                    Return Player.FurreColors.Wings = Spec
                Case 1
                    If Player.FurreColors.Wings = -1 Then
                        If Player.FurreColors.Wings = -1 Then Throw New MonkeyspeakException("Wings type not found, Try looking at the furre first")
                        Return Player.FurreColors.Wings = Spec
                    Else
                        Return Player.FurreColors.Wings = Spec
                    End If
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:609) set %Variable to the wings type the triggering furre is wearing.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeySpeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreWingsVar(reader As TriggerReader) As Boolean

            Dim Var = reader.ReadVariable(True)
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0
                    Var.Value = Player.FurreColors.Wings
                Case 1
                    If Player.FurreColors.Wings = -1 Then
                        If Player.FurreColors.Wings = -1 Then Throw New MonkeyspeakException("Wings type not found, Try looking at the furre first")
                        Var.Value = Player.FurreColors.Wings
                    Else
                        Var.Value = Player.FurreColors.Wings
                    End If
            End Select
            Return True

        End Function

        ''' <summary>
        ''' (5:615) turn the bot counter-clockwise one space.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TurnCCW(reader As TriggerReader) As Boolean
            Return SendServer("`<")
        End Function

        ''' <summary>
        ''' (5:614) turn the bot clock-wise one space.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TurnCW(reader As TriggerReader) As Boolean
            Return SendServer("`>")
        End Function

#End Region

    End Class

End Namespace