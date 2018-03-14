using System;

namespace Libraries.Dice
{
    /// <summary>
    /// Single Die object for Silver Monkey to generate a Dice Roll
    /// </summary>
    public class Die
    {
        private static Random faceSelector = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Die"/> class.
        /// </summary>
        /// <param name="faceCount">The face count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">faceCount - Dice must have one or more faces.</exception>
        public Die(double faceCount)
        {
            if (faceCount < 1)
            {
                throw new ArgumentOutOfRangeException("faceCount", "Dice must have one or more faces.");
            }

            FaceCount = faceCount;
        }

        /// <summary>
        /// Gets or sets the face count.
        /// </summary>
        /// <value>
        /// The face count.
        /// </value>
        public double FaceCount { get; set; }

        /// <summary>
        /// Gets the side of the die we land on
        /// </summary>
        /// <value>
        /// <see cref="Double"/>
        /// </value>
        public double Value { get; private set; }

        /// <summary>
        /// Roll a single die
        /// </summary>
        /// <returns>
        /// the side of the die as <see cref="Double"/>
        /// </returns>
        public double Roll()
        {
            Value = faceSelector.Next(1, (int)FaceCount);
            return Value;
        }
    }
}