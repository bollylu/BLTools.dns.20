﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BLTools.Encryption {
  public abstract class ARSAKey : IToXml, IFileXml, IEquatable<ARSAKey> {


    #region --- XML constants ----------------------------------------------------------------------------------
    public const string XML_THIS_ELEMENT = "Key";
    public const string XML_ATTRIBUTE_P = "P";
    public const string XML_ATTRIBUTE_Q = "Q";
    public const string XML_ATTRIBUTE_DP = "DP";
    public const string XML_ATTRIBUTE_DQ = "DQ";
    public const string XML_ATTRIBUTE_INVERSEQ = "InverseQ";
    public const string XML_ATTRIBUTE_D = "D";
    public const string XML_ATTRIBUTE_MODULUS = "Modulus";
    public const string XML_ATTRIBUTE_EXPONENT = "Exponent";
    #endregion --- XML constants -------------------------------------------------------------------------------

    #region Public properties
    public string Name { get; set; }
    public RSAParameters Parameters { get; set; }
    public abstract string Filename { get; }
    #endregion Public properties

    #region Constructor(s)
    public ARSAKey() {
      Name = "";
    }

    public ARSAKey(string name) {
      Name = name;
    }

    public ARSAKey(string name, RSAParameters parameters) {
      Name = name;
      Parameters = parameters;
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($" P({Parameters.P.ToHexString()})");
      RetVal.Append($" Q({Parameters.Q.ToHexString()})");
      RetVal.Append($" DP({Parameters.DP.ToHexString()})");
      RetVal.Append($" DQ({Parameters.DQ.ToHexString()})");
      RetVal.Append($" InverseQ({Parameters.InverseQ.ToHexString()})");
      RetVal.Append($" D({Parameters.D.ToHexString()})");
      RetVal.Append($" Modulus({Parameters.Modulus.ToHexString()})");
      RetVal.Append($" Exponent({Parameters.Exponent.ToHexString()})");
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(XML_THIS_ELEMENT);
      RetVal.SetAttributeValue(XML_ATTRIBUTE_P, Convert.ToBase64String(Parameters.P));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_Q, Convert.ToBase64String(Parameters.Q));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_DP, Convert.ToBase64String(Parameters.DP));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_DQ, Convert.ToBase64String(Parameters.DQ));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_INVERSEQ, Convert.ToBase64String(Parameters.InverseQ));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_D, Convert.ToBase64String(Parameters.D));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_MODULUS, Convert.ToBase64String(Parameters.Modulus));
      RetVal.SetAttributeValue(XML_ATTRIBUTE_EXPONENT, Convert.ToBase64String(Parameters.Exponent));
      return RetVal;
    }

    public void FromXml(XElement source) {
      if (source is null) {
        return;
      }

      Parameters = new RSAParameters() {
        P = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_P, "")),
        Q = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_Q, "")),
        DP = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_DP, "")),
        DQ = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_DQ, "")),
        InverseQ = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_INVERSEQ, "")),
        D = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_D, "")),
        Modulus = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_MODULUS, "")),
        Exponent = Convert.FromBase64String(source.SafeReadAttribute(XML_ATTRIBUTE_EXPONENT, "")),
      };
    }
    #endregion Converters

    #region Public methods

    [Obsolete("Use SaveXml() or SaveXmlAsync()")]
    public virtual void Save(string pathname) {
      string FullName = Path.Combine(pathname, Filename);
      try {
        XDocument XmlPublicKeyFile = new XDocument();
        XmlPublicKeyFile.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
        XmlPublicKeyFile.Add(new XElement("root"));
        XmlPublicKeyFile.Root.Add(this.ToXml());
        TextWriter XmlWriter = new StreamWriter(FullName, false, Encoding.UTF8);
        XmlPublicKeyFile.Save(XmlWriter, SaveOptions.None);
        XmlWriter.Close();
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
      }
    }

    [Obsolete("Use LoadXml() or LoadXmlAsync()")]
    public virtual void Load(string pathname) {
      string FullName = Path.Combine(pathname, Filename);
      try {
        XDocument XmlFile = XDocument.Load(FullName);
        XElement Key = XmlFile.Root.SafeReadElement(XML_THIS_ELEMENT);
        Parameters = new RSAParameters() {
          P = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_P, "")),
          Q = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_Q, "")),
          DP = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_DP, "")),
          DQ = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_DQ, "")),
          InverseQ = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_INVERSEQ, "")),
          D = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_D, "")),
          Modulus = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_MODULUS, "")),
          Exponent = Convert.FromBase64String(Key.SafeReadAttribute<string>(XML_ATTRIBUTE_EXPONENT, ""))
        };
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
      }
    }

    public bool Equals(ARSAKey other) {
      if (other == null) {
        return false;
      }

      if (Object.ReferenceEquals(this, other)) {
        return true;
      }

      if (!other.Parameters.P.SequenceEqual(Parameters.P)) {
        return false;
      }

      if (!other.Parameters.Q.SequenceEqual(Parameters.Q)) {
        return false;
      }
      if (!other.Parameters.DP.SequenceEqual(Parameters.DP)) {
        return false;
      }
      if (!other.Parameters.DQ.SequenceEqual(Parameters.DQ)) {
        return false;
      }
      if (!other.Parameters.D.SequenceEqual(Parameters.D)) {
        return false;
      }
      if (!other.Parameters.InverseQ.SequenceEqual(Parameters.InverseQ)) {
        return false;
      }
      if (!other.Parameters.Modulus.SequenceEqual(Parameters.Modulus)) {
        return false;
      }
      if (!other.Parameters.Exponent.SequenceEqual(Parameters.Exponent)) {
        return false;
      }

      return true;
    }

    public bool SaveXml(string filename, bool overwrite = true) {

      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        return false;
      }
      #endregion === Validate parameters ===

      try {
        XDocument XmlPublicKeyFile = new XDocument();
        XmlPublicKeyFile.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
        XmlPublicKeyFile.Add(new XElement("root"));
        XmlPublicKeyFile.Root.Add(this.ToXml());
        using (TextWriter XmlWriter = new StreamWriter(filename, false, Encoding.UTF8)) {
          XmlPublicKeyFile.Save(XmlWriter, SaveOptions.None);
          XmlWriter.Close();
        }
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }

    public async Task<bool> SaveXmlAsync(string filename, bool overwrite = true) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        return false;
      }
      #endregion === Validate parameters ===

      try {
        XDocument XmlPublicKeyFile = new XDocument();
        XmlPublicKeyFile.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
        XmlPublicKeyFile.Add(new XElement("root"));
        XmlPublicKeyFile.Root.Add(this.ToXml());
#if NETSTANDARD2_0
        using (TextWriter XmlWriter = new StreamWriter(filename, false, Encoding.UTF8)) {
          XmlPublicKeyFile.Save(XmlWriter, SaveOptions.None);
          await Task.Yield();
          XmlWriter.Close();
        }
        return true;
#else
        using (FileStream OutputStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
          await XmlPublicKeyFile.SaveAsync(OutputStream, SaveOptions.None, new CancellationTokenSource(1000).Token).ConfigureAwait(false);

        }
        return true;
#endif
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving public key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }

    /// <inheritdoc/>
    public bool LoadXml(string filename) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        return false;
      }
      #endregion === Validate parameters ===

      try {
        XDocument XmlFile = XDocument.Load(filename);
        XElement Key = XmlFile.Root.SafeReadElement(XML_THIS_ELEMENT);
        FromXml(Key);
        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }

    /// <inheritdoc/>
    public async Task<bool> LoadXmlAsync(string filename) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        return false;
      }
      #endregion === Validate parameters ===

      try {

#if NETSTANDARD2_0
        XDocument XmlFile = XDocument.Load(filename);
        await Task.Yield();
        XElement Key = XmlFile.Root.SafeReadElement(XML_THIS_ELEMENT);
        FromXml(Key);
#else
        using (FileStream InputStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          XDocument XmlFile = await XDocument.LoadAsync(InputStream, LoadOptions.None, new CancellationTokenSource(1000).Token).ConfigureAwait(false);
          XElement Key = XmlFile.Root.SafeReadElement(XML_THIS_ELEMENT);
          FromXml(Key);
        }
#endif

        return true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
        return false;
      }
    }


    #endregion Public methods


  }
}
