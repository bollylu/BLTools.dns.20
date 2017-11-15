using BLTools;
using BLTools.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
    private static string TEST_STRING_JSON_VALUE = $@"""{TEST_STRING}""";
    private static string TEST_STRING_JSON_PAIR = $@"""{TEST_STRING_NAME}"":{TEST_STRING_JSON_VALUE}";
    private static string TEST_STRING_JSON_OBJECT = $@"{{{TEST_STRING_JSON_PAIR}}}";
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

    private static string CRLF = Environment.NewLine;
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

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object")]
    public void CreateJsonObject_OneString_DataOk() {
      JsonString Source = new JsonString(TEST_STRING);
      JsonPair PairSource = new JsonPair(TEST_STRING_NAME, Source);
      JsonObject Actual = new JsonObject(PairSource);
      Assert.AreEqual(1, Actual.Count());
      Assert.AreEqual(TEST_STRING_JSON_OBJECT, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object")]
    public void CreateJsonObject_OneStringDirect_DataOk() {
      JsonPair PairSource = new JsonPair(TEST_STRING_NAME, TEST_STRING);
      JsonObject Actual = new JsonObject(PairSource);
      Assert.AreEqual(1, Actual.Count());
      Assert.AreEqual(TEST_STRING_JSON_OBJECT, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object")]
    public void CreateJsonObject_TwoStrings_DataOk() {
      JsonPair PairSource1 = new JsonPair(TEST_STRING_NAME, TEST_STRING);
      JsonPair PairSource2 = new JsonPair(TEST_STRING_NAME, $"+++{TEST_STRING}+++");
      JsonObject Actual = new JsonObject(PairSource1);
      Actual.Add(PairSource2);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual("{\"StringField\":\"TestContent\",\"StringField\":\"+++TestContent+++\"}", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object")]
    public void CreateJsonObject_OneStringOneInt_DataOk() {
      JsonPair PairSource1 = new JsonPair(TEST_STRING_NAME, TEST_STRING);
      JsonPair PairSource2 = new JsonPair(TEST_INT_NAME, TEST_INT);
      JsonObject Actual = new JsonObject(PairSource1);
      Actual.Add(PairSource2);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual($"{{{TEST_STRING_JSON_PAIR},{TEST_INT_JSON_PAIR}}}", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object.Parse")]
    public void ParseJsonObject_StringString_ValueOk() {
      string Source = "{\"Identifier\":\"Data\"}";

      JsonObject Actual = JsonObject.Parse(Source);
      Assert.AreEqual(1, Actual.Count());
      Assert.AreEqual("Identifier", Actual[0].Key);
      Assert.IsInstanceOfType(Actual[0].Content, typeof(JsonString));
      Assert.AreEqual("Data", Actual[0].StringContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object.Parse")]
    public void ParseJsonObject_MultiplePairs_ValueOk() {
      string Source = "{\"Identifier\":\"Data\",\"numeric\":3.14}";

      JsonObject Actual = JsonObject.Parse(Source);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual("Identifier", Actual[0].Key);
      Assert.AreEqual("numeric", Actual[1].Key);
      Assert.IsInstanceOfType(Actual[0].Content, typeof(JsonString));
      Assert.IsInstanceOfType(Actual[1].Content, typeof(JsonDouble));
      Assert.AreEqual(3.14d, Actual[1].DoubleContent.Value);
      Assert.AreEqual(Source, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object.RenderAsStringFormatted")]
    public void CreateJsonObject_MultiplePairs_OutputIsFormatted() {
      string Source = $"{{{CRLF}  \"Identifier\" : \"Data\",{CRLF}  \"numeric\" : 3.14{CRLF}}}";

      JsonObject Actual = JsonObject.Parse(Source);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual(Source, Actual.RenderAsString(true));
      Debug.WriteLine(Actual.RenderAsString(true));
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.Object")]
    public void RenderAsBytes_OneString_DataOk() {
      JsonString Source = new JsonString(TEST_STRING);
      JsonPair PairSource = new JsonPair(TEST_STRING_NAME, Source);
      JsonObject Actual = new JsonObject(PairSource);
      Assert.AreEqual(1, Actual.Count());
      byte[] SourceAsBytes = TEST_STRING_JSON_OBJECT.ToByteArray();
      byte[] ActualAsBytes = Actual.RenderAsBytes();
      Assert.AreEqual(Encoding.UTF8.GetBytes(TEST_STRING_JSON_OBJECT).Length, ActualAsBytes.Length);
      for ( int i = 0; i < ActualAsBytes.Length; i++ ) {
        Assert.AreEqual(SourceAsBytes[i], ActualAsBytes[i]);
      }
    }
  }
}
