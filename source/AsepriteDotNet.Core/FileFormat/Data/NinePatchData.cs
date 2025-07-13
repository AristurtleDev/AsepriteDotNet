//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct NinePatchData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<NinePatchData>();

    [FieldOffset(0)]
    internal int X;

    [FieldOffset(4)]
    internal int Y;

    [FieldOffset(8)]
    internal uint Width;

    [FieldOffset(12)]
    internal uint Height;
}
