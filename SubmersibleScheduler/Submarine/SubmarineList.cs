using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using SubmersibleScheduler.Enum;
using System.IO;
using System;
using System.Text;
using Dalamud.Utility;
using SubmersibleScheduler.Error;

namespace SubmersibleScheduler.Submarine
{
    public class SubmarineList
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
            var total = 0;
            foreach (var sub in this.Submarines)
            {
                total += (int)sub.GetValue();
            };
            return total;
        }

        public void WriteCsv(string path) //分割予定
        {
            if (path.IsNullOrEmpty()) throw new DirectoryNotFoundException("パスが見つかりません");

            if (!path.EndsWith('\\'))
            {
                path += '\\';
            }

            if (!Directory.Exists(path)) throw new DirectoryNotFoundException("パスが見つかりません");


            var today = DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd(ddd)");
            var padding = ",,";
            const string label = ",日付, 金額";
            var per_path = $"{path}test.csv";
            var data = $", {today}, {this.IntTotalValue()}";

            //ファイル作成処理
            if (!File.Exists(per_path))
            {
                var new_file = File.Create(per_path);
                new_file.Dispose();
                File.WriteAllText(per_path, $"{padding}\n{label}");
            }

            const short OFFSET = 1;
            const short DATE_CELL = 1;
            const short VALUE_CELL = 2;


            var file_data = File.ReadAllText(per_path);
            var split_file = file_data.Split(new[] { '\n', '\r' });
            var lastline = split_file[split_file.Length - OFFSET].Split(',');
            var date = lastline[DATE_CELL].Trim();
            var value = lastline[VALUE_CELL].Trim();
            if (value.Equals(this.IntTotalValue().ToString()) && date.Equals(today)) throw new DuplicateException("既に書き込まれいます");
            if (!string.IsNullOrEmpty(date) && date.Equals(today))
            {
                //既に今日のデータを書き込んでる場合
                split_file[split_file.Length - OFFSET] = data;
                var new_data = string.Join('\n', split_file);
                File.WriteAllText(per_path, new_data.ToString());
            }
            else
            {
                //まだ書き込んでない場合
                File.AppendAllText(per_path, $"\n{data}");
            }
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
