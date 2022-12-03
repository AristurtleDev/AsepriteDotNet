/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.IO.Compression;

namespace AsepriteDotNet.Compression;

internal static class Zlib
{
    public static byte[] Deflate(byte[] buffer)
    {
        using MemoryStream compressedStream = new(buffer);

        //  First 2 bytes are the zlib header information, skip past them.
        _ = compressedStream.ReadByte();
        _ = compressedStream.ReadByte();

        using MemoryStream decompressedStream = new();
        using DeflateStream deflateStream = new(compressedStream, CompressionMode.Decompress);
        deflateStream.CopyTo(decompressedStream);
        return decompressedStream.ToArray();
    }
}