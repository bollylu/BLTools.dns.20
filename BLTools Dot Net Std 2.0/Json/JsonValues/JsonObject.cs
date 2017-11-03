using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.IO;

namespace BLTools.Json {
  public class JsonObject : IJsonValue {

    public static JsonObject Empty = new JsonObject();

    public readonly JsonPairCollection Items = new JsonPairCollection();

    private object _JsonLock = new object();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonObject() {
    }

    public JsonObject(params IJsonPair[] jsonPairs) {
      lock ( _JsonLock ) {
        foreach ( IJsonPair JsonPairItem in jsonPairs ) {
          Items.Add(JsonPairItem);
        }
      }
    }

    public JsonObject(JsonObject jsonObject) {
      lock ( _JsonLock ) {
        foreach ( IJsonPair JsonPairItem in jsonObject.Items ) {
          Items.Add(JsonPairItem);
        }
      }
    }

    public void Dispose() {
      lock ( _JsonLock ) {
        Items.Dispose();
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region Public methods
    public virtual void AddItem(IJsonPair jsonPair) {
      lock ( _JsonLock ) {
        Items.Add(jsonPair);
      }
    }

    public void Clear() {
      lock ( _JsonLock ) {
        Items.Clear();
      }
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      if ( Items.Count() == 0 ) {
        if ( formatted ) {
          return $"{StringExtension.Spaces(indent)}{{}}";
        } else {
          return "{}";
        }

      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        if ( formatted && indent >= Json.DEFAULT_INDENT ) {
          RetVal.Append($"{StringExtension.Spaces(indent)}");
        }

        RetVal.Append("{");
        if ( formatted ) {
          RetVal.AppendLine();
        }

        foreach ( IJsonPair JsonPairItem in Items ) {
          RetVal.Append(JsonPairItem.RenderAsString(formatted, indent + Json.DEFAULT_INDENT));
          RetVal.Append(",");
          if ( formatted ) {
            RetVal.AppendLine();
          }
        }
        if ( formatted ) {
          RetVal.Truncate(Environment.NewLine.Length + 1);
          RetVal.AppendLine();
        } else {
          RetVal.Truncate(1);
        }

        if ( formatted && indent >= Json.DEFAULT_INDENT ) {
          RetVal.Append($"{StringExtension.Spaces(indent)}");
        }

        RetVal.Append("}");

        return RetVal.ToString();
      }

    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( StreamWriter Writer = new StreamWriter(RetVal) ) {

          if ( Items.Count() == 0 ) {
            if ( formatted ) {
              Writer.Write($"{StringExtension.Spaces(indent)}{{}}");
              return RetVal.ToArray();
            } else {
              Writer.Write("{}");
              return RetVal.ToArray();
            }

          }

          lock ( _JsonLock ) {

            if ( formatted && indent >= Json.DEFAULT_INDENT ) {
              Writer.Write($"{StringExtension.Spaces(indent)}");
            }

            Writer.Write("{");
            if ( formatted ) {
              Writer.WriteLine();
            }

            foreach ( IJsonPair JsonPairItem in Items ) {
              Writer.Write(JsonPairItem.RenderAsBytes(formatted, indent + Json.DEFAULT_INDENT));
              Writer.Write(",");
              if ( formatted ) {
                Writer.WriteLine();
              }
            }

            Writer.BaseStream.Position--;
            if ( formatted ) {
              Writer.BaseStream.Position -= Environment.NewLine.Length;
              Writer.WriteLine();
            }

            if ( formatted && indent >= Json.DEFAULT_INDENT ) {
              Writer.Write($"{StringExtension.Spaces(indent)}");
            }

            Writer.Write("}");

            return RetVal.ToArray();
          }
        }
      }
    }

    public T SafeGetValueFirst<T>(Func<IJsonPair, bool> predicate) {
      return SafeGetValueFirst<T>(predicate, default(T));
    }
    public T SafeGetValueFirst<T>(Func<IJsonPair, bool> predicate, T defaultValue) {
      if ( predicate == null ) {
        return defaultValue;
      }

      IJsonPair SelectedItem = Items.FirstOrDefault(predicate);
      if ( SelectedItem == null ) {
        return defaultValue;
      }

      return SelectedItem.SafeGetValue<T>(defaultValue);
    }

    public T SafeGetValueLast<T>(Func<IJsonPair, bool> predicate) {
      return SafeGetValueLast<T>(predicate, default(T));
    }
    public T SafeGetValueLast<T>(Func<IJsonPair, bool> predicate, T defaultValue) {
      if ( predicate == null ) {
        return defaultValue;
      }

      IJsonPair SelectedItem = Items.LastOrDefault(predicate);
      if ( SelectedItem == null ) {
        return defaultValue;
      }

      return SelectedItem.SafeGetValue<T>(defaultValue);
    }

    public T SafeGetValueSingle<T>(Func<IJsonPair, bool> predicate) {
      return SafeGetValueSingle<T>(predicate, default(T));
    }
    public T SafeGetValueSingle<T>(Func<IJsonPair, bool> predicate, T defaultValue) {
      if ( predicate == null ) {
        return defaultValue;
      }

      IJsonPair SelectedItem = Items.SingleOrDefault(predicate);
      if ( SelectedItem == null ) {
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
    #endregion Public methods

    public static JsonObject Parse(string source) {

      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(source) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return null;
      }

      if ( !( source.TrimStart().StartsWith("{") && source.TrimEnd().EndsWith("}") ) ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid, missing braces");
        return null;
      }

      // Remove the start and final braces
      StringBuilder Temp = new StringBuilder(source.Trim().Substring(1));
      Temp.Truncate(1);
      string objectContent = Temp.ToString();
      #endregion === Validate parameters ===

      JsonObject RetVal = new JsonObject();

      foreach ( string PairItem in _GetNextPair(objectContent) ) {
        RetVal.AddItem(JsonPair.Parse(PairItem));
      }

      return RetVal;
    }

    private static IEnumerable<string> _GetNextPair(string objectContent) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(objectContent) ) {
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

      while ( i < LengthOfSource ) {
        RetVal.Clear();
        bool GotOneItem = false;

        while ( i < LengthOfSource && !GotOneItem ) {

          #region --- Get the key --------------------------------------------
          bool GotTheKey = false;
          while ( i < LengthOfSource && !GotTheKey ) {

            char CurrentChar = ProcessedSource[i];

            if ( !NextCharIsControlChar && CurrentChar == '\\' ) {
              NextCharIsControlChar = true;
              i++;
              continue;
            }

            if ( NextCharIsControlChar && "\"\\\t\b\r\n\f".Contains(CurrentChar) ) {
              NextCharIsControlChar = false;
              RetVal.Append('\\');
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }


            if ( CurrentChar == '"' ) {
              RetVal.Append(CurrentChar);
              InQuote = !InQuote;
              i++;
              if ( !InQuote ) {
                GotTheKey = true;
              }
              continue;
            }

            if ( InQuote ) {
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

            if ( Json.WhiteSpaces.Contains(CurrentChar) ) {
              i++;
              continue;
            }
          }
          #endregion --- Get the key --------------------------------------------

          if ( i >= LengthOfSource && InQuote ) {
            Trace.WriteLine("Unable to parse Json string : source is invalid");
            yield break;
          }

          #region --- Between key and value --------------------------------------------
          // Skip white spaces
          while ( i < ProcessedSource.Length && Json.WhiteSpaces.Contains(ProcessedSource[i]) ) {
            i++;
          }

          if ( ProcessedSource[i++] != Json.InnerFieldSeparator ) {
            Trace.WriteLine("Unable to parse Json string : source is invalid");
            yield break;
          }

          RetVal.Append(Json.InnerFieldSeparator);
          // Skip white spaces
          while ( i < ProcessedSource.Length && Json.WhiteSpaces.Contains(ProcessedSource[i]) ) {
            i++;
          }
          #endregion --- Between key and value --------------------------------------------

          #region --- Get the value --------------------------------------------
          while ( i < LengthOfSource && !GotOneItem ) {

            char CurrentChar = ProcessedSource[i];

            if ( !NextCharIsControlChar && CurrentChar == '\\' ) {
              NextCharIsControlChar = true;
              i++;
              continue;
            }

            if ( NextCharIsControlChar && "\"\\\t\b\r\n\f".Contains(CurrentChar) ) {
              NextCharIsControlChar = false;
              RetVal.Append('\\');
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }


            if ( CurrentChar == '"' ) {
              RetVal.Append(CurrentChar);
              InQuote = !InQuote;
              i++;
              continue;
            }

            if ( InQuote ) {
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

            if ( Json.WhiteSpaces.Contains(CurrentChar) ) {
              i++;
              continue;
            }

            if ( CurrentChar == '[' ) {
              InArrayLevel++;
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

            if ( CurrentChar == ']' ) {
              InArrayLevel--;
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

            if ( CurrentChar == '{' ) {
              InObjectLevel++;
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

            if ( CurrentChar == '}' ) {
              InObjectLevel--;
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

            if ( CurrentChar == Json.InnerFieldSeparator ) {
              if ( InObjectLevel == 0 ) {
                Trace.WriteLine($"Unable to read content of json string array : invalid format at position {i}");
                yield break;
              }
              RetVal.Append(Json.InnerFieldSeparator);
              i++;
              continue;
            }

            if ( CurrentChar == Json.OuterFieldSeparator ) {
              if ( InArrayLevel > 0 || InObjectLevel > 0 ) {
                RetVal.Append(CurrentChar);
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

          if ( i == LengthOfSource && RetVal.Length > 0 ) {
            yield return RetVal.ToString();
          }
          #endregion --- Get the value --------------------------------------------

        }

      }
      yield break;

    }
  }
}
