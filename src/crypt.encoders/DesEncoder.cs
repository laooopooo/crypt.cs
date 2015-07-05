/// @file
/// Implementation of the `Crypt.Encoders.DesEncoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Linq;
  using System.Security.Cryptography;
  using System.Text;

  using Crypt.Encoders.Properties;
  
  /// Represents the DES encoding method.
  public class DesEncoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public DesEncoder() {}

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
      var bytes=Encoding.Default.GetBytes(text);
      return Convert.ToBase64String(DES.Create().CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
    }
  }
}