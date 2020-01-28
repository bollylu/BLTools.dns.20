using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.UnitTest.Core20.Extensions {
  [TestClass()]
  public class DictionaryExtensionTest {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    private static Dictionary<string, string> SourceDictStringString;
    private static Dictionary<string, string> EmptySourceDict;
    private static Dictionary<string, int> SourceDictStringInt;
    private static Dictionary<string, bool> SourceDictStringBool;
    private static Dictionary<int, string> SourceDictIntString;

    private const string KEY_STRING1 = "Key1";
    private const string KEY_STRING2 = "Key2";
    private const string KEY_STRING3 = "Key3";

    private const string VALUE_STRING1 = "Value1";
    private const string VALUE_STRING2 = "Value2";

    private const int VALUE_INT1 = 36;
    private const int VALUE_INT2 = 72;

    private const bool VALUE_BOOL1 = true;
    private const bool VALUE_BOOL2 = false;

    private const int KEY_INT1 = 30;
    private const int KEY_INT2 = 40;
    private const int KEY_INT3 = 60;

    private const string DEFAULT_STRING = "(default)";
    private const int DEFAULT_INT = -1;
    private const bool DEFAULT_BOOL = false;

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    [ClassInitialize()]
    public static void MyClassInitialize(TestContext testContext) {
      EmptySourceDict = new Dictionary<string, string>();

      SourceDictStringString = new Dictionary<string, string>();
      SourceDictStringString.Add(KEY_STRING1, VALUE_STRING1);
      SourceDictStringString.Add(KEY_STRING2, VALUE_STRING2);

      SourceDictStringInt = new Dictionary<string, int>();
      SourceDictStringInt.Add(KEY_STRING1, VALUE_INT1);
      SourceDictStringInt.Add(KEY_STRING2, VALUE_INT2);

      SourceDictStringBool = new Dictionary<string, bool>();
      SourceDictStringBool.Add(KEY_STRING1, VALUE_BOOL1);
      SourceDictStringBool.Add(KEY_STRING2, VALUE_BOOL2);

      SourceDictIntString = new Dictionary<int, string>();
      SourceDictIntString.Add(KEY_INT1, VALUE_STRING1);
      SourceDictIntString.Add(KEY_INT2, VALUE_STRING2);
    }

    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringString_GetStringKeyOk_ValueOk() {
      string Actual = SourceDictStringString.SafeGetValue(KEY_STRING1, DEFAULT_STRING);
      Assert.AreEqual(Actual, VALUE_STRING1);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringString_GetStringKeyInvalid_ValueDefault() {
      string Actual = SourceDictStringString.SafeGetValue(KEY_STRING3, DEFAULT_STRING);
      Assert.AreEqual(Actual, DEFAULT_STRING);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringString_GetStringKeyNull_ValueDefault() {
      string LookupKey = null;
      string Actual = SourceDictStringString.SafeGetValue(LookupKey, DEFAULT_STRING);
      Assert.AreEqual(Actual, DEFAULT_STRING);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringString_EmptyDictionary_ValueDefault() {
      string Actual = EmptySourceDict.SafeGetValue(KEY_STRING3, DEFAULT_STRING);
      Assert.AreEqual(Actual, DEFAULT_STRING);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringInt_GetIntKeyOk_ValueOk() {
      int Actual = SourceDictStringInt.SafeGetValue(KEY_STRING2, DEFAULT_INT);
      Assert.AreEqual(Actual, VALUE_INT2);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringInt_GetIntKeyInvalid_ValueDefault() {
      int Actual = SourceDictStringInt.SafeGetValue(KEY_STRING3, DEFAULT_INT);
      Assert.AreEqual(Actual, DEFAULT_INT);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringBool_GetBoolKeyOk_ValueOk() {
      bool Actual = SourceDictStringBool.SafeGetValue(KEY_STRING2, DEFAULT_BOOL);
      Assert.AreEqual(Actual, VALUE_BOOL2);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictStringBool_GetBoolKeyInvalid_ValueDefault() {
      bool Actual = SourceDictStringInt.SafeGetValue(KEY_STRING3, DEFAULT_BOOL);
      Assert.AreEqual(Actual, DEFAULT_BOOL);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictIntString_GetStringKeyOk_ValueOk() {
      string Actual = SourceDictIntString.SafeGetValue(KEY_INT2, DEFAULT_STRING);
      Assert.AreEqual(Actual, VALUE_STRING2);
    }

    [TestMethod(), TestCategory("Dictionary")]
    public void DictIntString_GetStringKeyInvalid_ValueDefault() {
      string Actual = SourceDictStringInt.SafeGetValue(KEY_INT3, DEFAULT_STRING);
      Assert.AreEqual(Actual, DEFAULT_STRING);
    }

  }

}
