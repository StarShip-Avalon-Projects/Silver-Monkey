using System;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// Log Configuration options
    /// </summary>
    public class LogStreamOptions
    {
        #region Private Fields

        private const string DateFormat = "MM_dd_yyyy_H-mm-ss";
        private const string DefaultLogFile = "Default";
        private const string ext = ".log";

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogStreamOptions"/> class.
        /// </summary>
        /// <param name="LogNameBase">The log name base.</param>
        /// <param name="LogEnabled">if set to <c>true</c> [log enabled].</param>
        public LogStreamOptions(string LogNameBase = DefaultLogFile, bool LogEnabled = false)
        {
            this.LogNameBase = LogNameBase;
            Enabled = LogEnabled;
            LogPath = IO.Paths.SilverMonkeyLogPath;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LogStreamOptions"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the index of the log.
        /// </summary>
        /// <value>
        /// The index of the log.
        /// </value>
        public int LogIdx { get; set; }

        /// <summary>
        /// Gets or sets the log name base.
        /// </summary>
        /// <value>
        /// The log name base.
        /// </value>
        public string LogNameBase { get; set; }

        /// <summary>
        /// Gets or sets the log option.
        /// </summary>
        /// <value>
        /// The log option.
        /// </value>
        public short LogOption { get; set; }

        /// <summary>
        /// Gets or sets the log path.
        /// </summary>
        /// <value>
        /// The log path.
        /// </value>
        public string LogPath { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the name of the logfile.
        /// </summary>
        /// <returns></returns>
        public string GetLogFileName()
        {
            string LogFile = null;
            switch (LogOption)
            {
                case -1:
                    LogFile = DefaultLogFile;
                    break;

                case 0:
                    LogFile = LogNameBase;
                    break;

                case 1:
                    LogIdx++;
                    LogFile = LogNameBase + LogIdx.ToString();
                    break;

                case 2:
                    DateTime.Now.ToString(DateFormat);
                    break;
            }
            return LogFile + ext;
        }

        #endregion Public Methods
    }
}