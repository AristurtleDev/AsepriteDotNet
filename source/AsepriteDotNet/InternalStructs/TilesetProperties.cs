//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct TilesetProperties
{
    public const int StructSize = sizeof(uint) +        //  Id
                                  sizeof(uint) +        //  Flags
                                  sizeof(uint) +        //  NumberOfTiles
                                  sizeof(ushort) +      //  TileWidth
                                  sizeof(ushort) +      //  TileHeight
                                  sizeof(short) +       //  BaseIndex
                                  (sizeof(byte) * 14) + //  Reserved
                                  sizeof(ushort);       //  NameLen

    [FieldOffset(0)]
    public uint Id;

    [FieldOffset(sizeof(uint))]
    public uint Flags;

    [FieldOffset(sizeof(uint))]
    public uint NumberOfTiles;

    [FieldOffset(sizeof(uint))]
    public ushort TileWidth;

    [FieldOffset(sizeof(ushort))]
    public ushort TileHeight;

    [FieldOffset(sizeof(ushort))]
    public short BaseIndex;

    [FieldOffset(sizeof(short))]
    public byte[] Reserved;

    [FieldOffset(sizeof(byte) * 14)]
    public ushort NameLen;


}
