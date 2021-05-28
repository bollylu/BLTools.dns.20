using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Text {
  /// <summary>
  /// Extensions to ease the use of TextBox
  /// </summary>
  public static class TextBoxExtensions {
    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated. The border is filled with IBM boxes characters
    /// </summary>
    /// <param name="source">The message</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <returns>A string containing the message in the box</returns>
    public static string BoxIBM(this string source, int margin = 0, TextBox.EStringAlignment alignment = TextBox.EStringAlignment.Left, char filler = '.') {
      return TextBox.BuildDynamicIBM(source, margin, alignment, filler);
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated.
    /// </summary>
    /// <param name="source">The message</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <param name="border">The border string (top-left/top/top-right/right/bottom-right/bottom/bottom-left/left)</param>
    /// <returns>A string containing the message in the box</returns>
    public static string Box(this string source, int margin = 0, TextBox.EStringAlignment alignment = TextBox.EStringAlignment.Left, char filler = '.', string border = "") {
      return TextBox.BuildDynamic(source, margin, alignment, filler, border);
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated. The title is in the border
    /// </summary>
    /// <param name="source">The message</param>
    /// <param name="title">The title</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <param name="border">The border string (top-left/top/top-right/right/bottom-right/bottom/bottom-left/left)</param>
    /// <returns>A string containing the message in the box</returns>
    public static string Box(this string source, string title, int margin = 0, TextBox.EStringAlignment alignment = TextBox.EStringAlignment.Left, char filler = '.', string border = "") {
      return TextBox.BuildDynamicWithTitle(source, title, margin, alignment, filler, border);

    }
  }
}
