using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Error
{
    public class WriteException : Exception
    {
        public WriteException(string msg) : base(msg) { }
    }
}
