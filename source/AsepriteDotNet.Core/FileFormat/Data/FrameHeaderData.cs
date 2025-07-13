//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal unsafe struct FrameHeaderData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<FrameHeaderData>();

    [FieldOffset(0)]
    internal uint Length;

    [FieldOffset(4)]
    internal ushort MagicNumber;

    [FieldOffset(6)]
    internal ushort OldChunkCount;

    [FieldOffset(8)]
    internal ushort Duration;

    [FieldOffset(10)]
    internal fixed byte Reserved[2];

    [FieldOffset(12)]
    internal uint NewChunkCount;
}
