using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Events
    public event EventHandler OnDatabaseOpened;
    public event EventHandler OnDatabaseClosed;

    public event EventHandler OnTransactionStarted;
    public event EventHandler OnTransactionCommit;
    public event EventHandler OnTransactionRollback;
    #endregion Events

    public event EventHandler<string> OnSelectQuery;
    private void NotifySelectQuery(string query) {
      if ( OnSelectQuery != null ) {
        OnSelectQuery(this, query);
      }
    }
    private void NotifySelectQuery(IDbCommand command) {
      if ( OnSelectQuery != null ) {
        OnSelectQuery(this, command.CommandText);
      }
    }


    public event EventHandler<IEnumerable<string>> OnSelectNonQuery;
    private void NotifySelectNonQuery(string query) {
      if ( OnSelectNonQuery != null ) {
        OnSelectNonQuery(this, new string[] { query });
      }
    }
    private void NotifySelectNonQuery(IDbCommand command) {
      if ( OnSelectNonQuery != null ) {
        OnSelectNonQuery(this, new string[] { command.CommandText });
      }
    }

    private void NotifySelectNonQuery(IEnumerable<string> queries) {
      if ( OnSelectNonQuery != null ) {
        OnSelectNonQuery(this, queries);
      }
    }

    private void NotifySelectNonQuery(IEnumerable<IDbCommand> commands) {
      if ( OnSelectNonQuery != null ) {
        OnSelectNonQuery(this, commands.Select(x => x.CommandText));
      }
    }
  }
}
