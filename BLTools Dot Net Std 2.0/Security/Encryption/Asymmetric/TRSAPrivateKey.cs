using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BLTools.Encryption {

  /// <summary>
  /// Implementation of a RSA private key
  /// </summary>
  public class TRSAPrivateKey : ARsaKey {

    #region Constructor(s)
    /// <summary>
    /// Create a new empty private key
    /// </summary>
    public TRSAPrivateKey() : base() {
    }

    /// <summary>
    /// Create a new named private key container
    /// </summary>
    /// <param name="name">The name of the private key</param>
    /// <param name="storagePath">The path to store the key</param>
    public TRSAPrivateKey(string name, string storagePath = "") : base(name) {
      StoragePath = storagePath;
      Filename = Path.Combine(storagePath, $"{Name}-pvt.blkey");
    }

    /// <summary>
    /// Create a new named private key with parameters
    /// </summary>
    /// <param name="name">The name of the key</param>
    /// <param name="parameters">The parameters for the key</param>
    /// <param name="storagePath">The path to store the key</param>
    public TRSAPrivateKey(string name, RSAParameters parameters, string storagePath = "") : base(name, parameters) {
      StoragePath = storagePath;
      Filename = Path.Combine(storagePath, $"{Name}-pvt.blkey");
    }
    #endregion Constructor(s)

    #region Converters
    /// <inheritdoc/>
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append("Private Key :");
      RetVal.Append(base.ToString());
      return RetVal.ToString();
    }

    #endregion Converters

    #region Public methods
    /// <inheritdoc/>
    public override bool Save(bool overwrite = true) {
      //Root = ToXml();

      try {
        base.Save(Path.Combine(StoragePath, Filename));
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving private key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }

    /// <inheritdoc/>
    public override bool Save(string filename, bool overwrite = true) {
      //Root = ToXml();

      StoragePath = Path.GetDirectoryName(filename);
      Filename = Path.GetFileName(filename);

      try {
        base.Save(Path.Combine(StoragePath, Filename));
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving private key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }

    /// <inheritdoc/>
    public override XElement Load() {

      try {
        Root = base.Load(Path.Combine(StoragePath, Filename));
        FromXml(Root);
        return Root;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading private key : {0}", ex.Message), Severity.Error);
        return null;
      }
    }

    /// <inheritdoc/>
    public override XElement Load(string filename) {
      Filename = filename;
      try {
        Root = base.Load(filename);
        FromXml(Root);
        return Root;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading private key : {0}", ex.Message), Severity.Error);
        return null;
      }
    }
    #endregion Public methods

    #region Public static methods
    public static TRSAPrivateKey Load(string name, string pathname) {
      TRSAPrivateKey RetVal = new TRSAPrivateKey(name);
      RetVal.Load(pathname);
      return RetVal;
    }
    #endregion Public static methods
  }

}
