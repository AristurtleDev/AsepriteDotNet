//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.Document;

[StructLayout(LayoutKind.Explicit)]
internal struct AsepritePivotProperties
{
    internal const int StructSize = sizeof(int) +  //  X
                                    sizeof(int);   //  Y

    [FieldOffset(0)]
    internal int X;

    [FieldOffset(4)]
    internal int Y;
}
