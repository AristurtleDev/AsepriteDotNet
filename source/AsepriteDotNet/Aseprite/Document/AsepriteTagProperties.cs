//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AsepriteTagProperties
{
    internal const int StructSize = sizeof(ushort) +        //  From
                                    sizeof(ushort) +        //  To
                                    sizeof(byte) +          //  Direction
                                    sizeof(ushort) +        //  Repeat
                                    sizeof(byte) * 6 +    //  Reserved
                                    sizeof(byte) * 3 +    //  RGB
                                    sizeof(byte) +          //  Ignore
                                    sizeof(ushort);         //  NameLen

    [FieldOffset(0)]
    internal ushort From;

    [FieldOffset(2)]
    internal ushort To;

    [FieldOffset(4)]
    internal byte Direction;

    [FieldOffset(5)]
    internal ushort Repeat;

    //[FieldOffset(7)]
    //internal fixed byte Reserved[6];

    [FieldOffset(13)]
    internal byte R;

    [FieldOffset(14)]
    internal byte G;

    [FieldOffset(15)]
    internal byte B;

    [FieldOffset(17)]
    internal ushort NameLen;
}
