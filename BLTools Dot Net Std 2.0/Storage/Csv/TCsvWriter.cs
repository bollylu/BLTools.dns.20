using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Writer for Csv
  /// </summary>
  public class TCsvWriter : StreamWriter {

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="outputStream">The stream to write to</param>
    public TCsvWriter(Stream outputStream) : base(outputStream, Encoding.UTF8) { }

    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="outputStream">The stream to write to</param>
    /// <param name="encoding">the encoding of the stream</param>
    public TCsvWriter(Stream outputStream, Encoding encoding) : base(outputStream, encoding) { } 
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <summary>
    /// Write a row in the stream
    /// </summary>
    /// <param name="row">The IRowCsv to write</param>
    public void WriteRow(IRowCsv row) {
      if (row is null) {
        return;
      }
      WriteLine(row.Render());
    }

  }
}
