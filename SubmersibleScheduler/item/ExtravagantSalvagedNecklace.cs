using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedNecklace : Item
    {
        private const uint NORMAL_VALUE = 34500;
        public ExtravagantSalvagedNecklace(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の高級首飾り";
            this.TotalValue = 34500;
            this.UnitPrice = NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
    }
}
