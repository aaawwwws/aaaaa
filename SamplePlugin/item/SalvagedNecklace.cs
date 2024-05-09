using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedNecklace : Item
    {
        public SalvagedNecklace(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の首飾り";
            this.Value = 13000;
            this.Value = this.Value * amount;
        }
    }
}
