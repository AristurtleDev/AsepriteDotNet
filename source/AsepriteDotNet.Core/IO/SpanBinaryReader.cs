// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Buffers.Binary;
using System.Text;

namespace AsepriteDotNet.Core.IO;

/// <summary>
/// Provides high-performance binary data reading from memory spans with zero heap allocations.
/// </summary>
/// <remarks>
/// Designed specifically for parsing binary file formats where performance and memory efficiency
/// are critical. All multi-byte values are automatically converted from little-endian byte order
/// as required by the Aseprite file format specification.
/// The ref struct design prevents accidental boxing and ensures stack-only allocation.
/// </remarks>
public ref struct SpanBinaryReader
{
    private readonly ReadOnlySpan<byte> _buffer;
    private int _position;

    /// <summary>
    /// Gets the current position within the buffer.
    /// </summary>
    public readonly int Position => _position;

    /// <summary>
    /// Gets the total length of the buffer being read.
    /// </summary>
    public readonly int Length => _buffer.Length;

    /// <summary>
    /// Gets the number of bytes remaining to be read from the current position.
    /// </summary>
    public readonly int Remaining => _buffer.Length - _position;

    /// <summary>
    /// Gets a value indicating whether the reader has reached the end of the buffer.
    /// </summary>
    public readonly bool IsAtEnd => _position >= _buffer.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpanBinaryReader"/> with the specified buffer.
    /// </summary>
    /// <param name="buffer">The memory span containing binary data to read.</param>
    /// <remarks>
    /// The reader maintains a reference to the provided span and advances its position
    /// as data is read. The span must remain valid for the lifetime of the reader.
    /// </remarks>
    public SpanBinaryReader(ReadOnlySpan<byte> buffer)
    {
        _buffer = buffer;
    }

    /// <summary>
    /// Reads a single byte from the buffer and advances the position.
    /// </summary>
    /// <returns>The byte value at the current position.</returns>
    /// <exception cref="EndOfStreamException">Thrown when attempting to read beyond the buffer length.</exception>
    public byte ReadByte()
    {
        if (_position >= _buffer.Length)
        {
            throw new EndOfStreamException();
        }

        return _buffer[_position++];
    }

    /// <summary>
    /// Reads the specified number of bytes from the buffer and advances the position.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>A span containing the requested bytes from the buffer.</returns>
    /// <exception cref="EndOfStreamException">Thrown when attempting to read beyond the buffer length.</exception>
    public ReadOnlySpan<byte> ReadBytes(int count)
    {
        if (_position + count > _buffer.Length)
        {
            throw new EndOfStreamException();
        }

        ReadOnlySpan<byte> result = _buffer.Slice(_position, count);
        _position += count;
        return result;
    }

    /// <summary>
    /// Reads a 16-bit unsigned integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The unsigned integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    public ushort ReadUInt16()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(ushort));
        return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 16-bit signed integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The signed integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    public short ReadInt16()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(short));
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit unsigned integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The unsigned integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    public uint ReadUInt32()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(uint));
        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit signed integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The signed integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    public int ReadInt32()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(int));
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit single-precision floating-point value in little-endian format and advances the position.
    /// </summary>
    /// <returns>The floating-point value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    public float ReadSingle()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(float));
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }

    /// <summary>
    /// Reads a UTF-8 encoded string with a length prefix and advances the position.
    /// </summary>
    /// <returns>The decoded string value.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain for the length prefix or string data.</exception>
    /// <remarks>
    /// Length prefix is assumed to be a 16-bit unsigned integer.
    /// </remarks>
    public string ReadString()
    {
        int len = ReadUInt16();
        return ReadString(len, Encoding.UTF8);
    }

    /// <summary>
    /// Reads a string of the specified length using the provided encoding and advances the position.
    /// </summary>
    /// <param name="len">The number of bytes to read for the string data.</param>
    /// <param name="encoding">The text encoding to use for decoding the bytes.</param>
    /// <returns>The decoded string value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="len"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="encoding"/> is null.</exception>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    /// <remarks>
    /// Provides flexibility for reading strings with different encodings or when the length
    /// is determined by external factors rather than a length prefix.
    /// </remarks>
    public string ReadString(int len, Encoding encoding)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(len);
        ArgumentNullException.ThrowIfNull(encoding);

        ReadOnlySpan<byte> buffer = ReadBytes(len);
        return encoding.GetString(buffer);
    }

    /// <summary>
    /// Reads a 128-bit UUID (Universally Unique Identifier) and advances the position.
    /// </summary>
    /// <returns>The GUID value constructed from the 16-byte sequence.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    public Guid ReadGuid()
    {
        const int GUID_BUFFER_SIZE = 16;
        ReadOnlySpan<byte> buffer = ReadBytes(GUID_BUFFER_SIZE);
        return new Guid(buffer);
    }

    /// <summary>
    /// Advances the reader position by the specified number of bytes without reading data.
    /// </summary>
    /// <param name="count">The number of bytes to skip.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative.</exception>
    /// <exception cref="EndOfStreamException">Thrown when attempting to skip beyond the buffer length.</exception>
    public void Ignore(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        if (_position + count > _buffer.Length)
        {
            throw new EndOfStreamException();
        }
        _position += count;
    }

    /// <summary>
    /// Reads a 16-bit unsigned integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The unsigned integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    /// <remarks>
    /// Convenience method that maps to the Aseprite file format WORD data type specification.
    /// Equivalent to <see cref="ReadUInt16()"/>.
    /// </remarks>
    public ushort ReadWord() => ReadUInt16();

    /// <summary>
    /// Reads a 16-bit signed integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The signed integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    /// <remarks>
    /// Convenience method that maps to the Aseprite file format SHORT data type specification.
    /// Equivalent to <see cref="ReadInt16()"/>.
    /// </remarks>
    public short ReadShort() => ReadInt16();

    /// <summary>
    /// Reads a 32-bit unsigned integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The unsigned integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    /// <remarks>
    /// Convenience method that maps to the Aseprite file format DWORD data type specification.
    /// Equivalent to <see cref="ReadUInt32()"/>.
    /// </remarks>
    public uint ReadDword() => ReadUInt32();

    /// <summary>
    /// Reads a 32-bit signed integer in little-endian format and advances the position.
    /// </summary>
    /// <returns>The signed integer value converted from little-endian bytes.</returns>
    /// <exception cref="EndOfStreamException">Thrown when insufficient bytes remain in the buffer.</exception>
    /// <remarks>
    /// Convenience method that maps to the Aseprite file format LONG data type specification.
    /// Equivalent to <see cref="ReadInt32()"/>.
    /// </remarks>
    public int ReadLong() => ReadInt32();
}
