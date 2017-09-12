using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace BLTools.Encryption {
  public class TRSAPublicKey : TRSAKey {

    #region Public properties
    public override string Filename {
      get {
        return $"{Name}-pub.blkey";
      }
    } 
    #endregion Public properties

    #region Constructor(s)
    public TRSAPublicKey() : base() {
    }

    public TRSAPublicKey(string name) : base(name) {
    }

    public TRSAPublicKey(string name, RSAParameters parameters) : base(name, parameters) {
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append("Public Key :");
      RetVal.Append(base.ToString());
      return RetVal.ToString();
    }

    #endregion Converters

    #region Public methods
    public override void Save(string pathname) {
      
      try {
        base.Save(pathname);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
      }
    }

    public override void Load(string pathname) {
      try {
        base.Load(pathname);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
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
