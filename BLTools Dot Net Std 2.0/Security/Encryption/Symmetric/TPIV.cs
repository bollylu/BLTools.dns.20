using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Encryption {
  public class TPIV {
    public byte[] Password;
    public byte[] IV;

    private const int DEFAULT_KEY_SIZE = 16;
    private const int DEFAULT_IV_SIZE = 16;

    #region Constructor(s)
    public TPIV(byte[] password, byte[] iv, int passwordLength = DEFAULT_KEY_SIZE) {
      if ( password == null ) {
        throw new ArgumentNullException(nameof(password));
      }
      List<byte> TempPassword = new List<byte>(password);
      while ( TempPassword.Count < passwordLength ) {
        TempPassword.Add(0);
      }
      Password = TempPassword.Take(passwordLength).ToArray();
      IV = iv;
    }
    public TPIV(string password, string iv, int passwordLength = DEFAULT_KEY_SIZE) {
      if ( password == null ) {
        throw new ArgumentNullException(nameof(password));
      }
      StringBuilder Pwd = new StringBuilder(password);
      while ( Pwd.Length < passwordLength ) {
        Pwd.Append(0);
      }
      Password = Encoding.UTF8.GetBytes(Pwd.ToString()).Take(passwordLength).ToArray();
      IV = Encoding.UTF8.GetBytes(iv);
    }
    public TPIV(string password, string iv, Encoding encoding, int passwordLength = DEFAULT_KEY_SIZE) {
      if ( password == null ) {
        throw new ArgumentNullException(nameof(password));
      }
      StringBuilder Pwd = new StringBuilder(password);
      while ( Pwd.Length < passwordLength ) {
        Pwd.Append(0);
      }
      Password = encoding.GetBytes(Pwd.ToString()).Take(passwordLength).ToArray();
      IV = encoding.GetBytes(iv);
    }

    public static TPIV Generate(string password, int requestedPasswordLength = DEFAULT_KEY_SIZE, int requestedIVLength = DEFAULT_IV_SIZE) {
      Rfc2898DeriveBytes DerivedBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password.Reverse().ToString()), 128);
      TPIV RetVal = new TPIV(DerivedBytes.GetBytes(requestedPasswordLength), DerivedBytes.GetBytes(requestedIVLength), requestedPasswordLength);
      return RetVal;
    }
    #endregion Constructor(s)
  }
}
