using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Diagnostic.Logging {
    public abstract class TLoggable
    {
        #region --- Logging --------------------------------------------
        public ILogger Logger { get; set; } = new TTraceLogger();

        public void Log(string text)
        {
            Logger.Log(text, this.GetType().Name);
        }

        public void Log(string text, string source)
        {
            Logger.Log(text, source);
        }

        public void LogWarning(string text)
        {
            Logger.LogWarning(text, this.GetType().Name);
        }

        public void LogWarning(string text, string source)
        {
            Logger.LogWarning(text, source);
        }

        public void LogError(string text)
        {
            Logger.LogError(text, this.GetType().Name);
        }

        public void LogError(string text, string source)
        {
            Logger.LogError(text, source);
        }
        public void LogDebug(string text)
        {
            Logger.LogDebug(text, this.GetType().Name);
        }

        public void LogDebug(string text, string source)
        {
            Logger.LogDebug(text, source);
        }
        public void LogDebugEx(string text)
        {
            Logger.LogDebugEx(text, this.GetType().Name);
        }
        public void LogDebugEx(string text, string source)
        {
            Logger.LogDebugEx(text, source);
        }
        #endregion --- Logging -----------------------------------------

        public TLoggable()
        {
            Logger = TLogger.SYSTEM_LOGGER;
        }
        public TLoggable(ILogger logger)
        {
            Logger = TLogger.Create(logger);
        }
    }
}
