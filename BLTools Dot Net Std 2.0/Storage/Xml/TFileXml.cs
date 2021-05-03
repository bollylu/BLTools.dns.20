using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Xml {
  public class TFileXml : AFileXml {

    /// <summary>
    /// Create a new xml storage
    /// </summary>
    public TFileXml() : base() { }

    /// <summary>
    /// Create a new xml storage with a filename
    /// </summary>
    /// <param name="filename">The filename to store data</param>
    public TFileXml(string filename) : base(filename) { }
  }
}
