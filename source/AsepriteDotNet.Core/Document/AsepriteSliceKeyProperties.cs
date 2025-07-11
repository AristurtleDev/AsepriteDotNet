//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepriteSliceKeyProperties
{
    internal const int StructSize = sizeof(uint) +  //  FrameNumber
                                    sizeof(int) +  //  X
                                    sizeof(int) +  //  Y
                                    sizeof(uint) +  //  Width
                                    sizeof(uint);   //  Height

    [FieldOffset(0)]
    internal uint FrameNumber;

    [FieldOffset(4)]
    internal int X;

    [FieldOffset(8)]
    internal int Y;

    [FieldOffset(12)]
    internal uint Width;

    [FieldOffset(16)]
    internal uint Height;
}
