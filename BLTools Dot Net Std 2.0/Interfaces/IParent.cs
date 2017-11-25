using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {
  public interface IParent {
    IParent Parent { get; }
    T GetParent<T>() where T : class, IParent;
  }
}
