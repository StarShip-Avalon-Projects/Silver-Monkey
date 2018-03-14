namespace MonkeyCore.Logging
{
    /// <summary>
    ///
    /// </summary>
    public interface ILogOutput : Monkeyspeak.Logging.ILogOutput, Furcadia.Logging.ILogOutput
    {
        /// <summary>
        /// Logs the specified log MSG.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        void Log(LogMessage logMsg);
    }
}