using System;
using System.Collections.Generic;
using System.IO;

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

        private List<string> Stack = new List<string>();

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
            try
            {
                using (FileStream fStream = new FileStream(LogFilePath, FileMode.Append))
                using (StreamWriter ioFile = new StreamWriter(fStream))
                {
                    foreach (string line in Stack.ToArray())
                    {
                        ioFile.WriteLine(line);
                    }

                    Stack.Clear();
                    ioFile.WriteLine(Message);
                }
            }
            catch (IOException ex)
            {
                if (ex.Message.StartsWith("The process cannot access the file")
                    && ex.Message.EndsWith("because it is being used by another process."))
                {
                    Stack.Add(Message);
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        //  IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stack.Clear();
                }
            }

            disposedValue = true;
        }

        #endregion Protected Methods
    }
}