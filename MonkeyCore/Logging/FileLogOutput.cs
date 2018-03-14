using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FurcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

namespace MonkeyCore.Logging
{
    /// <summary>
    /// </summary>
    /// <seealso cref="ILogOutput"/>
    /// <seealso cref="System.IEquatable{Logging.FileLogOutput}"/>
    public class FileLogOutput : ILogOutput, MsLog.ILogOutput, FurcLog.ILogOutput, IEquatable<FileLogOutput>
    {
        private readonly Level level;
        private readonly string FilePath;

        public FileLogOutput(string rootFolder, Level level = Level.Error)
        {
            if (Assembly.GetEntryAssembly() != null)
                FilePath = Path.Combine(rootFolder, $"{Assembly.GetEntryAssembly().GetName().Name}.{level}.log");
            else if (Assembly.GetCallingAssembly() != null)
                FilePath = Path.Combine(rootFolder, $"{Assembly.GetCallingAssembly().GetName().Name}.{level}.log");
            if (!Directory.Exists(Path.GetDirectoryName(FilePath))) Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            if (File.Exists(FilePath)) File.WriteAllText(FilePath, ""); // make sure it is a clean file
            this.level = level;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileLogOutput);
        }

        public bool Equals(FileLogOutput other)
        {
            return other != null &&
                FilePath == other.FilePath
                && level == other.level;
        }

        public override int GetHashCode()
        {
            var hashCode = 1789646697;
            hashCode = hashCode * -1521134295 + level.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
            return hashCode;
        }

        private Queue<LogMessage> LogMessageQueue = new Queue<LogMessage>();

        public void Log(LogMessage logMsg)
        {
            LogMessageQueue.Enqueue(logMsg);
            while (LogMessageQueue.Count > 0)
            {
                logMsg = LogMessageQueue.Peek();
                if (logMsg.Level != level) return;

                logMsg = BuildMessage(ref logMsg);
                try
                {
                    using (FileStream stream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.Write, 4096))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(logMsg.message);
                    }
                    LogMessageQueue.Dequeue();
                }
                catch { }
            }
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

        void MsLog.ILogOutput.Log(MsLog.LogMessage logMsg)
        {
            Log(logMsg);
        }

        void FurcLog.ILogOutput.Log(FurcLog.LogMessage logMsg)
        {
            Log(logMsg);
        }
    }
}