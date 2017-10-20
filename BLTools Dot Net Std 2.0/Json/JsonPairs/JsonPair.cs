using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonPair<T> : IDisposable, IJsonPair where T : IJsonValue {

    public string Key { get; private set; }
    public T Content { get; private set; }

    public JsonPair(string key, T jsonValue) {
      Key = key;
      Content = jsonValue;
    }

    public JsonPair(JsonPair<T> jsonPair) {
      Key = jsonPair.Key;
      Content = jsonPair.Content;
    }

    public JsonPair(string key, string content) {
      Key = key;
    }

    public string RenderAsString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"\"{Key}\":");
      RetVal.Append(Content.RenderAsString());
      return RetVal.ToString();
    }

    public void Dispose() {
      Key = null;
      Content.Dispose();
    }
  }
}
