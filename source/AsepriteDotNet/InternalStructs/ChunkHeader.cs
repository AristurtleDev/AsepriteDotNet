//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct ChunkHeader
{
    public const int StructSize = sizeof(uint) +    //  ChunkSize
                                  sizeof(ushort);   //  ChunkType

    [FieldOffset(0)]
    public uint ChunkSize;

    [FieldOffset(sizeof(uint))]
    public ushort ChunkType;
}