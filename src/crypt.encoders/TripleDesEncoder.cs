/// @file
/// Implementation of the `Crypt.Encoders.TripleDesEncoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Security.Cryptography;
  using System.Text;

  using Properties;
  
  /// Represents the TripleDES encoding method.
  public class TripleDesEncoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public TripleDesEncoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.TripleDesDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "TripleDES"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      var bytes = Encoding.Default.GetBytes(text);
      return Convert.ToBase64String(TripleDES.Create().CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
    }
  }
}