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
  public class TCsvWriter : BinaryWriter {

    /// <summary>
    /// The encoding for the reader
    /// </summary>
    public Encoding WriterEncoding { get; private set; }

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

    /// <summary>
    /// Write a row in the stream
    /// </summary>
    /// <param name="row">The IRowCsv to write</param>
    public void WriteRow(IRowCsv row) {
      if (row is null) {
        return;
      }
      Write(WriterEncoding.GetBytes($"{row.Render()}{Environment.NewLine}"));
    }

  }
}
