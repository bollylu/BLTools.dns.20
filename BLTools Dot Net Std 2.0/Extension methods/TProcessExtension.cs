using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArcelorInfraSoftLib {
  public static class TProcessExtension {
    public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken)) {
      process.EnableRaisingEvents = true;
      var TCS = new TaskCompletionSource<object>();
      process.Exited += (sender, args) => TCS.TrySetResult(null);
      if ( cancellationToken != default(CancellationToken) ) {
        cancellationToken.Register(() => TCS.TrySetCanceled());
      }
      return TCS.Task;
    }

    public static Task WaitForExitAsync(this Process process, int timeoutInMillisec) {
      CancellationTokenSource CTS = new CancellationTokenSource();
      CTS.CancelAfter(timeoutInMillisec);
      CancellationToken CT = CTS.Token;
      return process.WaitForExitAsync(CT);
    }

    public static Task WaitForExitAsync(this Process process, TimeSpan timeSpan) {
      CancellationTokenSource CTS = new CancellationTokenSource();
      CTS.CancelAfter(timeSpan);
      CancellationToken CT = CTS.Token;
      return process.WaitForExitAsync(CT);
    }
  }
}
