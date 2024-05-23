using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.YabaiPlayer
{
    public class YabaiPlayer
    {
        private string Name;
        private string Server;

        public YabaiPlayer()
        {
            this.Name = string.Empty;
            this.Server = string.Empty;
        }

        public ref string RefName()
        {
            return ref this.Name;
        }
        public ref string RefServer()
        {
            return ref this.Server;
        }
    }
}
