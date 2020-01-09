using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace BLTools.Encryption {
  public class TRSAPrivateKey : ARSAKey {

    #region Public properties
    public override string Filename {
      get {
        return $"{Name}-pvt.blkey";
      }
    }
    #endregion Public properties

    #region Constructor(s)
    public TRSAPrivateKey() : base() {
    }

    public TRSAPrivateKey(string name) : base(name) {
    }

    public TRSAPrivateKey(string name, RSAParameters parameters) : base(name, parameters) {
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append("Private Key :");
      RetVal.Append(base.ToString());
      return RetVal.ToString();
    }

    #endregion Converters

    #region Public methods
    public override void Save(string pathname) {

      try {
        base.Save(pathname);
      } catch ( Exception ex ) {
        Trace.WriteLine(string.Format("Error while saving private key : {0}", ex.Message), Severity.Error);
      }
    }

    public override void Load(string pathname) {
      try {
        base.Load(pathname);
      } catch ( Exception ex ) {
        Trace.WriteLine(string.Format("Error while reading private key : {0}", ex.Message), Severity.Error);
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
