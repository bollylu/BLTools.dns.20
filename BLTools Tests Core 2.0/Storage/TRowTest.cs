using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using BLTools.Storage.Csv;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Storage.csv.row {

  [TestClass]
  public class TRowTest {
    #region --- Test support --------------------------------------------
    private TestContext _TestContextInstance;
    public TestContext TestContext {
      get { return _TestContextInstance; }
      set { _TestContextInstance = value; }
    }
    #endregion --- Test support -----------------------------------------

    [TestMethod]
    public void CreateRow_Header_RowIsHeader() {
      IRowCsv Row = new TRowCsvHeader();
      Assert.AreEqual(ERowCsvType.Header, Row.RowType);
    }

    [TestMethod]
    public void CreateRow_Data_RowIsData() {
      IRowCsv Row = new TRowCsvData();
      Assert.AreEqual(ERowCsvType.Data, Row.RowType);
    }

    [TestMethod]
    public void CreateRow_Footer_RowIsFooter() {
      IRowCsv Row = new TRowCsvFooter();
      Assert.AreEqual(ERowCsvType.Footer, Row.RowType);
    }

    [TestMethod]
    public void RowHeaderContent_AddIdAndContent_AllIsOk() {
      const string FIELD_AGE = "Age";
      const int VALUE_AGE = 42;
      IRowCsv Row = new TRowCsvHeader() { Id = FIELD_AGE };
      Assert.AreEqual(FIELD_AGE, Row.Id);
      Row.Set(VALUE_AGE);

      TestContext.WriteLine(Row.ToString());

      Assert.AreEqual(VALUE_AGE, Row.Get<int>());
    }

    [TestMethod]
    public void RowDataContent_AddIdAndContent_AllIsOk() {
      const string FIELD_AGE = "AgeDuCapitaine";
      const double VALUE_AGE = 54.625;
      IRowCsv Row = new TRowCsvData() { Id = FIELD_AGE };
      Assert.AreEqual(FIELD_AGE, Row.Id);
      Row.Set(VALUE_AGE);

      TestContext.WriteLine(Row.ToString());

      Assert.AreEqual(VALUE_AGE, Row.Get<double>());
    }

    [TestMethod]
    public void RowFooterContent_AddIdAndContent_AllIsOk() {
      const string FIELD_SUMMARY = "Summary";
      const float VALUE_SUMMARY = 138.25f;
      IRowCsv Row = new TRowCsvFooter() { Id = FIELD_SUMMARY };
      Assert.AreEqual(FIELD_SUMMARY, Row.Id);
      Row.Set(VALUE_SUMMARY);

      TestContext.WriteLine(Row.ToString());

      Assert.AreEqual(VALUE_SUMMARY, Row.Get<float>());
    }

    [TestMethod]
    public void RowDataContent_SingleBool_AllIsOk() {
      const string ID = "Test";
      const bool VALUE = true;
      IRowCsv Row = new TRowCsvData() { Id = ID };
      Assert.AreEqual(ID, Row.Id);
      Row.Set(VALUE);

      TestContext.WriteLine(Row.ToString());

      Assert.AreEqual(VALUE, Row.Get<bool>());
    }

    [TestMethod]
    public void RowDataContent_SingleDateTime_AllIsOk() {
      const string ID = "Birth";
      DateTime VALUE = DateTime.Parse("1966-04-28T12:26:34");
      IRowCsv Row = new TRowCsvData() { Id = ID };
      Assert.AreEqual(ID, Row.Id);
      Row.Set(VALUE);

      TestContext.WriteLine(Row.ToString());

      Assert.AreEqual(VALUE, Row.Get<DateTime>());
    }

    [TestMethod]
    public void RowDataContent_SingleString_AllIsOk() {
      const string ID = "Surname";
      const string VALUE = "master";
      IRowCsv Row = new TRowCsvData() { Id = ID };
      Assert.AreEqual(ID, Row.Id);
      Row.Set(VALUE);

      TestContext.WriteLine(Row.Render());

      Assert.AreEqual(VALUE, Row.Get<string>());
    }

    [TestMethod]
    public void RowDataContent_MultiData_AllIsOk() {
      const string ID = "Points";
      const double VALUE1 = 10.2d;
      const double VALUE2 = 10.8d;
      const double VALUE3 = 12.6d;
      const double VALUE4 = 28.4d;
      IRowCsv Row = new TRowCsvData() { Id = ID };
      Assert.AreEqual(ID, Row.Id);
      Row.Set(VALUE1,
              VALUE2,
              VALUE3,
              VALUE4);

      TestContext.WriteLine(Row.Render());

      Assert.AreEqual(VALUE1, Row.Get<double>(0));
      Assert.AreEqual(VALUE2, Row.Get<double>(1));
      Assert.AreEqual(VALUE3, Row.Get<double>(2));
      Assert.AreEqual(VALUE4, Row.Get<double>(3));
    }


    [TestMethod]
    public void RowDataContent_MultiDataTypes_AllIsOk() {
      const string ID = "Points";
      const double VALUE1 = 10.269324789d;
      const float VALUE2 = 123.456f;
      const string VALUE3 = "Hellow world";
      DateTime VALUE4 = new DateTime(1966,4,28,12,24,36);
      
      IRowCsv Row = new TRowCsvData() { Id = ID };
      Assert.AreEqual(ID, Row.Id);

      Row.Set(VALUE1,
              VALUE2,
              VALUE3,
              VALUE4);

      TestContext.WriteLine(Row.Render());

      Assert.AreEqual(VALUE1, Row.Get<double>(0));
      Assert.AreEqual(VALUE2, Row.Get<float>(1));
      Assert.AreEqual(VALUE3, Row.Get<string>(2));
      Assert.AreEqual(VALUE4, Row.Get<DateTime>(3));
    }

    [TestMethod]
    public void RowDataContent_EnumerableDouble_AllIsOk() {
      const string ID = "Channels";
      double[] Channels = new double[] { 1.2, 2.3, 3.4, 4.5, 5.6, 6.7 };

      IRowCsv Row = new TRowCsvData() { Id = ID };
      Assert.AreEqual(ID, Row.Id);

      Row.Set(Channels);

      TestContext.WriteLine(Row.Render());

      Assert.AreEqual(6, Row.Get().Split(ARowCsv.SEPARATOR).Length); //Count of items
      Assert.AreEqual(6.7, Row.Get<double>(5)); // The last item
      Assert.AreEqual(-1d, Row.Get<double>(10, -1d)); // After the last item, hence default value
    }
  }
}
