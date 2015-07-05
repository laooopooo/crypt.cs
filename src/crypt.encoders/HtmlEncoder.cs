/// @file
/// Implementation of the `Crypt.Encoders.HtmlEncoder` class.

namespace Crypt.Encoders {
  using System;
  using System.Linq;
  
  using Crypt.Encoders.Properties;
  using MiniFramework.Web;
  
  /// Represents the HTML encoding method.
  public class HtmlEncoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public HtmlEncoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.HtmlDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "HTML"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    public string Encode(string text) {
      return Html.EntitiesEncode(text);
    }
  }
}