//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct LayerProperties
{
    public const int StructSize = sizeof(ushort) +      //  Flags
                                  sizeof(ushort) +      //  Type
                                  sizeof(ushort) +      //  Level
                                  sizeof(ushort) +      //  DefaultWidth
                                  sizeof(ushort) +      //  DefaultHeight
                                  sizeof(ushort) +      //  BlendMode
                                  sizeof(byte) +        //  Opacity
                                  (sizeof(byte) * 3) +  //  Ignore
                                  sizeof(ushort);       //  NameLen


    [FieldOffset(0)]
    public ushort Flags;

    [FieldOffset(sizeof(ushort))]
    public ushort Type;

    [FieldOffset(sizeof(ushort))]
    public ushort Level;

    [FieldOffset(sizeof(ushort))]
    public ushort DefaultWidth;

    [FieldOffset(sizeof(ushort))]
    public ushort DefaultHeight;

    [FieldOffset(sizeof(ushort))]
    public ushort BlendMode;

    [FieldOffset(sizeof(ushort))]
    public byte Opacity;

    [FieldOffset(sizeof(byte))]
    public byte[] Ignore;

    [FieldOffset(sizeof(byte) * 3)]
    public ushort NameLen;
}