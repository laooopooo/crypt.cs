/// @file
/// Implementation of the `Crypt.Encoders.MD2Encoder` class.

namespace Crypt.Encoders {
  using System.Globalization;
  using System.Text;
  
  using Crypt.Encoders.Properties;
  using Mono.Security.Cryptography;

  /// Represents the MD2 encoding method.
  public class MD2Encoder: IStringEncoder {

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.MD2Description; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "MD2"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      var buffer = Encoding.Default.GetBytes(text);
      var hash = MD2.Create().ComputeHash(buffer);

      var builder = new StringBuilder(hash.Length * 2);
      foreach(var item in hash) builder.Append(item.ToString("x2", CultureInfo.InvariantCulture));
      return builder.ToString();
    }
  }
}