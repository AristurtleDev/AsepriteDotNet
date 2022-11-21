/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
using System.Buffers.Binary;

namespace AsepriteDotNet.IO;

/// <summary>
///     A low-level binary reader designed to read data types from an
///     Aseprite file.
/// </summary>
public sealed class AsepriteBinaryReader : IDisposable
{
    private Stream _stream;
    private bool _isDisposed = false;
    private readonly byte[] _buffer = new byte[16];

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsepriteBinaryReader"/>
    ///     class.
    /// </summary>
    /// <param name="input">
    ///     The input stream to read from. Must support reading and seeking.
    /// </param>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the specified <see cref="Stream"/> does not support
    ///     reading or seeking.
    /// </exception>
    public AsepriteBinaryReader(Stream input)
    {
        if (!input.CanRead)
        {
            throw new NotSupportedException("The input stream must supported reading");
        }

        if (!input.CanSeek)
        {
            throw new NotSupportedException("The input stream must support seeking");
        }

        _stream = input;
    }

    ~AsepriteBinaryReader() => Dispose(false);

    /// <summary>
    ///     Reads the next byte from the underlying stream and advances the
    ///     stream by one byte.
    /// </summary>
    /// <returns>
    ///     The next byte read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    public byte ReadByte()
    {
        ThrowIfDisposed();

        int b = _stream.ReadByte();

        if (b == -1)
        {
            throw new EndOfStreamException("The end of stream was reached");
        }

        return (byte)b;

    }

    /// <summary>
    ///     Reads the specified number of bytes from the underlying stream and
    ///     advances the stream by that number of bytes.
    /// </summary>
    /// <param name="nbytes">
    ///     The number of bytes to read from the underlying stream.
    /// </param>
    /// <returns>
    ///     A new byte array containing the bytes read from the underlying
    ///     stream.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified number of bytes to read is less than zero.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the the end of stream is reached before reading the
    ///     specified number of bytes.
    /// </exception>
    public byte[] ReadBytes(int nbytes)
    {
        if (nbytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nbytes), "{nameof(count)} must be greater then 0");
        }

        ThrowIfDisposed();

        if (nbytes == 0)
        {
            return Array.Empty<byte>();
        }

        byte[] result = new byte[nbytes];

        _stream.ReadExactly(result, 0, nbytes);

        return result;
    }

    /// <summary>
    ///     Reads a 2-byte unsigned integer from the underlying stream and
    ///     advances the stream by two bytes.
    /// </summary>
    /// <returns>
    ///     The 2-byte unsigned integer read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the end of stream is reach while attempting to read the
    ///     data type.
    /// </exception>
    public ushort ReadWord()
    {
        ThrowIfDisposed();
        _stream.ReadExactly(_buffer.AsSpan(0, 2));
        return BinaryPrimitives.ReadUInt16LittleEndian(_buffer);

    }


    /// <summary>
    ///     Reads a 2-byte signed integer from the underlying stream and
    ///     advances the stream by two bytes.
    /// </summary>
    /// <returns>
    ///     The 2-byte signed integer read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the end of stream is reach while attempting to read the
    ///     data type.
    /// </exception>
    public short ReadShort()
    {
        ThrowIfDisposed();
        _stream.ReadExactly(_buffer.AsSpan(0, 2));
        return BinaryPrimitives.ReadInt16LittleEndian(_buffer);
    }

    /// <summary>
    ///     Reads a 4-byte unsigned integer from the underlying stream and
    ///     advances the stream by four bytes.
    /// </summary>
    /// <returns>
    ///     The 4-byte unsigned integer read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the end of stream is reach while attempting to read the
    ///     data type.
    /// </exception>
    public uint ReadDword()
    {
        ThrowIfDisposed();
        _stream.ReadExactly(_buffer.AsSpan(0, 4));
        return BinaryPrimitives.ReadUInt32LittleEndian(_buffer);
    }

    /// <summary>
    ///     Reads a 4-byte signed integer from the underlying stream and
    ///     advances the stream by four bytes.
    /// </summary>
    /// <returns>
    ///     The 4-byte signed integer read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the end of stream is reach while attempting to read the
    ///     data type.
    /// </exception>
    public int ReadLong()
    {
        ThrowIfDisposed();
        _stream.ReadExactly(_buffer.AsSpan(0, 4));
        return BinaryPrimitives.ReadInt32LittleEndian(_buffer);
    }

    /// <summary>
    ///     Reads a 4-byte floating point integer from the underlying stream and
    ///     advances the stream by four bytes.
    /// </summary>
    /// <returns>
    ///     The 4-byte floating point integer read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the end of stream is reach while attempting to read the
    ///     data type.
    /// </exception>
    public float ReadFixed()
    {
        ThrowIfDisposed();
        _stream.ReadExactly(_buffer.AsSpan(0, 4));
        return BinaryPrimitives.ReadSingleLittleEndian(_buffer);
    }

    /// <summary>
    ///     Reads an n-length string from the underlying stream.  Strings are
    ///     prepended by a 2-byte unsigned integer that specifies the length of
    ///     the string to read.  When this method returns, the underlying stream
    ///     will be advances by 2+len bytes.
    /// </summary>
    /// <returns>
    ///     The string read from the underlying stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    ///     Thrown if the end of stream is reach while attempting to read the
    ///     data type.
    /// </exception>
    /// <exception cref="EncodingException">
    ///     Thrown if an exception occurs while attempting to encode the data
    ///     as a string.  See inner exception for details.
    /// </exception>
    public string ReadString()
    {
        int sLen = ReadWord();
        byte[] sBytes = ReadBytes(sLen);

        try
        {
            return System.Text.Encoding.UTF8.GetString(sBytes);
        }
        catch (Exception ex)
        {
            throw new Exception("An exception occured while encoding the data as a string. Please see inner exception for details", ex);
        }
    }

    /// <summary>
    ///     Skips the specified number of bytes by advancing the underlying
    ///     stream by that number of bytes.
    /// </summary>
    /// <param name="nbytes">
    ///     The total number of bytes to skip.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified number of bytes is a negative number.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    public void Skip(int nbytes)
    {
        if (nbytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nbytes), $"{nameof(nbytes)} must be a non-negative number");
        }

        ThrowIfDisposed();
        _stream.Seek(_stream.Position + nbytes, SeekOrigin.Begin);
    }

    /// <summary>
    ///     Sets the position of the underlying stream to the specified
    ///     position.
    /// </summary>
    /// <param name="position">
    ///     The position to set the underlying stream to.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified position is a negative number.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/> 
    ///     class, or the underlying stream, has been disposed of prior to 
    ///     calling this method.
    /// </exception>
    public void Seek(long position)
    {
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), $"{nameof(position)} must be a non-negative number");
        }

        ThrowIfDisposed();
        _stream.Position = position;
    }

    /// <summary>
    ///     Throws a new instance of the <see cref="ObjectDisposedException"/>
    ///     class if this instance of the <see cref="AsepriteBinaryReader"/>
    ///     class has been disposed of.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown if this instance of the <see cref="AsepriteBinaryReader"/>
    ///     class has been disposed of.
    /// </exception>
    private void ThrowIfDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(null, $"Cannot access {nameof(AsepriteBinaryReader)}, it has already been disposed of");
        }
    }

    /// <summary>
    ///     Disposes of resources managed by this instance of the 
    ///     <see cref="AsepriteBinaryReader"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            _stream.Close();
        }

        _isDisposed = true;
    }
}