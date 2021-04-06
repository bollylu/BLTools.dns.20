using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  
  /// <summary>
  /// Allows the reading or saving in csv
  /// </summary>
  public interface IFileCsv {

    /// <summary>
    /// Save the item in an csv file
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool SaveCsv(string filename, bool overwrite = true);

    /// <summary>
    /// Save the item in an csv file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveCsvAsync(string filename, bool overwrite = true);

    /// <summary>
    /// Load the content of an csv file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool LoadCsv(string filename);

    /// <summary>
    /// Load the content of an csv file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadCsvAsync(string filename);
  }
}
