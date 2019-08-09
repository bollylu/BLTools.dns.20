using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BLTools.Diagnostic.Logging {
    public interface ILogger : IDisposable
    {
        int SourceWidth { get; set; }

        ESeverity SeverityLimit { get; set; }

        TraceListener Listener { get; }

        void Log(string text, string source = "", ESeverity severity = ESeverity.Info);
        void LogWarning(string text, string source = "");
        void LogError(string text, string source = "");
        void LogFatal(string text, string source = "");
        void LogDebug(string text, string source = "");
        void LogDebugEx(string text, string source = "");
    }
}
