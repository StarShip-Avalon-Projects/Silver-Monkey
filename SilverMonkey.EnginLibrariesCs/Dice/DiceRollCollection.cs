using System.Collections.Generic;

namespace Libraries.Dice
{
    /// <summary>
    /// Grab your dice and shake them till you're ready to roll
    /// </summary>
    public class DiceRollCollection : System.Collections.ObjectModel.Collection<Die>
    {
        private List<Die> Dice = new List<Die>();
        private double Offset;
        private char DiceModifyer = '+';

        /// <summary>
        /// Mix up the Dice in hand and roll them
        /// </summary>
        /// <returns>
        /// Sum of the result as <see cref="Double"/>
        /// </returns>
        public double RollAll()
        {
            double total = 0;
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

        public DiceRollCollection(double NumberDice, double NumberSides, double Offset = 0, char Modifyer = '+')
        {
            this.DiceModifyer = Modifyer;
            this.Offset = Offset;

            for (double I = 0; I <= NumberDice - 1; I++)
            {
                Dice.Add(new Die(NumberSides));
            }
        }
    }
}