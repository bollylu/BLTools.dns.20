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

    public string RenderAsString() {
      if ( Value == null ) {
        return "null";
      }
      return $"\"{( (DateTime)Value ).ToString("s")}\"";
    }
  }
}
