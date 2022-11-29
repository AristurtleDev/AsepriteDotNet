/* ----------------------------------------------------------------------------
The following code was ported from Aseprite, licensed under the MIT license.
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


using System.Drawing;

namespace AsepriteDotNet.ThirdParty;

internal static class Aseprite
{
    private const byte RGBA_R_SHIFT = 0;
    private const byte RGBA_G_SHIFT = 8;
    private const byte RGBA_B_SHIFT = 16;
    private const byte RGBA_A_SHIFT = 24;

    private static uint RGBA(Color color) => ((uint)color.R << RGBA_R_SHIFT) |
                                             ((uint)color.G << RGBA_G_SHIFT) |
                                             ((uint)color.B << RGBA_B_SHIFT) |
                                             ((uint)color.A << RGBA_A_SHIFT);


    private static byte Multiply(byte b, byte s) => Pixman.MUL_UN8(b, s);
    private static byte Screen(byte b, byte s) => (byte)(b + s - Pixman.MUL_UN8(b, s));
    private static byte Overlay(byte b, byte s) => HardLight(s, b);
    private static byte Darken(byte b, byte s) => Math.Min(b, s);
    private static byte Lighten(byte b, byte s) => Math.Max(b, s);
    private static byte HardLight(byte b, byte s)
    {
        if (s < 128)
        {
            return Multiply(b, (byte)(s << 1));
        }
        else
        {
            return Screen(b, (byte)((s << 1) - 255);
        }
    }

    private static byte Difference(byte b, byte s) => (byte)Math.Abs(b - s);
    private static byte Exclusion(byte b, byte s)
    {
        int t = Pixman.MUL_UN8(b, s);
        return (byte)(b + s - 2 * t);
    }

    private static byte Divide(byte b, byte s)
    {
        if (b == 0)
        {
            return 0;
        }
        else if (b >= s)
        {
            return 255;
        }
        else
        {
            return Pixman.DIV_UN8(b, s);    //  return b / s
        }
    }

    private static byte ColorDodge(byte b, byte s)
    {
        if (b == 0)
        {
            return 0;
        }

        s = (byte)(255 - s);

        if (b >= s)
        {
            return 255;
        }
        else
        {
            return Pixman.DIV_UN8(b, s);    //  return b / (1 -s)
        }
    }

    private static byte ColorBurn(uint b, uint s)
    {
        if (b == 255)
        {
            return 255;
        }

        b = (255 - b);

        if (b >= s)
        {
            return 0;
        }
        else
        {
            return (byte)(255 - Pixman.DIV_UN8((byte)b, (byte)s));  //  Return 1 - ((1-b)/s)
        }
    }

    private static byte SoftLight(uint b, uint s)
    {
        double bb = b / 255.0;
        double ss = s / 255.0;
        double rr, dd;

        if (bb <= 0.25)
        {
            dd = ((16 * b - 12) * bb + 4) * bb;
        }
        else
        {
            dd = Math.Sqrt(bb);
        }

        if (ss <= 0.5)
        {
            rr = bb - (1.0 - 2.0 * ss) * bb * (1.0 - bb);
        }
        else
        {
            rr = bb + (2.0 * ss - 1.0) * (dd - bb);
        }

        return (byte)(rr * 255 + 0.5);
    }



    #region RGBA Blender Functions
    public static Color NormalBlend(Color backdrop, Color source, int opacity)
    {
        uint b = RGBA(backdrop);
        uint s = RGBA(source);

        if ((b & RGBA_A_MASK) == 0)
        {
            uint a = RGBAGetA(s);
            a = Pixman.MUL_UN8((byte)a, (byte)opacity);
            a <<= (int)RGBA_A_SHIFT;
            return (s & RGBA_RGB_MASK) | a;
        }
        else if ((s & RGBA_A_MASK) == 0)
        {
            return b;
        }

        int Br = RGBAGetR(b);
        int Bg = RGBAGetG(b);
        int Bb = RGBAGetB(b);
        int Ba = RGBAGEtA(b);

        int Sr = RGBAGetR(src);
        int Sg = RGBAGetG(src);
        int Sb = RGBAGetB(src);
        int Sa = RGBAGEtA(src);
        Sa = Pixman.MUL_UN8((byte)Sa, (byte)opacity);

    }
    public static color_t rgba_blender_normal(color_t backdrop, color_t src, int opacity)
    {
        if ((backdrop & rgba_a_mask) == 0)
        {
            uint32_t a = rgba_geta(src);
            a = MUL_UN8((uint8_t)a, (uint8_t)opacity);
            a <<= (int)rgba_a_shift;
            return (src & rgba_rgb_mask) | a;
        }
        else if ((src & rgba_a_mask) == 0)
        {
            return backdrop;
        }

        int Br = rgba_getr(backdrop);
        int Bg = rgba_getg(backdrop);
        int Bb = rgba_getb(backdrop);
        int Ba = rgba_geta(backdrop);

        int Sr = rgba_getr(src);
        int Sg = rgba_getg(src);
        int Sb = rgba_getb(src);
        int Sa = rgba_geta(src);
        Sa = MUL_UN8((byte)Sa, (byte)opacity);

        // Ra = Sa + Ba*(1-Sa)
        //    = Sa + Ba - Ba*Sa
        int Ra = Sa + Ba - MUL_UN8((byte)Ba, (byte)Sa);

        // Ra = Sa + Ba*(1-Sa)
        // Ba = (Ra-Sa) / (1-Sa)
        // Rc = (Sc*Sa + Bc*Ba*(1-Sa)) / Ra                Replacing Ba with (Ra-Sa) / (1-Sa)...
        //    = (Sc*Sa + Bc*(Ra-Sa)/(1-Sa)*(1-Sa)) / Ra
        //    = (Sc*Sa + Bc*(Ra-Sa)) / Ra
        //    = Sc*Sa/Ra + Bc*Ra/Ra - Bc*Sa/Ra
        //    = Sc*Sa/Ra + Bc - Bc*Sa/Ra
        //    = Bc + (Sc-Bc)*Sa/Ra
        int Rr = Br + (Sr - Br) * Sa / Ra;
        int Rg = Bg + (Sg - Bg) * Sa / Ra;
        int Rb = Bb + (Sb - Bb) * Sa / Ra;

        return rgba((uint32_t)Rr, (uint32_t)Rg, (uint32_t)Rb, (uint32_t)Ra);
    }

    public static color_t rgba_blender_multiply(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_multiply(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_multiply(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_multiply(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_screen(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_screen(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_screen(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_screen(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_overlay(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_overlay(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_overlay(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_overlay(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_darken(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_darken(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_darken(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_darken(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_lighten(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_lighten(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_lighten(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_lighten(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_color_dodge(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_color_dodge(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_color_dodge(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_color_dodge(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_color_burn(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_color_burn(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_color_burn(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_color_burn(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_hard_light(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_hard_light(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_hard_light(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_hard_light(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_soft_light(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_soft_light(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_soft_light(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_soft_light(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_difference(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_difference(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_difference(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_difference(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_exclusion(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_exclusion(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_exclusion(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_exclusion(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }
    #endregion RGBA Blender Functions

    #region HSV Blender Functions
    private static double lum(double r, double g, double b)
    {
        return (0.3 * r) + (0.59 * g) + (0.11 * b);
    }

    private static double sat(double r, double g, double b)
    {
        return Math.Max(r, Math.Max(g, b)) - Math.Min(r, Math.Min(g, b));
    }

    private static void clip_color(ref double r, ref double g, ref double b)
    {
        double l = lum(r, g, b);
        double n = Math.Min(r, Math.Min(g, b));
        double x = Math.Max(r, Math.Max(g, b));

        if (n < 0)
        {
            r = l + (((r - l) * l) / (l - n));
            g = l + (((g - l) * l) / (l - n));
            b = l + (((b - l) * l) / (l - n));
        }

        if (x > 1)
        {
            r = l + (((r - l) * (1 - l)) / (x - l));
            g = l + (((g - l) * (1 - l)) / (x - l));
            b = l + (((b - l) * (1 - l)) / (x - l));
        }
    }

    private static void set_lum(ref double r, ref double g, ref double b, double l)
    {
        double d = l - lum(r, g, b);
#pragma warning disable IDE0054 // Use compound assignment
        r = r + d;
        g = g + d;
        b = b + d;
#pragma warning restore IDE0054 // Use compound assignment
        clip_color(ref r, ref g, ref b);
    }

    //  This is ugly, and i hate it, but it works
    static void set_sat(ref double r, ref double g, ref double b, double s)
    {
        ref double MIN(ref double x, ref double y) => ref (x < y ? ref x : ref y);
        ref double MAX(ref double x, ref double y) => ref (x > y ? ref x : ref y);
        ref double MID(ref double x, ref double y, ref double z) =>
            ref (x > y ? ref (y > z ? ref y : ref (x > z ?
                ref z : ref x)) : ref (y > z ? ref (z > x ? ref z :
                ref x) : ref y));


        ref double min = ref MIN(ref r, ref MIN(ref g, ref b));
        ref double mid = ref MID(ref r, ref g, ref b);
        ref double max = ref MAX(ref r, ref MAX(ref g, ref b));

        if (max > min)
        {
            mid = ((mid - min) * s) / (max - min);
            max = s;
        }
        else
        {
            mid = max = 0;
        }

        min = 0;
    }

    public static color_t rgba_blender_hsl_hue(color_t backdrop, color_t src, int opacity)
    {
        double r = rgba_getr(backdrop) / 255.0;
        double g = rgba_getg(backdrop) / 255.0;
        double b = rgba_getb(backdrop) / 255.0;
        double s = sat(r, g, b);
        double l = lum(r, g, b);

        r = rgba_getr(src) / 255.0;
        g = rgba_getg(src) / 255.0;
        b = rgba_getb(src) / 255.0;

        set_sat(ref r, ref g, ref b, s);
        set_lum(ref r, ref g, ref b, l);

        src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_hsl_saturation(color_t backdrop, color_t src, int opacity)
    {
        double r = rgba_getr(src) / 255.0;
        double g = rgba_getg(src) / 255.0;
        double b = rgba_getb(src) / 255.0;
        double s = sat(r, g, b);

        r = rgba_getr(backdrop) / 255.0;
        g = rgba_getg(backdrop) / 255.0;
        b = rgba_getb(backdrop) / 255.0;
        double l = lum(r, g, b);

        set_sat(ref r, ref g, ref b, s);
        set_lum(ref r, ref g, ref b, l);

        src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_hsl_color(color_t backdrop, color_t src, int opacity)
    {
        double r = rgba_getr(backdrop) / 255.0;
        double g = rgba_getg(backdrop) / 255.0;
        double b = rgba_getb(backdrop) / 255.0;
        double l = lum(r, g, b);

        r = rgba_getr(src) / 255.0;
        g = rgba_getg(src) / 255.0;
        b = rgba_getb(src) / 255.0;

        set_lum(ref r, ref g, ref b, l);

        src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_hsl_luminosity(color_t backdrop, color_t src, int opacity)
    {
        double r = rgba_getr(src) / 255.0;
        double g = rgba_getg(src) / 255.0;
        double b = rgba_getb(src) / 255.0;
        double l = lum(r, g, b);

        r = rgba_getr(backdrop) / 255.0;
        g = rgba_getg(backdrop) / 255.0;
        b = rgba_getb(backdrop) / 255.0;

        set_lum(ref r, ref g, ref b, l);

        src = rgba((uint32_t)(255.0 * r), (uint32_t)(255.0 * g), (uint32_t)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_addition(color_t backdrop, color_t src, int opacity)
    {
        int r = rgba_getr(backdrop) + rgba_getr(src);
        int g = rgba_getg(backdrop) + rgba_getg(src);
        int b = rgba_getb(backdrop) + rgba_getb(src);
        src = rgba((uint8_t)Math.Min(r, 255), (uint8_t)Math.Min(g, 255), (uint8_t)Math.Min(b, 255), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_subtract(color_t backdrop, color_t src, int opacity)
    {
        int r = rgba_getr(backdrop) - rgba_getr(src);
        int g = rgba_getg(backdrop) - rgba_getg(src);
        int b = rgba_getb(backdrop) - rgba_getb(src);
        src = rgba((uint8_t)Math.Max(r, 0), (uint8_t)Math.Max(g, 0), (uint8_t)Math.Max(b, 0), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static color_t rgba_blender_divide(color_t backdrop, color_t src, int opacity)
    {
        uint8_t r = blend_divide(rgba_getr(backdrop), rgba_getr(src));
        uint8_t g = blend_divide(rgba_getg(backdrop), rgba_getg(src));
        uint8_t b = blend_divide(rgba_getb(backdrop), rgba_getb(src));
        src = rgba(r, g, b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }
    #endregion HSV BLender Functions
}