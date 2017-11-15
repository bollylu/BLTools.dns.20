using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BLTools.Json {
  public class JsonPairCollection : List<IJsonPair>, IDisposable {

    public void Dispose() {
      foreach(IJsonPair ItemItem in this) {
        ItemItem.Dispose();
      }
      this.Clear();
    }

    public string RenderAsString() {
      StringBuilder RetVal = new StringBuilder();
      foreach(IJsonPair PairItem in this) {
        RetVal.Append(PairItem.RenderAsString());
        RetVal.Append(",");
      }
      RetVal.Truncate(1);
      return RetVal.ToString();
    }


  }
}
