//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct TagProperties
{
    public const int StructSize = sizeof(ushort) +      //  From
                                  sizeof(ushort) +      //  To
                                  sizeof(byte) +        //  Direction
                                  sizeof(ushort) +      //  Repeat
                                  (sizeof(byte) * 6) +  //  Reserved
                                  (sizeof(byte) * 3) +  //  RGB
                                  sizeof(byte) +        //  Ignore
                                  sizeof(ushort);       //  NameLen

    [FieldOffset(0)]
    public ushort From;

    [FieldOffset(sizeof(ushort))]
    public ushort To;

    [FieldOffset(sizeof(ushort))]
    public byte Direction;

    [FieldOffset(sizeof(byte))]
    public ushort Repeat;

    [FieldOffset(sizeof(ushort))]
    public byte[] Reserved;

    [FieldOffset(sizeof(byte) * 6)]
    public byte[] RGB;

    [FieldOffset(sizeof(byte) * 3)]
    public byte Ignore;

    [FieldOffset(sizeof(byte))]
    public ushort NameLen;
}