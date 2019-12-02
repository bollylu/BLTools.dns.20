using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  public static class PropertyInfoExtensions {
    public static bool HasAttribute(this PropertyInfo Info, Type attributeType) {
      return (Attribute.GetCustomAttribute(Info, attributeType) != null);
    }

    public static PropertyInfo GetPropertyInfo(this IEnumerable<PropertyInfo> propertiesInfo, string name) {
      #region === Validate parameters ===
      if (propertiesInfo == null) {
        return default;
      }
      if (propertiesInfo.IsEmpty()) {
        return default;
      }
      if (string.IsNullOrWhiteSpace(name)) {
        return default;
      }
      #endregion === Validate parameters ===

      return propertiesInfo.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    public static T GetValue<T>(this PropertyInfo propertyInfo, object o) {
      return (T)propertyInfo.GetValue(o);
    }
  }
}
