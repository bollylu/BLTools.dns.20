using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace BLTools {
  /// <summary>
  /// Used to parse a command line or similar into parameters
  /// </summary>
  public interface ISplitArgs {

    #region --- Parse input --------------------------------------------
    /// <summary>
    /// Parse an array of string (command line alike, i.e. prog p1=v p2=vv)
    /// </summary>
    /// <param name="arrayOfValues"></param>
    void Parse(IEnumerable<string> arrayOfValues);

    /// <summary>
    /// Parse the command line
    /// </summary>
    /// <param name="cmdLine"></param>
    void Parse(string cmdLine);

    /// <summary>
    /// Parse a NameValueCollection (e.g. from url)
    /// </summary>
    /// <param name="queryStringItems"></param>
    void Parse(NameValueCollection queryStringItems);
    #endregion --- Parse input --------------------------------------------

    #region --- Keys and values --------------------------------------------
    /// <summary>
    /// Indicate if a key was part of the arguments (whatever it has a value or not)
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>true if the key is defined, false otherwise</returns>
    bool IsDefined(string key);

    /// <summary>
    /// Indicates if the key exists and has any associated value
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns><see langword="true"/>if any value exists, false if null</returns>
    bool HasValue(string key);

    /// <summary>
    /// Indicates if the key exists and has any associated value
    /// </summary>
    /// <param name="index">The index of the key to check</param>
    /// <returns><see langword="true"/>if any value exists, false if null</returns>
    bool HasValue(int index);
    #endregion --- Keys and values --------------------------------------------

    #region --- Get the values --------------------------------------------
    /// <summary>
    /// Retrieve to typed value associated with a key
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="key">The key to retrieve</param>
    /// <returns></returns>
    T GetValue<T>(string key);

    T GetValue<T>(string key, T defaultValue);
    T GetValue<T>(string key, T defaultValue, CultureInfo cultureInfo);

    T GetValue<T>(int pos);
    T GetValue<T>(int pos, T defaultValue);
    T GetValue<T>(int pos, T defaultValue, CultureInfo cultureInfo); 
    #endregion --- Get the values --------------------------------------------

    /// <summary>
    /// Retrieve the argument at specific index
    /// </summary>
    /// <param name="index">The position of the key/value pair in the input data</param>
    /// <returns></returns>
    ArgElement this[int index] { get; }

    /// <summary>
    /// Clear all the key/value pairs
    /// </summary>
    void Clear();

  }
}
