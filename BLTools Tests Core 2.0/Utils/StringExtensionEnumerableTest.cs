using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Utils {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
  [TestClass]
  public class StringExtensionEnumerableTest {

    private const string COMPUTER = "Computer";
    private const string PERSONAL_COMPUTER = "Personal computers";
    private const string MAINFRAME = "Mainframe";
    private const string POCKET_PC = "Pocket PC";
    private const string TELEVISION_UK = "Television";
    private const string TELEVISION_FR = "Télévision";
    private const string CHAINE_HIFI = "Chaîne hifi";

    private string[] ListOfStrings = new string[] { COMPUTER, PERSONAL_COMPUTER, MAINFRAME, POCKET_PC, TELEVISION_FR };

    [TestMethod]
    public void SearchIsIn_StringInTheList_ResultTrue() {
      Assert.IsTrue(COMPUTER.IsIn(ListOfStrings));
    }

    [TestMethod]
    public void SearchIsIn_StringNotInTheList_ResultFalse() {
      Assert.IsFalse(CHAINE_HIFI.IsIn(ListOfStrings));
    }

    [TestMethod]
    public void SearchIsIn_StringInTheListIgnoreCase_ResultTrue() {
      Assert.IsTrue(MAINFRAME.ToLower().IsIn(ListOfStrings));
    }

    [TestMethod]
    public void SearchIsIn_StringInTheListCase_ResultFalse() {
      Assert.IsFalse(MAINFRAME.ToLower().IsIn(ListOfStrings, StringComparison.InvariantCulture));
    }

    [TestMethod]
    public void SearchIsIn_StringInTheListCaseFrench_ResultTrue() {
      CultureInfo OldCulture = CultureInfo.CurrentCulture;
      CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("FR-fr");
      Assert.IsFalse(TELEVISION_UK.IsIn(ListOfStrings, StringComparison.CurrentCulture));
      Assert.IsTrue(TELEVISION_FR.IsIn(ListOfStrings, StringComparison.CurrentCulture));
      CultureInfo.CurrentCulture = OldCulture;
    }


    [TestMethod]
    public void SearchIsNotIn_StringInNotTheList_ResultTrue() {
      Assert.IsTrue(CHAINE_HIFI.IsNotIn(ListOfStrings));
    }

    [TestMethod]
    public void SearchIsNotIn_StringNotInTheList_ResultFalse() {
      Assert.IsFalse(COMPUTER.IsNotIn(ListOfStrings));
    }
  }
#endif
}
