using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonDouble : IJsonValue {

    public double? Content { get; set; }


    public JsonDouble(double jsonDouble) {
      Content = jsonDouble;
    }

    public JsonDouble(JsonDouble jsonDouble) {
      Content = jsonDouble.Content;
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
