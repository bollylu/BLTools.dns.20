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
  public class JsonObjectTest {

    #region --- Constants --------------------------------------------
    private const string TEST_STRING_NAME = "StringField";
    private const string TEST_STRING = "TestContent";
    private const string TEST_STRING_JSON_VALUE = "\"TestContent\"";
    private const string TEST_STRING_JSON_PAIR = "\"StringField\":\"TestContent\"";
    private const string TEST_STRING_JSON_OBJECT = "{\"StringField\":\"TestContent\"}";
    private const string DEFAULT_STRING = "(default)";

    private const string TEST_INT_NAME = "IntField";
    private const int TEST_INT = 98765;
    private const string TEST_INT_JSON_VALUE = "98765";
    private const string TEST_INT_JSON_PAIR = "\"IntField\":98765";
    private const string TEST_INT_JSON_OBJECT = "{\"IntField\":98765}";
    private const int DEFAULT_INT = -1;

    private const string TEST_FLOAT_NAME = "FloatField";
    private const float TEST_FLOAT = 123.456f;
    private const string TEST_FLOAT_JSON = "123.456";
    private const float DEFAULT_FLOAT = -1.0f;

    private const string TEST_DOUBLE_NAME = "DoubleField";
    private const double TEST_DOUBLE = 123.456789d;
    private const string TEST_DOUBLE_JSON = "123.456789";
    private const double DEFAULT_DOUBLE = -1.0d;

    private const string TEST_DATETIME_NAME = "DateTimeField";
    private static DateTime TEST_DATETIME = DateTime.Today;
    private static string TEST_DATETIME_JSON = $"\"{DateTime.Today.ToString()}\"";
    private static DateTime DEFAULT_DATETIME = DateTime.MinValue;

    private const string TEST_BOOL_NAME = "BoolField";
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
    public void CreateJsonObject_OneString_DataOk() {
      JsonString Source = new JsonString(TEST_STRING);
      JsonPair<JsonString> PairSource = new JsonPair<JsonString>(TEST_STRING_NAME, Source);
      JsonObject Actual = new JsonObject(PairSource);
      Assert.IsNotNull(Actual.KeyValuePairs);
      Assert.AreEqual(1, Actual.KeyValuePairs.Count);
      Assert.AreEqual(TEST_STRING_JSON_OBJECT, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonObject_OneStringDirect_DataOk() {
      JsonPair<JsonString> PairSource = new JsonPair<JsonString>(TEST_STRING_NAME, TEST_STRING);
      JsonObject Actual = new JsonObject(PairSource);
      Assert.IsNotNull(Actual.KeyValuePairs);
      Assert.AreEqual(1, Actual.KeyValuePairs.Count);
      Assert.AreEqual(TEST_STRING_JSON_OBJECT, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonObject_TwoStrings_DataOk() {
      JsonPair<JsonString> PairSource1 = new JsonPair<JsonString>(TEST_STRING_NAME, TEST_STRING);
      JsonPair<JsonString> PairSource2 = new JsonPair<JsonString>(TEST_STRING_NAME, $"+++{TEST_STRING}+++");
      JsonObject Actual = new JsonObject(PairSource1);
      Actual.AddItem(PairSource2);
      Assert.IsNotNull(Actual.KeyValuePairs);
      Assert.AreEqual(2, Actual.KeyValuePairs.Count);
      Assert.AreEqual("{\"StringField\":\"TestContent\",\"StringField\":\"+++TestContent+++\"}", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonObject_OneStringOneInt_DataOk() {
      JsonPair<JsonString> PairSource1 = new JsonPair<JsonString>(TEST_STRING_NAME, TEST_STRING);
      JsonPair<JsonInt> PairSource2 = new JsonPair<JsonInt>(TEST_INT_NAME, TEST_INT);
      JsonObject Actual = new JsonObject(PairSource1);
      Actual.AddItem(PairSource2);
      Assert.IsNotNull(Actual.KeyValuePairs);
      Assert.AreEqual(2, Actual.KeyValuePairs.Count);
      Assert.AreEqual($"{{{TEST_STRING_JSON_PAIR},{TEST_INT_JSON_PAIR}}}", Actual.RenderAsString());
    }






  }
}
