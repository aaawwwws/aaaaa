using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class Message
    {
        public string CsvMsg { get; set; }

        public string SMMsg { get; set; }

        public Message()
        {
            this.CsvMsg = string.Empty;
            this.SMMsg = string.Empty;
        }
    }
}
