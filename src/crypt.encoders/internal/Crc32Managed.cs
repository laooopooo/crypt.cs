/// @file
/// Implementation of the `Crypt.Encoders.Internal.Crc32Managed` class.

namespace Crypt.Encoders.Internal {
  using System;

  /// Computes the SHA1 hash value for the input data using the managed library.
  [CLSCompliant(false)]
  public sealed class Crc32Managed: Crc32 {

    /// Initializes a new instance of the class.
    public Crc32Managed() {
      HashSizeValue = 32;
      Initialize();
    }

    /// @var hash
    /// The computed hash value.
    private uint hash;

    /// @var table
    /// The default table of the checksums of 8-bit encoded values.
    private static readonly uint[] table = InitializeTable(0xEDB88320);

    /// Initializes the instance.
    public override void Initialize() {
      hash = 0xFFFFFFFF;
    }

    /// Routes data written to the object into the hash algorithm for computing the hash value.
    /// @param array The input data.
    /// @param ibStart The offset into the byte array from which to begin using data.
    /// @param cbSize The number of bytes in the array to use as data.
    /// @exception System.ArgumentNullException The specified array is `null`.
    protected override void HashCore(byte[] array, int ibStart, int cbSize) {
      if(array == null) throw new ArgumentNullException("array");
      State = 1;
      for(int i = ibStart; i < cbSize; i++) hash = (hash>>8) ^ table[array[i] ^ (hash & 0xFF)];
    }

    /// Returns the computed hash value after all data has been written to the object.
    /// @returns The computed hash value.
    protected override byte[] HashFinal() {
      HashValue = BitConverter.GetBytes(~hash);
      State = 0;
      return HashValue;
    }

    /// Initializes the checksum table of a byte from the specified polynomial.
    /// @param polynomial The polynomial used to generate the checksum table.
    /// @returns The checksum table of 8-bit encoded values.
    private static uint[] InitializeTable(uint polynomial) {
      var checksums = new uint[256];

      for(int i = 0; i<checksums.Length; i++) {
        var value = (uint) i;

        for(int j = 0; j < 8; j++) {
          if((value & 1) != 0) value = (value >> 1) ^ polynomial;
          else value >>= 1;
        }

        checksums[i] = value;
      }

      return checksums;
    }
  }
}
