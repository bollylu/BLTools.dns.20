using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Json {
  public class JsonInt : IJsonValue {

    public int? Value { get; set; }

    public JsonInt(int jsonInt) {
      Value = jsonInt;
    }

    public JsonInt(JsonInt jsonInt) {
      Value = jsonInt.Value;
    }

    public JsonInt(string jsonInt) {
      try {
        Value = int.Parse(jsonInt);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse int : {jsonInt} : {ex.Message}");
        Value = null;
      }
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append("NaN");
      } else {
        RetVal.Append(Value.ToString());
      }

      return RetVal.ToString();
    }

  }
}
