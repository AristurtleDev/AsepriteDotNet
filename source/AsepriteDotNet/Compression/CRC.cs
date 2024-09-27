// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Compression;

/// <summary>
/// Utility class for calculating a CRC checksum.
/// </summary>
internal class CRC
{
    /// <summary>
    /// The default initial CRC value.
    /// </summary>
    public const uint DEFAULT = 0xFFFFFFFF;

    private static readonly uint[] _crcTable = new uint[256];

    private uint _value;

    /// <summary>
    /// Gets the current checksum value.
    /// </summary>
    internal uint CurrentValue => _value ^ 0xFFFFFFFF;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRC"/> class with the default value of 0.
    /// </summary>
    internal CRC() => _value = DEFAULT;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRC"/> class.
    /// </summary>
    /// <param name="initial">The initial checksum value to start with.</param>
    internal CRC(uint initial) => _value = initial;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRC"/> class.
    /// </summary>
    /// <param name="initial">
    /// A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> elements that the initial checksum value will be
    /// calculated from.
    /// </param>
    internal CRC(ReadOnlySpan<byte> initial) : this() => _ = Update(initial);

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
    /// Resets the underlying checksum value of this instance of the <see cref="CRC"/> class to
    /// <see cref="CRC.DEFAULT"/>.
    /// </summary>
    internal void Reset() => _value = DEFAULT;

    /// <summary>
    /// Updates and returns the underlying checksum value using the  specified <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">
    /// A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> elements that will be added to the underlying checksum
    /// value.
    /// </param>
    /// <returns>The updated checksum value.</returns>
    internal uint Update(ReadOnlySpan<byte> buffer)
    {
        for (int n = 0; n < buffer.Length; n++)
        {
            _value = _crcTable[(_value ^ buffer[n]) & 0xFF] ^ (_value >> 8);
        }

        return _value ^ 0xFFFFFFFF;
    }

    internal static uint Calculate(ReadOnlySpan<byte> buffer) => new CRC().Update(buffer);
}
