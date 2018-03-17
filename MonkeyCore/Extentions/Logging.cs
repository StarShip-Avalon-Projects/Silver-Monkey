using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using furcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;
using MonkeyCore.Logging;

namespace MonkeyCore.Logging
{
    public static class LoggingExtentions
    {
        public static Level ToLevel(this MsLog.Level level)
        {
            return (Level)level;
        }

        public static Level ToLevel(this furcLog.Level level)
        {
            return (Level)level;
        }
    }
}