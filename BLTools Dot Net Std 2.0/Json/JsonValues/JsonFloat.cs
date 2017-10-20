using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonFloat : IJsonValue {

    public float? Value { get; set; }


    public JsonFloat(float jsonFloat) {
      Value = jsonFloat;
    }

    public JsonFloat(JsonFloat jsonFloat) {
      Value = jsonFloat.Value;
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
