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
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "")]
  public class SplitArgs : ISplitArgs {

    /// <summary>
    /// The keys/values internal storage
    /// </summary>
    protected List<IArgElement> _Items = new List<IArgElement>();

    /// <summary>
    /// Default culture for conversions
    /// </summary>
    public readonly static CultureInfo DEFAULT_CULTURE = CultureInfo.InvariantCulture;

    #region Public properties
    /// <inheritdoc/>
    public CultureInfo CurrentCultureInfo {
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
    private CultureInfo _CurrentCultureInfo;

    /// <inheritdoc/>
    public bool IsCaseSensitive { get; set; } = false;

    /// <inheritdoc/>
    public char Separator { get; set; } = ';';

    /// <inheritdoc/>
    public char KeyValueSeparator { get; set; } = '=';
    #endregion Public properties

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Creates an empty dictonnary of command line arguments
    /// </summary>
    public SplitArgs() {
    }

    /// <summary>
    /// Creates a dictonnary of command line arguments from the args parameters list provided to Main function
    /// </summary>
    /// <param name="arrayOfValues">An array of parameters</param>
    [Obsolete("Use empty constructor + Parse(args)")]
    public SplitArgs(IEnumerable<string> arrayOfValues) {
      if (arrayOfValues == null) {
        throw new ArgumentNullException(nameof(arrayOfValues), "you must pass a valid IEnumerable[] arrayOfValues argument");
      }
      Parse(arrayOfValues);
    }

    /// <summary>
    /// Creates a dictionnary of command line parameters from a given command line 
    /// </summary>
    /// <param name="cmdLine">The command line</param>
    [Obsolete("Use empty constructor + Parse(cmdLine)")]
    public SplitArgs(string cmdLine) {
      Parse(cmdLine);
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="otherSplitArgs"></param>
    public SplitArgs(ISplitArgs otherSplitArgs) {
      Separator = otherSplitArgs.Separator;
      KeyValueSeparator = otherSplitArgs.KeyValueSeparator;
      IsCaseSensitive = otherSplitArgs.IsCaseSensitive;
      CurrentCultureInfo = otherSplitArgs.CurrentCultureInfo;
      _Items.AddRange(otherSplitArgs.GetAll());
    }

    /// <summary>
    /// Create a dictionnary of url parameters from a Request.QueryString
    /// </summary>
    /// <param name="queryStringItems">A Request.QueryString</param>
    [Obsolete("Use empty constructor + Parse(queryStringItems)")]
    public SplitArgs(NameValueCollection queryStringItems) {
      Parse(queryStringItems);
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- Parse input --------------------------------------------
    /// <inheritdoc/>
    public void Parse(string cmdLine) {
      #region Validate parameters
      if (cmdLine is null) {
        throw new ArgumentNullException(nameof(cmdLine), "you must pass a valid string cmdLine argument");
      }
      #endregion Validate parameters

      string PreprocessedLine = cmdLine.Trim();
      List<string> CmdLineValues = new List<string>();
      StringBuilder TempStr = new StringBuilder();

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

      if (TempStr.Length != 0) {
        CmdLineValues.Add(TempStr.ToString());
      }

      Parse(CmdLineValues.ToArray());
    }

    /// <inheritdoc/>
    public void Parse(IEnumerable<string> arrayOfValues) {
      int Position = 0;
      foreach (string ValueItem in arrayOfValues) {
        if (ValueItem.StartsWith("/") || ValueItem.StartsWith("-")) {
          if (ValueItem.Contains(KeyValueSeparator)) {
            if (IsCaseSensitive) {
              _Items.Add(new ArgElement(Position, ValueItem.Substring(1).Before(KeyValueSeparator).TrimStart(), ValueItem.After(KeyValueSeparator).TrimEnd()));
            } else {
              _Items.Add(new ArgElement(Position, ValueItem.Substring(1).Before(KeyValueSeparator).TrimStart().ToLower(CurrentCultureInfo), ValueItem.After(KeyValueSeparator).TrimEnd()));
            }
          } else {
            if (IsCaseSensitive) {
              _Items.Add(new ArgElement(Position, ValueItem.Substring(1).Trim(), ""));
            } else {
              _Items.Add(new ArgElement(Position, ValueItem.Substring(1).Trim().ToLower(CurrentCultureInfo), ""));
            }
          }
        } else {
          if (ValueItem.Contains("=")) {
            if (IsCaseSensitive) {
              _Items.Add(new ArgElement(Position, ValueItem.Before(KeyValueSeparator).TrimStart(), ValueItem.After(KeyValueSeparator).TrimEnd()));
            } else {
              _Items.Add(new ArgElement(Position, ValueItem.Before(KeyValueSeparator).TrimStart().ToLower(CurrentCultureInfo), ValueItem.After(KeyValueSeparator).TrimEnd()));
            }
          } else {
            if (IsCaseSensitive) {
              _Items.Add(new ArgElement(Position, ValueItem.Trim(), ""));
            } else {
              _Items.Add(new ArgElement(Position, ValueItem.Trim().ToLower(CurrentCultureInfo), ""));
            }
          }
        }
        Position++;
      }
    }

    /// <inheritdoc/>
    public void Parse(NameValueCollection queryStringItems) {
      #region === Validate parameters ===
      if (queryStringItems == null || queryStringItems.Count == 0) {
        return;
      }
      #endregion === Validate parameters ===
      foreach (string QueryStringItem in queryStringItems) {
        _Items.Add(new ArgElement(0, QueryStringItem, queryStringItems[QueryStringItem]));
      }
    }
    #endregion --- Parse input --------------------------------------------

    #region --- Elements management --------------------------------------------
    /// <inheritdoc/>
    public void Clear() {
      _Items.Clear();
    }

    /// <inheritdoc/>
    public int Count() {
      return _Items.Count();
    }

    /// <inheritdoc/>
    public void Add(IArgElement element) {
      _Items.Add(element);
    }


    /// <inheritdoc/>
    public IArgElement this[int index] {
      get {
        IArgElement CurrentElement = _Items.FirstOrDefault(a => a.Id == index);
        if (CurrentElement == null) {
          return new ArgElement(0, "", "");
        } else {
          return CurrentElement;
        }
      }
    }

    /// <inheritdoc/>
    public IArgElement this[string key] {
      get {
        IArgElement CurrentElement = _Items.FirstOrDefault(a => a.Name == key);
        if (CurrentElement is null) {
          return new ArgElement(0, "", "");
        } else {
          return CurrentElement;
        }
      }
    }

    /// <inheritdoc/>
    public IEnumerable<IArgElement> GetAll() {
      if (_Items.IsEmpty()) {
        yield break;
      }

      foreach(IArgElement ArgElementItem in _Items) {
        yield return ArgElementItem;
      }
    }

    #endregion --- Elements management --------------------------------------------

    #region --- Tests on keys/values --------------------------------------------
    /// <inheritdoc/>
    public bool IsDefined(string key) {
      if (key == null || _Items.IsEmpty()) {
        return false;
      }
      IArgElement CurrentElement;

      if (IsCaseSensitive) {
        CurrentElement = _Items.FirstOrDefault(a => a.Name == key);
      } else {
        string KeyLower = key.ToLower(CurrentCultureInfo);
        CurrentElement = _Items.FirstOrDefault(a => a.Name.ToLower(CurrentCultureInfo) == KeyLower);
      }
      return CurrentElement != null;
    }

    /// <inheritdoc/>
    public bool HasValue(string key) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(key) || !IsDefined(key)) {
        return false;
      }
      #endregion === Validate parameters ===

      IArgElement CurrentElement;

      if (IsCaseSensitive) {
        CurrentElement = _Items.FirstOrDefault(a => a.Name == key);
      } else {
        CurrentElement = _Items.FirstOrDefault(a => a.Name.ToLower(CurrentCultureInfo) == key.ToLower(CurrentCultureInfo));

      }
      return CurrentElement?.HasValue() ?? false;

    }

    /// <inheritdoc/>
    public bool HasValue(int index) {
      if (_Items.IsEmpty()) {
        return false;
      }

      if (index < 0 || index > (_Items.Count - 1)) {
        return false;
      }

      IArgElement CurrentElement = _Items[index];

      return CurrentElement?.HasValue() ?? false;

    }

    /// <inheritdoc/>
    public bool Any() {
      return _Items.Any();
    }

    /// <inheritdoc/>
    public bool IsEmpty() {
      return _Items.IsEmpty();
    }
    #endregion --- Tests on keys/values --------------------------------------------

    #region Generic version of the GetValue

    /// <inheritdoc/>
    public T GetValue<T>(string key) {
      return GetValue(key, default(T), CurrentCultureInfo);
    }

    /// <inheritdoc/>
    public T GetValue<T>(string key, T defaultValue) {
      return GetValue(key, defaultValue, CurrentCultureInfo);
    }

    /// <inheritdoc/>
    public T GetValue<T>(string key, T defaultValue, CultureInfo culture) {
      if (key is null || _Items.IsEmpty()) {
        return defaultValue;
      }

      try {
        IArgElement CurrentElement;

        if (IsCaseSensitive) {
          CurrentElement = _Items.FirstOrDefault(a => a.Name == key);
        } else {
          CurrentElement = _Items.FirstOrDefault(a => a.Name.ToLower(culture) == key.ToLower(culture));
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

    /// <inheritdoc/>
    public T GetValue<T>(int position) {
      return GetValue(position, default(T), CurrentCultureInfo);
    }

    /// <inheritdoc/>
    public T GetValue<T>(int position, T defaultValue) {
      return GetValue(position, defaultValue, CurrentCultureInfo);
    }

    /// <inheritdoc/>
    public T GetValue<T>(int position, T defaultValue, CultureInfo culture) {
      if (_Items.IsEmpty() || position.IsOutsideRange(0, _Items.Count)) {
        return defaultValue;
      }
      IArgElement CurrentElement = _Items[position];
      return BLConverter.BLConvert<T>(CurrentElement.Value, culture, defaultValue);

    }
    #endregion Generic version of the GetValue

  }
}
