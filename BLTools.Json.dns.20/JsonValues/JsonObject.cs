using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections;
using static BLTools.Text.TextBox;
using System.Security;

namespace BLTools.Json {
  public class JsonObject : IJsonValue, IEnumerable<IJsonPair> {

    public readonly static JsonObject Empty = new JsonObject();

    #region --- Inner data access --------------------------------------------
    private readonly TJsonPairCollection _Items = new TJsonPairCollection();
    private readonly object _JsonLock = new object();
    #endregion --- Inner data access --------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonObject() {
    }

    public JsonObject(params IJsonPair[] jsonPairs) {
      lock (_JsonLock) {
        foreach (IJsonPair JsonPairItem in jsonPairs) {
          _Items.Add(JsonPairItem);
        }
      }
    }

    public JsonObject(JsonObject jsonObject) {
      lock (_JsonLock) {
        foreach (IJsonPair JsonPairItem in jsonObject._Items) {
          _Items.Add(JsonPairItem);
        }
      }
    }

    private bool _IsDisposed = false;

    protected virtual void Dispose(bool isDisposing) {
      if (_IsDisposed) {
        return;
      }
      if (isDisposing) {
        lock (_JsonLock) {
          _Items.Dispose();
        }
        _IsDisposed = true;
      }
    }
    public void Dispose() {
      Dispose(true);
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- Items management --------------------------------------------
    #region --- AddItem --------------------------------------------
    public virtual void Add(IJsonPair jsonPair) {
      lock (_JsonLock) {
        _Items.Add(jsonPair);
      }
    }

    public virtual void Add(string key, IJsonValue jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, string jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, int jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, long jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, float jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, double jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, bool jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }

    public virtual void Add(string key, DateTime jsonValue) {
      lock (_JsonLock) {
        _Items.Add(new JsonPair(key, jsonValue));
      }
    }
    #endregion --- AddItem --------------------------------------------

    public void Clear() {
      lock (_JsonLock) {
        _Items.Clear();
      }
    }
    #endregion --- Items management --------------------------------------------

    #region --- Rendering --------------------------------------------
    public string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true) {
      if (_Items.Count() == 0) {
        if (formatted) {
          return $"{Spaces(indent)}{Json.START_OF_OBJECT}{Json.END_OF_OBJECT}";
        } else {
          return $"{Json.START_OF_OBJECT}{Json.END_OF_OBJECT}";
        }
      }

      lock (_JsonLock) {
        StringBuilder RetVal = new StringBuilder();

        if (formatted && indent >= 0 && needFrontSpaces) {
          RetVal.Append($"{Spaces(indent)}");
        }

        RetVal.Append(Json.START_OF_OBJECT);

        if (formatted) {
          RetVal.AppendLine();
        }

        foreach (IJsonPair JsonPairItem in _Items) {
          RetVal.Append(JsonPairItem.RenderAsString(formatted, indent + Json.DEFAULT_INDENT));
          RetVal.Append(Json.OUTER_FIELD_SEPARATOR);
          if (formatted) {
            RetVal.AppendLine();
          }
        }

        if (formatted) {
          RetVal.Truncate(Environment.NewLine.Length + 1);
          RetVal.AppendLine();
        } else {
          RetVal.Truncate(1);
        }

        if (formatted && indent >= 0) {
          RetVal.Append($"{Spaces(indent)}");
        }

        RetVal.Append(Json.END_OF_OBJECT);

        return RetVal.ToString();
      }

    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using (MemoryStream RetVal = new MemoryStream()) {
        using (JsonWriter Writer = new JsonWriter(RetVal)) {

          Writer.Seek(0, SeekOrigin.Begin);

          if (_Items.Count() == 0) {
            if (formatted) {
              Writer.Write($"{Spaces(indent)}{{}}");
              return RetVal.ToArray();
            } else {
              Writer.Write(Json.START_OF_OBJECT);
              Writer.Write(Json.END_OF_OBJECT);
              return RetVal.ToArray();
            }
          }

          lock (_JsonLock) {

            if (formatted && indent >= Json.DEFAULT_INDENT) {
              Writer.Write($"{Spaces(indent)}");
            }

            Writer.Write(Json.START_OF_OBJECT);
            if (formatted) {
              Writer.WriteLine();
            }

            foreach (IJsonPair JsonPairItem in _Items) {
              Writer.Write(JsonPairItem.RenderAsBytes(formatted, indent + Json.DEFAULT_INDENT));
              Writer.Write(Json.OUTER_FIELD_SEPARATOR);
              if (formatted) {
                Writer.WriteLine();
              }
            }

            if (_Items.Count > 0) {
              RetVal.Flush();
              RetVal.SetLength(RetVal.Length - 1);
              RetVal.Seek(0, SeekOrigin.End);
              if (formatted) {
                RetVal.SetLength(RetVal.Length - Environment.NewLine.Length);
                RetVal.Seek(0, SeekOrigin.End);
                Writer.WriteLine();
              }
            }

            if (formatted && indent >= Json.DEFAULT_INDENT) {
              Writer.Write($"{Spaces(indent)}");
            }

            Writer.Write(Json.END_OF_OBJECT);

            return RetVal.ToArray();
          }
        }
      }
    }
    #endregion --- Rendering --------------------------------------------

    #region --- SafeGetValue --------------------------------------------
    public T SafeGetValueFirst<T>(Func<IJsonPair, bool> predicate) {
      return SafeGetValueFirst<T>(predicate, default(T));
    }
    public T SafeGetValueFirst<T>(Func<IJsonPair, bool> predicate, T defaultValue) {
      if (predicate == null) {
        return defaultValue;
      }

      IJsonPair SelectedItem = _Items.FirstOrDefault(predicate);
      if (SelectedItem == null) {
        return defaultValue;
      }

      return SelectedItem.SafeGetValue<T>(defaultValue);
    }

    public T SafeGetValueLast<T>(Func<IJsonPair, bool> predicate) {
      return SafeGetValueLast<T>(predicate, default(T));
    }
    public T SafeGetValueLast<T>(Func<IJsonPair, bool> predicate, T defaultValue) {
      if (predicate == null) {
        return defaultValue;
      }

      IJsonPair SelectedItem = _Items.LastOrDefault(predicate);
      if (SelectedItem == null) {
        return defaultValue;
      }

      return SelectedItem.SafeGetValue<T>(defaultValue);
    }

    public T SafeGetValueSingle<T>(Func<IJsonPair, bool> predicate) {
      return SafeGetValueSingle<T>(predicate, default(T));
    }
    public T SafeGetValueSingle<T>(Func<IJsonPair, bool> predicate, T defaultValue) {
      if (predicate == null) {
        return defaultValue;
      }

      IJsonPair SelectedItem = _Items.SingleOrDefault(predicate);
      if (SelectedItem == null) {
        return defaultValue;
      }

      return SelectedItem.SafeGetValue<T>(defaultValue);
    }

    public T SafeGetValueFirst<T>(string key) {
      return SafeGetValueFirst<T>(key, default(T));
    }
    public T SafeGetValueFirst<T>(string key, T defaultValue) {
      return SafeGetValueFirst<T>(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant(), defaultValue);
    }

    public T SafeGetValueLast<T>(string key) {
      return SafeGetValueLast<T>(key, default(T));
    }
    public T SafeGetValueLast<T>(string key, T defaultValue) {
      return SafeGetValueLast<T>(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant(), defaultValue);
    }

    public T SafeGetValueSingle<T>(string key) {
      return SafeGetValueSingle<T>(key, default(T));
    }
    public T SafeGetValueSingle<T>(string key, T defaultValue) {
      return SafeGetValueFirst<T>(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant(), defaultValue);
    }
    #endregion --- SafeGetValue --------------------------------------------

    #region --- Parsing from a string --------------------------------------------
    public static JsonObject Parse(string source) {

      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(source)) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return null;
      }

      if (!(source.TrimStart().StartsWith("{") && source.TrimEnd().EndsWith("}"))) {
        Trace.WriteLine("Unable to parse Json string : source is invalid, missing braces");
        return null;
      }

      // Remove the start and final braces
      StringBuilder Temp = new StringBuilder(source.Trim().Substring(1));
      Temp.Truncate(1);
      string objectContent = Temp.ToString();
      #endregion === Validate parameters ===

      JsonObject RetVal = new JsonObject();

      foreach (string PairItem in _GetNextPair(objectContent)) {
        RetVal.Add(JsonPair.Parse(PairItem));
      }

      return RetVal;
    }

    //private static EJsonToken _GetNextToken(string source) {
    //  if (source is null) {
    //    return EJsonToken.UNKNOWN;
    //  }
      
    //  if (source == "") {
    //    return EJsonToken.EMPTY;
    //  }

    //  int InObject = 0;
    //  int InArray = 0;
    //  bool IsEscape = false;

    //  StringBuilder RetVal = new StringBuilder();

    //  char TestChar = source[0];
    //  switch (TestChar) {
    //    case '\\': // Entering an sub object
    //      if (IsEscape) {
    //        IsEscape = !IsEscape;
    //      }
    //      RetVal.Append(TestChar);
    //      break;
        
    //    case '{': // Entering an sub object
    //      InObject++;
    //      break;

    //    case '}': // Stopping an sub object
    //      InObject--;
    //      if (InObject==0) {
    //        return "result";
    //      }
    //      break;

    //    case '[': // Entering an array
    //      InArray++;
    //      break;

    //    case ']': // Stopping an array
    //      InArray--;
    //      break;

    //    case ':': // Get a property value
    //      break;

    //    case '"': // Get a string
    //      break;

    //    case ',': // Between 2 properties
    //      break;

    //  }
    //}

    private static IEnumerable<string> _GetNextPair(string objectContent) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(objectContent)) {
        Trace.WriteLine("Unable to read content of json string array : invalid format : string is null or empty");
        yield break;
      }
      #endregion === Validate parameters ===

      string ProcessedSource = objectContent.Trim();

      int i = 0;
      bool InQuote = false;
      bool NextCharIsControlChar = false;
      int InArrayLevel = 0;
      int InObjectLevel = 0;
      int LengthOfSource = ProcessedSource.Length;
      StringBuilder RetVal = new StringBuilder();

      do {
        RetVal.Clear();
        bool GotOneItem = false;

        #region --- Get the key --------------------------------------------
        bool GotTheKey = false;
        do {

          char CurrentChar = ProcessedSource[i];

          if (CurrentChar == Json.CHR_DOUBLE_QUOTE) {
            RetVal.Append(CurrentChar);
            InQuote = !InQuote;
            i++;
            if (!InQuote) {
              GotTheKey = true;
            }
            continue;
          }

          if (InQuote) {
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if (Json.WHITE_SPACES.Contains(CurrentChar)) {
            i++;
            continue;
          }
        } while (i < LengthOfSource && !GotTheKey);
        #endregion --- Get the key --------------------------------------------

        if (i >= LengthOfSource || InQuote) {
          Trace.WriteLine("Unable to parse Json string : source is invalid");
          yield break;
        }

        #region --- Between key and value --------------------------------------------
        // Skip white spaces
        while (i < LengthOfSource && Json.WHITE_SPACES.Contains(ProcessedSource[i])) {
          i++;
        }

        if (ProcessedSource[i++] != Json.INNER_FIELD_SEPARATOR) {
          Trace.WriteLine("Unable to parse Json string : source is invalid");
          yield break;
        }

        RetVal.Append(Json.INNER_FIELD_SEPARATOR);

        // Skip white spaces
        while (i < LengthOfSource && Json.WHITE_SPACES.Contains(ProcessedSource[i])) {
          i++;
        }
        #endregion --- Between key and value --------------------------------------------

        #region --- Get the value --------------------------------------------
        while (i < LengthOfSource && !GotOneItem) {

          char CurrentChar = ProcessedSource[i];

          if (!NextCharIsControlChar && CurrentChar == Json.CHR_BACKSLASH) {
            NextCharIsControlChar = true;
            i++;
            continue;
          }

          if (CurrentChar == Json.CHR_DOUBLE_QUOTE && !NextCharIsControlChar) {
            RetVal.Append(CurrentChar);
            InQuote = !InQuote;
            i++;
            continue;
          }

          if (CurrentChar == Json.CHR_DOUBLE_QUOTE && NextCharIsControlChar) {
            RetVal.Append(Json.CHR_BACKSLASH).Append(Json.CHR_DOUBLE_QUOTE);
            NextCharIsControlChar = false;
            i++;
            continue;
          }

          if (CurrentChar == Json.CHR_SLASH && NextCharIsControlChar) {
            RetVal.Append(Json.CHR_BACKSLASH).Append(Json.CHR_SLASH);
            NextCharIsControlChar = false;
            i++;
            continue;
          }

          if (CurrentChar == Json.CHR_BACKSLASH && NextCharIsControlChar) {
            RetVal.Append(Json.CHR_BACKSLASH).Append(Json.CHR_BACKSLASH);
            NextCharIsControlChar = false;
            i++;
            continue;
          }

          if (InQuote) {
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if (Json.WHITE_SPACES.Contains(CurrentChar)) {
            i++;
            continue;
          }

          if (CurrentChar == Json.START_OF_ARRAY) {
            InArrayLevel++;
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if (CurrentChar == Json.END_OF_ARRAY) {
            InArrayLevel--;
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if (CurrentChar == Json.START_OF_OBJECT) {
            InObjectLevel++;
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if (CurrentChar == Json.END_OF_OBJECT) {
            InObjectLevel--;
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if (CurrentChar == Json.INNER_FIELD_SEPARATOR) {
            RetVal.Append(Json.INNER_FIELD_SEPARATOR);
            i++;
            continue;
          }

          if (CurrentChar == Json.OUTER_FIELD_SEPARATOR) {
            if (InArrayLevel > 0 || InObjectLevel > 0) {
              RetVal.Append(Json.OUTER_FIELD_SEPARATOR);
              i++;
              continue;
            } else {
              i++;
              GotOneItem = true;
              yield return RetVal.ToString();
              continue;
            }
          }

          RetVal.Append(CurrentChar);
          i++;
        }

        if (i == LengthOfSource && RetVal.Length > 0) {
          yield return RetVal.ToString();
        }
        #endregion --- Get the value --------------------------------------------

      } while (i < LengthOfSource);
      yield break;

    }
    #endregion --- Parsing from a string --------------------------------------------

    #region --- Operators --------------------------------------------
    public static implicit operator Dictionary<string, IJsonValue>(JsonObject source) {
      if (source == null) {
        return null;
      }

      Dictionary<string, IJsonValue> RetVal = new Dictionary<string, IJsonValue>();
      if (source.Count() == 0) {
        return RetVal;
      }

      foreach (IJsonPair JsonPairItem in source) {
        RetVal.Add(JsonPairItem.Key, JsonPairItem.Content);
      }

      return RetVal;
    }

    public static implicit operator Dictionary<string, string>(JsonObject source) {
      if (source == null) {
        return null;
      }

      Dictionary<string, string> RetVal = new Dictionary<string, string>();
      if (source.Count() == 0) {
        return RetVal;
      }

      foreach (IJsonPair JsonPairItem in source) {
        RetVal.Add(JsonPairItem.Key, JsonPairItem.Content.RenderAsString());
      }

      return RetVal;
    }
    #endregion --- Operators --------------------------------------------

    #region --- IEnumerable<IJsonValue> --------------------------------------------
    public IEnumerator<IJsonPair> GetEnumerator() {
      return ((IEnumerable<IJsonPair>)_Items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return ((IEnumerable<IJsonPair>)_Items).GetEnumerator();
    }
    #endregion --- IEnumerable<IJsonValue> --------------------------------------------

    #region --- Indexer(s) --------------------------------------------
    public IJsonPair this[int index] {
      get {
        if (index < 0 || index >= _Items.Count) {
          return null;
        }
        return _Items[index];
      }
    }
    public IJsonValue this[string key] {
      get {
        if (key == null) {
          return null;
        }
        return _Items[key].Content;
      }
    }
    #endregion --- Indexer(s) --------------------------------------------
  }
}
