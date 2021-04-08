using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  
  /// <summary>
  /// Allows the reading or saving in xml
  /// </summary>
  public interface IFileXml {

    /// <summary>
    /// Save the item in an xml file
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool SaveXml(string filename, bool overwrite = true);

    /// <summary>
    /// Save the item in an xml file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveXmlAsync(string filename, bool overwrite = true);

    /// <summary>
    /// Load the content of an xml file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool LoadXml(string filename);

    /// <summary>
    /// Load the content of an xml file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadXmlAsync(string filename);
  }
}
