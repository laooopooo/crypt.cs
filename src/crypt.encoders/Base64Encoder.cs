/// @file
/// Implementation of the `Crypt.Encoders.Base64Encoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Text;

  using Crypt.Encoders.Properties;
  
  /// Represents the Base64 encoding method.
  public class Base64Encoder: IStringEncoder {

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.Base64Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "Base64"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      var buffer = Encoding.Default.GetBytes(text);
      return Convert.ToBase64String(buffer);
    }
  }
}