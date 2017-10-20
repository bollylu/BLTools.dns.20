using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  class JsonNull : IJsonValue {
    public object Value { get => null; }

    public void Dispose() {
    }

    public string RenderAsString() {
      return "null";
    }
  }
}
