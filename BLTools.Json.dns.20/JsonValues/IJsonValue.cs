using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public interface IJsonValue : IDisposable {

    string RenderAsString(bool formatted = false, int indent = 0);
    byte[] RenderAsBytes(bool formatted = false, int indent = 0);

  }

  public interface IJsonValue<T> : IJsonValue {
    T Value { get; set; }
  }
}
