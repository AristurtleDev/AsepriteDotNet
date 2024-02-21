//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct FrameHeader
{
    public const int StructSize = sizeof(uint) +        //  Length
                                  sizeof(ushort) +      //  MagicNumber
                                  sizeof(ushort) +      //  OldChunkCount
                                  sizeof(ushort) +      //  Duration
                                  (sizeof(byte) * 2) +  //  Reserved
                                  sizeof(uint);         //  NewChunkCount
    [FieldOffset(0)]
    public uint Length;

    [FieldOffset(sizeof(uint))]
    public ushort MagicNumber;

    [FieldOffset(sizeof(ushort))]
    public ushort OldChunkCount;

    [FieldOffset(sizeof(ushort))]
    public ushort Duration;

    [FieldOffset(sizeof(ushort))]
    public byte[] Reserved;

    [FieldOffset(sizeof(byte) * 2)]
    public uint NewChunkCount;
}