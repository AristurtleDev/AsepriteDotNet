// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Compression;

/// <summary>
/// Implements the CRC-32 (Cyclic Redundancy Check) algorithm for data integrity verification.
/// </summary>
/// <remarks>
/// Uses the IEEE 802.3 polynomial (0xEDB88320) as specified in the PNG specification.
/// The algorithm employs a lookup table for efficient computation and follows the standard
/// CRC-32 implementation used in PNG files, ZIP archives, and other common formats.
/// Initial and final values are XORed with 0xFFFFFFFF to detect leading/trailing zeros.
/// </remarks>
internal class CRC
{
    /// <summary>
    /// The default initial value for CRC-32 calculations.
    /// </summary>
    public const uint DEFAULT = 0xFFFFFFFF;

    private static readonly uint[] _crcTable = new uint[256];

    private uint _value;

    /// <summary>
    /// Gets the current CRC-32 checksum value with final XOR transformation applied.
    /// </summary>
    internal uint CurrentValue => _value ^ 0xFFFFFFFF;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRC"/> class with the default initial value.
    /// </summary>
    internal CRC() => _value = DEFAULT;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRC"/> class with a specified initial value.
    /// </summary>
    /// <param name="initial">The initial CRC value to start with.</param>
    internal CRC(uint initial) => _value = initial;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRC"/> class and immediately processes the provided data.
    /// </summary>
    /// <param name="initial">The initial data to process for CRC calculation.</param>
    internal CRC(ReadOnlySpan<byte> initial) : this() => _ = Update(initial);

    /// <summary>
    /// Initializes the CRC lookup table using the IEEE 802.3 polynomial.
    /// </summary>
    /// <remarks>
    /// Generates a 256-entry lookup table using polynomial 0xEDB88320 (reversed representation
    /// of 0x04C11DB7). Each table entry represents the CRC remainder for the corresponding
    /// byte value, enabling fast table-driven computation during CRC updates.
    /// </remarks>
    static CRC()
    {

        //  Make the table for fast crc
        //  https://www.w3.org/TR/2003/REC-PNG-20031110/#D-CRCAppendix
        uint c;
        for (uint n = 0; n < 256; n++)
        {
            c = n;
            for (int k = 0; k < 8; k++)
            {
                if ((c & 1) != 0)
                {
                    c = 0xEDB88320 ^ (c >> 1);
                }
                else
                {
                    c >>= 1;
                }
            }

            _crcTable[n] = c;
        }
    }

    /// <summary>
    /// Resets the CRC to the default initial value.
    /// </summary>
    internal void Reset() => _value = DEFAULT;

    /// <summary>
    /// Updates the CRC with the provided data buffer.
    /// </summary>
    /// <param name="buffer">The data to include in the CRC calculation.</param>
    /// <returns>The updated CRC-32 value with final XOR transformation applied.</returns>
    internal uint Update(ReadOnlySpan<byte> buffer)
    {
        for (int n = 0; n < buffer.Length; n++)
        {
            _value = _crcTable[(_value ^ buffer[n]) & 0xFF] ^ (_value >> 8);
        }

        return _value ^ 0xFFFFFFFF;
    }

    /// <summary>
    /// Calculates the CRC-32 checksum for the provided data buffer.
    /// </summary>
    /// <param name="buffer">The data to calculate the CRC for.</param>
    /// <returns>The complete CRC-32 checksum value.</returns>
    internal static uint Calculate(ReadOnlySpan<byte> buffer) => new CRC().Update(buffer);
}
