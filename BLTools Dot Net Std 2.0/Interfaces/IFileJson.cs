using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  
  /// <summary>
  /// Allows the reading or saving in json
  /// </summary>
  public interface IFileJson {

    /// <summary>
    /// Save the item in an json file
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool SaveJson(string filename, bool overwrite = true);

    /// <summary>
    /// Save the item in an json file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveJsonAsync(string filename, bool overwrite = true);

    /// <summary>
    /// Load the content of an json file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool LoadJson(string filename);

    /// <summary>
    /// Load the content of an json file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadJsonAsync(string filename);
  }
}
