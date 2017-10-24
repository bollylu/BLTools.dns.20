using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonObject : IJsonValue {

    public readonly JsonPairCollection KeyValuePairs = new JsonPairCollection();

    private object _JsonLock = new object();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonObject() {
    }

    public JsonObject(params IJsonPair[] jsonPairs) {
      lock ( _JsonLock ) {
        foreach ( IJsonPair JsonPairItem in jsonPairs ) {
          KeyValuePairs.Add(JsonPairItem);
        }
      }
    }

    public JsonObject(JsonObject jsonObject) {
      lock ( _JsonLock ) {
        foreach ( IJsonPair JsonPairItem in jsonObject.KeyValuePairs ) {
          KeyValuePairs.Add(JsonPairItem);
        }
      }
    }

    public JsonObject(string jsonObject) {

    }

    public void Dispose() {
      lock ( _JsonLock ) {
        KeyValuePairs.Dispose();
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public virtual void AddItem(IJsonPair jsonPair) {
      lock ( _JsonLock ) {
        KeyValuePairs.Add(jsonPair);
      }
    }

    public void Clear() {
      lock ( _JsonLock ) {
        KeyValuePairs.Clear();
      }
    }

    public string RenderAsString() {
      if ( KeyValuePairs.Count() == 0 ) {
        return "null";
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        RetVal.Append("{");

        foreach ( IJsonPair JsonPairItem in KeyValuePairs ) {
          RetVal.Append(JsonPairItem.RenderAsString());
          RetVal.Append(",");
        }
        RetVal.Truncate(1);

        RetVal.Append("}");

        return RetVal.ToString();
      }

    }

    public static JsonObject Parse(string source) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(source) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return null;
      }

      string ProcessedSource = source.Trim();

      if ( !( ProcessedSource.StartsWith("{") && ProcessedSource.EndsWith("}") ) ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid, missing braces");
        return null;
      }
      #endregion === Validate parameters ===
      JsonObject RetVal = new JsonObject();
      StringAndPointer NextPair = _GetNextPair(new StringAndPointer(source, 0));
      while (NextPair!=null) {
        //RetVal.AddItem(new )
      }

      return RetVal;
    }

    private static StringAndPointer _GetNextPair(StringAndPointer source) {
      if ( string.IsNullOrWhiteSpace(source.StringValue) ) {
        return null;
      }

      if ( source.StartPosition >= source.StringValue.Length ) {
        return null;
      }

      string ProcessedSource = source.StringValue.Substring(source.StartPosition).Trim();

      StringBuilder RetVal = new StringBuilder();

      int i = 0;
      bool InQuote = false;
      int InArrayLevel = 0;
      int InObjectLevel = 0;
      int LengthOfSource = ProcessedSource.Length;

      while ( i < LengthOfSource ) {
        if ( i > 0 && ProcessedSource[i] == '"' && ProcessedSource[i - 1] != '\\' ) {
          InQuote = !InQuote;
          i++;
          continue;
        }

        if ( i > 0 && InQuote && ProcessedSource[i] == '"' && ProcessedSource[i - 1] == '\\' ) {
          RetVal.Append(ProcessedSource[i]);
          i++;
          continue;
        }

        if ( Json.WhiteSpaces.Contains(ProcessedSource[i]) ) {
          i++;
          continue;
        }

        if ( ProcessedSource[i] == '[' ) {
          InArrayLevel++;
          RetVal.Append(ProcessedSource[i]);
          i++;
          continue;
        }

        if ( ProcessedSource[i] == ']' ) {
          InArrayLevel--;
          RetVal.Append(ProcessedSource[i]);
          i++;
          continue;
        }

        if ( ProcessedSource[i] == '{' ) {
          InObjectLevel++;
          RetVal.Append(ProcessedSource[i]);
          i++;
          continue;
        }

        if ( ProcessedSource[i] == '}' ) {
          InObjectLevel--;
          RetVal.Append(ProcessedSource[i]);
          i++;
          continue;
        }

        if ( ProcessedSource[i] == Json.InnerFieldSeparator ) {
          if ( InObjectLevel == 0 ) {
            return null;
          }
          RetVal.Append(Json.InnerFieldSeparator);
          i++;
          continue;
        }

        if ( ProcessedSource[i] == Json.OuterFieldSeparator ) {
          if ( InArrayLevel > 0 || InObjectLevel > 0 ) {
            RetVal.Append(ProcessedSource[i]);
            i++;
            continue;
          } else {
            break;
          }
        }

        RetVal.Append(ProcessedSource[i]);
        i++;
      }

      return new StringAndPointer(RetVal.ToString(), source.StartPosition + i + 1);
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
