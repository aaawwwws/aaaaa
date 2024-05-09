using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedBracelet : Item
    {
        public SalvagedBracelet(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の腕輪";
            this.Value = 9000;
            this.Value = this.Value * amount;
        }
    }
}
