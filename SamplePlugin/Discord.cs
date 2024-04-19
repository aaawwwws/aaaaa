using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SamplePlugin
{
    public class Discord
    {
        private readonly string webhook;
        private readonly string msg;
        public Discord(string end_point, string msg)
        {
            this.webhook = end_point;
            this.msg = msg;
        }

        public async Task SendMsg()
        {
            using var client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, this.webhook);
                var json = JsonSerializer.Serialize(new { username = "name", content = this.msg });
                request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await client.SendAsync(request);
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
}
