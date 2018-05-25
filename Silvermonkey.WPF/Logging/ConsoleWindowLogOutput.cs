using MonkeyCore.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using SilverMonkey.Extentions;

using MsLog = Monkeyspeak.Logging;
using FurcLog = Furcadia.Logging;

namespace SilverMonkey.Logging
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="MonkeyCore.Logging.ConsoleLogOutput" />
    /// <seealso cref="Monkeyspeak.Logging.ILogOutput" />
    /// <seealso cref="Furcadia.Logging.ILogOutput" />
    public class ConsoleWindowLogOutput : ILogOutput, MsLog.ILogOutput, FurcLog.ILogOutput
    {
        private readonly RichTextBox logBox;
        private readonly Level level;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWindowLogOutput"/> class.
        /// </summary>
        /// <param name="LogBox">The log box.</param>
        /// <param name="level">The level.</param>
        public ConsoleWindowLogOutput(RichTextBox LogBox, Level level = Level.Error)
        {
            this.logBox = LogBox;
            this.level = level;
        }

        /// <summary>
        /// Logs the specified log MSG.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        public void Log(MsLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }

        /// <summary>
        /// Logs the specified log MSG.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        public void Log(FurcLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }

        /// <summary>
        /// Logs the specified log MSG.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        public void Log(LogMessage logMsg)
        {
            if (logMsg.message == null || logMsg.Level != level) return;

            logBox.Dispatcher.Invoke(() =>
            {
                logMsg = BuildMessage(ref logMsg);
                Brush brush = Brushes.Black;
                switch (logMsg.Level)
                {
                    case Level.Error:
                        brush = Brushes.Red;
                        break;

                    case Level.Warning:
                        brush = Brushes.DarkOrange;
                        break;

                    case Level.Debug:
                        brush = Brushes.DarkCyan;
                        break;
                }
                logBox.AppendParagraph(logMsg.message, brush);
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected LogMessage BuildMessage(ref LogMessage msg)
        {
            var level = msg.Level;
            var text = msg.message;
            var sb = new StringBuilder();
            sb.Append('[')
              .Append(level.ToString().ToUpper())
              .Append(']')
              .Append("Thread+" + msg.Thread.ManagedThreadId)
              .Append(' ')
              //.Append(msg.TimeStamp.ToString("dd-MMM-yyyy")).Append(' ')
              .Append((msg.TimeStamp - Process.GetCurrentProcess().StartTime).ToString(@"hh\:mm\:ss\:fff"))
              .Append(" - ")
              .Append(text);
            msg.message = sb.ToString();
            return msg;
        }
    }
}