using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

    public JsonFloat(string jsonFloat) {
      try {
        Value = float.Parse(jsonFloat, CultureInfo.InvariantCulture);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse float : {jsonFloat} : {ex.Message}");
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
      return ( (float)Value ).ToString(CultureInfo.InvariantCulture);
    }

  }
}
