using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace BLTools.Text {
  public static class TextBox {

    #region --- Enums --------------------------------------------
    public enum EStringAlignment {
      Left,
      Center,
      Right
    }
    public enum EHorizontalRowType {
      Single,
      Double,
      FullLight,
      FullMedium,
      FullBold,
      Solid,
      Dot,
      Underline,
      Stars,
      SingleIBM,
      SingleIBMBold,
      DoubleIBM,
      Slash,
      Backslash,
      Pipe
    }
    #endregion --- Enums --------------------------------------------

    #region --- Constants --------------------------------------------
    public const char CHAR_CR = '\r';
    public const char CHAR_LF = '\n';
    public const string CRLF = "\r\n";
    public const string NEWLINE = "\n";
    #endregion --- Constants --------------------------------------------

    public static int DEFAULT_FIXED_WIDTH = 80;

    static private readonly Dictionary<EHorizontalRowType, char> _CharFinder = new Dictionary<EHorizontalRowType, char> {
      { EHorizontalRowType.Single, '-' },
      { EHorizontalRowType.Double, '=' },
      { EHorizontalRowType.Dot, '.' },
      { EHorizontalRowType.Underline, '_' },
      { EHorizontalRowType.Stars, '*' },
      { EHorizontalRowType.FullLight, '░' },
      { EHorizontalRowType.FullMedium, '▒' },
      { EHorizontalRowType.FullBold, '▓' },
      { EHorizontalRowType.Solid, '█' },
      { EHorizontalRowType.SingleIBM, '─' },
      { EHorizontalRowType.SingleIBMBold, '━' },
      { EHorizontalRowType.DoubleIBM, '═' },
      { EHorizontalRowType.Slash, '/' },
      { EHorizontalRowType.Backslash, '\\' },
      { EHorizontalRowType.Pipe, '|' }
    };

    #region --- Boxes --------------------------------------------
    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated. The border is filled with IBM boxes characters
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <returns>A string containing the message in the box</returns>
    public static string BuildDynamicIBM(string sourceString, int margin = 0, EStringAlignment alignment = EStringAlignment.Center, char filler = '·') {
      return BuildDynamic(sourceString, margin, alignment, filler, "╒═╕│╛═╘│");
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated.
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <param name="border">The border string (top-left/top/top-right/right/bottom-right/bottom/bottom-left/left)</param>
    /// <returns>A string containing the message in the box</returns>
    public static string BuildDynamic(string sourceString, int margin = 0, EStringAlignment alignment = EStringAlignment.Center, char filler = ' ', string border = "") {
      #region Validate parameters
      if (sourceString is null) {
        return null;
      }

      if (margin < 0) {
        margin = 0;
      }
      #endregion Validate parameters

      string CompletedBorder = $"{border}+-+|+-+|".Left(8);

      char TopLeft = CompletedBorder[0];
      char TopBar = CompletedBorder[1];
      char TopRight = CompletedBorder[2];
      char LeftBar = CompletedBorder[7];
      char RightBar = CompletedBorder[3];
      char BottomLeft = CompletedBorder[6];
      char BottomBar = CompletedBorder[5];
      char BottomRight = CompletedBorder[4];

      StringBuilder RetVal = new StringBuilder();

      string PreProcessedSourceString = sourceString.Replace(CRLF, NEWLINE);

      // Find larger string
      int MaxLength = 0;
      foreach (string StringItem in PreProcessedSourceString.Split(CHAR_LF)) {
        if (StringItem.Length > MaxLength) {
          MaxLength = StringItem.Length;
        }
      }

      RetVal.AppendLine($"{TopLeft}{new string(TopBar, MaxLength + (margin * 2) + 2)}{TopRight}");

      foreach (string StringItem in PreProcessedSourceString.Split(CHAR_LF)) {
        int LeftPadding = 0;
        int RightPadding = 0;

        switch (alignment) {
          case EStringAlignment.Left:
            RightPadding = MaxLength - StringItem.Length;
            break;
          case EStringAlignment.Right:
            LeftPadding = MaxLength - StringItem.Length;
            break;
          case EStringAlignment.Center:
            LeftPadding = Convert.ToInt32(Math.Floor((MaxLength - StringItem.Length) / 2d));
            RightPadding = MaxLength - StringItem.Length - LeftPadding;
            break;
        }

        RetVal.AppendLine($"{LeftBar}{new string(filler, margin)}{new string(filler, LeftPadding)} {StringItem} {new string(filler, RightPadding)}{new string(filler, margin)}{RightBar}");
      }
      RetVal.Append($"{BottomLeft}{new string(BottomBar, MaxLength + (margin * 2) + 2)}{BottomRight}");

      return RetVal.ToString();
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is DEFAULT_FIXED_WIDTH. If the message is larger than the box, it is split in several lines
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <param name="border">The border string (top-left/top/top-right/right/bottom-right/bottom/bottom-left/left)</param>
    /// <returns>A string with the box and the message</returns>
    public static string BuildFixedWidth(string sourceString, EStringAlignment alignment = EStringAlignment.Center, char filler = ' ', string border = "") {
      return BuildFixedWidth(sourceString, DEFAULT_FIXED_WIDTH, alignment, filler, border);
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is fixed. If the message is larger than the box, it is split in several lines
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="width">The width of the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <param name="border">The border string (top-left/top/top-right/right/bottom-right/bottom/bottom-left/left)</param>
    /// <returns>A string with the box and the message</returns>
    public static string BuildFixedWidth(string sourceString, int width, EStringAlignment alignment = EStringAlignment.Center, char filler = ' ', string border = "") {
      #region Validate parameters
      if (sourceString == null) {
        return null;
      }
      if (width <= 0) {
        width = DEFAULT_FIXED_WIDTH;
      }
      #endregion Validate parameters

      string CompletedBorder = string.Format("{0}+-+|+-+|", border).Left(8);

      char TopLeft = CompletedBorder[0];
      char TopBar = CompletedBorder[1];
      char TopRight = CompletedBorder[2];
      char LeftBar = CompletedBorder[7];
      char RightBar = CompletedBorder[3];
      char BottomLeft = CompletedBorder[6];
      char BottomBar = CompletedBorder[5];
      char BottomRight = CompletedBorder[4];

      int InnerWidth = width - 4;
      string PreProcessedSourceString = sourceString.Replace(CRLF, NEWLINE);

      StringBuilder RetVal = new StringBuilder();

      RetVal.AppendLine($"{TopLeft}{new string(TopBar, InnerWidth + 2)}{TopRight}");

      foreach (string StringItem in PreProcessedSourceString.Split(CHAR_LF)) {
        int StartPtr = 0;
        while (StartPtr < StringItem.Length) {
          string WorkString = StringItem.Substring(StartPtr, Math.Min(StringItem.Length - StartPtr, InnerWidth));
          StartPtr += WorkString.Length;

          int LeftPadding = 0;
          int RightPadding = 0;

          switch (alignment) {
            case EStringAlignment.Left:
              RightPadding = Math.Max(InnerWidth - WorkString.Length, 0);
              RetVal.AppendLine($"{LeftBar} {WorkString} {new string(filler, RightPadding)}{RightBar}");
              break;
            case EStringAlignment.Right:
              LeftPadding = Math.Max(InnerWidth - WorkString.Length, 0);
              RetVal.AppendLine($"{LeftBar}{new string(filler, LeftPadding)} {WorkString} {RightBar}");
              break;
            case EStringAlignment.Center:
              LeftPadding = Math.Max(Convert.ToInt32(Math.Floor((InnerWidth - WorkString.Length) / 2d)), 0);
              RightPadding = Math.Max(InnerWidth - WorkString.Length - LeftPadding, 0);
              RetVal.AppendLine($"{LeftBar}{new string(filler, LeftPadding)} {WorkString} {new string(filler, RightPadding)}{RightBar}");
              break;
          }


        }
      }
      RetVal.Append($"{BottomLeft}{new string(BottomBar, InnerWidth + 2)}{BottomRight}");
      return RetVal.ToString();
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is fixed. If the message is larger than the box, it is split in several lines.
    /// IBM characters are used for the border
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="width">The width of the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <returns>A string with the box and the message</returns>
    public static string BuildFixedWidthIBM(string sourceString, int width = 0, EStringAlignment alignment = EStringAlignment.Center, char filler = '·') {
      return BuildFixedWidth(sourceString, width, alignment, filler, "╒═╕│╛═╘│");
    }
    #endregion --- Boxes --------------------------------------------

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated. The border is filled with IBM boxes characters
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <returns>A string containing the message in the box</returns>
    public static string BoxIBM(this string source, int margin = 0, EStringAlignment alignment = EStringAlignment.Center, char filler = '.') {
      return BuildDynamicIBM(source, margin, alignment, filler);
    }

    /// <summary>
    /// Generate a box with the message inside it. The width of the box is dynamically calculated.
    /// </summary>
    /// <param name="sourceString">The message</param>
    /// <param name="margin">The margin around the message within the box</param>
    /// <param name="alignment">The alignment of the message within the box</param>
    /// <param name="filler">The character used to full the extra space aound the message within the box</param>
    /// <param name="border">The border string (top-left/top/top-right/right/bottom-right/bottom/bottom-left/left)</param>
    /// <returns>A string containing the message in the box</returns>
    public static string Box(this string source, int margin = 0, EStringAlignment alignment = EStringAlignment.Center, char filler = '.', string border = "") {
      return BuildDynamic(source, margin, alignment, filler, border);
    }

    #region --- Lines --------------------------------------------
    /// <summary>
    /// Build a string figuring an horizontal line. The length of the line is current console width.
    /// </summary>
    /// <returns>A string containing the horizontal line</returns>
    public static string BuildHorizontalRow() {
      return BuildHorizontalRow(-1, EHorizontalRowType.Single);
    }

    /// <summary>
    /// Build a string figuring an horizontal line. The length of the line is current console width.
    /// </summary>
    /// <param name="rowType">The type of char to use for the drawing</param>
    /// <returns>A string containing the horizontal line</returns>
    public static string BuildHorizontalRow(EHorizontalRowType rowType = EHorizontalRowType.Single) {
      return BuildHorizontalRow(-1, rowType);
    }

    /// <summary>
    /// Build a string figuring an horizontal line.
    /// </summary>
    /// <param name="width">The length of the line. -1 means current console width</param>
    /// <param name="rowType">The type of char to use for the drawing</param>
    /// <returns>A string containing the horizontal line</returns>
    public static string BuildHorizontalRow(int width, EHorizontalRowType rowType = EHorizontalRowType.Single) {

      if (width == -1) {
        width = Console.WindowWidth;
      }

      if (width <= 0) {
        width = DEFAULT_FIXED_WIDTH;
      }

      return new string(_CharFinder[rowType], width);

    }

    /// <summary>
    /// Build a string figuring an horizontal line, with a message in it.  The length of the line is current console width.
    /// </summary>
    /// <param name="message">The text message</param>
    /// <param name="rowType">The type of char to use for the drawing</param>
    /// <returns>A string containing the horizontal line with the message embedded</returns>
    public static string BuildHorizontalRowWithText(string sourceString, EHorizontalRowType rowType = EHorizontalRowType.Single) {
      return BuildHorizontalRowWithText(sourceString, -1, rowType);
    }

    /// <summary>
    /// Build a string figuring an horizontal line, with a message in it
    /// </summary>
    /// <param name="message">The text message</param>
    /// <param name="width">The length of the line. -1 means current console width. If the message is bigger than the width, it is truncated.</param>
    /// <param name="rowType">The type of char to use for the drawing</param>
    /// <returns>A string containing the horizontal line with the message embedded</returns>
    public static string BuildHorizontalRowWithText(string message, int width = -1, EHorizontalRowType rowType = EHorizontalRowType.Single) {

      if (message is null) {
        message = "";
      }

      if (width == -1) {
        width = Console.WindowWidth;
      }

      if (width <= 0) {
        width = DEFAULT_FIXED_WIDTH;
      }

      int SourceStringLength = message.Length;

      if (width < SourceStringLength) {
        width = SourceStringLength;
      }

      int BeforeText = 2;
      int AfterText = Math.Max(0, width - BeforeText - SourceStringLength - 2);

      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(new string(_CharFinder[rowType], BeforeText));
      RetVal.Append(message);
      RetVal.Append(new string(_CharFinder[rowType], AfterText));


      return RetVal.ToString().Left(width);

    }

    /// <summary>
    /// Generate a string of spaces
    /// </summary>
    /// <param name="number">The number of spaces</param>
    /// <returns>A string of spaces</returns>
    public static string Spaces(int number) {
      if (number <= 0) {
        return "";
      }
      return new string(' ', number);
    }
    #endregion --- Lines --------------------------------------------

  }
}
