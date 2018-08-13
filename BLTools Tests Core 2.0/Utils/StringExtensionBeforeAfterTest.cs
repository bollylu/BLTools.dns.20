using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.UnitTest.Core20.Extensions {


  /// <summary>
  ///This is a test class for StringExtensionTest and is intended
  ///to contain all StringExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class StringExtensionBeforeAfterTest {

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

    #region --- Before --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeWord_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "A brown";
      string actual = sourceString.Before(" fox");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeWordCaseInsensitive_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "A brown";
      string actual = sourceString.Before(" Fox", StringComparison.CurrentCultureIgnoreCase);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeLetter_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "A br";
      string actual = sourceString.Before("o");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeChar_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "A brown fo";
      string actual = sourceString.Before('x');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeDotInSmallText_ResultOK() {
      string sourceString = "1.10";
      string expected = "1";
      string actual = sourceString.Before('.');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeEmptyString_CompleteSource() {
      string sourceString = "1.10";
      string expected = "1.10";
      string actual = sourceString.Before("");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_SourceEmptyBeforeString_EmptyString() {
      string sourceString = "";
      string expected = "";
      string actual = sourceString.Before("toto");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Before")]
    public void StringExtension_BeforeInexistantString_ResultEmpty() {
      string sourceString = "1.10";
      string expected = "";
      string actual = sourceString.Before("toto");
      Assert.AreEqual(expected, actual);
    }
    #endregion --- Before --------------------------------------------

    #region --- BeforeLast --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_BeforeLastWord_ResultOK() {
      string sourceString = "A brown fox fox jumps over a lazy foxy-dog";
      string expected = "A brown fox fox jumps over a lazy ";
      string actual = sourceString.BeforeLast("fox");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_BeforeLastLetter_ResultOK() {
      string sourceString = @"\\server\sharename\folder\file.txt";
      string expected = @"\\server\sharename\folder";
      string actual = sourceString.BeforeLast(@"\");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_BeforeLastChar_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "A brown fox jumps over a lazy d";
      string actual = sourceString.BeforeLast('o');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_BeforeLastDotInSmallText_ResultOK() {
      string sourceString = "1.10";
      string expected = "1";
      string actual = sourceString.BeforeLast('.');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_BeforeLastEmptyString_CompleteSource() {
      string sourceString = "1.10";
      string expected = "1.10";
      string actual = sourceString.BeforeLast("");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_SourceEmptyBeforeLastString_EmptyString() {
      string sourceString = "";
      string expected = "";
      string actual = sourceString.BeforeLast("toto");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.BeforeLast")]
    public void StringExtension_BeforeLastInexistantString_ResultEmpty() {
      string sourceString = "1.10";
      string expected = "";
      string actual = sourceString.BeforeLast("toto");
      Assert.AreEqual(expected, actual);
    }
    #endregion --- BeforeLast --------------------------------------------

    #region --- After --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_AfterWord_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = " jumps over a lazy dog";
      string actual = sourceString.After(" fox");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_AfterLetter_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "wn fox jumps over a lazy dog";
      string actual = sourceString.After("o");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_AfterChar_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = " jumps over a lazy dog";
      string actual = sourceString.After('x');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_AFterDotInSmallText_ResultOK() {
      string sourceString = "1.10";
      string expected = "10";
      string actual = sourceString.After('.');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_AfterEmptyString_CompleteSource() {
      string sourceString = "1.10";
      string expected = "1.10";
      string actual = sourceString.After("");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_SourceEmptyAfterString_ResultEmpty() {
      string sourceString = "";
      string expected = "";
      string actual = sourceString.After("toto");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.After")]
    public void StringExtension_AfterInexistantString_ResultEmpty() {
      string sourceString = "1.10";
      string expected = "";
      string actual = sourceString.After("toto");
      Assert.AreEqual(expected, actual);
    }
    #endregion --- After --------------------------------------------

    #region --- AfterLast --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastWord_ResultOK() {
      string sourceString = "A brown fox fox jumps over a lazy foxy-dog";
      string expected = "y-dog";
      string actual = sourceString.AfterLast("fox");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastLetterOnlyOne_ResultOK() {
      string sourceString = "A brown fox fox jumps over a lazy foxy-dog";
      string expected = "g";
      string actual = sourceString.AfterLast("o");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastLetter_ResultOK() {
      string sourceString = @"\\server\sharename\folder\file.txt";
      string expected = @"file.txt";
      string actual = sourceString.AfterLast(@"\");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastChar_ResultOK() {
      string sourceString = "A brown fox jumps over a lazy dog";
      string expected = "g";
      string actual = sourceString.AfterLast('o');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastDotInSmallText_ResultOK() {
      string sourceString = "1.10";
      string expected = "10";
      string actual = sourceString.AfterLast('.');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastEmptyString_CompleteSource() {
      string sourceString = "1.10";
      string expected = "1.10";
      string actual = sourceString.AfterLast("");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_SourceEmptyAfterLastString_EmptyString() {
      string sourceString = "";
      string expected = "";
      string actual = sourceString.AfterLast("toto");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.AfterLast")]
    public void StringExtension_AfterLastInexistantString_ResultEmpty() {
      string sourceString = "1.10";
      string expected = "";
      string actual = sourceString.AfterLast("toto");
      Assert.AreEqual(expected, actual);
    }
    #endregion --- AfterLast --------------------------------------------

    #region --- Between chars --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharSourceEmpty_ResultEmpty() {
      string sourceString = "";
      string expected = "";
      string actual = sourceString.Between('[',']');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharSourceNormal_ResultOK() {
      string sourceString = "This  is a test [various data; example]";
      string expected = "various data; example";
      string actual = sourceString.Between('[', ']');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharBothDelimiterMissing_ResultEmpty() {
      string sourceString = "This  is a test various data; example";
      string expected = "";
      string actual = sourceString.Between('[', ']');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharFirstDelimiterMissing_ResultEmpty() {
      string sourceString = "This  is a test [various data; example";
      string expected = "";
      string actual = sourceString.Between('[', ']');
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharSecondDelimiterMissing_ResultEmpty() {
      string sourceString = "This  is a test various data; example]";
      string expected = "";
      string actual = sourceString.Between('[', ']');
      Assert.AreEqual(expected, actual);
    }
    #endregion --- Between chars --------------------------------------------

    #region --- Between strings --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsSourceEmpty_ResultEmpty() {
      string sourceString = "";
      string expected = "";
      string actual = sourceString.Between("=[", "]=");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsSourceNormal_ResultOK() {
      string sourceString = "This  is a test =[Live]= =[blabla]=";
      string expected = "Live";
      string actual = sourceString.Between("=[", "]=");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsSourceNormalCaseInsensitive_ResultOK() {
      string sourceString = "This  is a test DelimLiveDElim =[blabla]=";
      string expected = "Live";
      string actual = sourceString.Between("delim", "delim", StringComparison.InvariantCultureIgnoreCase);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsBothDelimiterMissing_ResultEmpty() {
      string sourceString = "This  is a test various data; example";
      string expected = "";
      string actual = sourceString.Between("=[", "]=");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsFirstDelimiterMissing_ResultEmpty() {
      string sourceString = "This  is a test =[various data; example";
      string expected = "";
      string actual = sourceString.Between("=[", "]=");
      Assert.AreEqual(expected, actual);
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsSecondDelimiterMissing_ResultEmpty() {
      string sourceString = "This  is a test various data; example]=";
      string expected = "";
      string actual = sourceString.Between("=[", "]=");
      Assert.AreEqual(expected, actual);
    }
    #endregion --- Between strings --------------------------------------------

    #region --- ItemsBetween chars --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharsMultipleValuesSourceEmpty_ResultZero() {
      string sourceString = "";
      int expected = 0;
      IEnumerable<string> actual = sourceString.ItemsBetween('[', ']');
      Assert.AreEqual(expected, actual.Count());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharsMultipleValues_ResultOK() {
      string sourceString = "source value [item1] [Item2] [item3;item4]";
      int expected = 3;
      List<string> actual = sourceString.ItemsBetween('[', ']').ToList();
      Assert.AreEqual(expected, actual.Count());
      Assert.AreEqual("item1", actual.First());
      Assert.AreEqual("Item2", actual[1]);
      Assert.AreEqual("item3;item4", actual.Last());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharsMultipleValuesErrors_ResultOK() {
      string sourceString = "source value [item1] [Item2 [item3;item4]";
      int expected = 2;
      List<string> actual = sourceString.ItemsBetween('[', ']').ToList();
      Assert.AreEqual(expected, actual.Count());
      Assert.AreEqual("item1", actual.First());
      Assert.AreEqual("Item2 [item3;item4", actual.Last());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharsMultipleValuesDeStartlimiterMissing_ResultZero() {
      string sourceString = "This  is a test various data; example]=";
      int expected = 0;
      IEnumerable<string> actual = sourceString.ItemsBetween('[', ']');
      Assert.AreEqual(expected, actual.Count());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenCharMultipleValuesEndDelimiterMissing_ResultZero() {
      string sourceString = "source value [item1 [Item2 [item3;item4";
      int expected = 0;
      List<string> actual = sourceString.ItemsBetween('[', ']').ToList();
      Assert.AreEqual(expected, actual.Count());
    }
    #endregion --- ItemsBetween chars --------------------------------------------

    #region --- ItemsBetween strings --------------------------------------------
    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringMultipleValuesSourceEmpty_ResultZero() {
      string sourceString = "";
      int expected = 0;
      IEnumerable<string> actual = sourceString.ItemsBetween("=[", "]=");
      Assert.AreEqual(expected, actual.Count());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsMultipleValues_ResultOK() {
      string sourceString = "source value =[item1]= =[Item2]==[item3;item4]=";
      int expected = 3;
      List<string> actual = sourceString.ItemsBetween("=[", "]=").ToList();
      Assert.AreEqual(expected, actual.Count());
      Assert.AreEqual("item1", actual.First());
      Assert.AreEqual("Item2", actual[1]);
      Assert.AreEqual("item3;item4", actual.Last());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsMultipleValuesErrors_ResultOK() {
      string sourceString = "source value =[item1]= =[Item2] =[item3;item4]=";
      int expected = 2;
      List<string> actual = sourceString.ItemsBetween("=[", "]=").ToList();
      Assert.AreEqual(expected, actual.Count());
      Assert.AreEqual("item1", actual.First());
      Assert.AreEqual("Item2] =[item3;item4", actual.Last());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsMultipleValuesStartDelimiterMissing_ResultZero() {
      string sourceString = "This  is a test various data; example]=";
      int expected = 0;
      IEnumerable<string> actual = sourceString.ItemsBetween("=[", "]=");
      Assert.AreEqual(expected, actual.Count());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsMultipleValuesEndDelimiterMissing_ResultZero() {
      string sourceString = "source value =[item1 =[Item2 =[item3;item4";
      int expected = 0;
      List<string> actual = sourceString.ItemsBetween("=[", "]=").ToList();
      Assert.AreEqual(expected, actual.Count());
    }

    [TestMethod(), TestCategory("NC20.String.Between")]
    public void StringExtension_BetweenStringsMultipleValuesCaseInsensitive_ResultZero() {
      string sourceString = "source value Debutitem1FINdeButItem2 fin";
      int expected = 2;
      List<string> actual = sourceString.ItemsBetween("debut", "fin", StringComparison.InvariantCultureIgnoreCase).ToList();
      Assert.AreEqual(expected, actual.Count());
      Assert.AreEqual("item1", actual.First());
      Assert.AreEqual("Item2 ", actual.Last());
    }
    #endregion --- ItemsBetween strings --------------------------------------------

  }
}
