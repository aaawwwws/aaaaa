using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class StarRuby : Item
    {
        private const uint NORMAL_VALUE = 3;
        private const uint HQ_VALUE = 4;
        public StarRuby(bool hq, ushort amount) : base(hq, amount)
        {
            this.Name = "スタールビー";
            this.UnitPrice = this.Hq ? HQ_VALUE : NORMAL_VALUE;
            this.TotalValue = this.UnitPrice * amount;
        }

        public override void Add(Item item)
        {
            this.TotalValue = (item.Amount + this.Amount) * this.UnitPrice;
        }
    }
}
