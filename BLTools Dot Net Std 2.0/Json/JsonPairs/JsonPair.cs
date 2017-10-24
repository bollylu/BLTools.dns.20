using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonPair<T> : IDisposable, IJsonPair where T : IJsonValue {

    public string Key { get; private set; }

    public T Content { get; private set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonPair(string key, T jsonValue) {
      Key = key;
      Content = jsonValue;
    }

    public JsonPair(JsonPair<T> jsonPair) {
      _Initialize(jsonPair);
    }

    public JsonPair(string key, string content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonString(content), typeof(T))));
    }

    public JsonPair(string key, int content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonInt(content), typeof(T))));
    }

    public JsonPair(string key, long content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonLong(content), typeof(T))));
    }

    public JsonPair(string key, float content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonFloat(content), typeof(T))));
    }

    public JsonPair(string key, double content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonDouble(content), typeof(T))));
    }

    public JsonPair(string key, DateTime content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonDateTime(content), typeof(T))));
    }

    public JsonPair(string key, bool content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonBool(content), typeof(T))));
    }

    private JsonPair<T> _Initialize(JsonPair<T> jsonPair) {
      Key = jsonPair.Key;
      Content = jsonPair.Content;
      return this;
    }

    public void Dispose() {
      Key = null;
      Content.Dispose();
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public string RenderAsString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"\"{Key}\":");
      RetVal.Append(Content.RenderAsString());
      return RetVal.ToString();
    }

    public static JsonPair<T> Parse(string source, IJsonValue defaultValue) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(source) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return null;
      }

      if ( !source.Contains(":") ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return null;
      }

      string ProcessedSource = source.Trim();

      if ( !ProcessedSource.StartsWith("\"") ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return null;
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
            return null;
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
        return null;
      }
      #endregion --- Get the key --------------------------------------------

      // Skip white spaces
      while ( i < ProcessedSource.Length && Json.WhiteSpaces.Contains(ProcessedSource[i]) ) {
        i++;
      }

      if ( ProcessedSource[i] != ':' ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return null;
      }

      #region --- Get the value --------------------------------------------
      _Content = ProcessedSource.Substring(i + 1).TrimStart();
      #endregion --- Get the value --------------------------------------------

      if ( typeof(T) == typeof(JsonString) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonString(_Content.RemoveExternalQuotes()), typeof(T)));
      }

      if ( typeof(T) == typeof(JsonInt) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonInt(_Content), typeof(T)));
      }

      if ( typeof(T) == typeof(JsonLong) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonLong(_Content), typeof(T)));
      }

      if ( typeof(T) == typeof(JsonFloat) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonFloat(_Content), typeof(T)));
      }

      if ( typeof(T) == typeof(JsonDouble) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonDouble(_Content), typeof(T)));
      }

      if ( typeof(T) == typeof(JsonBool) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonBool(_Content), typeof(T)));
      }

      if ( typeof(T) == typeof(JsonArray) ) {
        return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(new JsonArray(_Content), typeof(T)));
      }

      return new JsonPair<T>(_Key.ToString(), (T)Convert.ChangeType(defaultValue, typeof(T)));

    }

  }
}
