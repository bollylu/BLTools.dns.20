using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BLTools.Frames {
  public static class Cursor {

    private static readonly Stack<Point> CursorPositions = new Stack<Point>();

    public static void SavePosition() {
      CursorPositions.Push(GetCursorPosition());
    }

    public static void RestorePosition() {
      if ( CursorPositions.Any() ) {
        SetCursorPosition(CursorPositions.Pop());
      }
    }

    public static Point GetCursorPosition() {
      return new Point { X = Console.CursorLeft, Y = Console.CursorTop };
    }

    public static void SetCursorPosition(Point savedPosition) {
      Console.SetCursorPosition(savedPosition.X, savedPosition.Y);
    }

    public static void SetCursorPosition(int col, int row) {
      Console.SetCursorPosition(col, row);
    }
  }
}
