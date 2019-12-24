using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools
{
    public class TConditionAwaiter
    {
        protected Func<bool> _Condition;

        protected double _TimeoutInMsec;

        protected int _DurationToAwait;
        protected int _AwaitedDuration;

        public TConditionAwaiter(Func<bool> condition, double timeoutInMsec)
        {
            _Condition = condition;
            _TimeoutInMsec = timeoutInMsec;
        }

        public bool Execute(int pollingDelayInMsec = 1)
        {
            DateTime StartTime = DateTime.Now;

            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!_Condition() && _AwaitedDuration < _TimeoutInMsec)
            {
                Thread.Sleep(pollingDelayInMsec);
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return _Condition();
        }

        public async Task<bool> ExecuteAsync(int pollingDelayInMsec = 1)
        {
            DateTime StartTime = DateTime.Now;

            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!_Condition() && _AwaitedDuration < _TimeoutInMsec)
            {
                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return _Condition();
        }
    }
}
