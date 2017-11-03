using System;
using System.Collections.Generic;
using System.IO;
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

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {
          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}null");
            return RetVal.ToArray();
          } else {
            Writer.Write("null");
            return RetVal.ToArray();
          }
        }
      }
    }
  }
}
