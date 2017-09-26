using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data {
  public static class SqlMaker {

    public static string InConditionFactory(string dataFieldName, IEnumerable<string> items) {
      if ( string.IsNullOrWhiteSpace(dataFieldName) ) {
        throw new ArgumentException("Must specify data field name", "dataFieldName");
      }
      if ( ( items == null ) || items.Count() == 0 ) {
        throw new ArgumentException("Must specify list of possible items", "items");
      }
      if ( items.Count() > 2000 ) {
        throw new ArgumentException("Items count is too big, must stay under 2000", "items");
      }
      return $"{dataFieldName} IN ({string.Join(", ", items.Select(x => string.Format("'{0}'", x)))}";
    }

    public static string InConditionFactory(string dataFieldName, IEnumerable<int> items) {
      if ( string.IsNullOrWhiteSpace(dataFieldName) ) {
        throw new ArgumentException("Must specify data field name", "dataFieldName");
      }
      if ( ( items == null ) || items.Count() == 0 ) {
        throw new ArgumentException("Must specify list of possible items", "items");
      }
      if ( items.Count() > 2000 ) {
        throw new ArgumentException("Items count is too big, must stay under 2000", "items");
      }
      return $"{dataFieldName} IN ({string.Join(", ", items)})";
    }
  }
}
