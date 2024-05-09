using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using System.Threading.Tasks;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.System.Input;
using System.Xml.Xsl;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.System.String;
namespace SubmersibleScheduler.Windows;

public unsafe class MainWindow : Window, IDisposable
{
#pragma warning disable IDE1006 // 命名スタイル
    private readonly Plugin Plugin;
    private WebHook WebHook;
    private SubReturn SubReturn;
    private System.Object lockobj;
    private string msg;
    private bool init;
    private HousingManager* HousingManager;

    public MainWindow(Plugin plugin, HousingManager* hm)
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
        this.init = true;
        this.HousingManager = hm;
    }

    public void Dispose() { }

    public override unsafe void Draw()
    {
        if (this.HousingManager == null)
        {
            ImGui.Text("null");
            return;
        }

        var wt = this.HousingManager->WorkshopTerritory;

        if (wt == null)
        {
            ImGui.Text("潜水艦を確認してください。");
            return;
        }

        var sba = new SubMarineArray(wt->Submersible);

        ImGui.InputText("WebHookを入力", ref this.WebHook.EndPoint, (uint)128);

        if (Plugin.Configuration.WebHook != string.Empty && this.init)
        {
            init = false;
            this.WebHook.EndPoint = Plugin.Configuration.WebHook;
        }

        if (ImGui.Button("保存"))
        {
            this.WebHook.Save();
        }

        if (ImGui.Button("潜水艦の情報をdiscordに送信"))
        {
            //別スレッド
            System.Threading.Tasks.Task.Run(() =>
            {
                var res = sba.send_msg(this.WebHook.EndPoint);
                lock (lockobj)
                {
                    this.msg = res.Unwrap();
                }
            });
        }
        var test = new ResultItems();
        var cc = new List<uint>();
        foreach (var a in this.HousingManager->WorkshopTerritory->Submersible.DataListSpan)
        {
            foreach (var b in a.GatheredDataSpan)
            {
                if (b.ItemIdPrimary != 0)
                {
                    test.ItemPush(b.ItemIdPrimary, b.ItemHQAdditional, b.ItemCountPrimary);
                }
                else
                {
                    test.ItemPush(b.ItemIdAdditional, b.ItemHQAdditional, b.ItemCountAdditional);
                }
            }
        }
        if (ImGui.Button("コピー"))
        {
            var clip_bord = Framework.Instance()->UIClipboard->Data;
            clip_bord.SetCopyStagingText(Utf8String.FromString($"{test.ItemStr()}\n{test.TotalValue()}"));
            clip_bord.ApplyCopyStagingText();
        }
        ImGui.Text(test.ItemStr().ToString());
        ImGui.Text(test.TotalValue());
        ImGui.Text(this.msg);
    }
}
