using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedBracelet : Item
    {
        public SalvagedBracelet(bool hq) : base(hq)
        {
            this.name = "沈没船の腕輪";
            this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
        }
    }
}
