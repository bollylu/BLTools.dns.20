using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {

  /// <summary>
  /// Interface for an element with ID, name, value as string
  /// </summary>
  public interface IArgElement : IEquatable<IArgElement> {

    #region --- Public properties ------------------------------------------------------------------------------
    /// <summary>
    /// The position within the command line
    /// </summary>
    int Id { get; }
    /// <summary>
    /// The key name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// The value (stored as a string)
    /// </summary>
    string Value { get; }
    #endregion --- Public properties ---------------------------------------------------------------------------
    
    /// <summary>
    /// Test if the value exists
    /// </summary>
    /// <returns>true if the value is not null, false otherwise</returns>
    bool HasValue();
  }
}
