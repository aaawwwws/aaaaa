using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.item
{
    public abstract class Item
    {
        protected readonly bool hq;
        protected string name;
        protected uint value;
        protected const uint GEM_VALUE = 2;
        protected const uint HQ_VALUE = 1;
        protected Item(bool hq)
        {
            this.hq = hq;
        }
    }
}
