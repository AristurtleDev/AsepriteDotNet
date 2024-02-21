//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct TilemapCelProperties
{
    internal const int StructSize = sizeof(ushort) +        //  Width
                                    sizeof(ushort) +        //  Height
                                    sizeof(ushort) +        //  BitsPerTile
                                    sizeof(uint) +          //  TileIdBitmask
                                    sizeof(uint) +          //  VerticalFlipBitmask
                                    sizeof(uint) +          //  HorizontalFlipBitmask
                                    sizeof(uint) +          //  DiagonalFlipBitmask
                                    (sizeof(byte) * 10);    //  Reserved


    [FieldOffset(0)]
    internal ushort Width;

    [FieldOffset(sizeof(ushort))]
    internal ushort Height;

    [FieldOffset(sizeof(ushort))]
    internal ushort BitsPerTile;

    [FieldOffset(sizeof(ushort))]
    internal uint TileIdBitmask;

    [FieldOffset(sizeof(uint))]
    internal uint VerticalFlipBitmask;

    [FieldOffset(sizeof(uint))]
    internal uint HorizontalFlipBitmask;

    [FieldOffset(sizeof(uint))]
    internal uint DiagonalFlipBitmask;

    [FieldOffset(sizeof(uint))]
    internal byte[] Reserved;
}
