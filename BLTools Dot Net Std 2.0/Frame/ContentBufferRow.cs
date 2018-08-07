using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Frames {
  internal class ContentBufferRow : IDisposable {

    internal char[] Content;

    internal ContentBufferRow(int size) {
      Content = new char[size];
      for ( int i = 0; i < size; i++ ) {
        Content[i] = ContentBuffer.FILLER_CHAR;
      }
    }

    public void Dispose() {
      Content = null;
    }
  }
}
