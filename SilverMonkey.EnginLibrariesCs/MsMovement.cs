using Furcadia.Net.DreamInfo;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using Monkeyspeak.Logging;
using System;
using System.Text.RegularExpressions;

namespace Engine.Libraries
{
    /// <summary>
    /// <para>
    /// Causes: (0:600) -
    /// </para>
    /// <para>
    /// Conditions: (1:600) - (1:631)
    /// </para>
    /// <para>
    /// Effects: (5:600) - (5:625)
    /// </para>
    /// Furcadia Movement MonkeySpeak
    ///<para>
    /// Processing of Furre Triggers around the map, Interact with avatar
    /// settings such as thier map location, Type of Wings, Which avatar a
    /// given furre has.. The Avatar colors ect..
    /// </para>
    /// <para>
    /// Note: A Furre var only contains a description after the bot sends a look command to the server.
    /// </para>
    /// </summary>
    public sealed class MsMovement : MonkeySpeakLibrary
    {
        #region Public Fields

        /// <summary>
        /// Regex syntax for Fircadia isometric directions
        /// </summary>
        public const string RGEX_Mov_Steps = "(nw|ne|sw|se|1|3|7|9)";

        public override int BaseId => 600;

        #endregion Public Fields

        #region Public Methods

        public override void Initialize(params object[] args)
        {
            // Furre In View
            // (0:28) When anyone enters the bots view,
            Add(TriggerCategory.Cause,
                r => EnterView(r),
                "When anyone enters the bots view, ");

            // (0:28) When a furre named {..} enters the bots view
            Add(TriggerCategory.Cause,
                r => FurreNamedEnterView(r),
                "When a furre named {..} enters the bots view,");

            // Furre Leave View
            // (0:30) When anyone leaves the bots view,
            Add(TriggerCategory.Cause,
                r => LeaveView(r),
                "When anyone leaves the bots view, ");

            // (0:31) When a furre named {..} leaves the bots view
            Add(TriggerCategory.Cause,
                r => FurreNamedLeaveView(r),
                "When a furre named {..} leaves the bots view,");

            // (0:600) When the bot reads a description.
            Add(TriggerCategory.Cause,
                r => ReadTriggeringFurreParams(r),
                "When the bot sees a furre description,");

            // (1:600) and triggering furre's description contains {..}
            Add(TriggerCategory.Condition,
                r => DescContains(r),
                "and triggering furre\'s description contains {..}");

            // (1:601) and triggering furre's description does not contain {..}
            Add(TriggerCategory.Condition,
                r => NotDescContains(r),
                "and triggering furre\'s description does not contain {..}");

            // (1:602) and the furre named {..} description contains {..}
            Add(TriggerCategory.Condition,
                r => FurreNamedDescContains(r),
                "and the furre named {..} description contains {..}if they are in the dream,");

            // (1:603) and the furre named {..} description does not contain {..}
            Add(TriggerCategory.Condition,
                r => NotDescContainsFurreNamed(r),
                "and the furre named {..} description does not contain {..} if they are in the dream");

            // (1:604) and the triggering furre is male,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsGender(r),
                "and the triggering furre is gender #,");

            // (1:608) and the furre named {..}'s is male,
            Add(TriggerCategory.Condition,
                r => AndFurreNamedIsGender(r),
                "and the furre named {..}\'s is male if they are in the dream,");

            // (1:612) and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(TriggerCategory.Condition,
                r => TriggeringFurreSpecies(r),
                "and the trigger furre is Species # (please see http://www.furcadia.com/dsparams/ for info)");
            // (1:613) and the furre named {..} is Species # (please see http://www.furcadia.com/dsparams/ for info)
            Add(TriggerCategory.Condition,
                r => FurreNamedSpecies(r),
                "and the furre named {..} is Species # if they are in the dream (please see http://www.furcadia.com/dsparams/ for info)");
            // (1:614) and the triggering furre has wings of type #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreWings(r),
                "and the triggering furre has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)");
            // (1:615) and the triggering furre doesn't wings of type #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreNoWings(r),
                "and the triggering furre doesn\'t wings of type #, (please see http://www.furcadia.com/dsparams/ for info)");
            // (1:616) and the furre named {..} has wings of type #,
            Add(TriggerCategory.Condition,
                r => FurreNamedWings(r),
                "and the furre named {..} has wings of type #, (please see http://www.furcadia.com/dsparams/ for info)");
            // (1:617) and the furre named {..}  doesn't wings of type #,
            Add(TriggerCategory.Condition,
                r => FurreNamedNoWings(r),
                "and the furre named {..}  doesn\'t wings of type #, (please see http://www.furcadia.com/dsparams/ for info)");
            // (1:618) and the triggering furre is standing.
            Add(TriggerCategory.Condition,
                r => TriggeringFurreStanding(r),
                "and the triggering furre is standing.");
            // (1:619) and the triggering furre is sitting.
            Add(TriggerCategory.Condition,
                r => TriggeringFurreSitting(r),
                "and the triggering furre is sitting.");
            // (1:620) and the triggering furre is laying.
            Add(TriggerCategory.Condition,
                r => TriggeringFurreLaying(r),
                "and the triggering furre is laying.");
            // (1:621) and the triggering furre is facing NE,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreIsFacingDirection(r),
                "and the triggering furre is facing NE,");

            // (1:625) and the furre named {..} is standing.
            Add(TriggerCategory.Condition,
                r => FurreNamedStanding(r),
                "and the furre named {..} is standing.");
            // (1:626) and the furre named {..} is sitting.
            Add(TriggerCategory.Condition,
                r => FurreNamedSitting(r),
                "and the furre named {..} is sitting.");
            // (1:627) and the furre named {..} is laying.
            Add(TriggerCategory.Condition,
                r => FurreNamedLaying(r),
                "and the furre named {..} is laying.");
            // (1:628) and the furre named {..} is facing NE,
            Add(TriggerCategory.Condition,
                r => FurreNamedFacingIsFacingDirection(r),
                "and the furre named {..} is facing direction #,");

            // (5:600) set variable %Variable to the Triggering furre's description.
            Add(TriggerCategory.Effect,
                r => TriggeringFurreDescVar(r),
                "set variable %Variable to the Triggering furre\'s description.");
            // (5:601) set variable %Variable to the triggering furre's gender.
            Add(TriggerCategory.Effect,
                r => TriggeringFurreGenderVar(r),
                "set variable %Variable to the triggering furre\'s gender.");
            // (5:602) set variable %Variable to the triggering furre's species.
            Add(TriggerCategory.Effect,
                r => TriggeringFurreSpeciesVar(r),
                "set variable %Variable to the triggering furre\'s species.");
            // (5:604) set variable %Variable to the triggering furre's colors.
            Add(TriggerCategory.Effect,
                r => TriggeringFurreColorsVar(r),
                "set variable %Variable to the triggering furre\'s colors.");
            // (5:605) set variable %Variable to the furre named {..}'s gender if they are in the dream.
            Add(TriggerCategory.Effect,
                r => FurreNamedGenderVar(r),
                "set variable %Variable to the furre named {..}\'s gender if they are in the dream.");
            // (5:606) set variable %Variable to the furre named {..}'s species, if they are in the dream.
            Add(TriggerCategory.Effect,
                r => FurreNamedSpeciesVar(r),
                "set variable %Variable to the furre named {..}\'s species, if they are in the dream.");
            // (5:607) set variable %Variable to the furred named {..}'s description, if they are in the dream.
            Add(TriggerCategory.Effect,
                r => FurreNamedDescVar(r),
                "set variable %Variable to the furred named {..}\'s description, if they are in the dream.");
            // (5:608) set variable %Variable to the furre named {..}'s colors, if they are in the dream.
            Add(TriggerCategory.Effect,
                r => FurreNamedColorsVar(r),
                "set variable %Variable to the furre named {..}\'s colors, if they are in the dream.");
            // (5:609) set %Variable to the wings type the triggering furre is wearing.
            Add(TriggerCategory.Effect,
                r => TriggeringFurreWingsVar(r),
                "set %Variable to the wings type the triggering furre is wearing.");
            // (5:610) set %Variable to the wings type the furre named {..} is wearing.
            Add(TriggerCategory.Effect,
                r => FurreNamedWingsVar(r),
                "set %Variable to the wings type the furre named {..} is wearing.");
            // (0:601) When a furre moves,
            Add(TriggerCategory.Cause,
                r => ReadTriggeringFurreParams(r),
                "When a furre moves,");
            // (0:602) when a furre moves into (x,y),
            Add(TriggerCategory.Cause,
                r => MoveInto(r),
                "when a furre moves into (x,y),");
            // (1:900) and the triggering furre moved into/is standing at (x,y),
            Add(TriggerCategory.Condition,
                r => MoveInto(r),
                "and the triggering furre moved into/is standing at (x,y)");
            // (1:633) and the furre named {..} moved into/is standing at (x,y),
            Add(TriggerCategory.Condition,
                r => FurreNamedMoveInto(r),
            "and the furre named {..} moved into/is standing at (x,y),");
            // (1:634) and the triggering furre moved from (x,y),
            Add(TriggerCategory.Condition,
                r => MoveFrom(r), "and the triggering furre moved from (x,y),");
            // (1:635) and the furre named {..} moved from (x,y),
            Add(TriggerCategory.Condition,
                r => FurreNamedMoveFrom(r),
            "and the furre named {..} moved from (x,y),");
            // (1:638) and the triggering furre tried to move but stood still.
            Add(TriggerCategory.Condition,
                r => StoodStill(r),
                "and the triggering furre tried to move but stood still.");
            // (5:613) move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South=West)
            Add(TriggerCategory.Effect,
                r => MoveBot(r),
                "move the bot in direction # one space. (seven = North-West, nine = North-East, three = South-East, one = South-West)");
            // (5:614) turn the bot clock-wise one space.
            Add(TriggerCategory.Effect,
                r => TurnCW(r),
                "turn the bot clock-wise one space.");
            // (5:615) turn the bot counter-clockwise one space.
            Add(TriggerCategory.Effect,
                r => TurnCCW(r),
            "turn the bot counter-clockwise one space.");
            // (5:616) set variable %Variable to the X coordinate where the triggering furre moved into/is at.
            Add(TriggerCategory.Effect,
                r => SetCordX(r),
                "set variable %Variable to the X coordinate where the triggering furre moved into/is at.");
            // (5:617) set variable %Variable to the Y coordinate where the triggering furre moved into/is at.
            Add(TriggerCategory.Effect,
                r => SetCordY(r),
                "set variable %Variable to the Y coordinate where the triggering furre moved into/is at.");
            // (5:618) set variable %Variable to the X coordinate where the furre named {..} moved into/is at.
            Add(TriggerCategory.Effect,
                r => FurreNamedSetCordX(r),
                "set variable %Variable to the X coordinate where the furre named {..} moved into/is at.");
            // (5:619) set variable %Variable to the Y coordinate where the furre named {..} moved into/is at.
            Add(TriggerCategory.Effect,
                r => FurreNamedSetCordY(r),
            "set variable %Variable to the Y coordinate where the furre named {..} moved into/is at.");
            // (5:622) make the bot sit down.
            Add(TriggerCategory.Effect,
                r => BotSit(r),
            "make the bot sit down.");
            // (5:623) make the bot lay down.
            Add(TriggerCategory.Effect,
                r => BotLie(r),
                "make the bot lay down.");
            // (5:624) make the bot stand up.
            Add(TriggerCategory.Effect,
                r => BotStand(r),
                "make the bot stand up.");
            // (5:625) Move the bot  in this sequence {..} (one, sw, three, se, seven, nw, nine, or ne)
            Add(TriggerCategory.Effect,
                r => BotMoveSequence(r),
                "Move the bot  in this sequence {..} (one, sw, three, se, seven, nw, nine, or ne)");
        }

        private bool FurreNamedFacingIsFacingDirection(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        private bool TriggeringFurreIsFacingDirection(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private bool BotLie(TriggerReader reader)
        {
            return SendServer("`lie");
        }

        private bool BotMoveSequence(TriggerReader reader)
        {
            // TODO: http://bugtraq.tsprojects.org/view.php?id=55
            // Queue System?
            var directions = reader.ReadString();
            var r = new Regex(RGEX_Mov_Steps, RegexOptions.Compiled);
            MatchCollection m = r.Matches(directions);
            bool ServerSend = false;
            foreach (Match n in m)
            {
                if (n.Value.ToLower() == "ne")
                {
                    ServerSend = SendServer("`m9");
                }
                else if (n.Value.ToLower() == "se")
                {
                    ServerSend = SendServer("`m3");
                }
                else if (n.Value.ToLower() == "nw")
                {
                    ServerSend = SendServer("`m7");
                }
                else if (n.Value.ToLower() == "sw")
                {
                    ServerSend = SendServer("`m1");
                }
                else
                {
                    switch (int.Parse(n.Value))
                    {
                        case 1:
                        case 7:
                        case 3:
                        case 9:

                            ServerSend = SendServer("`m" + n.Value);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(m.ToString());
                    }
                }

                if (!ServerSend)
                {
                    break;
                }
            }

            return ServerSend;
        }

        private bool BotSit(TriggerReader reader)
        {
            return SendServer("`sit");
        }

        private bool BotStand(TriggerReader reader)
        {
            return SendServer("`stand");
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool DescContains(TriggerReader reader)
        {
            if (string.IsNullOrEmpty(Player.FurreDescription))
            {
                Logger.Warn("Description not found. Try looking at the furre first");
                return false;
            }

            return Player.FurreDescription.Contains(reader.ReadString());
        }

        private bool FurreNamedColorsVar(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var name = reader.ReadString();
            var Target = DreamInfo.Furres.GerFurreByName(name);
            Var.Value = Target.FurreColors.ToString();
            return Target.FurreID > 0;
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedDescContains(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            var Pattern = reader.ReadString();
            if (string.IsNullOrEmpty(Target.FurreDescription))
            {
                Logger.Warn("Description for {Target.Name}not found. Try looking at the furre first");
                return false;
            }

            return Target.FurreDescription.Contains(Pattern);
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedDescVar(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var name = reader.ReadString();
            var Target = DreamInfo.Furres.GerFurreByName(name);
            if (string.IsNullOrEmpty(Target.FurreDescription))
            {
                Logger.Warn("Description for {Target.Name}not found. Try looking at the furre first");
                return false;
            }

            Var.Value = Target.FurreDescription;
            return true;
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedGenderVar(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var name = reader.ReadString();
            var Target = DreamInfo.Furres.GerFurreByName(name);
            switch (Target.FurreColors.Gender)
            {
                case -1:
                    Logger.Warn("Gender not found. Try looking at the furre first");
                    return false;

                case 0:
                case 1:
                    Var.Value = Target.FurreColors.Gender;
                    break;
            }
            return true;
        }

        private bool FurreNamedLaying(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool AndFurreNamedIsGender(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool FurreNamedMoveFrom(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool FurreNamedMoveInto(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedNoWings(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            switch (Target.LastStat)
            {
                case -1:
                    Logger.Warn("Wings type not found. Try looking at the furre first");
                    return false;

                case (0 | 1):
                    return (Target.FurreColors.Wings != reader.ReadNumber());
            }
            return false;
        }

        private bool FurreNamedSetCordX(TriggerReader reader)
        {
            var Cord = reader.ReadVariable(true);
            var tPlayer = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            Cord.Value = tPlayer.Position.X;
            return true;
        }

        private bool FurreNamedSetCordY(TriggerReader reader)
        {
            var Cord = reader.ReadVariable(true);
            var tPlayer = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            Cord.Value = tPlayer.Position.Y;
            return true;
        }

        private bool FurreNamedSitting(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool FurreNamedSpecies(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedSpeciesVar(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var name = reader.ReadString();
            var TargetFurre = DreamInfo.Furres.GerFurreByName(name);
            switch (TargetFurre.LastStat)
            {
                case -1:
                    Logger.Warn("Species not found. Try looking at the furre first");
                    return false;

                case 0:
                case 1:
                    Var.Value = TargetFurre.FurreColors.Species;
                    break;
            }
            return true;
        }

        private bool FurreNamedStanding(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        private bool FurreNamedStoodStill(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedWings(TriggerReader reader)
        {
            var Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            switch (Target.LastStat)
            {
                case -1:
                    Logger.Warn("Wings type not found. Try looking at the furre first");
                    return false;

                case 0:
                case 1:
                    return true;
            }
            return false;
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool FurreNamedWingsVar(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var TargetFurre = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            switch (TargetFurre.LastStat)
            {
                case -1:
                    Logger.Warn("Wings type not found. Try looking at the furre first");
                    return false;

                case 0:
                case 1:
                    Var.Value = TargetFurre.FurreColors.Wings;
                    break;
            }
            return true;
        }

        private bool MoveBot(TriggerReader reader)
        {
            var Direction = reader.ReadNumber();
            switch (Direction)
            {
                case 7:
                case 9:
                case 3:
                case 1:
                    return SendServer("`m" + Direction.ToString());

                default:
                    throw new MonkeyspeakException("Directions must be in the form of  7, 9, 3, or 1");
            }
        }

        private bool MoveFrom(TriggerReader reader)
        {
            var X = reader.ReadNumber();
            var Y = reader.ReadNumber();

            return Player.LastPosition.X == X && Player.LastPosition.Y == Y;
        }

        private bool MoveInto(TriggerReader reader)
        {
            var X = reader.ReadNumber();
            var Y = reader.ReadNumber();

            return Player.Position.X == X && Player.Position.Y == Y;
        }

        /// <summary>
        /// (1:601) and triggering furre's description does not contain {..},
        /// <para>
        /// NOTE: This line only works after the bot has looked at the
        ///       specified furre
        /// </para>
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <exception cref="MonkeySpeakException">
        /// Thrown when a Furre doesn't have a a description. This can be
        /// prevented by looking at the triggering-furre first
        /// </exception>
        /// <returns>
        /// true on success
        /// </returns>
        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool NotDescContains(TriggerReader reader)
        {
            if (string.IsNullOrEmpty(Player.FurreDescription))
            {
                Logger.Warn("Description not found. Try looking at the furre first");
                return false;
            }

            return !Player.FurreDescription.Contains(reader.ReadString());
        }

        /// <summary>
        /// (1:603) and the furre named {..} description does not contain {..}
        /// <para>
        /// NOTE: This line only works after the bot has looked at the
        ///       specified furre
        /// </para>
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool NotDescContainsFurreNamed(TriggerReader reader)
        {
            Furre Target = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            if (string.IsNullOrEmpty(Target.FurreDescription))
            {
                Logger.Warn("Description not found. Try looking at the furre first");
                return false;
            }

            return !Target.FurreDescription.Contains(reader.ReadString());
        }

        /// <summary>
        /// (5:616) set variable %Variable to the X coordinate where the
        /// triggering furre moved into/is at.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool SetCordX(TriggerReader reader)
        {
            reader.ReadVariable(true).Value = Player.Position.X;
            return true;
        }

        /// <summary>
        /// (5:617) set variable %Variable to the Y coordinate where the
        /// triggering furre moved into/is at.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool SetCordY(TriggerReader reader)
        {
            reader.ReadVariable(true).Value = Player.Position.Y;
            return true;
        }

        /// <summary>
        /// (1:638) and the triggering furre tried to move but stood still.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool StoodStill(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (5:604) set variable %Variable to the triggering furre's colors.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreColorsVar(TriggerReader reader)
        {
            reader.ReadVariable(true).Value = Player.FurreColors.ToString();
            return true;
        }

        /// <summary>
        /// (5:600) set variable %Variable to the Triggering furre's description.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool TriggeringFurreDescVar(TriggerReader reader)
        {
            if (Player.FurreDescription == null)
            {
                Logger.Warn("Description not found, Try looking at the furre first.");
                return false;
            }

            reader.ReadVariable(true).Value = Player.FurreDescription;
            return true;
        }

        /// <summary>
        /// Triggerings the furre is gender.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private bool TriggeringFurreIsGender(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (5:601 set variable %Variable to the triggering furre's gender.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <exception cref="MonkeySpeakException">
        /// Thrown when a Furre doesn't have a a description. This can be
        /// prevented by looking at the triggering-furre first
        /// </exception>
        /// <returns>
        /// true on success
        /// </returns>
        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool TriggeringFurreGenderVar(TriggerReader reader)
        {
            if (Player.FurreColors.Gender == -1 || Player.LastStat == -1)
            {
                Logger.Warn("Wings type not found, Try looking at the furre first");
                return false;
            }

            var Var = reader.ReadVariable(true);
            if (Player.LastStat == 0 || Player.LastStat == 1)
            {
                Var.Value = Player.FurreColors.Gender;
            }

            return true;
        }

        /// <summary>
        /// (1:x) and the triggering furre is laying.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreLaying(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (1:615) and the triggering furre doesn't wings of type #
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <exception cref="MonkeySpeakException">
        /// Thrown when a Furre doesn't have a a description. This can be
        /// prevented by looking at the triggering-furre first
        /// </exception>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreNoWings(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (1:x) and the triggering furre is sitting.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreSitting(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool TriggeringFurreSpecies(TriggerReader reader)
        {
            if (Player.FurreColors.Gender == -1 || Player.LastStat == -1)
            {
                Logger.Warn("Wings type not found, Try looking at the furre first");
                return false;
            }

            var Spec = reader.ReadNumber();
            if (Player.LastStat == 0 || Player.LastStat == 1)
            {
                return true;
            }

            return false;
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool TriggeringFurreSpeciesVar(TriggerReader reader)
        {
            switch (Player.LastStat)
            {
                case -1:
                    Logger.Warn("Species not found. Try looking at the furre first");
                    return false;

                case (0 | 1):
                    reader.ReadVariable(true).Value = Player.FurreColors.Species;
                    break;
            }
            return true;
        }

        /// <summary>
        /// (1:x) and the triggering furre is standing.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool TriggeringFurreStanding(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (1:614) and the triggering furre has wings of type #
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <exception cref="MonkeySpeakException">
        /// Thrown when a Furre doesn't have a a description. This can be
        /// prevented by looking at the triggering-furre first
        /// </exception>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreWings(TriggerReader reader)
        {
            throw new NotImplementedException();
        }

        [TriggerDescription(" This line only works after the bot has looked at the specified furre")]
        private bool TriggeringFurreWingsVar(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            switch (Player.LastStat)
            {
                case -1:
                    Logger.Warn("Wings type not found. Try looking at the furre first");
                    return false;

                case 0:
                    Var.Value = Player.FurreColors.Wings;
                    break;

                case 1:
                    if (Player.FurreColors.Wings == -1)
                    {
                        if (Player.FurreColors.Wings == -1)
                        {
                            Logger.Warn("Wings type not found, Try looking at the furre first");
                            return false;
                        }

                        Var.Value = Player.FurreColors.Wings;
                    }
                    else
                    {
                        Var.Value = Player.FurreColors.Wings;
                    }

                    break;
            }
            return true;
        }

        /// <summary>
        /// (5:615) turn the bot counter-clockwise one space.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TurnCCW(TriggerReader reader)
        {
            return SendServer("`<");
        }

        /// <summary>
        /// (5:614) turn the bot clock-wise one space.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TurnCW(TriggerReader reader)
        {
            return SendServer("`>");
        }

        /// <summary>
        /// (0:28) When anyone enters the bots view,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool EnterView(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return Player.Visible == Player.WasVisible;
        }

        /// <summary>
        /// (0:30) When anyone leaves the bots view,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool LeaveView(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            return Player.Visible == Player.WasVisible;
        }

        /// <summary>
        /// (0:29) When a furre named {..} enters the bots view,"
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>true on success</returns>
        private bool FurreNamedEnterView(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var tPlayer = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            return tPlayer.Visible == tPlayer.WasVisible;
        }

        /// <summary>
        /// (0:31) When a furre named {..} leaves the bots view,
        /// </summary>
        /// <param name="reader"><see cref="TriggerReader"/></param>
        /// <returns>true on success</returns>
        private bool FurreNamedLeaveView(TriggerReader reader)
        {
            ReadTriggeringFurreParams(reader);
            var tPlayer = DreamInfo.Furres.GerFurreByName(reader.ReadString());
            return tPlayer.Visible == tPlayer.WasVisible;
        }

        #endregion Private Methods
    }
}