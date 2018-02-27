using Furcadia.Net.Utils.ServerObjects;
using Libraries.Dice;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System.Linq;

// https://cms.furcadia.com/help/commands/keycommands/generalcommands/basiccommands#
// roll "die"d"sides" (ex: roll 1d6)
// TODO: Check Dice Commands, Furcadia Docs see to be Missing a +/= Modifyer

namespace Libraries
{
    /// <summary>
    /// Monkeyspeak triggers for Dice rolls
    ///
    /// </summary>
    /// <seealso cref="Libraries.MonkeySpeakLibrary" />
    public class MsDice : MonkeySpeakLibrary
    {
        #region Private Fields

        private DiceObject dice;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 130;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initializes this instance. Add your trigger handlers here.
        /// </summary>
        /// <param name="args">
        /// Parametized argument of vars to use to pass runtime vars to a library at initialization
        /// </param>
        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            Add(TriggerCategory.Cause,
                r => RollNumber(r),
                "When the bot rolls #d#,");

            Add(TriggerCategory.Cause,
                r => RollNumberPlusModifyer(r),
                "When the bot rolls #d#+#,");

            Add(TriggerCategory.Cause,
                 r => RollNumberMinusModifyer(r),
                 "When the bot rolls #d#-#,");

            Add(TriggerCategory.Cause,
                r => RollNumber(r),
                "When a furre rolls #d#,");

            Add(TriggerCategory.Cause,
                r => RollNumberPlusModifyer(r),
                "When a furre rolls #d#+#,");

            Add(TriggerCategory.Cause,
                  r => RollNumberMinusModifyer(r),
                  "When a furre rolls #d#-#,");

            Add(TriggerCategory.Cause,
                AnyOneRollAnyThing,
                "When any one rolls anything,");

            Add(TriggerCategory.Condition,
                r => DiceResultNumberOrHigher(r),
                "and the dice roll result is # or higher,");

            Add(TriggerCategory.Condition,
                r => DiceResultNumberOrlower(r),
                "and the dice roll result is # or lower,");

            Add(TriggerCategory.Effect,
                DicePlusNumber,
                "set variable % to the total of rolling # dice with # sides plus #.");

            Add(TriggerCategory.Effect,
                DiceMinusNumber,
                "set variable % to the total of rolling # dice with # sides minus #.");

            Add(TriggerCategory.Effect,
                TrigFurreRolledVariable,
                "set variable %Variable to the number of the dice result that the triggering furre rolled.");

            Add(TriggerCategory.Effect,
                RollDice,
                "roll # furcadia dice with # sides. (optional Message {...})");

            Add(TriggerCategory.Effect,
                RollDicePlus,
                "roll # furcadia dice with # sides plus #. (optional Message {...})");

            Add(TriggerCategory.Effect,
                RollDiceMinus,
                "roll # furcadia dice with # sides minus #. (optional Message {...})");
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
        }

        #endregion Public Methods

        #region Private Methods

        [TriggerVariableParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        private static bool DiceMinusNumber(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var Number = reader.ReadNumber();
            var sides = reader.ReadNumber();
            var NumberPlus = reader.ReadNumber();
            var dice = new DiceRollCollection(Number, sides, NumberPlus, '-');

            Var.Value = dice.RollAll();
            return true;
        }

        [TriggerVariableParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        private static bool DicePlusNumber(TriggerReader reader)
        {
            var Var = reader.ReadVariable(true);
            var Number = reader.ReadNumber();
            var sides = reader.ReadNumber();
            var NumberPlus = reader.ReadNumber();
            var dice = new DiceRollCollection(Number, sides, NumberPlus);

            Var.Value = dice.RollAll();
            return true;
        }

        [TriggerParameter("Dice Object from Server")]
        private bool AnyOneRollAnyThing(TriggerReader reader)
        {
            var DiceParam = reader.GetParametersOfType<DiceObject>().FirstOrDefault();
            if (DiceParam != dice)
            {
                dice = DiceParam;
            }
            return true;
        }

        [TriggerNumberParameter]
        private bool DiceResultNumberOrHigher(TriggerReader reader)
        {
            var result = reader.ReadNumber();
            return result <= dice.DiceResult;
        }

        [TriggerNumberParameter]
        private bool DiceResultNumberOrlower(TriggerReader reader)
        {
            var result = reader.ReadNumber();
            return result >= dice.DiceResult;
        }

        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerStringParameter]
        private bool RollDice(TriggerReader reader)
        {
            var DiceCount = reader.ReadNumber();
            var NumberOfSides = reader.ReadNumber();
            var Message = reader.ReadString();
            return SendServer($"roll {DiceCount.ToString()}d{NumberOfSides.ToString()} {Message}");
        }

        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        private bool RollDiceMinus(TriggerReader reader)
        {
            var count = reader.ReadNumber();
            var side = reader.ReadNumber();
            var modifyer = reader.ReadNumber();
            return SendServer($"roll {count.ToString()}d{side.ToString()} {modifyer.ToString()}");
        }

        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        private bool RollDicePlus(TriggerReader reader)
        {
            var count = reader.ReadNumber();
            var side = reader.ReadNumber();
            var modifyer = reader.ReadNumber();
            return SendServer($"roll {count.ToString()}d{side.ToString()} {modifyer.ToString()}");
        }

        [TriggerParameter("Dice Object from Server")]
        private bool RollNumber(TriggerReader reader)
        {
            var DiceParam = reader.GetParametersOfType<DiceObject>().FirstOrDefault();
            if (DiceParam != dice)
            {
                dice = DiceParam;
            }

            var diceCount = reader.ReadNumber();
            var sides = reader.ReadNumber();
            if (sides != dice.DiceSides)
                return false;

            if (dice.DiceCount != diceCount)
                return false;

            return dice.DiceResult >= reader.ReadNumber();
        }

        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerParameter("Dice Object from Server")]
        private bool RollNumberMinusModifyer(TriggerReader reader)
        {
            var DiceCount = reader.ReadNumber();
            var sides = reader.ReadNumber();
            var DiceModifyer = reader.ReadNumber();
            var ExpectedDiceResult = reader.ReadNumber();

            var DiceParam = reader.GetParametersOfType<DiceObject>().FirstOrDefault();
            if (DiceParam != dice)
                dice = DiceParam;

            if (dice.DiceCompnentMatch != '-')
                return false;

            if (dice.DiceModifyer != DiceModifyer)
                return false;

            if (sides != dice.DiceSides)
                return false;

            if (dice.DiceCount != DiceCount)
                return false;

            return dice.DiceResult >= ExpectedDiceResult;
        }

        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerNumberParameter]
        [TriggerParameter("Dice Object from Server")]
        private bool RollNumberPlusModifyer(TriggerReader reader)
        {
            var DiceCount = reader.ReadNumber();
            var sides = reader.ReadNumber();
            var DiceModifyer = reader.ReadNumber();
            var ExpectedDiceResult = reader.ReadNumber();

            var DiceParam = reader.GetParametersOfType<DiceObject>().FirstOrDefault();
            if (DiceParam != dice)
                dice = DiceParam;

            if (dice.DiceCompnentMatch != '+')
                return false;

            if (dice.DiceModifyer != DiceModifyer)
                return false;

            if (sides != dice.DiceSides)
                return false;

            if (dice.DiceCount != DiceCount)
                return false;

            return dice.DiceResult >= ExpectedDiceResult;
        }

        [TriggerNumberParameter]
        private bool TrigFurreRolledVariable(TriggerReader reader)
        {
            reader.ReadVariable(true).Value = dice.DiceResult;
            return true;
        }

        #endregion Private Methods
    }
}