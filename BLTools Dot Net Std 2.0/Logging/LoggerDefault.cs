using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools {
  public class LoggerDefault : ISimpleLogger {
    public void Write(string message) {
      Trace.WriteLine(message);
    }

    public void Write(string message, ErrorLevel severity) {
      Trace.WriteLine(message, Severity.GetSeverity(severity));
    }
  }
}
