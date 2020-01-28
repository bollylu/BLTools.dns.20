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
    private const string TEST_STRING_NAME = "StringField";
    private const string TEST_STRING = @"Test\Content";
    private const string TEST_STRING_JSON = @"""Test\\Content""";
    private const string TEST_STRING_JSON_OBJECT = @"{""Test\\Content""}";
    private const string DEFAULT_STRING = "(default)";

    private const string TEST_INT_NAME = "IntField";
    private const int TEST_INT = 98765;
    private const string TEST_INT_JSON = "98765";
    private const string TEST_INT_JSON_OBJECT = "{98765}";
    private const int DEFAULT_INT = -1;

    private const string TEST_LONG_NAME = "LongField";
    private const long TEST_LONG = 987654321987;
    private const string TEST_LONG_JSON = "987654321987";
    private const string TEST_LONG_JSON_OBJECT = "{987654321987}";
    private const int DEFAULT_LONG = -1;

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
    private static string TEST_DATETIME_JSON = $"\"{TEST_DATETIME.ToString("s")}\"";
    private static DateTime DEFAULT_DATETIME = DateTime.MinValue;

    private const string TEST_BOOL_NAME = "BoolField";
    private const bool TEST_BOOL = true;
    private static string TEST_BOOL_JSON = "true";
    private const bool DEFAULT_BOOL = false;
    #endregion --- Constants --------------------------------------------

    #region --- Tests support --------------------------------------------
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

    #endregion --- Tests support --------------------------------------------


    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_EmptyString_ValueOk() {
      JsonString Actual = new JsonString("");
      Assert.IsNotNull(Actual.Value);
      Assert.AreEqual("", Actual.Value);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_NonEmptyString_ValueOk() {
      JsonString Actual = new JsonString(TEST_STRING);
      Assert.IsNotNull(Actual.Value);
      Assert.AreEqual(TEST_STRING, Actual.Value);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_Int_ValueOk() {
      JsonInt Actual = new JsonInt(TEST_INT);
      Assert.IsNotNull(Actual.Value);
      Assert.AreEqual(TEST_INT, Actual.Value);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_Long_ValueOk() {
      JsonLong Actual = new JsonLong(TEST_LONG);
      Assert.IsNotNull(Actual.Value);
      Assert.AreEqual(TEST_LONG, Actual.Value);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_Float_ValueOk() {
      JsonFloat Actual = new JsonFloat(TEST_FLOAT);
      Assert.IsNotNull(Actual.Value);
      Assert.AreEqual(TEST_FLOAT, Actual.Value);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_Double_ValueOk() {
      JsonDouble Actual = new JsonDouble(TEST_DOUBLE);
      Assert.IsNotNull(Actual.Value);
      Assert.AreEqual(TEST_DOUBLE, Actual.Value);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_String_ConvertToJsonOk() {
      IJsonValue Actual = new JsonString(TEST_STRING);
      Assert.AreEqual(TEST_STRING_JSON, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_Int_ConvertToJsonOk() {
      IJsonValue Actual = new JsonInt(TEST_INT);
      Assert.AreEqual(TEST_INT_JSON, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_Bool_ConvertToJsonOk() {
      IJsonValue Actual = new JsonBool(TEST_BOOL);
      Assert.AreEqual(TEST_BOOL_JSON, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value")]
    public void CreateJsonValue_DateTime_ConvertToJsonOk() {
      IJsonValue Actual = new JsonDateTime(TEST_DATETIME);
      Assert.AreEqual(TEST_DATETIME_JSON, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value.Parse")]
    public void ParseJsonValue_String_JsonStringOk() {
      IJsonValue Actual = HelperJsonValue.Parse(TEST_STRING_JSON);
      Assert.IsInstanceOfType(Actual, typeof(JsonString));
      Assert.AreEqual(TEST_STRING_JSON, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Value.Parse")]
    public void ParseJsonValue_Int_JsonLongOk() {
      IJsonValue Actual = HelperJsonValue.Parse(TEST_INT_JSON);
      Assert.IsInstanceOfType(Actual, typeof(JsonInt));
      Assert.AreEqual(TEST_INT_JSON, Actual.RenderAsString());
    }
  }
}
