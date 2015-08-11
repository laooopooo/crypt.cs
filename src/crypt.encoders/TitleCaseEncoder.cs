/// @file
/// Implementation of the `Crypt.Encoders.TitleCaseEncoder` class.

namespace Crypt.Encoders {
  using System;

  using MiniFramework;
  using Properties;

  /// Represents the TitleCase encoding method.
  public class TitleCaseEncoder: IStringEncoder {
  
    /// Initializes a new instance of the class.
    public TitleCaseEncoder() {}

    /// @property Description
    /// The encoder description.
    public string Description {
      get { return Resources.TitleCaseDescription; }
    }

    /// @property Name
    /// The encoder name.
    public string Name {
      get { return "TitleCase"; }
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    /// @exception System.ArgumentNullException The specified string is `null`.
    public string Encode(string text) {
      if(text==null) throw new ArgumentNullException("text");
      return text.CapitalizeWords();
    }
  }
}