using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Frames {
  public interface IFrameElement : IFrame {
    IFrameElement Owner { get; }

    
  }
}
