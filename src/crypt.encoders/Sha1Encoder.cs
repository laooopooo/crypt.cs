/// @file
/// Implementation of the `Crypt.Encoders.Sha1Encoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Linq;
  
  using Crypt.Encoders.Properties;
  using MiniFramework.Security.Cryptography;
  
  /// Represents the SHA-1 encoding method.
  public class Sha1Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public Sha1Encoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.Sha1Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "SHA-1"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return HashUtility.ComputeSha1(text);
    }
  }
}