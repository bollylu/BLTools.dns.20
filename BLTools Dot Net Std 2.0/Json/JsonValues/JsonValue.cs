﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public static class JsonValue {
    public static IJsonValue Parse(string source) {

      if (string.IsNullOrWhiteSpace(source)) {
        return new JsonNull();
      }

      string ProcessedSource = source.Trim();

      if ( ProcessedSource == "true" || ProcessedSource == "false" ) {
        return new JsonBool(ProcessedSource.ToBool());
      }

      if ( ProcessedSource == "null" ) {
        return new JsonNull();
      }

      if ( ProcessedSource.StartsWith("\"") && ProcessedSource.EndsWith("\"")) {
        return new JsonString(ProcessedSource.RemoveExternalQuotes());
      }

      if (ProcessedSource.IsNumericOnly()) {
        return new JsonLong(ProcessedSource);
      }

      if ( ProcessedSource.IsNumeric() ) {
        return new JsonDouble(ProcessedSource);
      }

      if ( ProcessedSource.StartsWith("[") && ProcessedSource.EndsWith("]") ) {
        return new JsonArray(ProcessedSource);
      }

      if ( ProcessedSource.StartsWith("{") && ProcessedSource.EndsWith("}") ) {
        return new JsonObject(ProcessedSource);
      }

      return new JsonNull();

    }
  }
}
