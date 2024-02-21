//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct NinePatchProperties
{
    internal const int StructSize = sizeof(long) +  //  X
                                    sizeof(long) +  //  Y
                                    sizeof(uint) +  //  Width
                                    sizeof(uint);   //  Height

    [FieldOffset(0)]
    internal long X;

    [FieldOffset(sizeof(long))]
    internal long Y;

    [FieldOffset(sizeof(long))]
    internal uint Width;

    [FieldOffset(sizeof(uint))]
    internal uint Height;
}
