﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BLTools.Diagnostic.Logging;

namespace BLTools
{
    public abstract class AConditionMonitor : ALoggable, IDisposable
    {
        public const int DEFAULT_DELAY_IN_MS = 5;
        public string Name { get; set; } = "";

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

        public bool IsMonitoring => _MonitorThread != null;

        protected bool _ContinueMonitor = false;
        protected Thread _MonitorThread;
        protected readonly object _LockMonitor = new object();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    _ContinueMonitor = false;
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
