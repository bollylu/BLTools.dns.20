using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BLTools.Diagnostic.Logging;

namespace BLTools
{
    /// <summary>
    /// Watch a condition continuously, until cancelled. When the condition is met, execute the associated action
    /// </summary>
    public class TConditionMonitor : AConditionMonitor
    {
        /// <summary>
        /// The predicate to evaluate
        /// </summary>
        public Func<bool> Condition { get; set; }

        /// <summary>
        /// The action to execute when the condition is met
        /// </summary>
        public Action WhenCondition { get; set; }

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        public TConditionMonitor() { }

        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        /// <param name="name">A name for the mmonitor</param>
        /// <param name="condition">The condition to evaluate. When true, triggers the action</param>
        /// <param name="action">The action to execute when condition is met</param>
        /// <param name="delay">The delay between two evaluation of the condition</param>
        public TConditionMonitor(string name, Func<bool> condition, Action action, int delay = DEFAULT_DELAY_IN_MS)
        {
            Name = name;
            Condition = condition;
            WhenCondition = action;
            Delay = delay;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Cancel();
                    base.Dispose(true);
                }
                disposedValue = true;
            }
        }
        #endregion IDisposable Support
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        /// <summary>
        /// Start the monitoring
        /// </summary>
        public void Start()
        {
            #region === Validate parameters ===
            if ( Condition == null )
            {
                LogError($"Unable to monitor {Name} : condition is missing");
                return;
            }

            if ( WhenCondition == null )
            {
                LogError($"Unable to monitor {Name} : action is missing");
                return;
            }

            if ( IsMonitoring )
            {
                LogWarning($"Unable to monitor {Name} : Attempt to start monitor thread more than once");
                return;
            }
            #endregion === Validate parameters ===

            lock ( _LockMonitor )
            {
                #region --- Defines the thread content --------------------------------------------
                ThreadStart MonitorThreadStart = new ThreadStart(() =>
                {
                    _ContinueMonitor = true;
                    LogDebug($"#### Entering monitoring thread {Name}");
                    while ( _ContinueMonitor )
                    {
                        if ( Condition() )
                        {
                            WhenCondition.Invoke();
                        }

                        if ( _ContinueMonitor )
                        {
                            Thread.Sleep(Delay);
                        }
                    }
                    LogDebug($"**** Leaving monitoring thread {Name}");
                });
                #endregion --- Defines the thread content -----------------------------------------

                #region --- Defines the thread conditions --------------------------------------------
                _MonitorThread = new Thread(MonitorThreadStart)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                    Name = this.Name
                };
                #endregion --- Defines the thread conditions -----------------------------------------

                _MonitorThread.Start();
            }
        }

        /// <summary>
        /// Stop the monitoring
        /// </summary>
        public void Cancel()
        {
            lock ( _LockMonitor )
            {
                if ( _MonitorThread != null )
                {
                    StringBuilder LogText = new StringBuilder($"Cancelling monitor {Name} : ");
                    try
                    {
                        _ContinueMonitor = false;
                        _MonitorThread?.Join(Delay * 10);
                        _MonitorThread = null;
                        LogText.Append("OK");
                    }
                    catch ( Exception ex )
                    {
                        LogText.Append($"FAILED : {ex.Message}");
                    }
                    finally
                    {
                        LogDebug(LogText.ToString());
                    }
                }
            }
        }
    }

    /// <summary>
    /// Watch a condition continuously, until cancelled. When the condition is met, execute the associated action
    /// </summary>
    /// <typeparam name="T">The type (reference, not value) to feed both the predicate and the action</typeparam>
    public class TConditionMonitor<T> : AConditionMonitor where T : class
    {
        /// <summary>
        /// The predicate to evaluate (the reference of the monitored item is passed to it)
        /// </summary>
        public Predicate<T> Condition { get; set; }

        /// <summary>
        /// The action to execute when the condition is met (the reference of the monitored item is passed to it)
        /// </summary>
        public Action<T> WhenCondition { get; set; }

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        public TConditionMonitor() { }

        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        /// <param name="name">A name for the mmonitor</param>
        /// <param name="predicate">The condition to evaluate. When true, triggers the action</param>
        /// <param name="action">The action to execute when condition is met</param>
        /// <param name="delay">The delay between two evaluation of the condition</param>
        public TConditionMonitor(string name, Predicate<T> predicate, Action<T> action, int delay = DEFAULT_DELAY_IN_MS)
        {
            Name = name;
            Condition = predicate;
            WhenCondition = action;
            Delay = delay;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Cancel();
                    base.Dispose(true);
                }
                disposedValue = true;
            }
        }
        #endregion IDisposable Support
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        /// <summary>
        /// Start the monitoring
        /// </summary>
        /// <param name="obj">The reference of the object to monitor</param>
        public void Start(T obj)
        {
            #region === Validate parameters ===
            if ( Condition == null )
            {
                LogError($"Unable to monitor {Name} : condition is missing");
                return;
            }

            if ( WhenCondition == null )
            {
                LogError($"Unable to monitor {Name} : action is missing");
                return;
            }

            if ( IsMonitoring )
            {
                LogWarning($"Unable to monitor {Name} : Attempt to start monitor thread more than once");
                return;
            }
            #endregion === Validate parameters ===

            lock ( _LockMonitor )
            {
                #region --- Defines the thread content --------------------------------------------
                ThreadStart MonitorThreadStart = new ThreadStart(() =>
                {
                    _ContinueMonitor = true;
                    LogDebug($"#### Entering monitoring thread {Name}");
                    while ( _ContinueMonitor )
                    {
                        if ( Condition(obj) )
                        {
                            WhenCondition.Invoke(obj);
                        }

                        if ( _ContinueMonitor )
                        {
                            Thread.Sleep(Delay);
                        }
                    }
                    LogDebug($"**** Leaving monitoring thread {Name}");
                });
                #endregion --- Defines the thread content -----------------------------------------

                #region --- Defines the thread conditions --------------------------------------------
                _MonitorThread = new Thread(MonitorThreadStart)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                    Name = this.Name
                };
                #endregion --- Defines the thread conditions -----------------------------------------

                _MonitorThread.Start();
            }
        }

        /// <summary>
        /// Stop the monitoring
        /// </summary>
        public void Cancel()
        {
            lock ( _LockMonitor )
            {
                if ( _MonitorThread != null )
                {
                    StringBuilder LogText = new StringBuilder($"Cancelling monitor {Name} : ");
                    try
                    {
                        _ContinueMonitor = false;
                        _MonitorThread?.Join(Delay * 10);
                        _MonitorThread = null;
                        LogText.Append("OK");
                    }
                    catch ( Exception ex )
                    {
                        LogText.Append($"FAILED : {ex.Message}");
                    }
                    finally
                    {
                        LogDebug(LogText.ToString());
                    }
                }
            }
        }
    }
}
