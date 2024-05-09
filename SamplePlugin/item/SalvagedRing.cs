using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedRing : Item
    {
        public SalvagedRing(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の指輪";
            this.Value = 8000;
            this.Value = this.Value * amount;
        }
    }
}
