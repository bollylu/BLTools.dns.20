using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Implementation of a footer row
  /// </summary>
  public class TRowCsvFooter : ARowCsv {

    /// <summary>
    /// Create a new footer row
    /// </summary>
    public TRowCsvFooter() : base(ERowCsvType.Footer) { }

    /// <summary>
    /// Create a new footer row with an Id and a string value
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public TRowCsvFooter(string id, string value) : base(ERowCsvType.Footer) {
      Id = id;
      Set(value);
    }
  }
}
