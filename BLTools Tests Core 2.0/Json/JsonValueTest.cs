using BLTools;
using BLTools.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BLTools.UnitTest.Core20.Json {


  /// <summary>
  ///This is a test class for ArgElementTest and is intended
  ///to contain all ArgElementTest Unit Tests
  ///</summary>
  [TestClass()]
  public class JsonValueTest {

    #region --- Constants --------------------------------------------
    private const string TEST_STRING = "TestContent";
    private const string TEST_STRING_JSON = "\"TestContent\"";
    private const string DEFAULT_STRING = "(default)";

    private const int TEST_INT = 98765;
    private const string TEST_INT_JSON = "98765";
    private const int DEFAULT_INT = -1;

    private const float TEST_FLOAT = 123.456f;
    private const string TEST_FLOAT_JSON = "123.456";
    private const float DEFAULT_FLOAT = -1.0f;

    private const double TEST_DOUBLE = 123.456789d;
    private const string TEST_DOUBLE_JSON = "123.456789";
    private const double DEFAULT_DOUBLE = -1.0d;

    private static DateTime TEST_DATETIME = DateTime.Today;
    private static string TEST_DATETIME_JSON = $"\"{DateTime.Today.ToString()}\"";
    private static DateTime DEFAULT_DATETIME = DateTime.MinValue;

    private const bool TEST_BOOL = true;
    private static string TEST_BOOL_JSON = "true";
    private const bool DEFAULT_BOOL = false;
    #endregion --- Constants --------------------------------------------

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

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
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


    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_EmptyString_ValueOk() {
      JsonValue Actual = new JsonValue("");
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual("", Actual.Content);
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_NonEmptyString_ValueOk() {
      JsonValue Actual = new JsonValue(TEST_STRING);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_STRING, Actual.Content);
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_Int_ValueOk() {
      JsonValue Actual = new JsonValue(TEST_INT);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_INT, Actual.Content);
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_Float_ValueOk() {
      JsonValue Actual = new JsonValue(TEST_FLOAT);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_FLOAT, Actual.Content);
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_Double_ValueOk() {
      JsonValue Actual = new JsonValue(TEST_DOUBLE);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_DOUBLE, Actual.Content);
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_String_ConvertToJsonOk() {
      JsonValue Actual = new JsonValue(TEST_STRING);
      Assert.AreEqual(TEST_STRING_JSON, Actual.JsonValueString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_Int_ConvertToJsonOk() {
      JsonValue Actual = new JsonValue(TEST_INT);
      Assert.AreEqual(TEST_INT_JSON, Actual.JsonValueString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_Bool_ConvertToJsonOk() {
      JsonValue Actual = new JsonValue(TEST_BOOL);
      Assert.AreEqual(TEST_BOOL_JSON, Actual.JsonValueString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonValue_DateTime_ConvertToJsonOk() {
      JsonValue Actual = new JsonValue(TEST_DATETIME);
      Assert.AreEqual(TEST_DATETIME_JSON, Actual.JsonValueString());
    }

  }
}
