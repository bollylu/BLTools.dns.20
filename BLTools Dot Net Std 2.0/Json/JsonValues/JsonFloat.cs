using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonFloat : IJsonValue {

    public float? Content { get; set; }


    public JsonFloat(float jsonFloat) {
      Content = jsonFloat;
    }

    public JsonFloat(JsonFloat jsonFloat) {
      Content = jsonFloat.Content;
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
