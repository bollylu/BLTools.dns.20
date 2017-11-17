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

    internal const char CHR_SLASH = '/';
    internal const char CHR_BACKSLASH = '\\';
    internal const char CHR_DOUBLE_QUOTE = '\"';

    public static string JsonEncode(this string source) {

      if ( source == null ) {
        return null;
      }

      if ( source == "" ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(source).Replace("\\", "\\\\")
                                                      .Replace("/", "\\/")
                                                      .Replace("\t", "\\t")
                                                      .Replace("\n", "\\n")
                                                      .Replace("\r", "\\r")
                                                      .Replace("\f", "\\f")
                                                      .Replace("\b", "\\b")
                                                      .Replace("\\\"", "\\\\\"");
      return RetVal.ToString();

      
    }

    public static string JsonDecode(this string source) {

      if ( source == null ) {
        return null;
      }

      if ( source == "" ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(source).Replace("\\\\", "\\")
                                                      .Replace("\\/", "/")
                                                      .Replace("\\t", "\t")
                                                      .Replace("\\n", "\n")
                                                      .Replace("\\r", "\r")
                                                      .Replace("\\f", "\f")
                                                      .Replace("\\b", "\b")
                                                      .Replace("\\\"", "\"");
      return RetVal.ToString();

    }
  }
}