//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepritePaletteEntry
{
    internal const int StructSize = sizeof(ushort) +    //  NewSize
                                    sizeof(byte) +      //  R
                                    sizeof(byte) +      //  G
                                    sizeof(byte) +      //  B
                                    sizeof(byte);       //  A

    [FieldOffset(0)]
    internal ushort Flags;

    [FieldOffset(2)]
    internal byte R;

    [FieldOffset(3)]
    internal byte G;

    [FieldOffset(4)]
    internal byte B;

    [FieldOffset(5)]
    internal byte A;
}
