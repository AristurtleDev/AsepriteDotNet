//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct FrameHeader
{
    internal const int StructSize = sizeof(uint) +          //  Length
                                    sizeof(ushort) +        //  MagicNumber
                                    sizeof(ushort) +        //  OldChunkCount
                                    sizeof(ushort) +        //  Duration
                                    (sizeof(byte) * 2) +    //  Reserved
                                    sizeof(uint);           //  NewChunkCount
    [FieldOffset(0)]
    internal uint Length;

    [FieldOffset(sizeof(uint))]
    internal ushort MagicNumber;

    [FieldOffset(sizeof(ushort))]
    internal ushort OldChunkCount;

    [FieldOffset(sizeof(ushort))]
    internal ushort Duration;

    [FieldOffset(sizeof(ushort))]
    internal byte[] Reserved;

    [FieldOffset(sizeof(byte) * 2)]
    internal uint NewChunkCount;
}
