//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal unsafe struct TagData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<TagData>();

    [FieldOffset(0)]
    internal ushort From;

    [FieldOffset(2)]
    internal ushort To;

    [FieldOffset(4)]
    internal byte Direction;

    [FieldOffset(5)]
    internal ushort Repeat;

    [FieldOffset(7)]
    internal fixed byte Reserved[6];

    [FieldOffset(13)]
    internal byte R;

    [FieldOffset(14)]
    internal byte G;

    [FieldOffset(15)]
    internal byte B;

    [FieldOffset(17)]
    internal ushort NameLen;
}
