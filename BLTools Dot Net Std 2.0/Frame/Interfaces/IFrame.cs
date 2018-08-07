using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Frames {
  public interface IFrame {

    int Width { get; }
    int Height { get; }

    int AbsoluteRow { get; }
    int AbsoluteCol { get; }

    List<IFrame> FrameItems { get; }

    int CurrentRow { get; set; }
    int CurrentCol { get; set; }

    bool IsDirtyContent { get; }

  }
}
