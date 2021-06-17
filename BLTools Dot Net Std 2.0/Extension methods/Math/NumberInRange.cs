using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {
  public static partial class NumberExtension {

    /// <summary>
    /// Checks if the value is in the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if the value is comprised within the range, false otherwise</returns>
    public static bool IsInRange(this int source, int min, int max) {
      return source >= min && source <= max;
    }

    /// <summary>
    /// Checks if the value is in the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if the value is comprised within the range, false otherwise</returns>
    public static bool IsInRange(this long source, long min, long max) {
      return source >= min && source <= max;
    }

    /// <summary>
    /// Checks if the value is in the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if the value is comprised within the range, false otherwise</returns>
    public static bool IsInRange(this float source, float min, float max) {
      return source >= min && source <= max;
    }

    /// <summary>
    /// Checks if the value is in the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if the value is comprised within the range, false otherwise</returns>
    public static bool IsInRange(this double source, double min, double max) {
      return source >= min && source <= max;
    }

    /// <summary>
    /// Checks if the value is outside the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if outside of range, false otherwise</returns>
    public static bool IsOutsideRange(this int source, int min, int max) {
      return source < min || source > max;
    }

    /// <summary>
    /// Checks if the value is outside the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if outside of range, false otherwise</returns>
    public static bool IsOutsideRange(this long source, long min, long max) {
      return source < min || source > max;
    }

    /// <summary>
    /// Checks if the value is outside the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if outside of range, false otherwise</returns>
    public static bool IsOutsideRange(this float source, float min, float max) {
      return source < min || source > max;
    }

    /// <summary>
    /// Checks if the value is outside the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="min">The minimum value of the range (included)</param>
    /// <param name="max">The maximum value of the range (included)</param>
    /// <returns>true if outside of range, false otherwise</returns>
    public static bool IsOutsideRange(this double source, double min, double max) {
      return source < min || source > max;
    }

#if NET

    /// <summary>
    /// Checks if the value is outside the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="range">The range to check against (both ends included)</param>
    /// <returns>true if outside of range, false otherwise</returns>
    public static bool IsOutsideRange(this int source, Range range) {
      return source < range.Start.Value || source > range.End.Value;
    }

    /// <summary>
    /// Checks if the value is in the given range (both ends count into the range)
    /// </summary>
    /// <param name="source">The value to test</param>
    /// <param name="range">The range to check against (both ends included)</param>
    /// <returns>true if the value is comprised within the range, false otherwise</returns>
    public static bool IsInRange(this int source, Range range) {
      return source >= range.Start.Value && source <= range.End.Value;
    }

#endif
  }
}
