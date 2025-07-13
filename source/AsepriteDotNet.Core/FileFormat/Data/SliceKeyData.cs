//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct SliceKeyData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<SliceKeyData>();

    [FieldOffset(0)]
    internal uint FrameNumber;

    [FieldOffset(4)]
    internal int X;

    [FieldOffset(8)]
    internal int Y;

    [FieldOffset(12)]
    internal uint Width;

    [FieldOffset(16)]
    internal uint Height;
}
