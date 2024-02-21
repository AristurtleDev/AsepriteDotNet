//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct PivotProperties
{
    internal const int StructSize = sizeof(long) +  //  X
                                    sizeof(long);   //  Y

    [FieldOffset(0)]
    internal long X;

    [FieldOffset(sizeof(long))]
    internal long Y;
}
