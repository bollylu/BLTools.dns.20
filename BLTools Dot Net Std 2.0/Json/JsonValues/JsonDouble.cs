using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonDouble : IJsonValue {

    public double? Value { get; set; }


    public JsonDouble(double jsonDouble) {
      Value = jsonDouble;
    }

    public JsonDouble(JsonDouble jsonDouble) {
      Value = jsonDouble.Value;
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
