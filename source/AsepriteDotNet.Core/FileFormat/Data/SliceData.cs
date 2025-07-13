//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct SliceData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<SliceData>();

    [FieldOffset(0)]
    internal uint KeyCount;

    [FieldOffset(4)]
    internal uint Flags;

    [FieldOffset(8)]
    internal uint Reserved;

    [FieldOffset(12)]
    internal ushort NameLen;

}
