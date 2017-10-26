using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonPair : IDisposable, IJsonPair {

    public static JsonPair Default => new JsonPair("(default)", new JsonNull());

    public string Key { get; private set; }

    public IJsonValue Content { get; private set; }

    public JsonString StringContent => Content is JsonString Temp ? Temp : null;

    public JsonInt IntContent => Content is JsonInt Temp ? Temp : null;

    public JsonLong LongContent => Content is JsonLong Temp ? Temp : null;

    public JsonFloat FloatContent => Content is JsonFloat Temp ? Temp : null;

    public JsonDouble DoubleContent => Content is JsonDouble Temp ? Temp : null;

    public JsonBool BoolContent => Content is JsonBool Temp ? Temp : null;

    public JsonDateTime DateTimeContent => Content is JsonDateTime Temp ? Temp : null;

    public JsonArray ArrayContent => Content is JsonArray Temp ? Temp : null;

    public JsonObject ObjectContent => Content is JsonObject Temp ? Temp : null;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonPair() { }

    public JsonPair(string key, IJsonValue jsonValue) {
      Key = key;
      Content = jsonValue;
    }

    public JsonPair(IJsonPair jsonPair) {
      Key = jsonPair.Key;
      Content = jsonPair.Content;
    }

    public JsonPair(string key, string content) {
      Key = key;
      Content = new JsonString(content);
    }

    public JsonPair(string key, int content) {
      Key = key;
      Content = new JsonInt(content);
    }

    public JsonPair(string key, long content) {
      Key = key;
      Content = new JsonLong(content);
    }

    public JsonPair(string key, float content) {
      Key = key;
      Content = new JsonFloat(content);
    }

    public JsonPair(string key, double content) {
      Key = key;
      Content = new JsonDouble(content);
    }

    public JsonPair(string key, DateTime content) {
      Key = key;
      Content = new JsonDateTime(content);
    }

    public JsonPair(string key, bool content) {
      Key = key;
      Content = new JsonBool(content);
    }

    public void Dispose() {
      Key = null;
      Content.Dispose();
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
        RetVal.Append($"\"{Key}\" : ");
      } else {
        RetVal.Append($"\"{Key}\":");
      }

      RetVal.Append(Content.RenderAsString(false));

      return RetVal.ToString();
    }

    public static IJsonPair Parse(string source) {

      return Parse(source, Default);

    }
    public static IJsonPair Parse(string source, IJsonPair defaultValue) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(source) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return defaultValue;
      }

      if ( !source.Contains(":") ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }

      string ProcessedSource = source.Trim();

      if ( !ProcessedSource.StartsWith("\"") ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }
      #endregion === Validate parameters ===

      StringBuilder _Key = new StringBuilder();
      string _Content = "";

      int LengthOfSource = ProcessedSource.Length;
      bool _InQuote = false;
      int i = 0;

      #region --- Get the key --------------------------------------------
      while ( i < LengthOfSource ) {
        if ( i == 0 && ProcessedSource[i] == '"' ) {
          _InQuote = true;
          i++;
          continue;
        }

        if ( i > 0 && ProcessedSource[i] == '"' && ProcessedSource[i - 1] != '\\' ) {
          if ( _InQuote ) {
            i++;
            _InQuote = false;
            break;
          } else {
            Trace.WriteLine("Unable to parse Json string : source is invalid");
            return defaultValue;
          }
        }

        if ( i > 0 && _InQuote ) {
          _Key.Append(ProcessedSource[i]);
          i++;
          continue;
        }

        i++;
        continue;

      }

      if ( _InQuote ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }
      #endregion --- Get the key --------------------------------------------

      // Skip white spaces
      while ( i < ProcessedSource.Length && Json.WhiteSpaces.Contains(ProcessedSource[i]) ) {
        i++;
      }

      if ( ProcessedSource[i] != ':' ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }

      #region --- Get the value --------------------------------------------
      _Content = ProcessedSource.Substring(i + 1).TrimStart();
      #endregion --- Get the value --------------------------------------------

      JsonPair RetVal = new JsonPair();

      RetVal.Key = _Key.ToString();
      RetVal.Content = JsonValue.Parse(_Content);

      return RetVal;
    }

    public T SafeGetValue<T>(T defaultValue) {
      try {
        switch ( typeof(T).Name.ToLowerInvariant() ) {
          case "string":
            return (T)Convert.ChangeType(StringContent.Value, typeof(T));
          case "int32":
          case "int64":
            return (T)Convert.ChangeType(IntContent.Value, typeof(T));
          case "long":
            return (T)Convert.ChangeType(LongContent.Value, typeof(T));
          case "float":
            return (T)Convert.ChangeType(FloatContent.Value, typeof(T));
          case "double":
            return (T)Convert.ChangeType(DoubleContent.Value, typeof(T));
          case "datetime":
            return (T)Convert.ChangeType(DateTimeContent.Value, typeof(T));
          case "boolean":
            return (T)Convert.ChangeType(BoolContent.Value, typeof(T));
          default:
            return defaultValue;
        }

      } catch {
        return defaultValue;
      }
    }

  }


}
