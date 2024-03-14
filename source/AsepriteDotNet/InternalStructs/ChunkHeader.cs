//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct ChunkHeader
{
    internal const int StructSize = sizeof(uint) +  //  ChunkSize
                                    sizeof(ushort); //  ChunkType

    [FieldOffset(0)]
    internal uint ChunkSize;

    [FieldOffset(4)]
    internal ushort ChunkType;
}
