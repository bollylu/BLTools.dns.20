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
        /// A function to evaluate if we continue or not
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
        /// <param name="exitCondition">The condition to evaluate : when true, repeat is complete</param>
        /// <param name="delay">The delay between two evaluations of the condition</param>
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

        /// <summary>
        /// Start the loop
        /// </summary>
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
                LogWarning($"Unable to repeat {Name} : Attempt to start repeat thread more than once");
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
                        ToDo.Invoke();

                        if ( ExitCondition() )
                        {
                            _ContinueLoop = false;
                        }

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
        /// Start the loop
        /// </summary>
        /// <param name="obj">The reference of the object to be used in evalution</param>
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

            lock ( _LockLoop )
            {
                #region --- Defines the thread content --------------------------------------------
                ThreadStart MonitorThreadStart = new ThreadStart(() =>
                {
                    _ContinueLoop = true;
                    LogDebug($"#### Entering repeat thread {Name}");
                    while ( _ContinueLoop )
                    {
                        ToDo.Invoke(obj);

                        if ( ExitCondition(obj) )
                        {
                            _ContinueLoop = false;
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
        /// Stop the repeat process
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
                        _LoopThread?.Join(Delay * 10);
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
