using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools
{
    public interface IActionManagerReturnValue
    {
        bool WasExecuted { get; }
    }

    public interface IActionManagerReturnValue<T> : IActionManagerReturnValue
    {
        T Value { get; }
    }
}
