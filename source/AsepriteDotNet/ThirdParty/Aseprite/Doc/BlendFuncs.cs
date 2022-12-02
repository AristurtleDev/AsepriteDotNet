/* ----------------------------------------------------------------------------
The following code was ported from Aseprite, licensed under the MIT license.
https://github.com/aseprite/aseprite/blob/main/src/doc/blend_funcs.cpp

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
using static AsepriteDotNet.ThirdParty.Pixman.Combine32;
using static AsepriteDotNet.ThirdParty.Aseprite.Doc.DocColor;
using AsepriteDotNet.Document;
using System.Drawing;

namespace AsepriteDotNet.ThirdParty.Aseprite.Doc;


internal static class BlendFuncs
{
    public static byte blend_multiply(byte b, byte s) => MUL_UN8(b, s);
    public static byte blend_screen(byte b, byte s) => (byte)(b + s - MUL_UN8(b, s));
    public static byte blend_overlay(byte b, byte s) => blend_hard_light(s, b);
    public static byte blend_darken(byte b, byte s) => Math.Min(b, s);
    public static byte blend_lighten(byte b, byte s) => Math.Max(b, s);
    public static byte blend_hard_light(byte b, byte s) => s < 128 ?
                                                          blend_multiply(b, (byte)(s << 1)) :
                                                          blend_screen(b, (byte)((s << 1) - 255));
    public static byte blend_difference(byte b, byte s) => (byte)Math.Abs(b - s);
    public static byte blend_exclusion(byte b, byte s) => (byte)(b + s - 2 * MUL_UN8(b, s));

    public static uint rgba_blender_n(BlendMode blendMode, uint backdrop, uint src, int opacity)
    {
        if ((backdrop & rgba_a_mask) != 0)
        {
            uint normal = rgba_blender_normal(backdrop, src, opacity);
            uint blend = blendMode switch
            {
                BlendMode.Multiply => rgba_blender_multiply(backdrop, src, opacity),
                BlendMode.Screen => rgba_blender_screen(backdrop, src, opacity),
                BlendMode.Overlay => rgba_blender_overlay(backdrop, src, opacity),
                BlendMode.Darken => rgba_blender_darken(backdrop, src, opacity),
                BlendMode.Lighten => rgba_blender_lighten(backdrop, src, opacity),
                BlendMode.ColorDodge => rgba_blender_color_dodge(backdrop, src, opacity),
                BlendMode.ColorBurn => rgba_blender_color_burn(backdrop, src, opacity),
                BlendMode.HardLight => rgba_blender_hard_light(backdrop, src, opacity),
                BlendMode.SoftLight => rgba_blender_soft_light(backdrop, src, opacity),
                BlendMode.Difference => rgba_blender_difference(backdrop, src, opacity),
                BlendMode.Exclusion => rgba_blender_exclusion(backdrop, src, opacity),
                BlendMode.Hue => rgba_blender_hsl_hue(backdrop, src, opacity),
                BlendMode.Saturation => rgba_blender_hsl_saturation(backdrop, src, opacity),
                BlendMode.Color => rgba_blender_hsl_color(backdrop, src, opacity),
                BlendMode.Luminosity => rgba_blender_hsl_luminosity(backdrop, src, opacity),
                BlendMode.Addition => rgba_blender_addition(backdrop, src, opacity),
                BlendMode.Subtract => rgba_blender_subtract(backdrop, src, opacity),
                BlendMode.Divide => rgba_blender_divide(backdrop, src, opacity),
                _ => throw new InvalidOperationException($"Invalid blend mode '{blendMode}'")
            };

            int Ba = rgba_geta(backdrop);
            uint normalToBlendMerge = rgba_blender_merge(normal, blend, Ba);
            int srcTotalAlpha = MUL_UN8(rgba_geta(src), (byte)opacity);
            int compositeAlpha = MUL_UN8((byte)Ba, (byte)srcTotalAlpha);
            return rgba_blender_merge(normalToBlendMerge, blend, compositeAlpha);
        }
        else
        {
            return rgba_blender_normal(backdrop, src, opacity);
        }
    }

    public static uint graya_blender_n(BlendMode blendMode, uint backdrop, uint src, int opacity)
    {
        if ((backdrop & graya_a_mask) != 0)
        {
            uint normal = graya_blender_normal(backdrop, src, opacity);
            uint blend = blendMode switch
            {
                BlendMode.Multiply => graya_blender_multiply(backdrop, src, opacity),
                BlendMode.Screen => graya_blender_screen(backdrop, src, opacity),
                BlendMode.Overlay => graya_blender_overlay(backdrop, src, opacity),
                BlendMode.Darken => graya_blender_darken(backdrop, src, opacity),
                BlendMode.Lighten => graya_blender_lighten(backdrop, src, opacity),
                BlendMode.ColorDodge => graya_blender_color_dodge(backdrop, src, opacity),
                BlendMode.ColorBurn => graya_blender_color_burn(backdrop, src, opacity),
                BlendMode.HardLight => graya_blender_hard_light(backdrop, src, opacity),
                BlendMode.SoftLight => graya_blender_soft_light(backdrop, src, opacity),
                BlendMode.Difference => graya_blender_difference(backdrop, src, opacity),
                BlendMode.Exclusion => graya_blender_exclusion(backdrop, src, opacity),
                BlendMode.Addition => graya_blender_addition(backdrop, src, opacity),
                BlendMode.Subtract => graya_blender_subtract(backdrop, src, opacity),
                BlendMode.Divide => graya_blender_divide(backdrop, src, opacity),
                _ => throw new InvalidOperationException($"Invalid blend mode '{blendMode}'")
            };

            int Ba = graya_geta((ushort)backdrop);
            uint normalToBlendMerge = graya_blender_merge(normal, blend, Ba);
            int srcTotalAlpha = MUL_UN8(graya_geta((ushort)src), (byte)opacity);
            int compositeAlpha = MUL_UN8((byte)Ba, (byte)srcTotalAlpha);
            return graya_blender_merge(normalToBlendMerge, blend, compositeAlpha);
        }
        else
        {
            return graya_blender_normal(backdrop, src, opacity);
        }
    }

    public static byte blend_divide(byte b, byte s)
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
            //  return b / s
            return DIV_UN8(b, s);
        }
    }

    public static byte blend_color_dodge(byte b, byte s)
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
            //  return b / (1 -s)
            return DIV_UN8(b, s);
        }
    }

    public static byte blend_color_burn(uint b, uint s)
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
            //  Return 1 - ((1-b)/s)
            return (byte)(255 - DIV_UN8((byte)b, (byte)s));
        }
    }

    public static uint blend_soft_light(uint _b, uint _s)
    {
        double b = _b / 255.0;
        double s = _s / 255.0;
        double r, d;

        if (b <= 0.25)
        {
            d = ((16 * b - 12) * b + 4) * b;
        }
        else
        {
            d = Math.Sqrt(b);
        }

        if (s <= 0.5)
        {
            r = b - (1.0 - 2.0 * s) * b * (1.0 - b);
        }
        else
        {
            r = b + (2.0 * s - 1.0) * (d - b);
        }

        return (uint)(r * 255 + 0.5);
    }

    #region RGB blenders

    public static uint rgba_blender_src(uint backdrop, uint src, int opacity) => src;

    public static uint rgba_blender_merge(uint backdrop, uint src, int opacity)
    {
        int Br, Bg, Bb, Ba;
        int Sr, Sg, Sb, Sa;
        int Rr, Rg, Rb, Ra;

        Br = rgba_getr(backdrop);
        Bg = rgba_getg(backdrop);
        Bb = rgba_getb(backdrop);
        Ba = rgba_geta(backdrop);

        Sr = rgba_getr(src);
        Sg = rgba_getg(src);
        Sb = rgba_getb(src);
        Sa = rgba_geta(src);

        if (Ba == 0)
        {
            Rr = Sr;
            Rg = Sg;
            Rb = Sb;
        }
        else if (Sa == 0)
        {
            Rr = Br;
            Rg = Bg;
            Rb = Bb;
        }
        else
        {
            Rr = Br + MUL_UN8((byte)(Sr - Br), (byte)opacity);
            Rg = Bg + MUL_UN8((byte)(Sg - Bg), (byte)opacity);
            Rb = Bb + MUL_UN8((byte)(Sb - Bb), (byte)opacity);
        }

        Ra = Ba + MUL_UN8((byte)(Sa - Ba), (byte)opacity);
        if (Ra == 0)
        {
            Rr = Rg = Rb = 0;
        }

        return rgba((byte)Rr, (byte)Rg, (byte)Rb, (byte)Ra);
    }

    public static uint rgba_blender_neg_bw(uint backdrop, uint src, int opacity)
    {
        if ((backdrop & rgba_a_mask) == 0)
        {
            return rgba(0, 0, 0, 255);
        }
        else if (rgba_luma(backdrop) < 128)
        {
            return rgba(255, 255, 255, 255);
        }
        else
        {
            return rgba(0, 0, 0, 255);
        }
    }

    public static uint rgba_blender_red_tint(uint backdrop, uint src, int opacity)
    {
        int v = rgba_luma(src);
        src = rgba((byte)((255 + v) / 2), (byte)(v / 2), (byte)(v / 2), rgba_geta(src));
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_blue_tint(uint backdrop, uint src, int opacity)
    {
        int v = rgba_luma(src);
        src = rgba((byte)(v / 2), (byte)(v / 2), (byte)((255 + v) / 2), rgba_geta(src));
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_normal(uint backdrop, uint src, int opacity)
    {
        if ((backdrop & rgba_a_mask) == 0)
        {
            uint a = rgba_geta(src);
            a = MUL_UN8((byte)a, (byte)opacity);
            a <<= rgba_a_shift;
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

        return rgba((byte)Rr, (byte)Rg, (byte)Rb, (byte)Ra);
    }

    public static uint rgba_blender_normal_dst_over(uint backdrop, uint src, int opacity)
    {
        uint Sa = MUL_UN8(rgba_geta(src), (byte)opacity);
        src = (src & rgba_rgb_mask) | (Sa << rgba_a_shift);
        return rgba_blender_normal(src, backdrop, 255);
    }

    public static uint rgba_blender_multiply(uint backdrop, uint src, int opacity)
    {
        int r = blend_multiply(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_multiply(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_multiply(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_screen(uint backdrop, uint src, int opacity)
    {
        int r = blend_screen(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_screen(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_screen(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_overlay(uint backdrop, uint src, int opacity)
    {
        int r = blend_overlay(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_overlay(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_overlay(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_darken(uint backdrop, uint src, int opacity)
    {
        int r = blend_darken(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_darken(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_darken(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_lighten(uint backdrop, uint src, int opacity)
    {
        int r = blend_lighten(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_lighten(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_lighten(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_color_dodge(uint backdrop, uint src, int opacity)
    {
        int r = blend_color_dodge(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_color_dodge(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_color_dodge(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_color_burn(uint backdrop, uint src, int opacity)
    {
        int r = blend_color_burn(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_color_burn(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_color_burn(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_hard_light(uint backdrop, uint src, int opacity)
    {
        int r = blend_hard_light(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_hard_light(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_hard_light(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_soft_light(uint backdrop, uint src, int opacity)
    {
        int r = (int)blend_soft_light(rgba_getr(backdrop), rgba_getr(src));
        int g = (int)blend_soft_light(rgba_getg(backdrop), rgba_getg(src));
        int b = (int)blend_soft_light(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_difference(uint backdrop, uint src, int opacity)
    {
        int r = blend_difference(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_difference(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_difference(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_exclusion(uint backdrop, uint src, int opacity)
    {
        int r = blend_exclusion(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_exclusion(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_exclusion(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    #endregion RGB blenders

    #region HSV blenders

    public static double lum(double r, double g, double b) => (0.3 * r) + (0.59 * g) + (0.11 * b);
    public static double sat(double r, double g, double b) => Math.Max(r, Math.Max(g, b)) - Math.Min(r, Math.Min(g, b));

    public static void clip_color(ref double r, ref double g, ref double b)
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

    public static void set_lum(ref double r, ref double g, ref double b, double l)
    {
        double d = l - lum(r, g, b);
        r += d;
        g += d;
        b += d;
        clip_color(ref r, ref g, ref b);
    }

    public static void set_sat(ref double r, ref double g, ref double b, double s)
    {
        // #define MIN(x,y)     (((x) < (y)) ? (x) : (y))
        ref double MIN(ref double x, ref double y) => ref (x < y ? ref x : ref y);

        // #define MAX(x,y)     (((x) > (y)) ? (x) : (y))
        ref double MAX(ref double x, ref double y) => ref (x > y ? ref x : ref y);

        //  #define MID(x,y,z)   ((x) > (y) ? ((y) > (z) ? (y) : ((x) > (z) ?
        //                         (z) : (x))) : ((y) > (z) ? ((z) > (x) ? (z) :
        //                         (x)): (y)))
        ref double MID(ref double x, ref double y, ref double z) =>
            ref (x > y ? ref (y > z ? ref y : ref (x > z ? ref z : ref x)) : ref (y > z ? ref (z > x ? ref z : ref x) : ref y));


        //  double& min = MIN(r, MIN(g, b));
        ref double min = ref MIN(ref r, ref MIN(ref g, ref b));

        //  double& mid = MID(r, g, b);
        ref double mid = ref MID(ref r, ref g, ref b);

        //  double& max = MAX(r, MAX(g, b));
        ref double max = ref MAX(ref r, ref MAX(ref g, ref b));

        if (max > min)
        {
            mid = ((mid - min) * s) / (max - min);
            max = s;
        }
        else
            mid = max = 0;

        min = 0;
    }

    public static uint rgba_blender_hsl_hue(uint backdrop, uint src, int opacity)
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

        src = rgba((byte)(255.0 * r), (byte)(255.0 * g), (byte)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);

    }

    public static uint rgba_blender_hsl_saturation(uint backdrop, uint src, int opacity)
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

        src = rgba((byte)(255.0 * r), (byte)(255.0 * g), (byte)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_hsl_color(uint backdrop, uint src, int opacity)
    {
        double r = rgba_getr(backdrop) / 255.0;
        double g = rgba_getg(backdrop) / 255.0;
        double b = rgba_getb(backdrop) / 255.0;
        double l = lum(r, g, b);

        r = rgba_getr(src) / 255.0;
        g = rgba_getg(src) / 255.0;
        b = rgba_getb(src) / 255.0;

        set_lum(ref r, ref g, ref b, l);

        src = rgba((byte)(255.0 * r), (byte)(255.0 * g), (byte)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_hsl_luminosity(uint backdrop, uint src, int opacity)
    {
        double r = rgba_getr(src) / 255.0;
        double g = rgba_getg(src) / 255.0;
        double b = rgba_getb(src) / 255.0;
        double l = lum(r, g, b);

        r = rgba_getr(backdrop) / 255.0;
        g = rgba_getg(backdrop) / 255.0;
        b = rgba_getb(backdrop) / 255.0;

        set_lum(ref r, ref g, ref b, l);

        src = rgba((byte)(255.0 * r), (byte)(255.0 * g), (byte)(255.0 * b), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_addition(uint backdrop, uint src, int opacity)
    {
        int r = rgba_getr(backdrop) + rgba_getr(src);
        int g = rgba_getg(backdrop) + rgba_getg(src);
        int b = rgba_getb(backdrop) + rgba_getb(src);
        src = rgba((byte)Math.Min(r, 255),
                   (byte)Math.Min(g, 255),
                   (byte)Math.Min(b, 255), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_subtract(uint backdrop, uint src, int opacity)
    {
        int r = rgba_getr(backdrop) - rgba_getr(src);
        int g = rgba_getg(backdrop) - rgba_getg(src);
        int b = rgba_getb(backdrop) - rgba_getb(src);
        src = rgba((byte)Math.Max(r, 0), (byte)Math.Max(g, 0), (byte)Math.Max(b, 0), 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    public static uint rgba_blender_divide(uint backdrop, uint src, int opacity)
    {
        int r = blend_divide(rgba_getr(backdrop), rgba_getr(src));
        int g = blend_divide(rgba_getg(backdrop), rgba_getg(src));
        int b = blend_divide(rgba_getb(backdrop), rgba_getb(src));
        src = rgba((byte)r, (byte)g, (byte)b, 0) | (src & rgba_a_mask);
        return rgba_blender_normal(backdrop, src, opacity);
    }

    #endregion HSV blenders

    #region Gray blenders

    public static uint graya_blender_src(uint backdrop, uint src, int opacity) => src;

    public static uint graya_blender_merge(uint backdrop, uint src, int opacity)
    {
        int Bk, Ba;
        int Sk, Sa;
        int Rk, Ra;

        Bk = graya_getv((ushort)backdrop);
        Ba = graya_geta((ushort)backdrop);

        Sk = graya_getv((ushort)src);
        Sa = graya_geta((ushort)src);

        if (Ba == 0)
        {
            Rk = Sk;
        }
        else if (Sa == 0)
        {
            Rk = Bk;
        }
        else
        {
            Rk = Bk + MUL_UN8((byte)(Sk - Bk), (byte)opacity);
        }

        Ra = Ba + MUL_UN8((byte)(Sa - Ba), (byte)opacity);
        if (Ra == 0)
            Rk = 0;

        return graya((byte)Rk, (byte)Ra);
    }

    public static uint graya_blender_neg_bw(uint backdrop, uint src, int opacity)
    {
        if ((backdrop & graya_a_mask) == 0)
        {
            return graya(0, 255);
        }
        else if (graya_getv((ushort)backdrop) < 128)
        {
            return graya(255, 255);
        }
        else
        {
            return graya(0, 255);
        }
    }

    public static uint graya_blender_normal(uint backdrop, uint src, int opacity)
    {
        if ((backdrop & graya_a_mask) == 0)
        {
            byte a = graya_geta((ushort)src);
            a = MUL_UN8((byte)a, (byte)opacity);
            a <<= graya_a_shift;
            return (src & 0xff) | a;
        }
        else if ((src & graya_a_mask) == 0)
        {
            return backdrop;
        }

        int Bg, Ba;
        int Sg, Sa;
        int Rg, Ra;

        Bg = graya_getv((ushort)backdrop);
        Ba = graya_geta((ushort)backdrop);

        Sg = graya_getv((ushort)src);
        Sa = graya_geta((ushort)src);
        Sa = MUL_UN8((byte)Sa, (byte)opacity);

        Ra = Ba + Sa - MUL_UN8((byte)Ba, (byte)Sa);
        Rg = Bg + (Sg - Bg) * Sa / Ra;

        return graya((byte)Rg, (byte)Ra);
    }

    public static uint graya_blender_normal_dst_over(uint backdrop, uint src, int opacity)
    {
        int Sa = MUL_UN8(graya_geta((ushort)src), (byte)opacity);
        src = (uint)((src & graya_v_mask)) | (uint)((Sa << graya_a_shift));
        return graya_blender_normal(src, backdrop, 255);
    }

    public static uint graya_blender_multiply(uint backdrop, uint src, int opacity)
    {
        int v = blend_multiply(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_screen(uint backdrop, uint src, int opacity)
    {
        int v = blend_screen(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_overlay(uint backdrop, uint src, int opacity)
    {
        int v = blend_overlay(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_darken(uint backdrop, uint src, int opacity)
    {
        int v = blend_darken(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_lighten(uint backdrop, uint src, int opacity)
    {
        int v = blend_lighten(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_color_dodge(uint backdrop, uint src, int opacity)
    {
        int v = blend_color_dodge(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_color_burn(uint backdrop, uint src, int opacity)
    {
        int v = blend_color_burn(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_hard_light(uint backdrop, uint src, int opacity)
    {
        int v = blend_hard_light(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_soft_light(uint backdrop, uint src, int opacity)
    {
        int v = (int)blend_soft_light(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_difference(uint backdrop, uint src, int opacity)
    {
        int v = blend_difference(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_exclusion(uint backdrop, uint src, int opacity)
    {
        int v = blend_exclusion(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_addition(uint backdrop, uint src, int opacity)
    {
        int v = graya_getv((ushort)backdrop) + graya_getv((ushort)src);
        src = graya((byte)Math.Min(v, 255), 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_subtract(uint backdrop, uint src, int opacity)
    {
        int v = graya_getv((ushort)backdrop) - graya_getv((ushort)src);
        src = graya((byte)Math.Max(v, 0), 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    public static uint graya_blender_divide(uint backdrop, uint src, int opacity)
    {
        int v = blend_divide(graya_getv((ushort)backdrop), graya_getv((ushort)src));
        src = graya((byte)v, 0) | (src & graya_a_mask);
        return graya_blender_normal(backdrop, src, opacity);
    }

    #endregion Gray blenders

    public static Color Blend(BlendMode mode, Color backdrop, Color src, int opacity)
    {
        uint b = rgba(backdrop.R, backdrop.G, backdrop.B, backdrop.A);
        uint s = rgba(src.R, src.G, src.B, src.A);
        // int o = MUL_UN8((byte)backdropOpacity, (byte)srcOpacity);

        Func<uint, uint, int, uint> blendFunc = mode switch
        {
            BlendMode.Normal => rgba_blender_normal,
            BlendMode.Multiply => rgba_blender_multiply,
            BlendMode.Screen => rgba_blender_screen,
            BlendMode.Overlay => rgba_blender_overlay,
            BlendMode.Darken => rgba_blender_darken,
            BlendMode.Lighten => rgba_blender_lighten,
            BlendMode.ColorDodge => rgba_blender_color_dodge,
            BlendMode.ColorBurn => rgba_blender_color_burn,
            BlendMode.HardLight => rgba_blender_hard_light,
            BlendMode.SoftLight => rgba_blender_soft_light,
            BlendMode.Difference => rgba_blender_difference,
            BlendMode.Exclusion => rgba_blender_exclusion,
            BlendMode.Hue => rgba_blender_hsl_hue,
            BlendMode.Saturation => rgba_blender_hsl_saturation,
            BlendMode.Color => rgba_blender_hsl_color,
            BlendMode.Luminosity => rgba_blender_hsl_luminosity,
            BlendMode.Addition => rgba_blender_addition,
            BlendMode.Subtract => rgba_blender_subtract,
            BlendMode.Divide => rgba_blender_divide,
            _ => throw new InvalidOperationException($"Unknown blend mode '{mode}'")
        };

        uint blended = blendFunc(b, s, opacity);

        return Color.FromArgb(rgba_geta(blended), rgba_getr(blended), rgba_getg(blended), rgba_getb(blended));
    }
}