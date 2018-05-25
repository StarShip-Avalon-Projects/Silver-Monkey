#region Usings

using Monkeyspeak.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FurcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

#endregion Usings

namespace MonkeyCore.Logging
{
    public enum Level : byte
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Debug = 4
    }

    internal class LogMessageComparer : IComparer<LogMessage>
    {
        public int Compare(LogMessage x, LogMessage y)
        {
            if (x.TimeStamp > y.TimeStamp) return -1;
            if (x.TimeStamp < y.TimeStamp) return 1;
            return 0;
        }
    }

    public struct LogMessage : IEquatable<LogMessage>
    {
        public string message;
        private DateTime expires, timeStamp;
        private Thread curThread;

        private bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(message); }
        }

        public bool IsSpam
        {
            get;
            internal set;
        }

        public Level Level { get; internal set; }

        public Thread Thread { get => curThread; set => curThread = value; }

        public DateTime TimeStamp { get => timeStamp; internal set => timeStamp = value; }

        private LogMessage(Level level, string msg, TimeSpan expireDuration)
        {
            this.Level = level;
            message = string.IsNullOrEmpty(msg) ? string.Empty : msg;
            var now = DateTime.Now;
            expires = now.Add(expireDuration);
            timeStamp = now;
            IsSpam = false;
            curThread = Thread.CurrentThread;
        }

        public static LogMessage? From(Level level, string msg, TimeSpan expireDuration)
        {
            LogMessage logMsg = new LogMessage(level, msg, expireDuration);
            var now = DateTime.Now;
            bool found = false;

            for (int i = Logger.history.Count - 1; i >= 0; i--)
            {
                var logMessage = Logger.history[i];
                if (!found && logMessage.Equals(logMsg))
                {
                    found = true;
                    break;
                }
                if (logMessage.expires < now)
                    Logger.history.RemoveAt(i);
            }

            if (found && Logger.SuppressSpam)
                logMsg.IsSpam = true;
            else logMsg.IsSpam = false;

            if (!logMsg.IsSpam)
            {
                Logger.history.Add(logMsg);
                return logMsg;
            }
            return null;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj is LogMessage lm && Equals(lm);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return message;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(LogMessage other)
        {
            return message == other.message &&
                   timeStamp == other.timeStamp &&
                   Level == other.Level &&
                   EqualityComparer<Thread>.Default.Equals(curThread, other.curThread);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = -975352547;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(message);
            hashCode = hashCode * -1521134295 + timeStamp.GetHashCode();
            hashCode = hashCode * -1521134295 + Level.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Thread>.Default.GetHashCode(curThread);
            return hashCode;
        }

        /// <summary>
        /// Convert <see cref="MsLog.LogMessage"/> to <see cref="LogMessage"/>
        /// </summary>
        /// <param name="msg"></param>
        public static explicit operator LogMessage(MsLog.LogMessage msg)
        {
            return new LogMessage()
            {
                Level = (Level)msg.Level,
                message = msg.message,
                IsSpam = msg.IsSpam,
                Thread = msg.Thread,
                TimeStamp = msg.TimeStamp,
            };
        }

        /// <summary>
        /// Convert <see cref="FurcLog.LogMessage"/> to <see cref="LogMessage"/>
        /// </summary>
        /// <param name="msg"></param>
        public static explicit operator LogMessage(FurcLog.LogMessage msg)
        {
            return new LogMessage()
            {
                Level = (Level)msg.Level,
                message = msg.message,
                IsSpam = msg.IsSpam,
                Thread = msg.Thread,
                TimeStamp = msg.TimeStamp,
            };
        }
    }

    public class Logger
    {
        internal static readonly ConcurrentList<LogMessage> history = new ConcurrentList<LogMessage>();
        internal static readonly ConcurrentQueue<LogMessage> queue = new ConcurrentQueue<LogMessage>();
        private static readonly ConcurrentList<Type> disabledTypes = new ConcurrentList<Type>();
        private static LogMessageComparer comparer = new LogMessageComparer();
        private static object syncObj = new object();
        private static bool singleThreaded, initialized;

        private static Task logTask;

        private static CancellationTokenSource cancelToken;
        private static bool abortMultithread;

        public static event Action<LogMessage> SpamFound;

        static Logger()
        {
            LogOutput = new ConsoleLogOutput();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => Error(args.ExceptionObject);

#if DEBUG
            DebugEnabled = true;
            LogCallingMethod = true;
#else
            DebugEnabled = false; // can be set via property
            LogCallingMethod = false;
#endif
            singleThreaded = true;

            Initialize();
        }

        private static void Initialize()
        {
            cancelToken = new CancellationTokenSource();
            logTask = new Task(ProcessQueue, cancelToken.Token, TaskCreationOptions.LongRunning);
            if (!singleThreaded) logTask.Start();
        }

        /// <summary>
        ///
        /// </summary>
        public static bool LogCallingMethod { get; set; }

        /// <summary>
        /// Info logger enable
        /// </summary>
        public static bool InfoEnabled { get; set; } = true;

        /// <summary>
        /// warning logger enable
        /// </summary>
        public static bool WarningEnabled { get; set; } = true;

        /// <summary>
        /// error logger enable
        /// </summary>
        public static bool ErrorEnabled { get; set; } = true;

        /// <summary>
        /// debug logger enable
        /// </summary>
        public static bool DebugEnabled { get; set; }

        /// <summary>
        /// Suppress Spam
        /// </summary>
        public static bool SuppressSpam { get; set; }

        /// <summary>
        /// Gets or sets the messages expire time limit. Messages that have expired are removed from
        /// history. This property used in conjunction with SupressSpam = true prevents too much
        /// memory from being used over time
        /// </summary>
        /// <value>The messages expire time limit.</value>
        public static TimeSpan MessagesExpire { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Sets the <see cref="ILogOutput"/>.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <exception cref="System.ArgumentNullException">output</exception>
        public static ILogOutput LogOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [single threaded].
        /// </summary>
        /// <value><c>true</c> if [single threaded]; otherwise, <c>false</c>.</value>
        public static bool SingleThreaded
        {
            get
            {
                return singleThreaded;
            }
            set
            {
                singleThreaded = value;
                if (singleThreaded) cancelToken.Cancel();
                else
                {
                    if (logTask.Status == TaskStatus.Running ||
                        logTask.Status == TaskStatus.WaitingToRun ||
                        logTask.Status == TaskStatus.WaitingForActivation)
                        return;
                    cancelToken.Dispose();
                    cancelToken = new CancellationTokenSource();

                    // exit while loop gracefully
                    abortMultithread = true;
                    Thread.Sleep(50);
                    abortMultithread = false;

                    logTask.Dispose();
                    logTask = new Task(ProcessQueue, cancelToken.Token, TaskCreationOptions.LongRunning);
                    logTask.Start();
                }
            }
        }

        /// <summary>
        /// Disables logging for the specified type.
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        public static void Disable<T>()
        {
            if (!disabledTypes.Contains(typeof(T)))
                disabledTypes.Add(typeof(T));
        }

        private static bool TypeCheck(Type type, out string typeName)
        {
            if (disabledTypes.Contains(type))
            {
                typeName = null;
                return false;
            }
            typeName = type.Name;
            return true;
        }

        private static void Log(LogMessage? msg)
        {
            if (msg == null) return;
            queue.Enqueue(msg.Value);
            if (singleThreaded)
            {
                Dump();
            }
        }

        private static void ProcessQueue()
        {
            while (!abortMultithread)
            {
                Thread.Sleep(10);
                if (!singleThreaded)
                {
                    // take a dump
                    Dump();
                }
                else break;
            }
        }

        private static void Dump()
        {
            if (queue.Count == 0) return;
            if (queue.TryDequeue(out LogMessage msg))
            {
                if (msg.IsSpam)
                {
                    SpamFound?.Invoke(msg);
                    if (SuppressSpam)
                        return;
                }
                switch (msg.Level)
                {
                    case Level.Debug:
                        if (!DebugEnabled)
                            return;
                        break;

                    case Level.Error:
                        if (!ErrorEnabled)
                            return;
                        break;

                    case Level.Info:
                        if (!InfoEnabled)
                            return;
                        break;

                    case Level.Warning:
                        if (!WarningEnabled)
                            return;
                        break;
                }
                LogOutput.Log(msg);
            }
        }

        public static bool Assert(bool cond, string failMsg)
        {
            if (!cond)
            {
                Error("ASSERT: " + failMsg);
                return false;
            }
            return true;
        }

        public static bool Assert<T>(bool cond, string failMsg)
        {
            if (!cond)
            {
                Error<T>("ASSERT: " + failMsg);
                return false;
            }
            return true;
        }

        public static bool Assert(Func<bool> cond, string failMsg)
        {
            if (!cond?.Invoke() ?? false)
            {
                Error("ASSERT: " + failMsg);
                return false;
            }
            return true;
        }

        public static bool Assert<T>(Func<bool> cond, string failMsg)
        {
            if (!cond?.Invoke() ?? false)
            {
                Error<T>("ASSERT: " + failMsg);
                return false;
            }
            return true;
        }

        public static bool Fails(bool cond, string failMsg)
        {
            if (cond)
            {
                Error("FAIL: " + failMsg);
                return true;
            }
            return false;
        }

        public static bool Fails<T>(bool cond, string failMsg)
        {
            if (cond)
            {
                Error<T>("FAIL: " + failMsg);
                return true;
            }
            return false;
        }

        public static bool Fails(Func<bool> cond, string failMsg)
        {
            if (cond?.Invoke() ?? false)
            {
                Error("FAIL: " + failMsg);
                return true;
            }
            return false;
        }

        public static bool Fails<T>(Func<bool> cond, string failMsg)
        {
            if (cond?.Invoke() ?? false)
            {
                Error<T>("FAIL: " + failMsg);
                return true;
            }
            return false;
        }

        public static void Debug(object msg, [CallerMemberName]string memberName = "")
        {
            if (!DebugEnabled) return;
            Log(LogMessage.From(Level.Debug, $"System{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {(msg != null ? msg.ToString() : "null")}", MessagesExpire));
        }

        public static void Debug<T>(object msg, [CallerMemberName]string memberName = "")
        {
            if (DebugEnabled && TypeCheck(typeof(T), out string typeName))
                Log(LogMessage.From(Level.Debug, $"{typeName}{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {msg}", MessagesExpire));
        }

        public static void Info(object msg, [CallerMemberName]string memberName = "")
        {
            if (!InfoEnabled) return;
            Log(LogMessage.From(Level.Info, $"System{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {(msg != null ? msg.ToString() : "null")}", MessagesExpire));
        }

        public static void Info<T>(object msg, [CallerMemberName]string memberName = "")
        {
            if (InfoEnabled && TypeCheck(typeof(T), out string typeName))
                Log(LogMessage.From(Level.Info, $"{typeName}{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {msg}", MessagesExpire));
        }

        public static void Error(object msg, [CallerMemberName]string memberName = "")
        {
            if (!ErrorEnabled) return;
            Log(LogMessage.From(Level.Error, $"System{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {(msg != null ? msg.ToString() : "null")}", MessagesExpire));
        }

        public static void Error<T>(object msg, [CallerMemberName]string memberName = "")
        {
            if (ErrorEnabled && TypeCheck(typeof(T), out string typeName))
                Log(LogMessage.From(Level.Error, $"{typeName}{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {msg}", MessagesExpire));
        }

        public static void Warn(object msg, [CallerMemberName]string memberName = "")
        {
            if (!WarningEnabled) return;
            Log(LogMessage.From(Level.Warning, $"System{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {(msg != null ? msg.ToString() : "null")}", MessagesExpire));
        }

        public static void Warn<T>(object msg, [CallerMemberName]string memberName = "")
        {
            if (WarningEnabled && TypeCheck(typeof(T), out string typeName))
                Log(LogMessage.From(Level.Warning, $"{typeName}{(LogCallingMethod && !memberName.IsNullOrBlank() ? $" ({memberName})" : "")}: {msg}", MessagesExpire));
        }

        public static MsLog.ILogOutput MsLogOutput
        {
            get { return (MsLog.ILogOutput)LogOutput; }
            set { LogOutput = (ILogOutput)value; }
        }

        public static FurcLog.ILogOutput FurcLogOutput
        {
            get { return (FurcLog.ILogOutput)LogOutput; }
            set { LogOutput = (ILogOutput)value; }
        }
    }
}