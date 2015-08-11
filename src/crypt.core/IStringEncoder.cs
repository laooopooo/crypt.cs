/// @file
/// Implementation of the `Crypt.IStringEncoder` interface.

namespace Crypt {
  
  /// Represents a string encoder.
  public interface IStringEncoder {
  
    /// @property Description
    /// The encoder description.
    string Description {
      get;
    }

    /// @property Name
    /// The encoder name.
    string Name {
      get;
    }

    /// Encodes the specified string.
    /// @param text The string to encode.
    /// @returns The encoded string.
    string Encode(string text);
  }
}