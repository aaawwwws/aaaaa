using System;
using System.Data;

namespace SubmersibleScheduler
{
    public class SubMarine
    {
#pragma warning disable IDE1006 // 命名スタイル
        private int Id;
        private DateTime ReturnTime;

        public SubMarine(DateTime return_time,int id)
        {
            this.ReturnTime = return_time.ToLocalTime();
            this.Id = id;
        }

        public string msg ()
        {
            string template = $"潜水艦{this.Id}";
            var time = this.ReturnTime - DateTime.Now;
            var days = time.Days;
            var hours = time.Hours;
            var minutes = time.Minutes;
            var return_time = days > 0 ? $"{days}日{hours}時間{minutes}分" : $"{hours}時間{minutes}分後";
            string msg = $"{template}:{this.ReturnTime}:({return_time})\n";
            return msg;
        }
    }
}
