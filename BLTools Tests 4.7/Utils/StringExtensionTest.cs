using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.UnitTest.FW47.Extensions {


  /// <summary>
  ///This is a test class for StringExtensionTest and is intended
  ///to contain all StringExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class StringExtensionTest {

    #region Test context
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
    #endregion Test context

    #region Left and right
    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_Left7_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      int length = 7;
      string expected = "A brown";
      string actual = sourceString.Left(length);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_Right8_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      int length = 8;
      string expected = "lazy dog";
      string actual = sourceString.Right(length);
      Assert.AreEqual(expected, actual);
    }
    #endregion Left and right

    #region After
    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterEmptyString_ResultEmpty() {
      string sourceString = "";
      string actual = sourceString.After("*");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterText_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.After("fox");
      Assert.AreEqual(" jumps over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterTextDelimIsAtStart_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.After("A");
      Assert.AreEqual("A brown fox jumps over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterTextDelimIsAtEnd_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.After("g");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterTextDelimIsNonExistent_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.After("0");
      Assert.AreEqual("A brown fox jumps over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterTextDelimIsSameAsSource_ResultEmpty() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.After("A brown fox jumps over a lazy dog");
      Assert.AreEqual("", actual);
    }
    #endregion After

    #region AfterLast
    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastEmptyString_ResultEmpty() {
      string sourceString = "";
      string actual = sourceString.AfterLast("*");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastText_ResultOK() {
      string sourceString = "A brown fox jumps fox over a lazy dog";
      string actual = sourceString.AfterLast("fox");
      Assert.AreEqual(" over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastTextDelimIsAtStart_ResultOK() {
      string sourceString = "A brown fox jumps over A lazy dog";
      string actual = sourceString.AfterLast("A");
      Assert.AreEqual(" lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastTextDelimIsAtEnd_ResultOK() {
      string sourceString = "A brown fox jumps g over a lazy dog";
      string actual = sourceString.AfterLast("g");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastTextDelimIsNonExistent_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.AfterLast("0");
      Assert.AreEqual("A brown fox jumps over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastTextDelimIsSameAsSource_ResultEmpty() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.AfterLast("A brown fox jumps over a lazy dog");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_AfterLastPathname_ResultFilename() {
      string sourceString = @"\\server\path\filename.txt";
      string actual = sourceString.AfterLast(@"\");
      Assert.AreEqual("filename.txt", actual);
    }
    #endregion AfterLast

    #region Before
    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeEmptyString_ResultEmpty() {
      string sourceString = "";
      string actual = sourceString.Before("*");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeText_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.Before("fox");
      Assert.AreEqual("A brown ", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeTextDelimIsAtStart_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.Before("A");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeTextDelimIsAtEnd_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.Before("g");
      Assert.AreEqual("A brown fox jumps over a lazy do", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeTextDelimIsNonExistent_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.Before("0");
      Assert.AreEqual("A brown fox jumps over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeTextDelimIsSameAsSource_ResultEmpty() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.After("A brown fox jumps over a lazy dog");
      Assert.AreEqual("", actual);
    }
    #endregion Before

    #region BeforeLast
    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastEmptyString_ResultEmpty() {
      string sourceString = "";
      string actual = sourceString.BeforeLast("*");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastText_ResultOK() {
      string sourceString = "A brown fox jumps fox over a lazy dog";
      string actual = sourceString.BeforeLast("fox");
      Assert.AreEqual("A brown fox jumps ", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastTextDelimIsAtStart_ResultOK() {
      string sourceString = "A brown fox jumps over A lazy dog";
      string actual = sourceString.BeforeLast("A");
      Assert.AreEqual("A brown fox jumps over ", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastTextDelimIsAtEnd_ResultOK() {
      string sourceString = "A brown fox jumps g over a lazy dog";
      string actual = sourceString.BeforeLast("g");
      Assert.AreEqual("A brown fox jumps g over a lazy do", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastTextDelimIsNonExistent_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.BeforeLast("0");
      Assert.AreEqual("A brown fox jumps over a lazy dog", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastTextDelimIsSameAsSource_ResultEmpty() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string actual = sourceString.BeforeLast("A brown fox jumps over a lazy dog");
      Assert.AreEqual("", actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void StringExtension_BeforeLastPathname_ResultFilename() {
      string sourceString = @"\\server\path\filename.txt";
      string actual = sourceString.BeforeLast(@"\");
      Assert.AreEqual(@"\\server\path", actual);
    }
    #endregion BeforeLast

    #region ToBool
    [TestMethod(), TestCategory("FW47.String")]
    public void ToBool_False_ResultFalse() {
      string booleanString = "false";
      bool expected = false;
      bool actual = booleanString.ToBool();
      Assert.AreEqual(expected, actual);
    }
    
    [TestMethod(), TestCategory("FW47.String")]
    public void ToBool_True_ResultTrue() {
      string booleanString = "true";
      bool expected = true;
      bool actual = booleanString.ToBool();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void ToBool_BadValue_ResultFalse() {
      string booleanString = "fal64se";
      bool expected = false;
      bool actual = booleanString.ToBool();
      Assert.AreEqual(expected, actual);
    } 
    #endregion ToBool

    #region IsAlpha
    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlpha_AlphaValue_ResultTrue() {
      string SourceValue = "OnlyAlphA";
      bool expected = true;
      bool actual = SourceValue.IsAlpha();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlpha_NonAlphaValue_ResultFalse() {
      string SourceValue = "Only1AlphA";
      bool expected = false;
      bool actual = SourceValue.IsAlpha();
      Assert.AreEqual(expected, actual);
    } 
    #endregion IsAlpha

    #region IsAlphaNumeric
    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumeric_AlphaValue_ResultTrue() {
      string SourceValue = "OnlyAlphA";
      bool expected = true;
      bool actual = SourceValue.IsAlphaNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumeric_NumericValue_ResultTrue() {
      string SourceValue = "1259763";
      bool expected = true;
      bool actual = SourceValue.IsAlphaNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumeric_AlphaNumericValue_ResultTrue() {
      string SourceValue = "Only123AlphA";
      bool expected = true;
      bool actual = SourceValue.IsAlphaNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumeric_NonAlphaNumericValue_ResultFalse() {
      string SourceValue = "Only1@lph@";
      bool expected = false;
      bool actual = SourceValue.IsAlphaNumeric();
      Assert.AreEqual(expected, actual);
    } 
    #endregion IsAlphaNumeric

    #region IsNumeric
    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumeric_NumericValue_ResultTrue() {
      string SourceValue = "12354";
      bool expected = true;
      bool actual = SourceValue.IsNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumeric_NegativeNumericValue_ResultTrue() {
      string SourceValue = "-12354";
      bool expected = true;
      bool actual = SourceValue.IsNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumeric_NumericValueWithSeparator_ResultTrue() {
      string SourceValue = "12.354,123";
      bool expected = true;
      bool actual = SourceValue.IsNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumeric_NonNumericValue_ResultFalse() {
      string SourceValue = "231655abc";
      bool expected = false;
      bool actual = SourceValue.IsNumeric();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumeric_BadNegativeNumericValue_ResultFalse() {
      string SourceValue = "231655-";
      bool expected = false;
      bool actual = SourceValue.IsNumeric();
      Assert.AreEqual(expected, actual);
    }
    #endregion IsNumeric

    #region IsAlphaOrBlank
    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaOrBlank_AlphaValue_ResultTrue() {
      string SourceValue = "Only AlphA";
      bool expected = true;
      bool actual = SourceValue.IsAlphaOrBlank();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaOrBlank_NonAlphaValue_ResultFalse() {
      string SourceValue = "Only1 AlphA";
      bool expected = false;
      bool actual = SourceValue.IsAlphaOrBlank();
      Assert.AreEqual(expected, actual);
    }
    #endregion IsAlphaOrBlank

    #region IsAlphaNumericOrBlank
    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumericOrBlank_AlphaValue_ResultTrue() {
      string SourceValue = "Only AlphA";
      bool expected = true;
      bool actual = SourceValue.IsAlphaNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumericOrBlank_NumericValue_ResultTrue() {
      string SourceValue = "1259 763";
      bool expected = true;
      bool actual = SourceValue.IsAlphaNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumericOrBlank_AlphaNumericValue_ResultTrue() {
      string SourceValue = "Only123 AlphA";
      bool expected = true;
      bool actual = SourceValue.IsAlphaNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsAlphaNumericOrBlank_NonAlphaNumericValue_ResultFalse() {
      string SourceValue = "Only1 @lph@";
      bool expected = false;
      bool actual = SourceValue.IsAlphaNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }
    #endregion IsAlphaNumeric

    #region IsNumericOrBlank
    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumericOrBlank_NumericValue_ResultTrue() {
      string SourceValue = "1235 4";
      bool expected = true;
      bool actual = SourceValue.IsNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumericOrBlank_NumericValueWithSeparator_ResultTrue() {
      string SourceValue = " 12.354,123 ";
      bool expected = true;
      bool actual = SourceValue.IsNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void IsNumericOrBlank_NonNumericValue_ResultFalse() {
      string SourceValue = "231655 abc";
      bool expected = false;
      bool actual = SourceValue.IsNumericOrBlank();
      Assert.AreEqual(expected, actual);
    }
    #endregion IsNumericOrBlank

    #region SecureString
    [TestMethod(), TestCategory("FW47.String")]
    public void ConvertToSecureString_ReverseConversion_ResultTrue() {
      string SourceValue = "1235 4";
      SecureString actual = SourceValue.ConvertToSecureString();
      Assert.AreEqual(SourceValue, actual.ConvertToUnsecureString());
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void ConvertToSecureString_Compare_ResultTrue() {
      string SourceValue = "1235 4";
      SecureString actual = SourceValue.ConvertToSecureString();
      Assert.IsTrue(SourceValue.ConvertToSecureString().IsEqualTo(actual));
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void ConvertToSecureString_CompareDifferentStringLength_ResultFalse() {
      string SourceValue = "1235 4";
      SecureString actual = "1234".ConvertToSecureString();
      Assert.IsFalse(SourceValue.ConvertToSecureString().IsEqualTo(actual));
    }

    [TestMethod(), TestCategory("FW47.String")]
    public void ConvertToSecureString_CompareSameStringLength_ResultFalse() {
      string SourceValue = "1235 4";
      SecureString actual = "1234 5".ConvertToSecureString();
      Assert.IsFalse(SourceValue.ConvertToSecureString().IsEqualTo(actual));
    }
    #endregion SecureString

    

  }
}
