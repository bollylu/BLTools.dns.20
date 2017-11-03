using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonDateTime : IJsonValue {

    public DateTime? Value { get; set; }

    public JsonDateTime(DateTime jsonDateTime) {
      Value = jsonDateTime;
    }

    public JsonDateTime(JsonDateTime jsonDateTime) {
      Value = jsonDateTime.Value;
    }

    public void Dispose() {
      Value = null;
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append(new JsonNull().RenderAsString());
      } else {
        RetVal.Append($"\"{( (DateTime)Value ).ToString("s")}\"");
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
            Writer.Write($"\"{( (DateTime)Value ).ToString("s")}\"");
          }

          return RetVal.ToArray();
        }
      }
    }

  }
}
