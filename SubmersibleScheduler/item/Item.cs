using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class Item
    {
        public bool Hq { get; protected set; }
        public string Name { get; set; }
        public uint TotalValue { get; protected set; }
        public uint Amount { get; protected set; }
        public uint UnitPrice { get; protected set; }

        protected Item(bool hq, ushort amount)
        {
            this.Hq = hq;
            this.Amount = amount;
        }

        public string GetQuality() => this.Hq ? "(HQ)" : string.Empty;

        public string GetAmout() => $":{this.Amount}個";

        public virtual void Add(Item item)
        {
            this.TotalValue = (item.Amount + this.Amount) * this.UnitPrice;
            this.Amount += item.Amount;
        }
    }
}
