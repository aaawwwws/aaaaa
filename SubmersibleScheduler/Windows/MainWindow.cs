using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.Housing;
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
    private string csv_res;
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
        this.initt = true;
        this.HousingManager = hm;
        this.path = string.Empty;
        this.csv_res = string.Empty;
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

        if (ImGui.Button("潜水艦の情報をdiscordに送信"))
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
        long now_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var drop = new SMDrop(sm_data);
        const long RETURN_OJ_TIME = 72960;
        var last_time = this.Plugin.Configuration.LastTime;

        var time_check = last_time != string.Empty ? long.Parse(last_time) : 0;

        var return_time = time_check + RETURN_OJ_TIME;

        if (ImGui.Button("コピー"))
        {
            ClipBord.Copy($"{drop.items.ItemStr()}\n{drop.items.TotalValue()}");
        }

        ImGui.Text(drop.items.ItemStr());
        ImGui.Text(drop.items.TotalValue());


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
        }

        ImGui.Text("全ての潜水艦が戻ってきたタイミングで押してください。\n例外(3隻OJ、1隻MROJZ等2日かかる場合OJの3隻戻ってきたタイミングで押す)\n普通にめんどくさいので早めに改良します");
        if (ImGui.Button("CSV書き出し(beta)") && now_time > return_time)
        {
            this.csv_res = drop.WriteCsv(this.path) switch
            {
                Enum.WriteCode.Success => "成功",
                Enum.WriteCode.WriteError => "書き込みエラー",
                Enum.WriteCode.PathError => "パスエラー",
                _ => "不明",
            };
            this.Plugin.Configuration.LastTime = now_time.ToString();
            this.Plugin.Configuration.Save();
        }
        ImGui.Text(this.csv_res);
        ImGui.Text(this.msg);
    }
}
