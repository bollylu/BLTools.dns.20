﻿using BLTools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace BLTools.UnitTest.Extensions {


  /// <summary>
  ///This is a test class for ByteArrayExtensionTest and is intended
  ///to contain all ByteArrayExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class ByteArrayExtensionTest {


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


    /// <summary>
    ///A test for ToHexString
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToHexString_CommaSeparator_ResultOK() {
      byte[] rawData = new byte[] { 0x0C, 0x17, 0x22, 0x2D, 0x38 };
      string separator = ",";
      string expected = "0C,17,22,2D,38";
      string actual;
      actual = rawData.ToHexString(separator);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for ToHexString
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToHexString_NoSeparator_ResultOK() {
      byte[] rawData = new byte[] { 0x0C, 0x17, 0x22, 0x2D, 0x38 };
      string expected = "0C 17 22 2D 38";
      string actual;
      actual = rawData.ToHexString();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for ToCharString
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToCharString_NormalCharacters_ResultOK() {
      byte[] rawData = "abcdef".ToByteArray();
      string expected = "abcdef";
      string actual;
      actual = rawData.ToCharString();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for ToCharString
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToCharString_EmptySource_ResultOK() {
      byte[] rawData = "".ToByteArray();
      string expected = "";
      string actual;
      actual = rawData.ToCharString();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for ToCharString
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToCharString_SpecialChars_ResultOK() {
      byte[] rawData = "Text\r\n".ToByteArray();
      string expected = "Text\r\n";
      string actual;
      actual = rawData.ToCharString();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for building a byte array from a hex string
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToByteArrayFromHexString_InputOk_ResultOK() {
      string Source = "2365A2B7";
      byte[] Target = Source.ToByteArrayFromHex();
      string Compare = Target.ToHexString("");
      Console.WriteLine(Compare);
      Assert.AreEqual(Source, Compare);
    }

    /// <summary>
    ///A test for building a byte array from a hex string
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToByteArrayFromHexString_InputEmpty_ResultOK() {
      string Source = "";
      byte[] Target = Source.ToByteArrayFromHex();
      Assert.AreEqual(0, Target.Length);
    }

    /// <summary>
    ///A test for building a byte array from a hex string
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    [ExpectedException(typeof(FormatException))]
    public void ToByteArrayFromHexString_InputInvalidLength_Exception() {
      string Source = "A3A";
      byte[] Target = Source.ToByteArrayFromHex();
    }

    /// <summary>
    ///A test for building a byte array from a hex string
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    [ExpectedException(typeof(FormatException))]
    public void ToByteArrayFromHexString_InputInvalid_Exception() {
      try {
        string Source = "A3Z4";
        byte[] Target = Source.ToByteArrayFromHex();
      } catch (Exception ex) {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.InnerException.Message);
        throw;
      }

    }

    /// <summary>
    ///A test for building a byte array from a hex string
    ///</summary>
    [TestMethod(), TestCategory("Array")]
    public void ToByteArrayFromHexString_InputMAC_ResultOK() {
      string Source = "A3:B4:DE:34:67:1C";
      byte[] Target = Source.ToByteArrayFromHex();
      string Compare = Target.ToHexString(":");
      Console.WriteLine(Compare);
      Assert.AreEqual(Source, Compare);
    }
  }
}
