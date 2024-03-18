// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.IO.Compression;

namespace AsepriteDotNet.Compression;

/// <summary>
/// Utility class for working with zlib compressed data.
/// </summary>
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
