using BLTools.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BLTools.Frames {
  public class Frame : IFrame {

    #region --- Frame position and size --------------------------------------------
    public Point TopLeft { get; set; }
    public Point BottomRight { get; set; }
    public int Width => BottomRight.X - TopLeft.X + 1;
    public int Height => BottomRight.Y - TopLeft.Y + 1;
    #endregion --- Frame position and size --------------------------------------------

    public List<IFrameElement> FrameItems { get; } = new List<IFrameElement>();

    public ConsoleColor Foreground { get; set; } = ConsoleColor.White;
    public ConsoleColor Background { get; set; } = ConsoleColor.Black;

    public int CurrentRow { get; set; }
    public int CurrentCol { get; set; }

    public int AbsoluteRow => TopLeft.Y + CurrentRow + 1;
    public int AbsoluteCol => TopLeft.X + CurrentCol + 1;

    #region --- Title --------------------------------------------
    public string Title {
      get {
        return _Title;
      }
      set {
        _Title = value;
        IsDirtyTitle = true;
      }
    }
    private string _Title;
    protected int TitleCol => TopLeft.X + 3;
    protected int TitleRow => TopLeft.Y;

    protected bool IsDirtyTitle = true;
    #endregion --- Title --------------------------------------------

    protected bool IsDirtyBorder = true;
    public bool DelayedRefresh = false;
    public bool ManualRefresh = false;

    public Frame(int left, int top, int right, int bottom) {
      TopLeft = new Point {
        X = left,
        Y = top
      };
      BottomRight = new Point {
        X = right,
        Y = bottom
      };
      CurrentCol = 0;
      CurrentRow = 0;

      FrameItems.Add(new TextFrame(this));
    }

    public virtual void Refresh() {
      Cursor.SavePosition();

      if ( IsDirtyBorder ) {
        DrawBorder();
      }

      if ( IsDirtyContent ) {
        DrawContent();
      }
      Cursor.RestorePosition();
    }

    protected virtual void DrawBorder(bool forceRefresh = false) {
      if ( IsDirtyBorder || forceRefresh ) {
        Line.DrawHLine(TopLeft, Width);
        Line.DrawHLine(TopLeft.X, BottomRight.Y, Width);
        Line.DrawVLine(TopLeft, Height);
        Line.DrawVLine(BottomRight.X, TopLeft.Y, Height);
        DrawTitle(forceRefresh);
        IsDirtyBorder = false;
      }
    }
    protected virtual void DrawTitle(bool forceRefresh = false) {
      if ( IsDirtyTitle || forceRefresh ) {
        Cursor.SavePosition();
        Console.SetCursorPosition(TitleCol, TitleRow);
        Console.Write($"[{Title}]");
        Cursor.RestorePosition();
        IsDirtyTitle = false;
      }
    }
    protected virtual void DrawContent(bool foreceRefresh = false) {
      if ( IsDirtyContent || foreceRefresh ) {
        Cursor.SavePosition();
        Content.Draw();
        Cursor.RestorePosition();
        IsDirtyContent = false;
      }
    }

    public virtual void Write(string text) {
      Content.Write(text.Replace("\r\n","\n"));
      IsDirtyContent = true;
      if ( !DelayedRefresh && !ManualRefresh ) {
        Refresh();
      }
    }

    public virtual void Write(object data) {
      Write(data.ToString());
    }

    public virtual void WriteLine(string text) {
      DelayedRefresh = true;
      Write(text.Replace("\r\n", "\n"));
      DelayedRefresh = false;
      Write("\n");
    }

  }
}
