using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class SalvagedEarring : Item
    {
        public SalvagedEarring(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の耳飾り";
            this.Value = 10000;
            this.Value = this.Value * amount;
        }
    }
}
