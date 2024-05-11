using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedBracelet : Item
    {
        private const uint NORMAL_VALUE = 9000;
        public SalvagedBracelet(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の腕輪";
            this.UnitPrice = NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
    }
}
