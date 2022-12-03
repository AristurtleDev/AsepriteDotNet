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
using System.IO.Compression;

using AsepriteDotNet.Compression;

namespace AsepriteDotNet.Tests;

public class ZlibTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(1000)]
    public void Zlib_DeflateTest(int size)
    {
        byte[] expected = GetRandomBytes(size);
        byte[] compressed = CompressBytes(expected);
        byte[] decompressed = Zlib.Deflate(compressed);
        string s = System.Text.Encoding.UTF8.GetString(decompressed);

        Assert.Equal(expected, decompressed);
    }

    private byte[] GetRandomBytes(int size)
    {
        byte[] data = new byte[size];
        Random.Shared.NextBytes(data);
        return data;
    }

    private byte[] CompressBytes(byte[] data)
    {
        using MemoryStream ms = new();
        using ZLibStream zOut = new(ms, CompressionMode.Compress);
        zOut.Write(data);
        zOut.Flush();
        return ms.ToArray();
    }   
}