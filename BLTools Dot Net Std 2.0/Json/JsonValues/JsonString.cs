using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonString : IJsonValue {

    public string Value { get; set; }

    public JsonString(string jsonString)  {
      Value = jsonString;
    }

    public JsonString(JsonString jsonString) {
      Value = jsonString.Value;
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString() {
      if ( Value == null ) {
        return "null";
      }
      return $"\"{Value.Replace("\"", "\\\"")}\"";
    }
  }
}
