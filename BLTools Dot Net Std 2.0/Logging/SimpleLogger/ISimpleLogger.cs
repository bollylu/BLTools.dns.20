using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools
{
    [Obsolete("Use ILogger instead")]
    public interface ISimpleLogger
    {
        void Write(string message);
        void Write(string message, ErrorLevel severity);
    }
}
