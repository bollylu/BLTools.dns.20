using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonArray : IJsonValue {

    public static JsonArray Empty => new JsonArray();

    private object _JsonLock = new object();

    public readonly JsonValueCollection Items = new JsonValueCollection();

    #region --- Constructor(s) --------------------------------------------
    public JsonArray() { }

    public JsonArray(IEnumerable<IJsonValue> values) {
      if ( values == null ) {
        Trace.WriteLine("Unable to create JsonArray : values are null");
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
      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values.Items ) {
          Items.Add(SourceItem);
        }
      }
    }

    public JsonArray(string values) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(values) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return;
      }

      if ( !values.TrimStart().StartsWith("[") || !values.TrimEnd().EndsWith("]") ) {
        Trace.WriteLine("Unable to parse Json string : source is not an array");
        return;
      }

      // Remove the start and final brackets
      StringBuilder Temp = new StringBuilder(values.Trim().Substring(1));
      Temp.Truncate(1);
      string arrayContent = Temp.ToString();
      #endregion === Validate parameters ===

      foreach ( string ValueItem in _GetNextValue(arrayContent) ) {
        Items.Add(JsonValue.Parse(ValueItem));
      }
    }

    public void Dispose() {
      Items.Clear();
      Items.Dispose();
    }
    #endregion --- Constructor(s) --------------------------------------------

    #region Public methods
    public string RenderAsString() {
      if ( Items.Count() == 0 ) {
        return "[]";
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        RetVal.Append("[");

        foreach ( IJsonValue JsonValueItem in Items ) {
          RetVal.Append(JsonValueItem.RenderAsString());
          RetVal.Append(",");
        }
        RetVal.Truncate(1);

        RetVal.Append("]");

        return RetVal.ToString();
      }
    }
    #endregion Public methods



    private IEnumerable<string> _GetNextValue(string arrayContent) {
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
      bool PossibleControlChar = false;
      int InArrayLevel = 0;
      int InObjectLevel = 0;
      int LengthOfSource = ProcessedSource.Length;
      StringBuilder RetVal = new StringBuilder();

      while ( i < LengthOfSource ) {

        RetVal.Clear();
        bool GotOneItem = false;

        while ( i < LengthOfSource && !GotOneItem ) {

          char CurrentChar = ProcessedSource[i];

          if ( CurrentChar == '\\' ) {
            PossibleControlChar = true;
            i++;
            continue;
          }

          if ( CurrentChar == '"' ) {

            if (PossibleControlChar) {
              PossibleControlChar = false;
              RetVal.Append(CurrentChar);
              i++;
              continue;
            }

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
            if ( InArrayLevel > 0 || InObjectLevel > 0  ) {
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

      }

      yield break;
    }

    private class StringAndPointer {
      public string StringValue;
      public int StartPosition;
      public StringAndPointer(string stringValue, int pointer) {
        StringValue = stringValue;
        StartPosition = pointer;
      }
    }
  }
}
