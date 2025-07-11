// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.IO.Compression;

namespace AsepriteDotNet.Core.Compression;

/// <summary>
/// Utility class for working with zlib compressed data.
/// </summary>
internal static class Zlib
{
    public static byte[] Deflate(byte[] buffer)
    {
        using MemoryStream compressedStream = new(buffer);
        using MemoryStream decompressedStream = new();
        using ZLibStream zlibStream = new(compressedStream, CompressionMode.Decompress);
        zlibStream.CopyTo(decompressedStream);

        return decompressedStream.ToArray();
    }
}
