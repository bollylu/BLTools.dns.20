using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Implementation of a header row
  /// </summary>
  public class TRowCsvHeader : ARowCsv {

    /// <summary>
    /// Create a new header row
    /// </summary>
    public TRowCsvHeader() : base(ERowCsvType.Header) { }

    /// <summary>
    /// Create a new header row with an Id and a string value
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public TRowCsvHeader(string id, string value) : base(ERowCsvType.Header) {
      Id = id;
      Set(value);
    }

  }
}
