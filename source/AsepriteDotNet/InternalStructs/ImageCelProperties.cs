//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct ImageCelProperties
{
    public const int SizeOf = sizeof(ushort) +  //  Width
                              sizeof(ushort);   //  Height


    [FieldOffset(0)]
    public ushort Width;

    [FieldOffset(sizeof(ushort))]
    public ushort Height;
}