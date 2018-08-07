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
    public enum StringAlignmentEnum {
      Left,
      Center,
      Right
    }
    public enum HorizontalRowType {
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

    public static int DEFAULT_FIXED_WIDTH = 80;

    public static string BuildDynamicIBM(string sourceString, int margin = 0, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = '·') {
      return BuildDynamic(sourceString, margin, alignment, filler, "╒═╕│╛═╘│");
    }
    public static string BuildDynamic(string sourceString, int margin = 0, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = ' ', string border = "") {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( margin < 0 ) {
        margin = 0;
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

      StringBuilder RetVal = new StringBuilder();

      string PreProcessedSourceString = sourceString.Replace("\r\n", "\n");

      // Find larger string
      int MaxLength = 0;
      foreach ( string StringItem in PreProcessedSourceString.Split('\n') ) {
        if ( StringItem.Length > MaxLength ) {
          MaxLength = StringItem.Length;
        }
      }

      RetVal.AppendLine($"{TopLeft}{new string(TopBar, MaxLength + ( margin * 2 ) + 2)}{TopRight}");

      foreach ( string StringItem in PreProcessedSourceString.Split('\n') ) {
        int LeftPadding = 0;
        int RightPadding = 0;

        switch ( alignment ) {
          case StringAlignmentEnum.Left:
            RightPadding = MaxLength - StringItem.Length;
            break;
          case StringAlignmentEnum.Right:
            LeftPadding = MaxLength - StringItem.Length;
            break;
          case StringAlignmentEnum.Center:
            LeftPadding = Convert.ToInt32(Math.Floor(( MaxLength - StringItem.Length ) / 2d));
            RightPadding = MaxLength - StringItem.Length - LeftPadding;
            break;
        }
        RetVal.AppendLine(string.Format("{0}{1}{2} {3} {4}{5}{6}", LeftBar,
                                                                   new string(filler, margin),
                                                                   new string(filler, LeftPadding),
                                                                   StringItem,
                                                                   new string(filler, RightPadding),
                                                                   new string(filler, margin),
                                                                   RightBar));
      }
      RetVal.Append($"{BottomLeft}{new string(BottomBar, MaxLength + ( margin * 2 ) + 2)}{BottomRight}");

      return RetVal.ToString();
    }

    public static string BuildFixedWidth(string sourceString, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = ' ', string border = "") {
      return BuildFixedWidth(sourceString, DEFAULT_FIXED_WIDTH, alignment, filler, border);
    }
    public static string BuildFixedWidth(string sourceString, int width, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = ' ', string border = "") {
      #region Validate parameters
      if ( sourceString == null ) {
        return null;
      }
      if ( width <= 0 ) {
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
      string PreProcessedSourceString = sourceString.Replace("\r\n", "\n");

      StringBuilder RetVal = new StringBuilder();

      RetVal.AppendLine($"{TopLeft}{new string(TopBar, InnerWidth + 2)}{TopRight}");

      foreach ( string StringItem in PreProcessedSourceString.Split('\n') ) {
        int StartPtr = 0;
        while ( StartPtr < StringItem.Length ) {
          string WorkString = StringItem.Substring(StartPtr, Math.Min(StringItem.Length - StartPtr, InnerWidth));
          StartPtr += WorkString.Length;

          int LeftPadding = 0;
          int RightPadding = 0;

          switch ( alignment ) {
            case StringAlignmentEnum.Left:
              RightPadding = Math.Max(InnerWidth - WorkString.Length, 0);
              RetVal.AppendLine($"{LeftBar} {WorkString} {new string(filler, RightPadding)}{RightBar}");
              break;
            case StringAlignmentEnum.Right:
              LeftPadding = Math.Max(InnerWidth - WorkString.Length, 0);
              RetVal.AppendLine($"{LeftBar}{new string(filler, LeftPadding)} {WorkString} {RightBar}");
              break;
            case StringAlignmentEnum.Center:
              LeftPadding = Math.Max(Convert.ToInt32(Math.Floor(( InnerWidth - WorkString.Length ) / 2d)), 0);
              RightPadding = Math.Max(InnerWidth - WorkString.Length - LeftPadding, 0);
              RetVal.AppendLine($"{LeftBar}{new string(filler, LeftPadding)} {WorkString} {new string(filler, RightPadding)}{RightBar}");
              break;
          }


        }
      }
      RetVal.Append($"{BottomLeft}{new string(BottomBar, InnerWidth + 2)}{BottomRight}");
      return RetVal.ToString();
    }
    public static string BuildFixedWidthIBM(string sourceString, int width = 0, StringAlignmentEnum alignment = StringAlignmentEnum.Center, char filler = '·') {
      return BuildFixedWidth(sourceString, width, alignment, filler, "╒═╕│╛═╘│");
    }

    static private Dictionary<HorizontalRowType, char> CharFinder = new Dictionary<HorizontalRowType, char> {
      { HorizontalRowType.Single, '-' },
      { HorizontalRowType.Double, '=' },
      { HorizontalRowType.Dot, '.' },
      { HorizontalRowType.Underline, '_' },
      { HorizontalRowType.Stars, '*' },
      { HorizontalRowType.FullLight, '░' },
      { HorizontalRowType.FullMedium, '▒' },
      { HorizontalRowType.FullBold, '▓' },
      { HorizontalRowType.Solid, '█' },
      { HorizontalRowType.SingleIBM, '─' },
      { HorizontalRowType.SingleIBMBold, '━' },
      { HorizontalRowType.DoubleIBM, '═' },
      { HorizontalRowType.Slash, '/' },
      { HorizontalRowType.Backslash, '\\' },
      { HorizontalRowType.Pipe, '|' }
    };

    public static string BuildHorizontalRow() {
      return BuildHorizontalRow(-1, HorizontalRowType.Single);
    }
    public static string BuildHorizontalRow(HorizontalRowType rowType = HorizontalRowType.Single) {
      return BuildHorizontalRow(-1, rowType);
    }
    public static string BuildHorizontalRow(int width, HorizontalRowType rowType = HorizontalRowType.Single) {

      if ( width == -1 ) {
        width = Console.WindowWidth;
      }

      if ( width <= 0 ) {
        width = DEFAULT_FIXED_WIDTH;
      }

      return new string(CharFinder[rowType], width);

    }

    public static string BuildHorizontalRowWithText(string sourceString, HorizontalRowType rowType = HorizontalRowType.Single) {
      return BuildHorizontalRowWithText(sourceString, -1, rowType);
    }
    public static string BuildHorizontalRowWithText(string sourceString, int width = -1, HorizontalRowType rowType = HorizontalRowType.Single) {

      if ( sourceString == null ) {
        sourceString = "";
      }

      if ( width == -1 ) {
        width = Console.WindowWidth;
      }

      if ( width <= 0 ) {
        width = DEFAULT_FIXED_WIDTH;
      }

      int SourceStringLength = sourceString.Length;

      if ( width < SourceStringLength ) {
        width = SourceStringLength;
      }

      int BeforeText = 2;
      int AfterText = Math.Max(0, width - BeforeText - SourceStringLength - 2);

      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(new string(CharFinder[rowType], BeforeText));
      RetVal.Append(sourceString);
      RetVal.Append(new string(CharFinder[rowType], AfterText));


      return RetVal.ToString().Left(width);

    }
  }
}
