using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using System.Net;

namespace SubmersibleScheduler
{
    public class WebHook
    {
        public string EndPoint;
        private Plugin Plugin;

        public WebHook(Plugin plugin)
        {
            this.EndPoint = string.Empty;
            this.Plugin = plugin;
        }

        public void Save ()
        {
            if(Plugin.Configuration.WebHook != this.EndPoint)
            {
                Plugin.Configuration.WebHook = this.EndPoint;
                Plugin.Configuration.Save();
            }
        }

        public string EndPointTrim()
        {
            return EndPoint.Replace("\r", "").Replace("\n", "").Trim();
        }
    }
}
