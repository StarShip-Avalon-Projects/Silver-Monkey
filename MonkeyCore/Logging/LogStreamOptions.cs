using static System.DateTime;

namespace MonkeyCore.Logging
{
    public class LogStreamOptions
    {
        private const string ext = ".log";

        public LogStreamOptions(string paramLogNameBase = "Default", bool paramEnabled = false)
        {
            LogNameBase = paramLogNameBase;
            Enabled = paramEnabled;
            LogPath = IO.Paths.SilverMonkeyLogPath;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LogStreamOptions"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        public int LogIdx { get; set; }

        public string LogNameBase { get; set; }

        public short LogOption { get; set; }

        public string LogPath { get; set; }

        public string GetLogName()
        {
            string LogFile = null;
            switch (LogOption)
            {
                case 0:
                    LogFile = LogNameBase;
                    break;

                case 1:
                    LogIdx++;
                    LogFile = LogNameBase + LogIdx.ToString();
                    break;

                case 2:
                    Now.ToString("MM_dd_yyyy_H-mm-ss");
                    break;
            }
            return LogFile + ext;
        }
    }
}