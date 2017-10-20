using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonBool : IJsonValue {

    public bool? Content { get; set; }

    public JsonBool(bool jsonBool) {
      Content = jsonBool;
    }

    public JsonBool(JsonBool jsonBool) {
      Content = jsonBool.Content;
    }

    public void Dispose() {
      Content = null;
    }

    public string RenderAsString() {
      if ( Content == null ) {
        return "null";
      }
      return (bool)Content ? "true" : "false";
    }
  }
}
