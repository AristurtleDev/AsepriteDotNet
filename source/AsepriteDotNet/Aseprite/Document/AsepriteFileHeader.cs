//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AsepriteFileHeader
{
    public const int StructSize = 128;

    [FieldOffset(0)]
    public uint FileSize;

    [FieldOffset(4)]
    public ushort MagicNumber;

    [FieldOffset(6)]
    public ushort FrameCount;

    [FieldOffset(8)]
    public ushort CanvasWidth;

    [FieldOffset(10)]
    public ushort CanvasHeight;

    [FieldOffset(12)]
    public ushort Depth;

    [FieldOffset(14)]
    public uint Flags;

    [FieldOffset(18)]
    public ushort Speed;

    //[FieldOffset(20)]
    //public uint Reserved1;

    //[FieldOffset(24)]
    //public uint Reserved2;

    [FieldOffset(28)]
    public byte TransparentIndex;

    //[FieldOffset(29)]
    //public fixed byte IgnoredBytes[3];

    [FieldOffset(32)]
    public ushort NumberOfColors;

    [FieldOffset(34)]
    public byte PixelWidth;

    [FieldOffset(35)]
    public byte PixelHeight;

    [FieldOffset(36)]
    public short GridX;

    [FieldOffset(38)]
    public short GridY;

    [FieldOffset(40)]
    public ushort GridWidth;

    [FieldOffset(42)]
    public ushort GridHeight;

    //[FieldOffset(44)]
    //public fixed byte FutureBytes[84];
}
