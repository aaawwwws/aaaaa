using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class ExtravagantSalvagedNecklace : Item
    {
        public ExtravagantSalvagedNecklace(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "沈没船の高級首飾り";
            this.Value = 34500;
            this.Value = this.Value * amount;
        }
    }
}
