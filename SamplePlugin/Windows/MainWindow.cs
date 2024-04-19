using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.System.Input;
using System.Text;
using System.IO;
using Dalamud.Configuration;
using Dalamud.Plugin.Services;
namespace SamplePlugin.Windows;

public unsafe class MainWindow : Window, IDisposable
{
    private IDalamudTextureWrap? GoatImage;
    private Plugin Plugin;
    private DateTime submarine;
    private string webhook;
    private bool b;
    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, IDalamudTextureWrap? goatImage)
        : base("潜水艦", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        GoatImage = goatImage;
        Plugin = plugin;
        this.webhook = System.String.Empty;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var hb = HousingManager.Instance();
        if (hb == null)
        {
            ImGui.Text("null");
            return;
        }
        var wt = hb->WorkshopTerritory;
        if (wt == null)
        {
            ImGui.Text("wt null");
            return;
        }

        ImGui.InputText("test", ref this.webhook, (uint)128);
        string[] array = new string[4];

        for (var i = 0; i < array.Length; i++)
        { 
            const string template = "潜水艦";
            array[i] = $"{template}{i + 1}:{wt->Submersible.DataPointerListSpan[i].Value->GetReturnTime().ToLocalTime()}\n";
        }

        if (ImGui.Button("保存") && Plugin.Configuration.WebHook != webhook)
        {
            Plugin.Configuration.WebHook = this.webhook;
            Plugin.Configuration.Save();
        }

        if (ImGui.Button("潜水艦の情報をdiscordに送信"))
        {
            //別スレッド
            Task.Run(() =>
            {
                try
                {
                    new Discord(this.webhook.Replace("\r", "").Replace("\n", "").Trim(), string.Join("", array)).SendMsg().GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    if (string.IsNullOrEmpty(e.Message))
                    {
                        ImGui.Text("error");
                    }
                    else
                    {
                        ImGui.Text(e.Message);
                    }
                }
            });
        }

    }
}
