using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using BLTools.Diagnostic.Logging;

namespace BLTools {
  public static class BLConverter {

    /// <summary>
    /// Set to true to obtain additional debug info
    /// </summary>
    public static bool TraceError = false;

    public static T BLConvert<T> (object source, T defaultValue) {
      return BLConvert(source, CultureInfo.CurrentCulture, defaultValue);
    }

    /// <summary>
    /// Convert a value from one type to another (possibly through an evaluation of the value : e.g. "0", "True", "T" all becomes True)
    /// </summary>
    /// <typeparam name="T">Requested output type</typeparam>
    /// <param name="source">Data source</param>
    /// <param name="culture">Culture used to performed certain conversions</param>
    /// <param name="defaultValue">What to return when unable to convert</param>
    /// <returns>The source value converted to a new type</returns>
    public static T BLConvert<T>(object source, CultureInfo culture, T defaultValue) {
      try {

        if (source.GetType() == typeof(T)) {
          return (T)source;
        }

        if (typeof(T).IsEnum) {
          string TestSource = source as string;
          return (T)Enum.Parse(typeof(T), TestSource);
        }

        if (typeof(T).Name == nameof(Version)) {
          string TestSource = source as string;
          Version RetVal = Version.Parse(TestSource);
          return (T)Convert.ChangeType(RetVal, typeof(Version));
        }

        if (culture == null) {
          culture = CultureInfo.CurrentCulture;
        }

        switch (typeof(T).Name) {
          case "Double":
          case "Single":
            if (source is string TestSource) {
              char DecimalSeparator = culture.NumberFormat.NumberDecimalSeparator[0];
              if (TestSource.Count(x => !x.IsNumeric() && x != DecimalSeparator) > 0) {
                _LogError($"Bad format for conversion : {TestSource} : unknown non numeric characters");
                return defaultValue;
              }
              if (TestSource.Count(x => !x.IsNumeric()) > 1) {
                _LogError($"Bad format for conversion : {TestSource} : too many non numeric characters");
                return defaultValue;
              }
              if (TestSource.Count(x => x == DecimalSeparator) > 1) {
                _LogError($"Bad format for conversion : {(string)source} : too many decimal separators ({DecimalSeparator})");
                return defaultValue;
              }
            }
            return (T)Convert.ChangeType(source, typeof(T), culture.NumberFormat);

          case "DateTime":
            return (T)Convert.ChangeType(source, typeof(T), culture.DateTimeFormat);

          case "String[]":
            if (!(source is string)) {
              _LogError(string.Format("Bad format for conversion to string[] : {0} : source is not a string", source.GetType().Name));
              return defaultValue;
            }
            return (T)Convert.ChangeType(((string)source).Split(SplitArgs.Separator), typeof(T));

          case "Int32[]": {
              if (!(source is string)) {
                _LogError($"Bad format for conversion to int32[] : {source.GetType().Name} : source is not a string");
                return defaultValue;
              }
              string[] aTemp = ((string)source).Split(SplitArgs.Separator);
              List<int> RetVal = new List<int>();
              foreach (string Item in aTemp) {
                RetVal.Add(Int32.Parse(Item));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Int64[]": {
              if (!(source is string)) {
                _LogError($"Bad format for conversion to int64[] : {source.GetType().Name} : source is not a string");
                return defaultValue;
              }
              string[] aTemp = ((string)source).Split(SplitArgs.Separator);
              List<long> RetVal = new List<long>();
              foreach (string Item in aTemp) {
                RetVal.Add(long.Parse(Item));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Double[]": {
              if (!(source is string)) {
                _LogError($"Bad format for conversion to double[] : {source.GetType().Name} : source is not a string");
                return defaultValue;
              }
              string[] aTemp = ((string)source).Split(SplitArgs.Separator);
              List<double> RetVal = new List<double>();
              foreach (string Item in aTemp) {
                RetVal.Add(double.Parse(Item, culture.NumberFormat));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Single[]": {
              if (!(source is string)) {
                _LogError($"Bad format for conversion to float[] : {source.GetType().Name} : source is not a string");
                return defaultValue;
              }
              string[] aTemp = ((string)source).Split(SplitArgs.Separator);
              List<float> RetVal = new List<float>();
              foreach (string Item in aTemp) {
                RetVal.Add(float.Parse(Item, culture.NumberFormat));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Boolean":
            if (source is string StringSource) {
              return (T)Convert.ChangeType(StringSource.ToBool(), typeof(T));
            }
            switch (source.GetType().Name) {
              case "Int16":
              case "Int32":
              case "Int64":
              case "SByte":
                Int64 SignedSourceValue = (Int64)source;
                return (T)Convert.ChangeType(SignedSourceValue == 1 ? true : false, typeof(T));
              case "UInt16":
              case "UInt32":
              case "UInt64":
              case "Byte":
                UInt64 UnsignedSourceValue = (UInt64)source;
                return (T)Convert.ChangeType(UnsignedSourceValue == 1 ? true : false, typeof(T));
            }
            _LogError($"Error during conversion of \"{source}\" to {typeof(T).Name} : {typeof(T).Name} is an unhandled source type");
            return defaultValue;

          case "Guid":
            if (source is string GuidStringSource) {
              if (GuidStringSource.Trim() == "") {
                return (T)Convert.ChangeType(new Guid(), typeof(T));
              }
              return (T)Convert.ChangeType(new Guid(GuidStringSource), typeof(T));
            }
            if (source is Guid GuidSource) {
              return (T)Convert.ChangeType(GuidSource, typeof(T));
            }
            return defaultValue;

          default:
            return (T)Convert.ChangeType(source, typeof(T));

        }
      } catch (Exception ex) {
        _LogError($"Error during conversion of \"{source}\" to {typeof(T).Name} : {ex.Message}");
        return defaultValue;
      }
    }

    public static ILogger Logger { get; set; } = ALogger.DEFAULT_LOGGER;

    private static void _LogError(string message) {
      if (TraceError) {
        Logger.LogError(message);
      }
    }
  }
}
