//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.FileFormat.Data;

[StructLayout(LayoutKind.Explicit)]
internal struct PaletteEntryData
{
    internal static readonly int SizeInBytes = Marshal.SizeOf<PaletteEntryData>();

    [FieldOffset(0)]
    internal ushort Flags;

    [FieldOffset(2)]
    internal Rgba32 Color;
}
