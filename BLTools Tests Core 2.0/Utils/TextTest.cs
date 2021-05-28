using System;
using BLTools;
using static BLTools.Text.TextBox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BLTools.UnitTest.Extension {
  [TestClass]
  public class TextTest {

    [TestMethod(), TestCategory("TextBox")]
    public void TextBox_FixedWidthDefault_ResultOk() {
      string source = "Sample text";
      string FormattedSource = source.PadRight(DEFAULT_FIXED_WIDTH - 4);
      string TopAndBottom = $"+{new string('-', DEFAULT_FIXED_WIDTH - 2)}+";
      string ExpectedResult = $"{TopAndBottom}{Environment.NewLine}| {FormattedSource} |{Environment.NewLine}{TopAndBottom}";
      string actual = BuildFixedWidth(source, EStringAlignment.Left);
      Assert.AreEqual(ExpectedResult, actual);
    }

    [TestMethod(), TestCategory("TextBox")]
    public void TextBox_FixedWidth40_ResultOk() {
      int BoxWidth = 40;
      string source = "Sample text";
      string FormattedSource = source.PadRight(BoxWidth - 4);
      string TopAndBottom = $"+{new string('-', BoxWidth - 2)}+";
      string ExpectedResult = $"{TopAndBottom}{Environment.NewLine}| {FormattedSource} |{Environment.NewLine}{TopAndBottom}";
      string actual = BuildFixedWidth(source, BoxWidth, EStringAlignment.Left);
      Assert.AreEqual(ExpectedResult, actual);
    }

    //[TestMethod(), TestCategory("TextBox")]
    //public void TextBox_HorizontalRowDefault_ResultOk() {
    //  string ExpectedResult = new string('-', Console.WindowWidth);
    //  string actual = TextBox.BuildHorizontalRow();
    //  Assert.AreEqual(ExpectedResult, actual);
    //}

    [TestMethod(), TestCategory("TextBox")]
    public void TextBox_HorizontalRowDouble40_ResultOk() {
      string ExpectedResult = new string('=', 40);
      string actual = BuildHorizontalRow(40, EHorizontalRowType.Double);
      Assert.AreEqual(ExpectedResult, actual);
    }

    [TestMethod(), TestCategory("TextBox")]
    public void TextBox_HorizontalRowWithTextSingle20_ResultOk() {
      string ExpectedResult = $"--This is text----";
      string actual = BuildHorizontalRowWithText("This is text", 20);
      Assert.AreEqual(ExpectedResult, actual);
    }

  }
}
