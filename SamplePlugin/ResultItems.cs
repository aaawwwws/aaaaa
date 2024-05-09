using SubmersibleScheduler.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class ResultItems
    {
        public List<Item> Items;

        public ResultItems()
        {
            Items = new List<Item>();
        }

        public void ItemPush(uint id, bool hq, ushort amount)
        {
            var item = ItemCheck(id, hq, amount);
            if (item == null)
            {
                return;
            }
            Items.Add(item);
        }

        private static Item? ItemCheck(uint id, bool hq, ushort amount)
        {
            return id switch
            {
                5187 => new Ruby(hq, amount),
                5188 => new Diamond(hq, amount),
                5189 => new Emerald(hq, amount),
                5192 => new Sapphire(hq, amount),
                9360 => new PlatinumIngot(hq, amount),
                22500 => new SalvagedRing(hq, amount),
                22501 => new SalvagedBracelet(hq, amount),
                22502 => new SalvagedEarring(hq, amount),
                22503 => new SalvagedNecklace(hq, amount),
                22504 => new ExtravagantSalvagedRing(hq, amount),
                22505 => new ExtravagantSalvagedBracelet(hq, amount),
                22506 => new ExtravagantSalvagedEarring(hq, amount),
                22507 => new ExtravagantSalvagedNecklace(hq, amount),
                _ => null,
            };
        }
        public string ItemStr()
        {
            var res = new StringBuilder();
            foreach (var item in Items)
            {
                res.Append($"{item.Name}:{item.Amount}個\n");
            }
            return res.ToString();
        }
        public string TotalValue()
        {
            uint total = 0;
            foreach (var item in Items)
            {
                total += item.Value;
            }
            return $"本日の収穫:{total}ギル";
        }
    }
}
