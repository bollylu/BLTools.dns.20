using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLTools.Json {
  public class JsonString : IJsonValue {

    public string Value { get; set; }

    public JsonString(string jsonString)  {
      Value = jsonString;
    }

    public JsonString(JsonString jsonString) {
      Value = jsonString.Value;
    }

    public void Dispose() {
      Value = null;
    }


    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(Value);
      return RetVal.ToString();
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append(new JsonNull().RenderAsString());
      } else {
        RetVal.Append($"\"{Value.JsonEncode()}\"");
      }
      return RetVal.ToString();
    }


    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {

          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}");
          }
          if ( Value == null ) {
            Writer.Write(new JsonNull().RenderAsBytes());
          } else {
            Writer.Write($"\"{Value.JsonEncode()}\"");
          }

          return RetVal.ToArray();
        }
      }
    }
  }
}
