using System.Collections.Generic;

namespace Libraries.Dice
{
    /// <summary>
    /// Grab your dice and shake them till you're ready to roll
    /// </summary>
    public class DiceRollCollection
    {
        private List<Die> Dice = new List<Die>();
        private double Offset;
        private char DiceModifyer = '+';
        private double total = 0;

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

        public double Result => total;

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

        //
        public DiceRollCollection(double NumberDice, double NumberSides, double Offset = 0, char Modifyer = '+')
        {
            this.DiceModifyer = Modifyer;
            this.Offset = Offset;

            for (double I = 0; I <= NumberDice - 1; I++)
            {
                Add(NumberSides);
            }
        }
    }
}