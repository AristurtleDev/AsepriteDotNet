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
using System.Text;

using AsepriteDotNet.IO;

namespace AsepriteDotNet.Tests;

public class AsepriteBinaryReaderTests
{
    [Fact]
    public void AsepriteBinaryReader_DisposeTests()
    {
        //  Multiple dispose should not throw an exception
        using (AsepriteBinaryReader reader = new(new MemoryStream()))
        {
            reader.Dispose();
            reader.Dispose();
            reader.Dispose();
        }
    }

    [Fact]
    public void AsepriteBinaryReader_DisposeTest_Negative()
    {
        using (AsepriteBinaryReader reader = new(new MemoryStream()))
        {
            reader.Dispose();
            ValidateDisposedExceptions(reader);
        }
    }

    [Fact]
    public void AsepriteBinaryReader_EndOfStreamTest_Negative()
    {
        ValidateEndOfStreamException(writer => writer.Write(byte.MinValue), reader => reader.ReadByte());
        ValidateEndOfStreamException(writer => writer.Write(byte.MaxValue), reader => reader.ReadByte());
        ValidateEndOfStreamException(writer => writer.Write(ushort.MinValue), reader => reader.ReadWord());
        ValidateEndOfStreamException(writer => writer.Write(ushort.MaxValue), reader => reader.ReadWord());
        ValidateEndOfStreamException(writer => writer.Write(short.MinValue), reader => reader.ReadShort());
        ValidateEndOfStreamException(writer => writer.Write(short.MaxValue), reader => reader.ReadShort());
        ValidateEndOfStreamException(writer => writer.Write(uint.MinValue), reader => reader.ReadDword());
        ValidateEndOfStreamException(writer => writer.Write(uint.MaxValue), reader => reader.ReadDword());
        ValidateEndOfStreamException(writer => writer.Write(int.MinValue), reader => reader.ReadLong());
        ValidateEndOfStreamException(writer => writer.Write(int.MaxValue), reader => reader.ReadLong());
        ValidateEndOfStreamException(writer => writer.Write(float.MinValue), reader => reader.ReadFixed());
        ValidateEndOfStreamException(writer => writer.Write(float.MaxValue), reader => reader.ReadFixed());
        ValidateEndOfStreamException(writer => WriteValidAsepriteString(writer, string.Empty), reader => reader.ReadString());
        ValidateEndOfStreamException(writer => WriteValidAsepriteString(writer, "Hello World"), reader => reader.ReadString());

    }

    [Fact]
    public void AsepriteBinaryReader_ReadByte()
    {
        ValidateRead(writer => writer.Write(byte.MinValue), reader => reader.ReadByte(), byte.MinValue);
        ValidateRead(writer => writer.Write(byte.MaxValue), reader => reader.ReadByte(), byte.MaxValue);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadBytes()
    {
        byte[] expected = new byte[] { 0x0000, 0x0001, 0x0002, 0x0003 };

        ValidateRead(writer =>
        {
            for (int i = 0; i < expected.Length; i++)
            {
                writer.Write(expected[i]);
            }
        }, reader => reader.ReadBytes(expected.Length), expected);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadWord()
    {
        ValidateRead(writer => writer.Write(ushort.MinValue), reader => reader.ReadWord(), ushort.MinValue);
        ValidateRead(writer => writer.Write(ushort.MaxValue), reader => reader.ReadWord(), ushort.MaxValue);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadShort()
    {
        ValidateRead(writer => writer.Write(short.MinValue), reader => reader.ReadShort(), short.MinValue);
        ValidateRead(writer => writer.Write(short.MaxValue), reader => reader.ReadShort(), short.MaxValue);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadDword()
    {
        ValidateRead(writer => writer.Write(uint.MinValue), reader => reader.ReadDword(), uint.MinValue);
        ValidateRead(writer => writer.Write(uint.MaxValue), reader => reader.ReadDword(), uint.MaxValue);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadLong()
    {
        ValidateRead(writer => writer.Write(int.MinValue), reader => reader.ReadLong(), int.MinValue);
        ValidateRead(writer => writer.Write(int.MaxValue), reader => reader.ReadLong(), int.MaxValue);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadFixed()
    {
        ValidateRead(writer => writer.Write(float.MinValue), reader => reader.ReadFixed(), float.MinValue);
        ValidateRead(writer => writer.Write(float.MaxValue), reader => reader.ReadFixed(), float.MaxValue);
    }

    [Fact]
    public void AsepriteBinaryReader_ReadString()
    {
        string expected = "hello world";
        ValidateRead(writer => WriteValidAsepriteString(writer, expected), reader => reader.ReadString(), expected);
    }

    [Fact]
    public void AsepriteBinaryReader_Skip()
    {
        byte expected = 0x0004;
        byte[] buffer = new byte[] { 0x0000, expected };
        MemoryStream stream = new(buffer);

        using AsepriteBinaryReader reader = new(stream);
        reader.Skip(1);
        byte actual = reader.ReadByte();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AsepriteBinaryReader_Seek()
    {
        byte expected = 0x0004;
        byte[] buffer = new byte[] { 0x0000, expected };
        MemoryStream stream = new(buffer);

        using AsepriteBinaryReader reader = new(stream);
        reader.Seek(1);
        byte actual = reader.ReadByte();
        Assert.Equal(expected, actual);
    }

    private void ValidateRead<T>(Action<BinaryWriter> writeAction, Func<AsepriteBinaryReader, T> readAction, T expected)
    {
        UTF8Encoding encoding = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        MemoryStream stream = new();

        //  Write value once to stream.
        BinaryWriter writer = new(stream, encoding, leaveOpen: true);
        writeAction(writer);

        //  Stream will be left open
        writer.Close();

        //  Ensure the stream was populated.
        Assert.True(stream.Length > 0);

        //  Reset stream position
        stream.Position = 0;

        //  Read the value back
        using AsepriteBinaryReader reader = new(stream);
        T actual = readAction(reader);
        Assert.Equal(expected, actual);
        Assert.IsType<T>(actual);
    }

    private void WriteValidAsepriteString(BinaryWriter writer, string s)
    {
        UTF8Encoding encoding = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        byte[] arr = encoding.GetBytes(s);
        writer.Write((ushort)arr.Length);

        for (int i = 0; i < arr.Length; i++)
        {
            writer.Write(arr[i]);
        }
    }

    private void ValidateDisposedExceptions(AsepriteBinaryReader reader)
    {
        Assert.Throws<ObjectDisposedException>(() => reader.ReadByte());
        Assert.Throws<ObjectDisposedException>(() => reader.ReadBytes(1));
        Assert.Throws<ObjectDisposedException>(() => reader.ReadWord());
        Assert.Throws<ObjectDisposedException>(() => reader.ReadShort());
        Assert.Throws<ObjectDisposedException>(() => reader.ReadDword());
        Assert.Throws<ObjectDisposedException>(() => reader.ReadLong());
        Assert.Throws<ObjectDisposedException>(() => reader.ReadFixed());
        Assert.Throws<ObjectDisposedException>(() => reader.ReadString());
        Assert.Throws<ObjectDisposedException>(() => reader.Skip(1));
        Assert.Throws<ObjectDisposedException>(() => reader.Seek(0));
    }

    private void ValidateEndOfStreamException(Action<BinaryWriter> writeAction, Action<AsepriteBinaryReader> readAction)
    {
        UTF8Encoding encoding = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        MemoryStream stream = new();

        //  Write twice to the memory stream.
        BinaryWriter writer = new(stream, encoding, leaveOpen: true);
        writeAction(writer);
        writeAction(writer);
        writer.Close(); //  Stream will be left open

        //  Ensure that the stream was populated
        Assert.True(stream.Length > 0);

        //  Reset the position within the stream
        stream.Position = 0;

        //  Truncate the last byte
        stream.SetLength(stream.Length - 1);

        using AsepriteBinaryReader reader = new(stream);

        //  First one should always succeed.
        readAction(reader);

        //  Second one should fail since we truncated the last byte
        Assert.Throws<EndOfStreamException>(() => readAction(reader));
    }
}