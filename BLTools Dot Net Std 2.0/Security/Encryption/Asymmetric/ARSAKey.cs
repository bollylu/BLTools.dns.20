using System;
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

using BLTools.Storage.Xml;

namespace BLTools.Encryption {

  /// <summary>
  /// Abstract implementation of a RSA key (public or private)
  /// </summary>
  public abstract class ARsaKey : AFileXml, IToXml, IEquatable<ARsaKey> {

    #region --- XML constants ----------------------------------------------------------------------------------
    public const string XML_THIS_ELEMENT = "RsaKey";
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
    /// <summary>
    /// Name of the key
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Parameters for the key
    /// </summary>
    public RSAParameters Parameters { get; set; }

    /// <summary>
    /// Path to store the key
    /// </summary>
    public string StoragePath { get; set; } = "";
    #endregion Public properties

    #region Constructor(s)
    /// <summary>
    /// Create new empty RSA key
    /// </summary>
    protected ARsaKey() { }

    /// <summary>
    /// Create a new named RSA key
    /// </summary>
    /// <param name="name">The name of the key</param>
    protected ARsaKey(string name) {
      Name = name;
    }

    /// <summary>
    /// Create a new named RSA key with parameters
    /// </summary>
    /// <param name="name">The name of the key</param>
    /// <param name="parameters">The key parameters</param>
    protected ARsaKey(string name, RSAParameters parameters) {
      Name = name;
      Parameters = parameters;
    }
    #endregion Constructor(s)

    #region Converters
    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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


    #region --- RSA key I/O --------------------------------------------
    /// <inheritdoc/>
    public override bool Save(string filename, bool overwrite = true) {
      Root = ToXml();
      return base.Save(filename, overwrite);
    }

    /// <inheritdoc/>
    public override async Task<bool> SaveAsync(string filename, bool overwrite = true) {
      Root = ToXml();
      return await base.SaveAsync(filename, overwrite);
    }

    /// <inheritdoc/>
    public override XElement Load(string filename) {

      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        return null;
      }
      #endregion === Validate parameters ===

      XElement RetVal = null;

      try {
        XElement RootKey = base.Load(filename);
        RetVal = RootKey.Element(XML_THIS_ELEMENT);
        FromXml(RetVal);
        return RetVal;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
        return RetVal;
      }
    }

    /// <inheritdoc/>
    public override async Task<XElement> LoadAsync(string filename) {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        return null;
      }
      #endregion === Validate parameters ===

      XElement RetVal = null;

      try {
        XElement RootKey = await base.LoadAsync(filename).ConfigureAwait(false);
        RetVal = RootKey.Element(XML_THIS_ELEMENT);
        FromXml(RetVal);
        return RetVal;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading public key : {0}", ex.Message), Severity.Error);
        return RetVal;
      }
    }

    #endregion --- RSA key I/O --------------------------------------------


    /// <inheritdoc/>
    public bool Equals(ARsaKey other) {
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

    /// <inheritdoc/>
    public override bool Equals(object obj) {
      return base.Equals(obj as ARsaKey);
    }

    /// <inheritdoc/>
    public override int GetHashCode() {
      return Parameters.GetHashCode() + Name.GetHashCode();
    }
  }
}
