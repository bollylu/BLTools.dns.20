using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {

  public class TActionManagerReturnValue : IActionManagerReturnValue {
    public bool WasExecuted { get; }

    public TActionManagerReturnValue(bool wasExecuted) {
      WasExecuted = wasExecuted;
    }

    public static TActionManagerReturnValue Executed {
      get {
        if (_Executed == null) {
          _Executed = new TActionManagerReturnValue(true);
        }
        return _Executed;
      }
    }
    private static TActionManagerReturnValue _Executed;

    public static TActionManagerReturnValue Skipped {
      get {
        if (_Skipped == null) {
          _Skipped = new TActionManagerReturnValue(false);
        }
        return _Skipped;
      }
    }
    private static TActionManagerReturnValue _Skipped;
  }

  public class TActionManagerReturnValue<T> : TActionManagerReturnValue, IActionManagerReturnValue<T> {
    public T Value { get; }

    public TActionManagerReturnValue(bool wasExecuted, T retValue) : base(wasExecuted) {
      Value = retValue;
    }

    public static new TActionManagerReturnValue<T> Executed(T retValue) => new TActionManagerReturnValue<T>(true, retValue);
    public static new TActionManagerReturnValue<T> Skipped => new TActionManagerReturnValue<T>(false, default(T));
  }
}
