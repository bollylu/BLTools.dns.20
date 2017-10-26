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

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append("null");
      } else {
        RetVal.Append($"\"{Value}\"");
      }
      return RetVal.ToString();
    }
    
  }
}
