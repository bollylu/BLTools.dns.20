using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Json {
  public class JsonBool : IJsonValue {

    public bool? Value { get; set; }

    public JsonBool(bool jsonBool) {
      Value = jsonBool;
    }

    public JsonBool(JsonBool jsonBool) {
      Value = jsonBool.Value;
    }

    public JsonBool(string jsonBool) {
      Value = jsonBool.ToBool();
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if (formatted) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append("null");
      } else {
        RetVal.Append((bool)Value ? "true" : "false");
      }
      return RetVal.ToString();
    }
  }
}
