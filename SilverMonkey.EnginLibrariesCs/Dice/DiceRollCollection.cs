using System.Collections.Generic;

namespace Libraries.Dice
{
    /// <summary>
    /// Grab your dice and shake them till you're ready to roll
    /// </summary>
    public class DiceRollCollection
    {
        #region Private Fields

        private List<Die> Dice = new List<Die>();
        private char DiceModifyer = '+';
        private double Offset;
        private double total = 0;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiceRollCollection"/> class.
        /// </summary>
        /// <param name="NumberDice">The number dice.</param>
        /// <param name="NumberSides">The number sides.</param>
        /// <param name="Offset">The offset.</param>
        /// <param name="Modifyer">The modifyer.</param>
        public DiceRollCollection(double NumberDice, double NumberSides, double Offset = 0, char Modifyer = '+')
        {
            this.DiceModifyer = Modifyer;
            this.Offset = Offset;

            for (double I = 0; I <= NumberDice - 1; I++)
            {
                Add(NumberSides);
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public double Result => total;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Adds the specified die.
        /// </summary>
        /// <param name="die">The die.</param>
        public void Add(Die die)
        {
            Dice.Add(die);
        }

        /// <summary>
        /// Add a Die with the number of sides specified
        /// </summary>
        /// <param name="sides">The sides.</param>
        public void Add(double sides)
        {
            Dice.Add(new Die(sides));
        }

        /// <summary>
        /// Mix up the Dice in hand and roll them
        /// </summary>
        /// <returns>
        /// Sum of the result as <see cref="Double"/>
        /// </returns>
        public double Roll()
        {
            foreach (Die die in Dice)
            {
                total += die.Roll();
            }
            if (DiceModifyer == '+')
                total += Offset;
            else if (DiceModifyer == '-')
                total -= Offset;
            return total;
        }

        #endregion Public Methods
    }
}