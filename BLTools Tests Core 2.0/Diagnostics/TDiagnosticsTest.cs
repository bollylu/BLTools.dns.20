using System;
using System.Collections.Generic;
using System.Text;

using BLTools.Debugging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Diagnostics {
  [TestClass()]
  public class TDiagnosticsTest {

    [TestMethod]
    public void StartProg_DisplayInfo() {
      string Target = ApplicationInfo.BuildStartupInfo();
      Console.WriteLine(Target);
      Assert.IsTrue(Target.Contains("BLTools"));
    }
  }
}
