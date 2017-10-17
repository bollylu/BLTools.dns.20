using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonObject : IDisposable {

    public readonly List<JsonObject> Items = new List<JsonObject>();
    public string JsonKey { get; set; }
    public JsonValue JsonContent { get; set; }

    private object _JsonLock = new object();

    public JsonObject() {
    }

    public JsonObject(string key, object value) {
      lock ( _JsonLock ) {
        JsonKey = key;
        JsonContent = new JsonValue(value);
        Items.Add(this);
      }
    }

    public JsonObject(object value) {
      lock ( _JsonLock ) {
        JsonContent = new JsonValue(value);
        Items.Add(this);
      }
    }

    public void Dispose() {
      lock ( _JsonLock ) {
        foreach ( JsonObject JsonObjectItem in Items ) {
          JsonObjectItem.Dispose();
        }
        Items.Clear();
      }
    }

    public void Clear() {
      lock ( _JsonLock ) {
        JsonKey = null;
        JsonContent = null;
        Items.Clear();
      }
    }

    public void AddItem(JsonObject jsonObject) {
      lock ( _JsonLock ) {
        Items.Add(jsonObject);
      }
    }

    public string ToJsonObjectString() {
      if ( Items.Count == 0 ) {
        return null;
      }

      lock ( _JsonLock ) {
        StringBuilder RetVal = new StringBuilder();

        RetVal.Append("{");
        foreach ( JsonObject JsonObjectItem in Items ) {
          if ( JsonObjectItem.JsonKey != null ) {
            RetVal.Append($"\"{JsonObjectItem.JsonKey}\":");
          }
          if ( JsonObjectItem.JsonContent.Content is JsonObject Component) {
            RetVal.Append(Component.ToJsonObjectString());
          } else {
            RetVal.Append(JsonObjectItem.JsonContent.JsonValueString());
          }
          RetVal.Append(",");
        }
        RetVal.Truncate(1);
        RetVal.Append("}");


        return RetVal.ToString();
      }

    }


  }
}
