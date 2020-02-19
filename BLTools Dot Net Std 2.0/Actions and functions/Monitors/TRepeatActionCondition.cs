using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BLTools
{
    /// <summary>
    /// Execute an action until a condition is met or cancelled
    /// WARNING : execution is done in a background thread, beware of UI
    /// </summary>
    public class TRepeatActionCondition : ARepeatAction
    {
        /// <summary>
        /// The action to execute when the condition is met (the reference of the monitored item is passed to it)
        /// </summary>
        public Action ToDo { get; set; }

        /// <summary>
        /// The predicate to evaluate (the reference of the monitored item is passed to it)
        /// </summary>
        public Func<bool> ExitCondition { get; set; }

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        /// <summary>
        /// Execute an action until a condition is met or cancelled
        /// WARNING : execution is done in a background thread, beware of UI
        /// </summary>
        public TRepeatActionCondition() { }

        /// <summary>
        /// Execute an action until a condition is met or cancelled
        /// WARNING : execution is done in a background thread, beware of UI
        /// </summary>
        /// <param name="name">A name for the item</param>
        /// <param name="todo">The action to execute until condition is met</param>
        /// <param name="predicate">The condition to evaluate. When true, execution is cancelled</param>
        /// <param name="delay">The delay between two evaluation of the condition</param>
        public TRepeatActionCondition(string name, Action todo, Func<bool> exitCondition, int delay = DEFAULT_DELAY_IN_MS)
        {
            Name = name;
            ExitCondition = exitCondition;
            ToDo = todo;
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

        public void Start()
        {
            #region === Validate parameters ===
            if ( ToDo == null )
            {
                LogError($"Unable to repeat {Name} : action is missing");
                return;
            }

            if ( ExitCondition == null )
            {
                ExitCondition = () => true;
                return;
            }

            if ( IsWorking )
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
                    TConditionAwaiter WaitForTimeOrCancel = new TConditionAwaiter(() => !_ContinueMonitor, Delay);

                    _ContinueMonitor = true;
                    LogDebug($"#### Entering repeat thread {Name}");
                    while ( _ContinueMonitor )
                    {
                        ToDo.Invoke();

                        if ( ExitCondition() )
                        {
                            _ContinueMonitor = false;
                        }

                        if ( _ContinueMonitor )
                        {
                            WaitForTimeOrCancel.Execute(5);
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

        public void Cancel()
        {
            lock ( _LockMonitor )
            {
                if ( _MonitorThread != null )
                {
                    StringBuilder LogText = new StringBuilder($"Cancelling repeat {Name} : ");
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
    /// Execute an action until a condition is met or cancelled
    /// WARNING : execution is done in a background thread, beware of UI
    /// </summary>
    public class TRepeatActionCondition<T> : ARepeatAction
    {
        /// <summary>
        /// The action to execute when the condition is met (the reference of the monitored item is passed to it)
        /// </summary>
        public Action<T> ToDo { get; set; }

        /// <summary>
        /// The predicate to evaluate (the reference of the monitored item is passed to it)
        /// </summary>
        public Predicate<T> ExitCondition { get; set; }



        #region --- Constructor(s) ---------------------------------------------------------------------------------
        /// <summary>
        /// Execute an action until a condition is met or cancelled
        /// WARNING : execution is done in a background thread, beware of UI
        /// </summary>
        public TRepeatActionCondition() { }

        /// <summary>
        /// Execute an action until a condition is met or cancelled
        /// WARNING : execution is done in a background thread, beware of UI
        /// </summary>
        /// <param name="name">A name for the item</param>
        /// <param name="todo">The action to execute until condition is met</param>
        /// <param name="predicate">The condition to evaluate. When true, execution is cancelled</param>
        /// <param name="delay">The delay between two evaluation of the condition</param>
        public TRepeatActionCondition(string name, Action<T> todo, Predicate<T> predicate, int delay = DEFAULT_DELAY_IN_MS)
        {
            Name = name;
            ExitCondition = predicate;
            ToDo = todo;
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
        /// Start the monitoring process
        /// </summary>
        /// <param name="obj">The reference of the object to be monitored</param>
        public void Start(T obj)
        {
            #region === Validate parameters ===
            if ( ToDo == null )
            {
                LogError($"Unable to repeat {Name} : action is missing");
                return;
            }

            if ( ExitCondition == null )
            {
                ExitCondition = _ => true;
                return;
            }

            if ( IsWorking )
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
                    LogDebug($"#### Entering repeat thread {Name}");
                    while ( _ContinueMonitor )
                    {
                        ToDo.Invoke(obj);

                        if ( ExitCondition(obj) )
                        {
                            _ContinueMonitor = false;
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
        /// Stop the monitoring process
        /// </summary>
        public void Cancel()
        {
            lock ( _LockMonitor )
            {
                if ( _MonitorThread != null )
                {
                    StringBuilder LogText = new StringBuilder($"Cancelling repeat {Name} : ");
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
