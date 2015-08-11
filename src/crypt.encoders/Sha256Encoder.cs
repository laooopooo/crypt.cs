/// @file
/// Implementation of the `Crypt.Encoders.Sha256Encoder` class.

namespace Crypt.Encoders {
  using MiniFramework.Security.Cryptography;
  using Properties;

  /// Represents the SHA-256 encoding method.
  public class Sha256Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public Sha256Encoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.Sha256Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "SHA-256"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return HashUtility.ComputeSha256(text);
    }
  }
}