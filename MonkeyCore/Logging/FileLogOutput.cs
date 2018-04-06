using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using furcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// </summary>
    /// <seealso cref="ILogOutput"/>
    /// <seealso cref="System.IEquatable{Logging.FileLogOutput}"/>
    public class FileLogOutput : ILogOutput, MsLog.ILogOutput, furcLog.ILogOutput, IEquatable<FileLogOutput>
    {
        private readonly Level level;
        private readonly string filePath;

        public FileLogOutput(string rootFolder, Level level = Level.Error)
        {
            this.level = level;
            if (Assembly.GetEntryAssembly() != null)
                filePath = Path.Combine(rootFolder, $"{Assembly.GetEntryAssembly().GetName().Name}.{this.level}.log");
            else if (Assembly.GetCallingAssembly() != null)
                filePath = Path.Combine(rootFolder, $"{Assembly.GetCallingAssembly().GetName().Name}.{this.level}.log");
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            if (File.Exists(filePath))
            {
                string MutexName = $"{GetType().Name}.{level}";
                if (Assembly.GetEntryAssembly() != null)
                    MutexName = $"{Assembly.GetEntryAssembly().GetName().Name}.{this.level}";
                else if (Assembly.GetCallingAssembly() != null)
                    MutexName = $"{Assembly.GetCallingAssembly().GetName().Name}.{this.level}";
                using (var mutex = new Mutex(false, MutexName))
                {
                    if (mutex.WaitOne(200, true))
                        try
                        {
                            File.WriteAllText(filePath, ""); // make sure it is a clean file
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileLogOutput);
        }

        public bool Equals(FileLogOutput other)
        {
            return other != null &&
                   this.level == other.level &&
                   filePath == other.filePath;
        }

        public override int GetHashCode()
        {
            var hashCode = 1789646697;
            hashCode = hashCode * -1521134295 + level.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(filePath);
            return hashCode;
        }

        /// <summary>
        /// Logs the specified log message to the file.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        public void Log(LogMessage logMsg)
        {
            if (logMsg.Level != level) return;
            logMsg = BuildMessage(ref logMsg);
            string MutexName = $"{GetType().Name}.{level}";
            if (Assembly.GetEntryAssembly() != null)
                MutexName = $"{Assembly.GetEntryAssembly().GetName().Name}.{this.level}";
            else if (Assembly.GetCallingAssembly() != null)
                MutexName = $"{Assembly.GetCallingAssembly().GetName().Name}.{this.level}";

            using (var mutex = new Mutex(false, MutexName))
            {
                if (mutex.WaitOne(200, true))
                    try
                    {
                        using (FileStream stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096))
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(logMsg.message);
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
            }
        }

        public void Log(MsLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }

        public void Log(furcLog.LogMessage logMsg)
        {
            Log((LogMessage)logMsg);
        }

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