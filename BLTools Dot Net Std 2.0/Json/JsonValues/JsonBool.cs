﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonBool : IJsonValue {

    public bool? Value { get; set; }

    public JsonBool(bool jsonBool) {
      Value = jsonBool;
    }

    public JsonBool(JsonBool jsonBool) {
      Value = jsonBool.Value;
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString() {
      if ( Value == null ) {
        return "null";
      }
      return (bool)Value ? "true" : "false";
    }
  }
}