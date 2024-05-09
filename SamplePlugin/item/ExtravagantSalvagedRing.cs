using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedRing : Item
    {
        public ExtravagantSalvagedRing(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の高級指輪";
            this.Value = 27000;
            this.Value = this.Value * amount;
        }
    }
}
