using System;

namespace Libraries.Dice
{
    /// <summary>
    /// Single Die object for Silver Monkey generating a Dice Roll
    /// </summary>
    public class Die
    {
        private static Random faceSelector = new Random();

        private double faceCount;

        private double value;

        public Die(double faceCount)
        {
            if (faceCount < 1)
            {
                throw new ArgumentOutOfRangeException("faceCount", "Dice must have one or more faces.");
            }

            this.faceCount = faceCount;
        }

        public double FaceCount
        {
            get
            {
                return faceCount;
            }
            set
            {
                faceCount = value;
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Roll a single die
        /// </summary>
        /// <returns>
        /// <see cref="Double"/>
        /// </returns>
        public double Roll()
        {
            value = faceSelector.Next(1, (int)FaceCount);
            return Value;
        }
    }
}