using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonString : IJsonValue {

    public string Content { get; set; }

    public JsonString(string jsonString)  {
      Content = jsonString;
    }

    public JsonString(JsonString jsonString) {
      Content = jsonString.Content;
    }

    public void Dispose() {
      Content = null;
    }

    public string RenderAsString() {
      if ( Content == null ) {
        return "null";
      }
      return $"\"{Content}\"";
    }
  }
}
