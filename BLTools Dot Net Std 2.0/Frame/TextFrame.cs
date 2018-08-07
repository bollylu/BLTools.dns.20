using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Frames {
  public class TextFrame : BaseFrameElement {

    protected ContentBuffer Content;
    protected bool IsDirtyContent = true;

    public TextFrame(IFrameElement owner) : base(owner) {
      Content = new ContentBuffer(this);
    }

    protected virtual void DrawContent(bool foreceRefresh = false) {
      if ( IsDirtyContent || foreceRefresh ) {
        Cursor.SavePosition();
        Content.Draw();
        Cursor.RestorePosition();
        IsDirtyContent = false;
      }
    }
  }
}
