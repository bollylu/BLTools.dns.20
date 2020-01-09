using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.Encryption;
using System.Diagnostics;
using System.Text;

namespace BLTools.UnitTest.Core20.Security {
  [TestClass]
  public class TSymmetricEncryptionAesTest {
    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersOk128_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 128);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.AES, 128);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersOk192_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 192);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.AES, 192);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersOk256_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_SourceEmpty_EncryptionDecryptionOK() {
      string SourceText = "";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }
    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricAes_BadKeyLengthTooSmall_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 253);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricAes_BadKeyLengthZero_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 0);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestEncryptSymmetricAes_BadKeyLengthTooBig_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 1024);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_NoPassword_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricAes_SourceTextIsNull_Exception() {
      string SourceText = null;
      string Password = "";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestEncryptSymmetricAes_NullPassword_Exception() {
      string SourceText = "Je vais bien, merci.";
      string Password = null;
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_WrongPassword_DecryptionFailed() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecryptPassword = "az12df34vc";
      string DecipheredText = EncryptedBase64.DecryptFromBase64(DecryptPassword, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.IsNull(DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_WrongPassword2_DecryptionFailed() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecryptPassword = "";
      string DecipheredText = EncryptedBase64.DecryptFromBase64(DecryptPassword, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.IsNull(DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersOKEncodingUTF8_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.UTF8, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersOKWrongEncoding_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.UTF8, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersASCIIEncodingASCII_EncryptionDecryptionOK() {
      string SourceText = "Je vais bien, merci. Celebration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreEqual(SourceText, DecipheredText);
    }

    [TestCategory("NC20.SymmetricEncryption"), TestMethod, TestCategory("NC20.AES")]
    public void TestEncryptSymmetricAes_ParametersASCII_Accents_EncodingASCII_DecryptionFailed() {
      string SourceText = "Je vais bien, merci. Célébration.";
      string Password = "az12df34vb";
      string EncryptedBase64 = SourceText.EncryptToBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.AES, 256);
      string DecipheredText = EncryptedBase64.DecryptFromBase64(Password, Encoding.ASCII, ESymmetricEncryptionAlgorithm.AES, 256);
      Assert.AreNotEqual(SourceText, DecipheredText);
    }
  }
}
