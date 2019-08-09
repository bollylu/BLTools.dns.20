using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BLTools {
  public static class Log {

    public static ErrorLevel DebugLevel = ErrorLevel.Info;

    public static ISimpleLogger Writer {
      get {
        lock ( _WriterLock ) {
          if ( _Writer == null ) {
            _Writer = new LoggerDefault();
          }
          return _Writer;
        }
      }
      set {
        lock ( _WriterLock ) {
          _Writer = value;
        }
      }
    }
    private static ISimpleLogger _Writer;

    private static readonly object _WriterLock = new object();

    public static void Write(string message) {
      lock ( _WriterLock ) {
        Writer.Write(message, ErrorLevel.Info);
      }
    }
    public static void Write(string message, ErrorLevel severity) {
      if ( DebugLevel >= severity ) {
        lock ( _WriterLock ) {
          Writer.Write(message, severity);
        }
      }
    }
  }
}
