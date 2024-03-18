//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepritePaletteEntry
{
    internal const int StructSize = sizeof(ushort) +        //  NewSize
                                    Rgba32.StructSize;    //  FirstIndex

    [FieldOffset(0)]
    internal ushort Flags;

    [FieldOffset(2)]
    internal Rgba32 Color;
}
