using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Json {
  public class JsonValue {

    public static IJsonValue Parse(string source) {

      if ( string.IsNullOrWhiteSpace(source) ) {
        return JsonNull.Default;
      }

      string ProcessedSource = source.Trim();

      if ( ProcessedSource == Json.TRUE_VALUE || ProcessedSource == Json.FALSE_VALUE ) {
        return new JsonBool(ProcessedSource.ToBool());
      }

      if ( ProcessedSource == Json.NULL_VALUE ) {
        return JsonNull.Default;
      }

      char FirstChar = ProcessedSource.First();
      char LastChar = ProcessedSource.Last();

      if ( FirstChar == Json.CHR_DOUBLE_QUOTE && LastChar == Json.CHR_DOUBLE_QUOTE ) {
        return new JsonString(ProcessedSource.RemoveExternalQuotes());
      }

      if ( ProcessedSource.IsNumericOnly() ) {
        int Dummy;
        if ( int.TryParse(ProcessedSource, out Dummy) ) {
          return new JsonInt(ProcessedSource);
        }
        return new JsonLong(ProcessedSource);
      }

      if ( ProcessedSource.IsNumeric() ) {
        return new JsonDouble(ProcessedSource);
      }

      if ( FirstChar == Json.START_OF_ARRAY && LastChar == Json.END_OF_ARRAY ) {
        return JsonArray.Parse(ProcessedSource);
      }

      if ( FirstChar == Json.START_OF_OBJECT && LastChar == Json.END_OF_OBJECT ) {
        return JsonObject.Parse(ProcessedSource);
      }

      return JsonNull.Default;

    }
  }
}
