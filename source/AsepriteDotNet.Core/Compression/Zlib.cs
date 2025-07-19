// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.IO.Compression;

namespace AsepriteDotNet.Core.Compression;

/// <summary>
/// Provides ZLIB decompression.
/// </summary>
internal static class Zlib
{
    /// <summary>
    /// Decompresses ZLIB-compressed data using the DEFLATE algorithm.
    /// </summary>
    /// <param name="buffer">The compressed data buffer containing ZLIB-formatted bytes.</param>
    /// <returns>A new byte array containing the decompressed data.</returns>
    public static byte[] Deflate(byte[] buffer)
    {
        using MemoryStream compressedStream = new(buffer);
        using MemoryStream decompressedStream = new();
        using ZLibStream zlibStream = new(compressedStream, CompressionMode.Decompress);
        zlibStream.CopyTo(decompressedStream);

        return decompressedStream.ToArray();
    }
}
