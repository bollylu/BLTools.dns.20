using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonDateTime : IJsonValue {

    public DateTime? Content { get; set; }

    public JsonDateTime(DateTime jsonDateTime) {
      Content = jsonDateTime;
    }

    public JsonDateTime(JsonDateTime jsonDateTime) {
      Content = jsonDateTime.Content;
    }

    public void Dispose() {
      Content = null;
    }

    public string RenderAsString() {
      if ( Content == null ) {
        return "null";
      }
      return $"\"{( (DateTime)Content ).ToString("s")}\"";
    }
  }
}
