using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public static class Json {

    public readonly static char[] WhiteSpaces = new char[] { ' ', '\b', '\t', '\r', '\n' };

    public const char InnerFieldSeparator = ':';

    public const char OuterFieldSeparator = ',';

    public const int DEFAULT_INDENT = 2;

  }
}
