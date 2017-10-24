using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

    public JsonDouble(string jsonDouble) {
      try {
        Value = double.Parse(jsonDouble, CultureInfo.InvariantCulture);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse double : {jsonDouble} : {ex.Message}");
        Value = null;
      }
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString() {
      if ( Value == null ) {
        return "NaN";
      }
      return ( (double)Value ).ToString(CultureInfo.InvariantCulture);
    }

  }
}
