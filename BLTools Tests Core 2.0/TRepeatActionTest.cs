using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest {
  [TestClass]
  public class TRepeatActionTest {

    #region --- Test support --------------------------------------------
    private TestContext _TestContextInstance;
    public TestContext TestContext {
      get { return _TestContextInstance; }
      set { _TestContextInstance = value; }
    }
    #endregion --- Test support -----------------------------------------

    [TestMethod]
    public void Repeat_Sync_ExecIsOrdered() {
      List<string> Result = new List<string>();
      int i = 0;
      TRepeatAction Repeat = new TRepeatAction();
      Repeat.ToDo = () => {
        Result.Add(i++.ToString());
      };
      Repeat.Delay = 20;

      Repeat.Start();
      Thread.Sleep(250);
      Repeat.Cancel();

      TestContext.WriteLine(string.Join(",", Result.Select(x => x.ToString())));
      Assert.AreEqual(i, Result.Count());
    }

    [TestMethod]
    public void Repeat_Async_ExecIsNotOrdered() {
      List<string> Result = new List<string>();
      TRepeatAction Repeat = new TRepeatAction();
      int i = 0;
      Repeat.ToDo = async () => {
        Result.Add((await GetValue().ConfigureAwait(false)).ToString());
        i++;
      };
      Repeat.Delay = 20;

      Repeat.Start();
      Thread.Sleep(250);
      Repeat.Cancel();

      TestContext.WriteLine(string.Join(",", Result.Select(x => x.ToString())));
      Assert.AreEqual(i, Result.Count());
    }

    private int AsyncI = 0;
    private async Task<int> GetValue() {
      Random R = new Random(DateTime.Now.Millisecond);
      await Task.Delay(R.Next(5, 50)).ConfigureAwait(false);
      return AsyncI++;
    }
  }
}
