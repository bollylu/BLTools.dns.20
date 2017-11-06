using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Json {
  public static class Json {

    public readonly static char[] WhiteSpaces = new char[] { ' ', '\b', '\t', '\r', '\n' };

    public const char InnerFieldSeparator = ':';

    public const char OuterFieldSeparator = ',';

    public const int DEFAULT_INDENT = 2;

    public static string JsonEncode(this string source) {

      if ( string.IsNullOrEmpty(source) ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder();

      int i = 0;
      bool InQuote = false;
      bool NextCharIsControlChar = false;
      int LengthOfSource = source.Length;

      while ( i < LengthOfSource ) {

        char CurrentChar = source[i];

        if (InQuote) {

          if ( !NextCharIsControlChar && CurrentChar == '\\' ) {
            NextCharIsControlChar = true;
            i++;
            continue;
          }

          if ( NextCharIsControlChar && "\"\\\t\b\r\n\f".Contains(CurrentChar) ) {
            NextCharIsControlChar = false;
            RetVal.Append('\\');
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if ( CurrentChar == '/' ) {
            RetVal.Append('\\');
            RetVal.Append(CurrentChar);
            i++;
            continue;
          }

          if ( CurrentChar == '"' ) {
            RetVal.Append(CurrentChar);
            InQuote = false;
            i++;
            continue;
          }

          if ( CurrentChar == '"' ) {
            RetVal.Append(CurrentChar);
            InQuote = true;
            i++;
            continue;
          }

          RetVal.Append(CurrentChar);
          i++;
          continue;
        }

        RetVal.Append(CurrentChar);
        i++;
      }

      return RetVal.ToString();

    }
  }
}