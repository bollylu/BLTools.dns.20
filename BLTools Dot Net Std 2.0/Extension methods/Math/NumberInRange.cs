using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {
  public static partial class NumberExtension {

    public static bool IsInRange(this int source, int min, int max) {
      return source >= min && source <= max;
    }

    public static bool IsInRange(this long source, long min, long max) {
      return source >= min && source <= max;
    }

    public static bool IsInRange(this float source, float min, float max) {
      return source >= min && source <= max;
    }

    public static bool IsInRange(this double source, double min, double max) {
      return source >= min && source <= max;
    }

    public static bool IsOutsideRange(this int source, int min, int max) {
      return source < min || source > max;
    }

    public static bool IsOutsideRange(this long source, long min, long max) {
      return source < min || source > max;
    }

    public static bool IsOutsideRange(this float source, float min, float max) {
      return source < min || source > max;
    }

    public static bool IsOutsideRange(this double source, double min, double max) {
      return source < min || source > max;
    }
  }
}
