using BLTools.Diagnostic.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace BLTools.MVVM {
  public class TRelayCommand : TLoggable, ICommand {

    protected Action _ExecuteAction;

    protected Predicate<object> _TestIfCanExecute;

    protected Action<bool> _WhenCanExecuteChanged;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TRelayCommand() { }

    public TRelayCommand(ILogger logger) {
      SetLogger(logger);
    }

    public TRelayCommand(Action executeAction) {
      _ExecuteAction = executeAction;
      
    }
    public TRelayCommand(Action executeAction, ILogger logger) {
      SetLogger(logger);
      _ExecuteAction = executeAction;
    }
    public TRelayCommand(Action executeAction, Predicate<object> testIfCanExecute, Action<bool> whenCanExecutedChangedAction) {
      _ExecuteAction = executeAction;
      _TestIfCanExecute = testIfCanExecute;
      _WhenCanExecuteChanged = whenCanExecutedChangedAction;
    }

    public TRelayCommand(Action executeAction, Predicate<object> testIfCanExecute, Action<bool> whenCanExecutedChangedAction, ILogger logger) {
      SetLogger(logger);
      _ExecuteAction = executeAction;
      _TestIfCanExecute = testIfCanExecute;
      _WhenCanExecuteChanged = whenCanExecutedChangedAction;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public event EventHandler CanExecuteChanged;

    public virtual void Execute(object parameter) {
      if (CanExecute(parameter)) {
        _ExecuteAction();
      }
    }

    public bool CanExecute(object parameter) {
      if (_TestIfCanExecute == null) {
        return true;
      }
      
      bool RetVal = _TestIfCanExecute(parameter);

      NotifyCanExecuteChanged(RetVal);
      
      return RetVal;
    }

    public void NotifyCanExecuteChanged(bool state) {
      _WhenCanExecuteChanged?.Invoke(state);
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  /*******************************************************************************************************************************/

  public class TRelayCommand<T> : TRelayCommand {

    protected readonly new Action<T> _ExecuteAction;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TRelayCommand(Action<T> executeAction) {
      _ExecuteAction = executeAction;
    }

    public TRelayCommand(Action<T> executeAction, Predicate<object> testIfCanExecute, Action<bool> whenCanExecutedChangedAction) {
      _ExecuteAction = executeAction;
      _TestIfCanExecute = testIfCanExecute;
      _WhenCanExecuteChanged = whenCanExecutedChangedAction;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override void Execute(object parameter) {
      if (CanExecute(parameter)) {
        _ExecuteAction((T)BLTools.BLConverter.BLConvert<T>(parameter, CultureInfo.CurrentCulture, default(T)));
      }
    }
  }

}
