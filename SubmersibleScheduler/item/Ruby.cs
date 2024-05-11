using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class Ruby : Item
    {
        private const uint NORMAL_VALUE = 2;
        private const uint HQ_VALUE = 3;
        public Ruby(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "ルビー";
            this.UnitPrice = this.Hq ? HQ_VALUE : NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }
        public override void Add(Item item)
        {
            this.TotalValue = (item.Amount + this.Amount) * this.UnitPrice;

        }
    }
}
