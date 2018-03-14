using MsLog = Monkeyspeak.Logging;
using FurcLog = Furcadia.Logging;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// Unverisal Logging interface that unites Furcadia.Logging and Monkeyspeak.Logging
    /// </summary>
    public interface ILogOutput : MsLog.ILogOutput, FurcLog.ILogOutput
    {
        /// <summary>
        /// Logs the specified log Message.
        /// </summary>
        /// <param name="logMsg">The Message object.</param>
        void Log(LogMessage logMsg);
    }
}