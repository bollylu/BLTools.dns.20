using System;
using System.Security.Cryptography;
using System.Text;

namespace BLTools.Encryption {

#if NET5_0_OR_GREATER
  /// <summary>
  /// Extension methods for calculation of hashes
  /// </summary>
  public static class StringExtensionsNet5AndUp {

    /// <summary>
    /// Calculate the hash of a given string
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="hashMethod">The security method to calculate the hash</param>
    /// <returns>A base64 string containing the hash value</returns>
    public static string HashToBase64(this string source, EHashingMethods hashMethod = EHashingMethods.SHA256) {

      byte[] SourceData = Encoding.UTF8.GetBytes(source);
      byte[] HashedData = hashMethod switch {
        EHashingMethods.MD5 => MD5.HashData(SourceData),
        EHashingMethods.SHA1 => SHA1.HashData(SourceData),
        EHashingMethods.SHA256 => SHA256.HashData(SourceData),
        EHashingMethods.SHA384 => SHA384.HashData(SourceData),
        EHashingMethods.SHA512 => SHA512.HashData(SourceData),
        _ => throw new ApplicationException()
      };

      return Convert.ToBase64String(HashedData, Base64FormattingOptions.InsertLineBreaks);
    }

    /// <summary>
    /// Check the hash of a string against a known hash
    /// </summary>
    /// <param name="source">The source string to verify the hash</param>
    /// <param name="base64Hash">The known hash</param>
    /// <param name="hashMethod">The security method to calculate the hash</param>
    /// <returns>true if both hashes match, false otherwise</returns>
    public static bool VerifyHashFromBase64(this string source, string base64Hash, EHashingMethods hashMethod = EHashingMethods.SHA256) {
      string HashToTest = source.HashToBase64(hashMethod);
      return HashToTest == base64Hash;
    }

  }
#endif

}
