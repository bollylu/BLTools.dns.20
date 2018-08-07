﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace BLTools {

  /// <summary>
  /// Extensions for string
  /// </summary>
  public static class StringExtension {

    #region Converters
    /// <summary>
    /// Converts a string to a bool
    /// </summary>
    /// <param name="booleanString">A string representing a bool (0,1; false,true; no,yes; n,y)</param>
    /// <returns>A bool as represented by the string (default=false)</returns>
    public static bool ToBool(this string booleanString) {
      #region Validate parameters
      if ( booleanString == null ) {
        return false;
      }
      #endregion Validate parameters
      switch ( booleanString.Trim().ToLower() ) {
        case "0":
        case "false":
        case "no":
        case "n":
          return false;
        case "1":
        case "true":
        case "yes":
        case "y":
          return true;
        default:
          return false;
      }

    }
    /// <summary>
    /// Converts a string to a bool
    /// </summary>
    /// <param name="booleanString">The string to convert</param>
    /// <param name="trueValue">The string value representing true</param>
    /// <param name="falseValue">The string value representing false</param>
    /// <param name="isCaseSensitive">Do we test the values with case sensitivity (default=false)</param>
    /// <returns>A bool as represented by the string (default=false)</returns>
    public static bool ToBool(this string booleanString, string trueValue = "true", string falseValue = "false", bool isCaseSensitive = false) {
      #region Validate parameters
      if ( string.IsNullOrWhiteSpace(booleanString) ) {
        return false;
      }
      #endregion Validate parameters

      string ValueToCompare;
      string TrueValue;
      string FalseValue;

      if ( !isCaseSensitive ) {
        ValueToCompare = booleanString.ToLowerInvariant();
        TrueValue = trueValue.ToLowerInvariant();
        FalseValue = falseValue.ToLowerInvariant();
      } else {
        ValueToCompare = booleanString;
        TrueValue = trueValue;
        FalseValue = falseValue;
      }

      if ( ValueToCompare == TrueValue ) {
        return true;
      }

      if ( ValueToCompare == FalseValue ) {
        return false;
      }

      Trace.WriteLine($"Error: value to convert doesn't match any possible value : \"{TrueValue}\", \"{FalseValue}\", \"{ValueToCompare}\"");
      return false;

    }

    /// <summary>
    /// Convert a string to an array of bytes
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <returns>The array of bytes</returns>
    public static byte[] ToByteArray(this string sourceString) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      #endregion Validate parameters
      return sourceString.Select<char, byte>(c => (byte)c).ToArray();
    }

    /// <summary>
    /// Convert a SecureString to a normal string
    /// </summary>
    /// <param name="securePassword">The source SecureString</param>
    /// <returns>The string</returns>
    public static string ConvertToUnsecureString(this SecureString securePassword) {
      if ( securePassword == null ) {
        return null;
      }

      IntPtr UnmanagedString = IntPtr.Zero;
      try {
        UnmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
        return Marshal.PtrToStringUni(UnmanagedString);
      } finally {
        Marshal.ZeroFreeGlobalAllocUnicode(UnmanagedString);
      }
    }

    /// <summary>
    /// Converts a string to a SecureString
    /// </summary>
    /// <param name="unsecureString">The source string</param>
    /// <returns>The SecureString</returns>
    public static SecureString ConvertToSecureString(this string unsecureString) {
      if ( unsecureString == null ) {
        return null;
      }
      SecureString RetVal = new SecureString();
      foreach ( char CharItem in unsecureString ) {
        RetVal.AppendChar(CharItem);
      }
      RetVal.MakeReadOnly();
      return RetVal;
    }
    #endregion Converters

    #region Tests
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlpha(this string sourceValue) {
      return Regex.IsMatch(sourceValue, @"^[A-Za-z]*$");
    }
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or numeric characters (0-9)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaNumeric(this string sourceValue) {
      return Regex.IsMatch(sourceValue, "^[A-Za-z0-9]*$");
    }
    /// <summary>
    /// Indicates if a string contains only numeric characters (0-9) or separators (-.,)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumeric(this string sourceValue) {
      return Regex.IsMatch(sourceValue, @"^[-\d][\d\.,]*$");
    }
    /// <summary>
    /// Indicates if a string contains only numeric characters (0-9) or separators (-.,)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumericOnly(this string sourceValue) {
      return Regex.IsMatch(sourceValue, @"^[-\d][\d]*$");
    }
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaOrBlank(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[A-Za-z ]*$");
    }
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or numeric characters (0-9) or blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaNumericOrBlank(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[A-Za-z\d ]*$");
    }
    /// <summary>
    /// Indicates if a string contains only numeric characters (0-9) or blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumericOrBlank(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[-\d][\d\., ]*$");
    }

    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or numeric characters (0-9) or blank or dashes
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaNumericOrBlankOrDashes(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[A-Za-z\d -]*$");
    }
    /// <summary>
    /// Indicates if a string contains only specified characters
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <param name="charList">The list of characters to test for</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsMadeOfTheseChars(this string sourceValue, params char[] charList) {
      return Regex.IsMatch(sourceValue.Trim(), string.Format(@"^[{0}]*$", string.Join("", charList)));
    }
    #endregion Tests

    /// <summary>
    /// Gets the left portion of a string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="length">The number of characters to get</param>
    /// <returns>The selected portion of the string. If Length > Length of the string, returns the string.</returns>
    public static string Left(this string sourceString, int length) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( length < 0 ) {
        return sourceString;
      }
      #endregion Validate parameters

      if ( sourceString.Length >= length ) {
        return sourceString.Substring(0, length);
      }

      return sourceString;
    }

    /// <summary>
    /// Gets the right portion of the string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="length">The number of characters to get</param>
    /// <returns>The selected portion of the string. If Length > Length of the string, returns the string.</returns>
    public static string Right(this string sourceString, int length) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( length < 0 ) {
        return sourceString;
      }
      #endregion Validate parameters

      if ( sourceString.Length >= length ) {
        return sourceString.Substring(sourceString.Length - length);
      }

      return sourceString;
    }

    /// <summary>
    /// Gets the portion of the string after a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <returns>The selected portion of the string after the delimiter</returns>
    public static string After(this string sourceString, string delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( string.IsNullOrEmpty(delimiter) ) {
        return sourceString;
      }
      #endregion Validate parameters

      if ( sourceString == delimiter ) {
        return "";
      }
      int Index = sourceString.IndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }

      return sourceString.Substring(Index + delimiter.Length);
    }

    /// <summary>
    /// Gets the portion of the string after a given char
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The char to search for</param>
    /// <returns>The selected portion of the string after the delimiter</returns>
    public static string After(this string sourceString, char delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.IndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }

      return sourceString.Substring(Index + 1);
    }

    /// <summary>
    /// Gets the portion of the string after the last occurence of a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <returns>The selected portion of the string after the last occurence of a delimiter</returns>
    public static string AfterLast(this string sourceString, string delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( string.IsNullOrEmpty(delimiter) ) {
        return sourceString;
      }
      #endregion Validate parameters

      if ( sourceString == delimiter ) {
        return "";
      }
      int Index = sourceString.LastIndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }

      return sourceString.Substring(Index + delimiter.Length);
    }

    /// <summary>
    /// Gets the portion of the string after the last occurence of a given char
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The char to search for</param>
    /// <returns>The selected portion of the string after the last occurence of a delimiter</returns>
    public static string AfterLast(this string sourceString, char delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.LastIndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }

      return sourceString.Substring(Index + 1);
    }

    /// <summary>
    /// Gets the portion of the string before a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <returns>The selected portion of the string before the delimiter</returns>
    public static string Before(this string sourceString, string delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( string.IsNullOrEmpty(delimiter) ) {
        return sourceString;
      }
      #endregion Validate parameters

      if ( sourceString == delimiter ) {
        return "";
      }
      int Index = sourceString.IndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }
      if ( Index < 1 ) {
        return "";
      }

      return sourceString.Left(Index);
    }

    /// <summary>
    /// Gets the portion of the string before a given char
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The char to search for</param>
    /// <returns>The selected portion of the string before the delimiter</returns>
    public static string Before(this string sourceString, char delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.IndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }
      if ( Index < 1 ) {
        return "";
      }

      return sourceString.Left(Index);
    }

    /// <summary>
    /// Gets the portion of the string before the last occurence of a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <returns>The selected portion of the string before the last occurence of the delimiter</returns>
    public static string BeforeLast(this string sourceString, string delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( string.IsNullOrEmpty(delimiter) ) {
        return sourceString;
      }
      #endregion Validate parameters

      if ( sourceString == delimiter ) {
        return "";
      }
      int Index = sourceString.LastIndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }
      if ( Index < 1 ) {
        return "";
      }

      return sourceString.Left(Index);
    }

    /// <summary>
    /// Gets the portion of the string before the last occurence of a given char
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The char to search for</param>
    /// <returns>The selected portion of the string before the last occurence of the delimiter</returns>
    public static string BeforeLast(this string sourceString, char delimiter) {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.LastIndexOf(delimiter);
      if ( Index == -1 ) {
        return sourceString;
      }
      if ( Index < 1 ) {
        return "";
      }

      return sourceString.Left(Index);
    }

    /// <summary>
    /// Capitalize the first letter of each word and uncapitalize other chars
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>The proper string</returns>
    public static string Proper(this string sourceValue) {
      if ( string.IsNullOrWhiteSpace(sourceValue) ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder();

      string[] Words = sourceValue.Split(' ');
      foreach ( string WordItem in Words ) {
        RetVal.Append($"{WordItem.Left(1).ToUpper()}{WordItem.Substring(1).ToLower()} ");
      }
      RetVal.Truncate(1);

      return RetVal.ToString();
    }

    /// <summary>
    /// Removes external quotes from a string (ex. "\"MyString\"" => "MyString")
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>The string without quotes</returns>
    public static string RemoveExternalQuotes(this string sourceValue) {
      if ( string.IsNullOrWhiteSpace(sourceValue) ) {
        return "";
      }

      if ( !sourceValue.Contains('"') ) {
        return sourceValue;
      }

      StringBuilder RetVal = new StringBuilder(sourceValue);

      if ( sourceValue.StartsWith("\"") ) {
        RetVal.Remove(0, 1);
      }

      if ( sourceValue.EndsWith("\"") ) {
        RetVal.Truncate(1);
      }

      return RetVal.ToString();
    }

    public static string ReplaceControlChars(this string sourceValue) {
      if ( string.IsNullOrWhiteSpace(sourceValue) ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(sourceValue.Length);

      int i = 0;
      int SourceLength = sourceValue.Length;
      bool InQuotes = false;
      bool NextCharIsControlChar = false;

      while ( i < SourceLength ) {

        char CurrentChar = sourceValue[i];

        if ( !InQuotes && !NextCharIsControlChar && CurrentChar == '\\' ) {
          NextCharIsControlChar = true;
          i++;
          continue;
        }

        if ( !InQuotes && NextCharIsControlChar && "\"\\\t\b\r\n\f".Contains(CurrentChar) ) {
          NextCharIsControlChar = false;
          RetVal.Append(CurrentChar);
          i++;
          continue;
        }

        if ( CurrentChar == '"' ) {
          RetVal.Append(CurrentChar);
          InQuotes = !InQuotes;
          i++;
          continue;
        }

        RetVal.Append(CurrentChar);
        i++;
      }

      return RetVal.ToString();
    }

    public static string Spaces(int number) {
      if ( number <= 0 ) {
        return "";
      }
      return new string(' ', number);
    }

  }
}
