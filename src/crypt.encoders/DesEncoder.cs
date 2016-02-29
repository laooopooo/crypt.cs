/// @file
/// Implementation of the `Crypt.Encoders.DesEncoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Security.Cryptography;
  using System.Text;

  using Crypt.Encoders.Properties;
  
  /// Represents the DES encoding method.
  public class DesEncoder: IStringEncoder {

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.DesDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "DES"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      var buffer = Encoding.Default.GetBytes(text);
      var hash = DES.Create().CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
      return Convert.ToBase64String(hash);
    }
  }
}