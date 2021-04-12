using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Implementation for IFileCsv
  /// </summary>
  public class TFileCsv : AFileCsv {
    /// <summary>
    /// Create a new csv storage
    /// </summary>
    public TFileCsv() : base() { }

    /// <summary>
    /// Create a new csv storage with a filename
    /// </summary>
    /// <param name="filename">The filename to store data</param>
    public TFileCsv(string filename) : base(filename) { }

  }
}
