using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonObject : IJsonValue {

    public readonly JsonPairCollection Items = new JsonPairCollection();

    private object _JsonLock = new object();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonObject(IJsonPair jsonPair) {
      Items.Add(jsonPair);
    }

    public JsonObject(JsonObject jsonObject) {
      foreach(IJsonPair JsonPairItem in jsonObject.Items) {
        Items.Add(JsonPairItem);
      }
    }

    public void Dispose() {
      lock ( _JsonLock ) {
        Items.Dispose();
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public virtual void AddItem(IJsonPair jsonObject) {
      lock ( _JsonLock ) {
        Items.Add(jsonObject);
      }
    }

    public void Clear() {
      lock ( _JsonLock ) {
        Items.Clear();
      }
    }

    public string RenderAsString() {
      if ( Items.Count() == 0 ) {
        return "null";
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        RetVal.Append("{");

        foreach ( IJsonPair JsonPairItem in Items ) {
          RetVal.Append(JsonPairItem.RenderAsString());
          RetVal.Append(",");
        }
        RetVal.Truncate(1);

        RetVal.Append("}");

        return RetVal.ToString();
      }

    }


  }
}
