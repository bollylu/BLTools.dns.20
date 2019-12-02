using System;
using System.Collections.Generic;
using System.Linq;

namespace BLTools {
  public static partial class IEnumerableExtension {

    #region --- IEnumerable<T> as pattern within IEnumerable<T> --------------------------------------------
    public static bool StartsWith<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null && pattern == null) {
        return true;
      }

      if (source == null || pattern == null) {
        return false;
      }

      if (source.IsEmpty() || pattern.IsEmpty()) {
        return false;
      }

      if (pattern.Count() > source.Count()) {
        return false;
      }
      #endregion === Validate parameters ===

      for (int i = 0; i < pattern.Count(); i++) {
        if (!source.ElementAt(i).Equals(pattern.ElementAt(i))) {
          return false;
        }
      }

      return true;
    }

    public static bool EndsWith<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null && pattern == null) {
        return true;
      }

      if (source == null || pattern == null) {
        return false;
      }

      if (source.IsEmpty() || pattern.IsEmpty()) {
        return false;
      }

      if (pattern.Count() > source.Count()) {
        return false;
      }
      #endregion === Validate parameters ===

      IEnumerable<T> ValuesToCompare = source.TakeLast(pattern.Count());

      for (int i = 0; i < ValuesToCompare.Count(); i++) {
        if (!pattern.ElementAt(i).Equals(ValuesToCompare.ElementAt(i))) {
          return false;
        }
      }

      return true;
    }


    public static IEnumerable<T> After<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null || pattern == null) {
        yield break;
      }

      if (source.IsEmpty() || pattern.IsEmpty()) {
        yield break;
      }

      if (pattern.Count() > source.Count()) {
        yield break;
      }
      #endregion === Validate parameters ===

      bool Found = false;
      IEnumerable<T> TempSource = source;
      int i = 1;
      while (!Found && i < (source.Count() - pattern.Count())) {
        if (TempSource.StartsWith(pattern)) {
          Found = true;
        } else {
          TempSource = source.Skip(i++);
        }
      }

      if (Found) {
        foreach (T Item in TempSource.Skip(pattern.Count())) {
          yield return Item;
        }
      }

    }

    public static IEnumerable<T> AfterLast<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (pattern == null) {
        yield break;
      }
      if (source.IsEmpty() || pattern.IsEmpty()) {
        yield break;
      }
      if (pattern.Count() > source.Count()) {
        yield break;
      }
      #endregion === Validate parameters ===

      bool Found = false;

      int i = pattern.Count();
      IEnumerable<T> TempSource = source.TakeLast(i);
      while (!Found && i <= source.Count()) {
        if (TempSource.StartsWith(pattern)) {
          Found = true;
        } else {
          TempSource = source.TakeLast(++i);
        }
      }

      if (Found) {
        foreach (T Item in TempSource.Skip(pattern.Count())) {
          yield return Item;
        }
      }

    }


    public static IEnumerable<T> Before<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (pattern == null) {
        yield break;
      }
      if (source.IsEmpty() || pattern.IsEmpty()) {
        yield break;
      }
      if (pattern.Count() > source.Count()) {
        yield break;
      }
      #endregion === Validate parameters ===

      bool Found = false;
      IEnumerable<T> TempSource = source;
      int i = 0;
      while (!Found && i <= (source.Count() - pattern.Count())) {
        if (TempSource.StartsWith(pattern)) {
          Found = true;
        } else {
          TempSource = source.Skip(++i);
        }
      }

      if (Found) {
        foreach (T Item in source.Take(i)) {
          yield return Item;
        }
      }

    }

    public static IEnumerable<T> BeforeLast<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (pattern == null) {
        yield break;
      }
      if (source.IsEmpty() || pattern.IsEmpty()) {
        yield break;
      }
      if (pattern.Count() > source.Count()) {
        yield break;
      }
      #endregion === Validate parameters ===

      bool Found = false;
      int i = pattern.Count();
      IEnumerable<T> TempSource = source.TakeLast(i);
      while (!Found && i <= source.Count()) {
        if (TempSource.StartsWith(pattern)) {
          Found = true;
        } else {
          TempSource = source.TakeLast(++i);
        }
      }

      if (Found) {
        foreach (T Item in source.Take(source.Count() - i)) {
          yield return Item;
        }
      }

    }

    public static bool Contains<T>(this IEnumerable<T> source, IEnumerable<T> pattern) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null && pattern == null) {
        return true;
      }

      if (source == null || source.IsEmpty() || pattern == null || pattern.IsEmpty()) {
        return false;
      }

      if (pattern.Count() > source.Count()) {
        return false;
      }
      #endregion === Validate parameters ===

      bool Found = false;
      IEnumerable<T> TempSource = source;
      int i = 1;
      while (!Found && i < (source.Count() - pattern.Count())) {
        if (TempSource.StartsWith(pattern)) {
          Found = true;
        } else {
          TempSource = source.Skip(i++);
        }
      }

      return Found;
    }
    #endregion --- IEnumerable<T> as pattern within IEnumerable<T> -----------------------------------------

    #region --- Item T within IEnumerable<T> --------------------------------------------
    public static IEnumerable<T> After<T>(this IEnumerable<T> source, T item) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null || item == null) {
        yield break;
      }

      if (source.IsEmpty()) {
        yield break;
      }
      #endregion === Validate parameters ===

      int Pos = -1;
      int i = 0;
      while (Pos == -1 && i < source.Count()) {
        if (source.ElementAt(i).Equals(item)) {
          Pos = i;
        } else {
          i++;
        }
      }

      if (Pos != -1) {
        foreach (T SourceItem in source.Skip(Pos + 1)) {
          yield return SourceItem;
        }
      }
    }

    public static IEnumerable<T> AfterLast<T>(this IEnumerable<T> source, T item) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null || item == null) {
        yield break;
      }

      if (source.IsEmpty()) {
        yield break;
      }
      #endregion === Validate parameters ===

      int Pos = -1;
      int i = source.Count() - 1;
      while (Pos == -1 && i >= 0) {
        if (source.ElementAt(i).Equals(item)) {
          Pos = i;
        } else {
          i--;
        }
      }

      if (Pos != -1) {
        foreach (T SourceItem in source.Skip(Pos + 1)) {
          yield return SourceItem;
        }
      }

    }

    public static IEnumerable<T> Before<T>(this IEnumerable<T> source, T item) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null || item == null) {
        yield break;
      }

      if (source.IsEmpty()) {
        yield break;
      }
      #endregion === Validate parameters ===

      int Pos = -1;
      int i = 0;
      while (Pos == -1 && i < source.Count()) {
        if (source.ElementAt(i).Equals(item)) {
          Pos = i;
        } else {
          i++;
        }
      }

      if (Pos != -1) {
        foreach (T SourceItem in source.Take(Pos)) {
          yield return SourceItem;
        }
      }

    }

    public static IEnumerable<T> BeforeLast<T>(this IEnumerable<T> source, T item) where T : IEquatable<T> {
      #region === Validate parameters ===
      if (source == null || item == null) {
        yield break;
      }

      if (source.IsEmpty()) {
        yield break;
      }
      #endregion === Validate parameters ===

      int Pos = -1;
      int i = source.Count() - 1;
      while (Pos == -1 && i >= 0) {
        if (source.ElementAt(i).Equals(item)) {
          Pos = i;
        } else {
          i--;
        }
      }

      if (Pos != -1) {
        foreach (T SourceItem in source.Take(Pos)) {
          yield return SourceItem;
        }
      }

    }
    #endregion --- Item T within IEnumerable<T> -----------------------------------------

    /// <summary>
    /// Take the count of items from IEnumerable<T>, starting to count from the end
    /// </summary>
    /// <typeparam name="T">The type of item</typeparam>
    /// <param name="source">The IEnumerable being extended</param>
    /// <param name="count">The maximum count of items to return</param>
    /// <returns>The last items of the IEnumerable, up to the maximum count</returns>
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count) {

      #region === Validate parameters ===
      if (source == null || source.IsEmpty()) {
        yield break;
      }

      if (count <= 0) {
        yield break;
      }

      int ValidatedCount = Math.Min(count, source.Count());
      #endregion === Validate parameters ===

      foreach (T sourceItem in source.Skip(source.Count() - ValidatedCount)) {
        yield return sourceItem;
      }

    }

    /// <summary>
    /// Indicate if an IEnumerable is empty
    /// </summary>
    /// <typeparam name="T">The type of item</typeparam>
    /// <param name="source">The IEnumerable being extended</param>
    /// <returns>True if the IEnumerable is empty, False otherwise</returns>
    public static bool IsEmpty<T>(this IEnumerable<T> source) {
      #region === Validate parameters ===
      if (source == null) {
        return true;
      }
      #endregion === Validate parameters ===
      return !source.Any();
    }

  }
}
