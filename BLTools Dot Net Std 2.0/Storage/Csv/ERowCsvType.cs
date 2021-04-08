using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {
  
  /// <summary>
  /// Defines the type of row
  /// </summary>
  public enum ERowCsvType {
    /// <summary>
    /// Unknown or invalid row
    /// </summary>
    Unknown,
    /// <summary>
    /// A header row, describing the csv data
    /// </summary>
    Header,
    /// <summary>
    /// The real csv data
    /// </summary>
    Data,
    /// <summary>
    /// A footer row, with summarizing data, etc
    /// </summary>
    Footer,
    /// <summary>
    /// No row type
    /// </summary>
    None
  }
}
