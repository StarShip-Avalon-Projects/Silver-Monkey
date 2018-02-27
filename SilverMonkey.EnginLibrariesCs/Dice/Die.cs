using System;

namespace Libraries.Dice
{
    /// <summary>
    /// Single Die object for Silver Monkey to generate a Dice Roll
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

        /// <summary>
        /// Gets or sets the face count.
        /// </summary>
        /// <value>
        /// The face count.
        /// </value>
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

        /// <summary>
        /// Gets the side of the die we land on
        /// </summary>
        /// <value>
        /// <see cref="Double"/>
        /// </value>
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
        /// the side of the die as <see cref="Double"/>
        /// </returns>
        public double Roll()
        {
            value = faceSelector.Next(1, (int)FaceCount);
            return Value;
        }
    }
}