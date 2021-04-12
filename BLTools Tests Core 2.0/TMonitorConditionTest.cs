using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest {
  internal class TContainerClass {
    internal int Value;
  }

  [TestClass]
  public class TMonitorConditionTest {

    #region --- Test support --------------------------------------------
    private TestContext _TestContextInstance;
    public TestContext TestContext {
      get { return _TestContextInstance; }
      set { _TestContextInstance = value; }
    }
    #endregion --- Test support -----------------------------------------

    [TestMethod]
    public void CreateMonitorCondition_ConstructorEmpty_AllPropertiesInitialized() {
      using (TConditionMonitor<TContainerClass> Monitor = new TConditionMonitor<TContainerClass>()) {
        Assert.AreEqual(AConditionMonitor.DEFAULT_DELAY_IN_MS, Monitor.Delay);
        Assert.IsNull(Monitor.Condition);
        Assert.IsNull(Monitor.WhenCondition);
        Assert.AreEqual(string.Empty, Monitor.Name);
      }
    }

    [TestMethod]
    public void CreateMonitorCondition_ConstructorWithValues_AllPropertiesInitialized() {
      bool Done = false;
      using (TConditionMonitor<TContainerClass> Monitor = new TConditionMonitor<TContainerClass>("Test ContainerClass", x => x.Value == 10, x => Done = true)) {
        Assert.AreEqual(AConditionMonitor.DEFAULT_DELAY_IN_MS, Monitor.Delay);
        Assert.IsNotNull(Monitor.Condition);
        Assert.IsNotNull(Monitor.WhenCondition);
        Assert.AreEqual("Test ContainerClass", Monitor.Name);
        Assert.IsFalse(Done);
      }
    }

    [TestMethod]
    public void CreateMonitorCondition_MonitorClassValue_TriggerFires() {
      bool Done = false;
      TContainerClass ContainerClass = new TContainerClass() { Value = 0 };

      using (TConditionMonitor<TContainerClass> Monitor = new TConditionMonitor<TContainerClass>("Test ContainerClass", x => x.Value == 10, x => Done = true)) {
        Monitor.Start(ContainerClass);
        while (ContainerClass.Value < 10) {
          Thread.Sleep(10);
          Assert.IsFalse(Done);
          ContainerClass.Value++;
        }
        Thread.Sleep(100);
        Assert.IsTrue(Done);
      }
    }
  }
}
