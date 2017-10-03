using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;

namespace BLTools.UnitTest.FW47.Security {
  [TestClass]
  public class THashMacExtensionStringTest {
    [TestMethod(), TestCategory("FW47.Security"), TestCategory("FW47.Hash"), TestCategory("FW47.MD5")]
    public void TestHMACMD5_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.MD5);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.MD5));
    }

    [TestMethod(), TestCategory("FW47.Security"), TestCategory("FW47.Hash"), TestCategory("FW47.SHA1")]
    public void TestHMACSHA1_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA1);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA1));
    }

    [TestMethod(), TestCategory("FW47.Security"), TestCategory("FW47.Hash"), TestCategory("FW47.SHA256")]
    public void TestHMACSHA256_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA256);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA256));
    }

    [TestMethod(), TestCategory("FW47.Security"), TestCategory("FW47.Hash"), TestCategory("FW47.SHA384")]
    public void TestHMACSHA384_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA384);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA384));
    }

    [TestMethod(), TestCategory("FW47.Security"), TestCategory("FW47.Hash"), TestCategory("FW47.SHA512")]
    public void TestHMACSHA512_StandardString_HMacIsOK() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA512);
      Assert.IsTrue(SourceString.VerifyHMACFromBase64(Key, Base64HMac, THashingMethods.SHA512));
    }

    [TestMethod(), TestCategory("FW47.Security"), TestCategory("FW47.Hash"), TestCategory("FW47.SHA256")]
    public void TestHMACSHA256_StandardStringWrongKeyForDecode_VerifyIsFalse() {
      string SourceString = "The quick brown fox jumps over the lazy dog";
      string Key = "key";
      string WrongKey = "anotherKey";
      string Base64HMac = SourceString.HMacToBase64(Key, THashingMethods.SHA256);
      Assert.IsFalse(SourceString.VerifyHMACFromBase64(WrongKey, Base64HMac, THashingMethods.SHA256));
    }
  }
}
