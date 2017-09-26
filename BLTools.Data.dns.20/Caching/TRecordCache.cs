using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data {
  /// <summary>
  /// Cache the content of a record from the IDataReader
  /// </summary>
  public class TRecordCache {

    public Dictionary<string, object> Values { get; set; }

    public TRecordCache(IDataReader dataReader) {
      Values = new Dictionary<string, object>(dataReader.FieldCount);
      for (int i = 0; i < dataReader.FieldCount; i++) {
        Values.Add(dataReader.GetName(i), dataReader.GetValue(i));
      }
    }

    public object this[string name] {
      get {
        if (name == null) {
          return null;
        }
        return Values[name];
      }
    }

  }

  
}
