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
        public Action<double> ProgressAction { get; set; }

        public int RefreshRateInMsec { get; set; }

        public int DurationToAwait { get; set; }

        /// <summary>
        /// Wait for a certain amount of time, while executing an action at a certain refresh rate
        /// </summary>
        /// <param name="durationInMSec">How long to wait</param>
        /// <param name="progressAction">The action to execute while waiting, the value passed as parameter is the number of milliseconds remaining before the timeout</param>
        /// <param name="refreshRateInMsec">How often to execute the action</param>
        public TTimerActionAwaiter(int durationInMSec, Action<double> progressAction, int refreshRateInMsec)
        {
            DurationToAwait = durationInMSec;
            ProgressAction = progressAction;
            RefreshRateInMsec = refreshRateInMsec;
        }

        /// <summary>
        /// Start to wait
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public void Execute(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while ( _AwaitedDuration < DurationToAwait )
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = DurationToAwait - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > RefreshRateInMsec)
                {
                    ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                Thread.Sleep(pollingDelayInMsec);
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return;
        }

        /// <summary>
        /// Start to wait asynchronously
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public async Task ExecuteAsync(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while ( _AwaitedDuration < DurationToAwait )
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = DurationToAwait - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > RefreshRateInMsec)
                {
                    ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return;
        }

    }
}
