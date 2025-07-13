//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal unsafe struct CelHeaderData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<CelHeaderData>();

    [FieldOffset(0)]
    internal ushort LayerIndex;

    [FieldOffset(2)]
    internal short X;

    [FieldOffset(4)]
    internal short Y;

    [FieldOffset(6)]
    internal byte Opacity;

    [FieldOffset(7)]
    internal ushort Type;

    [FieldOffset(9)]
    internal short ZIndex;

    [FieldOffset(11)]
    internal fixed byte Reserved[5];
}
