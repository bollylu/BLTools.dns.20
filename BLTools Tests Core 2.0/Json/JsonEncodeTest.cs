using BLTools;
using BLTools.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BLTools.UnitTest.Core20.Json {


  [TestClass()]
  public class JsonEncodeTest {

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


    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_EmptyString_ResultEmpty() {
      string Actual = "".JsonEncode();
      Assert.AreEqual("", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_SimpleString_ResultIsTheSame() {
      string Actual = "this is a test".JsonEncode();
      Assert.AreEqual("this is a test", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_StringWithControlChars_ResultIsEncoded() {
      string Actual = @"""this\tis\fan\encoding/test""".JsonEncode();
      Assert.AreEqual(@"""this\\tis\\fan\\encoding\/test""", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_StringWithQuotes_ResultIsEncoded() {
      string Actual = @"this ""is a"" test".JsonEncode();
      Assert.AreEqual(@"this ""is a"" test", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_StringWithQuotesAndControlChars_ResultIsEncoded() {
      string Source = @"""this ""is\\a"" test""";
      string Actual = Source.JsonEncode();
      Assert.AreEqual(@"""this ""is\\\\a"" test""", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_ObjectWithQuotesAndControlChars_ResultIsEncoded() {
      string Source = @"{""t"":""{\""a\"":1,\""b\"":\""a sample text\""}"",""c"":2,""r"":""some text""}";
      string Actual = Source.JsonEncode();
      Assert.AreEqual(@"{""t"":""{\\\""a\\\"":1,\\\""b\\\"":\\\""a sample text\\\""}"",""c"":2,""r"":""some text""}", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_ArrayWithStrings_ResultIsEncoded() {
      string Actual = @"[""Item1"",""Item2""]".JsonEncode();
      Assert.AreEqual(@"[""Item1"",""Item2""]", Actual);
    }

    [TestMethod(), TestCategory("NC20.Json"), TestCategory("NC20.Json.JsonEncode")]
    public void JsonEncode_FolderPath_ResultIsEncoded() {
      string Actual = @"c:\windows\test /param".JsonEncode();
      Assert.AreEqual(@"c:\\windows\\test \/param", Actual);
    }
  }
}
