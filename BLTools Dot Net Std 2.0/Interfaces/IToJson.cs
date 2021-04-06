using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// Convert an item to/from json
  /// </summary>
  public interface IToJson {

    /// <summary>
    /// Transform the item in json
    /// </summary>
    /// <returns>The json representation of the item</returns>
    string ToJson();

    /// <summary>
    /// Fill the item from a json source
    /// </summary>
    /// <param name="source"></param>
    void FromJson(string source);
  }
}
