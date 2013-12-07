using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace CommonLibrary
{
    public class TimeManager
    {

        public static void RunIn(TimeSpan dueTime, TimerCallback callback)
        {
            if (TimeSpan.Compare(dueTime, TimeSpan.Zero) < 0)
                throw new Exception("Due time must equal or larger than 'zero'");
            AutoResetEvent autoEvent = new AutoResetEvent(false);

            Timer t = new Timer(callback, autoEvent, dueTime, Timeout.InfiniteTimeSpan);
            return;
        }

        public static void RunInMinutes(double mins, TimerCallback callback)
        {
            TimeSpan ts = TimeSpan.FromMinutes(mins);
            RunIn(ts, callback);
        }

        public static void RunAt(DateTime dt, TimerCallback callback)
        {
            TimeSpan dueTime = TimeSpan.FromSeconds(dt.Subtract(DateTime.Now).TotalSeconds);
            RunIn(dueTime, callback);
        }

        public static void RunAt(string t, TimerCallback callback)
        {
            DateTime dt;
            DateTime.TryParse(t, out dt);
            RunAt(dt, callback);
        }
    }
}