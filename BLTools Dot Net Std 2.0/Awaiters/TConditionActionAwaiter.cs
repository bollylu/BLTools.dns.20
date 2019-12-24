using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools
{
    public class TConditionActionAwaiter
    {
        protected Func<bool> _Condition;
        protected Action<double> _ProgressAction;

        protected double _TimeoutInMsec;
        protected int _RefreshRateInMsec;

        protected int _DurationToAwait;
        protected int _AwaitedDuration;

        public TConditionActionAwaiter(Func<bool> condition, Action<double> progressAction, double timeoutInMsec, int refreshRateInMsec)
        {
            _Condition = condition;
            _ProgressAction = progressAction;
            _TimeoutInMsec = timeoutInMsec;
            _RefreshRateInMsec = refreshRateInMsec;
        }

        public TConditionActionAwaiter(int durationInSec, Action<double> progressAction, int refreshRateInMsec)
        {
            _DurationToAwait = durationInSec;
            _Condition = () => (_AwaitedDuration * 1000) < _DurationToAwait;
            _TimeoutInMsec = double.MaxValue;
            _ProgressAction = progressAction;
            _RefreshRateInMsec = refreshRateInMsec;
        }

        public bool Execute(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!_Condition() && _AwaitedDuration < _TimeoutInMsec)
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = _TimeoutInMsec - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > _RefreshRateInMsec)
                {
                    _ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                Thread.Sleep(pollingDelayInMsec);
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return _Condition();
        }

        public async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!_Condition() && _AwaitedDuration < _TimeoutInMsec)
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = _TimeoutInMsec - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > _RefreshRateInMsec)
                {
                    _ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return _Condition();
        }

    }
}
