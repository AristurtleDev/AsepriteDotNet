//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepriteNinePatchProperties
{
    internal const int StructSize = sizeof(long) +  //  X
                                    sizeof(long) +  //  Y
                                    sizeof(uint) +  //  Width
                                    sizeof(uint);   //  Height

    [FieldOffset(0)]
    internal long X;

    [FieldOffset(8)]
    internal long Y;

    [FieldOffset(16)]
    internal uint Width;

    [FieldOffset(20)]
    internal uint Height;
}
