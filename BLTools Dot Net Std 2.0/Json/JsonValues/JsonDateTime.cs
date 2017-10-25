using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonDateTime : IJsonValue {

    public DateTime? Value { get; set; }

    public JsonDateTime(DateTime jsonDateTime) {
      Value = jsonDateTime;
    }

    public JsonDateTime(JsonDateTime jsonDateTime) {
      Value = jsonDateTime.Value;
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      RetVal.Append($"\"{( (DateTime)Value ).ToString("s")}\"");

      return RetVal.ToString();
    }
  }
}
