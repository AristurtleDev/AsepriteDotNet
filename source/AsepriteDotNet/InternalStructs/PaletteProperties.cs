//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct PaletteProperties
{
    internal const int StructSize = sizeof(uint) +      //  NewSize
                                    sizeof(uint) +      //  FirstIndex
                                    sizeof(uint) +      //  LastIndex
                                    (sizeof(byte) * 8); //  Reserved

    [FieldOffset(0)]
    internal uint NewSize;

    [FieldOffset(sizeof(uint))]
    internal uint FirstIndex;

    [FieldOffset(sizeof(uint))]
    internal uint LastIndex;

    [FieldOffset(sizeof(uint))]
    internal byte[] Reserved;
}
