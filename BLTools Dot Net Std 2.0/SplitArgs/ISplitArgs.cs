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

    #region --- Public properties ------------------------------------------------------------------------------
    /// <summary>
    /// Get or Set the CultureInfo used to parse DateTime and numbers (decimal point)
    /// </summary>
    CultureInfo CurrentCultureInfo { get; }

    /// <summary>
    /// Get or Set the parameters name case sensitivity
    /// </summary>
    bool IsCaseSensitive { get; }

    /// <summary>
    /// Get or Set the separator used when reading an array from parameters (default value is ',')
    /// </summary>
    char Separator { get; }

    /// <summary>
    /// Separator between a key and its value
    /// </summary>
    char KeyValueSeparator { get; } 
    #endregion --- Public properties ---------------------------------------------------------------------------

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
    /// <returns>true if any value exists, false otherwise</returns>
    bool HasValue(string key);

    /// <summary>
    /// Indicates if the key exists and has any associated value
    /// </summary>
    /// <param name="index">The index of the key to check</param>
    /// <returns>true if any value exists, false otherwise</returns>
    bool HasValue(int index);

    /// <summary>
    /// Indicates if no keys at all are in the object
    /// </summary>
    /// <returns>true if the object contains no key at all, false otherwise</returns>
    bool IsEmpty();

    /// <summary>
    /// Indicates if any key is the object
    /// </summary>
    /// <returns>true if the object contains at least one key (with or without value), false otherwise</returns>
    bool Any();

    /// <summary>
    /// Returns all the elements
    /// </summary>
    /// <returns>The enumeration of current elements</returns>
    IEnumerable<IArgElement> GetAll();

    /// <summary>
    /// Retrieve one element by index
    /// </summary>
    /// <param name="position">The position in the source string used to parse (starts at 0)</param>
    /// <returns>The element or null</returns>
    IArgElement this[int position] { get; }

    /// <summary>
    /// Retrieve one element by key
    /// </summary>
    /// <param name="key">The key name in the source string used to parse</param>
    /// <returns>The element or null</returns>
    IArgElement this[string key] { get; }

    #endregion --- Keys and values --------------------------------------------

    #region --- Get the values --------------------------------------------
    /// <summary>
    /// Retrieve to typed value associated with a key
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="key">The key to retrieve</param>
    /// <returns></returns>
    T GetValue<T>(string key);

    /// <summary>
    /// Retrieve to typed value associated with a key with a optional default value, using current culture info
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid or conversion fails</param>
    /// <returns>The value</returns>
    T GetValue<T>(string key, T defaultValue);

    /// <summary>
    /// Retrieve to typed value associated with a key with a optional default value, using a specific culture info
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid or conversion fails</param>
    /// <param name="cultureInfo">The CultureInfo for the conversion</param>
    /// <returns>The value</returns>
    T GetValue<T>(string key, T defaultValue, CultureInfo cultureInfo);


    /// <summary>
    /// Retrieve to typed value at given position (0 based)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <returns>The value</returns>
    T GetValue<T>(int position);

    /// <summary>
    /// Retrieve to typed value at given position (0 based), with an optional default value, using current culture info
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <param name="defaultValue">The default value to be returned if the position is invalid or conversion fails</param>
    /// <returns>The value</returns>
    T GetValue<T>(int position, T defaultValue);

    /// <summary>
    /// Retrieve to typed value at given position (0 based), with an optional default value, using a specific culture info
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid or conversion fails</param>
    /// <param name="cultureInfo">The CultureInfo for the conversion</param>
    /// <returns>The value</returns>
    T GetValue<T>(int position, T defaultValue, CultureInfo cultureInfo);
    #endregion --- Get the values --------------------------------------------

    #region --- Elements management --------------------------------------------
    /// <summary>
    /// Clear all the key/value pairs
    /// </summary>
    void Clear();

    /// <summary>
    /// Manually add a new ArgElement
    /// </summary>
    /// <param name="element">The element to add</param>
    void Add(IArgElement element);

    /// <summary>
    /// Count the items
    /// </summary>
    /// <returns>The count of items</returns>
    int Count();
    #endregion --- Elements management --------------------------------------------

  }
}
