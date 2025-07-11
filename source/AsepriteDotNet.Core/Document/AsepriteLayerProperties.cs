//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.Document;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AsepriteLayerProperties
{
    internal const int StructSize = sizeof(ushort) +        //  Flags
                                    sizeof(ushort) +        //  Type
                                    sizeof(ushort) +        //  Level
                                    sizeof(ushort) +        //  DefaultWidth
                                    sizeof(ushort) +        //  DefaultHeight
                                    sizeof(ushort) +        //  BlendMode
                                    sizeof(byte) +          //  Opacity
                                    sizeof(byte) * 3 +    //  Ignore
                                    sizeof(ushort);         //  NameLen


    [FieldOffset(0)]
    internal ushort Flags;

    [FieldOffset(2)]
    internal ushort Type;

    [FieldOffset(4)]
    internal ushort Level;

    [FieldOffset(6)]
    internal ushort DefaultWidth;

    [FieldOffset(8)]
    internal ushort DefaultHeight;

    [FieldOffset(10)]
    internal ushort BlendMode;

    [FieldOffset(12)]
    internal byte Opacity;

    //[FieldOffset(13)]
    //internal fixed byte Ignore[3];

    [FieldOffset(16)]
    internal ushort NameLen;
}
