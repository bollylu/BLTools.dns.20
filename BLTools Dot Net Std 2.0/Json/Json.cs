using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BLTools;

namespace BLTools.Json {
  public static class Json {

    public readonly static char[] WhiteSpaces = new char[] { ' ', '\b', '\t', '\r', '\n' };

    public const char InnerFieldSeparator = ':';

    public const char OuterFieldSeparator = ',';

    public const int DEFAULT_INDENT = 2;

    private const char CHR_SLASH = '/';
    private const char CHR_BACKSLASH = '\\';
    private const char CHR_DOUBLE_QUOTE = '\"';

    public static string JsonEncode(this string source) {

      if ( string.IsNullOrEmpty(source) ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder();

      int i = 0;
      int LengthOfSource = source.Length;
      bool InQuote = false;
      bool IsEscapeChar = false;

      do {

        char CurrentChar = source[i];
        Trace.WriteLine($"Found [{CurrentChar.ToString().ToByteArray().ToHexString()}],[{CurrentChar}]");


        if ( CurrentChar == CHR_DOUBLE_QUOTE && !InQuote ) {
          InQuote = true;
          Trace.WriteLine("InQuote=true");
          RetVal.Append(CHR_DOUBLE_QUOTE);
          i++;
          continue;
        }

        if ( CurrentChar == CHR_BACKSLASH && InQuote && IsEscapeChar) {
          RetVal.Append(CHR_BACKSLASH);
          IsEscapeChar = false;
          Trace.WriteLine("IsEscapeChar=false");
          i++;
          continue;
        }

        if (CurrentChar == CHR_BACKSLASH && InQuote) {
          IsEscapeChar = true;
          RetVal.Append(CHR_BACKSLASH);
          Trace.WriteLine("IsEscapeChar=true");
          i++;
          continue;
        }

        if ( "\t\b\r\n\f/".Contains(CurrentChar) && InQuote) {
          RetVal = RetVal.Append(CHR_BACKSLASH).Append(CurrentChar);
          Trace.WriteLine($"Adding control char {CurrentChar.ToString()}");
          i++;
          continue;
        }
        if ( "tbrnf/".Contains(CurrentChar) && IsEscapeChar ) {
          RetVal = RetVal.Append(CurrentChar);
          Trace.WriteLine($"Adding control char {CurrentChar.ToString()}");
          i++;
          continue;
        }

        if ( CurrentChar == CHR_DOUBLE_QUOTE && InQuote && IsEscapeChar) {
          RetVal.Append(CHR_DOUBLE_QUOTE);
          Trace.WriteLine("Adding double quote inside quote");
          i++;
          continue;
        }

        if ( CurrentChar == CHR_DOUBLE_QUOTE && InQuote) {
          InQuote = false;
          Trace.WriteLine("InQuote=false");
          RetVal.Append(CHR_DOUBLE_QUOTE);
          i++;
          continue;
        }

        RetVal = RetVal.Append(CurrentChar);
        i++;

        Trace.WriteLine($"{RetVal.ToString()}");

      } while ( i < LengthOfSource );

      return RetVal.ToString();
    } 

    public static string JsonDecode(this string source) {

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

        if ( InQuote ) {

          if ( !NextCharIsControlChar && CurrentChar == '\\' ) {
            NextCharIsControlChar = true;
            i++;
            continue;
          }

          if ( NextCharIsControlChar && "\"\\\t\b\r\n\f/".Contains(CurrentChar) ) {
            NextCharIsControlChar = false;
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