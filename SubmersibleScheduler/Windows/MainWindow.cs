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
using SubmersibleScheduler.Request;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using SubmersibleScheduler.Error;
using System.IO;
namespace SubmersibleScheduler.Windows;

public class MainWindow : Window, IDisposable
{
#pragma warning disable IDE1006 // 命名スタイル
    private readonly Plugin Plugin;
    private WebHook WebHook;
    private string msg;
    private SubmarineList? SubmarineList;
    private RaidMacro.RaidMacro macro;
    private string Path;
    private readonly string Err;
    private readonly YabaiPlayer.YabaiPlayer YabaiPlayer;
    private List<YabaiJson> Res_Yabai;
    private string Yabai_Error;
    private object objlock = new object();
    private SynchronizationContext synchronizationContext;
    private Message message;
    private string TotalValue;
    public unsafe MainWindow(Plugin plugin, SynchronizationContext sc)
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
        this.SubmarineList = null;
        this.Err = string.Empty;
        this.YabaiPlayer = new YabaiPlayer.YabaiPlayer();
        this.Yabai_Error = string.Empty;
        this.synchronizationContext = sc;
        this.message = new Message();
        this.TotalValue = string.Empty;
        try
        {
            this.TotalValue = ReadFile.TotalValue(plugin.Configuration.Path);
        }
        catch (Exception ex)
        {
            this.TotalValue = "合計値を取得できませんでした。";
        }
        try
        {
            var req = new Request.Request();
            this.Res_Yabai = req.GetYabai().GetAwaiter().GetResult();
            req.Dispose();
        }
        catch (Exception ex)
        {
            this.Yabai_Error = "取得できませんでした";
        }
        try
        {
            var req = new Req.Request();
            this.macro = req.GetMacro().GetAwaiter().GetResult();
            req.Dispose();
        }
        catch (HttpRequestException)
        {
            this.Err = "取得できませんでした";
        }
        this.Path = this.Plugin.Configuration.Path == string.Empty ? string.Empty : this.Plugin.Configuration.Path;
    }

    public void Dispose()
    {
    }

    private unsafe HousingWorkshopSubmersibleData Aaa()
    {
        var housing = HousingManager.Instance();
        return housing->WorkshopTerritory->Submersible;
    }

    public override void Draw()
    {
        HousingWorkshopSubmersibleData sm_data;
        try
        {
            sm_data = this.Aaa();
        }
        catch (Exception)
        {
            ImGui.Text("潜水艦を確認してください");
            return;
        }

        ImGui.BeginTabBar("タブ");
        if (ImGui.BeginTabItem("潜水艦"))
        {
            var submarine_list = new SubmarineList(sm_data);

            ImGui.Text(submarine_list.TotalItem());
            ImGui.Text(submarine_list.StrTotalValue());

            if (ImGui.Button("コピー"))
            {
                ClipBord.Copy($"{submarine_list.TotalItem()}\n{submarine_list.StrTotalValue()}");
            }

            ImGui.Text("日付で判定するようにしたのでその日の利益が確定したら押してください。");
            if (ImGui.Button("CSV書き出し(beta)"))
            {
                try
                {
                    submarine_list.WriteCsv(Plugin.Configuration.Path);
                    this.message.CsvMsg = "成功";
                }
                catch (DuplicateException e)
                {
                    this.message.CsvMsg = e.Message;
                }
                catch (DirectoryNotFoundException e)
                {
                    this.message.CsvMsg = e.Message;
                }
                catch (Exception)
                {
                    this.message.CsvMsg = "原因不明なエラー";
                }
            }
            ImGui.Text(this.TotalValue);
            ImGui.Text(this.message.CsvMsg);
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
        /*
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
                await Task.Run(() =>
                {
                    try
                    {
                        var res = reqest.PostYabai(Service.ClientState.LocalPlayer.Name.ToString(), Service.ClientState.LocalPlayer.CurrentWorld.GameData.Name.ToString(), this.YabaiPlayer).GetAwaiter().GetResult();
                    }
                    catch (Exception)
                    {
                        UpdateUI(() => ImGui.Text("error"));
                    }
                });

            }

            if (ImGui.Button("再取得"))
            {
                var req = new Request.Request();
                await Task.Run(async () =>
                {
                    var req = new Request.Request();
                    var result = await req.GetYabai();
                    req.Dispose();

                    UpdateUI(() => this.Res_Yabai = result);

                });

            }

            if (this.Yabai_Error == string.Empty)
            {
                lock (objlock)
                {
                    foreach (var item in this.Res_Yabai)
                    {
                        ImGui.Text($"名前:{item.name} やばいカウント:{item.count}回");
                    }
                }
            }
            else
            {
                ImGui.Text(this.Yabai_Error);
            }
            ImGui.EndTabItem();
        }
            */

        ImGui.EndTabBar();
    }
}
