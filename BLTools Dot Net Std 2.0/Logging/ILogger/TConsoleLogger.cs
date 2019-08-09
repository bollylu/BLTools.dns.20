using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Diagnostic.Logging {
    public class TConsoleLogger : TLogger
    {

        public TConsoleLogger()
        {
            Listener = new TextWriterTraceListener(Console.Out);
        }

        public TConsoleLogger(ILogger logger) : base(logger)
        {
            Listener = new TextWriterTraceListener(Console.Out);
        }

    }
}
