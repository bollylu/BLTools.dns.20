using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Connection string parameters
    /// <summary>
    /// The timeout when trying to connect to the database (in seconds)
    /// </summary>
    /// <remarks>Max value is MAX_CONNECTION_TIMEOUT. If 0, then DEFAULT_CONNECTION_TIMEOUT.</remarks>
    public virtual int ConnectionTimeout {
      get {
        return _ConnectionTimeout;
      }
      set {
        if ( value <= 0 ) {
          _ConnectionTimeout = DEFAULT_CONNECTION_TIMEOUT;
        } else {
          _ConnectionTimeout = Math.Min(value, MAX_CONNECTION_TIMEOUT);
        }
      }
    }
    private int _ConnectionTimeout;

    /// <summary>
    /// True to use the Pooled Connections
    /// </summary>
    public bool UsePooledConnections { get; set; }

    /// <summary>
    /// True to use MARS. False otherwise.
    /// </summary>
    /// <remarks>Only available with SQL 2005 and up.</remarks>
    public bool UseMars { get; set; }

    /// <summary>
    /// Current Database server name
    /// </summary>
    public virtual string ServerName {
      get {
        if ( string.IsNullOrEmpty(_ServerName) ) {
          return DEFAULT_SERVERNAME;
        } else {
          return _ServerName;
        }
      }
      set {
        _ServerName = value ?? "";
      }
    }
    private string _ServerName;

    /// <summary>
    /// current database name
    /// </summary>
    public virtual string DatabaseName {
      get {
        if ( string.IsNullOrEmpty(_DatabaseName) ) {
          return DEFAULT_DATABASENAME;
        } else {
          return _DatabaseName;
        }
      }
      set {
        _DatabaseName = value ?? "";
      }
    }
    private string _DatabaseName;

    /// <summary>
    /// Current user name (SQL identification)
    /// </summary>
    public virtual string UserName {
      get {

        return _UserName ?? "";
      }
      set {
        _UserName = value ?? "";
      }
    }
    private string _UserName;

    /// <summary>
    /// Current user password (SQL identification)
    /// </summary>
    public virtual string Password {
      get {
        return _Password ?? "";

      }
      set {
        _Password = value ?? "";
      }
    }
    private string _Password;

    /// <summary>
    /// True to use integrated Windows security (aka SSPI), false otherwise
    /// </summary>
    public virtual bool UseIntegratedSecurity {
      get {
        return _UseIntegratedSecurity;

      }
      set {
        _UseIntegratedSecurity = value;
      }
    }
    private bool _UseIntegratedSecurity;

    /// <summary>
    /// Obtain the connection string based on current properties of the object
    /// </summary>
    public virtual string ConnectionString {
      get {
        SqlConnectionStringBuilder ConnectionBuilder = new SqlConnectionStringBuilder();
        ConnectionBuilder.DataSource = ServerName;
        ConnectionBuilder.InitialCatalog = DatabaseName;
        ConnectionBuilder.ConnectTimeout = ConnectionTimeout;
        ConnectionBuilder.MultipleActiveResultSets = UseMars;
        ConnectionBuilder.Pooling = UsePooledConnections;
        ConnectionBuilder.IntegratedSecurity = UseIntegratedSecurity;
        ConnectionBuilder.UserID = UserName;
        ConnectionBuilder.Password = Password;
        return ConnectionBuilder.ToString();
      }
    }
    #endregion Connection string parameters

    #region Private methods
    private Dictionary<string, object> ParseConnectionString(string connectionString) {
      Dictionary<string, object> RetVal = new Dictionary<string, object>();
      string[] ConnectionStringComponents = connectionString.Split(';');

      foreach ( string ComponentItem in ConnectionStringComponents ) {
        try {
          string[] ComponentKeyValuePair = ComponentItem.Split('=');
          RetVal.Add(ComponentKeyValuePair[0].ToLower(), ComponentKeyValuePair[1]);
        } catch ( Exception ex ) {
          Trace.WriteLine($"Error while parsing connection string \"{connectionString}\" : {ex.Message}", Severity.Warning);
        }
      }

      return RetVal;
    }

    private void InitConnection(Dictionary<string, object> parsedConnectionString) {
      ServerName = parsedConnectionString.SafeGetValue(CS_SERVERNAME, DEFAULT_SERVERNAME);
      DatabaseName = parsedConnectionString.SafeGetValue(CS_DATABASENAME, DEFAULT_DATABASENAME);

      UseIntegratedSecurity = parsedConnectionString.SafeGetValue(CS_INTEGRATED_SECURITY, DEFAULT_USE_INTEGRATED_SECURITY);
      if ( !UseIntegratedSecurity ) {
        UserName = parsedConnectionString.SafeGetValue(CS_USERID, "");
        if ( UserName == "" ) {
          UserName = parsedConnectionString.SafeGetValue(CS_USERNAME, "");
        }
        Password = parsedConnectionString.SafeGetValue(CS_PASSWORD, "");
      }

      UseMars = parsedConnectionString.SafeGetValue(CS_USE_MARS, DEFAULT_USE_MARS);
      UsePooledConnections = DEFAULT_USE_POOLED_CONNECTIONS;

      if ( _IsConnectionStringInvalid() ) {
        if ( DebugMode ) {
          Trace.Write($"Missing information for database ConnectionString: {ConnectionString}", Severity.Warning);
        }
      }
    }

    private bool _IsConnectionStringInvalid() {
      if ( ServerName == "" || DatabaseName == "" ) {
        return true;
      } else {
        return false;
      }
    }

    private string _HidePasswordFromConnectionString(string connectionString) {
      string[] ConnectionStringItems = connectionString.Split(';');
      for ( int i = 0; i < ConnectionStringItems.Length; i++ ) {
        if ( ConnectionStringItems[i].StartsWith("Password=") ) {
          ConnectionStringItems[i] = "Password=xxx(hidden)xxx";
        }
      }
      return string.Join(";", ConnectionStringItems);
    }
    #endregion Private methods

  }
}
