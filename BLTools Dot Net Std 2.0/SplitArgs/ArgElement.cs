using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// Single element of arguments : Id(position), Name, Value
  /// </summary>
  public class ArgElement : IEquatable<ArgElement> {
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
    public bool Equals(ArgElement other) {
      if (other is null) {
        return false;
      }
      return (Name == other.Name && Value == other.Value);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) {
      ArgElement other = obj as ArgElement;

      if (obj == null) {
        return false;
      }

      return (Name == other.Name && Value == other.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode() {
      return Id.GetHashCode() | Name.GetHashCode() | Value.GetHashCode();
    }

    /// <summary>
    /// Test if the value exists
    /// </summary>
    /// <returns>true if the value is not null, false otherwise</returns>
    public bool HasValue() {
      return Value != null;
    }
  }
}
