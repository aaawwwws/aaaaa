using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using System.Threading.Tasks;
using FFXIVClientStructs.Havok;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FFXIVClientStructs.STD;
namespace SubmersibleScheduler.Windows;

public class MainWindow : Window, IDisposable
{
#pragma warning disable IDE1006 // 命名スタイル
    private readonly Plugin Plugin;
    private WebHook WebHook;
    private SubReturn SubReturn;
    private System.Object lockobj;
    private string msg = string.Empty;
    public MainWindow(Plugin plugin)
        : base("潜水艦", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.Plugin = plugin;
        this.WebHook = new WebHook(Plugin);
        this.lockobj = new System.Object();
        this.msg = string.Empty;
    }

    public void Dispose() { }

    public unsafe override void Draw()
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


        string[] array = new string[4];

        for (var i = 0; i < array.Length; i++)
        {
            const string template = "潜水艦";
            array[i] = $"{template}{i + 1}:{wt->Submersible.DataPointerListSpan[i].Value->GetReturnTime().ToLocalTime()}\n";
        }

        ImGui.InputText("WebHookを入力", ref this.WebHook.EndPoint, (uint)128);
        if (ImGui.Button("保存"))
        {
            this.WebHook.Save();
        }

        if (ImGui.Button("潜水艦の情報をdiscordに送信"))
        {
            //別スレッド
            Task.Run(() =>
            {
                var res = Discord(array);
                lock (lockobj)
                {
                    this.msg = res.Unwrap();
                }
            });
        }
        ImGui.Text(this.msg);
    }
    private Result<string> Discord(string[] array)
    {
        try
        {
            new Discord(this.WebHook.EndPointTrim(), string.Join("", array)).SendMsg().GetAwaiter().GetResult();
            return new Result<string>("成功", Res.Ok);
        }
        catch (Exception e)
        {
            return new Result<string>(e.Message, Res.Err);
        }
    }
}
