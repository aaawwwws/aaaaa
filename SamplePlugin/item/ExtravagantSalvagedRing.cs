using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedRing : Item
    {
        public ExtravagantSalvagedRing(bool hq) : base(hq)
        {
            this.name = "沈没船の高級指輪";
            this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
        }
    }
}
