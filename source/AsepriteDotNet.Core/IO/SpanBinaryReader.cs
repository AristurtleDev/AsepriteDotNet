/*******************************************************************************
*** SpanBinaryReader
***
*** A high-performance ref struct that reads primitive data types directly from
*** ReadOnlySpan<byte> without allocations. Features automatic little-endian
*** conversion, bounds checking, and unsafe marshalling for structs. Ideal for
*** binary file parsing, network protocols, and performance-critical scenarios
*** where memory allocation overhead must be minimized.
***
*** Key features:
***     - Zero heap allocations - operates directly on memory spans
***     - Automatic little-endian byte order handling
***     - Built-in bounds checking with clear error messages
***     - Support for all common primitive types plus UTF-8 strings
***     - Unsafe struct marshalling for complex data types
***     - ref struct design prevents accidental boxing
*******************************************************************************/
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;

namespace AsepriteDotNet.Core.IO;

/// <summary>
/// Provides a high-performance binary reader for reading primitive data types from a read-only span of bytes.
/// </summary>
/// <remarks>
/// This reader operates directly on memory without allocations and automatically handles little-endian byte order
/// conversion. The reader maintains an internal position that advances with each read operation.
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
    /// Gets the total length of the underlying buffer.
    /// </summary>
    public readonly int Length => _buffer.Length;

    public readonly int Remaining => _buffer.Length - _position;

    /// <summary>
    /// Gets a value indicating whether the reader has reached the end of the buffer.
    /// </summary>
    public readonly bool IsAtEnd => _position >= _buffer.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpanBinaryReader"/> struct with the specified buffer.
    /// </summary>
    /// <param name="buffer">The read-only span of bytes to read from.</param>
    public SpanBinaryReader(ReadOnlySpan<byte> buffer)
    {
        _buffer = buffer;
    }

    /// <summary>
    /// Reads a single byte from the buffer and advances the position by one byte.
    /// </summary>
    /// <returns>The byte value at the current position.</returns>
    /// <exception cref="EndOfStreamException">Thrown when attempting to read beyond the end of the buffer.</exception>
    public byte ReadByte()
    {
        if (_position >= _buffer.Length)
        {
            throw new EndOfStreamException();
        }

        return _buffer[_position++];
    }

    /// <summary>
    /// Reads a specified number of bytes from the buffer and advances the position by the number of bytes read.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>A read-only span containing the requested bytes.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when attempting to read more bytes than are available in the buffer.
    /// </exception>
    /// <remarks>
    /// This method implements a no copy pattern by returning a span that is a slice of the original buffer that shares
    /// the same memory.
    /// </remarks>
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
    /// Reads a 16-bit unsigned integer from the buffer in little-endian format and advances the position by 2 bytes.
    /// </summary>
    /// <returns>The 16-bit unsigned integer value.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    public ushort ReadUInt16()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(ushort));
        return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 16-bit signed integer from the buffer in little-endian format and advances the position by 2 bytes.
    /// </summary>
    /// <returns>The 16-bit signed integer value.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    public short ReadInt16()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(short));
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit unsigned integer from the buffer in little-endian format and advances the position by 4 bytes.
    /// </summary>
    /// <returns>The 32-bit unsigned integer value.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    public uint ReadUInt32()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(uint));
        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit signed integer from the buffer in little-endian format and advances the position by 4 bytes.
    /// </summary>
    /// <returns>The 32-bit signed integer value.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    public int ReadInt32()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(int));
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit single-precision floating-point number from the buffer in little-endian format and advances the
    /// position by 4 bytes.
    /// </summary>
    /// <returns>The single-precision floating-point value.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    public float ReadSingle()
    {
        ReadOnlySpan<byte> buffer = ReadBytes(sizeof(float));
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }

    /// <summary>
    /// Reads a UTF-8 encoded string from the buffer, where the string length is prefixed as a 16-bit unsigned integer.
    /// </summary>
    /// <returns>The decoded string value.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    /// <remarks>
    /// This method first reads a 16-bit unsigned integer to determine the string length,
    /// then reads that many bytes and decodes them as UTF-8.
    /// </remarks>
    public string ReadString()
    {
        int len = ReadUInt16();
        return ReadString(len, Encoding.UTF8);
    }

    /// <summary>
    /// Reads a string of the specified length from the buffer using the specified encoding.
    /// </summary>
    /// <param name="len">The number of bytes to read for the string. Must be non-negative.</param>
    /// <param name="encoding">The encoding to use for decoding the bytes.</param>
    /// <returns>The decoded string value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="len"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="encoding"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    public string ReadString(int len, Encoding encoding)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(len);
        ArgumentNullException.ThrowIfNull(encoding);

        ReadOnlySpan<byte> buffer = ReadBytes(len);
        return encoding.GetString(buffer);
    }

    public Guid ReadGuid()
    {
        const int GUID_BUFFER_SIZE = 16;
        ReadOnlySpan<byte> buffer = ReadBytes(GUID_BUFFER_SIZE);
        return new Guid(buffer);
    }

    /// <summary>
    /// Reads a value type directly from the buffer using unsafe memory operations and advances the position by the
    /// specified size.
    /// </summary>
    /// <typeparam name="T">The value type to read. Must be a struct.</typeparam>
    /// <param name="size">The number of bytes to read from the buffer.</param>
    /// <returns>The value of type <typeparamref name="T"/> read from the buffer.</returns>
    /// <exception cref="EndOfStreamException">
    /// Thrown when there are insufficient bytes remaining in the buffer.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the type <typeparamref name="T"/> cannot be marshaled from the buffer data.
    /// </exception>
    /// <remarks>
    /// This method uses unsafe operations to directly marshal bytes to the target type.
    /// The caller must ensure that the size parameter matches the actual size of type <typeparamref name="T"/>
    /// and that the buffer contains valid data for the target type.
    /// </remarks>
    public T ReadUnsafe<T>(int size) where T : struct
    {
        T value;
        ReadOnlySpan<byte> buffer = ReadBytes(size);
        try
        {
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    value = Marshal.PtrToStructure<T>((IntPtr)ptr);
                }
            }
            return value;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Unable to read as type {typeof(T)}.  See inner exception for details", ex);
        }
        catch (MissingMethodException ex)
        {
            throw new InvalidOperationException($"Unable to read as type {typeof(T)}.  See inner exception for details", ex);
        }
    }

    public void Ignore(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        if (_position + count > _buffer.Length)
        {
            throw new EndOfStreamException();
        }
        _position += count;
    }

    public ushort ReadWord() => ReadUInt16();
    public short ReadShort() => ReadInt16();
    public uint ReadDword() => ReadUInt32();
    public int ReadLong() => ReadInt32();

}
