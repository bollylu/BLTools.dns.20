using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public interface IJsonValue : IDisposable {

    string RenderAsString();

  }
}
