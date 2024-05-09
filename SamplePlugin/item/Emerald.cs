using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class Emerald : Item
    {
        public Emerald(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "エメラルド";
            const uint normal_value = 2;
            const uint hq_value = 3;
            this.Value = this.Hq ? hq_value : normal_value;
            this.Value = this.Value * amount;
        }
    }
}
