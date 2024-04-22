using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmersibleScheduler
{
    public class Result <T>
    {
        private T Value;
        public Res Res { get; private set; }

        public Result (T value, Res result)
        {
            this.Value = value;
            this.Res = result;
        }

        public T Unwrap ()
        {
            return Value;
        }
    }

    public enum Res
    {
        Ok,
        Err,
    }
}
