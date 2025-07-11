// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.Compression;

/// <summary>
/// Utility class for calculating an Adler-32 checksum
/// </summary>
internal class Adler32
{
    private const uint BASE = 65521;    //  Largest prime smaller than 65536
    private const int NMAX = 5552;      //  NMAX is the largest n such that 255n(+1)/2 + (n+1)(BASE-1) <= 2^32-1

    private uint _value;

    /// <summary>
    /// Gets the current checksum value.
    /// </summary>
    internal uint CurrentValue => _value;

    /// <summary>
    /// Creates a new instance of the <see cref="Adler32"/> class.
    /// </summary>
    /// <remarks>
    /// This will initialize the underlying checksum value to 1
    /// </remarks>
    internal Adler32() => _value = 1U;

    /// <summary>
    /// Initializes a new instance of the <see cref="Adler32"/> class.
    /// </summary>
    /// <param name="initial">
    /// The initial checksum value to start with.
    /// </param>
    internal Adler32(uint initial) => _value = initial;

    /// <summary>
    /// Initializes a new instance of the <see cref="Adler32"/> class.
    /// </summary>
    /// <param name="initial">
    /// A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> elements that the initial checksum value will be
    /// calculated from
    /// </param>
    internal Adler32(ReadOnlySpan<byte> initial) : this() => _ = Update(initial);

    /// <summary>
    /// Resets the underlying checksum value of this instance of the <see cref="Adler32"/> class to 1.
    /// </summary>
    internal void Reset() => _value = 1U;

    /// <summary>
    /// Updates and returns the underlying checksum value using the  specified <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">
    /// A <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> elements that will be added to the underlying checksum value.
    /// </param>
    /// <returns>The updated checksum value.</returns>
    internal uint Update(ReadOnlySpan<byte> buffer)
    {
        uint sum2;
        uint n;
        int len = buffer.Length;
        int pos = 0;

        //  Split Adler-32 into component sums
        sum2 = (_value >> 16) & 0xFFFF;
        _value &= 0xFFFF;

        //  In case the user likes doing a byte at a time, keep it fast
        if (len == 1)
        {
            _value += buffer[0];

            if (_value >= BASE)
            {
                _value -= BASE;
            }

            sum2 += _value;

            if (sum2 >= BASE)
            {
                sum2 -= BASE;
            }

            _value |= (sum2 << 16);
            return _value;
        }

        //  In case short lengths are provided, keep it somewhat fast
        if (len < 16)
        {
            for (int i = 0; i < len; i++)
            {
                _value += buffer[pos++];
                sum2 += _value;
            }

            if (_value >= BASE)
            {
                _value -= BASE;
            }

            sum2 %= BASE;

            _value |= (sum2 << 16);
            return _value;
        }

        //  Do length NMAX blocks -- requires just one modulo operation
        while (len >= NMAX)
        {
            len -= NMAX;

            for (n = NMAX / 16; n > 0; --n)
            {
                for (int i = 0; i < 16; i++)
                {
                    _value += buffer[pos++];
                    sum2 += _value;
                }
            }

            _value %= BASE;
            sum2 %= BASE;
        }

        //  Do remaining bytes (less than NMAX, still just one modulo)
        if (len > 0)    //  avoid modulo if none remaining
        {
            while (len >= 16)
            {
                len -= 16;

                for (int i = 0; i < 16; i++)
                {
                    _value += buffer[pos++];
                    sum2 += _value;
                }
            }

            while (len-- > 0)
            {
                _value += buffer[pos++];
                sum2 += _value;
            }

            _value %= BASE;
            sum2 %= BASE;
        }

        //  Recombine sums
        _value |= (sum2 << 16);
        return _value;
    }
}
