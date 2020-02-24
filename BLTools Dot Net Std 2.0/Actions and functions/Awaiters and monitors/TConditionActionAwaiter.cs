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
        public Func<bool> Condition { get; set; }
        public Action<double> ProgressAction { get; set; }

        public int RefreshRateInMsec { get; set; }

        /// <summary>
        /// Wait for a condition to be met or timeout, while executing an action
        /// </summary>
        /// <param name="condition">The condition to evaluate</param>
        /// <param name="progressAction">The action to execute while waiting, the value passed as parameter is the number of milliseconds remaining before the timeout</param>
        /// <param name="timeoutInMsec">The timeout</param>
        /// <param name="refreshRateInMsec">How often is the progress action executed</param>
        public TConditionActionAwaiter(Func<bool> condition, Action<double> progressAction, long timeoutInMsec, int refreshRateInMsec)
        {
            Condition = condition;
            ProgressAction = progressAction;
            TimeoutInMsec = timeoutInMsec;
            RefreshRateInMsec = refreshRateInMsec;
        }

        /// <summary>
        /// Start to wait for the condition
        /// </summary>
        /// <remarks>Timeout > refresh rate > polling delay</remarks>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public bool Execute(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            bool RetVal = Condition();
            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!RetVal && _AwaitedDuration < TimeoutInMsec)
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = TimeoutInMsec - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > RefreshRateInMsec)
                {
                    ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                Thread.Sleep(pollingDelayInMsec);
                RetVal = Condition();
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
        public async Task<bool> ExecuteAsync(int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            bool RetVal = Condition();
            double _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            while (!RetVal && _AwaitedDuration < TimeoutInMsec)
            {
                DisplayCounter = (DateTime.Now - DisplayCounterStartTime).TotalMilliseconds;
                double TimeLeft = TimeoutInMsec - (DateTime.Now - StartTime).TotalMilliseconds;

                if (DisplayCounter > RefreshRateInMsec)
                {
                    ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                RetVal = Condition();
                _AwaitedDuration = (DateTime.Now - StartTime).TotalMilliseconds;
            }

            return RetVal;
        }
    }

    /// <summary>
    /// Wait for a condition to be met or timeout, while executing an action
    /// </summary>
    public class TConditionActionAwaiter<T> : AConditionAwaiter
    {
        public Predicate<T> Predicate { get; set; }

        public Action<double> ProgressAction { get; set; }

        public int RefreshRateInMsec { get; set; }

        /// <summary>
        /// Wait for a condition to be met or timeout, while executing an action
        /// </summary>
        /// <param name="predicate">The condition to evaluate</param>
        /// <param name="progressAction">The action to execute while waiting, the value passed as parameter is the number of milliseconds remaining before the timeout</param>
        /// <param name="timeoutInMsec">The timeout</param>
        /// <param name="refreshRateInMsec">How often is the progress action executed</param>
        public TConditionActionAwaiter(Predicate<T> predicate, Action<double> progressAction, long timeoutInMsec, int refreshRateInMsec)
        {
            Predicate = predicate;
            ProgressAction = progressAction;
            TimeoutInMsec = timeoutInMsec;
            RefreshRateInMsec = refreshRateInMsec;
        }

        /// <summary>
        /// Start to wait for the condition
        /// </summary>
        /// <remarks>Timeout > refresh rate > polling delay</remarks>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public bool Execute(T source, int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            bool RetVal = Predicate(source);
            double _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;
            while ( !RetVal && _AwaitedDuration < TimeoutInMsec )
            {
                DisplayCounter = ( DateTime.Now - DisplayCounterStartTime ).TotalMilliseconds;
                double TimeLeft = TimeoutInMsec - ( DateTime.Now - StartTime ).TotalMilliseconds;

                if ( DisplayCounter > RefreshRateInMsec )
                {
                    ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                Thread.Sleep(pollingDelayInMsec);
                RetVal = Predicate(source);
                _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;
            }

            return RetVal;
        }

        /// <summary>
        /// Start to wait asynchronously for the condition
        /// </summary>
        /// <remarks>Timeout > refresh rate > polling delay</remarks>
        /// <param name="pollingDelayInMsec">Delay before next evaluation of the condition</param>
        /// <returns>true if condition is met, false if timeout</returns>
        public async Task<bool> ExecuteAsync(T source, int pollingDelayInMsec = 5)
        {
            DateTime StartTime = DateTime.Now;
            double DisplayCounter;
            DateTime DisplayCounterStartTime = DateTime.Now;

            bool RetVal = Predicate(source);
            double _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;
            while ( !RetVal && _AwaitedDuration < TimeoutInMsec )
            {
                DisplayCounter = ( DateTime.Now - DisplayCounterStartTime ).TotalMilliseconds;
                double TimeLeft = TimeoutInMsec - ( DateTime.Now - StartTime ).TotalMilliseconds;

                if ( DisplayCounter > RefreshRateInMsec )
                {
                    ProgressAction(TimeLeft);
                    DisplayCounterStartTime = DateTime.Now;
                }

                await Task.Delay(pollingDelayInMsec).ConfigureAwait(false);
                RetVal = Predicate(source);
                _AwaitedDuration = ( DateTime.Now - StartTime ).TotalMilliseconds;
            }

            return RetVal;
        }

    }
}
