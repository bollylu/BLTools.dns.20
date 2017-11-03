using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

    public JsonBool(string jsonBool) {
      Value = jsonBool.ToBool();
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if (formatted) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append(new JsonNull().RenderAsBytes());
      } else {
        RetVal.Append((bool)Value ? "true" : "false");
      }
      return RetVal.ToString();
    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( StreamWriter Writer = new StreamWriter(RetVal) ) {

          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}");
          }
          if ( Value == null ) {
            Writer.Write(new JsonNull().RenderAsBytes());
          } else {
            Writer.Write((bool)Value ? "true" : "false");
          }

          return RetVal.ToArray();
        }
      }
    }
  }
}
