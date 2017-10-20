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
  public class JsonArrayTest {

    #region --- Constants --------------------------------------------
    private const string TEST_STRING_NAME = "StringField";
    private const string TEST_STRING = "TestContent";
    private const string TEST_STRING_JSON = "\"TestContent\"";
    private const string TEST_STRING_JSON_OBJECT = "{\"TestContent\"}";
    private const string DEFAULT_STRING = "(default)";

    private const string TEST_INT_NAME = "StringField";
    private const int TEST_INT = 98765;
    private const string TEST_INT_JSON = "98765";
    private const string TEST_INT_JSON_OBJECT = "{98765}";
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
    public void CreateJsonArray_EmptyConstructor_DefaultData() {
      JsonArray Actual = new JsonArray();
      Assert.AreEqual(0, Actual.Items.Count);
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonArray_ConstructorParams_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray Actual = new JsonArray(Source1, Source2);
      Assert.AreEqual(2, Actual.Items.Count);
      Assert.AreEqual($"[\"{TEST_STRING}\",{TEST_INT_JSON}]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonArray_ConstructorEmptyAdd_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray Actual = new JsonArray();
      Actual.Items.Add(Source1);
      Actual.Items.Add(Source2);
      Assert.AreEqual(2, Actual.Items.Count);
      Assert.AreEqual($"[\"{TEST_STRING}\",{TEST_INT_JSON}]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("NC20.Json")]
    public void CreateJsonArray_ConstructorEmptyAddArrayInside_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray Actual = new JsonArray();
      Actual.Items.Add(Source1);
      Actual.Items.Add(Source2);
      JsonArray Source3 = new JsonArray(Actual);
      Actual.Items.Add(Source3);
      Assert.AreEqual(3, Actual.Items.Count);
      Assert.AreEqual($"[\"{TEST_STRING}\",{TEST_INT_JSON},[\"{TEST_STRING}\",{TEST_INT_JSON}]]", Actual.RenderAsString());
    }

    //#region --- Int --------------------------------------------
    //[TestMethod(), TestCategory("NC20.Json")]
    //public void CreateJsonArray_AddMultipleArray_DataOk() {
    //  JsonObject Source1 = new JsonObject(TEST_STRING_NAME, TEST_STRING);
    //  JsonObject Source2 = new JsonObject(TEST_STRING_NAME, TEST_INT);
    //  JsonObject Source3 = new JsonObject("TestBool", TEST_BOOL);
    //  JsonArray SourceArray1 = new JsonArray(Source1, Source2);
    //  JsonArray SourceArray2 = new JsonArray(Source1, Source2);
    //  JsonArray SourceArray3 = new JsonArray(Source1, Source2, Source3);
    //  JsonArray SourceArray4 = new JsonArray(SourceArray1, SourceArray2, SourceArray3);
    //  JsonObject Actual = new JsonObject("Items", SourceArray4);
    //  Assert.IsNotNull(Actual.JsonKey);
    //  Assert.AreEqual("Items", Actual.JsonKey);
    //  Assert.IsNotNull(Actual.JsonContent);
    //  Assert.AreEqual($"{{\"Items\":[\"{TEST_STRING_NAME}\":{TEST_STRING_JSON},\"{TEST_INT_NAME}\":{TEST_INT_JSON}]}}", Actual.ToJsonObjectString());
    //}
    //#endregion --- Int --------------------------------------------


  }
}
