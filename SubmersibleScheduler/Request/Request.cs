using Newtonsoft.Json;
using SubmersibleScheduler.Json;
using SubmersibleScheduler.RaidMacro;
using SubmersibleScheduler.YabaiPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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
            var res = await HttpClient.GetAsync(URL);
            var json = await res.Content.ReadAsStringAsync();
            var macro = JsonConvert.DeserializeObject<List<Macro>>(json);
            this.HttpClient.Dispose();
            return new RaidMacro.RaidMacro(macro);
        }

        public async Task<HttpStatusCode> PostYabai(string poster, string post_server, YabaiPlayer.YabaiPlayer yabaiplayer)
        {
            var json = $@"{{""post_name"":""{poster}"",""post_server"":""{post_server}"",""yabai_name"":""{yabaiplayer.RefName()}"",""yabai_server"":""{yabaiplayer.RefServer()}""}}";
            var content = new StringContent(json, Encoding.UTF8, @"application/json");
            var res = await HttpClient.PostAsync(EndPoint.gas, content);
            return res.StatusCode;
        }

        public async Task<List<YabaiJson>> GetYabai()
        {
            var res = await HttpClient.GetAsync(EndPoint.gas);
            var json = await res.Content.ReadAsStringAsync();
            var yabai = JsonConvert.DeserializeObject<List<YabaiJson>>(json);
            return yabai;
        }

        public void Dispose()
        {
            this.HttpClient.Dispose();
        }
    }
}
