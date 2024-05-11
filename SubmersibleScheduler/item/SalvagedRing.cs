using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedRing : Item
    {
        private const uint NORMAL_VALUE = 8000;
        public SalvagedRing(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の指輪";
            this.UnitPrice = NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
    }

}
