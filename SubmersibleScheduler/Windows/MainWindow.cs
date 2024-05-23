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
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Info;
using SubmersibleScheduler.YabaiPlayer;
using System.Threading;
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
    private readonly YabaiPlayer.YabaiPlayer YabaiPlayer;
    private List<YabaiJson> Res_Yabai;
    private string Yabai_Error;
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
        this.YabaiPlayer = new YabaiPlayer.YabaiPlayer();
        this.Yabai_Error = string.Empty;
        try
        {
            this.Res_Yabai = new Request.Request().GetYabai().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            this.Yabai_Error = "取得できませんでした";
        }
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
            }
            ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("マクロ(実装予定)"))
        {
            ImGui.Text("もはや潜水艦とは全く関係のない機能の追加");
            if (this.Err == string.Empty)
            {
                this.macro.Test();
            }
            else
            {
                ImGui.Text("接続エラー");
                if (ImGui.Button("再取得"))
                {
                    this.macro = new Req.Request().GetMacro().GetAwaiter().GetResult();
                }
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
        if (ImGui.BeginTabItem("ヤバい人ランキング"))
        {
            //ガチ適当なので消す
            ImGui.Text("怒られたくないので消します。まだ制作中");
            ImGui.Text("名前 例:Aaa Bbb");
            ImGui.InputText("##Name", ref this.YabaiPlayer.RefName(), 128);
            ImGui.Text("サーバー 例:Anima");
            ImGui.InputText("##Server", ref this.YabaiPlayer.RefServer(), 128);
            var reqest = new Req.Request();

            if (ImGui.Button("送信"))
            {
                var trd = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        var res = reqest.PostYabai(Service.ClientState.LocalPlayer.Name.ToString(), Service.ClientState.LocalPlayer.CurrentWorld.GameData.Name.ToString(), this.YabaiPlayer).GetAwaiter().GetResult();
                        ImGui.Text(Service.ClientState.LocalPlayer.CurrentWorld.GameData.Name.ToString());
                    }
                    catch (Exception)
                    {
                        ImGui.Text("error");
                    }
                }));
                trd.Start();
            }
            if (this.Yabai_Error == string.Empty)
            {
                foreach (var item in this.Res_Yabai)
                {
                    ImGui.Text($"名前:{item.name} やばいカウント:{item.count}回");
                }
            }
            else
            {
                ImGui.Text(this.Yabai_Error);
            }
            if (ImGui.Button("再取得"))
            {
                var trd = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        this.Res_Yabai = reqest.GetYabai().GetAwaiter().GetResult();
                    }
                    catch (Exception)
                    {
                        ImGui.Text("error");
                    }
                }
                ));
                trd.Start();
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }
    }
}
