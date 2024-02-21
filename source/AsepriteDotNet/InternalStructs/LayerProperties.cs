//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct LayerProperties
{
    internal const int StructSize = sizeof(ushort) +        //  Flags
                                    sizeof(ushort) +        //  Type
                                    sizeof(ushort) +        //  Level
                                    sizeof(ushort) +        //  DefaultWidth
                                    sizeof(ushort) +        //  DefaultHeight
                                    sizeof(ushort) +        //  BlendMode
                                    sizeof(byte) +          //  Opacity
                                    (sizeof(byte) * 3) +    //  Ignore
                                    sizeof(ushort);         //  NameLen


    [FieldOffset(0)]
    internal ushort Flags;

    [FieldOffset(sizeof(ushort))]
    internal ushort Type;

    [FieldOffset(sizeof(ushort))]
    internal ushort Level;

    [FieldOffset(sizeof(ushort))]
    internal ushort DefaultWidth;

    [FieldOffset(sizeof(ushort))]
    internal ushort DefaultHeight;

    [FieldOffset(sizeof(ushort))]
    internal ushort BlendMode;

    [FieldOffset(sizeof(ushort))]
    internal byte Opacity;

    [FieldOffset(sizeof(byte))]
    internal byte[] Ignore;

    [FieldOffset(sizeof(byte) * 3)]
    internal ushort NameLen;
}
