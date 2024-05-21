using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Exceotion
{
    public class FileExistsException : Exception
    {
        public FileExistsException(string msg) : base(msg) { }
    }
}
