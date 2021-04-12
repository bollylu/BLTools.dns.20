using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BLTools.Storage.Csv;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Storage.csv {

  [TestClass]
  public class TCsvWriterTest {

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
    public void CsvWriter_WriteHeaderRow_RowIsOk() {

      byte[] Temp;
      using (MemoryStream TargetStream = new MemoryStream()) {
        using (TCsvWriter Writer = new TCsvWriter(TargetStream, true)) {

          IRowCsv HeaderRow = new TRowCsvHeader() { Id = "Title" };
          HeaderRow.Set("My project");
          Writer.WriteRow(HeaderRow);

        }

        Temp = TargetStream.ToArray();
      }

      TestContext.WriteLine(Encoding.Default.GetString(Temp));

      using (MemoryStream Source = new MemoryStream(Temp)) {
        using (TCsvReader Reader = new TCsvReader(Source, Encoding.Default)) {
          IRowCsv[] HeaderSection = Reader.ReadHeaderSection();
          Assert.IsFalse(HeaderSection.IsEmpty());
        }
      }
    }

    [TestMethod]
    public void CsvWriter_WriteHeaderAndDataRow_RowIsOk() {

      byte[] Temp;
      using (MemoryStream TargetStream = new MemoryStream()) {
        using (TCsvWriter Writer = new TCsvWriter(TargetStream, true)) {

          IRowCsv HeaderRow = new TRowCsvHeader() { Id = "Title" };
          HeaderRow.Set("My project");
          Writer.WriteRow(HeaderRow);

          IRowCsv DataRow = new TRowCsvData() { Id = "Curve" };
          DataRow.Set(12,35,65,98);
          Writer.WriteRow(DataRow);

        }

        Temp = TargetStream.ToArray();
      }

      TestContext.WriteLine(Encoding.Default.GetString(Temp));

      using (MemoryStream Source = new MemoryStream(Temp)) {
        using (TCsvReader Reader = new TCsvReader(Source, Encoding.Default)) {
          IRowCsv[] HeaderSection = Reader.ReadHeaderSection();
          Assert.IsFalse(HeaderSection.IsEmpty());
          IRowCsv[] DataSection = Reader.ReadDataSection();
          Assert.IsFalse(DataSection.IsEmpty());
        }
      }
    }
  }
}
