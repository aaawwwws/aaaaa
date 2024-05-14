using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using SubmersibleScheduler.Enum;
using System;
using System.IO;
using System.Linq;

namespace SubmersibleScheduler
{


    public class SMDrop
    {
        private readonly ResultItems _Items;
        public ResultItems items { get => _Items; }
        public SMDrop(HousingWorkshopSubmersibleData sm_data)
        {
            this._Items = new ResultItems();

            foreach (var a in sm_data.DataListSpan)
            {
                foreach (var b in a.GatheredDataSpan)
                {

                    if (b.ItemIdPrimary == 0 && b.ItemIdAdditional == 0)
                    {
                        continue;
                    }

                    if (0 < b.ItemIdPrimary)
                    {
                        this._Items.ItemPush(b.ItemIdPrimary, b.ItemHQPrimary, b.ItemCountPrimary);
                    }

                    if (0 < b.ItemIdAdditional)
                    {
                        this._Items.ItemPush(b.ItemIdAdditional, b.ItemHQAdditional, b.ItemCountAdditional);
                    }
                }
            }
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
                var data = $", {today}, {this._Items.TotalValueInt()}";
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
    }


}
