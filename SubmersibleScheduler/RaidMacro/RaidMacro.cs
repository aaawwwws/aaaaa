using System.Collections.Generic;
using System.Numerics;
using System.Text;
using ImGuiNET;
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
                    ImGui.Indent();
                    foreach (var mcr in macro.macro_list)
                    {
                        var uniqueLabel = $"{macro.field_name} - {i}マクロ目";
                        if (ImGui.CollapsingHeader(uniqueLabel))
                        {
                            var copy = mcr;
                            ImGui.InputTextMultiline($"##{macro.field_name}{i}", ref copy, 512, new Vector2(400, 300));
                        }
                        i++;
                    }
                    ImGui.Unindent();
                }
                i = 1;
            }
        }

    }
}
