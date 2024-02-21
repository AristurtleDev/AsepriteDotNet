//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct PaletteEntry
{
    public const int StructSize = sizeof(ushort) +      //  NewSize
                                  AseColor.StructSize;  //  FirstIndex

    [FieldOffset(0)]
    public ushort Flags;

    [FieldOffset(sizeof(uint))]
    public AseColor Color;    
}