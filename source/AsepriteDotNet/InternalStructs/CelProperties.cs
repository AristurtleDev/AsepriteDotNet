//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct CelProperties
{
    public const int StructSize = sizeof(ushort) +      //  LayerIndex
                                  sizeof(short) +       //  X
                                  sizeof(short) +       //  Y
                                  sizeof(byte) +        //  Opacity
                                  sizeof(ushort) +      //  Type
                                  sizeof(short) +       //  ZIndex
                                  (sizeof(byte) * 5);   //  Reserved


    [FieldOffset(0)]
    public ushort LayerIndex;

    [FieldOffset(sizeof(ushort))]
    public short X;

    [FieldOffset(sizeof(short))]
    public short Y;

    [FieldOffset(sizeof(short))]
    public byte Opacity;

    [FieldOffset(sizeof(byte))]
    public ushort Type;

    [FieldOffset(sizeof(ushort))]
    public short ZIndex;

    [FieldOffset(sizeof(short))]
    public byte[] Reserved;
}