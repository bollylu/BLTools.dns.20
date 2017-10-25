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
  public class JsonPairTest {

    #region --- Constants --------------------------------------------
    private const string TEST_STRING_NAME = "StringField";
    private const string TEST_STRING = "TestContent";
    private const string TEST_STRING_JSON = "\"TestContent\"";
    private const string TEST_STRING_JSON_OBJECT = "{\"TestContent\"}";
    private const string DEFAULT_STRING = "(default)";

    private const string TEST_INT_NAME = "IntField";
    private const int TEST_INT = 98765;
    private const string TEST_INT_JSON = "98765";
    private const string TEST_INT_JSON_OBJECT = "{98765}";
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
    private static string TEST_DATETIME_JSON = $"\"{DateTime.Today.ToString("s")}\"";
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

    #region --- Tests for constructors --------------------------------------------
    [TestMethod(), TestCategory("NC20.Json.Pair")]
    public void CreateJsonPair_String_ValueOk() {
      JsonPair Actual = new JsonPair(TEST_STRING_NAME, new JsonString(TEST_STRING));
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_STRING_NAME, Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonString));
      Assert.AreEqual(TEST_STRING, Actual.StringContent.Value);
      Assert.AreEqual(TEST_STRING_JSON, Actual.Content.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair")]
    public void CreateJsonPair_Int_ValueOk() {
      JsonPair Actual = new JsonPair(TEST_INT_NAME, new JsonInt(TEST_INT));
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_INT_NAME, Actual.Key);
      Assert.AreEqual(TEST_INT, Actual.IntContent.Value);
      Assert.AreEqual(TEST_INT_JSON, Actual.Content.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair")]
    public void CreateJsonPair_DirectString_ValueOk() {
      JsonPair Actual = new JsonPair(TEST_STRING_NAME, TEST_STRING);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_STRING_NAME, Actual.Key);
      Assert.AreEqual(TEST_STRING, Actual.StringContent.Value);
      Assert.AreEqual(TEST_STRING_JSON, Actual.Content.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair")]
    public void CreateJsonPair_DirectInt_ValueOk() {
      JsonPair Actual = new JsonPair(TEST_INT_NAME, TEST_INT);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual(TEST_INT_NAME, Actual.Key);
      Assert.AreEqual(TEST_INT, Actual.IntContent.Value);
      Assert.AreEqual(TEST_INT_JSON, Actual.Content.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair")]
    public void CreateJsonPair_JsonArray_ValueOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray SourceArray = new JsonArray(Source1, Source2);

      JsonPair Actual = new JsonPair("TestArray", SourceArray);
      Assert.IsNotNull(Actual.Content);
      Assert.AreEqual("TestArray", Actual.Key);
      Assert.AreEqual(2, Actual.ArrayContent.Items.Count);
      Assert.AreEqual($"\"TestArray\":[\"{TEST_STRING}\",{TEST_INT_JSON}]", Actual.RenderAsString());
    }
    #endregion --- Tests for constructors --------------------------------------------

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringString_ValueOk() {
      string Source = "\"Identifier\":\"Data\"";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonString));
      Assert.AreEqual("Data", Actual.StringContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringInt_ValueOk() {
      string Source = "\"Identifier\":123456";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonInt));
      Assert.AreEqual(123456, Actual.IntContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringLong_ValueOk() {
      string Source = "\"Identifier\":123456789123";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonLong));
      Assert.AreEqual(123456789123L, Actual.LongContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringFloat_ValueOkButDouble() {
      string Source = "\"Identifier\":123456.789123";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonDouble));
      Assert.AreEqual(123456.789123d, Actual.DoubleContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringDouble_ValueOk() {
      string Source = "\"Identifier\":123456.789123456789";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonDouble));
      Assert.AreEqual(123456.789123456789d, Actual.DoubleContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringBool_ValueOk() {
      string Source = "\"Identifier\":true";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonBool));
      Assert.AreEqual(true, Actual.BoolContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringBoolWithInnerSpaces_ValueOk() {
      string Source = "\"Identifier\" : true";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonBool));
      Assert.AreEqual(true, Actual.BoolContent.Value);
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayOfBool_ValueOk() {
      string Source = "\"Identifier\":[true,false]";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayOfString_ValueOk() {
      string Source = "\"Identifier\":[\"Blue\",\"Red\"]";
      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayOfInt_ValueOk() {
      string Source = "\"Identifier\":[12,36,48]";
      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayOfFloat_ValueOk() {
      string Source = "\"Identifier\":[12.98,36.78,48.21]";
      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayOfMulti_ValueOk() {
      string Source = "\"Identifier\":[12.98,\"toto\",48,true]";
      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[0], typeof(JsonDouble));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[1], typeof(JsonString));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[2], typeof(JsonInt));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[3], typeof(JsonBool));
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayOfMultiLevels_ValueOk() {
      string Source = "\"Identifier\":[12.98,\"toto\",48,true,[\"second level 1\",\"second level 2\"]]";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[0], typeof(JsonDouble));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[1], typeof(JsonString));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[2], typeof(JsonInt));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[3], typeof(JsonBool));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[4], typeof(JsonArray));
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_StringArrayWithReservedCharsInString_ValueOk() {
      string Source = "\"Identifier\":[12.98,\"toto\",48,true,\"[\\\"second level, no 1\\\",\\\"second level : 2\\\"]\"]";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual("Identifier", Actual.Key);
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonArray));
      Assert.AreEqual(Source, Actual.RenderAsString());
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[0], typeof(JsonDouble));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[1], typeof(JsonString));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[2], typeof(JsonInt));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[3], typeof(JsonBool));
      Assert.IsInstanceOfType(Actual.ArrayContent.Items[4], typeof(JsonString));
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_ErrorInSourceNoDefaultValue_GetDefaultDefaultValue() {
      string Source = "\"Identifier:[12.98,\"toto\",48,true,\"[\\\"second level, no 1\\\",\\\"second level : 2\\\"]\"]";

      IJsonPair Actual = JsonPair.Parse(Source);
      Assert.AreEqual(JsonPair.Default.Key, Actual.Key);
      Assert.AreEqual(JsonPair.Default.Content.GetType(), Actual.Content.GetType());
      Assert.IsInstanceOfType(Actual.Content, typeof(JsonNull));
    }

    [TestMethod(), TestCategory("NC20.Json.Pair.Parse")]
    public void ParseJsonPair_ErrorInSourceDefaultValueString_GetDefaultValueString() {
      string Source = "\"Identifier:[12.98,\"toto\",48,true,\"[\\\"second level, no 1\\\",\\\"second level : 2\\\"]\"]";

      IJsonPair Actual = JsonPair.Parse(Source, new JsonPair("MyValue", "Error while reading"));
      Assert.AreEqual("MyValue", Actual.Key);
      Assert.AreEqual("Error while reading", Actual.StringContent.Value);
    }
  }
}
