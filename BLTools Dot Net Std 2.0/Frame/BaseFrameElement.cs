using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Frames {
  public abstract class BaseFrameElement : IFrameElement {
    public IFrameElement Owner {
      get {
        return _Owner;
      }
    }
    protected readonly IFrameElement _Owner;

    public List<IFrame> FrameItems { get; } = new List<IFrame>();

    public virtual int Width {
      get {
        if ( Owner == null ) {
          return 0;
        }
        return Owner.Width;
      }
    }
    public virtual int Height {
      get {
        if ( Owner == null ) {
          return 0;
        }
        return Owner.Height;
      }
    }

    public int AbsoluteRow { get => throw new NotImplementedException(); }
    public int AbsoluteCol { get => throw new NotImplementedException(); }

    public BaseFrameElement(IFrameElement owner) {
      _Owner = owner;
    }

  }
}
