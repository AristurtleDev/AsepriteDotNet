//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct SliceKeyProperties
{
    internal const int StructSize = sizeof(uint) +  //  FrameNumber
                                    sizeof(long) +  //  X
                                    sizeof(long) +  //  Y
                                    sizeof(uint) +  //  Width
                                    sizeof(uint);   //  Height

    [FieldOffset(0)]
    internal uint FrameNumber;

    [FieldOffset(sizeof(uint))]
    internal long X;

    [FieldOffset(sizeof(long))]
    internal long Y;

    [FieldOffset(sizeof(long))]
    internal uint Width;

    [FieldOffset(sizeof(uint))]
    internal uint Height;
}
