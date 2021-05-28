using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLTools;
using BLTools.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.TextBox {
  [TestClass]
  public class TextBoxTest {
    

    [TestMethod]
    public void TextBoxWithTitle_NoTitle_ResultIsOk() {
      string Content = "blabla vldozid zerdo fizf";

      string Target = Content.Box();

      Console.WriteLine(Target);
      Assert.AreEqual("+---------------------------+", Target.Split('\n', '\r').First());
    }

    [TestMethod]
    public void TextBoxWithTitle_SimpleTitle_ResultIsOk() {
      string Title = "Test box";
      string Content = "blabla vldozid zerdo fizf";

      string Target = Content.Box(Title);

      Console.WriteLine(Target);
      Assert.AreEqual("+-[Test box]----------------+", Target.Split('\n', '\r').First());
    }

    [TestMethod]
    public void TextBoxWithTitle_TitleTooBig_TitleIsReduced() {
      string Title = "Test box";
      string Content = "blabla";

      string Target = Content.Box(Title);

      Console.WriteLine(Target);
      Assert.AreEqual("+-[Test]-+", Target.Split('\n', '\r').First());
    }

    [TestMethod]
    public void TextBoxWithTitle_SimpleTitleMargin2_ResultIsOk() {
      string Title = "Test box";
      string Content = "blabla vldozid zerdo fizf";

      string Target = Content.Box(Title, 2);

      Console.WriteLine(Target);
      Assert.AreEqual("+-[Test box]--------------------+", Target.Split('\n', '\r').First());
    }

    [TestMethod]
    public void TextBoxFixedWidth_SimpleTitle_ResultIsOk() {
      string Title = "Test box";
      string Content = "blabla vldozid zerdo fizf";

      string Target = Content.BoxFixedWidth(Title, 40);

      Console.WriteLine(Target);
      Assert.AreEqual("+-[Test box]---------------------------+", Target.Split('\n', '\r').First());
    }

    [TestMethod]
    public void TextBoxFixedWidth_VeryLargeTitle_ResultIsOk() {
      string Title = "Test box very large title, bigger than box";
      string Content = "blabla vldozid zerdo fizf";

      string Target = Content.BoxFixedWidth(Title, 40);

      Console.WriteLine(Target);
      Assert.AreEqual("+-[Test box very large title, bigger ]-+", Target.Split('\n', '\r').First());
    }
  }
}
