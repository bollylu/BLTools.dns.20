using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BLTools.Frames {
  public static class Line {

    public static void DrawHLine(Point startPosition, int length) {
      for ( int i = 0; i < length; i++ ) {
        Character.Draw(startPosition.X + i, startPosition.Y, FrameElementCommon.BORDER_CHAR);
      }
    }
    public static void DrawHLine(int col, int row, int length) {
      for ( int i = 0; i < length; i++ ) {
        Character.Draw(col + i, row, FrameElementCommon.BORDER_CHAR);
      }
    }

    public static void DrawVLine(Point startPosition, int length) {
      for ( int i = 0; i < length; i++ ) {
        Character.Draw(startPosition.X, startPosition.Y + i, FrameElementCommon.BORDER_CHAR);
      }
    }

    public static void DrawVLine(int col, int row, int length) {
      for ( int i = 0; i < length; i++ ) {
        Character.Draw(col, row + i, FrameElementCommon.BORDER_CHAR);
      }
    }

  }
}
