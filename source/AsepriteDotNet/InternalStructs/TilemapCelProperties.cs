//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct TilemapCelProperties
{
    public const int SizeOf = sizeof(ushort) +      //  Width
                              sizeof(ushort) +      //  Height
                              sizeof(ushort) +      //  BitsPerTile
                              sizeof(uint) +        //  TileIdBitmask
                              sizeof(uint) +        //  VerticalFlipBitmask
                              sizeof(uint) +        //  HorizontalFlipBitmask
                              sizeof(uint) +        //  DiagonalFlipBitmask
                              (sizeof(byte) * 10);  //  Reserved


    [FieldOffset(0)]
    public ushort Width;

    [FieldOffset(sizeof(ushort))]
    public ushort Height;

    [FieldOffset(sizeof(ushort))]
    public ushort BitsPerTile;

    [FieldOffset(sizeof(ushort))]
    public uint TileIdBitmask;

    [FieldOffset(sizeof(uint))]
    public uint VerticalFlipBitmask;

    [FieldOffset(sizeof(uint))]
    public uint HorizontalFlipBitmask;

    [FieldOffset(sizeof(uint))]
    public uint DiagonalFlipBitmask;

    [FieldOffset(sizeof(uint))]
    public byte[] Reserved;
}