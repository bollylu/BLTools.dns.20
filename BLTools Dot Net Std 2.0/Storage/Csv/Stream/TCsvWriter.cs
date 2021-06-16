using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Writer for Csv
  /// </summary>
  public class TCsvWriter : BinaryWriter {

    #region --- Public properties ------------------------------------------------------------------------------


#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <summary>
    /// The encoding for the reader
    /// </summary>
    public Encoding WriterEncoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// The end-of-line definition (default to Environment.NewLine)
    /// </summary>
    public string EOL { get; set; } = Environment.NewLine;
#else
    /// <summary>
    /// The encoding for the reader
    /// </summary>
    public Encoding WriterEncoding { get; init; } = Encoding.UTF8;

    /// <summary>
    /// The end-of-line definition (default to Environment.NewLine)
    /// </summary>
    public string EOL { get; init; } = Environment.NewLine;
#endif

    #endregion --- Public properties ---------------------------------------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="outputStream">The stream to write to</param>
    /// <param name="leaveOpen">tue to leave the stream open when closing the writer</param>
    public TCsvWriter(Stream outputStream, bool leaveOpen = true) : base(outputStream, Encoding.UTF8, leaveOpen) {
      WriterEncoding = Encoding.UTF8;
    }

    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="outputStream">The stream to write to</param>
    /// <param name="encoding">the encoding of the stream</param>
    /// <param name="leaveOpen">tue to leave the stream open when closing the writer</param>
    public TCsvWriter(Stream outputStream, Encoding encoding, bool leaveOpen = true) : base(outputStream, encoding, leaveOpen) {
      WriterEncoding = encoding;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- ICsvRow --------------------------------------------
    /// <summary>
    /// Write a full ICsvRow + EOL
    /// </summary>
    /// <param name="row">The IRowCsv to write</param>
    public void WriteRow(IRowCsv row) {
      if (row is null) {
        return;
      }
      Write(WriterEncoding.GetBytes($"{row.Render()}"));
      WriteLine();
    }

    /// <summary>
    /// Write a row type (i.e. Header, Data, Footer)
    /// </summary>
    public void Write(ERowCsvType RowType) {
      Write(WriterEncoding.GetBytes(RowType.ToString()));
    }
    #endregion --- ICsvRow --------------------------------------------

    #region --- Technical --------------------------------------------
    /// <summary>
    /// Write a separator
    /// </summary>
    public void WriteSeparator() {
      Write((byte)ARowCsv.SEPARATOR);
    }

    /// <summary>
    /// Write a EOL
    /// </summary>
    public void WriteLine() {
      Write(EOL.ToCharArray());
    }
    #endregion --- Technical --------------------------------------------

    #region --- Simple item --------------------------------------------
    /// <summary>
    /// Write a string item
    /// </summary>
    /// <param name="data">The string item</param>
    public override void Write(string data) {
      if (data is null) {
        return;
      }
      Write(WriterEncoding.GetBytes(data.WithQuotes()));
    }

    /// <summary>
    /// Write an int item
    /// </summary>
    /// <param name="data">The int item</param>
    public override void Write(int data) {
      Write(WriterEncoding.GetBytes(data.ToString()));
    }

    /// <summary>
    /// Write a long item
    /// </summary>
    /// <param name="data">The long item</param>
    public override void Write(long data) {
      Write(WriterEncoding.GetBytes(data.ToString()));
    }

    /// <summary>
    /// Write a float item using invariant culture info
    /// </summary>
    /// <param name="data">The float item</param>
    public override void Write(float data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a float item using culture info
    /// </summary>
    /// <param name="data">The float item</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(float data, CultureInfo cultureInfo) {
      Write(WriterEncoding.GetBytes(data.ToString(cultureInfo)));
    }

    /// <summary>
    /// Write a double item using invariant culture info
    /// </summary>
    /// <param name="data">The double item</param>
    public override void Write(double data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a double item using culture info
    /// </summary>
    /// <param name="data">The double item</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(double data, CultureInfo cultureInfo) {
      Write(WriterEncoding.GetBytes(data.ToString(cultureInfo)));
    }

    /// <summary>
    /// Write a decimal item using invariant culture info
    /// </summary>
    /// <param name="data">The decimal item</param>
    public override void Write(decimal data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a decimal item using culture info
    /// </summary>
    /// <param name="data">The decimal item</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(decimal data, CultureInfo cultureInfo) {
      Write(WriterEncoding.GetBytes(data.ToString(cultureInfo)));
    }

    /// <summary>
    /// Write a DateTime item using format ISO 8601
    /// </summary>
    /// <param name="data">The DateTime item</param>
    public void Write(DateTime data) {
      Write(WriterEncoding.GetBytes(data.ToString("s")));
    }

    /// <summary>
    /// Write a DateTime item using culture info
    /// </summary>
    /// <param name="data">The DateTime item</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(DateTime data, CultureInfo cultureInfo) {
      Write(WriterEncoding.GetBytes(data.ToString(cultureInfo)));
    }

    /// <summary>
    /// Write a bool item using invariant culture info
    /// </summary>
    /// <param name="data">The bool item</param>
    public override void Write(bool data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a bool item using culture info
    /// </summary>
    /// <param name="data">The bool item</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(bool data, CultureInfo cultureInfo) {
      Write(WriterEncoding.GetBytes(data.ToString(cultureInfo)));
    }
    #endregion --- Simple item --------------------------------------------

    #region --- IEnumerable of items --------------------------------------------
    /// <summary>
    /// Write a sequence of int items
    /// </summary>
    /// <param name="data">The enumerable int items</param>
    public void Write(IEnumerable<int> data) {
      if (data is null || data.IsEmpty()) {
        return;
      }
      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString()));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of long items
    /// </summary>
    /// <param name="data">The enumerable long items</param>
    public void Write(IEnumerable<long> data) {
      if (data is null || data.IsEmpty()) {
        return;
      }
      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString()));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of string items
    /// </summary>
    /// <param name="data">The enumerable string items</param>
    public void Write(IEnumerable<string> data) {
      if (data is null || data.IsEmpty()) {
        return;
      }
      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data);
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of float items with invariant culture info
    /// </summary>
    /// <param name="data">The enumerable float items</param>
    public void Write(IEnumerable<float> data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a sequence of float items with culture info
    /// </summary>
    /// <param name="data">The enumerable float items</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(IEnumerable<float> data, CultureInfo cultureInfo) {
      if (data is null || data.IsEmpty()) {
        return;
      }

      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString(cultureInfo)));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of double items with invariant culture info
    /// </summary>
    /// <param name="data">The enumerable double items</param>
    public void Write(IEnumerable<double> data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a sequence of double items with culture info
    /// </summary>
    /// <param name="data">The enumerable double items</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(IEnumerable<double> data, CultureInfo cultureInfo) {
      if (data is null || data.IsEmpty()) {
        return;
      }

      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString(cultureInfo)));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of decimal items with invariant culture info
    /// </summary>
    /// <param name="data">The enumerable decimal items</param>
    public void Write(IEnumerable<decimal> data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a sequence of decimal items with culture info
    /// </summary>
    /// <param name="data">The enumerable decimal items</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(IEnumerable<decimal> data, CultureInfo cultureInfo) {
      if (data is null || data.IsEmpty()) {
        return;
      }

      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString(cultureInfo)));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of DateTime items using format ISO 8601
    /// </summary>
    /// <param name="data">The enumerable DateTime items</param>
    public void Write(IEnumerable<DateTime> data) {
      if (data is null || data.IsEmpty()) {
        return;
      }

      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString("s", CultureInfo.InvariantCulture)));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of DateTime items using culture info
    /// </summary>
    /// <param name="data">The enumerable DateTime items</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(IEnumerable<DateTime> data, CultureInfo cultureInfo) {
      if (data is null || data.IsEmpty()) {
        return;
      }

      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString(cultureInfo)));
      Write(WriterEncoding.GetBytes(Joined));
    }

    /// <summary>
    /// Write a sequence of bool items with Invariant culture info
    /// </summary>
    /// <param name="data">The enumerable bool items</param>
    public void Write(IEnumerable<bool> data) {
      Write(data, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Write a sequence of bool items with culture info
    /// </summary>
    /// <param name="data">The enumerable bool items</param>
    /// <param name="cultureInfo">The culture info to use</param>
    public void Write(IEnumerable<bool> data, CultureInfo cultureInfo) {
      if (data is null || data.IsEmpty()) {
        return;
      }

      string Joined = string.Join($"{ARowCsv.SEPARATOR}", data.Select(x => x.ToString(cultureInfo)));
      Write(WriterEncoding.GetBytes(Joined));
    }
    #endregion --- IEnumerable of items --------------------------------------------

  }
}
