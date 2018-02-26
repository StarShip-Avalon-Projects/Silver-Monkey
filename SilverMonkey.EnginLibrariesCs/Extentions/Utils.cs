/// <summary>
/// General Utility functions we haven't found a home for yet
/// </summary>
using System;

namespace Extentions
{
    /// <summary>
    /// General Utilities with Phoenix Speak in mind
    /// </summary>
    public sealed class Utilities
    {
        #region Public Methods

        /// <summary>
        /// Convert DateTime to Unixtimestamp
        /// </summary>
        /// <param name="dTime">The Specified DataTime</param>
        /// <returns></returns>
        public static double DateTimeToUnixTimestamp(DateTime dTime)
        {
            return ((DateTimeOffset)dTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Letter increment for walking PS tables
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static char IncrementLetter(string Input)
        {
            Input = Input.Substring(0, 1);
            int i = Input[0];
            char test;
            switch (Input)
            {
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                case "G":
                case "H":
                case "I":
                case "J":
                case "K":
                case "L":
                case "M":
                case "N":
                case "O":
                case "P":
                case "Q":
                case "R":
                case "S":
                case "T":
                case "U":
                case "V":
                case "W":
                case "X":
                case "Y":
                case "Z":

                case "a":
                case "b":
                case "c":
                case "d":
                case "e":
                case "f":
                case "g":
                case "h":
                case "i":
                case "j":
                case "k":
                case "l":
                case "m":
                case "n":
                case "o":
                case "p":
                case "q":
                case "r":
                case "s":
                case "t":
                case "u":
                case "v":
                case "w":
                case "x":
                case "y":
                case "z":

                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    test = (char)(i + 1);
                    break;

                case "9":
                    test = 'a';
                    break;

                default:
                    test = '{';
                    break;
            }
            return test;
        }

        /// <summary>
        /// Converts a number representing a Unix Time stamp and converts it to a usable DateTime format
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns>DataTime</returns>
        public static DateTime UnixTimeStampToDateTime(ref double unixTimeStamp)
        {
            //  Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        #endregion Public Methods
    }
}