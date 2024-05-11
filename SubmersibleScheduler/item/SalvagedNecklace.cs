using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedNecklace : Item
    {
        private const uint NORMAL_VALUE = 13000;
        public SalvagedNecklace(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の首飾り";
            this.UnitPrice = NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
    }
}
