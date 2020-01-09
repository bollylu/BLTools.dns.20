using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Diagnostic.Logging
{
    public class TTraceLogger : ALogger
    {

        public TTraceLogger()
        {
            Listener = new DefaultTraceListener();
            _Initialize();
        }

        public TTraceLogger(ILogger logger)
        {
            Listener = new DefaultTraceListener();
            _Initialize();
            Listener.IndentSize = logger.Listener.IndentSize;
        }

        private bool _IsInitialized = false;

        protected override void _Initialize()
        {
            if ( _IsInitialized )
            {
                return;
            }
            base._Initialize();
            _IsInitialized = true;
        }
    }
}
