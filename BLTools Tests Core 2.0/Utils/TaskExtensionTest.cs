using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Utils {

  [TestClass]
  public class TaskExtensionTest {

    [TestMethod]
    [ExpectedException(typeof(OperationCanceledException))]
    public async Task TaskWithCancellation_TransformIntoTimeoutTask_TaskIsCancelled() {

      using (UdpClient Client = new UdpClient(12345)) {
        var Result = await Client.ReceiveAsync().WithTimeout(100).ConfigureAwait(false);
        Assert.IsNull(Result);
      }
    }

    [TestMethod]
    [ExpectedException(typeof(OperationCanceledException))]
    public async Task TaskWithCancellation_TransformIntoCancellableTask_TaskIsCancelled() {

      CancellationTokenSource CTS = new CancellationTokenSource();

      using (UdpClient Client = new UdpClient(12345)) {
        Task WaitBeforeCancel = new Task(async () => {
          await Task.Delay(100);
          CTS.Cancel();
        });
        WaitBeforeCancel.Start();
        var Result = await Client.ReceiveAsync().WithCancellation(CTS.Token);
        Assert.IsNull(Result);
      }
    }
  }
}
