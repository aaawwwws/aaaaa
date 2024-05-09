using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedBracelet : Item
    {
        public ExtravagantSalvagedBracelet(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の高級腕輪";
            this.Value = 28500;
            this.Value = this.Value * amount;
        }
    }
}
