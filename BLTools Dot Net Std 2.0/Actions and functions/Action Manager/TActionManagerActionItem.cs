using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools
{
    public class TActionManagerActionItem<X> : AActionManagerItem<X>
    {
        public Action<X> CodeBlock { get; }

        public TActionManagerActionItem(string name, Predicate<X> predicate, Action<X> action) : base(name, predicate)
        {
            CodeBlock = action;
        }

        public IActionManagerReturnValue ConditionalExecute(X arg)
        {
            if ( MatchCondition(arg) )
            {
                CodeBlock(arg);
                return TActionManagerReturnValue.Executed;
            }
            return TActionManagerReturnValue.Skipped;
        }
    }

    public class TActionManagerActionItem<X1, X2> : AActionManagerItem<X1>
    {
        public Action<X1, X2> CodeBlock { get; }

        public TActionManagerActionItem(string name, Predicate<X1> predicate, Action<X1, X2> action) : base(name, predicate)
        {
            CodeBlock = action;
        }

        public IActionManagerReturnValue ConditionalExecute(X1 arg1, X2 arg2)
        {
            if ( MatchCondition(arg1) )
            {
                CodeBlock(arg1, arg2);
                return TActionManagerReturnValue.Executed;
            }
            return TActionManagerReturnValue.Skipped;
        }
    }

    public class TActionManagerActionItem<X1, X2, X3> : AActionManagerItem<X1>
    {
        public Action<X1, X2, X3> CodeBlock { get; }

        public TActionManagerActionItem(string name, Predicate<X1> predicate, Action<X1, X2, X3> action) : base(name, predicate)
        {
            CodeBlock = action;
        }

        public IActionManagerReturnValue ConditionalExecute(X1 arg1, X2 arg2, X3 arg3)
        {
            if ( MatchCondition(arg1) )
            {
                CodeBlock(arg1, arg2, arg3);
                return TActionManagerReturnValue.Executed;
            }
            return TActionManagerReturnValue.Skipped;
        }
    }

    public class TActionManagerActionItem<X1, X2, X3, X4> : AActionManagerItem<X1>
    {
        public Action<X1, X2, X3, X4> CodeBlock { get; }

        public TActionManagerActionItem(string name, Predicate<X1> predicate, Action<X1, X2, X3, X4> action) : base(name, predicate)
        {
            CodeBlock = action;
        }

        public IActionManagerReturnValue ConditionalExecute(X1 arg1, X2 arg2, X3 arg3, X4 arg4)
        {
            if ( MatchCondition(arg1) )
            {
                CodeBlock(arg1, arg2, arg3, arg4);
                return TActionManagerReturnValue.Executed;
            }
            return TActionManagerReturnValue.Skipped;
        }
    }
}
