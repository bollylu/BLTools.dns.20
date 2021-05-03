using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools {
  
  /// <summary>
  /// Allows the reading or saving in xml
  /// </summary>
  public interface IFileXml {

    /// <summary>
    /// The filename for save/load
    /// </summary>
    string Filename { get; }

    /// <summary>
    /// The encoding for save/load
    /// </summary>
    Encoding FileEncoding { get; }

    /// <summary>
    /// The root of the file content
    /// </summary>
    XElement Root { get; }

    #region --- Save synchronously --------------------------------------------
    /// <summary>
    /// Save the item in Filename
    /// </summary>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool Save(bool overwrite = true);

    /// <summary>
    /// Save the XElement in an xml file
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="root">The root element to save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool Save(string filename, XElement root, bool overwrite = true);

    /// <summary>
    /// Save the Root in an xml file
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool Save(string filename, bool overwrite = true);

    /// <summary>
    /// Save the XElement in Filename
    /// </summary>
    /// <param name="root">The root element to save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool Save(XElement root, bool overwrite = true);

    #endregion --- Save synchronously --------------------------------------------

    /// <summary>
    /// Save the item in Filename asynchronously
    /// </summary>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsync(bool overwrite = true);

    /// <summary>
    /// Save the item in an xml file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsync(string filename, bool overwrite = true);

    /// <summary>
    /// Save the XElement in an xml file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="root">The root element to save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsync(string filename, XElement root, bool overwrite = true);

    /// <summary>
    /// Save the XElement in Filename asynchronously
    /// </summary>
    /// <param name="root">The root element to save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsync(XElement root, bool overwrite = true);

    /// <summary>
    /// Load the content of Filename
    /// </summary>
    /// <returns>The root of the xml file, null if anything wrong</returns>
    XElement Load();

    /// <summary>
    /// Load the content of an xml file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>The root of the xml file, null if anything wrong</returns>
    XElement Load(string filename);

    /// <summary>
    /// Load the content of Filename asynchronously
    /// </summary>
    /// <returns>The root of the xml file, null if anything wrong</returns>
    Task<XElement> LoadAsync();

    /// <summary>
    /// Load the content of an xml file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>The root of the xml file, null if anything wrong</returns>
    Task<XElement> LoadAsync(string filename);
  }
}
