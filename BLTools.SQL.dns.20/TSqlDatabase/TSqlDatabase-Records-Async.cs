using BLTools.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Records management async

    #region --- Select multiple records --------------------------------------------
    public virtual async Task<IEnumerable<T>> SelectQueryAsync<T>(string query, Func<IDataReader, T> mapMethod) {
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
      return await SelectQueryAsync<T>(new SqlCommand(query), mapMethod);
    }

    public virtual async Task<IEnumerable<T>> SelectQueryAsync<T>(SqlCommand command, Func<IDataReader, T> mapMethod) {
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
        using ( IDataReader R = await command.ExecuteReaderAsync() ) {
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

    public virtual async Task<IEnumerable<T>> SelectQueryAsync<T>(SqlCommand command, Func<TRecordCacheCollection, T> mapMethod) {

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

        using ( IDataReader R = await command.ExecuteReaderAsync() ) {
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
    public virtual async Task<T> SelectQueryRecordAsync<T>(string query, Func<IDataReader, T> mapMethod) where T : new() {
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
      return await SelectQueryRecordAsync<T>(new SqlCommand(query), mapMethod);
    }

    public virtual async Task<T> SelectQueryRecordAsync<T>(SqlCommand command, Func<IDataReader, T> mapMethod) where T : new() {
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
        using ( IDataReader R = await command.ExecuteReaderAsync() ) {
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
    public virtual async Task<T> SelectQueryValueAsync<T>(string query, Func<IDataReader, T> mapMethod) {
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
      return await SelectQueryValueAsync<T>(new SqlCommand(query), mapMethod);
    }

    public virtual async Task<T> SelectQueryValueAsync<T>(SqlCommand command, Func<IDataReader, T> mapMethod) {
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
        using ( IDataReader R = await command.ExecuteReaderAsync() ) {
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
    #endregion --- Select single value --------------------------------------------

    #region --- Execute non query commands -------------------------------------------
    public virtual async Task<bool> ExecuteNonQueryAsync(SqlTransaction transaction, params string[] sqlCommands) {
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
      return await ExecuteNonQueryAsync(transaction, sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(SqlTransaction transaction, IEnumerable<string> sqlCommands) {
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
      return await ExecuteNonQueryAsync(transaction, sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(SqlTransaction transaction, params SqlCommand[] sqlCommands) {
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
      return await ExecuteNonQueryAsync(transaction, (IEnumerable<SqlCommand>)sqlCommands);
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(SqlTransaction transaction, IEnumerable<SqlCommand> sqlCommands) {
      #region Validate parameters
      if ( sqlCommands == null ) {
        Trace.WriteLine("Unable to execute a Select with a null command");
        throw new ArgumentNullException(nameof(sqlCommands));
      }
      if ( sqlCommands.Count() == 0 ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to execute command from an empty query list");
        throw new ArgumentOutOfRangeException(nameof(sqlCommands));
      }
      if ( transaction == null ) {
        Trace.WriteLine("ExecuteNonQuery : Unable to use a null transaction");
        throw new ArgumentNullException(nameof(transaction));
      }
      #endregion Validate parameters

      NotifySelectNonQuery(sqlCommands);

      StringBuilder Status = new StringBuilder("Execute non query commands : ");

      try {
        Trace.Indent();

        Status.AppendLine($"{sqlCommands.Count()} command(s) => ");

        try {

          foreach ( SqlCommand SqlCommandItem in sqlCommands ) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = transaction;
            await SqlCommandItem.ExecuteNonQueryAsync();
          }

          Status.Append("successfull");
          transaction.Commit();

        } catch ( Exception ex ) {
          Status.Append($"failed : {ex.Message}");
          transaction.Rollback();
          return false;
        }

        return true;
      } finally {
        Trace.Unindent();
        Trace.WriteLine(Status.ToString());
      }
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(params string[] sqlCommands) {
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
      return await ExecuteNonQueryAsync(sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(IEnumerable<string> sqlCommands) {
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
      return await ExecuteNonQueryAsync(sqlCommands.Select(x => new SqlCommand(x)));
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(params SqlCommand[] sqlCommands) {
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
      return await ExecuteNonQueryAsync((IEnumerable<SqlCommand>)sqlCommands);
    }

    public virtual async Task<bool> ExecuteNonQueryAsync(IEnumerable<SqlCommand> sqlCommands) {

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

        Status.Append($"{sqlCommands.Count()} command(s)\r\n=> ");

        try {

          TryOpen();
          StartTransaction();

          foreach ( SqlCommand SqlCommandItem in sqlCommands ) {
            SqlCommandItem.Connection = Connection;
            SqlCommandItem.Transaction = Transaction;
            if ( await SqlCommandItem.ExecuteNonQueryAsync() == 0 ) {
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
    #endregion --- Execute non query commands --------------------------------------------

    #endregion Records management

  }
}
