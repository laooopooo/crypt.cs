/// @file
/// Implementation of the `Crypt.Encoders.UnixCryptEncoder` class.

namespace Crypt.Encoders {
  using Crypt.Encoders.Properties;
  using DigiWar.Security.Cryptography;

  /// Represents the UNIXCrypt encoding method.
  public class UnixCryptEncoder: IStringEncoder {

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.UnixCryptDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "UNIXCrypt"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return UnixCrypt.Crypt(text);
    }
  }
}