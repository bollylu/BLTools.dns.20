using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Collections {

  /// <summary>
  /// Defines a circular list. When iterating the list, after the last item, the itaration continue with the first item. The behaviour is similar in the other direction.
  /// </summary>
  /// <typeparam name="T">The type of data to store</typeparam>
  public interface ICircularList<T> : IList<T> {

    /// <summary>
    /// tru to indicate that this list behave as circular. If false, this is a standard list
    /// </summary>
    bool IsCircular { get; set; }

    /// <summary>
    /// Obtain the next item in iteration
    /// </summary>
    /// <returns>The next item</returns>
    T GetNext();

    /// <summary>
    /// Obtain the previous item in iteration
    /// </summary>
    /// <returns>The previous item</returns>
    T GetPrevious();

    /// <summary>
    /// Reset the index to the first item of the list
    /// </summary>
    void ResetIndex();

  }
}
