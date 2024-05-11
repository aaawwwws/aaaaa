using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedEarring : Item
    {
        private const uint NORMAL_VALUE = 10000;
        public SalvagedEarring(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の耳飾り";
            this.UnitPrice = NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
    }
}
