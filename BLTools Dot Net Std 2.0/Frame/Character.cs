using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Frames {
  public static class Character {
    public static void Draw(int col, int row, char oneChar) {
      Console.SetCursorPosition(col, row);
      Console.Write(oneChar);
    }

  }
}
