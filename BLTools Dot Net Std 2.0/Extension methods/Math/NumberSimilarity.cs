using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace BLTools {
  public static partial class NumberExtension {

    #region --- IsSimilarByPercent-------------------------------------------
    public static bool IsSimilarByPercent(this double value1, double value2, float percentageMargin = 0) {
      double Delta = Abs(value1 - value2);
      double PercentageValue = value1 / 100d * percentageMargin;
      return (Delta <= PercentageValue);
    }

    public static bool IsSimilarByPercent(this float value1, float value2, float percentageMargin = 0) {
      float Delta = Abs(value1 - value2);
      float PercentageValue = value1 / 100f * percentageMargin;
      return (Delta <= PercentageValue);
    }

    public static bool IsSimilarByPercent(this decimal value1, decimal value2, float percentageMargin = 0) {
      decimal Delta = Abs(value1 - value2);
      decimal PercentageValue = value1 / 100m * (decimal)percentageMargin;
      return (Delta <= PercentageValue);
    }

    public static bool IsSimilarByPercent(this int value1, int value2, float percentageMargin = 0) {
      int Delta = Abs(value1 - value2);
      double PercentageValue = value1 / 100d * percentageMargin;
      return (Delta <= PercentageValue);
    }

    public static bool IsSimilarByPercent(this long value1, long value2, float percentageMargin = 0) {
      long Delta = Abs(value1 - value2);
      double PercentageValue = value1 / 100d * percentageMargin;
      return (Delta <= PercentageValue);
    }
    #endregion --- IsSimilarByPercent -----------------------------------------

    #region --- IsSimilarByValue --------------------------------------------
    public static bool IsSimilarByValue(this double value1, double value2, double maxDelta = 0) {
      double Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsSimilarByValue(this float value1, float value2, float maxDelta = 0) {
      float Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsSimilarByValue(this decimal value1, decimal value2, decimal maxDelta = 0) {
      decimal Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsSimilarByValue(this int value1, int value2, int maxDelta = 0) {
      int Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsSimilarByValue(this long value1, long value2, long maxDelta = 0) {
      long Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }
    #endregion --- IsSimilarByValue -----------------------------------------

    #region --- IsBelowByPercent-------------------------------------------
    public static bool IsBelowByPercent(this double value1, double value2, float percentageMargin = 0) {
      if (value1 >= value2) {
        return false;
      }
      double Delta = value2 - value1;
      double DeltaMaxPerPercentageMargin = value1 / 100d * percentageMargin;
      return (Delta <= DeltaMaxPerPercentageMargin);
    }

    public static bool IsBelowByPercent(this float value1, float value2, float percentageMargin = 0) {
      if (value1 >= value2) {
        return false;
      }
      float Delta = value2 - value1;
      float DeltaMaxPerPercentageMargin = value1 / 100f * percentageMargin;
      return (Delta <= DeltaMaxPerPercentageMargin);
    }

    public static bool IsBelowByPercent(this decimal value1, decimal value2, float percentageMargin = 0) {
      if (value1 >= value2) {
        return false;
      }
      decimal Delta = value2 - value1;
      decimal DeltaMaxPerPercentageMargin = value1 / 100m * (decimal)percentageMargin;
      return (Delta <= DeltaMaxPerPercentageMargin);
    }

    public static bool IsBelowByPercent(this int value1, int value2, float percentageMargin = 0) {
      if (value1 >= value2) {
        return false;
      }
      int Delta = value2 - value1;
      double DeltaMaxPerPercentageMargin = value1 / 100 * percentageMargin;
      return (Delta <= DeltaMaxPerPercentageMargin);
    }

    public static bool IsBelowByPercent(this long value1, long value2, float percentageMargin = 0) {
      if (value1 >= value2) {
        return false;
      }
      long Delta = value2 - value1;
      double DeltaMaxPerPercentageMargin = value1 / 100 * percentageMargin;
      return (Delta <= DeltaMaxPerPercentageMargin);
    }
    #endregion --- IsBelowByPercent -----------------------------------------

    #region --- IsBelowByValue --------------------------------------------
    public static bool IsBelowByValue(this double value1, double value2, double maxDelta = 0) {
      if (value1 >= value2) {
        return false;
      }
      double Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsBelowByValue(this float value1, float value2, float maxDelta = 0) {
      if (value1 >= value2) {
        return false;
      }
      float Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsBelowByValue(this decimal value1, decimal value2, decimal maxDelta = 0) {
      if (value1 >= value2) {
        return false;
      }
      decimal Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsBelowByValue(this int value1, int value2, int maxDelta = 0) {
      if (value1 >= value2) {
        return false;
      }
      int Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }

    public static bool IsBelowByValue(this long value1, long value2, long maxDelta = 0) {
      if (value1 >= value2) {
        return false;
      }
      long Delta = Abs(value1 - value2);
      return Delta <= maxDelta;
    }
    #endregion --- IsBelowByValue -----------------------------------------

  }
}
