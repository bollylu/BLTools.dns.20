using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public class JsonPair<T> : IDisposable, IJsonPair where T : IJsonValue {

    public string Key { get; private set; }
    public T Content { get; private set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonPair(string key, T jsonValue) {
      Key = key;
      Content = jsonValue;
    }

    public JsonPair(JsonPair<T> jsonPair) {
      _Initialize(jsonPair);
    }

    public JsonPair(string key, string content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonString(content), typeof(T))));
    }

    public JsonPair(string key, int content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonInt(content), typeof(T))));
    }

    public JsonPair(string key, long content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonLong(content), typeof(T))));
    }

    public JsonPair(string key, float content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonFloat(content), typeof(T))));
    }

    public JsonPair(string key, double content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonDouble(content), typeof(T))));
    }

    public JsonPair(string key, DateTime content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonDateTime(content), typeof(T))));
    }

    public JsonPair(string key, bool content) {
      _Initialize(new JsonPair<T>(key, (T)Convert.ChangeType(new JsonBool(content), typeof(T))));
    }

    private void _Initialize(JsonPair<T> jsonPair) {
      Key = jsonPair.Key;
      Content = jsonPair.Content;
    }

    public void Dispose() {
      Key = null;
      Content.Dispose();
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public string RenderAsString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"\"{Key}\":");
      RetVal.Append(Content.RenderAsString());
      return RetVal.ToString();
    }

    
  }
}
