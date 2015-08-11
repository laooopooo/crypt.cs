/// @file
/// Implementation of the `Crypt.Encoders.MD2Encoder` class.

namespace Crypt.Encoders {
  using System.Text;
  
  using MiniFramework.Text;
  using Mono.Security.Cryptography;
  using Properties;

  /// Represents the MD2 encoding method.
  public class MD2Encoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public MD2Encoder() {}

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
      var buffer=Encoding.Default.GetBytes(text);
      var hash=MD2.Create().ComputeHash(buffer);
      return HexCodec.GetString(hash);
    }
  }
}