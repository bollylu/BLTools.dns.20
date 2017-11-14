using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BLTools;
using System.IO;

namespace BLTools.Json {
  public static class Json {

    public readonly static char[] WhiteSpaces = new char[] { ' ', '\b', '\t', '\r', '\n' };

    public const char InnerFieldSeparator = ':';

    public const char OuterFieldSeparator = ',';

    public const int DEFAULT_INDENT = 2;

    private const char CHR_SLASH = '/';
    private const char CHR_BACKSLASH = '\\';
    private const char CHR_DOUBLE_QUOTE = '\"';
    private const char CHR_BACKSPACE = '\b';
    private const char CHR_TAB = '\t';
    private const char CHR_LF = '\n';
    private const char CHR_CR = '\r';
    private const char CHR_FORMFEED = '\f';

    public static string JsonEncode(this string source) {

      if ( source == null ) {
        return null;
      }

      if ( source == "" ) {
        return "";
      }

      string ProcessedSource = source.Replace("\\", "\\\\")
                                     .Replace("/", "\\/")
                                     .Replace("\t", "\\t")
                                     .Replace("\n", "\\n")
                                     .Replace("\r", "\\r")
                                     .Replace("\f", "\\f")
                                     .Replace("\b", "\\b")
                                     .Replace("\\\"", "\\\\\"")
                                     ;
      return ProcessedSource;

      //StringBuilder RetVal = new StringBuilder();
      //using ( StringWriter Writer = new StringWriter(RetVal) ) {
      //  Writer.Write(ProcessedSource);
      //  Writer.Flush();
      //  return Writer.ToString();
      //}

      //int i = 0;

      //int InQuoteLevel = 0;
      ////bool IsEscapeChar = false;

      //string ProcessedSource = source.ToString();
      //int LengthOfSource = ProcessedSource.Length;

      //do {

      //  char CurrentChar = ProcessedSource[i];
      //  Trace.WriteLine($"Found [{CurrentChar.ToString().ToByteArray().ToHexString()}],[{CurrentChar}]");


      //  if ( CurrentChar == CHR_DOUBLE_QUOTE && InQuoteLevel == 0 ) {
      //    InQuoteLevel++;
      //    Trace.WriteLine("InQuote=true");
      //    RetVal.Append(CHR_DOUBLE_QUOTE);
      //    i++;
      //    continue;
      //  }

      //  //if ( CurrentChar == CHR_BACKSLASH && !InQuoteLevel ) {
      //  //  Trace.WriteLine("Unable to JsonEncode string : invalid json string");
      //  //  return null;
      //  //}

      //  #region --- Control chars --------------------------------------------
      //  if ( CurrentChar == CHR_BACKSLASH ) {
      //    i++;
      //    if (i<LengthOfSource) {
      //      CurrentChar = ProcessedSource[i];
      //      if ( CurrentChar == '"' ) {
      //        RetVal.Append(CHR_BACKSLASH).Append(CHR_BACKSLASH).Append(CHR_BACKSLASH).Append(CHR_DOUBLE_QUOTE);
      //        i++;
      //        continue;
      //      }
      //    }
      //    RetVal.Append(CHR_BACKSLASH).Append(CHR_BACKSLASH);
      //    i++;
      //    continue;
      //  }

      //  if ( CurrentChar == CHR_LF ) {
      //    RetVal.Append(CHR_BACKSLASH).Append('n');
      //    i++;
      //    continue;
      //  }

      //  if ( CurrentChar == CHR_CR ) {
      //    RetVal.Append(CHR_BACKSLASH).Append('r');
      //    i++;
      //    continue;
      //  }

      //  if ( CurrentChar == CHR_TAB ) {
      //    RetVal.Append(CHR_BACKSLASH).Append('t');
      //    i++;
      //    continue;
      //  }

      //  if ( CurrentChar == CHR_FORMFEED ) {
      //    RetVal.Append(CHR_BACKSLASH).Append('f');
      //    i++;
      //    continue;
      //  }

      //  if ( CurrentChar == CHR_BACKSPACE ) {
      //    RetVal.Append(CHR_BACKSLASH).Append('b');
      //    i++;
      //    continue;
      //  }

      //  if ( CurrentChar == CHR_SLASH ) {
      //    RetVal.Append(CHR_BACKSLASH).Append(CHR_SLASH);
      //    i++;
      //    continue;
      //  }
      //  #endregion --- Control chars --------------------------------------------

      //  if ( CurrentChar == CHR_DOUBLE_QUOTE ) {
      //    InQuoteLevel--;
      //    RetVal.Append(CHR_BACKSLASH).Append(CHR_DOUBLE_QUOTE);
      //    Trace.WriteLine("Adding double quote inside quote");
      //    i++;
      //    continue;
      //  }

      //  //if ( CurrentChar == CHR_DOUBLE_QUOTE && InQuoteLevel ) {
      //  //  InQuoteLevel--;
      //  //  Trace.WriteLine("InQuote=false");
      //  //  RetVal.Append(CHR_DOUBLE_QUOTE);
      //  //  i++;
      //  //  continue;
      //  //}

      //  RetVal = RetVal.Append(CurrentChar);
      //  i++;

      //  Trace.WriteLine(RetVal.ToString());

      //} while ( i < LengthOfSource );

      //return RetVal.ToString();
    }

    public static string JsonDecode(this string source) {

      if ( source == null ) {
        return null;
      }

      if ( source == "" ) {
        return "";
      }

      string ProcessedSource = source.Replace("\\\\", "\\")
                                     .Replace("\\/", "/")
                                     .Replace("\\t", "\t")
                                     .Replace("\\n", "\n")
                                     .Replace("\\r", "\r")
                                     .Replace("\\f", "\f")
                                     .Replace("\\b", "\b")
                                     .Replace("\\\"", "\"")
                                     ;
      return ProcessedSource;

    }
  }
}