//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepriteNinePatchProperties
{
    internal const int StructSize = sizeof(int) +  //  X
                                    sizeof(int) +  //  Y
                                    sizeof(uint) +  //  Width
                                    sizeof(uint);   //  Height

    [FieldOffset(0)]
    internal int X;

    [FieldOffset(4)]
    internal int Y;

    [FieldOffset(8)]
    internal uint Width;

    [FieldOffset(12)]
    internal uint Height;
}
