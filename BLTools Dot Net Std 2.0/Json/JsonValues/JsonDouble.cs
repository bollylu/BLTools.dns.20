using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonDouble : IJsonValue {

    public double? Value { get; set; }

    public JsonDouble(double jsonDouble) {
      Value = jsonDouble;
    }

    public JsonDouble(JsonDouble jsonDouble) {
      Value = jsonDouble.Value;
    }

    public JsonDouble(string jsonDouble) {
      try {
        Value = double.Parse(jsonDouble, CultureInfo.InvariantCulture);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse double : {jsonDouble} : {ex.Message}");
        Value = null;
      }
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
        RetVal.Append("NaN");
      } else {
        RetVal.Append(( (double)Value ).ToString(CultureInfo.InvariantCulture));
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
            Writer.Write("NaN");
          } else {
            Writer.Write(( (double)Value ).ToString(CultureInfo.InvariantCulture));
          }

          return RetVal.ToArray();
        }
      }
    }

  }
}
