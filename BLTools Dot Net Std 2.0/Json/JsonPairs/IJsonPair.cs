using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public interface IJsonPair : IDisposable {
    string Key { get; }
    string RenderAsString();
  }
}
