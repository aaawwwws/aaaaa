using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class SubMarineArray
    {
        private Queue<SubMarine> Data;

        public unsafe SubMarineArray (HousingWorkshopSubmersibleData hwsd)
        {
            this.Data = new Queue<SubMarine> (); 
            for (int i = 0; i < hwsd.DataListSpan.Length; i++)
            {
                if (hwsd.DataPointerListSpan[i].Value == null)
                {
                    break;
                }
                var sm = new SubMarine(hwsd.DataPointerListSpan[i].Value->GetReturnTime(), i + 1);
                Data.Enqueue(sm);
            }
        }

        public Result<string> send_msg (string webhook)
        {
            StringBuilder msg = new StringBuilder();
            foreach (var item in Data)
            {
                msg.Append(item.msg());
            }

            try
            {
                new Discord(webhook, msg.ToString()).SendMsg().GetAwaiter().GetResult();
                return new Result<string>("成功", Res.Ok);
            }
            catch (Exception e) {
                return new Result<string>(e.Message, Res.Err);
            }
        }
    }
}
