using Newtonsoft.Json;
using SubmersibleScheduler.Json;
using SubmersibleScheduler.RaidMacro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler.Request
{
    public class Request : IDisposable
    {
        private readonly HttpClient HttpClient;
        public Request()
        {
            this.HttpClient = new HttpClient();
        }

        public async Task<RaidMacro.RaidMacro> GetMacro()
        {
            const string URL = "https://raw.githubusercontent.com/aaawwwws/aaaaa/master/macro.json";
            try
            {
                var res = await HttpClient.GetAsync(URL);
                var json = await res.Content.ReadAsStringAsync();
                var macro = JsonConvert.DeserializeObject<List<Macro>>(json);
                return new RaidMacro.RaidMacro(macro);
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            this.HttpClient.Dispose();
        }
    }
}
