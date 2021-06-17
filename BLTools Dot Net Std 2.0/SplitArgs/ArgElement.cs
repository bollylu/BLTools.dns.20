using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// Single element of arguments : Id(position), Name, Value
  /// </summary>
  public class ArgElement : IArgElement {

    #region --- Public properties ------------------------------------------------------------------------------
    /// <summary>
    /// The position within the command line
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// The key name
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The value (stored as a string)
    /// </summary>
    public string Value { get; private set; }
    #endregion --- Public properties ---------------------------------------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Builds a key/pair value with an ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public ArgElement(int id, string name, string value) {
      Id = id;
      Name = name;
      Value = value;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <inheritdoc/>
    public bool HasValue() {
      return Value != null;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) {
      IArgElement other = obj as IArgElement;

      if (obj is null) {
        return false;
      }

      return (Name == other.Name && Value == other.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode() {
      return Id.GetHashCode() | Name.GetHashCode() | Value.GetHashCode();
    }

    /// <summary>
    /// Compare two IArgElements using StringComparison.CurrentCulture
    /// </summary>
    /// <param name="other">The other IArgElement to compare with</param>
    /// <returns>true if same, false otherwise</returns>
    public bool Equals(IArgElement other) {
      return Equals(other, StringComparison.CurrentCulture);
    }

    /// <summary>
    /// Compare two IArgElements
    /// </summary>
    /// <param name="other">The other IArgElement to compare with</param>
    /// <param name="comparison">How to compare the strings</param>
    /// <returns>true if same, false otherwise</returns>
    public bool Equals(IArgElement other, StringComparison comparison) {
      #region === Validate parameters ===
      if (other is null) {
        return false;
      }
      #endregion === Validate parameters ===
      return (Name == other.Name && Value == other.Value);
    }
  }
}
