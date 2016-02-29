/// @file
/// Implementation of the `Crypt.Encoders.UpperCaseEncoder` class.

namespace Crypt.Encoders {
  using System;
  using Crypt.Encoders.Properties;
  
  /// Represents the UpperCase encoding method.
  public class UpperCaseEncoder: IStringEncoder {

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.UpperCaseDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "UpperCase"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    /// @exception System.ArgumentNullException The specified string is `null`.
    public string Encode(string text) {
      if(text == null) throw new ArgumentNullException("text");
      return text.ToUpperInvariant();
    }
  }
}