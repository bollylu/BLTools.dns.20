using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonInt : IJsonValue {

    public int? Content { get; set; }

    public JsonInt(int jsonInt) {
      Content = jsonInt;
    }

    public JsonInt(JsonInt jsonInt) {
      Content = jsonInt.Content;
    }

    public void Dispose() {
      Content = null;
    }

    public string RenderAsString() {
      if ( Content == null ) {
        return "NaN";
      }
      return Content.ToString();
    }

  }
}
