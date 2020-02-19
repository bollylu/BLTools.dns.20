using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLTools
{
    public abstract class AConditionAwaiter : IConditionAwaiter
    {
        protected long _TimeoutInMsec;

        protected int _DurationToAwait;
        protected int _AwaitedDuration;

    }
}
