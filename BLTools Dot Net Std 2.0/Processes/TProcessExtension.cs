using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools {
  public static class TProcessExtension {
    public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken) {
      process.EnableRaisingEvents = true;
      var TCS = new TaskCompletionSource<object>();
      process.Exited += (sender, args) => TCS.TrySetResult(null);
      if ( cancellationToken != default(CancellationToken) ) {
        cancellationToken.Register(() => TCS.TrySetCanceled());
      }
      return TCS.Task;
    }

    public static Task WaitForExitAsync(this Process process, int timeoutInMillisec) {
      CancellationTokenSource CTS = new CancellationTokenSource(timeoutInMillisec);
      return process.WaitForExitAsync(CTS.Token);
    }

    public static Task WaitForExitAsync(this Process process, TimeSpan timeSpan) {
      CancellationTokenSource CTS = new CancellationTokenSource(timeSpan);
      return process.WaitForExitAsync(CTS.Token);
    }
  }
}
