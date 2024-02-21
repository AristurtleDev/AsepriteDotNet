//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct FileHeader
{
    public const int StructSize = 128;

    [FieldOffset(0)]
    public uint FileSize;

    [FieldOffset(sizeof(uint))]
    public ushort MagicNumber;

    [FieldOffset(sizeof(ushort))]
    public ushort FrameCount;

    [FieldOffset(sizeof(ushort))]
    public ushort CanvasWidth;

    [FieldOffset(sizeof(ushort))]
    public ushort CanvasHeight;

    [FieldOffset(sizeof(ushort))]
    public ushort Depth;

    [FieldOffset(sizeof(ushort))]
    public uint Flags;

    [FieldOffset(sizeof(uint))]
    public ushort Speed;

    [FieldOffset(sizeof(ushort))]
    public uint Ignore1;

    [FieldOffset(sizeof(uint))]
    public uint Ignore2;

    [FieldOffset(sizeof(uint))]
    public byte TransparentIndex;

    [FieldOffset(sizeof(byte))]
    public byte[] Ignore3;

    [FieldOffset(sizeof(byte) * 3)]
    public ushort NumberOfColors;

    [FieldOffset(sizeof(ushort))]
    public byte PixelWidth;

    [FieldOffset(sizeof(byte))]
    public byte PixelHeight;

    [FieldOffset(sizeof(byte))]
    public short GridXPosition;

    [FieldOffset(sizeof(short))]
    public short GridYPosition;

    [FieldOffset(sizeof(short))]
    public ushort GridWidth;

    [FieldOffset(sizeof(ushort))]
    public ushort GridHeight;

    [FieldOffset(sizeof(ushort))]
    public byte[] Ignore4;
}