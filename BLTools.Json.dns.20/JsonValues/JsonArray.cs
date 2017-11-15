using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections;

namespace BLTools.Json {
  public class JsonArray : IJsonValue, IEnumerable<IJsonValue> {

    public static JsonArray Empty => new JsonArray();

    private readonly JsonValueCollection Items = new JsonValueCollection();

    private object _JsonLock = new object();

    #region --- Constructor(s) --------------------------------------------
    public JsonArray() { }

    public JsonArray(IEnumerable<IJsonValue> values) {
      if ( values == null ) {
        Trace.WriteLine("Unable to create JsonArray : values are null");
        return;
      }
      if ( values.Count() == 0 ) {
        return;
      }

      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values ) {
          Items.Add(SourceItem);
        }
      }
    }

    public JsonArray(params IJsonValue[] values) {
      if ( values == null ) {
        Trace.WriteLine("Unable to create JsonArray : values are null");
        return;
      }
      if ( values.Count() == 0 ) {
        return;
      }

      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values ) {
          Items.Add(SourceItem);
        }
      }
    }

    public JsonArray(JsonArray values) {
      if ( values == null ) {
        return;
      }
      if ( values.Count() == 0 ) {
        return;
      }

      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values ) {
          Items.Add(SourceItem);
        }
      }
    }

    public void Dispose() {
      lock ( _JsonLock ) {
        Items.Clear();
        Items.Dispose();
      }
    }
    #endregion --- Constructor(s) --------------------------------------------

    #region Public methods

    public void Add(params IJsonValue[] newValue) {
      if ( newValue == null ) {
        return;
      }

      if ( newValue.Count() == 0 ) {
        return;
      }

      lock ( _JsonLock ) {
        foreach ( IJsonValue ValueItem in newValue ) {
          Items.Add(ValueItem);
        }
      }
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      if ( Items.Count() == 0 ) {
        if ( formatted ) {
          return $"{StringExtension.Spaces(indent)}[]";
        } else {
          return "[]";
        }
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        if ( formatted && indent >= Json.DEFAULT_INDENT ) {
          RetVal.Append($"{StringExtension.Spaces(indent)}");
        }
        RetVal.Append("[");

        if ( formatted ) {
          RetVal.AppendLine();
        }

        foreach ( IJsonValue JsonValueItem in Items ) {
          RetVal.Append(JsonValueItem.RenderAsString(formatted, indent + Json.DEFAULT_INDENT));
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

        RetVal.Append("]");

        return RetVal.ToString();
      }
    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {

          if ( Items.Count() == 0 ) {
            if ( formatted ) {
              Writer.Write($"{StringExtension.Spaces(indent)}[]");
              return RetVal.ToArray();
            } else {
              Writer.Write("[]");
              return RetVal.ToArray();
            }

          }

          lock ( _JsonLock ) {

            if ( formatted && indent >= Json.DEFAULT_INDENT ) {
              Writer.Write($"{StringExtension.Spaces(indent)}");
            }
            Writer.Write("[");

            if ( formatted ) {
              Writer.WriteLine();
            }

            foreach ( IJsonValue JsonValueItem in Items ) {
              Writer.Write(JsonValueItem.RenderAsBytes(formatted, indent + Json.DEFAULT_INDENT));
              Writer.Write(",");
              if ( formatted ) {
                Writer.WriteLine();
              }
            }

            if ( Items.Count > 0 ) {
              Writer.Flush();
              RetVal.Flush();
              RetVal.SetLength(RetVal.Length - 1);
              if ( formatted ) {
                RetVal.SetLength(RetVal.Length - Environment.NewLine.Length);
                Writer.WriteLine();
              }
            }

            if ( formatted && indent >= Json.DEFAULT_INDENT ) {
              Writer.Write($"{StringExtension.Spaces(indent)}");
            }

            Writer.Write("]");

            return RetVal.ToArray();
          }
        }
      }
    }
    #endregion Public methods

    #region --- Parsing from a string --------------------------------------------
    public static JsonArray Parse(string source) {

      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(source) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return null;
      }

      if ( !source.TrimStart().StartsWith("[") || !source.TrimEnd().EndsWith("]") ) {
        Trace.WriteLine("Unable to parse Json string : source is not an array");
        return null;
      }

      // Remove the start and final brackets
      StringBuilder Temp = new StringBuilder(source.Trim().Substring(1));
      Temp.Truncate(1);
      string arrayContent = Temp.ToString();
      #endregion === Validate parameters ===

      JsonArray RetVal = new JsonArray();

      foreach ( string ValueItem in _GetNextValue(arrayContent) ) {
        RetVal.Add(JsonValue.Parse(ValueItem));
      }

      return RetVal;
    }
    private static IEnumerable<string> _GetNextValue(string arrayContent) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(arrayContent) ) {
        Trace.WriteLine("Unable to read content of json string array : invalid format : string is null or empty");
        yield break;
      }

      //if ( !( ProcessedSource.StartsWith("[") && ProcessedSource.EndsWith("]") ) ) {
      //  Trace.WriteLine("Unable to read content of json string array : invalid format : missing brackets");
      //  yield break;
      //} 
      #endregion === Validate parameters ===

      string ProcessedSource = arrayContent.Trim();

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

        do {

          char CurrentChar = ProcessedSource[i];

          if ( !NextCharIsControlChar && CurrentChar == Json.CHR_BACKSLASH ) {
            NextCharIsControlChar = true;
            i++;
            continue;
          }


          if ( CurrentChar == Json.CHR_DOUBLE_QUOTE && !NextCharIsControlChar ) {
            RetVal.Append(CurrentChar);
            InQuote = !InQuote;
            i++;
            continue;
          }

          if ( CurrentChar == Json.CHR_DOUBLE_QUOTE && NextCharIsControlChar ) {
            RetVal.Append(Json.CHR_BACKSLASH).Append(Json.CHR_DOUBLE_QUOTE);
            NextCharIsControlChar = false;
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
        } while ( i < LengthOfSource && !GotOneItem );

        if ( i == LengthOfSource && RetVal.Length > 0 ) {
          yield return RetVal.ToString();
        }

      } while ( i < LengthOfSource );

      yield break;
    }
    #endregion --- Parsing from a string --------------------------------------------

    #region --- Operators --------------------------------------------
    public static implicit operator List<IJsonValue>(JsonArray source) {
      if ( source == null ) {
        return null;
      }

      List<IJsonValue> RetVal = new List<IJsonValue>();
      if ( source.Items.Count == 0 ) {
        return RetVal;
      }

      foreach ( IJsonValue ValueItem in source.Items ) {
        RetVal.Add(ValueItem);
      }

      return RetVal;
    }

    public static implicit operator IJsonValue[] (JsonArray source) {
      if ( source == null ) {
        return null;
      }

      int ItemsCount = source.Items.Count;
      IJsonValue[] RetVal = new IJsonValue[ItemsCount];
      if ( ItemsCount == 0 ) {
        return RetVal;
      }

      for ( int i = 0; i < ItemsCount; i++ ) {
        RetVal[i] = source.Items[i];
      }

      return RetVal;
    }
    #endregion --- Operators --------------------------------------------

    #region --- IEnumerable<IJsonValue> --------------------------------------------
    public IEnumerator<IJsonValue> GetEnumerator() {
      return ( (IEnumerable<IJsonValue>)Items ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return ( (IEnumerable<IJsonValue>)Items ).GetEnumerator();
    }
    #endregion --- IEnumerable<IJsonValue> --------------------------------------------

    public IJsonValue this[int index] {
      get {
        if ( index < 0 || index > Items.Count ) {
          return null;
        }
        return Items[index];
      }
    }
  }
}
