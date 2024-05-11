using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedBracelet : Item
    {
        private const uint NORMAL_VALUE = 28500;
        public ExtravagantSalvagedBracelet(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の高級腕輪";
            this.UnitPrice = NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
    }
}
