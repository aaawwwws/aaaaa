using Dalamud.IoC;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class Service
    {
        [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;
    }
}
