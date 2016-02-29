/// @file
/// Implementation of the `Crypt.Encoders.Sha512Encoder` class.

namespace Crypt.Encoders {
  using System.Globalization;
  using System.Security.Cryptography;
  using System.Text;

  using Crypt.Encoders.Properties;

  /// Represents the SHA-512 encoding method.
  public class Sha512Encoder: IStringEncoder {

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
      var buffer = Encoding.Default.GetBytes(text);
      var hash = SHA512.Create().ComputeHash(buffer);

      var builder = new StringBuilder(hash.Length * 2);
      foreach(var item in hash) builder.Append(item.ToString("x2", CultureInfo.InvariantCulture));
      return builder.ToString();
    }
  }
}