using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class ReadFile
    {
        public ReadFile() { }
        public static string TotalValue(string path)
        {
            var file = File.ReadAllText(path + @"\test.csv");
            var split = file.Trim().Split(',');
            var total_value = 0;
            foreach (var item in split)
            {
                try
                {
                    var cast = int.Parse(item);
                    total_value += cast;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return string.Format("合計{0:#,0}ギル", total_value);
        }
    }
}
