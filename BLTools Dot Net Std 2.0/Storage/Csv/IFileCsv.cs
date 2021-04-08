using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {
  
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
    bool Save(string filename, bool overwrite = true);

    /// <summary>
    /// Save the item in an csv file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsync(string filename, bool overwrite = true);

    /// <summary>
    /// Load the content of an csv file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool Load(string filename);

    /// <summary>
    /// Load the content of an csv file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadAsync(string filename);

    /// <summary>
    /// Load the header of the csv file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool LoadHeader(string filename);

    /// <summary>
    /// Load the header of the csv file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadHeaderAsync(string filename);
  }
}
