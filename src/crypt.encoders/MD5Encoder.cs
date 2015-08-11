/// @file
/// Implementation of the `Crypt.Encoders.MD5Encoder` class.

namespace Crypt.Encoders {
  using MiniFramework.Security.Cryptography;
  using Properties;

  /// Represents the MD5 encoding method.
  public class MD5Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public MD5Encoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.MD5Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "MD5"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return HashUtility.ComputeMD5(text);
    }
  }
}