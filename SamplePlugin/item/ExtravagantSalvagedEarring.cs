using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedEarring : Item
    {
        public ExtravagantSalvagedEarring(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の高級耳飾り";
            this.Value = 30000;
            this.Value = this.Value * amount;
        }
    }
}
