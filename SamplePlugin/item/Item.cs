using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public class Item
    {
        protected readonly bool Hq;
        public string Name { get; set; }
        public uint Value { get; protected set; }
        public uint Amount { get; protected set; }
        protected Item(bool hq, ushort amount)
        {
            this.Hq = hq;
            this.Amount = amount;
        }

        public string GetQuality()
        {
            return this.Hq ? "(HQ)" : string.Empty;
        }

        public string GetAmout()
        {
            return $":{this.Amount}個";
        }
    }
}
