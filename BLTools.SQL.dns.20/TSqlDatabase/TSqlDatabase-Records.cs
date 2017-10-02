using BLTools.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Records management

    #region --- Select multiple records --------------------------------------------
    public virtual IEnumerable<T> SelectQuery<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if ( string.IsNullOrWhiteSpace(query) ) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return new List<T>();
      }

      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return new List<T>();
      }
      #endregion Validate parameters
      return SelectQuery<T>(new SqlCommand(query), mapMethod);
    }

    public virtual IEnumerable<T> SelectQuery<T>(IDbCommand command, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if ( command == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return new List<T>();
      }
      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return new List<T>();
      }
      #endregion Validate parameters

      NotifySelectQuery(command);

      List<T> RetVal = new List<T>();
      bool LocalTransaction = false;
      try {
        if ( !IsOpened ) {
          LocalTransaction = true;
          TryOpen();
        }
        command.Connection = Connection;
        command.Transaction = Transaction;
        using ( IDataReader R = command.ExecuteReader() ) {
          while ( R.Read() ) {
            RetVal.Add(mapMethod(R));
          }
          R.Close();
        }
      } catch ( Exception ex ) {
        Trace.WriteLine($"Error during select : {command.CommandText} : {ex.Message}");
      } finally {
        if ( LocalTransaction ) {
          TryClose();
        }
      }
      return RetVal;
    }

    public virtual IEnumerable<T> SelectQuery<T>(IDbCommand command, Func<TRecordCacheCollection, T> mapMethod) {
      #region Validate parameters
      if ( command == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return new List<T>();
      }
      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return new List<T>();
      }
      #endregion Validate parameters

      NotifySelectQuery(command);

      List<T> RetVal = new List<T>();
      bool LocalTransaction = false;
      try {
        if ( !IsOpened ) {
          LocalTransaction = true;
          TryOpen();
        }
        command.Connection = Connection;
        command.Transaction = Transaction;

        TRecordCacheCollection Records;

        using ( IDataReader R = command.ExecuteReader() ) {
          Records = new TRecordCacheCollection(R);
          R.Close();
        }
        while ( Records.Read() != null ) {
          RetVal.Add(mapMethod(Records));
        }

      } catch ( Exception ex ) {
        Trace.WriteLine($"Error during select : {command.ToString()} : {ex.Message}");
      } finally {
        if ( LocalTransaction ) {
          TryClose();
        }
      }
      return RetVal;
    }
    #endregion --- Select multiple records --------------------------------------------

    #region --- Select single record --------------------------------------------
    public virtual T SelectQueryRecord<T>(string query, Func<IDataReader, T> mapMethod) where T : new() {
      #region Validate parameters
      if ( string.IsNullOrWhiteSpace(query) ) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return default(T);
      }
      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return default(T);
      }
      #endregion Validate parameters
      return SelectQueryRecord<T>(new SqlCommand(query), mapMethod);
    }

    public virtual T SelectQueryRecord<T>(IDbCommand command, Func<IDataReader, T> mapMethod) where T : new() {
      #region Validate parameters
      if ( command == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return default(T);
      }
      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return default(T);
      }
      #endregion Validate parameters

      NotifySelectQuery(command);

      T RetVal = default(T);
      bool LocalTransaction = false;
      try {
        if ( !IsOpened ) {
          TryOpen();
          LocalTransaction = true;
        }
        command.Connection = Connection;
        command.Transaction = Transaction;
        using ( IDataReader R = command.ExecuteReader() ) {
          R.Read();
          RetVal = mapMethod(R);
          R.Close();
        }
      } catch ( Exception ex ) {
        Trace.WriteLine(string.Format("Error during select : {0} : {1}", command.CommandText, ex.Message));
      } finally {
        if ( LocalTransaction ) {
          TryClose();
        }
      }
      return RetVal;
    }
    #endregion --- Select single record --------------------------------------------

    #region --- Select single value --------------------------------------------
    public virtual T SelectQueryValue<T>(string query, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if ( string.IsNullOrWhiteSpace(query) ) {
        Trace.WriteLine("Unable to execute a Select with a null or empty query string");
        return default(T);
      }

      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return default(T);
      }
      #endregion Validate parameters
      return SelectQueryValue<T>(new SqlCommand(query), mapMethod);
    }
    public virtual T SelectQueryValue<T>(IDbCommand command, Func<IDataReader, T> mapMethod) {
      #region Validate parameters
      if ( command == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return default(T);
      }

      if ( mapMethod == null ) {
        Trace.WriteLine("Missing mapMethod");
        return default(T);
      }
      #endregion Validate parameters

      NotifySelectQuery(command);

      T RetVal = default(T);
      bool LocalTransaction = false;
      try {
        if ( !IsOpened ) {
          TryOpen();
          LocalTransaction = true;
        }
        command.Connection = Connection;
        command.Transaction = Transaction;
        using ( IDataReader R = command.ExecuteReader() ) {
          R.Read();
          RetVal = mapMethod(R);
          R.Close();
        }
      } catch ( Exception ex ) {
        Trace.WriteLine($"Error during select : {command.CommandText} : {ex.Message}");
      } finally {
        if ( LocalTransaction ) {
          TryClose();
        }
      }
      return RetVal;
    }
    #endregion --- Select single value --------------------------------------------

    public virtual bool ExecuteNonQuery(SqlTransaction transaction, params string[] sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException("sqlCommands");
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException("sqlCommands");
      }
      if ( transaction == null ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to use a null transaction");
        throw new ArgumentNullException("transaction");
      }
      #endregion Validate parameters
      return ExecuteNonQuery(transaction, sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual bool ExecuteNonQuery(SqlTransaction transaction, IEnumerable<string> sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException("sqlCommands");
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException("sqlCommands");
      }
      if ( transaction == null ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to use a null transaction");
        throw new ArgumentNullException("transaction");
      }
      #endregion Validate parameters
      return ExecuteNonQuery(transaction, sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual bool ExecuteNonQuery(IDbTransaction transaction, params IDbCommand[] sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        return false;
      }
      if ( transaction == null ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to use a null transaction");
        throw new ArgumentNullException("transaction");
      }
      #endregion Validate parameters
      return ExecuteNonQuery(transaction, sqlCommands);
    }
    public virtual bool ExecuteNonQuery(IDbTransaction transaction, IEnumerable<IDbCommand> commands) {
      #region Validate parameters
      if ( commands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException(nameof(commands));
      }
      if ( commands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException(nameof(commands));
      }
      if ( transaction == null ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to use a null transaction");
        throw new ArgumentNullException(nameof(transaction));
      }
      #endregion Validate parameters

      NotifySelectNonQuery(commands);

      StringBuilder Status = new StringBuilder("Execute non query commands : ");

      try {
        Trace.Indent();

        Status.AppendLine(string.Format("{0} command(s) => ", commands.Count()));

        try {

          foreach ( IDbCommand SqlCommandItem in commands ) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = transaction;
            SqlCommandItem.ExecuteNonQuery();
          }

          Status.Append("successfull");
          transaction.Commit();

        } catch ( Exception ex ) {
          Status.AppendFormat("failed : {0}", ex.Message);
          transaction.Rollback();
          return false;
        }

        return true;
      } finally {
        Trace.Unindent();
        Trace.WriteLine(Status.ToString());
      }
    }

    public virtual bool ExecuteNonQuery(params string[] sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException("sqlCommands");
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException("sqlCommands");
      }
      #endregion Validate parameters
      return ExecuteNonQuery(sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual bool ExecuteNonQuery(IEnumerable<string> sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException("sqlCommands");
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException("sqlCommands");
      }
      #endregion Validate parameters
      return ExecuteNonQuery(sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual bool ExecuteNonQuery(params IDbCommand[] sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException("sqlCommands");
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException("sqlCommands");
      }
      #endregion Validate parameters
      return ExecuteNonQuery((IEnumerable<SqlCommand>)sqlCommands);
    }

    public virtual bool ExecuteNonQuery(IEnumerable<IDbCommand> sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException("sqlCommands");
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException("sqlCommands");
      }
      #endregion Validate parameters

      NotifySelectNonQuery(sqlCommands);

      StringBuilder Status = new StringBuilder("Execute non query commands : ");

      try {
        Trace.Indent();

        #region Validate parameters
        if ( sqlCommands == null ) {
          Trace.WriteLine("Unable to create a record from a null SqlCommand");
          return false;
        }
        if ( sqlCommands.Count() == 0 ) {
          Trace.WriteLine("Unable to create a record from an empty query list");
          return false;
        }
        #endregion Validate parameters

        Status.Append($"{sqlCommands.Count()} command(s)\r\n=> ");

        try {

          TryOpen();
          StartTransaction();

          foreach ( IDbCommand SqlCommandItem in sqlCommands ) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = Transaction;
            if ( SqlCommandItem.ExecuteNonQuery() == 0 ) {
              RollbackTransaction();
              Status.AppendFormat("failed : {0}", SqlCommandItem.CommandText);
              return false;
            }
          }

          CommitTransaction();
          Status.Append("successfull");

        } catch ( Exception ex ) {
          Status.AppendFormat("failed : {0}", ex.Message);
          RollbackTransaction();
          return false;

        } finally {
          TryClose();
        }
        return true;
      } finally {
        Trace.Unindent();
        Trace.WriteLine(Status.ToString());
      }
    }

    #endregion Records management

  }
}
