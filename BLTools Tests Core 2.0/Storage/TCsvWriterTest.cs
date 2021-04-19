using System;
using System.Collections.Generic;
using System.Globalization;
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
          DataRow.Set(12, 35, 65, 98);
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

    [TestMethod]
    public void CsvWriter_WriteInt_ResultOk() {
      using (MemoryStream DataStream = new MemoryStream()) {
        using (TCsvWriter Writer = new TCsvWriter(DataStream, true)) {
          Writer.Write(42);
        }
        byte[] Target = DataStream.ToArray();

        Assert.AreEqual(42, int.Parse(Encoding.UTF8.GetString(Target)));
      }
    }

    [TestMethod]
    public void CsvWriter_WriteString_ResultOk() {
      using (MemoryStream DataStream = new MemoryStream()) {
        using (TCsvWriter Writer = new TCsvWriter(DataStream, true)) {
          Writer.Write("Hello world");
        }
        byte[] Target = DataStream.ToArray();

        Assert.AreEqual("\"Hello world\"", Encoding.UTF8.GetString(Target));
      }
    }

    [TestMethod]
    public void CsvWriter_WriteMulti_ResultOk() {
      using (MemoryStream DataStream = new MemoryStream()) {
        using (TCsvWriter Writer = new TCsvWriter(DataStream, true)) {
          Writer.Write("Hello world");
          Writer.WriteSeparator();
          Writer.Write(42);
          Writer.WriteLine();
          Writer.Write("It's me");
          Writer.WriteSeparator();
          Writer.Write(55);
          Writer.WriteLine();
        }
        byte[] Target = DataStream.ToArray();

        Assert.AreEqual($"\"Hello world\";42{Environment.NewLine}\"It's me\";55{Environment.NewLine}", Encoding.UTF8.GetString(Target));
      }
    }

    [TestMethod]
    public void CsvWriter_WriteMultiForLinux_ResultOk() {
      const string TestEOL = "\r";
      using (MemoryStream DataStream = new MemoryStream()) {
        using (TCsvWriter Writer = new TCsvWriter(DataStream, true) { EOL = TestEOL }) {
          Writer.Write("Hello world");
          Writer.WriteSeparator();
          Writer.Write(42);
          Writer.WriteLine();
          Writer.Write("It's me");
          Writer.WriteSeparator();
          Writer.Write(55);
          Writer.WriteLine();
        }
        byte[] Target = DataStream.ToArray();

        Assert.AreEqual($"\"Hello world\";42{TestEOL}\"It's me\";55{TestEOL}", Encoding.UTF8.GetString(Target));
      }
    }
  }
}
