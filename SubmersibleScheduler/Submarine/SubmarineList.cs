using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using SubmersibleScheduler.Enum;
using System.IO;
using System.Linq;
using System;
using System.Text;
using SubmersibleScheduler.Interface;

namespace SubmersibleScheduler.Submarine
{
    public class SubmarineList : ToCsv
    {
        private readonly int NumSubmarines;
        public readonly Submarine[] Submarines; //List<Submarine>にしてもパフォーマス変わらないっぽいので変更するかも

        public SubmarineList(HousingWorkshopSubmersibleData hwsd)
        {
            this.NumSubmarines = hwsd.DataListSpan.Length;
            this.Submarines = new Submarine[NumSubmarines];
            var i = 0;
            foreach (var sub in hwsd.DataListSpan)
            {
                this.Submarines[i] = new Submarine(sub, i);
                i++;
            }
        }

        public string TotalItem()
        {
            var items = new StringBuilder();
            foreach (var sub in this.Submarines)
            {
                items.Append(sub.GetItem());
            }
            return items.ToString();
        }

        public string StrTotalValue()
        {
            uint total = 0;
            foreach (var sub in this.Submarines)
            {
                total += sub.GetValue();
            };
            return string.Format("本日の収穫{0:#,0}ギル", total);
        }

        public int IntTotalValue()
        {
            int total = 0;
            foreach (var sub in this.Submarines)
            {
                total += (int)sub.GetValue();
            };
            return total;
        }

        public WriteCode WriteCsv(string path)
        {
            if (path.Last() != '\\')
            {
                path = $"{path}\\";
            }

            if (!Directory.Exists(path))
            {
                return WriteCode.PathError;
            }

            var today = DateTime.Now.ToString("yyyy/MM/dd(ddd)");
            var padding = ",,";
            const string lavel = ",日付, 金額";

            var per_path = $"{path}test.csv";

            if (!File.Exists(per_path))
            {
                try
                {
                    using var fs = File.Create(per_path);
                    using var writer = new StreamWriter(fs);
                    writer.WriteLine(padding);
                    writer.WriteLine(lavel);
                }
                catch (Exception)
                {
                    return WriteCode.WriteError;
                }
            }

            using (var fs = new StreamWriter(per_path, true))
            {
                var data = $", {today}, {this.IntTotalValue()}";
                try
                {
                    fs.WriteLine(data);
                }
                catch (Exception)
                {
                    return WriteCode.WriteError;
                }
            }
            return WriteCode.Success;
        }

        public bool[] ReturnBools()
        {
            var bool_list = new bool[this.NumSubmarines];
            for (var i = 0; i < this.NumSubmarines; i++)
            {
                bool_list[i] = this.Submarines[i].ReturnTime.IsReturned();
            }
            return bool_list;
        }
    }
}
