using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools
{
    public class TRepeatAction : ARepeatAction
    {
        /// <summary>
        /// The action to execute every occurence of the delay
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

            lock ( _LockLoop )
            {
                #region --- Defines the thread content --------------------------------------------
                ThreadStart MonitorThreadStart = new ThreadStart(async () =>
                {
                    _ContinueLoop = true;
                    LogDebug($"#### Entering repeat thread {Name}");
                    bool IsAsync = ToDo.Method.GetCustomAttributes(typeof(AsyncStateMachineAttribute)).FirstOrDefault() != null;

                    while ( _ContinueLoop )
                    {
                        if ( IsAsync )
                        {
                            await Task.Run(ToDo);
                        }
                        else
                        {
                            ToDo.Invoke();
                        }

                        if ( _ContinueLoop )
                        {
                            Thread.Sleep(Delay);
                        }
                    }
                    LogDebug($"**** Leaving repeat thread {Name}");
                });
                #endregion --- Defines the thread content -----------------------------------------

                #region --- Defines the thread conditions --------------------------------------------
                _LoopThread = new Thread(MonitorThreadStart)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                    Name = this.Name
                };
                #endregion --- Defines the thread conditions -----------------------------------------

                _LoopThread.Start();
            }
        }

        /// <summary>
        /// Stop the execution of the loop
        /// </summary>
        public void Cancel()
        {
            Cancel(Delay * 10);
        }

        /// <summary>
        /// Stop the execution of the loop
        /// </summary>
        /// <param name="timeout">The timeout for the loop to be stopped, default is delay * 10</param>
        public void Cancel(int timeout)
        {
            lock ( _LockLoop )
            {
                if ( _LoopThread != null )
                {
                    StringBuilder LogText = new StringBuilder($"Cancelling repeat {Name} : ");
                    try
                    {
                        _ContinueLoop = false;
                        _LoopThread?.Join(timeout);
                        _LoopThread = null;
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

            lock ( _LockLoop )
            {
                #region --- Defines the thread content --------------------------------------------
                ThreadStart MonitorThreadStart = new ThreadStart(() =>
                {
                    TConditionAwaiter WaitForTimeOrCancel = new TConditionAwaiter(() => !_ContinueLoop, Delay);

                    _ContinueLoop = true;
                    LogDebug($"#### Entering repeat thread {Name}");
                    while ( _ContinueLoop )
                    {
                        ToDo.Invoke(obj);

                        if ( _ContinueLoop )
                        {
                            WaitForTimeOrCancel.Execute(5);
                        }
                    }
                    LogDebug($"**** Leaving repeat thread {Name}");
                });
                #endregion --- Defines the thread content -----------------------------------------

                #region --- Defines the thread conditions --------------------------------------------
                _LoopThread = new Thread(MonitorThreadStart)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest,
                    Name = this.Name
                };
                #endregion --- Defines the thread conditions -----------------------------------------

                _LoopThread.Start();
            }
        }

        /// <summary>
        /// Stop the execution of the loop
        /// </summary>
        public void Cancel()
        {
            Cancel(Delay * 10);
        }

        /// <summary>
        /// Stop the execution of the loop
        /// </summary>
        /// <param name="timeout">The timeout for the loop to be stopped, default is delay * 10</param>
        public void Cancel(int timeout)
        {
            lock ( _LockLoop )
            {
                if ( _LoopThread != null )
                {
                    StringBuilder LogText = new StringBuilder($"Cancelling repeat {Name} : ");
                    try
                    {
                        _ContinueLoop = false;
                        _LoopThread?.Join(timeout);
                        _LoopThread = null;
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
