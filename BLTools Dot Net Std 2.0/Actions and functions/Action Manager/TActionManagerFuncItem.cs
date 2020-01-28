using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools
{
    public class TActionManagerFuncItem<X, R> : AActionManagerItem<X> where R : IActionManagerReturnValue<R>
    {
        public Func<X, R> CodeBlock { get; }

        public TActionManagerFuncItem(string name, Predicate<X> predicate, Func<X, R> func) : base(name, predicate)
        {
            CodeBlock = func;
        }

        public IActionManagerReturnValue<R> ConditionalExecute(X arg)
        {
            if ( MatchCondition(arg) )
            {
                return TActionManagerReturnValue<R>.Executed(CodeBlock(arg));
            }

            return TActionManagerReturnValue<R>.Skipped;
        }
    }

    public class TActionManagerFuncItem<X1, X2, R> : AActionManagerItem<X1>
    {
        public Func<X1, X2, R> CodeBlock { get; }

        public TActionManagerFuncItem(string name, Predicate<X1> predicate, Func<X1, X2, R> func) : base(name, predicate)
        {
            CodeBlock = func;
        }

        public IActionManagerReturnValue<R> ConditionalExecute(X1 arg1, X2 arg2)
        {
            if ( MatchCondition(arg1) )
            {
                return TActionManagerReturnValue<R>.Executed(CodeBlock(arg1, arg2));
            }

            return TActionManagerReturnValue<R>.Skipped;
        }
    }

    public class TActionManagerFuncItem<X1, X2, X3, R> : AActionManagerItem<X1>
    {
        public Func<X1, X2, X3, R> CodeBlock { get; }

        public TActionManagerFuncItem(string name, Predicate<X1> predicate, Func<X1, X2, X3, R> func) : base(name, predicate)
        {
            CodeBlock = func;
        }

        public IActionManagerReturnValue<R> ConditionalExecute(X1 arg1, X2 arg2, X3 arg3)
        {
            if ( MatchCondition(arg1) )
            {
                return TActionManagerReturnValue<R>.Executed(CodeBlock(arg1, arg2, arg3));
            }

            return TActionManagerReturnValue<R>.Skipped;
        }
    }

    public class TActionManagerFuncItem<X1, X2, X3, X4, R> : AActionManagerItem<X1>
    {
        public Func<X1, X2, X3, X4, R> CodeBlock { get; }

        public TActionManagerFuncItem(string name, Predicate<X1> predicate, Func<X1, X2, X3, X4, R> func) : base(name, predicate)
        {
            CodeBlock = func;
        }

        public IActionManagerReturnValue<R> ConditionalExecute(X1 arg1, X2 arg2, X3 arg3, X4 arg4)
        {
            if ( MatchCondition(arg1) )
            {
                return TActionManagerReturnValue<R>.Executed(CodeBlock(arg1, arg2, arg3, arg4));
            }

            return TActionManagerReturnValue<R>.Skipped;
        }
    }
}
