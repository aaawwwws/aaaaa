using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using SubmersibleScheduler.Submarine;
using System.Collections.Generic;
using SubmersibleScheduler.Json;
using System.Runtime.CompilerServices;
using Req = SubmersibleScheduler.Request;
using SubmersibleScheduler.RaidMacro;
using System.Net.Http;
namespace SubmersibleScheduler.Windows;

public unsafe class MainWindow : Window, IDisposable
{
#pragma warning disable IDE1006 // 命名スタイル
    private readonly Plugin Plugin;
    private WebHook WebHook;
    private string msg;
    private string res_csv;
    private HousingManager* HousingManager;
    private SubmarineList? SubmarineList;
    private RaidMacro.RaidMacro macro;
    private string Path;
    private readonly string Err;
    public MainWindow(Plugin plugin, HousingManager* hm)
        : base("メイン")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(380, 380),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.Plugin = plugin;
        this.WebHook = new WebHook(Plugin);
        this.msg = string.Empty;
        this.HousingManager = hm;
        this.res_csv = string.Empty;
        this.SubmarineList = null;
        this.Err = string.Empty;
        try
        {
            this.macro = new Req.Request().GetMacro().GetAwaiter().GetResult();
        }
        catch (HttpRequestException)
        {
            this.Err = "取得できませんでした";
        }
        this.Path = this.Plugin.Configuration.Path == string.Empty ? string.Empty : this.Plugin.Configuration.Path;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.BeginTabBar("タブ");
        if (ImGui.BeginTabItem("潜水艦"))
        {
            if (this.SubmarineList == null && this.HousingManager->WorkshopTerritory != null)
            {
                this.SubmarineList = new SubmarineList(this.HousingManager->WorkshopTerritory->Submersible);
                return;
            }

            var wt = this.HousingManager->WorkshopTerritory;

            if (wt == null)
            {
                ImGui.Text("潜水艦を確認してください。");
                ImGui.EndTabItem();
            }
            else
            {
                var sm_data = wt->Submersible;

                var submarine_list = new SubmarineList(sm_data);
                ImGui.Text(submarine_list.TotalItem());
                ImGui.Text(submarine_list.StrTotalValue());

                var now_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                const long RETURN_OJ_TIME = 72960;
                var last_time = this.Plugin.Configuration.LastTime;

                var time_check = last_time != string.Empty ? long.Parse(last_time) : 0;

                var return_time = time_check + RETURN_OJ_TIME;

                if (ImGui.Button("コピー"))
                {
                    ClipBord.Copy($"{submarine_list.TotalItem()}\n{submarine_list.StrTotalValue()}");
                }

                ImGui.Text("日付で判定するようにしたのでその日の利益が確定したら押してください。");
                if (ImGui.Button("CSV書き出し(beta)"))
                {
                    this.Plugin.Configuration.ReturnBools = submarine_list.ReturnBools();
                    this.res_csv = submarine_list.WriteCsv(Plugin.Configuration.Path) switch
                    {
                        Enum.WriteCode.Success => "成功",
                        Enum.WriteCode.WriteError => "書き込みエラー",
                        Enum.WriteCode.PathError => "パスエラー",
                        Enum.WriteCode.Duplicated => "既に書き込んでいます。",
                        _ => "不明",
                    };
                    this.Plugin.Configuration.LastTime = now_time.ToString();
                    this.Plugin.Configuration.Save();
                }
                ImGui.Text(this.res_csv);
                ImGui.Text(this.msg);
                ImGui.EndTabItem();
            }
        }
        if (ImGui.BeginTabItem("マクロ(実装予定)"))
        {
            if (this.Err == string.Empty)
            {
                this.macro.Test();
            }
            else
            {
                ImGui.Text("接続エラー");
            }
            ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem("設定"))
        {
            ImGui.Text("CSVを出力するフォルダのパスを入力");
            if (ImGui.InputText(string.Empty, ref this.Path, 128))
            {
                this.Plugin.Configuration.Path = this.Path;
                this.Plugin.Configuration.Save();
            }
            ImGui.EndTabItem();
        }
        ImGui.EndTabBar();
    }
}
