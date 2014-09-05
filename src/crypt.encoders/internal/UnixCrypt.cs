﻿/**
 * Implementation of the `DigiWar.Security.Cryptography.UnixCrypt` class.
 * @module encoders/internal/UnixCrypt
 */

namespace DigiWar.Security.Cryptography {
  using System;
  using System.Linq;
  using System.Text;
  
  /**
   * Provides the Unix `crypt()` encryption algorithm.
   *
   * This class is a port from Java source. I do not understand the underlying algorithms, I just converted it to C# and it works.
   * Because I do not understand the underlying algorithms I cannot give most of the variables useful names. I have no clue what their
   * significance is. I tried to give the variable names as much meaning as possible, but the original source just called them `a`, `b`, `c`, etc...
   * 
   * A very important thing to note is that all integers in this code are UNSIGNED integers! Do not change this, ever!!! It will seriously fuckup the working
   * of this class. It uses major bitshifting and while Java gives you the >>> operator to signify a right bitshift without setting the MSB for
   * a signed integer, C# does not have this operator and will just set the new MSB for you if it happened to be set the moment you bitshifted it.
   * This is undesirable for most bitshifts and in the cases it did matter, I casted the variable back to an integer. This was only required where
   * a variable was on the right-side of a bitshift operator.
   * 
   * @class DigiWar.Security.Cryptography.UnixCrypt
   * @static
   */
  internal static class UnixCrypt {
  
    /**
     * The list with characters allowed in a Unix encrypted password.
     * It is used to randomly chose two characters for use in the encryption.
     * @property m_encryptionSaltCharacters
     * @type System.String
     * @static
     * @final
     * @private
     */
    private const string m_encryptionSaltCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789./";

    /**
     * A lookup-table, presumably filled with some sort of encryption key. 
     * It is used to calculate the index to the `m_SPTranslationTable` lookup-table.
     * @property m_saltTranslation
     * @type System.UInt32[]
     * @static
     * @final
     * @private
     */
    private static readonly uint[] m_saltTranslation = {
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 
      0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 
      0x0A, 0x0B, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 
      0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 
      0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 
      0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20, 0x21, 0x22, 
      0x23, 0x24, 0x25, 0x20, 0x21, 0x22, 0x23, 0x24, 
      0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 
      0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 
      0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 
      0x3D, 0x3E, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 
    };

    /**
     * A lookup-table.
     * It is used to calculate the index to the `m_skb` lookup-table.
     * @property m_shifts
     * @type System.Boolean[]
     * @static
     * @final
     * @private
     */
    private static readonly bool[] m_shifts = {
      false, false, true, true, true, true, true, true,
      false, true,  true, true, true, true, true, false
    };

    /**
     * A lookup-table.
     * It is used the dynamically create the schedule lookup-table.
     * @property m_skb
     * @type System.UInt32[]
     * @static
     * @final
     * @private
     */
    private static readonly uint[,] m_skb = {
      {
        /* for C bits (numbered as per FIPS 46) 1 2 3 4 5 6 */
        0x00000000, 0x00000010, 0x20000000, 0x20000010, 
        0x00010000, 0x00010010, 0x20010000, 0x20010010, 
        0x00000800, 0x00000810, 0x20000800, 0x20000810, 
        0x00010800, 0x00010810, 0x20010800, 0x20010810, 
        0x00000020, 0x00000030, 0x20000020, 0x20000030, 
        0x00010020, 0x00010030, 0x20010020, 0x20010030, 
        0x00000820, 0x00000830, 0x20000820, 0x20000830, 
        0x00010820, 0x00010830, 0x20010820, 0x20010830, 
        0x00080000, 0x00080010, 0x20080000, 0x20080010, 
        0x00090000, 0x00090010, 0x20090000, 0x20090010, 
        0x00080800, 0x00080810, 0x20080800, 0x20080810, 
        0x00090800, 0x00090810, 0x20090800, 0x20090810, 
        0x00080020, 0x00080030, 0x20080020, 0x20080030, 
        0x00090020, 0x00090030, 0x20090020, 0x20090030, 
        0x00080820, 0x00080830, 0x20080820, 0x20080830, 
        0x00090820, 0x00090830, 0x20090820, 0x20090830, 
      },
      {
        /* for C bits (numbered as per FIPS 46) 7 8 10 11 12 13 */
        0x00000000, 0x02000000, 0x00002000, 0x02002000, 
        0x00200000, 0x02200000, 0x00202000, 0x02202000, 
        0x00000004, 0x02000004, 0x00002004, 0x02002004, 
        0x00200004, 0x02200004, 0x00202004, 0x02202004, 
        0x00000400, 0x02000400, 0x00002400, 0x02002400, 
        0x00200400, 0x02200400, 0x00202400, 0x02202400, 
        0x00000404, 0x02000404, 0x00002404, 0x02002404, 
        0x00200404, 0x02200404, 0x00202404, 0x02202404, 
        0x10000000, 0x12000000, 0x10002000, 0x12002000, 
        0x10200000, 0x12200000, 0x10202000, 0x12202000, 
        0x10000004, 0x12000004, 0x10002004, 0x12002004, 
        0x10200004, 0x12200004, 0x10202004, 0x12202004, 
        0x10000400, 0x12000400, 0x10002400, 0x12002400, 
        0x10200400, 0x12200400, 0x10202400, 0x12202400, 
        0x10000404, 0x12000404, 0x10002404, 0x12002404, 
        0x10200404, 0x12200404, 0x10202404, 0x12202404, 
      },
      {
        /* for C bits (numbered as per FIPS 46) 14 15 16 17 19 20 */
        0x00000000, 0x00000001, 0x00040000, 0x00040001, 
        0x01000000, 0x01000001, 0x01040000, 0x01040001, 
        0x00000002, 0x00000003, 0x00040002, 0x00040003, 
        0x01000002, 0x01000003, 0x01040002, 0x01040003, 
        0x00000200, 0x00000201, 0x00040200, 0x00040201, 
        0x01000200, 0x01000201, 0x01040200, 0x01040201, 
        0x00000202, 0x00000203, 0x00040202, 0x00040203, 
        0x01000202, 0x01000203, 0x01040202, 0x01040203, 
        0x08000000, 0x08000001, 0x08040000, 0x08040001, 
        0x09000000, 0x09000001, 0x09040000, 0x09040001, 
        0x08000002, 0x08000003, 0x08040002, 0x08040003, 
        0x09000002, 0x09000003, 0x09040002, 0x09040003, 
        0x08000200, 0x08000201, 0x08040200, 0x08040201, 
        0x09000200, 0x09000201, 0x09040200, 0x09040201, 
        0x08000202, 0x08000203, 0x08040202, 0x08040203, 
        0x09000202, 0x09000203, 0x09040202, 0x09040203, 
      },
      {
        /* for C bits (numbered as per FIPS 46) 21 23 24 26 27 28 */
        0x00000000, 0x00100000, 0x00000100, 0x00100100, 
        0x00000008, 0x00100008, 0x00000108, 0x00100108, 
        0x00001000, 0x00101000, 0x00001100, 0x00101100, 
        0x00001008, 0x00101008, 0x00001108, 0x00101108, 
        0x04000000, 0x04100000, 0x04000100, 0x04100100, 
        0x04000008, 0x04100008, 0x04000108, 0x04100108, 
        0x04001000, 0x04101000, 0x04001100, 0x04101100, 
        0x04001008, 0x04101008, 0x04001108, 0x04101108, 
        0x00020000, 0x00120000, 0x00020100, 0x00120100, 
        0x00020008, 0x00120008, 0x00020108, 0x00120108, 
        0x00021000, 0x00121000, 0x00021100, 0x00121100, 
        0x00021008, 0x00121008, 0x00021108, 0x00121108, 
        0x04020000, 0x04120000, 0x04020100, 0x04120100, 
        0x04020008, 0x04120008, 0x04020108, 0x04120108, 
        0x04021000, 0x04121000, 0x04021100, 0x04121100, 
        0x04021008, 0x04121008, 0x04021108, 0x04121108, 
      },
      {
        /* for D bits (numbered as per FIPS 46) 1 2 3 4 5 6 */
        0x00000000, 0x10000000, 0x00010000, 0x10010000, 
        0x00000004, 0x10000004, 0x00010004, 0x10010004, 
        0x20000000, 0x30000000, 0x20010000, 0x30010000, 
        0x20000004, 0x30000004, 0x20010004, 0x30010004, 
        0x00100000, 0x10100000, 0x00110000, 0x10110000, 
        0x00100004, 0x10100004, 0x00110004, 0x10110004, 
        0x20100000, 0x30100000, 0x20110000, 0x30110000, 
        0x20100004, 0x30100004, 0x20110004, 0x30110004, 
        0x00001000, 0x10001000, 0x00011000, 0x10011000, 
        0x00001004, 0x10001004, 0x00011004, 0x10011004, 
        0x20001000, 0x30001000, 0x20011000, 0x30011000, 
        0x20001004, 0x30001004, 0x20011004, 0x30011004, 
        0x00101000, 0x10101000, 0x00111000, 0x10111000, 
        0x00101004, 0x10101004, 0x00111004, 0x10111004, 
        0x20101000, 0x30101000, 0x20111000, 0x30111000, 
        0x20101004, 0x30101004, 0x20111004, 0x30111004, 
      },
      {
        /* for D bits (numbered as per FIPS 46) 8 9 11 12 13 14 */
        0x00000000, 0x08000000, 0x00000008, 0x08000008, 
        0x00000400, 0x08000400, 0x00000408, 0x08000408, 
        0x00020000, 0x08020000, 0x00020008, 0x08020008, 
        0x00020400, 0x08020400, 0x00020408, 0x08020408, 
        0x00000001, 0x08000001, 0x00000009, 0x08000009, 
        0x00000401, 0x08000401, 0x00000409, 0x08000409, 
        0x00020001, 0x08020001, 0x00020009, 0x08020009, 
        0x00020401, 0x08020401, 0x00020409, 0x08020409, 
        0x02000000, 0x0A000000, 0x02000008, 0x0A000008, 
        0x02000400, 0x0A000400, 0x02000408, 0x0A000408, 
        0x02020000, 0x0A020000, 0x02020008, 0x0A020008, 
        0x02020400, 0x0A020400, 0x02020408, 0x0A020408, 
        0x02000001, 0x0A000001, 0x02000009, 0x0A000009, 
        0x02000401, 0x0A000401, 0x02000409, 0x0A000409, 
        0x02020001, 0x0A020001, 0x02020009, 0x0A020009, 
        0x02020401, 0x0A020401, 0x02020409, 0x0A020409, 
      },
      {
        /* for D bits (numbered as per FIPS 46) 16 17 18 19 20 21 */
        0x00000000, 0x00000100, 0x00080000, 0x00080100, 
        0x01000000, 0x01000100, 0x01080000, 0x01080100, 
        0x00000010, 0x00000110, 0x00080010, 0x00080110, 
        0x01000010, 0x01000110, 0x01080010, 0x01080110, 
        0x00200000, 0x00200100, 0x00280000, 0x00280100, 
        0x01200000, 0x01200100, 0x01280000, 0x01280100, 
        0x00200010, 0x00200110, 0x00280010, 0x00280110, 
        0x01200010, 0x01200110, 0x01280010, 0x01280110, 
        0x00000200, 0x00000300, 0x00080200, 0x00080300, 
        0x01000200, 0x01000300, 0x01080200, 0x01080300, 
        0x00000210, 0x00000310, 0x00080210, 0x00080310, 
        0x01000210, 0x01000310, 0x01080210, 0x01080310, 
        0x00200200, 0x00200300, 0x00280200, 0x00280300, 
        0x01200200, 0x01200300, 0x01280200, 0x01280300, 
        0x00200210, 0x00200310, 0x00280210, 0x00280310, 
        0x01200210, 0x01200310, 0x01280210, 0x01280310, 
      },
      {
        /* for D bits (numbered as per FIPS 46) 22 23 24 25 27 28 */
        0x00000000, 0x04000000, 0x00040000, 0x04040000, 
        0x00000002, 0x04000002, 0x00040002, 0x04040002, 
        0x00002000, 0x04002000, 0x00042000, 0x04042000, 
        0x00002002, 0x04002002, 0x00042002, 0x04042002, 
        0x00000020, 0x04000020, 0x00040020, 0x04040020, 
        0x00000022, 0x04000022, 0x00040022, 0x04040022, 
        0x00002020, 0x04002020, 0x00042020, 0x04042020, 
        0x00002022, 0x04002022, 0x00042022, 0x04042022, 
        0x00000800, 0x04000800, 0x00040800, 0x04040800, 
        0x00000802, 0x04000802, 0x00040802, 0x04040802, 
        0x00002800, 0x04002800, 0x00042800, 0x04042800, 
        0x00002802, 0x04002802, 0x00042802, 0x04042802, 
        0x00000820, 0x04000820, 0x00040820, 0x04040820, 
        0x00000822, 0x04000822, 0x00040822, 0x04040822, 
        0x00002820, 0x04002820, 0x00042820, 0x04042820, 
        0x00002822, 0x04002822, 0x00042822, 0x04042822, 
      }
    };

    /**
     * A lookup-table.
     * It is used to calculate two integers that are used to encrypt the password.
     * @property m_SPTranslationTable
     * @type System.UInt32[]
     * @static
     * @final
     * @private
     */
    private static readonly uint[,] m_SPTranslationTable = {
      {
        /* nibble 0 */
        0x00820200, 0x00020000, 0x80800000, 0x80820200,
        0x00800000, 0x80020200, 0x80020000, 0x80800000,
        0x80020200, 0x00820200, 0x00820000, 0x80000200,
        0x80800200, 0x00800000, 0x00000000, 0x80020000,
        0x00020000, 0x80000000, 0x00800200, 0x00020200,
        0x80820200, 0x00820000, 0x80000200, 0x00800200,
        0x80000000, 0x00000200, 0x00020200, 0x80820000,
        0x00000200, 0x80800200, 0x80820000, 0x00000000,
        0x00000000, 0x80820200, 0x00800200, 0x80020000,
        0x00820200, 0x00020000, 0x80000200, 0x00800200,
        0x80820000, 0x00000200, 0x00020200, 0x80800000,
        0x80020200, 0x80000000, 0x80800000, 0x00820000,
        0x80820200, 0x00020200, 0x00820000, 0x80800200,
        0x00800000, 0x80000200, 0x80020000, 0x00000000,
        0x00020000, 0x00800000, 0x80800200, 0x00820200,
        0x80000000, 0x80820000, 0x00000200, 0x80020200,
      },
      {
        /* nibble 1 */
        0x10042004, 0x00000000, 0x00042000, 0x10040000,
        0x10000004, 0x00002004, 0x10002000, 0x00042000,
        0x00002000, 0x10040004, 0x00000004, 0x10002000,
        0x00040004, 0x10042000, 0x10040000, 0x00000004,
        0x00040000, 0x10002004, 0x10040004, 0x00002000,
        0x00042004, 0x10000000, 0x00000000, 0x00040004,
        0x10002004, 0x00042004, 0x10042000, 0x10000004,
        0x10000000, 0x00040000, 0x00002004, 0x10042004,
        0x00040004, 0x10042000, 0x10002000, 0x00042004,
        0x10042004, 0x00040004, 0x10000004, 0x00000000,
        0x10000000, 0x00002004, 0x00040000, 0x10040004,
        0x00002000, 0x10000000, 0x00042004, 0x10002004,
        0x10042000, 0x00002000, 0x00000000, 0x10000004,
        0x00000004, 0x10042004, 0x00042000, 0x10040000,
        0x10040004, 0x00040000, 0x00002004, 0x10002000,
        0x10002004, 0x00000004, 0x10040000, 0x00042000,
      },
      {
        /* nibble 2 */
        0x41000000, 0x01010040, 0x00000040, 0x41000040,
        0x40010000, 0x01000000, 0x41000040, 0x00010040,
        0x01000040, 0x00010000, 0x01010000, 0x40000000,
        0x41010040, 0x40000040, 0x40000000, 0x41010000,
        0x00000000, 0x40010000, 0x01010040, 0x00000040,
        0x40000040, 0x41010040, 0x00010000, 0x41000000,
        0x41010000, 0x01000040, 0x40010040, 0x01010000,
        0x00010040, 0x00000000, 0x01000000, 0x40010040,
        0x01010040, 0x00000040, 0x40000000, 0x00010000,
        0x40000040, 0x40010000, 0x01010000, 0x41000040,
        0x00000000, 0x01010040, 0x00010040, 0x41010000,
        0x40010000, 0x01000000, 0x41010040, 0x40000000,
        0x40010040, 0x41000000, 0x01000000, 0x41010040,
        0x00010000, 0x01000040, 0x41000040, 0x00010040,
        0x01000040, 0x00000000, 0x41010000, 0x40000040,
        0x41000000, 0x40010040, 0x00000040, 0x01010000,
      },
      {
        /* nibble 3 */
        0x00100402, 0x04000400, 0x00000002, 0x04100402,
        0x00000000, 0x04100000, 0x04000402, 0x00100002,
        0x04100400, 0x04000002, 0x04000000, 0x00000402,
        0x04000002, 0x00100402, 0x00100000, 0x04000000,
        0x04100002, 0x00100400, 0x00000400, 0x00000002,
        0x00100400, 0x04000402, 0x04100000, 0x00000400,
        0x00000402, 0x00000000, 0x00100002, 0x04100400,
        0x04000400, 0x04100002, 0x04100402, 0x00100000,
        0x04100002, 0x00000402, 0x00100000, 0x04000002,
        0x00100400, 0x04000400, 0x00000002, 0x04100000,
        0x04000402, 0x00000000, 0x00000400, 0x00100002,
        0x00000000, 0x04100002, 0x04100400, 0x00000400,
        0x04000000, 0x04100402, 0x00100402, 0x00100000,
        0x04100402, 0x00000002, 0x04000400, 0x00100402,
        0x00100002, 0x00100400, 0x04100000, 0x04000402,
        0x00000402, 0x04000000, 0x04000002, 0x04100400,
      },
      {
        /* nibble 4 */
        0x02000000, 0x00004000, 0x00000100, 0x02004108,
        0x02004008, 0x02000100, 0x00004108, 0x02004000,
        0x00004000, 0x00000008, 0x02000008, 0x00004100,
        0x02000108, 0x02004008, 0x02004100, 0x00000000,
        0x00004100, 0x02000000, 0x00004008, 0x00000108,
        0x02000100, 0x00004108, 0x00000000, 0x02000008,
        0x00000008, 0x02000108, 0x02004108, 0x00004008,
        0x02004000, 0x00000100, 0x00000108, 0x02004100,
        0x02004100, 0x02000108, 0x00004008, 0x02004000,
        0x00004000, 0x00000008, 0x02000008, 0x02000100,
        0x02000000, 0x00004100, 0x02004108, 0x00000000,
        0x00004108, 0x02000000, 0x00000100, 0x00004008,
        0x02000108, 0x00000100, 0x00000000, 0x02004108,
        0x02004008, 0x02004100, 0x00000108, 0x00004000,
        0x00004100, 0x02004008, 0x02000100, 0x00000108,
        0x00000008, 0x00004108, 0x02004000, 0x02000008,
      },
      {
        /* nibble 5 */
        0x20000010, 0x00080010, 0x00000000, 0x20080800,
        0x00080010, 0x00000800, 0x20000810, 0x00080000,
        0x00000810, 0x20080810, 0x00080800, 0x20000000,
        0x20000800, 0x20000010, 0x20080000, 0x00080810,
        0x00080000, 0x20000810, 0x20080010, 0x00000000,
        0x00000800, 0x00000010, 0x20080800, 0x20080010,
        0x20080810, 0x20080000, 0x20000000, 0x00000810,
        0x00000010, 0x00080800, 0x00080810, 0x20000800,
        0x00000810, 0x20000000, 0x20000800, 0x00080810,
        0x20080800, 0x00080010, 0x00000000, 0x20000800,
        0x20000000, 0x00000800, 0x20080010, 0x00080000,
        0x00080010, 0x20080810, 0x00080800, 0x00000010,
        0x20080810, 0x00080800, 0x00080000, 0x20000810,
        0x20000010, 0x20080000, 0x00080810, 0x00000000,
        0x00000800, 0x20000010, 0x20000810, 0x20080800,
        0x20080000, 0x00000810, 0x00000010, 0x20080010,
      },
      {
        /* nibble 6 */
        0x00001000, 0x00000080, 0x00400080, 0x00400001,
        0x00401081, 0x00001001, 0x00001080, 0x00000000,
        0x00400000, 0x00400081, 0x00000081, 0x00401000,
        0x00000001, 0x00401080, 0x00401000, 0x00000081,
        0x00400081, 0x00001000, 0x00001001, 0x00401081,
        0x00000000, 0x00400080, 0x00400001, 0x00001080,
        0x00401001, 0x00001081, 0x00401080, 0x00000001,
        0x00001081, 0x00401001, 0x00000080, 0x00400000,
        0x00001081, 0x00401000, 0x00401001, 0x00000081,
        0x00001000, 0x00000080, 0x00400000, 0x00401001,
        0x00400081, 0x00001081, 0x00001080, 0x00000000,
        0x00000080, 0x00400001, 0x00000001, 0x00400080,
        0x00000000, 0x00400081, 0x00400080, 0x00001080,
        0x00000081, 0x00001000, 0x00401081, 0x00400000,
        0x00401080, 0x00000001, 0x00001001, 0x00401081,
        0x00400001, 0x00401080, 0x00401000, 0x00001001,
      },
      {
        /* nibble 7 */
        0x08200020, 0x08208000, 0x00008020, 0x00000000,
        0x08008000, 0x00200020, 0x08200000, 0x08208020,
        0x00000020, 0x08000000, 0x00208000, 0x00008020,
        0x00208020, 0x08008020, 0x08000020, 0x08200000,
        0x00008000, 0x00208020, 0x00200020, 0x08008000,
        0x08208020, 0x08000020, 0x00000000, 0x00208000,
        0x08000000, 0x00200000, 0x08008020, 0x08200020,
        0x00200000, 0x00008000, 0x08208000, 0x00000020,
        0x00200000, 0x00008000, 0x08000020, 0x08208020,
        0x00008020, 0x08000000, 0x00000000, 0x00208000,
        0x08200020, 0x08008020, 0x08008000, 0x00200020,
        0x08208000, 0x00000020, 0x00200020, 0x08008000,
        0x08208020, 0x00200000, 0x08200000, 0x08000020,
        0x00208000, 0x00008020, 0x08008020, 0x08200000,
        0x00000020, 0x08208000, 0x00208020, 0x00000000,
        0x08000000, 0x08200020, 0x00008000, 0x00208020
      }
    };

    /**
     * A lookup-table filled with printable characters.
     * It is used to make sure the encrypted password contains only printable characters. It is filled with
     * ASCII characters 46 - 122 (from the dot (`.`) until (including) the lowercase `'z'`).
     * @property m_characterConversionTable
     * @type System.UInt32[]
     * @static
     * @final
     * @private
     */
    private static readonly uint[] m_characterConversionTable = {
      0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 
      0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 
      0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 
      0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53, 0x54, 
      0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x61, 0x62, 
      0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 
      0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 
      0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A
    };

    /**
     * Marks the size of the dynamically created schedule lookup-table.
     * @property m_desIterations
     * @type System.UInt32
     * @static
     * @final
     * @private
     * @default 16
     */
    private const int m_desIterations = 16;

    /**
     * Converts four seperate bytes into one unsigned integer.
     * @method FourBytesToInt
     * @param {System.Byte[]} inputBytes The bytes to use for the conversion.
     * @param {System.UInt32} offset The offset at which to start in the `inputBytes` buffer.
     * @return {System.UInt32} The resulting unisgned integer.
     */
    private static uint FourBytesToInt(byte[] inputBytes, uint offset) {
      // I used an int here because the compiler would complain the stuff below would require a cast from int to uint.
      // To keep the code cleaner I opted to use an int and cast it when I returned it.
      int resultValue = 0;

      resultValue = (inputBytes[offset++] & 0xFF);
      resultValue |= (inputBytes[offset++] & 0xFF) << 8;
      resultValue |= (inputBytes[offset++] & 0xFF) << 16;
      resultValue |= (inputBytes[offset++]& 0xFF) << 24;

      return (uint)resultValue;
    }

    /**
     * Converts an unsigned integer into 4 seperate bytes.
     * @method IntToFourBytes
     * @param {System.UInt32} inputInt The unsigned integers to convert.
     * @param {System.Byte[]} outputBytes The byte buffer into which to store the result.
     * @param {System.UInt32} offset The offset to start storing at in the `outputBytes` buffer.
     * @static
     * @private
     */
    private static void IntToFourBytes(uint inputInt, byte[] outputBytes, uint offset) {
      outputBytes[offset++] = (byte)(inputInt & 0xFF);
      outputBytes[offset++] = (byte)((inputInt >> 8) & 0xFF);
      outputBytes[offset++] = (byte)((inputInt >> 16) & 0xFF);
      outputBytes[offset++] = (byte)((inputInt >> 24) & 0xFF);
    }

    /**
     * Performs some operation on 4 unsigned integers. It's labeled `PERM_OP` in the original source.
     * @method PermOperation
     * @param {System.UInt32} firstInt The first unsigned integer to use.
     * @param {System.UInt32} secondInt The second unsigned integer to use.
     * @param {System.UInt32} thirdInt The third unsigned integer to use.
     * @param {System.UInt32} fourthInt The fourth unsigned integer to use.
     * @param {System.UInt32[]} operationResults An array of 2 unsigned integers that are the result of this operation.
     * @static
     * @private
     */
    private static void PermOperation(uint firstInt, uint secondInt, uint thirdInt, uint fourthInt, uint[] operationResults) {
      // Because here an uint variable is at the right side of a bitshift, I needed to cast it to int. See the remarks of the class itself
      // for more details.
      uint tempInt = ((firstInt >> (int)thirdInt) ^ secondInt) & fourthInt;
      firstInt ^= tempInt << (int)thirdInt;
      secondInt ^= tempInt;

      operationResults[0] = firstInt;
      operationResults[1] = secondInt;
    }

    /**
     * Performs some operation on 3 integers. It's labeled `HPERM_OP` in the original source.
     * @method HPermOperation
     * @param {System.UInt32} firstInt The first unsigned integer to use.
     * @param {System.Int32} secondInt The second signed integer to use.
     * @param {System.UInt32} thirdInt The third unsigned integer to use.
     * @return {System.UInt32} An integer that is the result of this operation.
     * @static
     * @private
     */
    private static uint HPermOperation(uint firstInt, int secondInt, uint thirdInt) {
      // The variable secondInt is always used to calculate the number at the right side of a
      // bitshift. It is not used anywhere else, so I made the method parameter an int, to avoid
      // unnecessary casting.
      uint tempInt = ((firstInt << (16 - secondInt)) ^ firstInt) & thirdInt;
      uint returnInt = firstInt ^ tempInt ^ (tempInt >> (16 - secondInt));

      return returnInt;
    }

    /**
     * This method does some very complex bit manipulations.
     * @method SetDESKey
     * @param {System.Byte[]} encryptionKey The input data to use for the bit manipulations.
     * @return {System.UInt32[]} `m_desIterations * 2` number of unsigned integers that are the result of the manipulations.
     * @static
     * @private
     */
    private static uint[] SetDESKey(byte[] encryptionKey) {
      uint[] schedule = new uint[m_desIterations * 2];

      uint firstInt = FourBytesToInt(encryptionKey, 0);
      uint secondInt = FourBytesToInt(encryptionKey, 4);

      uint[] operationResults = new uint[2];
      PermOperation(secondInt, firstInt, 4, 0x0F0F0F0F, operationResults);
      secondInt = operationResults[0];
      firstInt = operationResults[1];

      firstInt = HPermOperation(firstInt, -2, 0xCCCC0000);
      secondInt = HPermOperation(secondInt, -2, 0xCCCC0000);

      PermOperation(secondInt, firstInt, 1, 0x55555555, operationResults);
      secondInt = operationResults[0];
      firstInt = operationResults[1];

      PermOperation(firstInt, secondInt, 8, 0x00FF00FF, operationResults);
      firstInt = operationResults[0];
      secondInt = operationResults[1];

      PermOperation(secondInt, firstInt, 1, 0x55555555, operationResults);
      secondInt = operationResults[0];
      firstInt = operationResults[1];

      secondInt = (((secondInt & 0xFF) << 16) | (secondInt & 0xFF00) |
        ((secondInt & 0xFF0000) >> 16) | ((firstInt & 0xF0000000) >> 4));

      firstInt &= 0x0FFFFFFF;

      bool needToShift;
      uint firstSkbValue, secondSkbValue;
      uint scheduleIndex = 0;

      for(int index = 0; index < m_desIterations; index++) {
        needToShift = m_shifts[index];
        if(needToShift) {
          firstInt = (firstInt >> 2) | (firstInt << 26);
          secondInt = (secondInt >> 2) | (secondInt << 26);
        }
        else {
          firstInt = (firstInt >> 1) | (firstInt << 27);
          secondInt = (secondInt >> 1) | (secondInt << 27);
        }

        firstInt &= 0x0FFFFFFF;
        secondInt &= 0xFFFFFFF;

        firstSkbValue = m_skb[0, firstInt & 0x3F] |
          m_skb[1, ((firstInt >> 6) & 0x03) | ((firstInt >> 7) & 0x3C)] |
          m_skb[2, ((firstInt >> 13) & 0x0F) | ((firstInt >> 14) & 0x30)] |
          m_skb[3, ((firstInt >> 20) & 0x01) | ((firstInt >> 21) & 0x06) | ((firstInt >> 22) & 0x38)];

        secondSkbValue = m_skb[4, secondInt & 0x3F] |
          m_skb[5, ((secondInt >> 7) & 0x03) | ((secondInt >> 8) & 0x3C)] |
          m_skb[6, (secondInt >> 15) & 0x3F] |
          m_skb[7, ((secondInt >> 21) & 0x0F) | ((secondInt >> 22) & 0x30)];

        schedule[scheduleIndex++] = ((secondSkbValue << 16) | (firstSkbValue & 0xFFFF)) & 0xFFFFFFFF;
        firstSkbValue = ((firstSkbValue >> 16) | (secondSkbValue & 0xFFFF0000));

        firstSkbValue = (firstSkbValue << 4) | (firstSkbValue >> 28);
        schedule[scheduleIndex++] = firstSkbValue & 0xFFFFFFFF;
      }

      return schedule;
    }

    /**
     * This method does some bit manipulations.
     * @method DEncrypt
     * @param {System.UInt32} left An input that is manipulated and then used for output.
     * @param {System.UInt32} right This is used for the bit manipulation.
     * @param {System.UInt32} scheduleIndex The index of an uint to use from the `schedule` array.
     * @param {System.UInt32} firstSaltTranslator The translated salt for the first salt character.
     * @param {System.UInt32} secondSaltTranslator The translated salt for the second salt character.
     * @param {System.UInt32[]} schedule The schedule arrray calculated before.
     * @return {System.UInt32} The result of these manipulations.
     * @static
     * @private
     */
    private static uint DEncrypt(uint left, uint right, uint scheduleIndex, uint firstSaltTranslator, uint secondSaltTranslator, uint[] schedule) {
      uint firstInt, secondInt, thirdInt;

      thirdInt = right ^ (right >> 16);
      secondInt = thirdInt & firstSaltTranslator;
      thirdInt = thirdInt & secondSaltTranslator;

      secondInt = (secondInt ^ (secondInt << 16)) ^ right ^ schedule[scheduleIndex];
      firstInt = (thirdInt ^ (thirdInt << 16)) ^ right ^ schedule[scheduleIndex+1];
      firstInt = (firstInt >> 4) | (firstInt << 28);

      left ^= (m_SPTranslationTable[1, firstInt & 0x3F] |
        m_SPTranslationTable[3, (firstInt >> 8) & 0x3F] |
        m_SPTranslationTable[5, (firstInt >> 16) & 0x3F] |
        m_SPTranslationTable[7, (firstInt >> 24) & 0x3F] |
        m_SPTranslationTable[0, secondInt & 0x3F] |
        m_SPTranslationTable[2, (secondInt >> 8) & 0x3F] |
        m_SPTranslationTable[4, (secondInt >> 16) & 0x3F] |
        m_SPTranslationTable[6, (secondInt >> 24) & 0x3F]);

      return left;
    }

    /**
     * Calculates two unsigned integers that are used to encrypt the password.
     * @method Body
     * @param {System.UInt32[]} schedule The schedule table calculated earlier.
     * @param {System.UInt32} firstSaltTranslator The first translated salt character.
     * @param {System.UInt32} secondSaltTranslator The second translated salt character.
     * @return {System.UInt32[]} 2 unsigned integers in an array.
     * @static
     * @private
     */
    private static uint[] Body(uint[] schedule, uint firstSaltTranslator, uint secondSaltTranslator) {
      uint left = 0;
      uint right = 0;
      uint tempInt;

      for(int index = 0; index < 25; index++) {
        for(uint secondIndex = 0; secondIndex < m_desIterations * 2; secondIndex += 4) {
          left = DEncrypt(left, right, secondIndex, firstSaltTranslator, secondSaltTranslator, schedule);
          right = DEncrypt(right, left, secondIndex + 2, firstSaltTranslator, secondSaltTranslator, schedule);
        }
            
        tempInt = left;
        left = right;
        right = tempInt;
      }

      tempInt = right;
      right = (left >> 1) | (left << 31);
      left = (tempInt >> 1) | (tempInt << 31);

      left &= 0xFFFFFFFF;
      right &= 0xFFFFFFFF;

      uint[] operationResults = new uint[2];

      PermOperation(right, left, 1, 0x55555555, operationResults);
      right = operationResults[0];
      left = operationResults[1];

      PermOperation(left, right, 8, 0x00FF00FF, operationResults);
      left = operationResults[0];
      right = operationResults[1];

      PermOperation(right, left, 2, 0x33333333, operationResults);
      right = operationResults[0];
      left = operationResults[1];

      PermOperation(left, right, 16, 0xFFFF, operationResults);
      left = operationResults[0];
      right = operationResults[1];

      PermOperation(right, left, 4, 0x0F0F0F0F, operationResults);
      right = operationResults[0];
      left = operationResults[1];

      uint[] singleOutputKey = new uint[2];
      singleOutputKey[0] = left;
      singleOutputKey[1] = right;

      return singleOutputKey;
    }

    /**
     * Encrypts the specified string using the Unix `crypt` algorithm.
     * @method Crypt
     * @param {System.String} [encryptionSalt] 2 random printable characters that are used to randomize the encryption.
     * @param {System.String} textToEncrypt The text that must be encrypted.
     * @return {System.String} The encrypted text.
     * @static
     */
    public static string Crypt(string textToEncrypt) {
      Random randomGenerator = new Random();

      int maxGeneratedNumber = m_encryptionSaltCharacters.Length;
      int randomIndex;
      StringBuilder encryptionSaltBuilder = new StringBuilder();
      for(int index = 0; index < 2; index++) {
        randomIndex = randomGenerator.Next(maxGeneratedNumber);
        encryptionSaltBuilder.Append(m_encryptionSaltCharacters[randomIndex]);
      }

      string encryptionSalt = encryptionSaltBuilder.ToString();
      return Crypt(encryptionSalt, textToEncrypt);
    }

    public static string Crypt(string encryptionSalt, string textToEncrypt) {
      if(encryptionSalt==null) throw new ArgumentNullException("encryptionSalt");
      if(textToEncrypt==null) throw new ArgumentNullException("textToEncrypt");
          
      bool isSaltTooSmall = (encryptionSalt.Length < 2);
      if(isSaltTooSmall) throw new ArgumentException("The encryptionSalt must be 2 characters big.");

      char firstSaltCharacter = encryptionSalt[0];
      char secondSaltCharacter = encryptionSalt[1];

      // Make sure the string builder is big enough AND filled with 13 characters (the length of the encrypted password).
      // We will use the index operator to set them, but when the characters are not present, even though the string builder
      // has enough capacity, it will throw an exception.
      StringBuilder encryptionBuffer = new StringBuilder("*************");
      encryptionBuffer[0] = firstSaltCharacter;
      encryptionBuffer[1] = secondSaltCharacter;

      // Use the ASCII value of the salt characters to lookup a number in the salt translation table.
      uint firstSaltTranslator = m_saltTranslation[Convert.ToUInt32(firstSaltCharacter)];
      uint secondSaltTranslator = m_saltTranslation[Convert.ToUInt32(secondSaltCharacter)] << 4;

      // Build the first encryption key table by taking the ASCII value of every character in the text to encrypt and
      // multiplying it by two. Note how the cast will not lose any information. The highest possible ASCII character
      // in a password is the tilde (~), which has ASCII value 126, so the highest possible value after the
      // multiplication would be 252.
      byte[] encryptionKey = new byte[8];
      for(int index = 0; index < encryptionKey.Length && index < textToEncrypt.Length; index++) {
        int shiftedCharacter = Convert.ToInt32(textToEncrypt[index]);
        encryptionKey[index] = (byte)(shiftedCharacter << 1);
      }

      uint[] schedule = SetDESKey(encryptionKey);
      uint[] singleOutputKey = Body(schedule, firstSaltTranslator, secondSaltTranslator);

      byte[] binaryBuffer = new byte[9];
      IntToFourBytes(singleOutputKey[0], binaryBuffer, 0);
      IntToFourBytes(singleOutputKey[1], binaryBuffer, 4);
      binaryBuffer[8] = 0;

      uint binaryBufferIndex = 0;
      uint passwordCharacter;
      uint bitChecker = 0x80;
      bool isAnyBitSet, bitCheckerOverflow;
      for(int index = 2; index < 13; index++) {
        passwordCharacter = 0;
        for(int secondIndex = 0; secondIndex < 6; secondIndex++) {
          passwordCharacter <<= 1;
          isAnyBitSet = ((binaryBuffer[binaryBufferIndex] & bitChecker) != 0);
          if(isAnyBitSet) passwordCharacter |= 1;

          bitChecker >>= 1;
          bitCheckerOverflow = (bitChecker == 0);
          if(bitCheckerOverflow) {
            binaryBufferIndex++;
            bitChecker = 0x80;
          }

          // The original source had the line below, I moved it outside the compound signs, because it will overwrite the value
          // a few times before incrementing the index. Where it is now it will be written only once.
          // Just to be on the safe side, I keep the original line here, so I know where it originally was.
          //encryptionBuffer[index] = Convert.ToChar(m_characterConversionTable[passwordCharacter]);
        }

        encryptionBuffer[index] = Convert.ToChar(m_characterConversionTable[passwordCharacter]);
      }
         
      return encryptionBuffer.ToString();
    }
  }
}
