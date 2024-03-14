//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct ImageCelProperties
{
    internal const int StructSize = sizeof(ushort) +    //  Width
                                    sizeof(ushort);     //  Height


    [FieldOffset(0)]
    internal ushort Width;

    [FieldOffset(2)]
    internal ushort Height;
}
