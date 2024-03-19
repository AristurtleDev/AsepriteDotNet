//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AsepritePaletteProperties
{
    internal const int StructSize = sizeof(uint) +      //  NewSize
                                    sizeof(uint) +      //  FirstIndex
                                    sizeof(uint) +      //  LastIndex
                                    sizeof(byte) * 8; //  Reserved

    [FieldOffset(0)]
    internal uint NewSize;

    [FieldOffset(4)]
    internal uint FirstIndex;

    [FieldOffset(8)]
    internal uint LastIndex;

    //[FieldOffset(12)]
    //internal fixed byte Reserved[8];
}
