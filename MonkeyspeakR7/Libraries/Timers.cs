using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Monkeyspeak.Libraries
{
    /// <summary>
    /// A TimerTask object contains Timer and Page Owner.  Timer is not started from a TimerTask constructor.
    /// </summary>
    internal sealed class TimerTask
    {
        private Timer timer;
        private double interval;

        public double Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        private Page owner;

        public Page Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private double id;

        public double ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Timer task that executes (0:300) when it triggers
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="interval">Interval in Seconds</param>
        /// <param name="Id"></param>
        public TimerTask(Page owner, double interval, double Id)
        {
            ID = Id;
            this.owner = owner;
            this.interval = interval;
            timer = new Timer(TimeSpan.FromSeconds(interval).TotalMilliseconds)
            {
                AutoReset = true
            };
            timer.Elapsed += (sender, args) => timer_Elapsed(this);
            timer.Enabled = true;
            owner.Resetting += () =>
            {
                Dispose();
            };
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Dispose()
        {
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
        }

        private static void timer_Elapsed(object sender)
        {
            try
            {
                TimerTask timerTask = (TimerTask)sender;
                if (timerTask.timer.Enabled)
                    timerTask.owner.Execute(300);
            }
            catch
            {
                // Eat the exception.. yummy!
            }
        }
    }

    // Changed from Internal to public in order to expose DestroyTimers() - Gerolkae
    public class Timers : AbstractBaseLibrary
    {
        private static readonly object lck = new object();
        private static readonly List<TimerTask> timers = new List<TimerTask>();

        /// <summary>
        /// Default Timer Library.  Call static method Timers.DestroyTimers() when your application closes.
        /// </summary>
        public Timers()
        {
            // (0:300) When timer # goes off,
            Add(new Trigger(TriggerCategory.Cause, 300), WhenTimerGoesOff,
                "(0:300) When timer # goes off,");

            // (1:300) and timer # is running,
            Add(new Trigger(TriggerCategory.Condition, 300), AndTimerIsRunning,
                "(1:300) and timer # is running,");
            // (1:301) and timer # is not running,
            Add(new Trigger(TriggerCategory.Condition, 301), AndTimerIsNotRunning,
                "(1:301) and timer # is not running,");

            // (5:300) create timer # to go off every # second(s).
            Add(new Trigger(TriggerCategory.Effect, 300), CreateTimer,
                "(5:300) create timer # to go off every # second(s).");

            // (5:301) stop timer #.
            Add(new Trigger(TriggerCategory.Effect, 301), StopTimer,
                "(5:301) stop timer #.");
        }

        internal static void DestroyTimer(TimerTask task)
        {
            lock (lck)
            {
                task.Dispose();
                timers.Remove(task);
            }
        }

        public override void OnPageDisposing(Page page)
        {
            lock (lck)
            {
                foreach (var task in timers.ToArray())
                    DestroyTimer(task);
            }
        }

        private bool AndTimerIsNotRunning(TriggerReader reader)
        {
            return !AndTimerIsRunning(reader);
        }

        private bool AndTimerIsRunning(TriggerReader reader)
        {
            TimerTask timerTask;
            if (!TryGetTimerFrom(reader, out timerTask))
                return false;
            bool test = timerTask.Timer.Enabled;

            return test;
        }

        private bool CreateTimer(TriggerReader reader)
        {
            if (timers.Count > reader.Page.Engine.Options.TimerLimit)
            {
                throw new MonkeyspeakException("The amount of timers has exceeded the limit by {0}", timers.Count - reader.Page.Engine.Options.TimerLimit);
            }
            double id = double.NaN;
            if (reader.PeekVariable())
            {
                var var = reader.ReadVariable();
                if (var.Value is double)
                    id = (double)var.Value;
            }
            else if (reader.PeekNumber())
            {
                id = reader.ReadNumber();
            }

            double interval = double.NaN;
            if (reader.PeekVariable())
            {
                var var = reader.ReadVariable();
                if (var.Value is double)
                    interval = (double)var.Value;
            }
            else if (reader.PeekNumber())
            {
                interval = reader.ReadNumber();
            }

            if (double.IsNaN(interval)) return false;

            lock (lck)
            {
                var timerTask = new TimerTask(reader.Page, interval, id);
                var existing = timers.FirstOrDefault(task => task.ID == id);
                if (existing != null)
                {
#warning Replacing existing timer may cause any triggers dependent on that timer to behave differently
                    existing.Dispose();
                    timers.Add(timerTask);
                }
                else
                {
                    timers.Add(timerTask);
                }
            }

            return true;
        }

        private bool StopTimer(TriggerReader reader)
        {
            double num = 0;
            if (reader.PeekVariable())
            {
                var var = reader.ReadVariable();
                if (!Double.TryParse(var.Value.ToString(), out num))
                    num = 0;
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }
            try
            {
                lock (lck)
                {
                    TimerTask task = timers.FirstOrDefault(timer => timer.ID == num);
                    task.Dispose();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            return true;
        }

        private bool TryGetTimerFrom(TriggerReader reader, out TimerTask timerTask)
        {
            double num = double.NaN;
            if (reader.PeekVariable())
            {
                var var = reader.ReadVariable();
                if (var.Value is double)
                    num = (double)var.Value;
            }
            else if (reader.PeekNumber())
            {
                num = reader.ReadNumber();
            }

            if (!double.IsNaN(num))
            {
                if (!timers.Any(task => task.ID == num))
                {
                    // Don't add a timer to the Dictionary if it don't
                    // exist. Just return a blank timer
                    // - Gerolkae
                    timerTask = new TimerTask(reader.Page, -1, num);
                    return false;
                }
                timerTask = timers.FirstOrDefault(task => task.ID == num);
                return true;
            }
            timerTask = null;
            return false;
        }

        private bool WhenTimerGoesOff(TriggerReader reader)
        {
            if (!reader.PeekNumber()) return false;

            double timer = reader.ReadNumber();
            return true;
        }
    }
}