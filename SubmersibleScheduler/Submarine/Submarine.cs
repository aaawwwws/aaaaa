using System;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;

namespace SubmersibleScheduler.Submarine
{
    public class Submarine
    {
        public readonly ReturnTime ReturnTime;
        public readonly ItemList ItemList;
        public readonly int ID;
        public Submarine(HousingWorkshopSubmersibleSubData hwss, int id)
        {
            this.ID = ++id;
            this.ItemList = new ItemList();
            this.ReturnTime = new ReturnTime(hwss.GetReturnTime());

            foreach (var gathered in hwss.GatheredDataSpan)
            {
                if (gathered.ItemIdPrimary == 0 && gathered.ItemIdAdditional == 0)
                {
                    continue;
                }

                if (0 < gathered.ItemIdPrimary)
                {
                    this.ItemList.ItemPush(gathered.ItemIdPrimary, gathered.ItemHQPrimary, gathered.ItemCountPrimary);
                }

                if (0 < gathered.ItemIdAdditional)
                {
                    this.ItemList.ItemPush(gathered.ItemIdAdditional, gathered.ItemHQAdditional, gathered.ItemCountAdditional);
                }
            }

        }

        public string GetItem()
        {
            var template = $"{this.ID}隻目\n";
            if (this.ItemList.GetItems() == string.Empty)
            {
                return $"{template}対応していないアイテムです";
            }
            return $"{template}{this.ItemList.GetItems()}";
        }

        public uint GetValue()
        {
            return this.ItemList.GetValue();
        }
    }
}
