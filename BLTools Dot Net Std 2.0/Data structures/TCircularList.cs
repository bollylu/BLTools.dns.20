using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Collections {
  public class TCircularList<T> : List<T>, ICircularList<T> {

    public bool IsCircular { get; set; }

    private int CurrentIndex = -1;

    public T GetNext() {
      if (this.IsEmpty()) {
        return default(T);
      }
      CurrentIndex++;
      if (CurrentIndex >= this.Count && !IsCircular) {
        return default(T);
      }
      if (CurrentIndex >= this.Count) {
        CurrentIndex = 0;
      }
      return this[CurrentIndex];

    }

    public T GetPrevious() {
      if (!this.Any()) {
        return default(T);
      }
      CurrentIndex--;
      if (CurrentIndex < 0 && !IsCircular) {
        return default(T);
      }
      if (CurrentIndex < 0) {
        CurrentIndex = this.Count - 1;
      }
      return this[CurrentIndex];
    }

    public void ResetIndex() {
      CurrentIndex = -1;
    }

  }
}
