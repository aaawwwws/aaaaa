using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Newtonsoft.Json;
using SubmersibleScheduler.Json;
namespace SubmersibleScheduler.RaidMacro
{
    public class RaidMacro
    {
        private readonly List<Macro> MacroList;
        public RaidMacro(List<Macro> macro_list)
        {
            this.MacroList = macro_list;
        }
        public void Test()
        {
            var i = 1;
            foreach (var macro in MacroList)
            {
                if (ImGui.CollapsingHeader(macro.field_name))
                {
                    foreach (var mcr in macro.macro_list)
                    {
                        var uniqueLabel = $"{macro.field_name} - {i}マクロ目";
                        if (ImGui.CollapsingHeader(uniqueLabel))
                        {
                            ImGui.Text(mcr);
                            if (ImGui.Button($"コピー##{macro.field_name}{i}"))
                            {
                                ClipBord.Copy(mcr);
                            }
                        }
                        i++;
                    }
                    ImGui.Text("");
                }
                i = 1;
            }
        }

    }
}
