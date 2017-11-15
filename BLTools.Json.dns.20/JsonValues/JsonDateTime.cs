using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonDateTime : IJsonValue<DateTime?> {

    public DateTime? Value { get; set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonDateTime(DateTime jsonDateTime) {
      Value = jsonDateTime;
    }

    public JsonDateTime(JsonDateTime jsonDateTime) {
      Value = jsonDateTime.Value;
    }

    public void Dispose() {
      Value = null;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(Value.HasValue ? Value.ToString() : "null");
      return RetVal.ToString();
    }

    public string RenderAsString(bool formatted = false, int indent = 0) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append(JsonNull.Default.RenderAsString());
      } else {
        RetVal.Append($"\"{( (DateTime)Value ).ToString("s")}\"");
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
            Writer.Write(JsonNull.Default.RenderAsBytes());
          } else {
            Writer.Write($"\"{( (DateTime)Value ).ToString("s")}\"");
          }

          return RetVal.ToArray();
        }
      }
    }

  }
}
