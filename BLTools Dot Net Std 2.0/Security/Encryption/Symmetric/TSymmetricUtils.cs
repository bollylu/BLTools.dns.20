using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Encryption {
  internal static class TSymmetricUtils {

    private static Dictionary<ESymmetricEncryptionAlgorithm, int[]> AllowedKeyLengths = new Dictionary<ESymmetricEncryptionAlgorithm, int[]>() {
        {ESymmetricEncryptionAlgorithm.AES, new int[] {128, 192, 256}}
      , {ESymmetricEncryptionAlgorithm.Rijndael, new int[] {128, 192, 256}}
      , {ESymmetricEncryptionAlgorithm.DES, new int[] {64}}
      , {ESymmetricEncryptionAlgorithm.TripleDES, new int[] {128, 192}}
      , {ESymmetricEncryptionAlgorithm.RC2, new int[] {40, 48, 56, 64, 72, 80, 88, 96, 104, 112, 128}}
    };

    internal static void CheckKeyLength(int keyLength, ESymmetricEncryptionAlgorithm encryptionMethod) {
      if (!AllowedKeyLengths[encryptionMethod].Contains(keyLength)) {
        string Msg = string.Format("Only {0} bits key length allowed for {1} : {2}", string.Join(",", AllowedKeyLengths[encryptionMethod]), encryptionMethod.ToString(), keyLength);
        Trace.WriteLine(Msg);
        throw new ArgumentException(Msg);
      }
    }

  }
}
