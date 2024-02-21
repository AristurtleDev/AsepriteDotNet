//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct FileHeader
{
    internal const int StructSize = 128;

    [FieldOffset(0)]
    internal uint FileSize;

    [FieldOffset(sizeof(uint))]
    internal ushort MagicNumber;

    [FieldOffset(sizeof(ushort))]
    internal ushort FrameCount;

    [FieldOffset(sizeof(ushort))]
    internal ushort CanvasWidth;

    [FieldOffset(sizeof(ushort))]
    internal ushort CanvasHeight;

    [FieldOffset(sizeof(ushort))]
    internal ushort Depth;

    [FieldOffset(sizeof(ushort))]
    internal uint Flags;

    [FieldOffset(sizeof(uint))]
    internal ushort Speed;

    [FieldOffset(sizeof(ushort))]
    internal uint Ignore1;

    [FieldOffset(sizeof(uint))]
    internal uint Ignore2;

    [FieldOffset(sizeof(uint))]
    internal byte TransparentIndex;

    [FieldOffset(sizeof(byte))]
    internal byte[] Ignore3;

    [FieldOffset(sizeof(byte) * 3)]
    internal ushort NumberOfColors;

    [FieldOffset(sizeof(ushort))]
    internal byte PixelWidth;

    [FieldOffset(sizeof(byte))]
    internal byte PixelHeight;

    [FieldOffset(sizeof(byte))]
    internal short GridXPosition;

    [FieldOffset(sizeof(short))]
    internal short GridYPosition;

    [FieldOffset(sizeof(short))]
    internal ushort GridWidth;

    [FieldOffset(sizeof(ushort))]
    internal ushort GridHeight;

    [FieldOffset(sizeof(ushort))]
    internal byte[] Ignore4;
}
