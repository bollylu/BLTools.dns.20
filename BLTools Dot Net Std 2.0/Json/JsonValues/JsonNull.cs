using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonNull : IJsonValue {
    public object Value { get => null; }

    public void Dispose() {
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      if ( formatted ) {
        return $"{StringExtension.Spaces(indent)}null";
      } else {
        return "null";
      }
    }
  }
}
