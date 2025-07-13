//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal unsafe struct TilesetData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<TilesetData>();

    [FieldOffset(0)]
    internal uint Id;

    [FieldOffset(4)]
    internal uint Flags;

    [FieldOffset(8)]
    internal uint NumberOfTiles;

    [FieldOffset(12)]
    internal ushort TileWidth;

    [FieldOffset(14)]
    internal ushort TileHeight;

    [FieldOffset(16)]
    internal short BaseIndex;

    [FieldOffset(18)]
    internal fixed byte Reserved[14];

    [FieldOffset(32)]
    internal ushort NameLen;
}
