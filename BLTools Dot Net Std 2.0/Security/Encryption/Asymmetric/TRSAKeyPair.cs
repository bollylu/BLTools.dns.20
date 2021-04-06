﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace BLTools.Encryption {
  public class TRSAKeyPair : IToXml {

    public enum EKeyType {
      Unknown,
      Random
    }

    #region Public properties
    public string Name { get; set; }
    public EKeyType KeyType { get; set; }
    public int KeyLength { get; set; }
    public TRSAPrivateKey PrivateKey { get; } = new TRSAPrivateKey();
    public TRSAPublicKey PublicKey { get; } = new TRSAPublicKey();
    #endregion Public properties

    #region Constructor(s)
    public TRSAKeyPair() {
      Name = "";
      KeyType = EKeyType.Unknown;
      KeyLength = 1024;
    }

    public TRSAKeyPair(string keyname, int keyLength = 1024) {
      Name = keyname;
      KeyLength = keyLength;
      KeyType = EKeyType.Random;
      using (RSACryptoServiceProvider RSACSP = new RSACryptoServiceProvider(keyLength)) {
        RSAParameters TempParams = RSACSP.ExportParameters(true);
        PublicKey = new TRSAPublicKey(Name, TempParams);
        PrivateKey = new TRSAPrivateKey(Name, TempParams);
      }
    }

    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine(PublicKey.ToString());
      RetVal.AppendLine(PrivateKey.ToString());
      return RetVal.ToString();
    }

    #endregion Converters

    public void Save(string pathname) {
      PublicKey.Save(pathname);
      PrivateKey.Save(pathname);
    }

    public void Load(string pathname) {
      PublicKey.Load(pathname);
      PrivateKey.Load(pathname);
    }

    public static TRSAKeyPair Load(string keyname, string pathname) {
      TRSAKeyPair RetVal = new TRSAKeyPair(keyname);
      RetVal.Load(pathname);
      return RetVal;
    }

    #region --- IToXml --------------------------------------------
    public XElement ToXml() {
      XElement RetVal = new XElement("RSAKeyPair");
      RetVal.Add(PrivateKey.ToXml());
      RetVal.Add(PublicKey.ToXml());
      return RetVal;
    }

    public void FromXml(XElement source) {
      if (source is null) {
        return;
      }

      PrivateKey.FromXml(source.Element(TRSAPrivateKey.XML_THIS_ELEMENT));
      PublicKey.FromXml(source.Element(TRSAPublicKey.XML_THIS_ELEMENT));

    }
    #endregion --- IToXml --------------------------------------------
  }
}
