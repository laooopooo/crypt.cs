/// @file
/// Implementation of the `Crypt.Encoders.LowerCaseEncoder` class.

namespace Crypt.Encoders {
  using System;
  using Properties;
  
  /// Represents the LowerCase encoding method.
  public class LowerCaseEncoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public LowerCaseEncoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.LowerCaseDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "LowerCase"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    /// @exception System.ArgumentNullException The specified string is `null`.
    public string Encode(string text) {
      if(text==null) throw new ArgumentNullException("text");
      return text.ToLowerInvariant();
    }
  }
}