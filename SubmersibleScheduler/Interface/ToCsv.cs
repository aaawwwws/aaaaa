using SubmersibleScheduler.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Interface
{
    public interface FileHandler
    {
        public void CreateFile(int value);
        public void EditFile(int value);
    }
}
