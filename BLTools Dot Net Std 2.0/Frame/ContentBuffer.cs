using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Frames {
  public class ContentBuffer : IDisposable {

    protected IFrameElement Owner;
    public static char FILLER_CHAR = ' ';

    internal bool IsDirty = true;

    private int Width => Owner.Width - 2;
    private int Height => Owner.Height - 2;

    protected int Col = 0;
    protected int Row = 0;

    internal readonly List<ContentBufferRow> Rows = new List<ContentBufferRow>();

    public ContentBuffer(IFrameElement frame) {
      Owner = frame;
      _InitBuffer();
    }

    private void _InitBuffer() {
      for ( int i = 0; i < Height; i++ ) {
        Rows.Add(new ContentBufferRow(Width));
      }
      IsDirty = true;
    }

    public void Dispose() {
      Owner = null;
      Rows.ForEach(x => x.Dispose());
    }

    public void Clear() {
      Rows.Clear();
      _InitBuffer();
    }

    public virtual void Write(string text) {
      foreach ( char CharItem in text ) {
        switch ( CharItem ) {
          case '\n':
            Col = 0;
            Row++;
            break;
          default:
            Rows[Row].Content[Col] = CharItem;
            Col++;
            if ( Col == Width ) {
              Col = 0;
              Row++;
            }
            break;
        }
        if ( Row == Height ) {
          Scroll();
          Row--;
        }
      }
    }

    public virtual void Draw() {
      Owner.CurrentRow = 0;
      Owner.CurrentCol = 0;
      foreach ( ContentBufferRow ContentBufferRowItem in Rows ) {
        Cursor.SetCursorPosition(Owner.AbsoluteCol, Owner.AbsoluteRow);
        foreach ( char CharItem in ContentBufferRowItem.Content ) {
          Console.Write(CharItem);
          Owner.CurrentCol++;
          if ( Owner.CurrentCol == Owner.Width ) {
            Owner.CurrentCol = 0;
            Owner.CurrentRow++;
          }
        }
        Owner.CurrentCol = 0;
        Owner.CurrentRow++;
      }
    }

    private void Scroll() {
      Rows.RemoveAt(0);
      Rows.Add(new ContentBufferRow(Width));
    }
  }
}
