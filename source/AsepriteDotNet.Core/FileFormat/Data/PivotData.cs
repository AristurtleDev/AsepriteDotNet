//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct PivotData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<PivotData>();

    [FieldOffset(0)]
    internal int X;

    [FieldOffset(4)]
    internal int Y;
}
