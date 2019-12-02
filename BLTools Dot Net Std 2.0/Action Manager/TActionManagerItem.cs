using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {
  public class TActionManagerItem {
    public string Name { get; }
    public TActionManagerItem(string name) {
      Name = name;
    }
  }

  public class TActionManagerItem<T> : TActionManagerItem {
    public Predicate<T> MatchCondition { get; }

    public TActionManagerItem(string name, Predicate<T> predicate) : base(name) {
      MatchCondition = predicate;
    }
  }


  public class TActionManagerActionItem<X> : TActionManagerItem<X> {
    public Action<X> CodeBlock { get; }

    public TActionManagerActionItem(string name, Predicate<X> predicate, Action<X> action) : base(name, predicate) {
      CodeBlock = action;
    }

    public IActionManagerReturnValue ConditionalExecute(X arg) {
      if (MatchCondition(arg)) {
        CodeBlock(arg);
        return TActionManagerReturnValue.Executed;
      }
      return TActionManagerReturnValue.Skipped;
    }
  }

  public class TActionManagerActionItem<X1, X2> : TActionManagerItem<X1> {
    public Action<X1, X2> CodeBlock { get; }

    public TActionManagerActionItem(string name, Predicate<X1> predicate, Action<X1, X2> action) : base(name, predicate) {
      CodeBlock = action;
    }

    public IActionManagerReturnValue ConditionalExecute(X1 arg1, X2 arg2) {
      if (MatchCondition(arg1)) {
        CodeBlock(arg1, arg2);
        return TActionManagerReturnValue.Executed;
      }
      return TActionManagerReturnValue.Skipped;
    }
  }

  public class TActionManagerActionItem<X1, X2, X3> : TActionManagerItem<X1> {
    public Action<X1, X2, X3> CodeBlock { get; }

    public TActionManagerActionItem(string name, Predicate<X1> predicate, Action<X1, X2, X3> action) : base(name, predicate) {
      CodeBlock = action;
    }

    public IActionManagerReturnValue ConditionalExecute(X1 arg1, X2 arg2, X3 arg3) {
      if (MatchCondition(arg1)) {
        CodeBlock(arg1, arg2, arg3);
        return TActionManagerReturnValue.Executed;
      }
      return TActionManagerReturnValue.Skipped;
    }
  }

  public class TActionManagerActionItem<X1, X2, X3, X4> : TActionManagerItem<X1> {
    public Action<X1, X2, X3, X4> CodeBlock { get; }

    public TActionManagerActionItem(string name, Predicate<X1> predicate, Action<X1, X2, X3, X4> action) : base(name, predicate) {
      CodeBlock = action;
    }

    public IActionManagerReturnValue ConditionalExecute(X1 arg1, X2 arg2, X3 arg3, X4 arg4) {
      if (MatchCondition(arg1)) {
        CodeBlock(arg1, arg2, arg3, arg4);
        return TActionManagerReturnValue.Executed;
      }
      return TActionManagerReturnValue.Skipped;
    }
  }



  public class TActionManagerFuncItem<X, R> : TActionManagerItem<X> {
    public Func<X, R> CodeBlock { get; }

    public TActionManagerFuncItem(string name, Predicate<X> predicate, Func<X, R> func) : base(name, predicate) {
      CodeBlock = func;
    }

    public IActionManagerReturnValue<R> ConditionalExecute(X arg) {
      if (MatchCondition(arg)) {
        return TActionManagerReturnValue<R>.Executed(CodeBlock(arg));
      }

      return TActionManagerReturnValue<R>.Skipped;
    }
  }

  public class TActionManagerFuncItem<X, Y, R> : TActionManagerItem<X> {
    public Func<X, Y, R> CodeBlock { get; }

    public TActionManagerFuncItem(string name, Predicate<X> predicate, Func<X, Y, R> func) : base(name, predicate) {
      CodeBlock = func;
    }

    public IActionManagerReturnValue<R> ConditionalExecute(X arg1, Y arg2) {
      if (MatchCondition(arg1)) {
        return TActionManagerReturnValue<R>.Executed(CodeBlock(arg1, arg2));
      }

      return TActionManagerReturnValue<R>.Skipped;
    }
  }

  public class TActionManagerFuncItem<X1, X2, X3, R> : TActionManagerItem<X1> {
    public Func<X1, X2, X3, R> CodeBlock { get; }

    public TActionManagerFuncItem(string name, Predicate<X1> predicate, Func<X1, X2, X3, R> func) : base(name, predicate) {
      CodeBlock = func;
    }

    public IActionManagerReturnValue<R> ConditionalExecute(X1 arg1, X2 arg2, X3 arg3) {
      if (MatchCondition(arg1)) {
        return TActionManagerReturnValue<R>.Executed(CodeBlock(arg1, arg2, arg3));
      }

      return TActionManagerReturnValue<R>.Skipped;
    }
  }

  public class TActionManagerFuncItem<X1, X2, X3, X4, R> : TActionManagerItem<X1> {
    public Func<X1, X2, X3, X4, R> CodeBlock { get; }

    public TActionManagerFuncItem(string name, Predicate<X1> predicate, Func<X1, X2, X3, X4, R> func) : base(name, predicate) {
      CodeBlock = func;
    }

    public IActionManagerReturnValue<R> ConditionalExecute(X1 arg1, X2 arg2, X3 arg3, X4 arg4) {
      if (MatchCondition(arg1)) {
        return TActionManagerReturnValue<R>.Executed(CodeBlock(arg1, arg2, arg3, arg4));
      }

      return TActionManagerReturnValue<R>.Skipped;
    }
  }
}
