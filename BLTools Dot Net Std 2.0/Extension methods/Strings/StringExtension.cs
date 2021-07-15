using System;
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
  public static partial class StringExtension {

    /// <summary>
    /// Gets the left portion of a string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="length">The number of characters to get</param>
    /// <returns>The selected portion of the string. If Length > Length of the string, returns the string.</returns>
    public static string Left(this string sourceString, int length) {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (length < 0) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString.Length >= length) {
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
      if (sourceString == null) {
        return null;
      }
      if (length < 0) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString.Length >= length) {
        return sourceString.Substring(sourceString.Length - length);
      }

      return sourceString;
    }

    /// <summary>
    /// Gets the portion of the string after a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string after the delimiter</returns>
    public static string After(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (string.IsNullOrEmpty(delimiter)) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString == delimiter) {
        return "";
      }
      int Index = sourceString.IndexOf(delimiter, 0, stringComparison);
      if (Index == -1) {
        return "";
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
      if (sourceString == null) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.IndexOf(delimiter);
      if (Index == -1) {
        return "";
      }

      return sourceString.Substring(Index + 1);
    }

    /// <summary>
    /// Gets the portion of the string after the last occurence of a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string after the last occurence of a delimiter</returns>
    public static string AfterLast(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (string.IsNullOrEmpty(delimiter)) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString == delimiter) {
        return "";
      }
      int Index = sourceString.LastIndexOf(delimiter, sourceString.Length - 1, stringComparison);
      if (Index == -1) {
        return "";
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
      if (sourceString == null) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.LastIndexOf(delimiter);
      if (Index == -1) {
        return "";
      }

      return sourceString.Substring(Index + 1);
    }

    /// <summary>
    /// Gets the portion of the string before a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string before the delimiter</returns>
    public static string Before(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (string.IsNullOrEmpty(delimiter)) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString == delimiter) {
        return "";
      }
      int Index = sourceString.IndexOf(delimiter, stringComparison);
      //if ( Index == -1 ) {
      //  return sourceString;
      //}
      if (Index < 1) {
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
      if (sourceString == null) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.IndexOf(delimiter);
      //if ( Index == -1 ) {
      //  return sourceString;
      //}
      if (Index < 1) {
        return "";
      }

      return sourceString.Left(Index);
    }

    /// <summary>
    /// Gets the portion of the string before the last occurence of a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string before the last occurence of the delimiter</returns>
    public static string BeforeLast(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (string.IsNullOrEmpty(delimiter)) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString == delimiter) {
        return "";
      }
      int Index = sourceString.LastIndexOf(delimiter, stringComparison);
      //if ( Index == -1 ) {
      //  return sourceString;
      //}
      if (Index < 1) {
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
      if (sourceString == null) {
        return null;
      }
      #endregion Validate parameters

      int Index = sourceString.LastIndexOf(delimiter);
      //if ( Index == -1 ) {
      //  return sourceString;
      //}
      if (Index < 1) {
        return "";
      }

      return sourceString.Left(Index);
    }

    /// <summary>
    /// Get the whole string but the part to remove
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="dataToRemove">The string to remove</param>
    /// <returns>The cleaned string</returns>
    public static string Except(this string sourceString, string dataToRemove) {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (string.IsNullOrEmpty(dataToRemove)) {
        return sourceString;
      }
      #endregion Validate parameters

      int Index = sourceString.IndexOf(dataToRemove);
      if (Index == 0) {
        return sourceString;
      }

      return sourceString.Replace(dataToRemove, "");
    }

    /// <summary>
    /// Gets the portion of the string after a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="firstDelimiter">The first string to search for</param>
    /// <param name="secondDelimiter">The second string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string between the delimiters</returns>
    public static string Between(this string sourceString, string firstDelimiter = "[", string secondDelimiter = "]", StringComparison stringComparison = StringComparison.CurrentCulture) {
      return sourceString.After(firstDelimiter, stringComparison).Before(secondDelimiter, stringComparison);
    }

    /// <summary>
    /// Gets the portion of the string between two given chars
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="firstDelimiter">The first char to search for</param>
    /// <param name="secondDelimiter">The second char to search for</param>
    /// <returns>The selected portion of the string between the delimiters</returns>
    public static string Between(this string sourceString, char firstDelimiter = '[', char secondDelimiter = ']') {
      return sourceString.After(firstDelimiter).Before(secondDelimiter);
    }

    /// <summary>
    /// Gets the strings between two given strings
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="firstDelimiter">The first string to search for</param>
    /// <param name="secondDelimiter">The second string to search for</param>
    /// <returns>A list of items found between both delimiters</returns>
    public static IEnumerable<string> ItemsBetween(this string sourceString, string firstDelimiter = "[", string secondDelimiter = "]", StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (string.IsNullOrEmpty(sourceString)) {
        yield break;
      }
      if (firstDelimiter == "") {
        yield break;
      }
      if (secondDelimiter == "") {
        yield break;
      }
      #endregion Validate parameters

      string ProcessedString = sourceString;

      while (ProcessedString != "" && ProcessedString.IndexOf(firstDelimiter, stringComparison) != -1 && ProcessedString.IndexOf(secondDelimiter, stringComparison) != -1) {
        yield return ProcessedString.After(firstDelimiter, stringComparison).Before(secondDelimiter, stringComparison);
        ProcessedString = ProcessedString.After(secondDelimiter, stringComparison);
      }

      yield break;

    }

    /// <summary>
    /// Gets the strings between two given chars
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="firstDelimiter">The first char to search for</param>
    /// <param name="secondDelimiter">The second char to search for</param>
    /// <returns>A list of items found between both delimiters</returns>
    public static IEnumerable<string> ItemsBetween(this string sourceString, char firstDelimiter = '[', char secondDelimiter = ']') {
      #region Validate parameters
      if (string.IsNullOrEmpty(sourceString)) {
        yield break;
      }
      #endregion Validate parameters

      string ProcessedString = sourceString;

      while (ProcessedString != "" && ProcessedString.IndexOf(firstDelimiter) != -1 && ProcessedString.IndexOf(secondDelimiter) != -1) {
        yield return ProcessedString.After(firstDelimiter).Before(secondDelimiter);
        ProcessedString = ProcessedString.After(secondDelimiter);
      }

      yield break;

    }


    public static IEnumerable<string> GetItems(this string sourceString, string delimiter = ";", StringSplitOptions stringSplitOptions = StringSplitOptions.None) {
      #region === Validate parameters ===
      if (string.IsNullOrEmpty(sourceString)) {
        yield break;
      }
      if (delimiter == "") {
        yield return sourceString;
      }
      #endregion === Validate parameters ===
      foreach (string SplitItem in sourceString.Split(new string[] { delimiter }, stringSplitOptions)) {
        yield return SplitItem;
      }
    }

    public static IEnumerable<string> GetXmlTags(this string sourceString) {
      return sourceString.ItemsBetween('<', '>');
    }

    /// <summary>
    /// Capitalize the first letter of each word and uncapitalize other chars
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <param name="delimiter"></param>
    /// <returns>The proper string</returns>
    public static string Proper(this string sourceValue, char delimiter = ' ') {
      if (string.IsNullOrWhiteSpace(sourceValue)) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder();

      string[] Words = sourceValue.Split(delimiter);
      foreach (string WordItem in Words) {
        RetVal.Append($"{WordItem.Left(1).ToUpper()}{WordItem.Substring(1).ToLower()}{delimiter}");
      }
      RetVal.Truncate(1);

      return RetVal.ToString();
    }

    /// <summary>
    /// Removes external quotes from a string (ex. "\"MyString\"" => "MyString")
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>The string without inner quotes</returns>
    public static string RemoveExternalQuotes(this string sourceValue) {
      if (string.IsNullOrWhiteSpace(sourceValue)) {
        return "";
      }

      if (!sourceValue.Contains('"')) {
        return sourceValue;
      }

      StringBuilder RetVal = new StringBuilder(sourceValue);

      if (sourceValue.StartsWith("\"")) {
        RetVal.Remove(0, 1);
      }

      if (sourceValue.EndsWith("\"")) {
        RetVal.Truncate(1);
      }

      return RetVal.ToString();
    }

    /// <summary>
    /// Get a new string surrounded by double quotes
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>a new string surrounded by double quotes, or null if the source is null</returns>
    public static string WithQuotes(this string sourceValue) {
      if (sourceValue is null) {
        return null;
      }
      return $"\"{sourceValue}\"";
    }

    /// <summary>
    /// Get a new string with control chars replaced by escape sequence
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>a new string with control chars replaced by escape sequence</returns>
    public static string ReplaceControlChars(this string sourceValue) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(sourceValue)) {
        return "";
      }
      #endregion === Validate parameters ===

      StringBuilder RetVal = new StringBuilder(sourceValue.Length);

      int i = 0;
      int SourceLength = sourceValue.Length;
      bool InQuotes = false;
      bool NextCharIsControlChar = false;

      while (i < SourceLength) {

        char CurrentChar = sourceValue[i];

        if (!InQuotes && !NextCharIsControlChar && CurrentChar == '\\') {
          NextCharIsControlChar = true;
          i++;
          continue;
        }

        if (!InQuotes && NextCharIsControlChar && "\"\\\t\b\r\n\f".Contains(CurrentChar)) {
          NextCharIsControlChar = false;
          RetVal.Append(CurrentChar);
          i++;
          continue;
        }

        if (CurrentChar == '"') {
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

    /// <summary>
    /// Aligns the source string to the right of a string of width length, filling any remaining places with the filler
    /// </summary>
    /// <param name="source">The source string to align</param>
    /// <param name="width">The width of the result string</param>
    /// <param name="filler">The filler for the missing characters</param>
    /// <returns></returns>
    public static string AlignedRight(this string source, int width, char filler = ' ') {
      #region === Validate parameters ===
      if (width <= 0) {
        width = source.Length;
      }
      #endregion === Validate parameters ===
      return source.PadLeft(width, filler).Left(width);
    }

    /// <summary>
    /// Aligns the source string to the left of a string of width length, filling any remaining places with the filler
    /// </summary>
    /// <param name="source">The source string to align</param>
    /// <param name="width">The width of the result string</param>
    /// <param name="filler">The filler for the missing characters</param>
    /// <returns></returns>
    public static string AlignedLeft(this string source, int width, char filler = ' ') {
      #region === Validate parameters ===
      if (width <= 0) {
        width = source.Length;
      }
      #endregion === Validate parameters ===
      return source.PadRight(width, filler).Left(width);
    }

    /// <summary>
    /// Centers the source string in a string of width length, filling any remaining places with the filler
    /// </summary>
    /// <param name="source">The source string to align</param>
    /// <param name="width">The width of the result string</param>
    /// <param name="filler">The filler for the missing characters</param>
    /// <returns>The new string, limited to <paramref name="width"/> characters</returns>
    public static string AlignedCenter(this string source, int width, char filler = ' ') {
      #region === Validate parameters ===
      if (width <= 0) {
        width = source.Length;
      }
      #endregion === Validate parameters ===
      string LeftPart = new string(filler, width / 2 - source.Length / 2);
      return $"{LeftPart}{source}".PadRight(width, filler).Left(width);
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
    /// <summary>
    /// How to do a match when looking for a string in a list
    /// </summary>
    public enum EStringMatch {
      /// <summary>
      /// The comparison take all characters into account
      /// </summary>
      ExactMatch,
      /// <summary>
      /// The comparison can be partial (e.g. "filename.txt.sample" => true for ".txt.", true for "name", false for "nice")
      /// </summary>
      PartialMatch,
      /// <summary>
      /// The searched string must be at the start of one string in the list
      /// </summary>
      StartsWith,
      /// <summary>
      /// The searched string must be at the end of one string in the list
      /// </summary>
      EndsWith
    }

    /// <summary>
    /// Indicates if a string is in a list of string, using CultureInvariantIgnoreCase
    /// </summary>
    /// <param name="source">The string to look for</param>
    /// <param name="listOfMatches">The list of strings to compare with</param>
    /// <param name="match">How the match if done (default to an exact match)</param>
    /// <returns>true if the string is in the list, false otherwise. If any of the source or the list is null/empty, false</returns>
    public static bool IsIn(this string source, IEnumerable<string> listOfMatches, EStringMatch match = EStringMatch.ExactMatch) {
      return IsIn(source, listOfMatches, StringComparison.InvariantCultureIgnoreCase, match);
    }


    /// <summary>
    /// Indicates if a string is in a list of string
    /// </summary>
    /// <param name="source">The string to look for</param>
    /// <param name="listOfMatches">The list of strings to compare with</param>
    /// <param name="comparison">How the comparison is done</param>
    /// <param name="match">How the match if done (default to an exact match)</param>
    /// <returns>true if the string is in the list, false otherwise. If any of the source or the list is null/empty, false</returns>
    public static bool IsIn(this string source, IEnumerable<string> listOfMatches, StringComparison comparison, EStringMatch match = EStringMatch.ExactMatch) {
    #region === Validate parameters ===
      if (source is null) {
        return false;
      }
      if (listOfMatches is null || listOfMatches.IsEmpty()) {
        return false;
      }
    #endregion === Validate parameters ===

      return match switch {
        EStringMatch.PartialMatch => listOfMatches.Any(x => x.Contains(source, comparison)),
        EStringMatch.StartsWith => listOfMatches.Any(x => x.StartsWith(source, comparison)),
        EStringMatch.EndsWith => listOfMatches.Any(x => x.EndsWith(source, comparison)),
        EStringMatch.ExactMatch => listOfMatches.Any(x => x.Equals(source, comparison)),
        _ => throw new NotImplementedException()
      };

    }


    /// <summary>
    /// Indicates if a string is NOT in a list of string, using CultureInvariantIgnoreCase
    /// </summary>
    /// <param name="source">The string to look for</param>
    /// <param name="listOfMatches">The list of strings to compare with</param>
    /// <param name="match">How the match if done (default to an exact match)</param>
    /// <returns>true if the string is not in the list, false otherwise. If any of the source or the list is null/empty, false</returns>
    public static bool IsNotIn(this string source, IEnumerable<string> listOfMatches, EStringMatch match = EStringMatch.ExactMatch) {
      return IsNotIn(source, listOfMatches, StringComparison.InvariantCultureIgnoreCase, match);
    }

    /// <summary>
    /// Indicates if a string is NOT in a list of string
    /// </summary>
    /// <param name="source">The string to look for</param>
    /// <param name="listOfMatches">The list of strings to compare with</param>
    /// <param name="comparison">How the comparison is done</param>
    /// <param name="match">How the match if done (default to an exact match)</param>
    /// <returns>true if the string is not in the list, false otherwise. If any of the source or the list is null/empty, false</returns>
    public static bool IsNotIn(this string source, IEnumerable<string> listOfMatches, StringComparison comparison, EStringMatch match = EStringMatch.ExactMatch) {
    #region === Validate parameters ===
      if (source is null) {
        return true;
      }
      if (listOfMatches is null || listOfMatches.IsEmpty()) {
        return true;
      } 
    #endregion === Validate parameters ===

      return match switch {
        EStringMatch.PartialMatch => listOfMatches.All(x => !x.Contains(source, comparison)),
        EStringMatch.StartsWith => listOfMatches.All(x => !x.StartsWith(source, comparison)),
        EStringMatch.EndsWith => listOfMatches.All(x => !x.EndsWith(source, comparison)),
        EStringMatch.ExactMatch => listOfMatches.All(x => !x.Equals(source, comparison)),
        _ => throw new NotImplementedException()
      };
    }
#endif

  }
}
