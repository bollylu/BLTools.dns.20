using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BLTools {
  /// <summary>
  /// Convert item to/from xml
  /// </summary>
  public interface IToXml {
    
    /// <summary>
    /// Transform the item into Xml
    /// </summary>
    /// <returns>The Xml represention of the item</returns>
    XElement ToXml();

    /// <summary>
    /// Fill the item from an Xml source
    /// </summary>
    /// <param name="source">The data source in Xml format</param>
    void FromXml(XElement source);

  }
}
