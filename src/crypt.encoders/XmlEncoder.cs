/// @file
/// Implementation of the `Crypt.Encoders.XmlEncoder` class.

namespace Crypt.Encoders {
  using System.Web;
  using Crypt.Encoders.Properties;
  
  /// Represents the XML encoding method.
  public class XmlEncoder: IStringEncoder {

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.XmlDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "XML"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return HttpUtility.HtmlAttributeEncode(text).Replace(">", "&gt;");
    }
  }
}