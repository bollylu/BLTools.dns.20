using BLTools.Diagnostic.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace BLTools.MVVM {
  public class TRelayCommand : TLoggable, ICommand {

    protected readonly Action _ExecuteAction;
    protected Predicate<object> _TestIfCanExecute;

    protected bool _CanExecute;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TRelayCommand() { }

    public TRelayCommand(ILogger logger) : base(logger) { }

    public TRelayCommand(Action executeAction) {
      _ExecuteAction = executeAction;
    }
    public TRelayCommand(Action executeAction, ILogger logger) : base(logger) {
      _ExecuteAction = executeAction;
    }
    public TRelayCommand(Action executeAction, Predicate<object> testIfCanExecute) {
      _ExecuteAction = executeAction;
      _TestIfCanExecute = testIfCanExecute;
      CanExecuteChanged += TRelayCommand_CanExecuteChanged;
    }

    public TRelayCommand(Action executeAction, Predicate<object> testIfCanExecute, ILogger logger) : base(logger) {
      _ExecuteAction = executeAction;
      _TestIfCanExecute = testIfCanExecute;
      CanExecuteChanged += TRelayCommand_CanExecuteChanged;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public event EventHandler CanExecuteChanged;
    protected void TRelayCommand_CanExecuteChanged(object sender, EventArgs e) {
      LogDebugEx($"Can execute changed");
      _CanExecute = CanExecute(sender);
    }

    public virtual void Execute(object parameter) {
      if (_CanExecute) {
        LogDebugEx("Executing command");
        _ExecuteAction();
      }
    }

    public bool CanExecute(object parameter) {
      return _TestIfCanExecute(parameter);
    }
  }

  /*******************************************************************************************************************************/

  public class TRelayCommand<T> : TRelayCommand {

    protected readonly new Action<T> _ExecuteAction;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TRelayCommand(Action<T> executeAction) {
      _ExecuteAction = executeAction;
    }

    public TRelayCommand(Action<T> executeAction, Predicate<object> testIfCanExecute) {
      _ExecuteAction = executeAction;
      _TestIfCanExecute = testIfCanExecute;
      CanExecuteChanged += TRelayCommand_CanExecuteChanged;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override void Execute(object parameter) {
      if (_CanExecute) {
        _ExecuteAction((T)BLTools.BLConverter.BLConvert<T>(parameter, CultureInfo.CurrentCulture, default(T)));
      }
    }
  }

}
