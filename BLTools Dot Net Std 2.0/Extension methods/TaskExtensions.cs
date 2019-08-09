using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLTools
{
  public static class TaskExtensions
  {

    public static async Task Attempt(this Task task, int attempt)
    {
      #region === Validate parameters ===
      if (attempt <= 0)
      {
        attempt = 1;
      }
      #endregion === Validate parameters ===

      int Counter = 1;
      while (Counter <= attempt)
      {
        try
        {
          Counter++;
          await task;
          return;
        }
        catch (Exception ex)
        {
          Trace.WriteLine($"Error executing task on attempt {Counter} : {ex.Message}");
        }
      }
      return;
    }

    public static async Task<T> Attempt<T>(this Task<T> task, int attempt)
    {
      #region === Validate parameters ===
      if (attempt <= 0)
      {
        attempt = 1;
      }
      #endregion === Validate parameters ===

      int Counter = 1;
      while (Counter <= attempt)
      {
        try
        {
          Counter++;
          return await task;
        }
        catch (Exception ex)
        {
          Trace.WriteLine($"Error executing task<T> on attempt {Counter} : {ex.Message}");
        }
      }
      return default(T);
    }

  }
}
