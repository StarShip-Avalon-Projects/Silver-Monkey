using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// Log stream for normal logs
    /// </summary>
    public class LogStream : IDisposable
    {
        #region Private Fields

        private bool disposedValue;
        private LogStreamOptions Options;

        private List<string> LogList = new List<string>();

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogStream"/> class.
        /// </summary>
        public LogStream()
        {
            Options = new LogStreamOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogStream"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public LogStream(LogStreamOptions options)
        {
            Options = options;
        }

        private string LogFilePath => Path.Combine(Options.LogPath, Options.GetLogFileName());

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //  Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
        }

        /// <summary>
        /// Write a line to the log file
        /// </summary>
        /// <param name="Message">
        /// </param>
        public void WriteLine(string Message)
        {
            if (!Options.Enabled)
            {
                return;
            }
            // TODO: Fix the Time options on this
            Message = $"{DateTime.Now.ToString("MM/dd/yyyy H:mm:ss")}: {Message}";

            using (var mutex = new Mutex(false, Options.GetLogFileName(), out bool test))
            {
                if (mutex.WaitOne(800, true))
                    try
                    {
                        using (FileStream fStream = new FileStream(LogFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 4096))
                        using (StreamWriter ioFile = new StreamWriter(fStream))
                        {
                            foreach (string line in LogList.ToArray())
                            {
                                ioFile.WriteLine(line);
                            }

                            LogList.Clear();
                            ioFile.WriteLine(Message);
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LogList.Clear();
                }
            }

            disposedValue = true;
        }

        #endregion Protected Methods
    }
}