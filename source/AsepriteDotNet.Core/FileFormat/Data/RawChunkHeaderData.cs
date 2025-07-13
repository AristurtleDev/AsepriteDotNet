//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct ChunkHeaderData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<ChunkHeaderData>();

    [FieldOffset(0)]
    internal uint ChunkSize;

    [FieldOffset(4)]
    internal ushort ChunkType;
}
