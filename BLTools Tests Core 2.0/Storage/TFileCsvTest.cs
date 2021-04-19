using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using BLTools.Storage.Csv;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLTools.UnitTest.Storage.file.csv {
  [TestClass]
  public class TFileCsvTest {

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
    public void TFileCsv_LoadFileHeaderFromMultipleRows_HeaderIsOk() {

      string Sourcefile = Path.GetTempFileName();
      try {
        File.WriteAllText(Sourcefile, HCsvStorageGenerator.MultipleHeaderAndDataAndFooterFile());
        IFileCsv Target = new TFileCsv(Sourcefile);
        Target.LoadHeader();

        TestContext.WriteLine(Target.ToString());

        Assert.IsFalse(Target.GetAll().IsEmpty());
        Assert.IsFalse(Target.GetHeaders().IsEmpty());
        Assert.IsTrue(Target.GetData().IsEmpty());
        Assert.IsTrue(Target.GetFooters().IsEmpty());

      } finally {
        File.Delete(Sourcefile);
      }

    }

    [TestMethod]
    public void TFileCsv_LoadFileFromMultipleRows_HeaderIsOk() {

      string Sourcefile = Path.GetTempFileName();
      try {
        File.WriteAllText(Sourcefile, HCsvStorageGenerator.MultipleHeaderAndDataAndFooterFile());
        IFileCsv Target = new TFileCsv(Sourcefile);
        Target.Load();

        TestContext.WriteLine(Target.ToString());

        Assert.IsFalse(Target.GetAll().IsEmpty());
        Assert.IsFalse(Target.GetHeaders().IsEmpty());
        Assert.IsFalse(Target.GetData().IsEmpty());
        Assert.IsFalse(Target.GetFooters().IsEmpty());

      } finally {
        File.Delete(Sourcefile);
      }

    }

    [TestMethod]
    public void TFileCsv_SaveMultipleRowsIntoFile_ReadBackOk() {

      string TestFile = Path.GetTempFileName();
      try {

        TFileCsv Storage = new TFileCsv(TestFile);

        foreach(string RawDataItem in HCsvStorageGenerator.MultipleHeaderAndDataAndFooterArray()) {
          IRowCsv Row = ARowCsv.Parse(RawDataItem);
          Storage.AddRow(Row);
        }

        Storage.Save();

        TFileCsv Target = new TFileCsv(TestFile);
        Target.Load();

        TestContext.WriteLine(Target.ToString());

        Assert.IsFalse(Target.GetAll().IsEmpty());
        Assert.IsFalse(Target.GetHeaders().IsEmpty());
        Assert.IsFalse(Target.GetData().IsEmpty());
        Assert.IsFalse(Target.GetFooters().IsEmpty());

      } finally {
        File.Delete(TestFile);
      }

    }


    [TestMethod]
    public async Task TFileCsv_AsyncLoadFileHeaderFromMultipleRows_HeaderIsOk() {

      string Sourcefile = Path.GetTempFileName();
      try {
        File.WriteAllText(Sourcefile, HCsvStorageGenerator.MultipleHeaderAndDataAndFooterFile());
        IFileCsv Target = new TFileCsv(Sourcefile);
        await Target.LoadHeaderAsync();

        TestContext.WriteLine(Target.ToString());

        Assert.IsFalse(Target.GetAll().IsEmpty());
        Assert.IsFalse(Target.GetHeaders().IsEmpty());
        Assert.IsTrue(Target.GetData().IsEmpty());
        Assert.IsTrue(Target.GetFooters().IsEmpty());

      } finally {
        File.Delete(Sourcefile);
      }

    }

    [TestMethod]
    public async Task TFileCsv_AsyncLoadFileFromMultipleRows_HeaderIsOk() {

      string Sourcefile = Path.GetTempFileName();
      try {
        File.WriteAllText(Sourcefile, HCsvStorageGenerator.MultipleHeaderAndDataAndFooterFile());
        IFileCsv Target = new TFileCsv(Sourcefile);
        await Target.LoadAsync();

        TestContext.WriteLine(Target.ToString());

        Assert.IsFalse(Target.GetAll().IsEmpty());
        Assert.IsFalse(Target.GetHeaders().IsEmpty());
        Assert.IsFalse(Target.GetData().IsEmpty());
        Assert.IsFalse(Target.GetFooters().IsEmpty());

      } finally {
        File.Delete(Sourcefile);
      }

    }


    [TestMethod]
    public async Task TFileCsv_AsyncSaveLoadMultipleRowsIntoFile_ReadBackOk() {

      string TestFile = Path.GetTempFileName();
      try {

        TFileCsv Storage = new TFileCsv(TestFile);

        foreach (string RawDataItem in HCsvStorageGenerator.MultipleHeaderAndDataAndFooterArray()) {
          IRowCsv Row = ARowCsv.Parse(RawDataItem);
          Storage.AddRow(Row);
        }

        await Storage.SaveAsync();

        TFileCsv Target = new TFileCsv(TestFile);
        await Target.LoadAsync();

        TestContext.WriteLine(Target.ToString());

        Assert.IsFalse(Target.GetAll().IsEmpty());
        Assert.IsFalse(Target.GetHeaders().IsEmpty());
        Assert.IsFalse(Target.GetData().IsEmpty());
        Assert.IsFalse(Target.GetFooters().IsEmpty());

      } finally {
        File.Delete(TestFile);
      }

    }

  }
}
