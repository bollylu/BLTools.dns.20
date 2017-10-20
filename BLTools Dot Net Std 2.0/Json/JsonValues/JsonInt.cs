using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonInt : IJsonValue {

    public int? Value { get; set; }

    public JsonInt(int jsonInt) {
      Value = jsonInt;
    }

    public JsonInt(JsonInt jsonInt) {
      Value = jsonInt.Value;
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
