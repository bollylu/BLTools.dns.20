using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {
  public interface IParent {
    IParent Parent { get; set; }
    IParent GetParent<T>() where T : IParent;
  }
}
