using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Processes {
  public static class ProcessHelper {

    public static IEnumerable<string> ExecuteProcess(string processName, string parameters) {
      if (string.IsNullOrWhiteSpace(processName)) {
        yield break;
      }

      ProcessStartInfo PSI = new ProcessStartInfo(processName, parameters);
      Process RequestedProcess = new Process {
        StartInfo = PSI
      };

      RequestedProcess.BeginOutputReadLine();

      yield break;
    }
  }
}
