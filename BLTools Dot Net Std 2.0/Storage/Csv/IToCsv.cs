using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// Convert item to/from csv
  /// </summary>
  public interface IToCsv {
    /// <summary>
    /// Transform the item into csv
    /// </summary>
    /// <returns>The csv representation of the item</returns>
    string ToCsv();

    /// <summary>
    /// Fill the item from a csv source
    /// </summary>
    /// <param name="source">The data source in csv format</param>
    void FromCsv(string source);
  }
}
