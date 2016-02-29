/// @file
/// Implementation of the `Crypt.Encoders.Sha224Encoder` class.

namespace Crypt.Encoders {
  using System.Text;
  
  using MiniFramework.Text;
  using Mono.Security.Cryptography;
  using Properties;

  /// Represents the SHA-224 encoding method.
  public class Sha224Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public Sha224Encoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.Sha224Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "SHA-224"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      var buffer = Encoding.Default.GetBytes(text);
      var hash = SHA224.Create().ComputeHash(buffer);
      return HexCodec.GetString(hash);
    }
  }
}