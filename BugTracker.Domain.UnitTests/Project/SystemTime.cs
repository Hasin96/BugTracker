using System;
using System.Collections.Generic;
using System.Text;

namespace BugTracker.Domain
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}
