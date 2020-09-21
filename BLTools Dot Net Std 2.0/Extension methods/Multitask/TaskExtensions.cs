using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools {
  public static class TaskExtensions {
    public static async Task Attempt(this Task task, int attempt) {
      #region === Validate parameters ===
      if (attempt <= 0) {
        attempt = 1;
      }
      #endregion === Validate parameters ===

      int Counter = 1;
      while (Counter <= attempt) {
        try {
          Counter++;
          await task;
          return;
        } catch (Exception ex) {
          Trace.WriteLine($"Error executing task on attempt {Counter} : {ex.Message}");
        }
      }
      return;
    }

    public static async Task<T> Attempt<T>(this Task<T> task, int attempt) {
      #region === Validate parameters ===
      if (attempt <= 0) {
        attempt = 1;
      }
      #endregion === Validate parameters ===

      int Counter = 1;
      while (Counter <= attempt) {
        try {
          Counter++;
          return await task;
        } catch (Exception ex) {
          Trace.WriteLine($"Error executing task<T> on attempt {Counter} : {ex.Message}");
        }
      }
      return default(T);
    }

    public static async Task WithCancellation(this Task task, CancellationToken cancellationToken) {

      TaskCompletionSource<bool> Tcs = new TaskCompletionSource<bool>();

      using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), Tcs)) {
        if (task != await Task.WhenAny(task, Tcs.Task)) {
          throw new OperationCanceledException(cancellationToken);
        }
      }

    }

    public static async Task WithTimeout(this Task task, int timeoutInMs) {

      CancellationToken CancelTimeout = new CancellationTokenSource(timeoutInMs).Token;

      TaskCompletionSource<bool> Tcs = new TaskCompletionSource<bool>();

      using (CancelTimeout.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), Tcs)) {
        if (task != await Task.WhenAny(task, Tcs.Task)) {
          throw new OperationCanceledException(CancelTimeout);
        }
      }

    }

    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken) {

      TaskCompletionSource<bool> Tcs = new TaskCompletionSource<bool>();

      using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), Tcs)) {
        if (task != await Task.WhenAny(task, Tcs.Task)) {
          throw new OperationCanceledException(cancellationToken);
        }
      }

      return task.Result;
    }

    public static async Task<T> WithTimeout<T>(this Task<T> task, int timeoutInMs) {

      CancellationToken CancelTimeout = new CancellationTokenSource(timeoutInMs).Token;

      TaskCompletionSource<bool> Tcs = new TaskCompletionSource<bool>();

      using (CancelTimeout.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), Tcs)) {
        if (task != await Task.WhenAny(task, Tcs.Task)) {
          throw new OperationCanceledException(CancelTimeout);
        }
      }

      return task.Result;
    }
  }
}
