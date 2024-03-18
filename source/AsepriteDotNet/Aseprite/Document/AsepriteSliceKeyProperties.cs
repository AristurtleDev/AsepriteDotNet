//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Aseprite.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepriteSliceKeyProperties
{
    internal const int StructSize = sizeof(uint) +  //  FrameNumber
                                    sizeof(long) +  //  X
                                    sizeof(long) +  //  Y
                                    sizeof(uint) +  //  Width
                                    sizeof(uint);   //  Height

    [FieldOffset(0)]
    internal uint FrameNumber;

    [FieldOffset(4)]
    internal long X;

    [FieldOffset(12)]
    internal long Y;

    [FieldOffset(20)]
    internal uint Width;

    [FieldOffset(24)]
    internal uint Height;
}
