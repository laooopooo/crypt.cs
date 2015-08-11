/// @file
/// Implementation of the `Crypt.Encoders.Sha512Encoder` class.

namespace Crypt.Encoders {
  using MiniFramework.Security.Cryptography;
  using Properties;

  /// Represents the SHA-512 encoding method.
  public class Sha512Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public Sha512Encoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.Sha512Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "SHA-512"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return HashUtility.ComputeSha512(text);
    }
  }
}