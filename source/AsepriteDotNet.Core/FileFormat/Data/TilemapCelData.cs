//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct TilemapCelData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<TilemapCelData>();

    [FieldOffset(0)]
    internal ushort Width;

    [FieldOffset(2)]
    internal ushort Height;

    [FieldOffset(4)]
    internal ushort BitsPerTile;

    [FieldOffset(6)]
    internal uint TileIdBitmask;

    [FieldOffset(10)]
    internal uint VerticalFlipBitmask;

    [FieldOffset(14)]
    internal uint HorizontalFlipBitmask;

    [FieldOffset(18)]
    internal uint DiagonalFlipBitmask;

    [FieldOffset(22)]
    internal fixed byte Reserved[10];
}
