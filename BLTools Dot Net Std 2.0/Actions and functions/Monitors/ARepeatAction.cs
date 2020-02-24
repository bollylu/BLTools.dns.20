using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BLTools.Diagnostic.Logging;

namespace BLTools
{
    public abstract class ARepeatAction : ALoggable, IDisposable
    {
        /// <summary>
        /// A name for debug
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Default constant value for delay between two evaluations of the condition
        /// </summary>
        public const int DEFAULT_DELAY_IN_MS = 5;

        /// <summary>
        /// The delay between two evaluations of the condition
        /// </summary>
        public int Delay
        {
            get
            {
                if ( _Delay <= 0 )
                {
                    return DEFAULT_DELAY_IN_MS;
                }
                return _Delay;
            }
            set
            {
                _Delay = value;
            }
        }
        private int _Delay;

        /// <summary>
        /// When true, the repeat action is running
        /// </summary>
        public bool IsWorking => _LoopThread != null;

        protected bool _ContinueLoop = false;
        protected Thread _LoopThread;
        protected readonly object _LockLoop = new object();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    _ContinueLoop = false;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
