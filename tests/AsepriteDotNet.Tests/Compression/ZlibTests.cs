﻿// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.IO.Compression;
using AsepriteDotNet.Compression;

namespace AsepriteDotNet.Tests.Compression;

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

    private static byte[] GetRandomBytes(int size)
    {
        byte[] data = new byte[size];
        #pragma warning disable CA5394
        Random.Shared.NextBytes(data);
        #pragma warning restore CA5394
        return data;
    }

    private static byte[] CompressBytes(byte[] data)
    {
        using MemoryStream ms = new();
        using ZLibStream zOut = new(ms, CompressionMode.Compress);
        zOut.Write(data);
        zOut.Flush();
        return ms.ToArray();
    }
}
