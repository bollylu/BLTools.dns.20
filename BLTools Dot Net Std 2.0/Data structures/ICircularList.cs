using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Collections {
  public interface ICircularList<T> : IList<T> {

    bool IsCircular { get; set; }
    T GetNext();
    T GetPrevious();
    void ResetIndex();

  }
}
