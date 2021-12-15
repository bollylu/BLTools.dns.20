using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public enum EJsonToken {
    UNKNOWN,
    EMPTY,
    START_OBJECT,
    END_OBJECT,
    START_ARRAY,
    END_ARRAY,
    PROPERTY_NAME,
    PROPERTY_VALUE,
    PROPERTY_SEPARATOR,
    PROPERTY_NAME_VALUE_SEPARATOR

  }
}
