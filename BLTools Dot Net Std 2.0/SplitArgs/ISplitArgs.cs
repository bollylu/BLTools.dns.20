using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLTools {
  /// <summary>
  /// Used to parse a command line or similar into parameters
  /// </summary>
  public interface ISplitArgs {

    /// <summary>
    /// Parse the array of string values from a command line
    /// </summary>
    /// <param name="arrayOfValues"></param>
    void Parse(IEnumerable<string> arrayOfValues);

    /// <summary>
    /// Indicates if a key is defined
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>true if the key is defined, false otherwise</returns>
    bool IsDefined(string key);

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

    ArgElement this[int index] { get; }

    

  }
}
