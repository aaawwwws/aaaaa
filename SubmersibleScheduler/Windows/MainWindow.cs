using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using SubmersibleScheduler.Submarine;
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
    private bool initt;
    private string path;
    private string res_csv;
    private HousingManager* HousingManager;
    private bool test;

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
        this.initt = true;
        this.HousingManager = hm;
        this.path = string.Empty;
        this.res_csv = string.Empty;
        this.test = false;
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

        var sm_data = wt->Submersible;

        var sma = new SubMarineArray(sm_data);

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

        if (ImGui.Button("潜水艦の情報をdiscordに送信a"))
        {
            //別スレッド
            System.Threading.Tasks.Task.Run(() =>
            {
                var res = sma.send_msg(this.WebHook.EndPoint);
                lock (lockobj)
                {
                    this.msg = res.Unwrap();
                }
            });
        }

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

        if (this.Plugin.Configuration.Path != string.Empty && this.initt)
        {
            initt = false;
            this.path = Plugin.Configuration.Path;
        }

        ImGui.InputText("CSVを出力するフォルダのパスを入力", ref this.path, (uint)128);
        if (ImGui.Button("パスを保存") && this.Plugin.Configuration.Path != this.path)
        {
            this.Plugin.Configuration.Path = this.path;
            this.Plugin.Configuration.Save();
            this.test = true;
        }
        ImGui.Text(submarine_list.test("F:\\Download"));

        ImGui.Text("全ての潜水艦が戻ってきたタイミングで押してください。\n例外(3隻OJ、1隻MROJZ等2日かかる場合OJの3隻戻ってきたタイミングで押す)\n普通にめんどくさいので早めに改良します");
        ImGui.Text("日付で判定するようにしたので適当に押してもらって構いません。");

        if (ImGui.Button("CSV書き出し(beta)"))
        {
            this.Plugin.Configuration.ReturnBools = submarine_list.ReturnBools();
            this.res_csv = submarine_list.WriteCsv(this.path) switch
            {
                Enum.WriteCode.Success => "成功",
                Enum.WriteCode.WriteError => "書き込みエラー",
                Enum.WriteCode.PathError => "パスエラー",
                _ => "不明",
            };
            this.Plugin.Configuration.LastTime = now_time.ToString();
            this.Plugin.Configuration.Save();
        }
        ImGui.Text(this.res_csv);
        ImGui.Text(this.msg);
    }
}
