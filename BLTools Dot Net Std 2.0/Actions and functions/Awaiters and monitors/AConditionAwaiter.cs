using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLTools
{
    public abstract class AConditionAwaiter : IConditionAwaiter
    {
        protected Func<bool> _Condition;
        protected double _TimeoutInMsec;

        protected int _DurationToAwait;
        protected int _AwaitedDuration;

        abstract public bool Execute(int pollingDelayInMsec = 5);
        abstract public Task<bool> ExecuteAsync(int pollingDelayInMsec = 5);
    }
}
