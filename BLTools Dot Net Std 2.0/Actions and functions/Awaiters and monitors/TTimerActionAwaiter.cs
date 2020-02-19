using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools
{
    /// <summary>
    /// Wait for a condition to be met or timeout, while executing an action
    /// </summary>
    public class TTimerActionAwaiter : AConditionAwaiter
    {
        protected Func<bool> _Condition;
        protected Action<double> _ProgressAction;

        protected int _RefreshRateInMsec;

        /// <summary>
        /// Wait for a certain amount of time, while executing an action at a certain refresh rate
        /// </summary>
        /// <param name="durationInMSec">How long to wait</param>
        /// <param name="progressAction">The action to execute while waiting, the value passed as parameter is the number of milliseconds remaining before the timeout</param>
        /// <param name="refreshRateInMsec">How often to execute the action</param>
        public TTimerActionAwaiter(int durationInMSec, Action<double> progressAction, int refreshRateInMsec)
        {
            _DurationToAwait = durationInMSec;
            _Condition = () => (_AwaitedDuration) < _DurationToAwait;
            _TimeoutInMsec = long.MaxValue;
            _ProgressAction = progressAction;
            _RefreshRateInMsec = refreshRateInMsec;
        }

        /// <summary>
        /// Start to wait
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public bool Execute(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            bool RetVal = _Condition();
            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!RetVal && _AwaitedDuration < _TimeoutInMsec)
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = _TimeoutInMsec - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > _RefreshRateInMsec)
                {
                    _ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                Thread.Sleep(pollingDelayInMsec);
                RetVal = _Condition();
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return RetVal;
        }

        /// <summary>
        /// Start to wait asynchronously
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            bool RetVal = _Condition();
            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!RetVal && _AwaitedDuration < _TimeoutInMsec)
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = _TimeoutInMsec - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > _RefreshRateInMsec)
                {
                    _ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                RetVal = _Condition();
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return RetVal;
        }

    }
}
