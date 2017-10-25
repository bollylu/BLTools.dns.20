using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Json {
  public class JsonLong : IJsonValue {

    public long? Value { get; set; }

    public JsonLong(long jsonLong) {
      Value = jsonLong;
    }

    public JsonLong(JsonLong jsonLong) {
      Value = jsonLong.Value;
    }

    public JsonLong(string jsonLong) {
      try {
        Value = long.Parse(jsonLong);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse long : {jsonLong} : {ex.Message}");
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
