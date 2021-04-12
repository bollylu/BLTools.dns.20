using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using BLTools.Storage.Csv;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Storage.csv {
  [TestClass]
  public class TCsvReaderTest {

    #region --- Test support --------------------------------------------
    private TestContext _TestContextInstance;
    public TestContext TestContext {
      get { return _TestContextInstance; }
      set { _TestContextInstance = value; }
    }

    private void TraceRows(IEnumerable<IRowCsv> rows) {
      if (rows is null) {
        TestContext.WriteLine("(null)");
        return;
      }
      if (rows.IsEmpty()) {
        TestContext.WriteLine("(empty)");
        return;
      }
      foreach (IRowCsv RowItem in rows) {
        TestContext.WriteLine(RowItem.ToString());
      }
    }
    #endregion --- Test support -----------------------------------------

    [TestMethod]
    public void CsvReader_ReadHeaderFromHeaderOnly_ReadOk() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(HCsvStorageGenerator.HeaderOnlyContentFile()))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> HeaderSection = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection);
          
          Assert.IsFalse(HeaderSection.IsEmpty());
          Assert.AreEqual(3, HeaderSection.Count());
        }
      }

    }

    [TestMethod]
    public void CsvReader_ReadHeaderFromHeaderPlusData_ReadOk() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(HCsvStorageGenerator.HeaderAndDataContentFile()))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> HeaderSection = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection);

          Assert.IsFalse(HeaderSection.IsEmpty());
          Assert.AreEqual(3, HeaderSection.Count());
        }
      }

    }

    [TestMethod]
    public void CsvReader_ReadDataFromHeaderPlusData_ReadOk() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(HCsvStorageGenerator.HeaderAndDataContentFile()))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> DataSection = Reader.ReadDataSection(true);
          TraceRows(DataSection);

          Assert.IsFalse(DataSection.IsEmpty());
          Assert.AreEqual(2, DataSection.Count());
        }
      }
    }

    [TestMethod]
    public void CsvReader_ReadHeaderAndDataFromHeaderPlusData_ReadOk() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(HCsvStorageGenerator.HeaderAndDataContentFile()))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> HeaderSection = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection);

          Assert.IsFalse(HeaderSection.IsEmpty());
          Assert.AreEqual(3, HeaderSection.Count());

          IEnumerable<IRowCsv> DataSection = Reader.ReadDataSection(true);
          TraceRows(DataSection);

          Assert.IsFalse(DataSection.IsEmpty());
          Assert.AreEqual(2, DataSection.Count());
        }
      }
    }

    [TestMethod]
    public void CsvReader_ReadHeaderAndDataAndFooterFromHeaderPlusDataPlusFooter_ReadOk() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(HCsvStorageGenerator.MultipleHeaderAndDataAndFooterFile()))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> HeaderSection = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection);

          Assert.IsFalse(HeaderSection.IsEmpty());
          Assert.AreEqual(3, HeaderSection.Count());

          IEnumerable<IRowCsv> DataSection = Reader.ReadDataSection(true);
          TraceRows(DataSection);

          Assert.IsFalse(DataSection.IsEmpty());
          Assert.AreEqual(2, DataSection.Count());

          IEnumerable<IRowCsv> FooterSection = Reader.ReadFooterSection(true);
          TraceRows(FooterSection);

          Assert.IsFalse(FooterSection.IsEmpty());
          Assert.AreEqual(2, FooterSection.Count());
        }
      }
    }

    [TestMethod]
    public void CsvReader_ReadMultipleSectionsFromHeaderPlusDataPlusFooter_ReadOk() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(HCsvStorageGenerator.MultipleHeaderAndDataAndFooterFile()))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> HeaderSection1 = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection1);
          Assert.IsFalse(HeaderSection1.IsEmpty());
          Assert.AreEqual(3, HeaderSection1.Count());

          IEnumerable<IRowCsv> DataSection1 = Reader.ReadDataSection(true);
          TraceRows(DataSection1);
          Assert.IsFalse(DataSection1.IsEmpty());
          Assert.AreEqual(2, DataSection1.Count());

          IEnumerable<IRowCsv> HeaderSection2 = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection2);
          Assert.IsFalse(HeaderSection2.IsEmpty());
          Assert.AreEqual(3, HeaderSection2.Count());

          IEnumerable<IRowCsv> DataSection2 = Reader.ReadDataSection(true);
          TraceRows(DataSection2);
          Assert.IsFalse(DataSection2.IsEmpty());
          Assert.AreEqual(2, DataSection2.Count());

          IEnumerable<IRowCsv> FooterSection = Reader.ReadFooterSection(true);
          TraceRows(FooterSection);
          Assert.IsFalse(FooterSection.IsEmpty());
          Assert.AreEqual(2, FooterSection.Count());
        }
      }
    }

    [TestMethod]
    public void CsvReader_ReadHeaderFromEmptySource_ResultEmpty() {

      using (MemoryStream Source = new MemoryStream(Encoding.UTF8.GetBytes(""))) {
        using (TCsvReader Reader = new TCsvReader(Source)) {
          IEnumerable<IRowCsv> HeaderSection = Reader.ReadHeaderSection(true);
          TraceRows(HeaderSection);

          Assert.IsTrue(HeaderSection.IsEmpty());
        }
      }

    }
  }
}
