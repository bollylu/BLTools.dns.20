using BLTools;
using BLTools.Json;
using BLTools.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BLTools.UnitTest.Json {

  /// <summary>
  ///This is a test class for ArgElementTest and is intended
  ///to contain all ArgElementTest Unit Tests
  ///</summary>
  [TestClass()]
  public class JsonArrayTest {

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

    private const float TEST_FLOAT = 123.456f;
    private const string TEST_FLOAT_JSON = "123.456";
    private const float DEFAULT_FLOAT = -1.0f;

    private const double TEST_DOUBLE = 123.456789d;
    private const string TEST_DOUBLE_JSON = "123.456789";
    private const double DEFAULT_DOUBLE = -1.0d;

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

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_EmptyConstructor_DefaultData() {
      JsonArray Actual = new JsonArray();
      Assert.AreEqual(0, Actual.Count());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_ConstructorParams_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray Actual = new JsonArray(Source1, Source2);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual($@"[{TEST_STRING_JSON},{TEST_INT_JSON}]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_ConstructorEmptyAdd_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray Actual = new JsonArray();
      Actual.Add(Source1);
      Actual.Add(Source2);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual($@"[{TEST_STRING_JSON},{TEST_INT_JSON}]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_ConstructorEmptyAddTwoStrings_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonString Source2 = new JsonString(TEST_STRING);
      JsonArray Actual = new JsonArray();
      Actual.Add(Source1);
      Actual.Add(Source2);
      Assert.AreEqual(2, Actual.Count());
      Assert.AreEqual($"[{TEST_STRING_JSON},{TEST_STRING_JSON}]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_ConstructorEmptyAddArrayInside_DataOk() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray SourceArray = new JsonArray(Source1, Source2);
      JsonArray Actual = new JsonArray(Source1, Source2, SourceArray);
      Assert.AreEqual(3, Actual.Count());
      Assert.AreEqual($"[{TEST_STRING_JSON},{TEST_INT_JSON},[{TEST_STRING_JSON},{TEST_INT_JSON}]]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_AddMultipleArray_DataOk() {
      JsonObject Source1 = new JsonObject(new JsonPair(TEST_STRING_NAME, TEST_STRING));
      JsonObject Source2 = new JsonObject(new JsonPair(TEST_INT_NAME, TEST_INT));
      JsonObject Source3 = new JsonObject(new JsonPair(TEST_BOOL_NAME, TEST_BOOL));
      JsonArray SourceArray1 = new JsonArray(Source1, Source2);
      JsonArray SourceArray2 = new JsonArray(Source1, Source2);
      JsonArray SourceArray3 = new JsonArray(Source1, Source2, Source3);
      JsonArray Actual = new JsonArray(SourceArray1, SourceArray2, SourceArray3);
      Assert.AreEqual(3, Actual.Count());
      TestContext.WriteLine(Actual.RenderAsString());
      Assert.AreEqual($@"[[{{""{TEST_STRING_NAME}"":{TEST_STRING_JSON}}},{{""{TEST_INT_NAME}"":{TEST_INT_JSON}}}],[{{""{TEST_STRING_NAME}"":{TEST_STRING_JSON}}},{{""{TEST_INT_NAME}"":{TEST_INT_JSON}}}],[{{""{TEST_STRING_NAME}"":{TEST_STRING_JSON}}},{{""{TEST_INT_NAME}"":{TEST_INT_JSON}}},{{""{TEST_BOOL_NAME}"":{TEST_BOOL_JSON}}}]]", Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array")]
    public void CreateJsonArray_AddArrayWithinArray_DataOk() {
      JsonObject Source1 = new JsonObject(new JsonPair(TEST_STRING_NAME, TEST_STRING));
      JsonObject Source2 = new JsonObject(new JsonPair(TEST_INT_NAME, TEST_INT));
      JsonArray SourceArray1 = new JsonArray(Source1, Source2);
      JsonArray SourceArray2 = new JsonArray(Source2, Source1);
      JsonArray SourceArray3 = new JsonArray(SourceArray1, SourceArray2);
      JsonArray Actual = new JsonArray(SourceArray1, SourceArray2, SourceArray3);
      Assert.AreEqual(3, Actual.Count());
      Debug.WriteLine(Actual.RenderAsString());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.RenderAsStringFormatted")]
    public void CreateJsonArray_SingleArray_OutputIsFormatted() {
      JsonString Source1 = new JsonString(TEST_STRING);
      JsonInt Source2 = new JsonInt(TEST_INT);
      JsonArray Actual = new JsonArray(Source1, Source2);
      Assert.AreEqual(2, Actual.Count());
      Trace.WriteLine(TextBox.BuildDynamic(Actual.RenderAsString(true), 0, TextBox.EStringAlignment.Left));
      Assert.AreEqual($"[{CRLF}  {TEST_STRING_JSON},{CRLF}  {TEST_INT_JSON}{CRLF}]", Actual.RenderAsString(true));
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.RenderAsStringFormatted")]
    public void CreateJsonArray_AddArrayWithinArray_OutputIsFormatted() {
      JsonObject Source1 = new JsonObject(new JsonPair(TEST_STRING_NAME, TEST_STRING));
      JsonObject Source2 = new JsonObject(new JsonPair(TEST_INT_NAME, TEST_INT));
      JsonArray SourceArray1 = new JsonArray(Source1, Source2);
      JsonArray SourceArray2 = new JsonArray(Source2, Source1);
      JsonArray SourceArray3 = new JsonArray(SourceArray1, SourceArray2);
      JsonArray Actual = new JsonArray(SourceArray1, SourceArray2, SourceArray3);
      Trace.WriteLine(TextBox.BuildDynamic(Actual.RenderAsString(true), 0, TextBox.EStringAlignment.Left));

      Assert.AreEqual(3, Actual.Count());
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.RenderAsStringFormatted")]
    public void CreateJsonArray_AddMultipleArray_OutputIsFormatted() {
      JsonObject Source1 = new JsonObject(new JsonPair(TEST_STRING_NAME, TEST_STRING));
      JsonObject Source2 = new JsonObject(new JsonPair(TEST_INT_NAME, TEST_INT));
      JsonObject Source3 = new JsonObject(new JsonPair(TEST_BOOL_NAME, TEST_BOOL));
      JsonArray SourceArray1 = new JsonArray(Source1, Source2);
      JsonArray SourceArray2 = new JsonArray(Source1, Source2);
      JsonArray SourceArray3 = new JsonArray(Source1, Source2, Source3);
      JsonArray Actual = new JsonArray(SourceArray1, SourceArray2, SourceArray3);
      Assert.AreEqual(3, Actual.Count());
      Debug.WriteLine(Actual.RenderAsString(true));
      Assert.AreEqual($"[{CRLF}  [{CRLF}    {{{CRLF}      \"{TEST_STRING_NAME}\" : {TEST_STRING_JSON}{CRLF}    }},{CRLF}    {{{CRLF}      \"{TEST_INT_NAME}\" : {TEST_INT_JSON}{CRLF}    }}{CRLF}  ],{CRLF}  [{CRLF}    {{{CRLF}      \"{TEST_STRING_NAME}\" : {TEST_STRING_JSON}{CRLF}    }},{CRLF}    {{{CRLF}      \"{TEST_INT_NAME}\" : {TEST_INT_JSON}{CRLF}    }}{CRLF}  ],{CRLF}  [{CRLF}    {{{CRLF}      \"{TEST_STRING_NAME}\" : {TEST_STRING_JSON}{CRLF}    }},{CRLF}    {{{CRLF}      \"{TEST_INT_NAME}\" : {TEST_INT_JSON}{CRLF}    }},{CRLF}    {{{CRLF}      \"{TEST_BOOL_NAME}\" : {TEST_BOOL_JSON}{CRLF}    }}{CRLF}  ]{CRLF}]", Actual.RenderAsString(true));
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.ImplicitConversion")]
    public void JsonArrayEmpty_ConvertToArray_ConvertedOk() {
      JsonArray Source = JsonArray.Empty;
      IJsonValue[] Actual = Source;
      Assert.IsNotNull(Actual);
      Assert.AreEqual(0, Actual.Length);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.ImplicitConversion")]
    public void JsonArrayWithContent_ConvertToArray_ConvertedOk() {
      JsonArray Source = new JsonArray();
      JsonString SourceString = new JsonString(TEST_STRING);
      JsonInt SourceInt = new JsonInt(TEST_INT);
      Source.Add(SourceString, SourceInt);
      IJsonValue[] Actual = Source;
      Assert.IsNotNull(Actual);
      Assert.AreEqual(2, Actual.Length);
      Assert.IsInstanceOfType(Actual[0], typeof(JsonString));
      Assert.IsInstanceOfType(Actual[1], typeof(JsonInt));
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.ImplicitConversion")]
    public void JsonArrayEmpty_ConvertToList_ConvertedOk() {
      JsonArray Source = JsonArray.Empty;
      List<IJsonValue> Actual = Source;
      Assert.IsNotNull(Actual);
      Assert.AreEqual(0, Actual.Count);
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.ImplicitConversion")]
    public void JsonArrayWithContent_ConvertToList_ConvertedOk() {
      JsonArray Source = new JsonArray();
      JsonString SourceString = new JsonString(TEST_STRING);
      JsonInt SourceInt = new JsonInt(TEST_INT);
      Source.Add(SourceString, SourceInt);
      List<IJsonValue> Actual = Source;
      Assert.IsNotNull(Actual);
      Assert.AreEqual(2, Actual.Count);
      Assert.IsInstanceOfType(Actual.First(), typeof(JsonString));
      Assert.IsInstanceOfType(Actual.Last(), typeof(JsonInt));
    }

    [TestMethod(), TestCategory("Json"), TestCategory("Json.Array.Linq")]
    public void JsonArrayWithContent_Linq_LinqOk() {
      JsonArray Actual = new JsonArray();
      JsonString SourceString = new JsonString(TEST_STRING);
      JsonInt SourceInt = new JsonInt(TEST_INT);
      Actual.Add(SourceString, SourceInt);
      Assert.IsInstanceOfType(Actual.First(), typeof(JsonString));
      Assert.IsInstanceOfType(Actual.Last(), typeof(JsonInt));
      Assert.AreEqual(1, Actual.OfType<JsonString>().Count());
    }
  }
}
