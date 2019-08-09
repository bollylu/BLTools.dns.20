using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BLTools {

  /// <summary>
  /// Splits arguments of CommandLine. You can use either / or - as parameter prefix or nothing but the keyword.
  /// If values are specified, they are separated from the keyword by an = sign.
  /// (c) 2004-2012 Luc Bolly
  /// </summary>
  public class SplitArgs : List<ArgElement>, ISplitArgs {

    public readonly static CultureInfo DEFAULT_CULTURE = CultureInfo.InvariantCulture;

    #region Public static properties
    /// <summary>
    /// Get or Set the CultureInfo used to parse DateTime and numbers (decimal point)
    /// </summary>
    public static CultureInfo CurrentCultureInfo {
      get {
        if (_CurrentCultureInfo == null) {
          return DEFAULT_CULTURE;
        }
        return _CurrentCultureInfo;
      }
      set {
        _CurrentCultureInfo = value;
      }
    }
    private static CultureInfo _CurrentCultureInfo;

    /// <summary>
    /// Get or Set the parameters name case sensitivity
    /// </summary>
    public static bool IsCaseSensitive = false;

    /// <summary>
    /// Get or Set the separator used when reading an array from parameters (default value is ',')
    /// </summary>
    public static char Separator = ',';

    /// <summary>
    /// Separator between a key and its value
    /// </summary>
    public static char KeyValueSeparator = '=';
    #endregion Public static properties

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Creates a dictonnary of command line arguments from the args parameters list provided to Main function
    /// </summary>
    /// <param name="arrayOfValues">An array of parameters</param>
    public SplitArgs(IEnumerable<string> arrayOfValues) {
      if (arrayOfValues == null) {
        throw new ArgumentNullException("arrayOfValues", "you must pass a valid IEnumerable[] arrayOfValues argument");
      }
      _ParseValues(arrayOfValues);
    }

    /// <summary>
    /// Creates a dictionnary of command line parameters from a given command line 
    /// </summary>
    /// <param name="cmdLine">The command line</param>
    public SplitArgs(string cmdLine) {
      #region Validate parameters
      if (cmdLine == null) {
        throw new ArgumentNullException("cmdLine", "you must pass a valid string cmdLine argument");
      }
      #endregion Validate parameters

      string PreprocessedLine = cmdLine.Trim();
      List<string> CmdLineValues = new List<string>();
      StringBuilder TempStr = new StringBuilder("");

      int i = 0;
      bool InQuote = false;

      int PreprocessedLineLength = PreprocessedLine.Length;

      while (i < PreprocessedLineLength) {

        if (PreprocessedLine[i] == '"') {
          InQuote = !(InQuote);
          i++;
          continue;
        }

        if (PreprocessedLine[i] == ' ') {
          if (InQuote) {
            TempStr.Append(PreprocessedLine[i]);
            i++;
            continue;

          }

          if (!(TempStr.Length == 0)) {
            CmdLineValues.Add(TempStr.ToString());
            TempStr.Clear();
            i++;
            continue;
          }

        }

        if (PreprocessedLine[i] != '"') {
          TempStr.Append(PreprocessedLine[i]);
          i++;
          continue;
        }

      }

      if (!(TempStr.Length == 0)) {
        CmdLineValues.Add(TempStr.ToString());
      }

      _ParseValues(CmdLineValues.ToArray());

    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="otherSpliArgs"></param>
    public SplitArgs(SplitArgs otherSplitArgs) {
      this.AddRange(otherSplitArgs);
    }

    /// <summary>
    /// Create a dictionnary of url parameters from a Request.QueryString
    /// </summary>
    /// <param name="queryStringItems">A Request.QueryString</param>
    public SplitArgs(NameValueCollection queryStringItems) {
      #region === Validate parameters ===
      if (queryStringItems == null || queryStringItems.Count == 0) {
        return;
      }
      #endregion === Validate parameters ===
      foreach (string QueryStringItem in queryStringItems) {
        this.Add(new ArgElement(0, QueryStringItem, queryStringItems[QueryStringItem]));
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region Private methods
    private void _ParseValues(IEnumerable<string> arrayOfValues) {
      int Position = 0;
      foreach (string ValueItem in arrayOfValues) {
        if (ValueItem.StartsWith("/") || ValueItem.StartsWith("-")) {
          if (ValueItem.Contains(KeyValueSeparator)) {
            if (IsCaseSensitive) {
              Add(new ArgElement(Position, ValueItem.Substring(1).Before(KeyValueSeparator).TrimStart(), ValueItem.After(KeyValueSeparator).TrimEnd()));
            } else {
              Add(new ArgElement(Position, ValueItem.Substring(1).Before(KeyValueSeparator).TrimStart().ToLower(CurrentCultureInfo), ValueItem.After(KeyValueSeparator).TrimEnd()));
            }
          } else {
            if (IsCaseSensitive) {
              Add(new ArgElement(Position, ValueItem.Substring(1).Trim(), ""));
            } else {
              Add(new ArgElement(Position, ValueItem.Substring(1).Trim().ToLower(CurrentCultureInfo), ""));
            }
          }
        } else {
          if (ValueItem.Contains("=")) {
            if (IsCaseSensitive) {
              Add(new ArgElement(Position, ValueItem.Before(KeyValueSeparator).TrimStart(), ValueItem.After(KeyValueSeparator).TrimEnd()));
            } else {
              Add(new ArgElement(Position, ValueItem.Before(KeyValueSeparator).TrimStart().ToLower(CurrentCultureInfo), ValueItem.After(KeyValueSeparator).TrimEnd()));
            }
          } else {
            if (IsCaseSensitive) {
              Add(new ArgElement(Position, ValueItem.Trim(), ""));
            } else {
              Add(new ArgElement(Position, ValueItem.Trim().ToLower(CurrentCultureInfo), ""));
            }
          }
        }
        Position++;
      }
    }
    #endregion Private methods

    #region Public properties
    public new ArgElement this[int index] {
      get {
        ArgElement CurrentElement = this.FirstOrDefault(a => a.Id == index);
        if (CurrentElement == null) {
          return new ArgElement(0, "", "");
        } else {
          return CurrentElement;
        }
      }
    }
    #endregion Public properties

    public bool IsDefined(string key) {
      if (key == null || this.IsEmpty()) {
        return false;
      }
      ArgElement CurrentElement;

      if (IsCaseSensitive) {
        CurrentElement = this.FirstOrDefault(a => a.Name == key);
      } else {
        string KeyLower = key.ToLower(CurrentCultureInfo);
        CurrentElement = this.FirstOrDefault(a => a.Name.ToLower(CurrentCultureInfo) == KeyLower);
      }
      return CurrentElement != null;
    }

    #region Generic version of the GetValue

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <returns>The value</returns>
    public T GetValue<T>(string key) {
      return GetValue(key, default(T), CurrentCultureInfo);
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(string key, T defaultValue) {
      return GetValue(key, defaultValue, CurrentCultureInfo);
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(string key, T defaultValue, CultureInfo culture) {
      if (key == null || this.IsEmpty()) {
        return defaultValue;
      }

      try {
        ArgElement CurrentElement;

        if (IsCaseSensitive) {
          CurrentElement = this.FirstOrDefault(a => a.Name == key);
        } else {
          CurrentElement = this.FirstOrDefault(a => a.Name.ToLower(culture) == key.ToLower(culture));
        }

        if (CurrentElement != null) {
          return BLConverter.BLConvert<T>(CurrentElement.Value, culture, defaultValue);
        } else {
          return defaultValue;
        }
      } catch {
        return defaultValue;
      }
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <returns>The value</returns>
    public T GetValue<T>(int position) {
      return GetValue(position, default(T), CurrentCultureInfo);
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <param name="defaultValue">The default value to be returned if the position is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(int position, T defaultValue) {
      return GetValue(position, defaultValue, CurrentCultureInfo);
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(int position, T defaultValue, CultureInfo culture) {
      if (this.IsEmpty() || position < 0 || position > this.Count) {
        return defaultValue;
      }
      ArgElement CurrentElement = this[position];
      return BLConverter.BLConvert<T>(CurrentElement.Value, culture, defaultValue);

    }
    #endregion Generic version of the GetValue

  }
}
