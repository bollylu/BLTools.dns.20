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
    public class TConditionActionAwaiter : AConditionAwaiter
    {
        protected Action<double> _ProgressAction;

        protected int _RefreshRateInMsec;

        /// <summary>
        /// Wait for a condition to be met or timeout, while executing an action
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="progressAction">The progress action to execute</param>
        /// <param name="timeoutInMsec">The timeout</param>
        /// <param name="refreshRateInMsec">How often is the progress action executed</param>
        public TConditionActionAwaiter(Func<bool> condition, Action<double> progressAction, double timeoutInMsec, int refreshRateInMsec)
        {
            _Condition = condition;
            _ProgressAction = progressAction;
            _TimeoutInMsec = timeoutInMsec;
            _RefreshRateInMsec = refreshRateInMsec;
        }

        /// <summary>
        /// Wait for a certain amount of time, while executing an action at a certain refresh rate
        /// </summary>
        /// <param name="durationInSec">How long to wait</param>
        /// <param name="progressAction">The action to execute while waiting</param>
        /// <param name="refreshRateInMsec">How often to execute the action</param>
        public TConditionActionAwaiter(int durationInSec, Action<double> progressAction, int refreshRateInMsec)
        {
            _DurationToAwait = durationInSec;
            _Condition = () => (_AwaitedDuration * 1000) < _DurationToAwait;
            _TimeoutInMsec = double.MaxValue;
            _ProgressAction = progressAction;
            _RefreshRateInMsec = refreshRateInMsec;
        }

        /// <summary>
        /// Start to wait for the condition
        /// </summary>
        /// <remarks>Timeout > refresh rate > polling delay</remarks>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public override bool Execute(int pollingDelayInMsec = 5)
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
        /// Start to wait asynchronously for the condition
        /// </summary>
        /// <remarks>Timeout > refresh rate > polling delay</remarks>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public override async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
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
