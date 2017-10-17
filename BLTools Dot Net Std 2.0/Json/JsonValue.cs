using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonValue {
    public object Content { get; protected set; }

    public JsonValue(object source) {
      Content = source;
    }
    public string JsonValueString() {

      switch ( Content ) {
        case string StringSource:
          return $"\"{StringSource}\"";

        case int IntSource:
          return $"{IntSource.ToString()}";

        case long LongSource:
          return $"{LongSource.ToString()}";

        case float FloatSource:
          return $"{FloatSource.ToString()}";

        case double DoubleSource:
          return $"{DoubleSource.ToString()}";

        case bool BoolSource:
          return $"{ ( BoolSource ? "true" : "false" )}";

        case DateTime DateTimeSource:
          return $"\"{DateTimeSource.ToString()}\"";

        case null:
          return "null";

        default:
          return "";
      }
    }
  }
}
