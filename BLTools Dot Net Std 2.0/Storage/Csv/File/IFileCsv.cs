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
    /// The filename for save/load
    /// </summary>
    string Filename { get; }

    /// <summary>
    /// The encoding for save/load
    /// </summary>
    Encoding FileEncoding { get; }

    #region --- I/O synchronous --------------------------------------------
    /// <summary>
    /// Save the item in an csv file using the internal filename
    /// </summary>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool Save(bool overwrite = true);

    /// <summary>
    /// Save the item in an csv file
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    bool SaveAs(string filename, bool overwrite = true);

    /// <summary>
    /// Save the item in an csv file asynchronously using the internal filename
    /// </summary>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsync(bool overwrite = true);

    /// <summary>
    /// Save the item in an csv file asynchronously
    /// </summary>
    /// <param name="filename">The filename for the save</param>
    /// <param name="overwrite">true to overwrite any existing file, false otherwise</param>
    /// <returns>true if save is successfull, false otherwise</returns>
    Task<bool> SaveAsAsync(string filename, bool overwrite = true);

    /// <summary>
    /// Load the content of an csv file using the internal filename
    /// </summary>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool Load();

    /// <summary>
    /// Load the content of an csv file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool Load(string filename);



    /// <summary>
    /// Load the header of the csv file using the internal filename
    /// </summary>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool LoadHeader();

    /// <summary>
    /// Load the header of the csv file
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    bool LoadHeader(string filename); 
    #endregion --- I/O synchronous --------------------------------------------

    #region --- I/O asynchronous --------------------------------------------
    /// <summary>
    /// Load the content of an csv file asynchronously using the internal filename
    /// </summary>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadAsync();

    /// <summary>
    /// Load the content of an csv file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadAsync(string filename);

    /// <summary>
    /// Load the header of the csv file asynchronously using the internal filename
    /// </summary>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadHeaderAsync();

    /// <summary>
    /// Load the header of the csv file asynchronously
    /// </summary>
    /// <param name="filename">The file to load</param>
    /// <returns>true if load is successfull, false otherwise</returns>
    Task<bool> LoadHeaderAsync(string filename); 
    #endregion --- I/O asynchronous --------------------------------------------

    #region --- Memory content --------------------------------------------
    /// <summary>
    /// Obtain all the rows from csv file
    /// </summary>
    /// <returns></returns>
    IRowCsv[] GetAll();

    /// <summary>
    /// Obtain all the headers
    /// </summary>
    /// <returns></returns>
    IRowCsv[] GetHeaders();

    /// <summary>
    /// Obtain all the data
    /// </summary>
    /// <returns></returns>
    IRowCsv[] GetData();

    /// <summary>
    /// Obtain all the footers 
    /// </summary>
    /// <returns></returns>
    IRowCsv[] GetFooters();

    /// <summary>
    /// Obtain a specific row
    /// </summary>
    /// <param name="rowType">Type of row (i.e. header, data, footer)</param>
    /// <param name="Id">The id of the row (case insensitive)</param>
    /// <returns>The requested row or null if the row doesn't exist</returns>
    IRowCsv GetRow(ERowCsvType rowType, string Id);

    /// <summary>
    /// Add a ICsvRow to the file content in memory
    /// </summary>
    /// <param name="row">The ICsvRow to add</param>
    void AddRow(IRowCsv row); 
    #endregion --- Memory content --------------------------------------------
  }
}
