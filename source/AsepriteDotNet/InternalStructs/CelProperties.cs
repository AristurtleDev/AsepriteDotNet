//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct CelProperties
{
    internal const int StructSize = sizeof(ushort) +    //  LayerIndex
                                    sizeof(short) +     //  X
                                    sizeof(short) +     //  Y
                                    sizeof(byte) +      //  Opacity
                                    sizeof(ushort) +    //  Type
                                    sizeof(short) +     //  ZIndex
                                    (sizeof(byte) * 5); //  Reserved


    [FieldOffset(0)]
    internal ushort LayerIndex;

    [FieldOffset(sizeof(ushort))]
    internal short X;

    [FieldOffset(sizeof(short))]
    internal short Y;

    [FieldOffset(sizeof(short))]
    internal byte Opacity;

    [FieldOffset(sizeof(byte))]
    internal ushort Type;

    [FieldOffset(sizeof(ushort))]
    internal short ZIndex;

    [FieldOffset(sizeof(short))]
    internal byte[] Reserved;
}
