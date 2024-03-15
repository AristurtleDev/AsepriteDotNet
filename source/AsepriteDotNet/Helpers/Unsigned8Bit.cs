// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Helpers;

internal static class Unsigned8Bit
{
    public static byte Multiply(int a, int b)
    {
        int v = (a * b) + 0x80;
        return (byte)(((v >> 8) + v) >> 8);
    }

    public static byte Divide(int a, int b) => (byte)(((ushort)a * 0xFF + (b / 2)) / b);
}
