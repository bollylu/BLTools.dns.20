using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Diagnostic.Logging {
  public interface ILoggable
  {
    ILogger Logger { get; set; }
  }
}
