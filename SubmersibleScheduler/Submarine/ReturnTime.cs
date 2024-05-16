using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Submarine
{
    public class ReturnTime
    {
        private readonly DateTime Time;
        public ReturnTime(DateTime return_time)
        {
            Time = return_time;
        }

        public bool IsReturned()
        {
            var now_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var return_time = new DateTimeOffset(this.Time.ToLocalTime()).ToUnixTimeSeconds();
            return now_time >= return_time;
        }
    }
}
