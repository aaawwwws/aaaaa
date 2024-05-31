using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Error
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string msg) : base(msg) { }
    }
}
