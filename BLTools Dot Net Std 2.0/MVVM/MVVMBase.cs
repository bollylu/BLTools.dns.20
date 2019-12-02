using BLTools.Diagnostic.Logging;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BLTools.MVVM {
  /// <summary>
  /// Base class for a new MVVM class
  /// </summary>
  public abstract class MVVMBase : TLoggable, INotifyPropertyChanged {

    /// <summary>
    /// Minimum level for tracing. If under the level, the callback is skipped
    /// </summary>
    public static ErrorLevel MinTraceLevel = ErrorLevel.Info;

    /// <summary>
    /// Indicates when an operation is in progress
    /// </summary>
    public virtual bool WorkInProgress {
      get {
        return _WorkInProgress;
      }
      set {
        if (value != _WorkInProgress) {
          _WorkInProgress = value;
          NotifyPropertyChanged(nameof(WorkInProgress));
        }
      }
    }
    private bool _WorkInProgress;

    #region --- Progress bar ----------------------------------------------------------------------
    /// <summary>
    /// Request progress bar initialisation. Provides the maximum value
    /// </summary>
    public static event EventHandler<IntEventArgs> OnInitProgressBar;

    /// <summary>
    /// Indicates progress bar change. Provides new current value
    /// </summary>
    public static event EventHandler<IntEventArgs> OnProgressBarNewValue;

    /// <summary>
    /// Indicates progress bar termination
    /// </summary>
    public static event EventHandler OnProgressBarCompleted;

    /// <summary>
    /// Notify that a progress bar is to be reinitialised
    /// </summary>
    /// <param name="maxValue">Maximum value of the bar</param>
    protected virtual void NotifyInitProgressBar(int maxValue) {
      OnInitProgressBar?.Invoke(this, new IntEventArgs(maxValue));
    }

    /// <summary>
    /// Notify the progress bar of a new current value
    /// </summary>
    /// <param name="value">The current value</param>
    protected virtual void NotifyProgressBarNewValue(int value) {
      OnProgressBarNewValue?.Invoke(this, new IntEventArgs(value));
    }

    /// <summary>
    /// Notify a progress bar of a job completion, with optional message and status
    /// </summary>
    /// <param name="message">The optional message</param>
    /// <param name="status">The optional status (true/false)</param>
    protected virtual void NotifyProgressBarCompleted(string message = "", bool status = true) {
      OnProgressBarCompleted?.Invoke(this, EventArgs.Empty);
      NotifyExecutionCompleted(message, status);
    }
    #endregion --- Progress bar ------------------------------------------------------------------

    #region --- Execution status ------------------------------------------------------------------
    /// <summary>
    /// Indicates a change in operation status. Transmit a string.
    /// </summary>
    public static event EventHandler<StringEventArgs> OnExecutionStatus;
    /// <summary>
    /// Indicates that an operation is completed. Provides a bool to reflect the operation success and optionally a message
    /// </summary>
    public static event EventHandler<BoolAndMessageEventArgs> OnExecutionCompleted;

    /// <summary>
    /// Sends an empty execution status to clear it
    /// </summary>
    protected virtual void ClearExecutionStatus() {
      OnExecutionStatus?.Invoke(this, new StringEventArgs(""));
    }

    /// <summary>
    /// Sends an execution status message
    /// </summary>
    /// <param name="statusMessage">The message</param>
    protected virtual void NotifyExecutionStatus(string statusMessage = "") {
      OnExecutionStatus?.Invoke(this, new StringEventArgs(statusMessage));
    }

    /// <summary>
    /// Sends an execution completed status message
    /// </summary>
    /// <param name="statusMessage">The message</param>
    /// <param name="completionStatus">The status at the completion of the process</param>
    protected virtual void NotifyExecutionCompleted(string statusMessage = "", bool completionStatus = false) {
      OnExecutionCompleted?.Invoke(this, new BoolAndMessageEventArgs(completionStatus, statusMessage));
    }
    #endregion --- Execution status ---------------------------------------------------------------

    #region --- Execution progress ----------------------------------------------------------------
    /// <summary>
    /// Indicates a change in operation progress. Provides a message and optionally a integer value
    /// </summary>
    public static event EventHandler<IntAndMessageEventArgs> OnExecutionProgress;

    /// <summary>
    /// Sends an empty execution progress message to clear it
    /// </summary>
    public virtual void ClearExecutionProgress() {
      OnExecutionProgress?.Invoke(this, new IntAndMessageEventArgs(0, ""));
    }

    /// <summary>
    /// Sends a message for progress
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="errorlevel">The optional errorlevel (will be filtered by MinTraceLevel)</param>
    public virtual void NotifyExecutionProgress(string message = "", ErrorLevel errorlevel = ErrorLevel.Info) {
      NotifyExecutionProgress(message, 0, errorlevel);
    }

    /// <summary>
    /// Sends a message and an integer to indicate progress
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="progress">The integer</param>
    /// <param name="errorlevel">The optional errorlevel (will be filtered by MinTraceLevel)</param>
    public virtual void NotifyExecutionProgress(string message, int progress, ErrorLevel errorlevel = ErrorLevel.Info) {
      if (errorlevel < MinTraceLevel) {
        return;
      }
      OnExecutionProgress?.Invoke(this, new IntAndMessageEventArgs(progress, message));
    }
    #endregion --- Execution progress -------------------------------------------------------------

    #region --- Execution error -------------------------------------------------------------------
    /// <summary>
    /// Indicates an error in operation progress. Provides a message and an errorlevel
    /// </summary>
    public static event EventHandler<IntAndMessageEventArgs> OnExecutionError;

    /// <summary>
    /// Sends an message to indicate an error
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="errorlevel">The optional errorlevel (will be filtered by MinTraceLevel)</param>
    protected virtual void NotifyExecutionError(string message = "", ErrorLevel errorlevel = ErrorLevel.Warning) {
      if (errorlevel < MinTraceLevel) {
        return;
      }
      OnExecutionError?.Invoke(this, new IntAndMessageEventArgs((int)errorlevel, message));
    }
    #endregion --- Execution error ----------------------------------------------------------------

    #region === INotifyPropertyChanged ============================================================
    /// <summary>
    /// Event handler for when a property was changed
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Calls any hook when the property was changed
    /// </summary>
    /// <param name="propertyName"></param>
    protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "") {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion === INotifyPropertyChanged =========================================================

  }
}
