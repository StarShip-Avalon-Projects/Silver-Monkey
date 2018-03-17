#region Usings

using furcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

#endregion Usings

namespace MonkeyCore.Logging
{
    /// <summary>
    /// MonkeyCore Extentions for <see cref="Logging"/>
    /// </summary>
    public static class LoggingExtentions
    {
        /// <summary>
        /// Converts <see cref="MsLog.Level"/> to <see cref="Level"/>
        /// </summary>
        /// <param name="level">The Monkeyspeak level.</param>
        /// <returns></returns>
        public static Level ToLevel(this MsLog.Level level)
        {
            return (Level)level;
        }

        /// <summary>
        /// Converts <see cref="furcLog.Level"/> to <see cref="Level"/>
        /// </summary>
        /// <param name="level">The Furcadia level.</param>
        /// <returns></returns>
        public static Level ToLevel(this furcLog.Level level)
        {
            return (Level)level;
        }
    }
}