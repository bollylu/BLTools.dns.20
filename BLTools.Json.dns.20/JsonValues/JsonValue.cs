using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonValue {

    public static IJsonValue Parse(string source) {

      if (string.IsNullOrWhiteSpace(source)) {
        return JsonNull.Default;
      }

      string ProcessedSource = source.Trim();

      if ( ProcessedSource == "true" || ProcessedSource == "false" ) {
        return new JsonBool(ProcessedSource.ToBool());
      }

      if ( ProcessedSource == "null" ) {
        return JsonNull.Default;
      }

      if ( ProcessedSource.StartsWith("\"") && ProcessedSource.EndsWith("\"")) {
        return new JsonString(ProcessedSource.RemoveExternalQuotes());
      }

      if (ProcessedSource.IsNumericOnly()) {
        int Dummy;
        if (int.TryParse(ProcessedSource, out Dummy)) {
          return new JsonInt(ProcessedSource);
        }
        return new JsonLong(ProcessedSource);
      }

      if ( ProcessedSource.IsNumeric() ) {
        return new JsonDouble(ProcessedSource);
      }

      if ( ProcessedSource.StartsWith("[") && ProcessedSource.EndsWith("]") ) {
        return JsonArray.Parse(ProcessedSource);
      }

      if ( ProcessedSource.StartsWith("{") && ProcessedSource.EndsWith("}") ) {
        return JsonObject.Parse(ProcessedSource);
      }

      return JsonNull.Default;

    }
  }
}
