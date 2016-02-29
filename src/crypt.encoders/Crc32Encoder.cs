/// @file
/// Implementation of the `Crypt.Encoders.Crc32Encoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Globalization;
  using System.Text;

  using Crypt.Encoders.Internal;
  using Crypt.Encoders.Properties;

  /// Represents the CRC32 encoding method.
  public class Crc32Encoder: IStringEncoder {

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
      var buffer = Encoding.Default.GetBytes(text);
      var hash = Crc32.Create().ComputeHash(buffer);
      return BitConverter.ToUInt32(hash, 0).ToString(CultureInfo.InvariantCulture);
    }
  }
}