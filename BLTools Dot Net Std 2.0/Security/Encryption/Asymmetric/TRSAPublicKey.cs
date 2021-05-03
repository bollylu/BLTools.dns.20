using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace BLTools.Encryption {
  /// <summary>
  /// Implementation of a RSA public key
  /// </summary>
  public class TRSAPublicKey : ARsaKey {

    #region Constructor(s)
    /// <summary>
    /// Create a new empty public key
    /// </summary>
    public TRSAPublicKey() : base() {
    }

    /// <summary>
    /// Create a new named public key
    /// </summary>
    /// <param name="name">The name of the key</param>
    /// <param name="storagePath">The path to store the key</param>
    public TRSAPublicKey(string name, string storagePath = "") : base(name) {
      StoragePath = storagePath;
      Filename = $"{Name}-pub.blkey";
    }

    /// <summary>
    /// Create a new named public key with parameters
    /// </summary>
    /// <param name="name">The name of the key</param>
    /// <param name="parameters">The parameters for the key</param>
    /// <param name="storagePath">The path to store the key</param>
    public TRSAPublicKey(string name, RSAParameters parameters, string storagePath = "") : base(name, parameters) {
      StoragePath = storagePath;
      Filename = $"{Name}-pub.blkey";
    }
    #endregion Constructor(s)

    #region Converters
    /// <inheritdoc/>
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append("Public Key :");
      RetVal.Append($" Name = {Name}");
      RetVal.Append($", {base.ToString()}");
      return RetVal.ToString();
    }

    #endregion Converters

    #region Public methods
    /// <inheritdoc/>
    public override bool Save(bool overwrite = true) {

      try {
        base.Save(Path.Combine(StoragePath, Filename));
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }

    /// <inheritdoc/>
    public override bool Save(string filename, bool overwrite = true) {

      StoragePath = Path.GetDirectoryName(filename);
      Filename = Path.GetFileName(filename);

      try {
        base.Save(Path.Combine(StoragePath, Filename));
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
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
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
        return null;
      }
    }

    /// <inheritdoc/>
    public override XElement Load(string filename) {

      StoragePath = Path.GetDirectoryName(filename);
      Filename = Path.GetFileName(filename);

      try {
        Root = base.Load(Path.Combine(StoragePath, Filename));
        FromXml(Root);
        return Root;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
        return null;
      }
    }
    #endregion Public methods

    #region Public static methods
    public static TRSAPublicKey Load(string name, string pathname) {
      TRSAPublicKey RetVal = new TRSAPublicKey(name);
      RetVal.Load(pathname);
      return RetVal;
    }
    #endregion Public static methods
  }
}
