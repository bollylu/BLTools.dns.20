using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Diagnostic.Logging {

  public abstract class ALoggable {
    #region --- Logging --------------------------------------------
    public ILogger Logger { get; set; } = ALogger.DEFAULT_LOGGER;

    #region --- Log --------------------------------------------
    public void Log(string text) {
      Logger.Log(text, this.GetType().Name);
    }
    public void Log(string text, string source) {
      Logger.Log(text, source);
    }
    #endregion --- Log --------------------------------------------

    #region --- LogWarning --------------------------------------------
    public void LogWarning(string text) {
      Logger.LogWarning(text, this.GetType().Name);
    }

    public void LogWarning(string text, string source) {
      Logger.LogWarning(text, source);
    }
    #endregion --- LogWarning --------------------------------------------

    #region --- LogError --------------------------------------------
    public void LogError(string text) {
      Logger.LogError(text, this.GetType().Name);
    }

    public void LogError(string text, string source) {
      Logger.LogError(text, source);
    }
    #endregion --- LogError --------------------------------------------

    #region --- LogDebug --------------------------------------------
    public void LogDebug(string text) {
      Logger.LogDebug(text, this.GetType().Name);
    }

    public void LogDebug(string text, string source) {
      Logger.LogDebug(text, source);
    }
    #endregion --- LogDebug --------------------------------------------

    #region --- LogDebugEx --------------------------------------------
    public void LogDebugEx(string text) {
      Logger.LogDebugEx(text, this.GetType().Name);
    }
    public void LogDebugEx(string text, string source) {
      Logger.LogDebugEx(text, source);
    }
    #endregion --- LogDebugEx --------------------------------------------

    #region --- LogFatal --------------------------------------------
    public void LogFatal(string text) {
      Logger.LogFatal(text, this.GetType().Name);
    }
    public void LogFatal(string text, string source) {
      Logger.LogFatal(text, source);
    }
    #endregion --- LogFatal --------------------------------------------

    #endregion --- Logging -----------------------------------------

    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
  }

    [Obsolete("Use ALoggable instead")]
    public abstract class TLoggable : ALoggable { }

}
