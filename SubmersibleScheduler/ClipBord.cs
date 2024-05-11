using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.System.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SubmersibleScheduler
{
    public static class ClipBord
    {
        public static unsafe void Copy(string str)
        {
            var clip_bord = Framework.Instance()->UIClipboard->Data;
            clip_bord.SetCopyStagingText(Utf8String.FromString(str));
            clip_bord.ApplyCopyStagingText();
        }
    }
}
