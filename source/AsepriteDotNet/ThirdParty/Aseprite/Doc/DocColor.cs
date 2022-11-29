/* ----------------------------------------------------------------------------
The following code was ported from Aseprite, licensed under the MIT license.
https://github.com/aseprite/aseprite/blob/main/src/doc/color.h

This ported code is also licensed under the MIT license.

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice (including the next
paragraph) shall be included in all copies or substantial portions of the
Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
---------------------------------------------------------------------------- */

/* ----------------------------------------------------------------------------
The following is the original MIT license from Aseprite

Copyright (c) 2018 - 2020 Igara Studio S.A.
Copyright (c) 2001 - 2018 David Capello

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice (including the next
paragraph) shall be included in all copies or substantial portions of the
Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
---------------------------------------------------------------------------- */
namespace AsepriteDotNet.ThirdParty.Aseprite.Doc;

//  Changed name to DocColor so there's no collisions with System.Drawing.Color
internal static class DocColor
{
    public const byte rgba_r_shift = 0;
    public const byte rgba_g_shift = 8;
    public const byte rgba_b_shift = 16;
    public const byte rgba_a_shift = 24;
    public const ushort graya_v_shift = 0;
    public const ushort graya_a_shift = 8;
    public const ushort graya_v_mask = 0x00FF;
    public const ushort graya_a_mask = 0xFF00;


    public const uint rgba_r_mask = 0x000000ff;
    public const uint rgba_g_mask = 0x0000ff00;
    public const uint rgba_b_mask = 0x00ff0000;
    public const uint rgba_rgb_mask = 0x00ffffff;
    public const uint rgba_a_mask = 0xff000000;

    public static byte rgba_getr(uint c) => (byte)((c >> rgba_r_shift) & 0xFF);
    public static byte rgba_getg(uint c) => (byte)((c >> rgba_g_shift) & 0xFF);
    public static byte rgba_getb(uint c) => (byte)((c >> rgba_b_shift) & 0xFF);
    public static byte rgba_geta(uint c) => (byte)((c >> rgba_a_shift) & 0xFF);
    public static uint rgba_seta(uint c, byte a) => (c & rgba_rgb_mask) | (uint)(a << rgba_a_shift);
    public static uint rgba(byte r, byte g, byte b, byte a) => (uint)((r << rgba_r_shift) |
                                                                      (g << rgba_g_shift) |
                                                                      (b << rgba_b_shift) |
                                                                      (a << rgba_a_shift));

    public static int rgb_luma(int r, int g, int b) => (r * 2126 + g * 7152 + b * 722) / 10000;
    public static byte rgba_luma(uint c) => (byte)rgb_luma(rgba_getr(c), rgba_getg(c), rgba_getb(c));
    public static byte graya_getv(ushort c) => (byte)((c >> graya_v_shift) & 0xFF);
    public static byte graya_geta(ushort c) => (byte)((c >> graya_a_shift) & 0xFF);
    public static ushort graya_seta(ushort c, byte a) => (ushort)((c & graya_v_mask) | (a << graya_a_shift));
    public static ushort graya(byte v, byte a) => (ushort)((v << graya_v_shift) | (a << graya_a_shift));
    public static ushort gray(byte v) => graya(v, 255);
}
