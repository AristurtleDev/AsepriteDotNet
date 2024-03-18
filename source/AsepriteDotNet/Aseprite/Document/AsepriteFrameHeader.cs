//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AsepriteFrameHeader
{
    internal const int StructSize = sizeof(uint) +          //  Length
                                    sizeof(ushort) +        //  MagicNumber
                                    sizeof(ushort) +        //  OldChunkCount
                                    sizeof(ushort) +        //  Duration
                                    sizeof(byte) * 2 +    //  Reserved
                                    sizeof(uint);           //  NewChunkCount
    [FieldOffset(0)]
    internal uint Length;

    [FieldOffset(4)]
    internal ushort MagicNumber;

    [FieldOffset(6)]
    internal ushort OldChunkCount;

    [FieldOffset(8)]
    internal ushort Duration;

    //[FieldOffset(10)]
    //internal fixed byte Reserved[2];

    [FieldOffset(12)]
    internal uint NewChunkCount;
}
