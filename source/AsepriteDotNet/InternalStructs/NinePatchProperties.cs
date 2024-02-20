//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct NinePatchProperties
{
    public const int StructSize = sizeof(long) +    //  X
                                  sizeof(long) +    //  Y
                                  sizeof(uint) +    //  Width
                                  sizeof(uint);     //  Height

    [FieldOffset(0)]
    public long X;

    [FieldOffset(sizeof(long))]
    public long Y;

    [FieldOffset(sizeof(long))]
    public uint Width;

    [FieldOffset(sizeof(uint))]
    public uint Height;
}
