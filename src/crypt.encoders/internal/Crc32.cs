/// @file
/// Implementation of the `Crypt.Encoders.Internal.Crc32` class.

namespace Crypt.Encoders.Internal {
  using System.Security.Cryptography;

  /// Computes the CRC32 hash value for the input data.
  public abstract class Crc32: HashAlgorithm {

    /// Initializes a new instance of the class.
    protected Crc32() {}

    /// Creates an instance of the default implementation of CRC32.
    /// @returns A new instance of CRC32 using the default implementation.
    public static new Crc32 Create() {
      return Create("Crypt.Encoders.Crc32");
    }

    /// Creates an instance of the specified implementation of CRC32.
    /// @param algName The name of the specific implementation of CRC32 to be used.
    /// @returns A new instance of CRC32 using the specified implementation.
    public static new Crc32 Create(string algName) {
      var algorithm = CryptoConfig.CreateFromName(algName);
      return (Crc32) (algorithm ?? new Crc32Managed());
    }
  }
}
