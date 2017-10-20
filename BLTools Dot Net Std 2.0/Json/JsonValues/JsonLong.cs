using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonLong : IJsonValue {

    public long? Content { get; set; }

    public JsonLong(long jsonLong) {
      Content = jsonLong;
    }

    public JsonLong(JsonLong jsonLong) {
      Content = jsonLong.Content;
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
