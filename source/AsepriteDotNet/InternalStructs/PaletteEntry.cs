//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct PaletteEntry
{
    internal const int StructSize = sizeof(ushort) +        //  NewSize
                                    AseColor.StructSize;    //  FirstIndex

    [FieldOffset(0)]
    internal ushort Flags;

    [FieldOffset(sizeof(uint))]
    internal AseColor Color;
}
