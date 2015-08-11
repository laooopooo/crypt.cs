/// @file
/// Implementation of the `Crypt.Encoders.UpperCaseEncoder` class.

namespace Crypt.Encoders {
  using System;
  using Properties;
  
  /// Represents the UpperCase encoding method.
  public class UpperCaseEncoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public UpperCaseEncoder() {}

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
      if(text==null) throw new ArgumentNullException("text");
      return text.ToUpperInvariant();
    }
  }
}