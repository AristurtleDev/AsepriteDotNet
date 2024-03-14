//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct SliceProperties
{
    internal const int StructSize = sizeof(uint) +  //  KeyCount
                                    sizeof(uint) +  //  Flags
                                    sizeof(uint) +  //  Reserved
                                    sizeof(ushort); //  NameLen

    [FieldOffset(0)]
    internal uint KeyCount;

    [FieldOffset(4)]
    internal uint Flags;

    [FieldOffset(8)]
    internal uint Reserved;

    [FieldOffset(12)]
    internal ushort NameLen;

}
