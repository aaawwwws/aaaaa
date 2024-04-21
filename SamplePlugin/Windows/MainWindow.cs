using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using System.Threading.Tasks;
namespace SubmersibleScheduler.Windows;

public unsafe class MainWindow : Window, IDisposable
{
#pragma warning disable IDE1006 // 命名スタイル
    private readonly Plugin Plugin;
    private WebHook WebHook;
    private SubReturn SubReturn;
    public MainWindow(Plugin plugin)
        : base("潜水艦", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        Plugin = plugin;
        WebHook = new WebHook(Plugin);
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
            ImGui.Text("潜水艦を確認してください。");
            return;
        }

        ImGui.InputText("WebHookを入力", ref this.WebHook.EndPoint, (uint)128);
        string[] array = new string[4];

        for (var i = 0; i < array.Length; i++)
        { 
            const string template = "潜水艦";
            array[i] = $"{template}{i + 1}:{wt->Submersible.DataPointerListSpan[i].Value->GetReturnTime().ToLocalTime()}\n";
        }

        this.WebHook.Save(ImGui.Button("保存"));

        if (ImGui.Button("潜水艦の情報をdiscordに送信"))
        {
            //別スレッド
            Task.Run(() =>
            {
                try
                {
                    new Discord(this.WebHook.EndPointTrim(), string.Join("", array)).SendMsg().GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    if (string.IsNullOrEmpty(e.Message))
                    {
                        ImGui.Text("謎エラー");
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
