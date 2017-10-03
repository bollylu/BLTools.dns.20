using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLTools {
  public static class DictionaryExtension {

    public static T SafeGetValue<K, T>(this IDictionary source, K key, T defaultValue) {

      if ( key == null ) {
        return defaultValue;
      }

      if ( source.Count == 0 ) {
        return defaultValue;
      }

      if ( source.Contains(key) ) {
        return BLConverter.BLConvert<T>(source[key], CultureInfo.CurrentCulture, defaultValue);
      }

      return defaultValue;
    }
  }
}
