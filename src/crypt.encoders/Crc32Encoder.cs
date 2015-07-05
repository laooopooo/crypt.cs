/// @file
/// Implementation of the `Crypt.Encoders.Crc32Encoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Globalization;
  using System.Linq;
  
  using Crypt.Encoders.Properties;
  using MiniFramework.Security.Cryptography;
  
  /// Represents the CRC32 encoding method.
  public class Crc32Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public Crc32Encoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.Crc32Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "CRC32"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return HashUtility.ComputeCrc32(text).ToString(CultureInfo.InvariantCulture);
    }
  }
}