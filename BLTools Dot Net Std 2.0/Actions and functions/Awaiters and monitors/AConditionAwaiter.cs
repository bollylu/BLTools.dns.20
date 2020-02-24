using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLTools
{
    public abstract class AConditionAwaiter : IConditionAwaiter
    {
        public long TimeoutInMsec { get; set; }
    }
}
