using FFXIVClientStructs.FFXIV.Client.UI;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class DropItem
    {
        private readonly bool hq;
        private string name;
        private uint value;
        private const uint INGOT_VALUE = 4;
        private const uint GEM_VALUE = 2;
        private const uint HQ_VALUE = 1;
        public DropItem(ushort id, bool hq)
        {
            this.hq = hq;
            this.ItemCheck(id);
        }

        private void ItemCheck(ushort id)
        {
            switch (id)
            {
                case 5187:
                    this.name = "ルビー";
                    this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
                    break;
                case 5188:
                    this.name = "ダイヤモンド";
                    this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
                    break;
                case 5189:
                    this.name = "エメラルド";
                    this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
                    break;
                case 5192:
                    this.name = "サファイア";
                    this.value = this.hq ? GEM_VALUE + HQ_VALUE : GEM_VALUE;
                    break;
                case 9360:
                    this.name = "プラチナインゴット";
                    this.value = this.hq ? INGOT_VALUE + HQ_VALUE : INGOT_VALUE;
                    break;
                case 22500:
                    this.name = "沈没船の指輪";
                    this.value = this.hq ? INGOT_VALUE + HQ_VALUE : INGOT_VALUE;
                    break;
            }
        }
    }
}
