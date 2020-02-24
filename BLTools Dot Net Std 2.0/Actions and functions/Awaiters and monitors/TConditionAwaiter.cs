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
        protected Func<bool> _Condition;

        /// <summary>
        /// Wait for a condition to be met or timeout
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="timeoutInMsec">The timeout in msec</param>
        public TConditionAwaiter(Func<bool> condition, long timeoutInMsec)
        {
            _Condition = condition;
            TimeoutInMsec = timeoutInMsec;
        }

        /// <summary>
        /// Start the wait process
        /// </summary>
        /// <param name="pollingDelayInMsec">Delay in ms to wait between evalution of the condition</param>
        /// <returns>true if condition was met, false if timeout</returns>
        public bool Execute(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            
            bool RetVal = _Condition();
            double _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;

            while (!RetVal && _AwaitedDuration < TimeoutInMsec)
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
        public async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;

            bool RetVal = _Condition();
            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;

            while (!RetVal && _AwaitedDuration < TimeoutInMsec)
            {
                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                RetVal = _Condition();
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return RetVal;
        }
    }

    /// <summary>
    /// Wait for a condition to be met or timeout
    /// </summary>
    public class TConditionAwaiter<T> : AConditionAwaiter
    {
        protected Predicate<T> _Predicate;

        /// <summary>
        /// Wait for a condition to be met or timeout
        /// </summary>
        /// <param name="predicate">The predicate to evaluate</param>
        /// <param name="timeoutInMsec">The timeout in msec</param>
        public TConditionAwaiter(Predicate<T> predicate, long timeoutInMsec)
        {
            _Predicate = predicate;
            TimeoutInMsec = timeoutInMsec;
        }

        /// <summary>
        /// Start the wait process
        /// </summary>
        /// <param name="source">The reference to an object used to the predicate evaluation</param>
        /// <param name="pollingDelayInMsec">Delay in ms to wait between evalution of the condition</param>
        /// <returns>true if condition was met, false if timeout</returns>
        public bool Execute(T source, int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;

            bool RetVal = _Predicate(source);
            double _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;

            while ( !RetVal && _AwaitedDuration < TimeoutInMsec )
            {
                Thread.Sleep(pollingDelayInMsec);
                RetVal = _Predicate(source);
                _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;
            }

            return RetVal;
        }

        /// <summary>
        /// Start the wait process asynchronously
        /// </summary>
        /// <param name="source">The reference to an object used to the predicate evaluation</param>
        /// <param name="pollingDelayInMsec">Delay in ms to wait between evalution of the condition</param>
        /// <returns>true if condition was met, false if timeout</returns>
        public async Task<bool> ExecuteAsync(T source, int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;

            bool RetVal = _Predicate(source);
            double _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;

            while ( !RetVal && _AwaitedDuration < TimeoutInMsec )
            {
                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                RetVal = _Predicate(source);
                _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;
            }

            return RetVal;
        }
    }
}
