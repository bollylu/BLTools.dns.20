using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools
{
    /// <summary>
    /// Wait for a condition to be met or timeout
    /// </summary>
    public class TConditionAwaiter : AConditionAwaiter
    {
        /// <summary>
        /// Wait for a condition to be met or timeout
        /// </summary>
        public TConditionAwaiter(Func<bool> condition, double timeoutInMsec)
        {
            _Condition = condition;
            _TimeoutInMsec = timeoutInMsec;
        }

        /// <summary>
        /// Start the wait process
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay in ms to wait between evalution of the condition</param>
        /// <returns>true if condition was met, false if timeout</returns>
        public override bool Execute(int pollingDelayInMsec = 5)
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

        /// <summary>
        /// Start the wait process asynchronously
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay in ms to wait between evalution of the condition</param>
        /// <returns>true if condition was met, false if timeout</returns>
        public override async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
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
