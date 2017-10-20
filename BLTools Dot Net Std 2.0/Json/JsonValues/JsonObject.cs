using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonObject : IJsonValue {

    public readonly JsonPairCollection KeyValuePairs = new JsonPairCollection();

    private object _JsonLock = new object();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonObject(IJsonPair jsonPair) {
      KeyValuePairs.Add(jsonPair);
    }

    public JsonObject(JsonObject jsonObject) {
      foreach(IJsonPair JsonPairItem in jsonObject.KeyValuePairs) {
        KeyValuePairs.Add(JsonPairItem);
      }
    }

    public void Dispose() {
      lock ( _JsonLock ) {
        KeyValuePairs.Dispose();
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public virtual void AddItem(IJsonPair jsonObject) {
      lock ( _JsonLock ) {
        KeyValuePairs.Add(jsonObject);
      }
    }

    public void Clear() {
      lock ( _JsonLock ) {
        KeyValuePairs.Clear();
      }
    }

    public string RenderAsString() {
      if ( KeyValuePairs.Count() == 0 ) {
        return "null";
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        RetVal.Append("{");

        foreach ( IJsonPair JsonPairItem in KeyValuePairs ) {
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
