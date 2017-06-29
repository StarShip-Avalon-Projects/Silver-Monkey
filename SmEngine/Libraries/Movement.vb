Imports System.Text.RegularExpressions
Imports Furcadia.Net.Dream
Imports Monkeyspeak

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
    Public Class Movement
        Inherits MonkeySpeakLibrary

#Region "Private Fields"

        ''' <summary>
        ''' Regex syntax for direction movement
        ''' </summary>
        Private Const RGEX_Mov_Steps As String = "(nw|ne|sw|se|1|3|7|9)"

#End Region

#Region "Public Constructors"

        Public Sub New(ByRef session As BotSession)
            MyBase.New(session)

            '(0:600) When the bot reads a description.
            Add(TriggerCategory.Cause, 600,
                Function()
                    Return True
                End Function, "(0:600) When the bot sees a furre description,")

            '(1:600) and triggering furre's description contains {..}
            Add(New Trigger(TriggerCategory.Condition, 600), AddressOf DescContains,
                "(1:600) and triggering furre's description contains {..}")

            '(1:601) and triggering furre's description does not contain {..}
            Add(New Trigger(TriggerCategory.Condition, 601), AddressOf NotDescContains,
                "(1:601) and triggering furre's description does not contain {..}")

            '(1:602) and the furre named {..} description contains {..}
            Add(New Trigger(TriggerCategory.Condition, 602), AddressOf FurreNamedDescContains,
                "(1:602) and the furre named {..} description contains {..}if they are in the dream,")

            '(1:603) and the furre named {..} description does not contain {..}
            Add(New Trigger(TriggerCategory.Condition, 603), AddressOf NotDescContainsFurreNamed,
             "(1:603) and the furre named {..} description does not contain {..} if they are in the dream")

            '(1:604) and the triggering furre is male,
            Add(New Trigger(TriggerCategory.Condition, 604), AddressOf TriggeringFurreMale,
                "(1:604) and the triggering furre is male,")

            '(1:605) and the triggering furre is female,
            Add(New Trigger(TriggerCategory.Condition, 605), AddressOf TriggeringFurreFemale,
                "(1:605) and the triggering furre is female,")

            '(1:606) and the triggering furre is unspecified,
            Add(New Trigger(TriggerCategory.Condition, 606), AddressOf TriggeringFurreUnspecified,
                "(1:606) and the triggering furre is unspecified,")

            '(1:608) and the furre named {..}'s is male,
            Add(New Trigger(TriggerCategory.Condition, 608), AddressOf FurreNamedMale,
                "(1:608) and the furre named {..}'s is male if they are in the dream,")

            '(1:609) and the furre named {..}'s is female,
            Add(New Trigger(TriggerCategory.Condition, 609), AddressOf FurreNamedFemale,
                "(1:609) and the furre named {..}'s is female if they are in the dream,")

            '(1:609) and the furre named {..}'s is female,
            Add(New Trigger(TriggerCategory.Condition, 610), AddressOf FurreNamedUnSpecified,
                 "(1:610) and the furre Named {..}'s is unspecified if they are in the dream,")

            'TODO: as a URL replace tag for descriptions with urls in an ini file

            '(1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(New Trigger(TriggerCategory.Condition, 612), AddressOf TriggeringFurreSpecies,
                "(1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:613) and the furre named {..} is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(New Trigger(TriggerCategory.Condition, 613), AddressOf FurreNamedSpecies,
                "(1:613) and the furre named {..} is Species # if they are in the dream (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:614) and the triggering furre has wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 614), AddressOf TriggeringFurreWings,
                "(1:614) and the triggering furre has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:615) and the triggering furre doesn't wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 615), AddressOf TriggeringFurreNoWings,
                "(1:615) and the triggering furre doesn't wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:616) and the furre named {..} has wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 616), AddressOf FurreNamedWings,
                "(1:616) and the furre named {..} has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

            '(1:617) and the furre named {..}  doesn't wings of type #,
            Add(New Trigger(TriggerCategory.Condition, 617), AddressOf FurreNamedNoWings,
                "(1:617) and the furre named {..}  doesn't wings of type #, (please see http://www.furcadia.com/dsparams/ for info)")

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

            '(1:625) and the furre named {..} is standing.
            Add(New Trigger(TriggerCategory.Condition, 625), AddressOf FurreNamedStanding,
                "(1:625) and the furre named {..} is standing.")

            '(1:626) and the furre named {..} is sitting.
            Add(New Trigger(TriggerCategory.Condition, 626), AddressOf FurreNamedSitting,
                "(1:626) and the furre named {..} is sitting.")

            '(1:627) and the furre named {..} is laying.
            Add(New Trigger(TriggerCategory.Condition, 627), AddressOf FurreNamedLaying,
                "(1:627) and the furre named {..} is laying.")

            '(1:628) and the furre named {..} is facing NE,
            Add(New Trigger(TriggerCategory.Condition, 628), AddressOf FurreNamedFacingNE,
                "(1:628) and the furre named {..} is facing NE,")

            '(1:629) and the furre named {..} is facing NW,
            Add(New Trigger(TriggerCategory.Condition, 629), AddressOf FurreNamedFacingNW,
                "(1:629) and the furre named {..} is facing NW,")

            '(1:630) and the furre named {..} is facing SE,
            Add(New Trigger(TriggerCategory.Condition, 630), AddressOf FurreNamedFacingSE,
                "(1:630) and the furre named {..} is facing SE,")

            '(1:631) and the furre named {..} is facing SW,
            Add(New Trigger(TriggerCategory.Condition, 631), AddressOf FurreNamedFacingSW,
                "(1:631) and the furre named {..} is facing SW,")

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

            '(5:605) set variable %Variable to the furre named {..}'s gender if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 605), AddressOf FurreNamedGenderVar,
                "(5:605) set variable %Variable to the furre named {..}'s gender if they are in the dream.")

            '(5:606) set variable %Variable to the furre named {..}'s species, if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 606), AddressOf FurreNamedSpeciesVar,
                "(5:606) set variable %Variable to the furre named {..}'s species, if they are in the dream.")

            '(5:607) set variable %Variable to the furred named {..}'s description, if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 607), AddressOf FurreNamedDescVar,
                    "(5:607) set variable %Variable to the furred named {..}'s description, if they are in the dream.")

            '(5:608) set variable %Variable to the furre named {..}'s colors, if they are in the dream.
            Add(New Trigger(TriggerCategory.Effect, 608), AddressOf FurreNamedColorsVar,
                "(5:608) set variable %Variable to the furre named {..}'s colors, if they are in the dream.")

            '(5:609) set %Variable to the wings type the triggering furre is wearing.
            Add(New Trigger(TriggerCategory.Effect, 609), AddressOf TriggeringFurreWingsVar,
                "(5:609) set %Variable to the wings type the triggering furre is wearing.")

            '(5:610) set %Variable to the wings type the furre named {..} is wearing.
            Add(New Trigger(TriggerCategory.Effect, 610), AddressOf FurreNamedWingsVar,
                "(5:610) set %Variable to the wings type the furre named {..} is wearing.")

            '(0:601) When a furre moves,
            Add(TriggerCategory.Cause, 601,
                 Function()
                     Return True
                 End Function, "(0:601) When a furre moves,")
            '(0:602) when a furre moves into (x,y),
            Add(TriggerCategory.Cause, 602, AddressOf MoveInto, "(0:602) when a furre moves into (x,y),")

            '(1:900) and the triggering furre moved into/is standing at (x,y),
            Add(New Trigger(TriggerCategory.Condition, 632), AddressOf MoveInto, "(1:632) and the triggering furre moved into/is standing at (x,y)")

            '(1:633) and the furre named {..} moved into/is standing at (x,y),
            Add(New Trigger(TriggerCategory.Condition, 633), AddressOf FurreNamedMoveInto, "(1:633) and the furre named {..} moved into/is standing at (x,y),")

            '(1:634) and the triggering furre moved from (x,y),
            Add(New Trigger(TriggerCategory.Condition, 634), AddressOf MoveFrom, "(1:634) and the triggering furre moved from (x,y),")

            '(1:635) and the furre named {..} moved from (x,y),
            Add(New Trigger(TriggerCategory.Condition, 635), AddressOf FurreNamedMoveFrom, "(1:635) and the furre named {..} moved from (x,y),")

            '(1:636) and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Condition, 636), AddressOf MoveIntoDirection, "(1:636) and the triggering furre successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(1:637) and the furre named {..}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Condition, 637), AddressOf FurreNamedMoveIntoDirection, "(1:637) and the furre named {..}, successfully moved in direction # (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(1:638) and the triggering furre tried to move but stood still.
            Add(New Trigger(TriggerCategory.Condition, 638), AddressOf StoodStill, "(1:638) and the triggering furre tried to move but stood still.")

            '(1:639) and the furre named {..} tried to move but stood still.
            Add(New Trigger(TriggerCategory.Condition, 639), AddressOf FurreNamedMoveIntoDirection, "(1:639) and the furre named {..} tried to move but stood still.")

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

            '(5:618) set variable %Variable to the X coordinate where the furre named {..} moved into/is at.
            Add(New Trigger(TriggerCategory.Effect, 618), AddressOf FurreNamedSetCordX, "(5:618) set variable %Variable to the X coordinate where the furre named {..} moved into/is at.")

            '(5:619) set variable %Variable to the Y coordinate where the furre named {..} moved into/is at.
            Add(New Trigger(TriggerCategory.Effect, 619), AddressOf FurreNamedSetCordY, "(5:619) set variable %Variable to the Y coordinate where the furre named {..} moved into/is at.")

            '(5:620) set %Variable to the direction number the triggering furre is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Effect, 620), AddressOf FaceDirectionNumber, "(5:620) set %Variable to the direction number the triggering furre is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:621) set %Variable to the direction number the furre names {..}, is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(New Trigger(TriggerCategory.Effect, 621), AddressOf FurreNamedFaceDirectionNumber, "(5:621) set %Variable to the direction number the furre names {..}, is facing/ moved in. (seven = North-West, nine = North-East, three = South-East, one = South=West)")

            '(5:622) make the bot sit down.
            Add(New Trigger(TriggerCategory.Effect, 622), AddressOf BotSit, "(5:622) make the bot sit down.")

            '(5:623) make the bot lay down.
            Add(New Trigger(TriggerCategory.Effect, 623), AddressOf BotLie, "(5:623) make the bot lay down.")

            '(5:624) make the bot stand up.
            Add(New Trigger(TriggerCategory.Effect, 624), AddressOf BotStand, "(5:624) make the bot stand up.")

            '(5:625) Move the bot  in this sequence {..} (one, sw, three, se, seven, nw, nine, or ne)
            Add(New Trigger(TriggerCategory.Effect, 625), AddressOf BotMoveSequence, "(5:625) Move the bot  in this sequence {..} (one, sw, three, se, seven, nw, nine, or ne)")
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
        Function BotLie(reader As TriggerReader) As Boolean

            Return sendServer("`lie")

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
        Function BotMoveSequence(reader As TriggerReader) As Boolean
            'TODO: http://bugtraq.tsprojects.org/view.php?id=55
            'Queue System?

            Dim directions As String = reader.ReadString
            Dim r As New Regex(RGEX_Mov_Steps, RegexOptions.IgnoreCase)
            Dim m As MatchCollection = r.Matches(directions)
            Dim ServerSend As Boolean
            For Each n As Match In m
                If n.Value.ToLower = "ne" Then
                    ServerSend = sendServer("`m9")
                ElseIf n.Value.ToLower = "se" Then
                    ServerSend = sendServer("`m3")
                ElseIf n.Value.ToLower = "nw" Then
                    ServerSend = sendServer("`m7")
                ElseIf n.Value.ToLower = "sw" Then
                    ServerSend = sendServer("`m1")
                ElseIf n.Value = (1 Or 7 Or 3 Or 9) Then
                    ServerSend = sendServer("`m" + n.Value)
                Else
                    Throw New ArgumentOutOfRangeException("the arguments supplied are invalid")
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
        Function BotSit(reader As TriggerReader) As Boolean
            Return sendServer("`sit")
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
        Function BotStand(reader As TriggerReader) As Boolean
            Return sendServer("`stand")
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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <remarks>
        ''' </remarks>
        Function DescContains(reader As TriggerReader) As Boolean

            Dim Pattern As String = reader.ReadString
            If String.IsNullOrEmpty(Player.Desc) Then Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            Return Player.Desc.Contains(Pattern)

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
        Function FaceDirectionNumber(reader As TriggerReader) As Boolean
            Dim Dir As Variable = reader.ReadVariable(True)
            Dim tPlayer As FURRE = Player
            Dim direction As Double = 0

            If tPlayer.LastPosition.x <> tPlayer.Position.x Or tPlayer.LastPosition.y <> tPlayer.Position.y Then

                If (tPlayer.LastPosition.y > tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x > tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x))) Then
                    direction = 7
                ElseIf (tPlayer.LastPosition.y > tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x < tPlayer.Position.x))) Then
                    direction = 9
                ElseIf (tPlayer.LastPosition.y < tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x < tPlayer.Position.x))) Then
                    direction = 3
                ElseIf (tPlayer.LastPosition.y < tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x > tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x))) Then
                    direction = 1
                End If

            ElseIf tPlayer.LastPosition.x = tPlayer.Position.x AndAlso tPlayer.LastPosition.y = tPlayer.Position.y Then

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
        Function FurreNamedColorsVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Var.Value = Target.Color.ToString
            Return Target.ID > 0

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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the specified-furre first
        ''' </exception>
        ''' <returns>
        ''' True on Success
        ''' </returns>
        Function FurreNamedDescContains(reader As TriggerReader) As Boolean
            Dim name As String = reader.ReadString()
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Pattern As String = reader.ReadString
            If String.IsNullOrEmpty(Target.Desc) Then Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            Return Target.Desc.Contains(Pattern)

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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' True if the specified furre is in the dream
        ''' </returns>
        Function FurreNamedDescVar(reader As TriggerReader) As Boolean
            Dim Var As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            If String.IsNullOrEmpty(Target.Desc) Then Throw New MonkeyspeakException("Description not found, Try looking at the furre first.")
            Var.Value = Target.Desc
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
        Function FurreNamedFaceDirectionNumber(reader As TriggerReader) As Boolean
            Dim Dir As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim direction As Double = 0

            If tPlayer.LastPosition.x <> tPlayer.Position.x Or tPlayer.LastPosition.y <> tPlayer.Position.y Then

                If (tPlayer.LastPosition.y > tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x > tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x))) Then
                    direction = 7
                ElseIf (tPlayer.LastPosition.y > tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x < tPlayer.Position.x))) Then
                    direction = 9
                ElseIf (tPlayer.LastPosition.y < tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x < tPlayer.Position.x))) Then
                    direction = 3
                ElseIf (tPlayer.LastPosition.y < tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x > tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x))) Then
                    direction = 1
                End If

            ElseIf tPlayer.LastPosition.x = tPlayer.Position.x AndAlso tPlayer.LastPosition.y = tPlayer.Position.y Then

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
        Function FurreNamedFacingNE(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
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
        Function FurreNamedFacingNW(reader As TriggerReader) As Boolean
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
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
        Function FurreNamedFacingSE(reader As TriggerReader) As Boolean
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.Direction
                Case 1
                    Return True
            End Select
            Return False

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
        Function FurreNamedFacingSW(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.Direction
                Case 0
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:609) and the furre named {..} is female,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedFemale(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0 Or 1
                    Return Target.Color.Gender = 0

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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedGenderVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Select Case Target.Color.Gender
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0 Or 1
                    Var.Value = Target.Color.Gender

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
        Function FurreNamedLaying(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.Pose
                Case 4
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:608) and the furre named {..} is male,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedMale(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
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
        Function FurreNamedMoveFrom(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim X As Double = ReadVariableOrNumber(reader, False)
            Dim Y As Double = ReadVariableOrNumber(reader, False)
            Return tPlayer.LastPosition.x = Convert.ToUInt32(X) AndAlso tPlayer.LastPosition.y = Convert.ToUInt32(Y)

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
        Function FurreNamedMoveInto(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim X As Double = ReadVariableOrNumber(reader, False)
            Dim Y As Double = ReadVariableOrNumber(reader, False)
            Return tPlayer.Position.x = Convert.ToUInt32(X) AndAlso tPlayer.Position.y = Convert.ToUInt32(Y)

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
        Function FurreNamedMoveIntoDirection(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Dir As Double = ReadVariableOrNumber(reader, False)
            Dim Direction As Double = 0
            If tPlayer.LastPosition.x = tPlayer.Position.x AndAlso tPlayer.LastPosition.y = tPlayer.Position.y Then
                Return False
            End If
            If (tPlayer.LastPosition.y > tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x > tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x))) Then
                Direction = 7
            ElseIf (tPlayer.LastPosition.y > tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x < tPlayer.Position.x))) Then
                Direction = 9
            ElseIf (tPlayer.LastPosition.y < tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x < tPlayer.Position.x))) Then
                Direction = 3
            ElseIf (tPlayer.LastPosition.y < tPlayer.Position.y) And ((IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x > tPlayer.Position.x)) Or (Not IsOdd(tPlayer.LastPosition.y) And (tPlayer.LastPosition.x = tPlayer.Position.x))) Then
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
        Function FurreNamedNoWings(reader As TriggerReader) As Boolean
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0 Or 1
                    Return Target.Color.Wings <> Spec

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
        Function FurreNamedSetCordX(reader As TriggerReader) As Boolean

            Dim Cord As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Cord.Value = tPlayer.Position.x
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
        Function FurreNamedSetCordY(reader As TriggerReader) As Boolean

            Dim Cord As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Cord.Value = tPlayer.Position.y
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
        Function FurreNamedSitting(reader As TriggerReader) As Boolean
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.Pose
                Case 0
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:613) and the furre named {..} is Species # (please
        ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/dsparams"/>
        ''' for info)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedSpecies(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species not found. Try looking at the furre first")
                Case 0 Or 1
                    Return Target.Color.Species = Spec

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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedSpeciesVar(reader As TriggerReader) As Boolean
            Dim Var As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species not found. Try looking at the furre first")
                Case 0 Or 1
                    Var.Value = Target.Color.Species

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
        Function FurreNamedStanding(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.Pose
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
        Function FurreNamedStoodStill(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim tPlayer As FURRE = Dream.FurreList.GerFurreByName(name)
            Return tPlayer.LastPosition.x = tPlayer.Position.x AndAlso tPlayer.LastPosition.y = tPlayer.Position.y

        End Function

        ''' <summary>
        ''' (1:610) and the furre Named {..} is unspecified,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedUnSpecified(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Select Case Target.Color.Gender
                Case -1
                    Throw New Exception("Gender not found. Try looking at the furre first")
                Case 0 To 2

                    Return Target.Color.Gender = 2

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:616) and the furre named {..} has wings of type #
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedWings(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Spec As Double = reader.ReadNumber()
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")

                Case 1 Or 0

                    Return Target.Color.Wings = Spec

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:610) set %Variable to the wings type the furre named {..} is wearing.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function FurreNamedWingsVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Dim name As String = reader.ReadString
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Select Case Target.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")

                Case 1 Or 0

                    Var.Value = Target.Color.Wings

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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when the directions don't match the correct format
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function MoveBot(reader As TriggerReader) As Boolean

            Dim Dir As Double = ReadVariableOrNumber(reader, False)
            Select Case Dir
                Case 7 Or 9 Or 3 Or 1
                    Return sendServer("`m" + Dir.ToString)
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
        Function MoveFrom(reader As TriggerReader) As Boolean

            Dim X As Double = ReadVariableOrNumber(reader, False)
            Dim Y As Double = ReadVariableOrNumber(reader, False)
            Return Player.LastPosition.x = X AndAlso Player.LastPosition.y = Y

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
        Function MoveInto(reader As TriggerReader) As Boolean

            Dim X As Double = ReadVariableOrNumber(reader, False)
            Dim Y As Double = ReadVariableOrNumber(reader, False)
            Return Player.Position.x = X AndAlso Player.Position.y = Y

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
        Function MoveIntoDirection(reader As TriggerReader) As Boolean
            Dim Dir As Double = ReadVariableOrNumber(reader, False)
            Dim Direction As Double = 0
            If Player.LastPosition.x = Player.Position.x AndAlso Player.LastPosition.y = Player.Position.y Then
                Return False
            End If
            If (Player.LastPosition.y > Player.Position.y) And ((IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x > Player.Position.x)) Or (Not IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x = Player.Position.x))) Then
                Direction = 7
            ElseIf (Player.LastPosition.y > Player.Position.y) And ((IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x = Player.Position.x)) Or (Not IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x < Player.Position.x))) Then
                Direction = 9
            ElseIf (Player.LastPosition.y < Player.Position.y) And ((IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x = Player.Position.x)) Or (Not IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x < Player.Position.x))) Then
                Direction = 3
            ElseIf (Player.LastPosition.y < Player.Position.y) And ((IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x > Player.Position.x)) Or (Not IsOdd(CInt(Player.LastPosition.y)) And (Player.LastPosition.x = Player.Position.x))) Then
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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function NotDescContains(reader As TriggerReader) As Boolean

            Dim Pattern As String = reader.ReadString
            If String.IsNullOrEmpty(Player.Desc) Then Throw New MonkeyspeakException("Description not found. Try looking at the furre first")
            Return Not Player.Desc.Contains(Pattern)

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
        Function NotDescContainsFurreNamed(reader As TriggerReader) As Boolean

            Dim name As String = reader.ReadString()
            Dim Target As FURRE = Dream.FurreList.GerFurreByName(name)
            Dim Pattern As String = reader.ReadString
            If String.IsNullOrEmpty(Target.Desc) Then Throw New Exception("Description not found. Try looking at the furre first")
            Return Not Target.Desc.Contains(Pattern)

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
        Function SetCordX(reader As TriggerReader) As Boolean

            Dim Cord As Variable = reader.ReadVariable(True)

            Cord.Value = Player.Position.x
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
        Function SetCordY(reader As TriggerReader) As Boolean
            Dim Cord As Variable = reader.ReadVariable(True)
            Cord.Value = Player.Position.y
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
        Function StoodStill(reader As TriggerReader) As Boolean

            Return Player.LastPosition.x = Player.Position.x AndAlso Player.LastPosition.y = Player.Position.y

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
        Function TriggeringFurreColorsVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Var.Value = Player.Color.ToString
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
        Function TriggeringFurreDescVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            If Player.Desc = Nothing Then Throw New MonkeyspeakException("Description not found, Try looking at the furre first.")
            Var.Value = Player.Desc
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
        Function TriggeringFurreFacingNE(reader As TriggerReader) As Boolean

            Select Case Player.Direction
                Case 3
                    Return True
            End Select
            Return False

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
        Function TriggeringFurreFacingNW(reader As TriggerReader) As Boolean

            Select Case Player.Direction
                Case 2
                    Return True
            End Select
            Return False

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
        Function TriggeringFurreFacingSE(reader As TriggerReader) As Boolean
            Select Case Player.Direction
                Case 1
                    Return True
            End Select
            Return False

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
        Function TriggeringFurreFacingSW(reader As TriggerReader) As Boolean

            Select Case Player.Direction
                Case 0
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:605) and the triggering furre is female,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreFemale(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Player.Color.Gender = 0
                Case 1
                    If Player.Color.Gender = -1 Then
                        If Player.Color.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Player.Color.Gender = 0
                    Else
                        Return Player.Color.Gender = 0
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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreGenderVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Select Case Player.LastStat
                Case -1
                    Throw New Exception("Gender not found. Try looking at the furre first")
                Case 0
                    If Player.Color.Gender = -1 Then Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                    Var.Value = Player.Color.Gender
                Case 1
                    If Player.Color.Gender = -1 Then
                        If Player.Color.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Var.Value = Player.Color.Gender
                    Else
                        Var.Value = Player.Color.Gender
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
        Function TriggeringFurreLaying(reader As TriggerReader) As Boolean

            Select Case Player.Pose
                Case 4
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:604) and the triggering furre is male,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreMale(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Player.Color.Gender = 1
                Case 1
                    If Player.Color.Gender = -1 Then
                        If Player.Color.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Player.Color.Gender = 1
                    Else
                        Return Player.Color.Gender = 1
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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreNoWings(reader As TriggerReader) As Boolean

            Dim Spec As Double = reader.ReadNumber()
            Select Case Player.LastStat
                Case -1
                    Throw New Exception("Wings type not found. Try looking at the furre first")
                Case 0
                    Return Player.Color.Wings <> Spec
                Case 1
                    If Player.Color.Wings = -1 Then
                        If Player.Color.Wings = -1 Then Throw New Exception("Wings type not found, Try looking at the furre first")
                        Return Player.Color.Wings <> Spec
                    Else
                        Return Player.Color.Wings <> Spec
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
        Function TriggeringFurreSitting(reader As TriggerReader) As Boolean

            Select Case Player.Pose
                Case 0
                    Return True
            End Select
            Return False

        End Function

        ''' <summary>
        ''' (1:612) and the trigger furre is Species # (please
        ''' <see href="https://cms.furcadia.com/creations/dreammaking/dragonspeak/dsparams"/>
        ''' for info)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreSpecies(reader As TriggerReader) As Boolean

            Dim Spec As Double = reader.ReadNumber()
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species type not found. Try looking at the furre first")
                Case 0
                    Return Player.Color.Species = Spec
                Case 1

                    Return Player.Color.Species = Spec

            End Select
            Return False

        End Function

        ''' <summary>
        ''' (5:602) set variable %Variable to the triggering furre's species.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreSpeciesVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Species not found. Try looking at the furre first")
                Case 0 Or 1
                    Var.Value = Player.Color.Species

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
        Function TriggeringFurreStanding(reader As TriggerReader) As Boolean

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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreUnspecified(reader As TriggerReader) As Boolean

            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Gender not found. Try looking at the furre first")
                Case 0
                    Return Player.Color.Gender = 2
                Case 1
                    If Player.Color.Gender = -1 Then
                        If Player.Color.Gender = -1 Then Throw New MonkeyspeakException("Gender not found, Try looking at the furre first")
                        Return Player.Color.Gender = 2
                    Else
                        Return Player.Color.Gender = 2
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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreWings(reader As TriggerReader) As Boolean

            Dim Spec As Double = reader.ReadNumber()
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0
                    Return Player.Color.Wings = Spec
                Case 1
                    If Player.Color.Wings = -1 Then
                        If Player.Color.Wings = -1 Then Throw New MonkeyspeakException("Wings type not found, Try looking at the furre first")
                        Return Player.Color.Wings = Spec
                    Else
                        Return Player.Color.Wings = Spec
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
        ''' <exception cref="MonkeyspeakException">
        ''' Thrown when a Furre doesn't have a a description. This can be
        ''' prevented by looking at the triggering-furre first
        ''' </exception>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Function TriggeringFurreWingsVar(reader As TriggerReader) As Boolean

            Dim Var As Variable = reader.ReadVariable(True)
            Select Case Player.LastStat
                Case -1
                    Throw New MonkeyspeakException("Wings type not found. Try looking at the furre first")
                Case 0
                    Var.Value = Player.Color.Wings
                Case 1
                    If Player.Color.Wings = -1 Then
                        If Player.Color.Wings = -1 Then Throw New MonkeyspeakException("Wings type not found, Try looking at the furre first")
                        Var.Value = Player.Color.Wings
                    Else
                        Var.Value = Player.Color.Wings
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
        Function TurnCCW(reader As TriggerReader) As Boolean

            Return sendServer("`<")

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
        Function TurnCW(reader As TriggerReader) As Boolean

            Return sendServer("`>")

        End Function

#End Region

    End Class

End Namespace