using System;
using System.Collections.Generic;
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

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString() {
      if ( Value == null ) {
        return "NaN";
      }
      return Value.ToString();
    }

  }
}
