using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonArray : IJsonValue {

    private object _JsonLock = new object();

    public readonly List<IJsonValue> Items = new List<IJsonValue>();

    public JsonArray(IEnumerable<IJsonValue> values) {
      if ( values == null ) {
        Trace.WriteLine("Unable to create JsonArray : values are null");
        return;
      }

      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values ) {
          Items.Add(SourceItem);
        }
      }
    }

    public JsonArray(params IJsonValue[] values) {
      if ( values == null ) {
        Trace.WriteLine("Unable to create JsonArray : values are null");
        return;
      }

      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values ) {
          Items.Add(SourceItem);
        }
      }
    }

    public JsonArray(JsonArray values) {
      if (values==null) {
        return;
      }
      lock ( _JsonLock ) {
        foreach ( IJsonValue SourceItem in values.Items ) {
          Items.Add(SourceItem);
        }
      }
    }

    public string RenderAsString() {
      if ( Items.Count() == 0 ) {
        return null;
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        RetVal.Append("[");

        foreach ( IJsonValue JsonValueItem in Items ) {
          RetVal.Append(JsonValueItem.RenderAsString());
          RetVal.Append(",");
        }
        RetVal.Truncate(1);

        RetVal.Append("]");

        return RetVal.ToString();
      }
    }

    public void Dispose() {
      Items.Clear();
    }
  }
}
