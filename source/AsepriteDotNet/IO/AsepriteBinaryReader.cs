//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Buffers.Binary;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using AsepriteDotNet.Compression;

/// <summary>
/// A low-level binary reader designed to read data types from an Aseprite file.
/// </summary>
internal sealed class AsepriteBinaryReader : IDisposable
{
    private readonly Stream _stream;
    private readonly bool _leaveOpen;
    private bool _isDisposed;

    /// <summary>
    /// Gets the current position of the underlying stream.
    /// </summary>
    public long Position => _stream.Position;

    public AsepriteBinaryReader(Stream input, bool leaveOpen = false)
    {
        ArgumentNullException.ThrowIfNull(input);
        ValidateCanRead(input.CanRead);
        ValidateCanSeek(input.CanSeek);
        _stream = input;
        _leaveOpen = leaveOpen;
    }

    ~AsepriteBinaryReader() => Dispose(false);

    /// <summary>
    /// Reads the next byte from the underlying stream and advances the stream by one byte.
    /// </summary>
    /// <returns>The next byte read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public byte ReadByte()
    {
        ValidateDisposed(_isDisposed);

        int b = _stream.ReadByte();
        if (b < 0)
        {
            throw new EndOfStreamException("The end of stream was reached");
        }
        return unchecked((byte)b);
    }

    /// <summary>
    /// Reads the specified number of bytes from the underlying stream and advances the stream by that number of bytes.
    /// </summary>
    /// <param name="count">The number of bytes to read from the underlying stream</param>
    /// <returns>A new byte array containing the bytes read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throw if <paramref name="count"/> is less than zero.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public byte[] ReadBytes(int count)
    {
        ValidateDisposed(_isDisposed);
#if NET6_0
        if(count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
#else
        ArgumentOutOfRangeException.ThrowIfNegative(count);
#endif

        if (count == 0)
        {
            return Array.Empty<byte>();
        }

        byte[] result = new byte[count];
#if NET6_0
        ReadExactly(_stream, result);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(result);
#endif
        return result;
    }

    /// <summary>
    /// Reads a 2-byte unsigned integer from the underlying stream and advances the stream by two bytes.
    /// </summary>
    /// <returns>The 2-byte unsigned integer read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public ushort ReadWord()
    {
        ValidateDisposed(_isDisposed);

        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
#if NET6_0
        ReadExactly(_stream, buffer);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(buffer);
#endif
        return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 2-byte signed integer from the underlying stream and advances the stream by two bytes.
    /// </summary>
    /// <returns>The 2-byte signed integer read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public short ReadShort()
    {
        ValidateDisposed(_isDisposed);

        Span<byte> buffer = stackalloc byte[sizeof(short)];
#if NET6_0
        ReadExactly(_stream, buffer);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(buffer);
#endif
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 4-byte unsigned integer from the underlying stream and advances the stream by four bytes.
    /// </summary>
    /// <returns>The 4-byte unsigned integer read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public uint ReadDword()
    {
        ValidateDisposed(_isDisposed);

        Span<byte> buffer = stackalloc byte[sizeof(uint)];
#if NET6_0
        ReadExactly(_stream, buffer);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(buffer);
#endif
        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 4-byte signed integer from the underlying stream and advances the stream by four bytes.
    /// </summary>
    /// <returns>The 4-byte signed integer read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public int ReadLong()
    {
        ValidateDisposed(_isDisposed);

        Span<byte> buffer = stackalloc byte[sizeof(int)];
#if NET6_0
        ReadExactly(_stream, buffer);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(buffer);
#endif
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }

    /// <summary>
    /// Reads a 4-byte fixed-point (16:16) value from the underlying stream and advances the stream by four bytes.
    /// </summary>
    /// <returns>The 4-byte floating point representation of the value read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public float ReadFixed()
    {
        ValidateDisposed(_isDisposed);

        Span<byte> buffer = stackalloc byte[sizeof(int)];
#if NET6_0
        ReadExactly(_stream, buffer);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(buffer);
#endif
        return BinaryPrimitives.ReadInt32LittleEndian(buffer) / 65536.0f;
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the underlying stream and advances the stream by four bytes.
    /// </summary>
    /// <returns>The 4-byte floating point value read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    public float ReadFloat()
    {
        ValidateDisposed(_isDisposed);

        Span<byte> buffer = stackalloc byte[sizeof(float)];
#if NET6_0
        ReadExactly(_stream, buffer);
#elif NET8_0_OR_GREATER
        _stream.ReadExactly(buffer);
#endif
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }

    /// <summary>
    /// Reads an n-length string from the underlying stream.  Strings are prepended by a 2-byte unsigned integer that
    /// specifies the length of the string to read.  When this method returns, the underlying stream will be advanced by
    /// 2+n bytes.
    /// </summary>
    /// <returns>The string read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an exception occurs during the internal call to <see cref="Encoding.GetString(byte[])"/>.  See inner
    /// exception for details.
    /// </exception>
    public string ReadString()
    {
        int len = ReadWord();
        byte[] data = ReadBytes(len);
        try
        {
            return Encoding.UTF8.GetString(data);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An exception occurred while encoding the data as a string.  Please see inner exception for details", ex);
        }
    }

    /// <summary>
    /// Reads a string of the given byte length from the underlying stream and advances the stream by that many bytes.
    /// </summary>
    /// <param name="len">The byte length of the string to read.</param>
    /// <returns>The string read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an exception occurs during the internal call to <see cref="Encoding.GetString(byte[])"/>.  See inner
    /// exception for details.
    /// </exception>
    public string ReadString(int len)
    {
        byte[] data = ReadBytes(len);
        try
        {
            return Encoding.UTF8.GetString(data);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An exception occurred while encoding the data as a string.  Please see inner exception for details", ex);
        }
    }

    /// <summary>
    /// Reads a <typeparamref name="T"/> value from the underlying stream and advances the underlying stream by a number
    /// of bytes equal to <paramref name="structSize"/>.
    /// </summary>
    /// <typeparam name="T">The value type to read from the underlying stream.</typeparam>
    /// <param name="structSize">The exact byte size of an instance of <typeparamref name="T"/></param>
    /// <returns>the <typeparamref name="T"/> read from the underlying stream.</returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throw if <paramref name="structSize"/> is less than zero.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown if the end of stream was reached when attempting to read.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an exception occurs while attempting to map the type ot the <typeparamref name="T"/> struct. See the
    /// inner exception for details.
    /// </exception>
    public T ReadUnsafe<T>(int structSize) where T : struct
    {
        T value;
        Span<byte> data = ReadBytes(structSize);
        try
        {
            unsafe
            {
                fixed (byte* ptr = data)
                {
                    value = Marshal.PtrToStructure<T>((IntPtr)ptr);
                }
            }
            return value;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An exception occurred while reading the type.  Please see inner exception for details", ex);
        }
    }

    /// <summary>
    /// Reads the specified number of compressed bytes from the underlying stream and advances the stream by that
    /// number of bytes.
    /// </summary>
    /// <param name="count">The number of compressed bytes to read from the underlying stream.</param>
    /// <returns>The bytes read and decompressed from the underlying stream.</returns>
    public byte[] ReadCompressed(int count)
    {
        byte[] compressed = ReadBytes(count);
        return Zlib.Deflate(compressed);
    }

    /// <summary>
    /// Ignores the specified number of bytes by advancing the underlying stream by that number of bytes.
    /// </summary>
    /// <param name="count">The number of bytes to ignore.</param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an exception occurs during the internal call to <see cref="Stream.Seek(long, SeekOrigin)"/>.  See
    /// inner exception for details.
    /// </exception>
    public void Ignore(int count) => Seek(count, SeekOrigin.Current);

    /// <summary>
    /// Sets the position of the underlying stream.
    /// </summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter</param>
    /// <param name="origin">
    /// A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance of the <see cref="AsepriteBinaryReader"/> class, or the underlying stream, has been
    /// disposed of prior to calling this method.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an exception occurs during the internal call to <see cref="Stream.Seek(long, SeekOrigin)"/>.  See
    /// inner exception for details.
    /// </exception>
    public void Seek(long offset, SeekOrigin origin)
    {
        ValidateDisposed(_isDisposed);
        try
        {
            _stream.Seek(offset, origin);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An exception occurred while attempting to seek the stream.  Please see inner exception for details", ex);
        }
    }

    /// <summary>
    /// Disposes of resources managed by this instance of the <see cref="AsepriteBinaryReader"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing)
    {
        if (_isDisposed) { return; }

        if (isDisposing && !_leaveOpen)
        {
            _stream.Close();
        }

        _isDisposed = true;
    }

    private static void ValidateCanRead(bool canRead)
    {
        if (!canRead)
        {
            throw new NotSupportedException("The input stream must support reading");
        }
    }

    private static void ValidateCanSeek(bool canSeek)
    {
        if (!canSeek)
        {
            throw new NotSupportedException("The input stream must support seeking");
        }
    }

    private static void ValidateDisposed(bool disposed)
    {
        if (disposed)
        {
            throw new ObjectDisposedException(nameof(AsepriteBinaryReader), $"The {nameof(AsepriteBinaryReader)} has been disposed previously.");
        }
    }

#if NET6_0
    private static void ReadExactly(Stream stream, Span<byte> buffer)
    {
        buffer.Clear();

        int total = 0;
        int read = 0;

        while (total < buffer.Length)
        {
            if ((read = stream.Read(buffer.Slice(total))) == 0)
            {
                throw new EndOfStreamException();
            }

            total += read;
        }
    }
#endif
}
