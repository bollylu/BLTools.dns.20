using System;
using System.Security.Cryptography;
using System.Text;

namespace BLTools.Encryption {


  public static class StringExtensionsBeforeNet5 {

#if NETSTANDARD2_1 || NETSTANDARD2_0
    public static string HashToBase64(this string source, EHashingMethods hashMethod = EHashingMethods.SHA256) {

      byte[] SourceData = Encoding.UTF8.GetBytes(source);
      byte[] HashedData;

      switch (hashMethod) {

        case EHashingMethods.MD5:
          using (MD5CryptoServiceProvider MD5Hasher = new MD5CryptoServiceProvider()) {
            HashedData = MD5Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA1:
          using (SHA1CryptoServiceProvider SHA1Hasher = new SHA1CryptoServiceProvider()) {
            HashedData = SHA1Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA256:
          using (SHA256CryptoServiceProvider SHA256Hasher = new SHA256CryptoServiceProvider()) {
            HashedData = SHA256Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA384:
          using (SHA384CryptoServiceProvider SHA384Hasher = new SHA384CryptoServiceProvider()) {
           HashedData = SHA384Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA512:
          using (SHA512CryptoServiceProvider SHA512Hasher = new SHA512CryptoServiceProvider()) {
            HashedData = SHA512Hasher.ComputeHash(SourceData);
          }
          break;

        default:
          return "";

      }

      return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);

    }
    public static bool VerifyHashFromBase64(this string source, string base64Hash, EHashingMethods hashMethod = EHashingMethods.SHA256) {
      string HashToTest = source.HashToBase64(hashMethod);
      return (HashToTest == base64Hash);
    }
#endif

    public static string HMacToBase64(this string source, string key, EHashingMethods hashMethod = EHashingMethods.SHA256) {
      return source.HMacToBase64(Encoding.UTF8.GetBytes(key), hashMethod);
    }
    public static string HMacToBase64(this string source, byte[] key, EHashingMethods hashMethod = EHashingMethods.SHA256) {

      byte[] SourceData = Encoding.UTF8.GetBytes(source);
      byte[] HashedData;

      switch (hashMethod) {

        case EHashingMethods.MD5:
          using (HMACMD5 HMACMD5Hasher = new HMACMD5(key)) {
            HashedData = HMACMD5Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA1:
          using (HMACSHA1 HMACSHA1Hasher = new HMACSHA1(key)) {
            HashedData = HMACSHA1Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA256:
          using (HMACSHA256 HMACSHA256Hasher = new HMACSHA256(key)) {
            HashedData = HMACSHA256Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA384:
          using (HMACSHA384 HMACSHA384Hasher = new HMACSHA384(key)) {
            HashedData = HMACSHA384Hasher.ComputeHash(SourceData);
          }
          break;

        case EHashingMethods.SHA512:
          using (HMACSHA512 HMACSHA512Hasher = new HMACSHA512(key)) {
            HashedData = HMACSHA512Hasher.ComputeHash(SourceData);
          }
          break;

        default:
          return "";
      }

      return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);

    }

    public static bool VerifyHMACFromBase64(this string source, string key, string base64Hash, EHashingMethods hashMethod = EHashingMethods.SHA256) {
      string HashToTest = source.HMacToBase64(key, hashMethod);
      return (HashToTest == base64Hash);
    }
    public static bool VerifyHMACFromBase64(this string source, byte[] key, string base64Hash, EHashingMethods hashMethod = EHashingMethods.SHA256) {
      string HashToTest = source.HMacToBase64(key, hashMethod);
      return (HashToTest == base64Hash);
    }
    

  }


}

