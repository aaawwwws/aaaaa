using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class PlatinumIngot : Item
    {
        public PlatinumIngot(bool hq) : base(hq)
        {
            this.name = "プラチナインゴット";
            this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
        }
    }
}
