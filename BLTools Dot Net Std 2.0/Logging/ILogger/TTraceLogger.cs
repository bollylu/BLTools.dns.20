using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Diagnostic.Logging {
  public class TTraceLogger : TLogger {

    public TTraceLogger() {
      Listener = new DefaultTraceListener();
      _Initialize();
    }

    public TTraceLogger(ILogger logger) : base(logger) {
      Listener = new DefaultTraceListener();
      _Initialize();
      Listener.IndentSize = logger.Listener.IndentSize;
    }

  }
}
