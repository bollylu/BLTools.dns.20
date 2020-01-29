using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BLTools
{
    public class TRepeatAction : ARepeatAction
    {
        /// <summary>
        /// The action to execute when the condition is met (the reference of the monitored item is passed to it)
        /// </summary>
        public Action ToDo { get; set; }

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        public TRepeatAction() { }

        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        /// <param name="name">A name for the mmonitor</param>
        /// <param name="predicate">The condition to evaluate. When true, triggers the action</param>
        /// <param name="action">The action to execute when condition is met</param>
        /// <param name="delay">The delay between two evaluation of the condition</param>
        public TRepeatAction(string name, Action todo, int delay = DEFAULT_DELAY_IN_MS)
        {
            Name = name;
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
                        ToDo.Invoke();

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

    public class TRepeatAction<T> : ARepeatAction
    {
        /// <summary>
        /// The action to execute when the condition is met (the reference of the monitored item is passed to it)
        /// </summary>
        public Action<T> ToDo { get; set; }

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        public TRepeatAction() { }

        /// <summary>
        /// Monitor a condition, triggering an action when condition is met
        /// WARNING : Monitoring is done in a background thread, beware of UI
        /// </summary>
        /// <param name="name">A name for the mmonitor</param>
        /// <param name="predicate">The condition to evaluate. When true, triggers the action</param>
        /// <param name="action">The action to execute when condition is met</param>
        /// <param name="delay">The delay between two evaluation of the condition</param>
        public TRepeatAction(string name, Action<T> todo, Predicate<T> predicate, int delay = DEFAULT_DELAY_IN_MS)
        {
            Name = name;
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

        public void Start(T obj)
        {
            #region === Validate parameters ===
            if ( ToDo == null )
            {
                LogError($"Unable to repeat {Name} : action is missing");
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
