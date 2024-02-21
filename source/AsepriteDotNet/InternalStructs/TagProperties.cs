//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct TagProperties
{
    internal const int StructSize = sizeof(ushort) +        //  From
                                    sizeof(ushort) +        //  To
                                    sizeof(byte) +          //  Direction
                                    sizeof(ushort) +        //  Repeat
                                    (sizeof(byte) * 6) +    //  Reserved
                                    (sizeof(byte) * 3) +    //  RGB
                                    sizeof(byte) +          //  Ignore
                                    sizeof(ushort);         //  NameLen

    [FieldOffset(0)]
    internal ushort From;

    [FieldOffset(sizeof(ushort))]
    internal ushort To;

    [FieldOffset(sizeof(ushort))]
    internal byte Direction;

    [FieldOffset(sizeof(byte))]
    internal ushort Repeat;

    [FieldOffset(sizeof(ushort))]
    internal byte[] Reserved;

    [FieldOffset(sizeof(byte) * 6)]
    internal byte[] RGB;

    [FieldOffset(sizeof(byte) * 3)]
    internal byte Ignore;

    [FieldOffset(sizeof(byte))]
    internal ushort NameLen;
}
