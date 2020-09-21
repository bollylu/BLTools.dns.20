﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// Extension methods for arrays
  /// </summary>
  public static class ByteArrayExtension {

    /// <summary>
    /// Convert an array of byte into a string of hex values, each separated by a space (ex. "A6 34 F2")
    /// </summary>
    /// <param name="rawData">The byte array</param>
    /// <param name="separator">An optional separator</param>
    /// <returns>The string of hex values</returns>
    public static string ToHexString(this IEnumerable<byte> rawData, string separator = " ") {

      if ( rawData.IsEmpty() ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(rawData.Count() * 3);
      foreach ( byte ByteItem in rawData ) {
        RetVal.Append($"{ByteItem:X2}{separator}");
      }
      RetVal.Truncate(separator.Length);
      return RetVal.ToString();
    }

    /// <summary>
    /// Convert an array of bytes to a string of chars
    /// </summary>
    /// <param name="rawData">The array to convert</param>
    /// <returns></returns>
    public static string ToCharString(this byte[] rawData) {
      if ( rawData.Length == 0 ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(rawData.Length);
      foreach ( byte ByteItem in rawData ) {
        RetVal.Append((char)ByteItem);
      }
      return RetVal.ToString();
    }
  }
}