using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;
using System.Diagnostics;
using System.Text;

namespace BLTools.UnitTest.Core20.Security {
  [TestClass]
  public class TSymmetricEncryptionDesTest {
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_ParametersOk64_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_SourceEmpty_EncryptionDecryptionOK() {
      string SourceText = "";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreEqual(SourceText, DecipheredText);
    }
    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricDes_BadKeyLengthTooSmall_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 63);
    }

    [TestMethod, TestCategory("DES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricDes_BadKeyLengthZero_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 0);
    }

    [TestMethod, TestCategory("DES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricDes_BadKeyLengthTooBig_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 1024);
    }

    [TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_NoPassword_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestMethod, TestCategory("DES")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricDes_SourceTextIsNull_Exception() {
      string SourceText = null;
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
    }

    [TestMethod, TestCategory("DES")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricDes_NullPassword_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = null;
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
    }

    [TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_WrongPassword_DecryptionFailed() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecryptPassword = "az12df34vc";
      string DecipheredText = EncryptedBase64.DecryptFromBase64(DecryptPassword, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.IsNull(DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_ParametersOKEncodingUTF8_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.UTF8, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_ParametersOKWrongEncoding_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_ParametersASCIIEncodingASCII_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci. Celebration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("SymmetricEncryption"), TestMethod, TestCategory("DES")]
    public void TestEncryptSymmetricDes_ParametersASCII_Accents_EncodingASCII_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.DES, 64);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.DES, 64);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }
  }
}
