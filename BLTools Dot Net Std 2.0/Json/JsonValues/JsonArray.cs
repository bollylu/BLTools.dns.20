﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonArray : IJsonValue {

    public static JsonArray Empty => new JsonArray();

    public readonly JsonValueCollection Items = new JsonValueCollection();

    private object _JsonLock = new object();

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

    public void Dispose() {
      Items.Clear();
      Items.Dispose();
    }
    #endregion --- Constructor(s) --------------------------------------------

    #region Public methods

    public void AddItem(IJsonValue newValue) {
      lock ( _JsonLock ) {
        Items.Add(newValue);
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
    #endregion Public methods

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
        RetVal.AddItem(JsonValue.Parse(ValueItem));
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

            if ( PossibleControlChar ) {
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

      }

      yield break;
    }

    public static implicit operator Dictionary<string, IJsonValue>(JsonArray source) {
      if ( source == null ) {
        return null;
      }

      Dictionary<string, IJsonValue> RetVal = new Dictionary<string, IJsonValue>();
      if ( source.Items.Count == 0 ) {
        return RetVal;
      }

      foreach ( IJsonPair PairItem in source.Items ) {
        RetVal.Add(PairItem.Key, PairItem.Content);
      }

      return RetVal;
    }
  }
}
