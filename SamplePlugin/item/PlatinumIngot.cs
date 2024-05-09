using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class PlatinumIngot : Item
    {
        public PlatinumIngot(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "プラチナインゴット";
            const uint normal_value = 4;
            const uint hq_value = 5;
            this.Value = this.Hq ? hq_value : normal_value;
            this.Value = this.Value * amount;
        }
    }
}
