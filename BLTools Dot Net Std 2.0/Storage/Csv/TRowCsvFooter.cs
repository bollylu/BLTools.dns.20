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

  }
}
