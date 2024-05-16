using SubmersibleScheduler.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Submarine
{
    public class ItemList
    {
        public List<Item> Items { get; private set; }

        public ItemList()
        {
            this.Items = new List<Item>();
        }

        private static Item? ItemCheck(uint id, bool hq, ushort amount)
        {
            return id switch
            {
                5069 => new GoldIngot(hq, amount),
                5187 => new Ruby(hq, amount),
                5188 => new Diamond(hq, amount),
                5189 => new Emerald(hq, amount),
                5192 => new Sapphire(hq, amount),
                9360 => new PlatinumIngot(hq, amount),
                12544 => new StarRuby(hq, amount),
                12545 => new StarSapphire(hq, amount),
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

        public void ItemPush(uint id, bool hq, ushort amount)
        {
            var item = ItemCheck(id, hq, amount);
            if (item == null)
            {
                return;
            }

            var index = this.Items.FindIndex(i => i.Name == item.Name && i.Hq == item.Hq);

            if (index >= 0)
            {
                this.Items[index].Add(item);
                return;
            }

            this.Items.Add(item);
            this.Items.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        public string GetItems()
        {
            var items = new StringBuilder();

            foreach (var item in this.Items)
            {
                if (item == null)
                {
                    continue;
                }
                items.Append($"{item.Name}{item.GetQuality()}{item.GetAmout()}({item.TotalValue}ギル)\n");
            }

            return items.ToString();
        }

        public uint GetValue()
        {
            uint total = 0;
            foreach (var item in this.Items)
            {
                total += item.TotalValue;
            }
            return total;
        }
    }
}
