using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Implementation of a data row
  /// </summary>
  public class TRowCsvData : ARowCsv {

    /// <summary>
    /// Create a new data row
    /// </summary>
    public TRowCsvData() : base(ERowCsvType.Data) { }

  }
}
