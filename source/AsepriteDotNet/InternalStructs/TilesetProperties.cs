//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct TilesetProperties
{
    internal const int StructSize = sizeof(uint) +          //  Id
                                    sizeof(uint) +          //  Flags
                                    sizeof(uint) +          //  NumberOfTiles
                                    sizeof(ushort) +        //  TileWidth
                                    sizeof(ushort) +        //  TileHeight
                                    sizeof(short) +         //  BaseIndex
                                    (sizeof(byte) * 14) +   //  Reserved
                                    sizeof(ushort);         //  NameLen

    [FieldOffset(0)]
    internal uint Id;

    [FieldOffset(sizeof(uint))]
    internal uint Flags;

    [FieldOffset(sizeof(uint))]
    internal uint NumberOfTiles;

    [FieldOffset(sizeof(uint))]
    internal ushort TileWidth;

    [FieldOffset(sizeof(ushort))]
    internal ushort TileHeight;

    [FieldOffset(sizeof(ushort))]
    internal short BaseIndex;

    [FieldOffset(sizeof(short))]
    internal byte[] Reserved;

    [FieldOffset(sizeof(byte) * 14)]
    internal ushort NameLen;
}
