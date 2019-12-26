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

        public bool Execute(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            
            bool RetVal = _Condition();
            double _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;

            while (!RetVal && _AwaitedDuration < _TimeoutInMsec)
            {
                Thread.Sleep(pollingDelayInMsec);
                RetVal = _Condition();
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return RetVal;
        }

        public async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;

            bool RetVal = _Condition();
            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;

            while (!RetVal && _AwaitedDuration < _TimeoutInMsec)
            {
                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                RetVal = _Condition();
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return RetVal;
        }
    }
}
