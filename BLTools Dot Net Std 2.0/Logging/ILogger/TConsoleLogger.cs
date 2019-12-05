using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Diagnostic.Logging
{
    public class TConsoleLogger : TLogger
    {
        public TConsoleLogger()
        {
            Listener = new TextWriterTraceListener(Console.Out);
            _Initialize();
        }

        public TConsoleLogger(ILogger logger)
        {
            Listener = new TextWriterTraceListener(Console.Out);
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
