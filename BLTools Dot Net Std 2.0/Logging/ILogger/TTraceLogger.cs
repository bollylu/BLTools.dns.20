using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Diagnostic.Logging {
    public class TTraceLogger : TLogger
    {

        public TTraceLogger()
        {
            Listener = new DefaultTraceListener();
            Listener.IndentSize = 2;
        }

        public TTraceLogger(ILogger logger) : base(logger)
        {
            Listener = new DefaultTraceListener();
            Listener.IndentSize = logger.Listener.IndentSize;
        }
    }
}
